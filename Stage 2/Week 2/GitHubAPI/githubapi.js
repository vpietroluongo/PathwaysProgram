async function getReposForUser(){
    let apiString = "https://api.github.com/users";
    let githubUserID = document.getElementById("githubUserID").value;
    apiString += `/${githubUserID}/repos?sort=updated`;
    //apiString += `/${githubUserID}/repos`;
    alert(apiString);

    let response = await fetch(apiString);
    
    document.getElementById("theUserID").innerHTML = "";
    document.getElementById("theRepos").innerHTML = "";
    
    if (response.status >= 200 && response.status <= 299) {
        console.log(`200-299 ${response.status}`);

        let jsonData = await response.json();
        document.getElementById("theUserID").innerHTML = githubUserID;
        for (let repo in jsonData){
            document.getElementById("theRepos").innerHTML += "<p href=\"" + jsonData[repo].html_url + "\">\"" + jsonData[repo].name + "</p>";
        }
    }
    else if (response.status >= 300 && response.status <= 399) {
        console.log(`300-399 ${response.status}`);
        document.getElementById("theRepos").innerHTML = `<p>Request is redirected to another URL: ${response.status}</p>`;
    }
    else if (response.status >= 400 && response.status <= 499) {
        console.log(`400-499 ${response.status}`);
        document.getElementById("theRepos").innerHTML = `<p>Error from client side: ${response.status} ${response.statusText}</p>`;
    }
    else if (response.status >= 500 && response.status <= 599) {
        console.log(`500-599 ${response.status}`);
        document.getElementById("theRepos").innerHTML = `<p>Error from server side: ${response.status}</p>`;
    }
    else {
        console.log(`Error ${response.status}`);
    }
    
    document.getElementById("githubUserID").value = "";
}