const uri = 'api/mybackpackitems';
let gearitems = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addGearTextbox = document.getElementById('add-gear');
    const addCategoryDropdown = document.getElementById('add-category-dropdown');
    const addWeightTextbox = document.getElementById('add-weight');
    const addWeightUnitDropdown = document.getElementById('add-weight-unit-dropdown');
    const addIsConsumableDropdown = document.getElementById('add-isConsumable-dropdown');
    const addIsWornDropdown = document.getElementById('add-isWorn-dropdown');
    //const addIsConsumableRadio = document.getElementById('add-isConsumable-radio');
    //const addIsWornRadio = document.getElementById('add-isWorn-radio');


    const item = {
        gearName: addGearTextbox.value.trim(),
        category: addCategoryDropdown.value.trim(),
        weight: parseFloat(addWeightTextbox.value.trim()),
       // weightUnit: addWeightUnit.value.trim(),
        weightUnit: addWeightUnitDropdown.value.trim(),
        isConsumable: addIsConsumableDropdown.value.trim(),
        isWorn: addIsWornDropdown.value.trim()
        //isConsumable: addIsConsumableDropdown.value === 'true',
        //isWorn: addIsWornDropdown.value === 'true'
        //isConsumable: addIsConsumableRadio.value,
        //isWorn: addIsWornRadio.value
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
            getTotalWeight();
            document.getElementById('myBackpackForm').reset();
            //addGearTextbox.value = '';
            //addCategoryDropdown.value = 'Select category';
            //addWeightTextbox.value = '';
            //addWeightUnitDropdown.value = 'Select unit of weight';
            //addIsConsumableRadio.checked = true;
            //addIsWornRadio.checked = false;
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => {
            getItems();
            getTotalWeight();
        })
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = gearitems.find(item => item.id === id);

    document.getElementById('edit-gear').value = item.gearName;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-category-dropdown').value = item.category;
    document.getElementById('edit-weight').value = item.weight;
    document.getElementById('edit-weight-unit-dropdown').value = item.weightUnit;
    document.getElementById('edit-isConsumable-dropdown').value = item.isConsumable;
    document.getElementById('edit-isWorn-dropdown').value = item.isWorn;
    //document.getElementById('edit-isConsumable').checked = item.isConsumable;
    //document.getElementById('edit-isWorn').checked = item.isWorn;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        //isConsumable: document.getElementById('edit-isConsumable').checked,
        gearName: document.getElementById('edit-gear').value.trim(),
        category: document.getElementById('edit-category-dropdown').value,
        weight: document.getElementById('edit-weight').value.trim(),
        weightUnit: document.getElementById('edit-weight-unit-dropdown').value,
        isConsumable: document.getElementById('edit-isConsumable-dropdown').value,
        isWorn: document.getElementById('edit-isWorn-dropdown').value
        // isConsumable: document.getElementById('edit-isConsumable-dropdown').value === 'true',
        // isWorn: document.getElementById('edit-isWorn-dropdown').value === 'true'
        //isWorn: document.getElementById('edit-isWorn').checked 
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            getItems();
            getTotalWeight();
        })
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'item' : 'items';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayTotalWeight(weightInPounds) {
    const weightTotalParagraph = document.getElementById("total-weight");

    weightTotalParagraph.innerHTML = `Total weight: ${weightInPounds} lb`; 
}
function _displayItems(data) {
    const tBody = document.getElementById('gearitems');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        //let isConsumableCheckbox = document.createElement('input');
        //isConsumableCheckbox.type = 'checkbox';
        //isConsumableCheckbox.disabled = true;
        //isConsumableCheckbox.checked = item.isConsumable;

        //let isWornCheckbox = document.createElement('input');
        //isWornCheckbox.type = 'checkbox';
        //isWornCheckbox.disabled = true;
        //isWornCheckbox.checked = item.isWorn;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(item.gearName);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.category);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(item.weight);
        td3.classList.add("no-right-border");
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        let textNode4 = document.createTextNode(item.weightUnit);
        td4.classList.add("no-left-border");
        td4.appendChild(textNode4);

        let td5 = tr.insertCell(4);
        let textNode5 = document.createTextNode(item.isConsumable);
        td5.appendChild(textNode5);
        //td5.appendChild(isConsumableCheckbox);

        let td6 = tr.insertCell(5);
        let textNode6 = document.createTextNode(item.isWorn);
        td6.appendChild(textNode6);
        //td6.appendChild(isWornCheckbox);

        let td7 = tr.insertCell(6);
        td7.appendChild(editButton);

        let td8 = tr.insertCell(7);
        td8.appendChild(deleteButton);
    });

    gearitems = data;
}

function getTotalWeight() {
    fetch(`${uri}/totalWeight`)
        .then(response => response.json())
        .then(data => _displayTotalWeight(data))
        .catch(error => console.error("Unable to get total weight.", error));
}