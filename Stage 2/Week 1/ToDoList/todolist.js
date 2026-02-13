let globalTaskArray = [];


function handleTask(){
    let taskToAdd = document.forms["inputForm"]["taskToAdd"].value;
    let taskToComplete = document.forms["inputForm"]["taskToComplete"].value;
    let taskToRemove = document.forms["inputForm"]["taskToRemove"].value;
    let tableRef = document.getElementById("listOfTasks");
    //let globalTaskArray = [];
    let arrayLength = 0;

    // for (let i = 0; i < tableRef.rows.length; i ++){
    //     globalTaskArray.push((tableRef.rows[i]).innerHTML);
    // }

    arrayLength = globalTaskArray.length;
    if (taskToRemove == "" && taskToAdd == "" && taskToComplete == ""){
        alert ("Please enter a value in at least one of the fields.");
        return;
    }

    let taskFound = false;
    let newArray = [];
    if (taskToComplete != ""){
        for (let i = 0; i < globalTaskArray.length; i++){
            //if (globalTaskArray[i].text.startsWith(taskToComplete)){
            if (globalTaskArray[i].numberOrder == taskToComplete){
                //tableRef.rows[i].style.textDecoration = "line-through";
                tableRef.rows[i].classList.add("completed");
                globalTaskArray[i].isCompleted = true;
                taskFound = true;
            } 
        }
        if (!taskFound){
            alert ("Task not found.  Enter corresponding list number.");
        }
        document.forms["inputForm"]["taskToComplete"].value = "";
    }

    taskFound = false;  //reset taskFound to false
    if (taskToRemove != ""){
        //let checkNumber = taskToRemove - 1;
        for (let i = 0; i < globalTaskArray.length; i++){
            //if (globalTaskArray[i].text.startsWith(taskToRemove)){
            if (globalTaskArray[i].numberOrder == taskToRemove){
                //taskArray.splice(1,i);
                //(tableRef.deleteRow(i));
                tableRef.innerHTML = "";
                taskFound = true;
            }
            else{
                newArray.push(globalTaskArray[i]);
            }
        }
        globalTaskArray = Array.from(newArray);
        if (taskFound){
            for (let i = 0; i < globalTaskArray.length; i++){
                let newNumberOrder = i +1;
                //let updatedString = (i+1) + globalTaskArray[i].text.substring(1);
                globalTaskArray[i].numberOrder = newNumberOrder;
                (tableRef.insertRow(tableRef.rows.length)).innerHTML = globalTaskArray[i].numberOrder + globalTaskArray[i].text;
                //globalTaskArray[i].text = updatedString;
                if (globalTaskArray[i].isCompleted == true){
                    tableRef.rows[i].classList.add("completed");
                }
            }
        }
        else {
            alert ("Task not found.  Enter corresponding list number.");
        }
        document.forms["inputForm"]["taskToRemove"].value = "";
    }

    
    if (taskToAdd != ""){
        //taskArray.push(taskToAdd);
        // let newRow = (tableRef.insertRow(tableRef.rows.length));
        // newRow.innerHTML = String(tableRef.rows.length) + ". " + taskToAdd ;
        // newRow.id = "tr" + String(tableRef.rows.length);
        (tableRef.insertRow(tableRef.rows.length)).innerHTML = String(tableRef.rows.length) + ". " + taskToAdd ;
        //const task = {numberOrder: tableRef.rows.length, text : `${numberOrder}. ${taskToAdd}`, isCompleted : false};
        const task = {numberOrder: tableRef.rows.length, text : `. ${taskToAdd}`, isCompleted : false};
        globalTaskArray.push(task);
        document.forms["inputForm"]["taskToAdd"].value = "";
    }
}

function clearEverything(){
    let tableRef = document.getElementById("listOfTasks");
    tableRef.innerHTML = "";

    document.forms["inputForm"]["taskToAdd"].value = "";
    document.forms["inputForm"]["taskToComplete"].value = "";
    document.forms["inputForm"]["TaskToRemove"].value = "";
    globalTaskArray = [];
}

// const task = {text : "test", isCompleted : false};
// task.isCompleted = true;
// task["text"] = "newtext";