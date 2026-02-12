function validateANDadd() {
    var inputMinString = document.forms["inputForm"]["minNumber"].value;
    var inputMaxString = document.forms["inputForm"]["maxNumber"].value;
    var inputNumberString = document.forms["inputForm"]["inputNumber"].value;
    var inputMin = Number(inputMinString);
    var inputMax = Number(inputMaxString);
    var inputNumber = Number(inputNumberString);
    if (isNaN(inputMin) || isNaN(inputMax) || isNaN(inputNumber)) {
        alert("Please enter a number in all fields.");
    }
    else if (inputMin == "" || inputMax == "" || inputNumber == "") {
        alert("Please enter a number in all fields.");
    }
    else if (inputMin >= inputMax) {
        alert("Minimum number must be less than maximum number.");
    }
    else if (inputNumber > inputMax || inputNumber < inputMin) {
        alert("Number to include must be greater than or equal to the minimum number and less than or equal to the maximum number.");
    }
    else {
        var tableRef = document.getElementById("numberList");
        (tableRef.insertRow(tableRef.rows.length)).innerHTML = String(inputNumber);
        var tableLength = tableRef.rows.length;
        var numberArray = [];
        var mean = 0;
        var median = 0;
        var sum = 0;
        for (var i = 0; i < tableLength; i++) {
            numberArray.push(parseInt((tableRef.rows[i]).innerHTML));
            sum += parseInt((tableRef.rows[i]).innerHTML);
            console.log(tableRef.rows[i]);
        }
        mean = sum / tableLength;
        console.log(mean);
        var meanLabel = document.getElementById("calculatedMean");
        meanLabel.innerHTML = String(mean);
        numberArray.sort(function (a, b) { return a - b; });
        if (tableLength % 2 == 0) {
            var position1 = tableLength / 2;
            var position2 = Math.trunc((tableLength - 1) / 2);
            median = (numberArray[position1] + numberArray[position2]) / 2;
            alert(median);
        }
        else {
            var position = Math.trunc(tableLength / 2);
            median = numberArray[position];
            alert(median);
        }
        var medianLabel = document.getElementById("calculatedMedian");
        medianLabel.innerHTML = String(median);
        document.forms["inputForm"]["inputNumber"].value = "";
    }
}
function clearEverything() {
    var tableRef = document.getElementById("numberList");
    tableRef.innerHTML = "";
    document.forms["inputForm"]["minNumber"].value = "";
    document.forms["inputForm"]["maxNumber"].value = "";
    document.forms["inputForm"]["inputNumber"].value = "";
    var meanLabel = document.getElementById("calculatedMean");
    meanLabel.innerHTML = "";
    var medianLabel = document.getElementById("calculatedMedian");
    medianLabel.innerHTML = "";
    var modeLabel = document.getElementById("calculatedMode");
    modeLabel.innerHTML = "";
}
