var refreshinterval = 20;
var starttime;
var nowtime;
var reloadseconds = 0;
var secondssinceloaded = 0;
var btnActualizar;
var cambio = 1;
var posY = $(window).scrollTop();
var iniciar = 0;

//$(document).on("ready", inicio);
//function inicio2() {
////    $(document).keypress(
////        function (e) {
////            if (e.which == 13) {
////                $('#camb').trigger('click');
////            }
////        }
////    );
//    starttime();
//}

function starttime() {
    starttime = new Date();
    starttime = starttime.getTime();
    pageScroll();
}

function pageScroll() {
    var tiempo = 40;
    if (iniciar == 2) {
        $(window).scrollTop(0);
        iniciar = 0;
        $("#Form1").submit();
    }

    if (cambio == 1) {
        if ($(window).scrollTop() == 0) {
            tiempo = 5000;
        }
        this.window.scrollBy(0, 1); // horizontal and vertical scroll increments x,y
        if (posY == $(window).scrollTop()) {
            iniciar = 1;
            tiempo = 5000;
        }
        else {
            posY = $(window).scrollTop();
        }

    }
    if (iniciar == 0) {
        scrolldelay = setTimeout('pageScroll()', tiempo); // scrolls every 80 milliseconds
    }
    else {
        scrolldelay = setTimeout('pageScroll()', tiempo);
        iniciar = 2;
    }
}

function cambioScroll(boton) {
    if (cambio == 1) {
        cambio = 0;
        boton.value = "Seguir>>";
    }
    else {
        cambio = 1;
        boton.value = "Parar!";
    }
}