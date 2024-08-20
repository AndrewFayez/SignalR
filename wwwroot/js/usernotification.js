"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationUserHub").build();   

connection.on("sendToUser", (articleHeading, articleContent) => {
    var heading = document.createElement("h3");
    heading.textContent = articleHeading;
    var p = document.createElement("p");
    p.innerText = articleContent;
    var div = document.createElement("div");
    div.appendChild(heading);
    div.appendChild(p); document.getElementById("articleList").appendChild(div);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
}).then(function () {
    document.getElementById("sendMesg").addEventListener("click", async (event) => {
        var groupRec = document.getElementById("signalRConnectionId").value;
      
        try {
            await connection.invoke("GetConnectionId", groupRec);
        }
        catch (e) {
            console.error(e.toString());
        }
        event.preventDefault();
    });
});


///*document.getElementById("sendMesg").innerHTML = "UserId: " + userId;*/
//var recId = document.getElementById("recId").value;
//connection.invoke("GetConnectionId", recId).then(function (connectionId) {
//    document.getElementById("signalRConnectionId").innerHTML = connectionId;
//})

