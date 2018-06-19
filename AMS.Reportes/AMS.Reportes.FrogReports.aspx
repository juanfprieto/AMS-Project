<%@ page language="C#" autoeventwireup="true" codebehind="AMS.Reportes.FrogReports.aspx.cs"  inherits="AMS.Reportes.FrogReports" %>

<%@ Register Assembly="GridViewTree" Namespace="DigitalTools" TagPrefix="DT" %>
<%@ register tagprefix="owd" namespace="OboutInc.Window" assembly="obout_Window_NET" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:scriptmanager id="ScriptManager" runat="server" />
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>AMS</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../img/AMS.ico" type="image/ico" rel="icon">
    <link href="../css/AMS.css" type="text/css" rel="stylesheet">

    <link rel="stylesheet" href="../css/fecha.css" />

    <script type="text/javascript" src="../js/jquery-1.4.1.js"></script>
    <script type="text/javascript" src="../js/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="../js/jquery-1.4.1-vsdoc.js"></script>
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.datepicker.js"></script>
    <link href="../css/jquery-ui.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
    <script type="text/javascript" src="../js/generales.js"></script>
    <script type="text/javascript" src="../js/modernizr.js"></script>  



    <link rel="stylesheet" href="../css/jquery.ui.all.css" />
    <link rel="stylesheet" href="../css/jquery.ui.theme.css" />

    <script type="text/javascript">

      
        $(document).ready(function () {

            $(window).scroll(function () {
                var top = -99;
                var posLabel = $('#lblTitulo').position();
                var p = $('#grid1_ob_grid1MainContainer').position();
                top += p.top;
                if ($(window).scrollTop() >= posLabel.top) {
                    var largo = $(window).width();
                    var largoGrilla = $('#grid1_ob_grid1HeaderContainer').css('width').substring(0, $('#grid1_ob_grid1HeaderContainer').css('width').length - 2);
                    //var lGrilla = parseInt(largoGrilla);
                    //var largoGrilla1 = lGrilla + 1;
                    //alert(window.screen.availWidth + ' - ' + $('#grid1_ob_grid1HeaderContainer').css('width') + ' - ' + largo + ' - ' + largoGrilla + ' - ' + largoGrilla1);

                    if (largo > largoGrilla) {
                        $('#grid1_ob_grid1HeaderContainer').addClass("ob_gHConte");
                        $('#grid1_ob_grid1HeaderContainer').css('width', largoGrilla);
                    }
                    else {
                        $('#grid1_ob_grid1HeaderContainer').addClass("ob_gHConte");
                    }



                    if ($(window).scrollLeft() > 0) {
                        $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHConte");
                        $('#grid1_ob_grid1HeaderContainer').addClass("ob_gHContScroll");

                        $('#grid1_ob_grid1HeaderContainer').css('top', $(this).scrollTop() - top);
                    }
                    else {
                        $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHContScroll");
                        $('#grid1_ob_grid1HeaderContainer').css('top', '100px');
                        //$('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHConte");
                        //$('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHContScroll");
                    }

                    //$('#grid1_ob_grid1HeaderContainer').css('width', '100px');
                }
                else {
                    $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHConte");
                    $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHContScroll");
                    $('#grid1_ob_grid1HeaderContainer').css("cssText", "width: 100% !important;");
                }
            });

            $('#dgConsulta2').on('click', function () {
                var $pRow = $(this).parents('tr');
                var $nextRow = $pRow.next('tr');
                $nextRow.toggle();
                $(this).toggleClass('icon-s icon-e');
            });

            $("#dgConsulta2").on("click", function () {
                var obj = $(this);
                if (obj.hasClass("GVTb")) {
                    obj.hide();
                    obj.next().show();
                    obj.parent().parent().next().show();
                } else {
                    obj.hide();
                    obj.prev().show();
                    obj.parent().parent().next().hide();
                }
            });
        });



        function fixedGrid() {
            var p = $('#grid1_ob_grid1HeaderContainer').position()
            var scrollY = 0;
            var scrollX = 0;
            if ($(window).scrollLeft() >= 1) {
                //$('#grid1_ob_grid1HeaderContainer').addClass("ob_gHConte");
                scrollY = p.top;
            }
            else {
                $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHConte");
                scrollX = p.left;
            }

        }
        var printStyleSheetMoved = false;
        var gridBodyStyle = null;
        var refreshinterval = 20;
        var starttime;
        var nowtime;
        var reloadseconds = 0;
        var secondssinceloaded = 0;
        var btnActualizar;
        var cambio = 1;
        var posY = $(window).scrollTop();
        var iniciar = 0;

        $(document).on("ready", inicio);

        function inicio() {
            //window.onscroll = fixedGrid;
            var url = document.location.href.toString();
            var n = url.indexOf("tim");
            //$('.ob_gCc1').draggable(); //draggable para mobiles

            if (n != -1) {
                $("#botonesDiv").hide();
                $("#botonesDiv").css("visibility", "hidden");
                pageScroll();
            }
        }

        $(function () {
            //mirar documentacion JQueryUI sobre datepicker
            var fechaVal = $(".calendario").val();

            $(".calendario").datepicker();
            $(".calendario").datepicker("option", "dateFormat", "yy-mm-dd");
            $(".calendario").datepicker("option", "showAnim", "slideDown");
            $(".calendario").val(fechaVal);
        });

        function pageScroll() {
            var tiempo = 40;
            if (iniciar == 2) {
                $(window).scrollTop(0);
                iniciar = 0;
            }

            if ($(window).scrollTop() == 0) {
                tiempo = 2000;
            }
            this.window.scrollBy(0, 1); // horizontal and vertical scroll increments x,y

            if (posY == $(window).scrollTop()) {
                iniciar = 1;
                tiempo = 2000;
            }
            else {
                posY = $(window).scrollTop();
            }


            if (iniciar == 0) {
                scrolldelay = setTimeout('pageScroll()', tiempo);
            }
            else {
                scrolldelay = setTimeout('pageScroll()', tiempo);
                iniciar = 2;
            }
        }

        function printGrid() {

            if (!printStyleSheetMoved) {
                grid1.GridMainContainer.appendChild(document.getElementById('PrintStyleSheetContainer'));
            }

            if (grid1.PageSelectedRecords.length == 0) {
                alert('Seleccione al menos una fila para imprimir.');
                return;
            }

            grid1.print();
            window.setTimeout(restoreGridColumn, 250);
        }

        function restoreGridColumn() {
            alert("Imprimiendo...");
        }

        function printGrid2() {

            gridBodyStyle = grid1.GridBodyContainer.getAttribute('style');
            grid1.GridBodyContainer.style.maxHeight = '';

            grid1.print();

            window.setTimeout("grid1.GridBodyContainer.setAttribute('style', gridBodyStyle);", 250);

            return false;
        }

        function ocultarColumnaChk(obj, index) {
            var hdChecks = document.getElementById("hdCheckBoxes");
            var resChecks = hdChecks.value.split(",");

            if (obj.checked == true) {
                grid1.showColumn(index);
                resChecks[index] = "true";
            } else {
                grid1.hideColumn(index);
                resChecks[index] = "false";
            }

            hdChecks.value = resChecks;
        }

        function show_hide_Column(index) {
            var oChk = document.getElementById("chkCol" + index);
            if (oChk.checked == true) {
                grid1.showColumn(index);
            } else {
                grid1.hideColumn(index);
            }
        }

        function exportToExcel() {
            grid1.exportToExcel();
        }

        function exportToWord() {
            grid1.exportToWord();
            return false;
        }

        function cargaBoton(icono) {

            if (icono == 'carga') {
                if (document.getElementById('imag1').style.visibility == 'visible'
                    || document.getElementById('imag1').style.visibility == '') {
                    document.getElementById('imag1').style.visibility = 'hidden';
                    document.getElementById('imag3').style.visibility = 'visible';
                    document.getElementById('impr1').style.visibility = 'hidden';
                    document.getElementById('impr2').style.visibility = 'visible';
                    document.getElementById('excl1').style.visibility = 'hidden';
                    document.getElementById('excl2').style.visibility = 'visible';
                    document.getElementById('gra1').style.visibility = 'hidden';
                    document.getElementById('gra2').style.visibility = 'visible';
                    document.getElementById('mail1').style.visibility = 'hidden';
                    document.getElementById('mail2').style.visibility = 'visible';
                    document.getElementById('plano1').style.visibility = 'hidden';
                    document.getElementById('plano2').style.visibility = 'visible';
                    document.getElementById('word1').style.visibility = 'hidden';
                    document.getElementById('word2').style.visibility = 'visible';
                    document.getElementById('pdf1').style.visibility = 'hidden';
                    document.getElementById('pdf2').style.visibility = 'visible';
                }
            }
            else if (icono == 'grafica') {
                document.getElementById('gra1').style.visibility = 'hidden';
                document.getElementById('gra3').style.visibility = 'visible';
                document.getElementById('imag1').style.visibility = 'hidden';
                document.getElementById('imag2').style.visibility = 'visible';
                document.getElementById('impr1').style.visibility = 'hidden';
                document.getElementById('impr2').style.visibility = 'visible';
                document.getElementById('excl1').style.visibility = 'hidden';
                document.getElementById('excl2').style.visibility = 'visible';
                document.getElementById('mail1').style.visibility = 'hidden';
                document.getElementById('mail2').style.visibility = 'visible';
                document.getElementById('plano1').style.visibility = 'hidden';
                document.getElementById('plano2').style.visibility = 'visible';
                document.getElementById('word1').style.visibility = 'hidden';
                document.getElementById('word2').style.visibility = 'visible';
                document.getElementById('pdf1').style.visibility = 'hidden';
                document.getElementById('pdf2').style.visibility = 'visible';
            }
            else if (icono == 'excel') {
                alert("Su archivo se descargará acontinuación. \n Encontrará el estado de la descarga en la parte inferior izquierda de su ventana. \n" +
                                        "Una vez termine la descarga podrá abrirlo. Gracias!");
                document.getElementById('excl1').style.visibility = 'hidden';
                document.getElementById('excl3').style.visibility = 'visible';
                document.getElementById('gra1').style.visibility = 'hidden';
                document.getElementById('gra2').style.visibility = 'visible';
                document.getElementById('imag1').style.visibility = 'hidden';
                document.getElementById('imag2').style.visibility = 'visible';
                document.getElementById('impr1').style.visibility = 'hidden';
                document.getElementById('impr2').style.visibility = 'visible';
                document.getElementById('mail1').style.visibility = 'hidden';
                document.getElementById('mail2').style.visibility = 'visible';
                document.getElementById('plano1').style.visibility = 'hidden';
                document.getElementById('plano2').style.visibility = 'visible';
                document.getElementById('word1').style.visibility = 'hidden';
                document.getElementById('word2').style.visibility = 'visible';
                document.getElementById('pdf1').style.visibility = 'hidden';
                document.getElementById('pdf2').style.visibility = 'visible';
                setTimeout(excelImg, 3500);
            }

            return true;
        }

        function excelImg() {
            document.getElementById('excl1').style.visibility = 'visible';
            document.getElementById('excl3').style.visibility = 'hidden';
            document.getElementById('gra1').style.visibility = 'visible';
            document.getElementById('gra2').style.visibility = 'hidden';
            document.getElementById('imag1').style.visibility = 'visible';
            document.getElementById('imag2').style.visibility = 'hidden';
            document.getElementById('impr1').style.visibility = 'visible';
            document.getElementById('impr2').style.visibility = 'hidden';
            document.getElementById('mail1').style.visibility = 'visible';
            document.getElementById('mail2').style.visibility = 'hidden';
            document.getElementById('plano1').style.visibility = 'visible';
            document.getElementById('plano2').style.visibility = 'hidden';
            document.getElementById('word1').style.visibility = 'hidden';
            document.getElementById('word2').style.visibility = 'visible';
            document.getElementById('pdf1').style.visibility = 'hidden';
            document.getElementById('pdf2').style.visibility = 'visible';
        }
        function clickOnce(btn) {
            // Comprobamos si se está haciendo una validación
            if (typeof (Page_ClientValidate) == 'function') {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }

            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'image') {
                btn.src = urlImagen;
                document.getElementById('ImprimirTod').src = '../img/AMS.Icon.PrinterShdw.png';
                document.getElementById('BtnExcel').src = '../img/AMS.Icon.ExcelShdw.png';
                document.getElementById('BtnGraficar').src = '../img/AMS.Icon.GraphShdw.png';
                document.getElementById('BtnCorreo').src = '../img/AMS.Icon.CorreoShdw.png';
                document.getElementById('BtnPlano').src = '../img/AMS.Icon.plane_iconShdw.png';
                document.getElementById('BtnWord').src = '../img/AMS.Icon.word_iconShdw.png';
                document.getElementById('BtnPdf').src = '../img/AMS.Icon.pdf_iconShdw.png';
            }

            return true;
        }

        function animarBoton(btnV, urlImagen) {
            btnV.src = urlImagen;
            return true;
        }

        function unSoloCheck(chk, filas) {
            for (var i = 0; i < filas - 1; i++) {
                var ch = document.getElementById('myWindow_chkEnc_' + i);
                if (ch.id != chk.id) {
                    ch.checked = false;
                }
            }
            return true;
        }
      
    </script>

    <style>
        .box {
            position: fixed;
            width: 100%;
            height: 100px;
            left: 0px;
            top: 0px;
            background: white;
            z-index: 998;
        }

        .ob_gCS, .ob_gCS div, .ob_gCS_F, .ob_gCS_F div {
            background-color: #FFFFFF !important;
        }

        a.ob_gALF {
            color: #44bf42 !important;
        }

        #grid1_ob_grid1HeaderContainer {
            /*width: 100% !important;*/
            z-index: 999;
        }
    </style>

    <style type="text/css">
        .ChildGrid {
            text-align: left;
        }

            .ChildGrid td {
                background-color: #eee !important;
                color: black;
            }

            .ChildGrid th {
                background-color: #6C6C6C !important;
                color: White;
            }
    </style>
</head>

<body>
    <br>
    <br>
    <br>
    <br>
    <br>

    <div class="box">
        <asp:placeholder id="plcEncabezado" runat="server" />
    </div>
    <div id="dialog" title="Basic dialog" style="visibility: hidden; width: 40%; z-index: 999;"></div>


    <form id="form1" runat="server">
        <br>
        <table class="tablaBasica">
            <tr>
                <td>
                    <asp:placeholder id="plcFiltros" runat="server" visible="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="botonesDiv">
                        <asp:placeholder id="plcBotones" runat="server" visible="true">
            <table class="tableRoundBorder" width="200"  align="left" >
                <tr>
                    <td align="center" valign="bottom" height="40">
                        <div style="position:relative; top:-30px;">
                            <div id="imag1" class="iniciar1" onClick="cargaBoton('carga');">
		                        <asp:ImageButton id="BtnGenerar" OnClick="GenerarReporte"  UseSubmitBehavior="false" runat="server" ImageUrl="../img/AMS.Icon.GenF.png" BorderWidth="0px"></asp:ImageButton>
	                        </div>
                            <div id="imag2" class="iniciar2" >
		                        <img src="../img/AMS.Icon.GenFShdw.png" >
	                        </div>
	                        <div id="imag3" class="iniciar2" >
		                        <img src="../img/flecha.gif" >
	                        </div>
                        </div>
	                    <font size="1" face="Georgia, Arial">Generar</font> 
                    </td>
                    <td align="center" valign="bottom">
                        <div style="position:relative; top:-30px; left:7px;">
                            <div id="impr1" class="iniciar1" >
		                        <asp:ImageButton id="ImprimirTod" OnClientClick="return printGrid2();" runat="server" alt="Imprimir Documento" ImageUrl="../img/AMS.Icon.PrinterF.png" BorderWidth="0px" ></asp:ImageButton>
	                        </div>
	                        <div id="impr2" class="iniciar2">
		                        <img src="../img/AMS.Icon.PrinterShdw.png" >
	                        </div>
                        </div>
                        <font size="1" face="Georgia, Arial">Imprimir</font>
                    </td>
                    <td align="center" valign="bottom">
                        <div style="position:relative; top:-30px;">
                            <div id="excl1" class="iniciar1" onClick="cargaBoton('excel')">
		                        <asp:ImageButton id="BtnExcel" onClick="Excel" runat="server" alt="Exportar a Excel" ImageUrl="../img/AMS.Icon.ExcelF.png" BorderWidth="0px"></asp:ImageButton>
	                        </div>
	                        <div id="excl2" class="iniciar2">
		                        <img src="../img/AMS.Icon.ExcelShdw.png" >
	                        </div>
                            <div id="excl3" class="iniciar2">
		                        <img src="../img/excelanim.gif" >
	                        </div>
                        </div>
                        <font size="1" face="Georgia, Arial">Excel</font>
                    </td>
                    <td align="center" valign="bottom">
                        <div style="position:relative; top:-30px; left:5px;">
                            <div id="gra1" class="iniciar1" onClick="cargaBoton('grafica')";>
		                        <asp:ImageButton id="BtnGraficar" onClick="Graficar" runat="server" alt="Graficar" ImageUrl="../img/AMS.Icon.GraphF.png" BorderWidth="0px"></asp:ImageButton>
	                        </div>
	                        <div id="gra2" class="iniciar2">
		                        <img src="../img/AMS.Icon.GraphShdw.png" >
	                        </div>
                            <div id="gra3" class="iniciar2">
		                        <img src="../img/grafica.gif" >
	                        </div>
                        </div>
                        <font size="1" face="Georgia, Arial">Graficar</font>
                    </td>
                    <td align="center" valign="bottom">
                        <div style="position:relative; top:-25px;">
                            <div id="mail1" class="iniciar1" >
		                        <asp:ImageButton id="BtnCorreo" onClick="EnviarCorreo" runat="server" alt="Enviar Correo" ImageUrl="../img/AMS.Icon.Correo.png" BorderWidth="0px"></asp:ImageButton>
	                        </div>
	                        <div id="mail2" class="iniciar2">
		                        <img src="../img/AMS.Icon.CorreoShdw.png" >
	                        </div>
                        </div>
                        <font size="1" face="Georgia, Arial">Correo</font>
                    </td>
                    <td align="center" valign="bottom" style="width: 45px;">
                        <div style="position:relative; top:-32px;">
                            <div id="plano1" class="iniciar1" >
		                        <asp:ImageButton id="BtnPlano" onClick="GenerarPlanoTXT" runat="server" alt="Generar Plano" ImageUrl="../img/AMS.Icon.plane_icon.png" BorderWidth="0px" style="width: 35px; margin-left: 4px;"></asp:ImageButton>
	                        </div>
	                        <div id="plano2" class="iniciar2">
		                        <img src="../img/AMS.Icon.plane_iconShdw.png" style="width: 35px; margin-left: 4px;">
	                        </div>
                        </div>
                        <font size="1" face="Georgia, Arial">Plano</font>
                    </td>
                    <td align="center" valign="bottom" style="width: 35px;">
                        <div style="position:relative; top:-32px; left: -5px;">
                            <div id="word1" class="iniciar1" >
		                        <asp:ImageButton id="BtnWord" OnClientClick="return exportToWord();" runat="server" alt="Generar Documento" ImageUrl="../img/AMS.Icon.word_icon.png" BorderWidth="0px" style="width: 26px; margin-left: 10px;"></asp:ImageButton>
	                        </div>
	                        <div id="word2" class="iniciar2">
		                        <img src="../img/AMS.Icon.word_iconShdw.png" style="width: 26px; margin-left: 10px;">
	                        </div>
                        </div>
                        <font size="1" face="Georgia, Arial">Word</font>
                    </td>
                    <td align="center" valign="bottom" style="width: 35px;">
                        <div style="position:relative; top:-32px; left: -5px;">
                            <div id="pdf1" class="iniciar1" >
		                        <asp:ImageButton id="BtnPDF" onClick="GenerarPDF" runat="server" alt="Generar PDF" ImageUrl="../img/AMS.Icon.pdf_icon.png" BorderWidth="0px" style="width: 26px; margin-left: 10px;"></asp:ImageButton>
	                        </div>
	                        <div id="pdf2" class="iniciar2">
		                        <img src="../img/AMS.Icon.pdf_iconShdw.png" style="width: 26px; margin-left: 10px;">
	                        </div>
                        </div>
                        <font size="1" face="Georgia, Arial">PDF</font>
                    </td>
                    
                </tr>
            </table>
                <asp:Button ID="ImprimirSel" Text="Imprimir Seleccion" runat="server" OnClientClick="printGrid(); return false;" Visible="false"/>
        </asp:placeholder>
                        <asp:button id="btnRegresar" runat="server" text="Regresar" onclick="btnRegresar_Click" visible="false"
                            style="background-image: linear-gradient(to bottom,#337ab7 0,#265a88 100%); background-repeat: repeat-x; border-color: #245580; color: #fff; background-color: #337ab7; padding: 6px 12px; font-size: 14px; border: 1px solid transparent; border-radius: 4px; display: inline-block; cursor: pointer;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:hyperlink id="hDescarga" runat="server" visible="false">Descargar Archivo Excel</asp:hyperlink>
                    <asp:hyperlink id="hlDescargaPlano" runat="server" visible="false">Descargar Archivo Plano</asp:hyperlink>
                </td>
            </tr>
        </table>
        <br>
        <asp:placeholder id="phForm" runat="server"></asp:placeholder>
        <center><asp:Label id="lblTitulo" runat="server" Visible="true"></asp:Label></center>
        <br>
        <asp:placeholder id="plcReport" runat="server" visible="false">
	<p>
		<table class="reports" width="780" bgColor="gray">
			<tbody>
				<tr>
					<td><asp:table id="tabHeader" BorderWidth="0px" Width="100%" CellSpacing="0" CellPadding="1" BackColor="White"
							GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"></asp:table></td>
				</tr>
				<tr>
					<td><asp:datagrid id="report" runat="server" BorderWidth="2px" CellSpacing="1" CellPadding="3" BackColor="White"
							GridLines="None" BorderColor="White" BorderStyle="Ridge" width="100%" EnableViewState="True"
							AutoGenerateColumns="False">
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#9471DE"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F0F0F0"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#DEDFDE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#E7E7FF" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="Black" BackColor="#C6C3C6"></FooterStyle>
							<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6"></PagerStyle>
						</asp:datagrid></td>
					<td><asp:datagrid id="reportA" runat="server" BorderWidth="2px" CellSpacing="1" CellPadding="3" BackColor="White"
							GridLines="None" BorderColor="White" BorderStyle="Ridge" width="100%" EnableViewState="False"
							AutoGenerateColumns="False">
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#9471DE"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F0F0F0"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#DEDFDE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#E7E7FF" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="Black" BackColor="#C6C3C6"></FooterStyle>
							<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6"></PagerStyle>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td><asp:table id="tabFooter" BorderWidth="0px" Width="100%" CellSpacing="0" CellPadding="1" BackColor="White"
							GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"></asp:table></td>
				</tr>
			</tbody>
		</table>
	</p>
</asp:placeholder>
        <asp:placeholder id="plcVisibilityPanel" runat="server"></asp:placeholder>
        <br />
        <asp:placeholder id="plcReporte" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin: 6px;" align="auto">
                <div align="left" style="width: 353px;">
             <DT:GridViewTree ID="dgConsulta2" runat="server" AutoGenerateColumns="True"
                              AllowSorting="True" ForeColor="#333333" GridLines="Both" IsParentDataColumn="IsFolder"
                              ImageUrlClosedParent="~/img/TreeView/closeFolder.png" ImageUrlOpenedParent="~/img/TreeView/openFolder.png"                               
                              ShowExpandCollapse="True">
                <FooterStyle BackColor="#C6C3C6" ForeColor="Black" Font-Bold="True" />
                <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#76cbf3" Height="20px" ForeColor="Black" />
                <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#5D7B9D" />
                <AlternatingRowStyle BackColor="#acddf5" ForeColor="black" />
                </DT:GridViewTree>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        <br />
        <%-- <asp:PlaceHolder id="plcTotales" runat="server">
            <asp:Label ID="lbTotal" runat="server">TOTAL: </asp:Label>
            <asp:TextBox ID="txtTotales" runat="server"></asp:TextBox>
        </asp:PlaceHolder>--%>
        <asp:placeholder id="plcGrafica" runat="server" visible="true" />

        <owd:window id="myWindow" status="Statusbar" runat="server"
            left="200" top="100" height="240" width="320" title="Obout Window">
        </owd:window>

        <owd:window id="vntGrafica" status="Statusbar" runat="server"
            left="200" top="100" height="240" width="320" title="Obout Window">
        </owd:window>

        <owd:window id="vntCorreo" runat="server" left="200" top="80" height="530"
            width="500" title="Enviar correo electronico" isdraggable="true" ismodal="false" status="Enviar Correo..."
            stylefolder="../grid/wdstyles/geryon" pageopacity="0" visibleonload="false"
            showclosebutton="false">
            <center>
            <table class="tableRoundBorder" width="400">
                <tr>
                    <td align="left" width="100">
                        Para:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtPara" runat="server" Width="290"/>
                    </td>
                </tr>
            </table>
            <br>

            <table class="tableRoundBorder" width="400">
                <tr>
                    <td align="left" width="100">
                        Asunto:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtAsunto" runat="server" Width="290"/>
                    </td>
                </tr>
            </table>
            <br>

            <table class="tableRoundBorder" width="400">
                <tr>
                    <td>
                        <asp:TextBox ID="txtArea" runat="server" Height="280" Width="385" textmode="multiline"/>
                    </td>
                </tr>
            </table>
            <br>

            <table class="tableRoundBorder" width="200">
                <tr>
                    <td align="center" valign="bottom">
                        <asp:ImageButton id="btnEnviarCorreo" onClick="EnviarCorreo2" runat="server" ImageUrl="../img/AMS.Icon.CorreoF.png" BorderWidth="0px"></asp:ImageButton>
                        <br><font size="1" face="Georgia, Arial">Enviar</font>
                    </td>
                    <td align="center" valign="bottom">
                        <asp:ImageButton id="bntCerrarCorreo" OnClientClick="vntCorreo.Close();" runat="server" ImageUrl="../img/AMS.Icon.Close.png" BorderWidth="0px"></asp:ImageButton>
                        <br><font size="1" face="Georgia, Arial">Cerrar</font>
                    </td>
                </tr>
            </table>
            </center>
            <br>
        </owd:window>
        <asp:hiddenfield id="hdCheckBoxes" runat="server" value="" />

    </form>

    <div id="PrintStyleSheetContainer">
        <style type="text/css" media="print">
            .ob_gR, .ob_gRA, .ob_gHSI, .ob_gFCont {
                display: none;
            }
        </style>
    </div>

</body>
</html>

