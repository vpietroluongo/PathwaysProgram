function validateANDadd(){
    let inputMinString : string = document.forms["inputForm"]["minNumber"].value;
    let inputMaxString : string = document.forms["inputForm"]["maxNumber"].value;
    let inputNumberString : string = document.forms["inputForm"]["inputNumber"].value;
    
 
    let inputMin = Number(inputMinString);
    let inputMax = Number(inputMaxString);
    let inputNumber = Number(inputNumberString);

    if (isNaN(inputMin) || isNaN(inputMax) || isNaN(inputNumber)){
        alert ("Please enter a number in all fields.");
    }
    else if (inputMin == "" || inputMax == "" || inputNumber == ""){
        alert ("Please enter a number in all fields.");
    }
    else if (inputMin >= inputMax){
        alert ("Minimum number must be less than maximum number.");
    }
    else if (inputNumber > inputMax || inputNumber < inputMin){
        alert ("Number to include must be greater than or equal to the minimum number and less than or equal to the maximum number.");
    }
    else {
        let tableRef = document.getElementById("numberList") as HTMLTableElement;
        (tableRef.insertRow(tableRef.rows.length)).innerHTML = String(inputNumber);
        let tableLength = tableRef.rows.length;
        let numberArray : number[] = [];
        let mean = 0;
        let median = 0;
        let sum = 0;
        for (let i = 0; i < tableLength; i++){
            numberArray.push(parseInt((tableRef.rows[i]).innerHTML));
            sum += parseInt((tableRef.rows[i]).innerHTML);
            console.log(tableRef.rows[i]);
        }
        mean = sum / tableLength;
        console.log(mean);

        let meanLabel = document.getElementById("calculatedMean") as HTMLLabelElement;
        meanLabel.innerHTML = String(mean); 

        numberArray.sort(function(a, b){return a - b});
        if (tableLength % 2 == 0){
            let position1 : number = tableLength / 2;
            let position2 :number = Math.trunc((tableLength - 1) /2);
            median = (numberArray[position1] + numberArray[position2]) / 2;
            alert (median);
        }
        else {
            let position : number = Math.trunc(tableLength / 2);
            median = numberArray[position];
            alert (median);
        }
        let medianLabel = document.getElementById("calculatedMedian") as HTMLLabelElement;
        medianLabel.innerHTML = String(median);

        document.forms["inputForm"]["inputNumber"].value = "";
    }
}

function clearEverything(){
    let tableRef = document.getElementById("numberList") as HTMLTableElement;
    tableRef.innerHTML = "";

    document.forms["inputForm"]["minNumber"].value = "";
    document.forms["inputForm"]["maxNumber"].value = "";
    document.forms["inputForm"]["inputNumber"].value = "";

    let meanLabel = document.getElementById("calculatedMean") as HTMLLabelElement;
    meanLabel.innerHTML = "";

    let medianLabel = document.getElementById("calculatedMedian") as HTMLLabelElement;
    medianLabel.innerHTML = "";

    let modeLabel = document.getElementById("calculatedMode") as HTMLLabelElement;
    modeLabel.innerHTML = "";
}