var refreshinterval = 20;
var starttime;
var nowtime;
var reloadseconds = 0;
var secondssinceloaded = 0;
var btnActualizar;
var cambio = 1;
var posX = $(window).scrollLeft();
var iniciar = 0;

$(document).on("ready", inicio);
function inicio() {
    //Temporizador para cambio de paginas.
    clearInterval(idInt);
    idInt = setInterval('SetURL()', intervalo);

    //KeyPress
    $(document).keypress(
        function (e) {
            if (e.which == 13) {
                $('#camb').trigger('click');
            }
        }
    );
    starttime();
}

function starttime() {
    starttime = new Date();
    starttime = starttime.getTime();
    pageScroll();
}

function pageScroll() {
    var tiempo = 40;
    if (iniciar == 2) {
        $(window).scrollLeft(0);
        iniciar = 0;
    }

    if (cambio == 1) {
        if ($(window).scrollLeft() == 0) {
            tiempo = 5000;
        }
        this.window.scrollBy(1, 0); // horizontal and vertical scroll increments x,y
        if (posX == $(window).scrollLeft()) {
            iniciar = 1;
            tiempo = 5000;
        }
        else {
            posX = $(window).scrollLeft();
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

function cambioModo(boton) {
    if (boton.value == "Pantalla") {
        $('.meca').css({ "position": "absolute" });
        cambio = 0;
        $("#mover").attr('value', 'Seguir>>');
        $(window).scrollLeft(0);
        boton.value = "Tablero";
        clearInterval(idInt);
    }
    else {
        $('.meca').css({ "position": "fixed" });
        cambio = 1;
        $("#mover").attr('value', 'Parar!');
        $(window).scrollLeft(0);
        boton.value = "Pantalla";
        idInt = setInterval('SetURL()', intervalo);
    }

}

function cambioURL(indA, indM) {
    var urlA = document.URL;
    var original = '&pag=' + indA;
    var cambio = '&pag=' + indM;
    var urlA = urlA.replace(original, cambio);

    return urlA;
}

function espera() {
    var imaG = document.getElementById("contImg");
    imaG.style.visibility = "visible";
}

//Temporizador para cambio de paginas.

var intervalo = 30000; //milisegundos cambio de pantalla planning.
var idInt; //temporizador.

function SetURL() {
    clearInterval(idInt);
    //Tomo la url completa
    var url = document.location.href.toString();
    //Saco las variables get
    var gets = url.substring((url.indexOf("?")) + 1, url.length);
    //Divido las variables get
    var arrGets = gets.split('&');
    //En la var get de la pagina saco el valor con el que viene y le sumo uno
    var nPag = arrGets[1].substring(arrGets[1].indexOf("=") + 1, arrGets[1].length)
    var numPag = parseInt(nPag) + 1;

    var tipoFiltro = "";
    if (arrGets[3] !== undefined && arrGets[3].indexOf("planning") != -1)
        tipoFiltro = "&" + arrGets[3];

    //Redirecciono a la nueva pagina
    var pagina = '';
    if(planning == 'vehiculos')
        pagina = 'AMS.Automotriz.PlanificacionTallerVeh.aspx?' + arrGets[0] + '&pag=' + numPag.toString(); // + "&" + arrGets[2] + tipoFiltro;
    else
        pagina = 'AMS.Automotriz.PlanificacionTaller.aspx?' + arrGets[0] + '&pag=' + numPag.toString() + "&" + arrGets[2] + tipoFiltro;
    document.location.href = pagina;
}