async function lookupAuthor(){
    //clear previous HTML elements
    document.getElementById("coverSpot").innerHTML = "";
    document.getElementById("authorImageSpot").innerHTML = "";
    document.getElementById("authorName").innerHTML = "";
    document.getElementById("birthday").innerHTML = "";
    document.getElementById("authorInfo").innerHTML = "";
    bookTitle.innerHTML = "";
    document.getElementById("publishDate").innerHTML = "";
    document.getElementById("authorNameDisplayTop").innerHTML = "";

    //setup first API call to get author key and their top work
    let apiString = "https://openlibrary.org/";
    let author = (document.getElementById("author").value);
    author = encodeURIComponent(author);  //encodes characters, such as spaces as %20
    apiStringAuthor = apiString + "search/authors.json?q=" + author;
    console.log(`apiStringAuthor: ${apiStringAuthor}`);
    
    let response = await fetch(apiStringAuthor);
    

    let myBookBox = document.getElementById("myBookBox");  
    if (response.status >= 200 && response.status <= 299){
        let jsonData = await response.json();
        
        //check if author exists
        if (jsonData.numFound != 0){
            myBookBox.classList.remove("my-box-hidden");
            
            let authorKey = jsonData.docs[0].key;
            console.log(`Author key: ${authorKey}`);
            
            let topWork = jsonData.docs[0].top_work;
            console.log(`Top work: ${topWork}`);
            let topWorkFormatted = encodeURIComponent(topWork);

            //setup second API call using author key to get author info, such as their bio, birth date, and personal name
            let apiStringAuthorInfo = apiString + "authors/" + authorKey + ".json";
            console.log(`apiStringAuthorInfo: ${apiStringAuthorInfo}`);
            response = await fetch(apiStringAuthorInfo);
            if (response.status >= 200 && response.status <= 299){
                jsonData = await response.json();

                //check if author has a bio in bio.value, and if so, display it. If not, hide the author info box
                if (jsonData.bio && jsonData.bio.value){
                    if (document.getElementById("authorInfoBox").classList.contains("my-box-hidden")){
                        document.getElementById("authorInfoBox").classList.remove("my-box-hidden");
                    }
                     if (document.getElementById("authorNameDisplayTop").classList.contains("my-box-hidden")) {
                        document.getElementById("authorNameDisplayTop").classList.remove("my-box-hidden");
                    }
                    document.getElementById("authorInfo").innerHTML = jsonData.bio.value;
                    document.getElementById("authorInfo").style.fontSize = "10px";
                    document.getElementById("authorName").innerHTML = jsonData.personal_name;
                    document.getElementById("authorNameDisplayTop").innerHTML = jsonData.personal_name;
                    document.getElementById("birthday").innerHTML = jsonData.birth_date;

                    authorImageBoxSetup(authorKey);
                }
                //check if author has a bio in bio, and if so, display it. If not, hide the author info box
                else if (jsonData.bio && (typeof jsonData.bio === "string")){
                    //check if author info box is hidden, and if so, show it
                    if (document.getElementById("authorInfoBox").classList.contains("my-box-hidden")) {
                        document.getElementById("authorInfoBox").classList.remove("my-box-hidden");
                    }
                    if (document.getElementById("authorNameDisplayTop").classList.contains("my-box-hidden")) {
                        document.getElementById("authorNameDisplayTop").classList.remove("my-box-hidden");
                    }
               
                    document.getElementById("authorInfo").innerHTML = jsonData.bio;
                    document.getElementById("authorInfo").style.fontSize = "10px";
                    document.getElementById("authorName").innerHTML = jsonData.personal_name;
                    document.getElementById("authorNameDisplayTop").innerHTML = jsonData.personal_name;
                    document.getElementById("birthday").innerHTML = jsonData.birth_date;

                    authorImageBoxSetup(authorKey);
                } 
                //if author doesn't have a bio, hide the author info box
                else{
                    document.getElementById("authorInfoBox").classList.add("my-box-hidden");
                }
            }
            else{
                document.getElementById("authorInfoBox").classList.add("my-box-hidden");
                alert(`Error: ${response.status}`);
            }
            
            //setup third API call using author key and top work to get book info, such as the cover key and publish date
            let apiStringTopWork = apiString + "search.json?q=" + topWorkFormatted + "&author=" + authorKey;
            console.log(`apiStringTopWork: ${apiStringTopWork}`);
            response = await fetch(apiStringTopWork);
            jsonData = await response.json();

            let coverKey = jsonData.docs[0].cover_edition_key;
            console.log(`coverKey: ${coverKey}`);
            let publishDate = jsonData.docs[0].first_publish_year;
            document.getElementById("publishDate").innerHTML = publishDate;
            let bookTitle = document.getElementById("bookTitle");
            bookTitle.innerHTML = topWork;
            
            //setup URL to get the book cover
            let coverURL = `https://covers.openlibrary.org/b/olid/${coverKey}-M.jpg`;
            console.log(`Cover URL: ${coverURL}`);

            const myDiv = document.querySelector('#myDiv .row g-0 border rounded overflow-hidden flex-md-row mb-4 shadow-sm h-md-250 position-relative');
            const img = document.createElement("img");
  
            //check if cover key exists, and if so, display the cover. If not, display an "image not found" placeholder
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
            //hide book info and author info boxes if author isn't found, and alert the user 
            myBookBox.classList.add("my-box-hidden");
            alert(`Author ${author} not found.`);
            document.getElementById("authorNameDisplayTop").innerHTML = "Author Not Found";
        }
    }
    else{
        alert(`Error: ${response.status}`);
    }

    //clear the input text box after search
    document.getElementById("author").value = "";
}

//set up an "image not found" placeholder using svg if the book cover or author image isn't found
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

//set up the box containing the author image
function authorImageBoxSetup(authorKey, authorName){      
    let authorURL = `https://covers.openlibrary.org/a/olid/${authorKey}-M.jpg`;

    const myDiv = document.querySelector('#myDiv .row g-0 border rounded overflow-hidden flex-md-row mb-4 shadow-sm h-md-250 position-relative');
    const img = document.createElement("img");

    //check if author key exists, and if so, display the author image. If not, display an "image not found" placeholder
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
