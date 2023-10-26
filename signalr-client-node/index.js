process.env["NODE_TLS_REJECT_UNAUTHORIZED"] = 0; // Disabling certificate verification

const signalR = require("@microsoft/signalr");
let computer = {};

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7275/chathub")
    .configureLogging(signalR.LogLevel.Information)
    .build();


async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
}

connection.on("RequestLogin", (message, data) => {
    console.log(message, data);
    // send a message with a username
    connection.invoke("Login", "john");
});

connection.on("LoginSuccess", (message, data) => {
    computer = data;
    console.log(message, data);
});

connection.on("ReceiveMessage", (message, data) => {
    console.log(message, data);
});

start();