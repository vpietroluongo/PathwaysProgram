function handleTask(){
    let taskToAdd = document.forms["inputForm"]["taskToAdd"].value;
    let taskToComplete = document.forms["inputForm"]["taskToComplete"].value;
    let taskToRemove = document.forms["inputForm"]["taskToRemove"].value;
    let tableRef = document.getElementById("listOfTasks");
    let taskArray = [];
    let arrayLength = 0;

    for (let i = 0; i < tableRef.rows.length; i ++){
        taskArray.push((tableRef.rows[i]).innerHTML);
    }

    arrayLength = taskArray.length;

    if (taskToAdd != ""){
        taskArray.push(taskToAdd);
        // let newRow = (tableRef.insertRow(tableRef.rows.length));
        // newRow.innerHTML = String(tableRef.rows.length) + ". " + taskToAdd ;
        // newRow.id = "tr" + String(tableRef.rows.length);
        (tableRef.insertRow(tableRef.rows.length)).innerHTML = String(tableRef.rows.length) + ". " + taskToAdd ;
        document.forms["inputForm"]["taskToAdd"].value = "";
    }

    let taskFound = false;
    let newArray = [];
    if (taskToRemove != ""){
        //let checkNumber = taskToRemove - 1;
        for (let i = 0; i < taskArray.length; i++){
            if (taskArray[i].startsWith(taskToRemove)){
                //taskArray.splice(1,i);
                //(tableRef.deleteRow(i));
                tableRef.innerHTML = "";
                taskFound = true;
            }
            else{
                newArray.push(taskArray[i]);
            }
        }
        taskArray = Array.from(newArray);
        if (taskFound){
            for (let i = 0; i < taskArray.length; i++){
                let updatedString = (i+1) + taskArray[i].substring(1);
                (tableRef.insertRow(tableRef.rows.length)).innerHTML = updatedString;
            }
        }
        else {
            alert ("Task not found.  Enter corresponding list number.");
        }
        document.forms["inputForm"]["taskToRemove"].value = "";
    }

    taskFound = false;
    if (taskToComplete != ""){
        for (let i = 0; i < taskArray.length; i++){
            if (taskArray[i].startsWith(taskToComplete)){
                //tableRef.rows[i].style.textDecoration = "line-through";
                tableRef.rows[i].classList.add("completed");
                taskFound = true;
            } 
        }
        if (!taskFound){
            alert ("Task not found.  Enter corresponding list number.");
        }
        document.forms["inputForm"]["taskToComplete"].value = "";
    }
}

function clearEverything(){
    let tableRef = document.getElementById("listOfTasks");
    tableRef.innerHTML = "";

    document.forms["inputForm"]["taskToAdd"].value = "";
    document.forms["inputForm"]["taskToComplete"].value = "";
    document.forms["inputForm"]["TaskToRemove"].value = "";
}