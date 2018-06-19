$(document).on("ready", inicio);

var press = 0;
function inicio() {
    
    agregarEspera();
    onChangeCalendar();    
    cargarMenu();
    cargarBienvenida();
    $("label[for='showMenu']").on('click', ocultaMenu);
    $("#cargar").on('load', function () {
        $("#all").addClass('hidden');
        $("#rest").addClass('hidden');
    })
   
}

function ver(obj)
{
    alert(obj);
}

function nuevoAjax() {
    var xmlhttp = false;
    try {
        xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (E) {
            xmlhttp = false;
        }
    }

    if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
        xmlhttp = new XMLHttpRequest();
    }
    return xmlhttp;
}


function cargarArchivo(url) {
    var contenedor;
    contenedor = $('#cargar');    
    if (url !== 0 || url !== '') {
        $("#all").removeClass('hidden');
        $("#rest").removeClass('hidden');
        contenedor.attr('src', url);
        
        }
    else  {
        $("#all").addClass('hidden');
        $("#rest").addClass('hidden');
    }
    ocultaMenu();
    //var alto = $(window).height() + "px";
    var alto2 = window.screen.availHeight;
    alto2 -= 61;
    $('#Opciones').css("height", alto2);
    $('#contImg').css("height", alto2);
    $('#cargar').css("height", alto2 - 3);
}



function cargarBienvenida() {
    $('#cargar').attr('src','AMS.Web.bienvenida.aspx');
}

function cargarMenu() {   
    $('#NavigationFrame').load('AMS.Web.JMenu.aspx');    
}

function ocultaMenu() {
    if (press == 0) {
        $("label[for='showMenu']").html('<span class="glyphicon glyphicon-triangle-right" aria-hidden="true"></span>&nbsp;<span class="glyphicon glyphicon-list" aria-hidden="true"></span>');
        $("#NavigationFrame").toggle('slow', function () {
            $("#Opciones").removeClass('col-md-9');
            $("#Opciones").addClass('col-md-12');
        });       
        press = 1;
    }
    else {
        $("#Opciones").removeClass('col-md-12');
        $("label[for='showMenu']").html('<span class="glyphicon glyphicon-triangle-left" aria-hidden="true"></span>&nbsp;<span class="glyphicon glyphicon-align-justify" aria-hidden="true"></span>');
        $("#NavigationFrame").toggle('slow', function () {            
            $("#Opciones").addClass('col-md-9');
        });
        
        press = 0;
    }
}

      
    //(function () {
    //    var $body = document.body
    //    , $menu_trigger = $body.getElementsByClassName('menu-trigger')[0];

    //    if (typeof $menu_trigger !== 'undefined') {
    //        $menu_trigger.addEventListener('click', function () {
    //            $body.className = ($body.className == 'menu-active') ? '' : 'menu-active';
    //        });
    //    }

    //}).call(this);

function onChangeCalendar()
{
    for (i = 0; i < 10; i++) {
        controlCalendar = "#_ctl1_img_" + i;
        $(controlCalendar).hover(
            function()
            {
                $(this).attr("src", "../img/AMS.Icon.Calendarioanimado.gif");
                $(this).datepicker("show");
            },function(){
             $(this).attr("src", "../img/AMS.Icon.Calendar.png");                
            }
            )
    }
}

function agregarEspera() {   
    $("input:submit").on("click", espera);
    $("input:button").on("click", espera);    
    $(".noEspera").off("click", espera);
    $("input:text").addClass("form-control");
    $("input:button").addClass("btn btn-primary");
    $("input:submit").addClass("btn btn-primary");
    $("select").addClass("form-control");
    
}

function espera() 
{
    var imaG = document.getElementById("contImg");
    imaG.style.visibility = "visible";
}

function mostrarConfirmacion(mensaje) {
    return confirm(mensaje);
}

function establecer() {
    var direccion = document.URL;
    var n = direccion.split("aspx");
    window.open("" + n[0], "_self");
}

function cambiar(obj) {
    var caja = document.body.clientWidth;
    var pantalla = screen.width;

    if ((pantalla == 1024 && caja >= 1000) ||
            (pantalla == 1280 && caja >= 1256) ||
            (pantalla == 800 && caja >= 776) ||
            (pantalla == 640 && caja >= 664)) {
        obj.value = "Poner";
    }
    else {
        obj.value = "Quitar";
    }
    s = obj.value == "Quitar";

    destino = (s) ? 0 : 30;
    incremento = (s) ? -1 : 1;
    origen = (s) ? 30 : 0;
    obj.value = (s) ? "Poner" : "Quitar";
    mover(origen, destino, incremento);
}

function mover(origen, destino, incremento) {
    origen += incremento;
    top.document.getElementById('indexFrameSet').cols = origen + '%,*';
    if (origen != destino) {
        o = origen;
        d = destino;
        i = incremento;
        setTimeout("mover(o,d,i)", 50);
    }
}

function CallConfirmBox(obj, msg) {
    if (confirm(msg))
        return true;
    else {
        $(obj).off("click", espera);
        return false;
    }
}
function soloNumero(evt, obj) {
    var valor = evt.key;
    var esLetra = isNaN(valor);
    if (esLetra || valor == " ") {
        return false;
    } else
        return true;
}