async function getForecast(){
    let apiStringWeather = "https://api.weather.gov/gridpoints/";
    let location = document.getElementById("locationDropDown").value;
    apiStringWeather += `${location}/31,80/forecast`;
    alert(apiStringWeather);

    response = await fetch(apiStringWeather);

    document.getElementById("forecastDay").innerHTML = ""
    document.getElementById("forecastTemperature").innerHTML = ""
    document.getElementById("forecastDescription").innerHTML = ""
    if (response.status >= 200 && response.status <= 299) {
        jsonData = await response.json();
        let forecastInfoArray = [];

        for (let day in jsonData.properties.periods){
            console.log(jsonData);
            const forecastInfo= {
                                day: jsonData.properties.periods[day].name, 
                                temperatureString: jsonData.properties.periods[day].temperature + " " + jsonData.properties.periods[day].temperatureUnit,
                                shortForecast: jsonData.properties.periods[day].shortForecast,
                                weatherIconURL: jsonData.properties.periods[day].icon 
                                };
            document.getElementById("forecastDay").innerHTML += `<p>${forecastInfo.day}</p>`;
            document.getElementById("forecastTemperature").innerHTML += `<p>${forecastInfo.temperatureString}</p>`;
            document.getElementById("forecastDescription").innerHTML += `<p>${forecastInfo.shortForecast}</p>`;
            
            forecastInfoArray.push(forecastInfo);
        }

        populateCarousel(forecastInfoArray);
    }
    else {
        document.getElementById("forecast").innerHTML = `Error status: ${response.status}`;
    }
}

function populateCarousel(forecastInfoArray){

    const carouselInner = document.querySelector('#myCarousel .carousel-inner');
    const carouselIndicators = document.querySelector('#myCarousel .carousel-indicators');
    
    carouselInner.innerHTML = '';
    carouselIndicators.innerHTML = '';
    
    for (let i = 0; i <forecastInfoArray.length; i++){
    
        // 1. Create the carousel item (slide)
        const carouselItem = document.createElement('div');
        carouselItem.classList.add('carousel-item');
        
        // The first item must have the 'active' class
        if (i === 0) {
            carouselItem.classList.add('active');
        }
        // 2. Create the svg 
        const svgNS = "http://www.w3.org/2000/svg";
        const svg = document.createElementNS(svgNS, "svg");
        svg.classList.add("bd-placeholder-img");
        svg.setAttribute("width","100%");
        svg.setAttribute("height","100%");
        svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
        svg.setAttribute("aria-hidden", "true");
        svg.setAttribute("preserveAspectRatio", "xMidYMid slice");
        svg.setAttribute("focusable","false");

        // 3.  Create the rect (the grey background rectangle)  
        const rect = document.createElementNS(svgNS, "rect");
        rect.classList.add("myCarousel-rect");
        rect.setAttribute("width", "100%");
        rect.setAttribute("height", "100%");
        rect.setAttribute("fill", "#777");

        // 4. Add the rectangle element inside the svg
        svg.appendChild(rect);
 
        // 5.  Add the svg element inside the carousel item
        carouselItem.appendChild(svg);
        
        // 6. Create the carousel container
        const carouselContainer = document.createElement("div");
        carouselContainer.classList.add("container");
    
        // 7. Create the carousel caption
        const carouselCaption = document.createElement('div');
        carouselCaption.classList.add("carousel-caption");
    
        // 8. Create the image element for the weather icon
        const img = document.createElement("img");
        img.src = forecastInfoArray[i].weatherIconURL;
         
        img.classList.add("myCarousel-image"); 
        img.alt = `Slide ${i + 1}`;
        
        // 9. Add the forecast info inside the carousel caption and then the image element
        carouselCaption.innerHTML = `<h1>${forecastInfoArray[i].day}</h1>
                                     <p>${forecastInfoArray[i].temperatureString}</p>
                                     <p>${forecastInfoArray[i].shortForecast}</p>`;
        carouselCaption.appendChild(img);
    
        // 10.  Add the carousel caption to the carousel container
        carouselContainer.appendChild(carouselCaption);

        // 11.  Append the carousel container to the carousel item  
        carouselItem.appendChild(carouselContainer);

        // 12. Append the item to the carousel inner
        carouselInner.appendChild(carouselItem);
    
        // 13. Create the indicator button
        const indicatorButton = document.createElement('button');
        indicatorButton.type = 'button';
        indicatorButton.setAttribute('data-bs-target', '#myCarousel');
        indicatorButton.setAttribute('data-bs-slide-to', i);
        indicatorButton.setAttribute('aria-label', `Slide ${i + 1}`);
        // The first indicator must also be 'active'
        if (i === 0) {
        indicatorButton.classList.add('active');
        indicatorButton.setAttribute('aria-current', 'true');
        }
    
        // 14. Append the indicator to the carousel indicators container
        carouselIndicators.appendChild(indicatorButton);
        
    }
}