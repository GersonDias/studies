// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener( 'resume', onResume.bind( this ), false );
        
        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };
})();

(function () {
    document.getElementById('camera').addEventListener('click', abrirCamera);
    document.getElementById('album').addEventListener('click', abrirAlbum);

    function abrirCamera() {
        navigator.camera.getPicture(captureSuccess, onFail, {
            quality: 50,
            destinationType: navigator.camera.DestinationType.FILE_URI
        });
    }

    function abrirAlbum() {
        navigator.camera.getPicture(captureSuccess, onFail, {
            quality: 50,
            destinationType: navigator.camera.DestinationType.FILE_URI,
            sourceType: navigator.camera.PictureSourceType.SAVEDPHOTOALBUM 
        });
    }

    function captureSuccess(foto) {
        document.getElementById('foto').src = foto;
    }

    function onFail(err) {
        alert("Erro: " + err);
        //usar o puglin Notification (cordova-plugin-dialog)
    }
})();   