<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Reportes.FrogReports.aspx.cs" Inherits="AMS.Reportes.FrogReports" %>
<%@ Register TagPrefix="chart" Namespace="System.Web.UI.DataVisualization.Charting" Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="owd" Namespace="OboutInc.Window" Assembly="obout_Window_NET"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>
<asp:ScriptManager ID="ScriptManager" runat="server" />
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<title>AMS</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <LINK href="../img/AMS.ico" type="image/ico" rel="icon">
        <link href="../css/AMS.css" type="text/css" rel="stylesheet" />
        <LINK href="../css/AMS.css" type="text/css" rel="stylesheet">
  
        <link rel="stylesheet" href="../css/fecha.css"/>

        <script type="text/javascript" src="../js/jquery.js"></script>
        <script type="text/javascript" src="../js/jquery-ui.js"></script>
        <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
        <script type="text/javascript" src="../js/ui/jquery.ui.core.js" ></script>
        <script type="text/javascript" src="../js/ui/jquery.ui.widget.js" ></script>
        <script type="text/javascript" src="../js/ui/jquery.ui.datepicker.js" ></script>
        <link href="../css/jquery-ui.css" type="text/css" rel="stylesheet"  />
        <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
        <script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
        <script type ="text/javascript" src="../js/generales.js"></script>
        <script type="text/javascript" src="../js/modernizr.js"></script>

        <link rel="stylesheet" href="../css/jquery.ui.all.css"/>
        <link rel="stylesheet" href="../css/jquery.ui.theme.css"/>    

        <script type="text/javascript">
            $(document).ready(function () {
                $(window).scroll(function () {
                    var top = -99;
                    var posLabel = $('#lblTitulo').position();
                    var p = $('#grid1_ob_grid1MainContainer').position();
                    top += p.top;
                    if ($(window).scrollTop() >= posLabel.top) {
                        $('#grid1_ob_grid1HeaderContainer').addClass("ob_gHConte");
                        if ($(window).scrollLeft() > 0) {
                            $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHConte");
                            $('#grid1_ob_grid1HeaderContainer').addClass("ob_gHContScroll");

                            $('#grid1_ob_grid1HeaderContainer').css('top', $(this).scrollTop() - top);
                        }
                        else {
                            $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHContScroll");
                            $('#grid1_ob_grid1HeaderContainer').css('top', '100px');
                        }
                    }
                    else {
                        $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHConte");
                        $('#grid1_ob_grid1HeaderContainer').removeClass("ob_gHContScroll");
                    }
                });
            });
//            function saludoPrueba() {
//                var p = $('#grid1_ob_grid1BodyContainer').position();
//                alert('prueba 1' + p.left);
//            }
            //             window.onload = saludoPrueba;
            


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
			}

			function cargaBoton(icono) {

			    if (icono == 'carga') 
			    {
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
			        }
			    }
			    else if (icono == 'grafica') 
			    {
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
			    }
			    else if (icono == 'excel') 
			    {
			        alert( "Su archivo se descargará acontinuación. \n Encontrará el estado de la descarga en la parte inferior izquierda de su ventana. \n" +
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
                    setTimeout(excelImg, 5000);
                }

			    return true;
			}
			
			function excelImg()
			{
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
			    }
                
                return true;
            }

            function animarBoton(btnV, urlImagen) {
                btnV.src = urlImagen;
                return true;
            }

            function unSoloCheck(chk, filas) {
                for (var i=0;i<filas-1;i++) {
                    var ch = document.getElementById('myWindow_chkEnc_' + i);
                    if ( ch.id != chk.id)
                    {
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
					background:white;
					z-index: 1;
				}
			.ob_gCS, .ob_gCS div, .ob_gCS_F, .ob_gCS_F div
				{
					  background-color: #FFFFFF !important;
				}
		</style>
</head>

<body>
	<br><br><br><br><br>
    
    <div class="box"><asp:placeholder id="plcEncabezado" runat="server" /></div>
    <div id="dialog" title="Basic dialog" style="visibility:hidden;  width:40%; z-index:999;"></div>
     

	<form id="form1" runat="server">
        
        <br>
        <table class="tablaBasica">
            <tr>
                <td><asp:placeholder id="plcFiltros" runat="server" visible="true" /></td>
            </tr>
            <tr><td>
        <div id="botonesDiv">
        <asp:placeholder id="plcBotones" runat="server" visible="true" >
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
                    
                </tr>
            </table>
            
		    <asp:Button ID="ImprimirSel" Text="Imprimir Seleccion" runat="server" OnClientClick="printGrid(); return false;" Visible="false"/>
        </asp:placeholder>
        </div>
        </td>
        </tr>
        <tr>
            <td><asp:HyperLink id="hDescarga" runat="server"  Visible="false">Descargar Archivo Excel</asp:HyperLink>
                <asp:HyperLink id="hlDescargaPlano" runat="server"  Visible="false">Descargar Archivo Plano</asp:HyperLink></td>
        </tr>
        </table>
        <br>
        <center><asp:Label id="lblTitulo" runat="server" Visible="true"></asp:Label></center>
        <br>
        <asp:placeholder id="plcReporte" runat="server" />
        <br>
       <%-- <asp:PlaceHolder id="plcTotales" runat="server">
            <asp:Label ID="lbTotal" runat="server">TOTAL: </asp:Label>
            <asp:TextBox ID="txtTotales" runat="server"></asp:TextBox>
        </asp:PlaceHolder>--%>
        <asp:PlaceHolder ID="plcGrafica" runat="server" Visible="true" />
        
        <owd:Window ID="myWindow" Status="Statusbar" runat="server" 
                        Left="200" Top="100" Height="240"  Width="320"  Title="Obout Window">
        </owd:Window> 

        <owd:Window ID="vntGrafica" Status="Statusbar" runat="server" 
                        Left="200" Top="100" Height="240"  Width="320"  Title="Obout Window">
        </owd:Window> 

        <owd:Window ID="vntCorreo" runat="server" Left="200" Top="80" Height="530"  
        Width="500"  Title="Enviar correo electronico" IsDraggable="true" IsModal = "false" Status = "Enviar Correo..."
        StyleFolder = "../grid/wdstyles/geryon" PageOpacity = "0" VisibleOnLoad = "false"
        ShowCloseButton = "false">
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
        </owd:Window> 
	</form>

	<div id="PrintStyleSheetContainer">
		<style type="text/css" media="print">
			.ob_gR, .ob_gRA, .ob_gHSI, .ob_gFCont
			{
				display: none;
			}
		</style>
	</div>
</body>
</html>

