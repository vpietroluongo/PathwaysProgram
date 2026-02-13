function handleTask(){
    let taskToAdd : string = document.forms["inputForm"]["taskToAdd"].value;
    let taskToComplete : string = document.forms["inputForm"]["taskToComplete"].value;
    let taskToRemove : string = document.forms["inputForm"]["taskToRemove"].value;
    let tableRef = document.getElementById("listOfTasks");
    let taskArray : string[] = [];
    let arrayLength = 0;

    for (let i = 0; i < tableRef.rows.length; i ++){
        taskArray.push((tableRef.rows[i]).innerHTML);
    }

    arrayLength = taskArray.length;
    if (taskToRemove == "" && taskToAdd == "" && taskToComplete == ""){
        alert ("Please enter a value in at least one of the fields.");
        return;
    }

    if (taskToAdd != ""){
        taskArray.push(taskToAdd);
        // let newRow = (tableRef.insertRow(tableRef.rows.length));
        // newRow.innerHTML = String(tableRef.rows.length) + ". " + taskToAdd ;
        // newRow.id = "tr" + String(tableRef.rows.length);
        (tableRef.insertRow(tableRef.rows.length)).innerHTML = String(tableRef.rows.length) + ". " + taskToAdd;
        taskArray.push(String(tableRef.rows.length) + ". " + taskToAdd);
        document.forms["inputForm"]["taskToAdd"].value = "";
    }

    let taskFound = false;
    let newArray : string[] = [];
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
    let tableRef = document.getElementById("listOfTasks") as HTMLTableElement;
    tableRef.innerHTML = "";

    document.forms["inputForm"]["taskToAdd"].value = "";
    document.forms["inputForm"]["taskToComplete"].value = "";
    document.forms["inputForm"]["TaskToRemove"].value = "";
}