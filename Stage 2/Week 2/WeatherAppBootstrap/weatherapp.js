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
            //document.getElementById("forecast").innerHTML += `<p>${jsonData.properties.periods[day].name}: ${jsonData.properties.periods[day].temperature}${jsonData.properties.periods[day].temperatureUnit} <img src=\"${jsonData.properties.periods[day].icon}\"></p>`;
            const forecastInfo= {
                                day: jsonData.properties.periods[day].name, 
                                temperatureString: jsonData.properties.periods[day].temperature + jsonData.properties.periods[day].temperatureUnit,
                                shortForecast: jsonData.properties.periods[day].shortForecast,
                                weatherIconURL: jsonData.properties.periods[day].icon 
                                };
            document.getElementById("forecastDay").innerHTML += `<p>${forecastInfo.day}</p>`;
            document.getElementById("forecastTemperature").innerHTML += `<p>${forecastInfo.temperatureString}</p>`;
            document.getElementById("forecastDescription").innerHTML += `<p>${forecastInfo.shortForecast}</p>`;
            // document.getElementById("forecastDay").innerHTML += `<p>${jsonData.properties.periods[day].name}</p>`;
            // document.getElementById("forecastTemperature").innerHTML += `<p>${jsonData.properties.periods[day].temperature} ${jsonData.properties.periods[day].temperatureUnit}</p>`;
            // document.getElementById("forecastDescription").innerHTML += `<p>${jsonData.properties.periods[day].shortForecast}</p>`;
            //document.getElementById("weatherIcon").src += jsonData.properties.periods[day].icon; <img src=\"jsonData.properties.periods[day].icon\">
            forecastInfoArray.push(forecastInfo);
        }
        //populateCarousel(forecastInfoArray);
        const carouselInner = document.querySelector('#myCarousel .carousel-inner');
        const carouselIndicators = document.querySelector('#myCarousel .carousel-indicators');
        
        carouselInner.innerHTML = '';
        carouselIndicators.innerHTML = '';
        
        for (let i = 0; i <forecastInfoArray.length; i++){
            
            // if (i != 1)
            //     continue;

            // 1. Create the carousel item (slide)
            const carouselItem = document.createElement('div');
            carouselItem.classList.add('carousel-item');
            //carouselItem.style.backgroundcolor = "#777";
            
            const svg = document.createElement('svg');
            //svg.classList.add("bd-placeholder-img" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" preserveAspectRatio="xMidYMid slice" focusable="false"');
            svg.classList.add("bd-placeholder-img");
            svg.setAttribute("width","100%");
            svg.setAttribute("height","100%");
            svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
            svg.setAttribute("aria-hidden", "true");
            svg.setAttribute("preserveAspectRatio", "xMidYMid slice");
            svg.setAttribute("focusable","false");
            //const rect = document.createElement('rect');
            //rect.classList.add("myCarousel-rect");
            //rect.setAttribute("width", "100%");
            //rect.setAttribute("height", "100%");
            //rect.setAttribute("fill", "#777");
            //svg.appendChild(rect);
            svg.innerHTML = '<rect width="100%" height="100%" fill="#777"/>';
            //let svg = '<svg class="bd-placeholder-img" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" preserveAspectRatio="xMidYMid slice" focusable="false"><rect width="100%" height="100%" fill="#777"/></svg>'
            //carouselItem.appendChild(svg);
            carouselItem.appendChild(svg);
            
            // The first item must have the 'active' class
            if (i === 0) {
                carouselItem.classList.add('active');
            }

            const carouselContainer = document.createElement("div");
            carouselContainer.classList.add("container");

            const carouselCaption = document.createElement('div');
            carouselCaption.classList.add("carousel-caption");
        
            // 2. Add the image element inside the item
            const img = document.createElement("img");
            img.src = forecastInfoArray[i].weatherIconURL;
            //img.classList.add('d-block', 'w-100'); // Bootstrap classes for responsive images
            img.classList.add("myCarousel-image"); 
            img.alt = `Slide ${i + 1}`;
            //carouselCaption.append(`<p>${forecastInfoArray[i].day}</p>`, img);
            carouselCaption.innerHTML = `<h1 style="color:black">${forecastInfoArray[i].day}</h1>
                                         <p style="color:black">${forecastInfoArray[i].temperatureString}</p>
                                         <p style="color:black">${forecastInfoArray[i].shortForecast}</p>`;
            carouselCaption.appendChild(img);
        
            carouselContainer.appendChild(carouselCaption);
            carouselItem.appendChild(carouselContainer);
            // 3. Append the item to the carousel inner
            carouselInner.appendChild(carouselItem);

            // 4. Create the indicator button
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

            // 5. Append the indicator to the carousel indicators container
            carouselIndicators.appendChild(indicatorButton);
            
        }
    }
    else {
        document.getElementById("forecast").innerHTML = `Error status: ${response.status}`;
    }
}

function populateCarousel(forecastInfoArray){
}