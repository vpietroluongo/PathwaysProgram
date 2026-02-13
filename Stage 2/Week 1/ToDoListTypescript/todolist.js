function handleTask() {
    var taskToAdd = document.forms["inputForm"]["taskToAdd"].value;
    var taskToComplete = document.forms["inputForm"]["taskToComplete"].value;
    var taskToRemove = document.forms["inputForm"]["taskToRemove"].value;
    var tableRef = document.getElementById("listOfTasks");
    var taskArray = [];
    var arrayLength = 0;
    for (var i = 0; i < tableRef.rows.length; i++) {
        taskArray.push((tableRef.rows[i]).innerHTML);
    }
    arrayLength = taskArray.length;
    if (taskToRemove == "" && taskToAdd == "" && taskToComplete == "") {
        alert("Please enter a value in at least one of the fields.");
        return;
    }
    if (taskToAdd != "") {
        taskArray.push(taskToAdd);
        // let newRow = (tableRef.insertRow(tableRef.rows.length));
        // newRow.innerHTML = String(tableRef.rows.length) + ". " + taskToAdd ;
        // newRow.id = "tr" + String(tableRef.rows.length);
        (tableRef.insertRow(tableRef.rows.length)).innerHTML = String(tableRef.rows.length) + ". " + taskToAdd;
        taskArray.push(String(tableRef.rows.length) + ". " + taskToAdd);
        document.forms["inputForm"]["taskToAdd"].value = "";
    }
    var taskFound = false;
    var newArray = [];
    if (taskToRemove != "") {
        //let checkNumber = taskToRemove - 1;
        for (var i = 0; i < taskArray.length; i++) {
            if (taskArray[i].startsWith(taskToRemove)) {
                //taskArray.splice(1,i);
                //(tableRef.deleteRow(i));
                tableRef.innerHTML = "";
                taskFound = true;
            }
            else {
                newArray.push(taskArray[i]);
            }
        }
        taskArray = Array.from(newArray);
        if (taskFound) {
            for (var i = 0; i < taskArray.length; i++) {
                var updatedString = (i + 1) + taskArray[i].substring(1);
                (tableRef.insertRow(tableRef.rows.length)).innerHTML = updatedString;
            }
        }
        else {
            alert("Task not found.  Enter corresponding list number.");
        }
        document.forms["inputForm"]["taskToRemove"].value = "";
    }
    taskFound = false;
    if (taskToComplete != "") {
        for (var i = 0; i < taskArray.length; i++) {
            if (taskArray[i].startsWith(taskToComplete)) {
                //tableRef.rows[i].style.textDecoration = "line-through";
                tableRef.rows[i].classList.add("completed");
                taskFound = true;
            }
        }
        if (!taskFound) {
            alert("Task not found.  Enter corresponding list number.");
        }
        document.forms["inputForm"]["taskToComplete"].value = "";
    }
}
function clearEverything() {
    var tableRef = document.getElementById("listOfTasks");
    tableRef.innerHTML = "";
    document.forms["inputForm"]["taskToAdd"].value = "";
    document.forms["inputForm"]["taskToComplete"].value = "";
    document.forms["inputForm"]["TaskToRemove"].value = "";
}
