var express = require('express');
var app     = express();
var http    = require('http').Server(app);
var io      = require('socket.io')(http);
var users   = {};

//Routing
app.use(express.static(__dirname + "/public"));

http.listen(3000, function(){
    console.log('listening on *:3000');
})

io.on('connection', function(client){
    console.log('user is connected');

    client.on('join', function(userName){
        users[client.id] = userName;
        client.emit('chat', 'You have connected as \'' + userName + '\'');
        client.emit('connected');
        client.broadcast.emit('chat', userName + ' entered in chat');
    })

    client.on('typing', function(){
        console.log('typing');
        client.broadcast.emit('chat', 'typing...');
    });    

    client.on('disconnect', function(){
        console.log('user is disconnected');
    });
    
    client.on('chat message server', function(message){
        console.log(users[client.id]);
        client.broadcast.emit('chat', message, users[client.id]);
    });
});

