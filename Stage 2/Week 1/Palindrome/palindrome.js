function validateANDadd() {
    console.log("inside function");
    var inputWord = document.forms["listForm"]["word"].value;
   // let element = document.getElementById("palindromeList1");
   // element.hidden = true;
    //var inputNumber = document.forms["listForm"]["listNumber"].value;
    var casePreference = document.forms["listForm"]["casePreference"].value;

    //check if input word is blank and alert the user
    if (inputWord == "") {
        alert ("Please enter a word");
        return false;
    }
    //check if a 1 or a 2 were not entered and alert the user
    else if ((casePreference.toUpperCase() != "Y") && (casePreference.toUpperCase() != "YES") &&
             (casePreference.toUpperCase() != "N") && (casePreference.toUpperCase() != "NO")) {
        alert ("Please enter yes or no");
        //document.forms["listForm"]["listNumber"].value = "";  //clear the value for the list number
        document.forms["listForm"]["casePreference"].value = " ";  //clear the value for the list number
        return false;
    }
    else {
        
        //if (inputNumber == 1) {
        if (casePreference.toUpperCase() == "N" ||  casePreference.toUpperCase() == "NO") {
            let i = 0;
            let j = inputWord.length -1;
            let isItPalindrome = false;
            while (i < inputWord.length && i < j){
                if (inputWord[i] == inputWord[j]) {
                    isItPalindrome = true;
                }
                else{
                    isItPalindrome = false;
                }
                i++;
                j--;
            }
            if (isItPalindrome == true){
                var tableRef = document.getElementById("palindromeList1");
                (tableRef.insertRow(tableRef.rows.length)).innerHTML = inputWord;
            }
            else {
                alert (`${inputWord} is not a palindrome`);
            }
        }
        else {
            let reversedString = inputWord.split('').reverse().join('');

            if (inputWord == reversedString){
                var tableRef = document.getElementById("palindromeList2");
                (tableRef.insertRow(tableRef.rows.length)).innerHTML = inputWord;
            }
            else {
                alert (`${inputWord} is not a palindrome`);
            }
        }


        document.forms["listForm"]["word"].value = "";
        //document.forms["listForm"]["listNumber"].value = "";
        document.forms["listForm"]["casePreference"].value = "";
        return true;
    }
}

function clearList1() {
    var tableRef = document.getElementById("palindromeList1");
    tableRef.innerHTML = "";
}

function clearList2() {
    var tableRef = document.getElementById("palindromeList2");
    tableRef.innerHTML = "";
}