async function getForecast(){
    let apiStringWeather = "https://api.weather.gov/gridpoints/";
    let location = document.getElementById("locationDropDown").value;
    apiStringWeather += `${location}/31,80/forecast`;
    alert(apiStringWeather);

    response = await fetch(apiStringWeather);

    document.getElementById("forecast").innerHTML = ""
    if (response.status >= 200 && response.status <= 299) {
        jsonData = await response.json();
        for (let day in jsonData.properties.periods){
            console.log(jsonData);
            document.getElementById("forecast").innerHTML += `<p>${jsonData.properties.periods[day].name}: ${jsonData.properties.periods[day].temperature}${jsonData.properties.periods[day].temperatureUnit} <img src=\"${jsonData.properties.periods[day].icon}\"></p>`;
            //document.getElementById("weatherIcon").src += jsonData.properties.periods[day].icon; <img src=\"jsonData.properties.periods[day].icon\">
        }
    }
    else {
        document.getElementById("forecast").innerHTML = `Error status: ${response.status}`;
    }
    // switch (location){
    //     case "OAX":

    // }
    //let apiStringGeocode = "https://maps.googleapis.com/maps/api/geocode/json?"
}