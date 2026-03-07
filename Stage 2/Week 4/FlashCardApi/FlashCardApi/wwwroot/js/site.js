const uri = 'api/FlashCardItems';
let flashcards = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addQuestionTextbox = document.getElementById('add-question');
    const addAnswerTextbox = document.getElementById('add-answer');

    if (addQuestionTextbox.value == "" || addAnswerTextbox.value == "") {
        alert("Please provide both a question and answer");
    }
    else {
        const item = {
            isCorrect: false,
            question: addQuestionTextbox.value.trim(),
            answer: addAnswerTextbox.value.trim() 
        };

        fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(response => response.json())
            .then(() => {
                getItems();
                addQuestionTextbox.value = '';
                addAnswerTextbox.value = '';
            })
            .catch(error => console.error('Unable to add item.', error));
    }
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = flashcards.find(item => item.id === id);

    document.getElementById('edit-question').value = item.question;
    document.getElementById('edit-answer').value = item.answer;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isCorrect;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isCorrect: document.getElementById('edit-isComplete').checked,
        question: document.getElementById('edit-question').value.trim(),
        answer: document.getElementById('edit-answer').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function submitItem(id) {
    //const itemId = document.getElementById('edit-id').value;
    //const item = {
    //    id: parseInt(itemId, 10),
    //    isCorrect: document.getElementById('edit-isComplete').checked,
    //    question: document.getElementById('edit-question').value.trim(),
    //    answer: document.getElementById('edit-answer').value.trim()
    //};
    console.log(id);
    const inputAnswer = document.getElementById(`answer-box-${id}`).value.trim();
    let newIsCorrect = false;

    //fetch(`${uri}/${itemId}`, {
    fetch(`${uri}/${id}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())
        .then(response => {
            if (inputAnswer.toLowerCase() == (response.answer).toLowerCase()) {
                //if (response.answer == '4') {
                newIsCorrect = true;
                console.log(uri + " " + id);
                console.log("input: " + inputAnswer);
                console.log("response: " + response.answer);
                console.log(newIsCorrect);
            }
            else {
                newIsCorrect = false;
                alert("Answer is incorrect.");
            }
            const updateItem = {
                id: id,
                isCorrect: newIsCorrect,
                question: response.question,
                answer: response.answer
            }
            fetch(`${uri}/${id}`, {
                method: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updateItem)
            })
                .then(() => getItems())
                .catch(error => console.error('Unable to update item.', error))
        })
        .catch(error => console.error('Unable to check answer.', error));
        //.then(() => updateItem())

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'Flash Card' : 'Flash Cards';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('flashcards');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.disabled = false;
        isCompleteCheckbox.checked = item.isCorrect;

        let answerBox = document.createElement('input');
        answerBox.type = 'text';
        answerBox.id = `answer-box-${item.id}`;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let submitButton = button.cloneNode(false);
        submitButton.innerText = 'Submit';
        submitButton.setAttribute('onclick', `submitItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let textNode1 = document.createTextNode(item.question);
        td2.appendChild(textNode1);

        //let td3 = tr.insertCell(2);
        //let textNode2 = document.createTextNode(item.answer);
        //td3.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        td3.appendChild(answerBox);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);

        let td6 = tr.insertCell(5);
        td6.appendChild(submitButton);
    });

    flashcards = data;
}