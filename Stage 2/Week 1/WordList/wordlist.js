function validateANDadd() {
    console.log("inside function");
    var inputWord = document.forms["listForm"]["word"].value;
    var inputNumber = document.forms["listForm"]["listNumber"].value;

    //check if input word is blank and alert the user
    if (inputWord == "") {
        alert ("Please enter a word");
        return false;
    }
    //check if a 1 or a 2 was not entered and alert the user 
    else if ((inputNumber != 1) && (inputNumber != 2)) {
        alert ("Please enter either number 1 or 2");
        document.forms["listForm"]["listNumber"].value = "";  //clear the value for the list number
        return false;
    }
    else {
        if (inputNumber == 1) {
            var tableRef = document.getElementById("list1");
            (tableRef.insertRow(tableRef.rows.length)).innerHTML = inputWord;
        }
        else {
            var tableRef = document.getElementById("list2");
            (tableRef.insertRow(tableRef.rows.length)).innerHTML = inputWord;
        }
        document.forms["listForm"]["word"].value = "";
        document.forms["listForm"]["listNumber"].value = "";
        return true;
    }
}

function clearList1() {
    var tableRef = document.getElementById("list1");
    tableRef.innerHTML = "";
}

function clearList2() {
    var tableRef = document.getElementById("list2");
    tableRef.innerHTML = "";
}