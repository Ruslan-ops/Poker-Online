const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/game")
    .build();

hubConnection.start();

hubConnection.on("SitAtRandomPlace", function (message, userName) {
    console.log(message);
    console.log(userName);
});

hubConnection.invoke('SitAtRandomPlace', "Hello SignalR");

