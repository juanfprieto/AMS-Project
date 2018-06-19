<%@ Page language="c#" Codebehind="AMS.Web.Inicio.aspx.cs" AutoEventWireup="True" Inherits="AMS.Web.Inicio" %>


<!DOCTYPE html>
<html lang="es">
    <head>
        <script type="text/javascript" src="../js/jquery.js"></script>
        <script type="text/javascript" src="../js/jquery-ui.js"></script>
        <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
        <script type="text/javascript" src="../js/ui/jquery.ui.core.js" ></script>
        <script type="text/javascript" src="../js/ui/jquery.ui.widget.js" ></script>
        <script type="text/javascript" src="../js/ui/jquery.ui.datepicker.js" ></script>
        <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
        <script src="../js/bootstrap.min.js"></script>
        <script type ="text/javascript" src="../js/generales.js"></script>
               
        <!--estilos, normalizacion y bootstrap-->
        <link href="../css/normalize.css" type="text/css" rel="stylesheet">
        <link href="../css/AMS.css" type="text/css" rel="stylesheet">
        <link href="../css/bootstrap.min.css" rel="stylesheet"/>
        <link href="../css/bootstrap-theme.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="../js/bootstrap.min.js"></script>
        <script type="text/javascript" src="../js/npm.js"></script>
        <!-- fin de estilos, normalizacion y bootstrap-->
        <link href="../css/AMS.Menu.css" type="text/css" rel="stylesheet" media="screen" />        
        <link href="../css/tabber.css" type="text/css" rel="stylesheet" media="screen" />
        <link href="../css/jquery-ui.css" type="text/css" rel="stylesheet" media="screen" />

		<meta charset="utf-8" />
		<title>AMS</title>		
        
        <script type="text/javascript">
            $(document).on("ready", medirAlto);

            function medirAlto()
            {
                var alto = $(window).height(); //+ "px";// este es el alto normal y se ve influenciado por otro elementos(cono el div de inspeccionar una página)
                var alto1 = screen.height; //este es el total de altura de la pantalla sin tener en cuenta otros elementos
                var alto2 = window.screen.availHeight; //este es el ideal.
                alto2 -= 68;

                //var body = document.body;
                //var html = document.documentElement;
                //var alto3 = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
                
                //document.getElementById("").css("height", alto);
                //document.getElementById("").css("height", alto);
                //div1.css("height", alto);
                //div2.css("height", alto);
                //alert(alto + ' - ' + alto2);
                //alert(alto + ' - ' + alto1 + ' - ' + alto2 + " - " + alto3);
                //alert(window.screen.availHeight);
                //$('#all').css("height", alto2);
                $('#NavigationFrame').css("height", alto2);
                $('#cargar').css("height", alto2);
                $('#Opciones').css("height", alto2);
                $('#cargar').css("max-height", alto2);
                $('#Opciones').css("max-height", alto2);
                
            }
        </script>      
	</head>
    <body>
        <div id="all" class="hidden"></div>
        <div id="rest" class="hidden">            
            <img id="imagenCarga" src="../img/jMenu/rest.gif" alt="Cargando..." /><br />
            <label>
                <b id="cargaText" class="alert alert-info">Ecas Cloud Cargando...</b>
            </label>
        </div>
        <div id="NavigationFrame" class='col-md-3'>               
		</div>
		<div id="Opciones" class="col-md-9">
            <input type="checkbox"  id="showMenu" /> 
            <label  class="menu-trigger" for="showMenu" > <span class="glyphicon glyphicon-triangle-left" aria-hidden="true"></span>&nbsp;<span class="glyphicon glyphicon-align-justify" aria-hidden="true"></span></label>
            <iframe id="cargar">                    
            </iframe>                
		</div><br />
           
        </body>
	</html>