var socket    = io();
var connected = false;

$(document).ready(function(){
    $('#m').prop('disabled', true);
});

$('form').submit(function(){
    var $message = $('#m');
    socket.emit('chat message server', $message.val());
    appendMessage($message.val(), 'you');
    $message.val('');
    return false;
});

$('#user').change(function(){
    socket.emit('join', $('#user').val());
});

$('#m').keydown(function(key){
    if (key.keyCode == 13) return;

    //socket.emit('typing');
});

socket.on('connected', function(){
    $('#m').prop('disabled', false);
})

socket.on('chat', function(message, userName){
    appendMessage(message, userName);
});

socket.on('chat message client', function(message){
    appendMessage(message);
});

var appendMessage = function(message, userName) {
    var person = "says: ";

    if (userName == "you") person = "say: ";
    if (userName == undefined) { userName = ""; person = ""; }

    $('#messages').append($('<li>').text(userName + ' ' + person + message));
};