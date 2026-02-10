console.log("outside function");
function myFunction() 
{
    var newtheme = document.getElementById("theme").value;
    alert ("Survey received!  We'll see you for the " + newtheme + " themed party!");
    //alert ("Survey received!  We'll see you for the themed party!");
    console.log("in function");
}