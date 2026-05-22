(function () {
  const cfg = window.ChatConfig;
  if (!cfg) {
    return;
  }

  const socket = io();
  const messageForm = document.getElementById("messageForm");
  const messageInput = document.getElementById("messageInput");
  const messageList = document.getElementById("messageList");
  const typingIndicator = document.getElementById("typingIndicator");
  const typingDots = document.getElementById("typingDots");
  const statusDots = document.querySelectorAll(".status-dot[data-user-id]");
  const isRoomMode = cfg.chatMode === "room";
  const typingUsers = new Map();

  let stopTypingTimeout = null;
  let typingStarted = false;

  function scrollToBottom() {
    messageList.scrollTop = messageList.scrollHeight;
  }

  function renderMessage(message) {
    const isMine = Number(message.sender_id) === Number(cfg.userId);
    const row = document.createElement("div");
    row.className = `message-row ${isMine ? "mine" : ""}`.trim();

    const bubble = document.createElement("div");
    bubble.className = "message-bubble";

    const time = message.timestamp ? new Date(message.timestamp) : new Date();
    const hh = String(time.getHours()).padStart(2, "0");
    const mm = String(time.getMinutes()).padStart(2, "0");

    const meta = document.createElement("div");
    meta.className = "small text-secondary mb-1";
    const senderName = isMine ? cfg.username : (message.sender_username || "User #" + message.sender_id);
    meta.textContent = `${senderName} • ${hh}:${mm}`;

    const body = document.createElement("div");
    body.textContent = message.content;

    bubble.appendChild(meta);
    bubble.appendChild(body);
    row.appendChild(bubble);
    messageList.appendChild(row);
  }

  function applyOnlineUsers(userIds) {
    const onlineSet = new Set(userIds.map((id) => Number(id)));
    statusDots.forEach((dot) => {
      const userId = Number(dot.getAttribute("data-user-id"));
      dot.classList.remove("status-online", "status-offline");
      dot.classList.add(onlineSet.has(userId) ? "status-online" : "status-offline");
    });
  }

  function setTypingIndicatorText() {
    const names = Array.from(typingUsers.values());
    if (names.length === 0) {
      typingIndicator.textContent = "";
      if (typingDots) {
        typingDots.classList.remove("active");
      }
      return;
    }

    if (typingDots) {
      typingDots.classList.add("active");
    }

    if (names.length === 1) {
      typingIndicator.textContent = `${names[0]} is typing...`;
      return;
    }

    if (names.length === 2) {
      typingIndicator.textContent = `${names[0]} and ${names[1]} are typing...`;
      return;
    }

    typingIndicator.textContent = `${names.length} people are typing...`;
  }

  function emitTyping(isTyping) {
    const payload = {
      user_id: cfg.userId,
      is_typing: isTyping,
    };

    if (isRoomMode) {
      payload.room_id = cfg.roomId;
    } else {
      payload.receiver_id = cfg.receiverId;
    }

    socket.emit("typing", payload);
  }

  function scheduleStopTyping() {
    if (stopTypingTimeout) {
      clearTimeout(stopTypingTimeout);
    }

    stopTypingTimeout = setTimeout(function () {
      if (typingStarted) {
        emitTyping(false);
        typingStarted = false;
      }
    }, 1000);
  }

  socket.on("connect", function () {
    const joinPayload = {
      user_id: cfg.userId,
      username: cfg.username,
    };

    if (isRoomMode && cfg.roomId !== null) {
      joinPayload.room_id = cfg.roomId;
    }

    socket.emit("join_room", joinPayload);
  });

  socket.on("new_message", function (message) {
    if (isRoomMode) {
      if (Number(message.room_id) !== Number(cfg.roomId)) {
        return;
      }
    } else {
      const sender = Number(message.sender_id);
      const receiver = Number(message.receiver_id);
      const me = Number(cfg.userId);
      const peer = Number(cfg.receiverId);
      const isDirectPair = (sender === me && receiver === peer) || (sender === peer && receiver === me);
      if (!isDirectPair) {
        return;
      }
    }

    renderMessage(message);
    scrollToBottom();
  });

  socket.on("typing", function (payload) {
    if (Number(payload.user_id) === Number(cfg.userId)) {
      return;
    }

    const userId = Number(payload.user_id);
    const username = payload.username || `User #${userId}`;
    if (payload.is_typing) {
      typingUsers.set(userId, username);
    } else {
      typingUsers.delete(userId);
    }
    setTypingIndicatorText();
  });

  socket.on("online_users", function (payload) {
    applyOnlineUsers(payload.user_ids || []);
  });

  socket.on("error", function (payload) {
    if (payload && payload.message) {
      console.error(payload.message);
    }
  });

  messageForm.addEventListener("submit", function (event) {
    event.preventDefault();
    const content = messageInput.value.trim();
    if (!content) {
      return;
    }

    const payload = {
      sender_id: cfg.userId,
      content: content,
    };

    if (isRoomMode) {
      payload.room_id = cfg.roomId;
    } else {
      payload.receiver_id = cfg.receiverId;
    }

    socket.emit("send_message", payload);

    if (typingStarted) {
      emitTyping(false);
      typingStarted = false;
    }
    if (stopTypingTimeout) {
      clearTimeout(stopTypingTimeout);
      stopTypingTimeout = null;
    }

    messageInput.value = "";
    messageInput.focus();
  });

  messageInput.addEventListener("input", function () {
    const hasText = messageInput.value.trim().length > 0;

    if (hasText) {
      if (!typingStarted) {
        emitTyping(true);
        typingStarted = true;
      }
      scheduleStopTyping();
      return;
    }

    if (typingStarted) {
      emitTyping(false);
      typingStarted = false;
    }
    if (stopTypingTimeout) {
      clearTimeout(stopTypingTimeout);
      stopTypingTimeout = null;
    }
  });

  messageInput.addEventListener("blur", function () {
    if (!typingStarted) {
      return;
    }

    emitTyping(false);
    typingStarted = false;
    if (stopTypingTimeout) {
      clearTimeout(stopTypingTimeout);
      stopTypingTimeout = null;
    }
  });

  scrollToBottom();
})();
