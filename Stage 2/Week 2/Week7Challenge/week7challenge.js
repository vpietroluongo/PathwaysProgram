async function lookupAuthor(){
    let apiString = "https://openlibrary.org/";
    let author = (document.getElementById("author").value);
    //author = author.replaceAll(". ", "%20");
    //author = author.replaceAll(" ", "%20");
    author = encodeURIComponent(author);
    apiStringAuthor = apiString + "search/authors.json?q=" + author;
    alert(apiStringAuthor);

    let response = await fetch(apiStringAuthor);

    document.getElementById("coverSpot").innerHTML = "";
    document.getElementById("authorImageSpot").innerHTML = "";
    document.getElementById("authorName").innerHTML = "";
    document.getElementById("birthday").innerHTML = "";
    document.getElementById("authorInfo").innerHTML = "";
    bookTitle.innerHTML = "";
    document.getElementById("publishDate").innerHTML = "";

    let myBookBox = document.getElementById("myBookBox");  
    if (response.status >= 200 && response.status <= 299){
        let jsonData = await response.json();
        
        if (jsonData.numFound != 0){
            myBookBox.classList.remove("my-box-hidden");
            
            let authorKey = jsonData.docs[0].key;
            console.log(authorKey);
            
            let topWork = jsonData.docs[0].top_work;
            console.log(topWork);
            let topWorkFormatted = encodeURIComponent(topWork);
            //topWorkFormatted = topWork.replaceAll(" ", "%20");

            let apiStringAuthorInfo = apiString + "authors/" + authorKey + ".json";
            alert(apiStringAuthorInfo);
            response = await fetch(apiStringAuthorInfo);
            if (response.status >= 200 && response.status <= 299){
                jsonData = await response.json();
                if (jsonData.bio && jsonData.bio.value){
                    if (document.getElementById("authorInfoBox").classList.contains("my-box-hidden")){
                        document.getElementById("authorInfoBox").classList.remove("my-box-hidden");
                        document.getElementById("authorInfo").innerHTML = jsonData.bio.value;
                        document.getElementById("authorInfo").style.fontSize = "10px";
                    }
                    else {
                        document.getElementById("authorInfo").innerHTML = jsonData.bio.value;
                        document.getElementById("authorInfo").style.fontSize = "10px";

                    }
                    
                    
                    document.getElementById("authorName").innerHTML = jsonData.personal_name;
                    document.getElementById("birthday").innerHTML = jsonData.birth_date;
                    authorInfoBoxSetup(authorKey);
                }
                else if (jsonData.bio && (typeof jsonData.bio === "string")){
                    if (document.getElementById("authorInfoBox").classList.contains("my-box-hidden")) {
                        document.getElementById("authorInfoBox").classList.remove("my-box-hidden");
                        document.getElementById("authorInfo").innerHTML = jsonData.bio;
                        document.getElementById("authorInfo").style.fontSize = "10px";
                    }
                    else {
                        document.getElementById("authorInfo").innerHTML = jsonData.bio;
                        document.getElementById("authorInfo").style.fontSize = "10px";

                    }

                    document.getElementById("authorName").innerHTML = jsonData.personal_name;
                    document.getElementById("birthday").innerHTML = jsonData.birth_date;
                    authorInfoBoxSetup(authorKey);
                }   
                else{
                    document.getElementById("authorInfoBox").classList.add("my-box-hidden");
                }
            }
            else{
                document.getElementById("authorInfoBox").classList.add("my-box-hidden");
                alert(`Error: ${response.status}`);
            }
            
            let apiStringTopWork = apiString + "search.json?q=" + topWorkFormatted + "&author=" + authorKey;
            response = await fetch(apiStringTopWork);
            jsonData = await response.json();
            let coverKey = jsonData.docs[0].cover_edition_key;
            console.log(coverKey);
            let publishDate = jsonData.docs[0].first_publish_year;
            document.getElementById("publishDate").innerHTML = publishDate;
            let bookTitle = document.getElementById("bookTitle");
            bookTitle.innerHTML = topWork;

            
            
            let coverURL = `https://covers.openlibrary.org/b/olid/${coverKey}-M.jpg`;
            console.log(coverURL);

            const myDiv = document.querySelector('#myDiv .row g-0 border rounded overflow-hidden flex-md-row mb-4 shadow-sm h-md-250 position-relative');
            const img = document.createElement("img");
            // if (coverKey == undefined){
            //     const svgNS = "http://www.w3.org/2000/svg";
            //     const svg = document.createElementNS(svgNS, "svg");
            //     svg.classList.add("bd-placeholder-img");
            //     svg.setAttribute("width","200");
            //     svg.setAttribute("height","250");
            //     svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
            //     svg.setAttribute("aria-hidden", "true");
            //     svg.setAttribute("preserveAspectRatio", "xMidYMid slice");
            //     svg.setAttribute("focusable","false");

            //     // 3.  Create the rect (the grey background rectangle)  
            //     const rect = document.createElementNS(svgNS, "rect");
            //     rect.setAttribute("width", "100%");
            //     rect.setAttribute("height", "100%");
            //     rect.setAttribute("fill", "#55595c");
                
            //     const text = document.createElementNS(svgNS, "text");
            //     text.setAttribute("x", "50%");
            //     text.setAttribute("y", "50%");
            //     text.setAttribute("fill", "#eceeef");
            //     text.setAttribute("dy", ".3em");
             
                
            //     text.innerHTML = "Cover Not Found";
            //     svg.appendChild(rect);
            //     svg.appendChild(text);
            //     document.getElementById("coverSpot").appendChild(svg);
            // }
            if (coverKey == undefined) {
                imageNotFoundBoxSetup("cover");
            }
            else{
                img.src = coverURL;
                img.alt = `${topWork} Cover`;
                img.title = `${topWork} Cover`;
                img.setAttribute("width", "200");
                img.setAttribute("height", "250");
                document.getElementById("coverSpot").appendChild(img);
            }
        }
        else{
             myBookBox.classList.add("my-box-hidden");
            alert(`Author ${author} not found.`);
        }
    }
    else{
        alert(`Error: ${response.status}`);
    }


    document.getElementById("author").value = "";
}


function imageNotFoundBoxSetup(flag){
        const svgNS = "http://www.w3.org/2000/svg";
        const svg = document.createElementNS(svgNS, "svg");
        svg.classList.add("bd-placeholder-img");
        svg.setAttribute("width", "200");
        svg.setAttribute("height", "250");
        svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
        svg.setAttribute("aria-hidden", "true");
        svg.setAttribute("preserveAspectRatio", "xMidYMid slice");
        svg.setAttribute("focusable", "false");

        // 3.  Create the rect (the grey background rectangle)  
        const rect = document.createElementNS(svgNS, "rect");
        rect.setAttribute("width", "100%");
        rect.setAttribute("height", "100%");
        rect.setAttribute("fill", "#55595c");

        const text = document.createElementNS(svgNS, "text");
        text.setAttribute("x", "50%");
        text.setAttribute("y", "50%");
        text.setAttribute("fill", "#eceeef");
        text.setAttribute("dy", ".3em");


        text.innerHTML = "Image Not Found";
        svg.appendChild(rect);
        svg.appendChild(text);

        if (flag == "cover"){
            document.getElementById("coverSpot").appendChild(svg);
        }
        else{
            document.getElementById("authorImageSpot").appendChild(svg);
        }
}


function authorInfoBoxSetup(authorKey, authorName){      
    let authorURL = `https://covers.openlibrary.org/a/olid/${authorKey}-M.jpg`;

    const myDiv = document.querySelector('#myDiv .row g-0 border rounded overflow-hidden flex-md-row mb-4 shadow-sm h-md-250 position-relative');
    const img = document.createElement("img");

    if (authorKey == undefined) {
        imageNotFoundBoxSetup("author");

    }
    else {
        img.src = authorURL;
        img.alt = `${authorName} Cover`;
        img.title = `${authorName} Cover`;
        img.setAttribute("width", "200");
        img.setAttribute("height", "250");
        document.getElementById("authorImageSpot").appendChild(img);
    }
}
