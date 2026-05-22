(() => {
  const root = document.documentElement;
  const toggleButton = document.getElementById("theme-toggle");
  const savedTheme = localStorage.getItem("theme") || "light";

  root.setAttribute("data-theme", savedTheme);
  if (toggleButton) {
    toggleButton.textContent = savedTheme === "dark" ? "Light mode" : "Dark mode";
    toggleButton.addEventListener("click", () => {
      const current = root.getAttribute("data-theme") || "light";
      const next = current === "dark" ? "light" : "dark";
      root.setAttribute("data-theme", next);
      localStorage.setItem("theme", next);
      toggleButton.textContent = next === "dark" ? "Light mode" : "Dark mode";
    });
  }

  if (!window.CHAT_CONTEXT) {
    return;
  }

  const socket = io();
  const context = window.CHAT_CONTEXT;
  const roomId = context.id;
  const roomChannel = `room:${roomId}`;

  const messageList = document.getElementById("message-list");
  const form = document.getElementById("message-form");
  const input = document.getElementById("message-input");
  const typingIndicator = document.getElementById("typing-indicator");

  const appendMessage = (message) => {
    const row = document.createElement("div");
    row.className = "message-row";

    const bubble = document.createElement("div");
    bubble.className = "message-bubble";
    bubble.innerHTML = `<div class="small fw-bold">${message.sender.username}</div>
      <div>${message.content}</div>
      <div class="small text-muted">${new Date(message.created_at).toLocaleString()}</div>`;

    row.appendChild(bubble);
    messageList.appendChild(row);
    messageList.scrollTop = messageList.scrollHeight;
  };

  socket.emit("join_room", { room_id: roomId });

  socket.on("joined_room", (payload) => {
    if (payload.room_id !== roomId) {
      return;
    }
    socket.emit("typing", { room_id: roomId, is_typing: false });
  });

  socket.on("new_message", (payload) => {
    appendMessage(payload);
  });

  socket.on("typing", (payload) => {
    if (payload.room_id !== roomId) {
      return;
    }
    typingIndicator.textContent = payload.is_typing ? `${payload.user.username} is typing...` : "";
  });

  input.addEventListener("input", () => {
    socket.emit("typing", { room_id: roomId, is_typing: input.value.length > 0 });
  });

  form.addEventListener("submit", (event) => {
    event.preventDefault();
    const content = input.value.trim();
    if (!content) {
      return;
    }

    socket.emit("send_message", { room_id: roomId, content });
    input.value = "";
    socket.emit("typing", { room_id: roomId, is_typing: false });
  });
})();
