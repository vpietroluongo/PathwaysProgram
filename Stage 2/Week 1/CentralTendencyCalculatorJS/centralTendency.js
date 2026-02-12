function validateANDadd(){
    let inputMin = document.forms["inputForm"]["minNumber"].value;
    let inputMax = document.forms["inputForm"]["maxNumber"].value;
    let inputNumber = document.forms["inputForm"]["inputNumber"].value;
    
    inputMin = Number(inputMin);
    inputMax = Number(inputMax);
    inputNumber = Number(inputNumber);

    if (isNaN(inputMin) || isNaN(inputMax) || isNaN(inputNumber)){
        alert ("All fields must be a number");
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
        let tableRef = document.getElementById("numberList");
        (tableRef.insertRow(tableRef.rows.length)).innerHTML = inputNumber;
        let numberArray = [];
        let tableLength = tableRef.rows.length;
        let mean = 0;
        let median = 0;
        let sum = 0;
        for (let i = 0; i < tableLength; i++){
            numberArray.push(parseInt((tableRef.rows[i]).innerHTML));
            sum += parseInt((tableRef.rows[i]).innerHTML);
        }
        mean = sum / tableLength;
        console.log(mean);

        let meanLabel = document.getElementById("calculatedMean");
        meanLabel.innerHTML = mean; 

        //numberArray.sort((a,b) => a - b);
        numberArray.sort(function(a, b){return a - b});
        if (tableLength % 2 == 0){
            let position1 = tableLength / 2;
            let position2 = Math.trunc((tableLength - 1) /2);
            median = (numberArray[position1] + numberArray[position2]) / 2;
        }
        else {
            let position = Math.trunc(tableLength / 2);
            median = numberArray[position];
        }
        let medianLabel = document.getElementById("calculatedMedian");
        medianLabel.innerHTML = median;


        document.forms["inputForm"]["inputNumber"].value = "";
    }
}

function clearEverything(){
    let tableRef = document.getElementById("numberList");
    tableRef.innerHTML = "";

    document.forms["inputForm"]["minNumber"].value = "";
    document.forms["inputForm"]["maxNumber"].value = "";
    document.forms["inputForm"]["inputNumber"].value = "";

    let meanLabel = document.getElementById("calculatedMean");
    meanLabel.innerHTML = "";

    let medianLabel = document.getElementById("calculatedMedian");
    medianLabel.innerHTML = "";
    
    let modeLabel = document.getElementById("calculatedMode");
    modeLabel.innerHTML = "";
}