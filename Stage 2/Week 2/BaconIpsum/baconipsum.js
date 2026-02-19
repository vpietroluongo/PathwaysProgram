async function getBaconIpsum(){
    let apiString = "https://baconipsum.com/api/";
    let baconIpsumType = document.getElementById("typeDropDown").value;
    let numberOfParagraphs = document.getElementById("paragraphDropDown").value;
    apiString += `?type=${baconIpsumType}&paras=${numberOfParagraphs}`;
    alert(apiString);

    let response = await fetch(apiString);

    document.getElementById("jsonRawData").innerHTML = "";
    document.getElementById("jsonFormattedData").innerHTML = "";

    let jsonData = await response.json();

    document.getElementById("jsonRawData").innerHTML = JSON.stringify(jsonData);

    for (let paragraph in jsonData) {
        document.getElementById("jsonFormattedData").innerHTML += `<p>${jsonData[paragraph]}</p>`;
    }
}