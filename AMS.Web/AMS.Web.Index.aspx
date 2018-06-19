<%@ Page Language="c#" Debug="true" AutoEventWireup="True" CodeBehind="AMS.Web.Index.aspx.cs"
    Inherits="AMS.Web.Index" EnableEventValidation="false" ValidateRequest="false"%>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>AMS - Sistemas eCAS SAS</title>
     <meta name="viewport" content="width=device-width", initial-scale=1, maximum-scale=1.5/>
    <link href="../img/AMS.ico" type="image/ico" rel="icon" />
    <link href="../css/lightbox.css" type="text/css" rel="stylesheet">
    <link href="../css/normalize.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="../css/bootstrap.min.css"/>
    <link rel="stylesheet" href="../css/bootstrap-theme.min.css"/>
    <link href="../css/AMS.css" type="text/css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" type="text/css" rel="stylesheet"  />
    <link rel="stylesheet" href="../css/jquery.ui.theme.css">
    <link href="../css/tab.webfx.css" type="text/css" rel="StyleSheet">
    <script type="text/javascript" src="../js/modernizr.js"></script>
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>
    <script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
    <script type="text/javascript" src="../js/generales.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.core.js" ></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.widget.js" ></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.datepicker.js" ></script>
    <script type="text/javascript" src="../js/angular.min.js"></script>
    <script type="text/javascript" src="../js/angularController.js"></script>
    <script type="text/javascript" src="../js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/npm.js"></script>
    <script type ="text/javascript" src="https://cdn.jsdelivr.net/jquery.validation/1.15.0/jquery.validate.min.js"></script>
    <script type ="text/javascript" src="https://cdn.jsdelivr.net/jquery.validation/1.15.0/additional-methods.min.js"></script>
    <link rel="stylesheet" href="../css/fecha.css"/>
    <link rel="stylesheet" href="../css/jquery.ui.all.css"/>
    <link rel="stylesheet" href="../css/jquery.ui.theme.css"/>
    <link rel="stylesheet" href="../css/AMS.GridView.css"/>



    <script language='javascript'>
        $(function () {
            $("#" + 'divContManual').draggable();
            
//        $(".datepicker").datepicker({
//            changeMonth: true,
//            changeYear: true
//        });
        $(".datepicker").datepicker({
            showOn: "button",
            buttonImage: "../img/AMS.Calendar.png",
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true
        });
        $(".datepicker").datepicker("option", "dateFormat", "yy-mm-dd");
        $(".datepicker").datepicker($.datepicker.regional["es"]);
    });
    jQuery(function ($) {
        $.datepicker.regional['es'] = {
            closeText: 'Done',
            prevText: "Atras", // Display text for previous month link
            nextText: "Siguiente", // Display text for next month link
            currentText: "Today", // Display text for current month link
            monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
		    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"], // Names of months for drop-down and formatting
            monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"], // For formatting
            dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"], // For formatting
            dayNamesShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"], // For formatting
            dayNamesMin: ["DO", "LU", "MA", "MI", "JU", "VI", "SA"], // Column headings for days starting at Sunday
            weekHeader: "Wk", // Column header for week of the year
            dateFormat: "mm-dd-yy", // See format options on parseDate
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['es']);
    });

    </script>

</head>
<body>
    <form id="Form" method="post" runat="server">
    <div id="dialog" title="Basic dialog" style="visibility:hidden"></div>
    <table>
        <tbody>
            <tr>
                <td>
                    <h2>
                        <font color="#2a2a2a">&nbsp;
                            <input type="image" src="../img/marcos.jpg" onclick="cambiar(this);" value="Quitar" style="width: 16px; cursor: pointer" />&nbsp;&nbsp;
                            <asp:Label ID="lbSystemName" runat="server" Font-Names="Tahoma" Font-Size="15pt" ForeColor="#424242">
                                AMS - 
                            </asp:Label>
                            <asp:Label ID="lbCompanyName" runat="server" Font-Names="Tahoma" Font-Size="15pt" ForeColor="#424242">
                                Sistemas eCAS
                            </asp:Label>
                        </font>
                    </h2>
                </td>
                <td align="right" valign="top">
                    <font color="#2a2a2a">&nbsp;
                        <asp:Label ID="lblUsuario" runat="server" Font-Names="Tahoma" Font-Size="10pt" ForeColor="#424242">Usuario</asp:Label>
                        <br />
                        <asp:Label ID="lblLinkManual" runat="server" />
                        <asp:Button id="btnManual" runat="server" Text="Ver Manual" OnClick="MotrarManual" Visible="false" style="padding: 1px;"></asp:Button>
                    </font>
                    <font size=1><a href="#" target=_top>movil</a>&nbsp;&nbsp; - &nbsp;&nbsp; <a href="javascript:establecer()">normal</a>
                    </font>
                </td>
            </tr>
            <tr>
                <td colspan="2" height="1px">
                    <asp:PlaceHolder ID="menuHolder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="breadcrumb">
                    <asp:Label ID="infoProcess" runat="server" Font-Names="Arial" Font-Size="12px" ForeColor="#337ab7"
                        Font-Bold="True" BackColor="#F2F2F2" CssClass="infoProcess" Width="100%"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="mainIframe">
        <div id="contImg">
            <img id="imagenCarga" src="../img/Send3.gif" alt="Cargando..." /><br /><br />
            <label>
                <b class="alert alert-info">Ecas Cloud Cargando...</b>
            </label>
        </div>
        <p>
            <div ng-app="amsApp">
                <asp:PlaceHolder ID="gridHolder" runat="server"></asp:PlaceHolder>
            </div>
        </p>
    </div>
    <br>
    <asp:Label ID="lb" runat="server"></asp:Label>
    
        <div id="divContManual" style="
        width: 698px;
    height: 680px;
    position: absolute;
    top: 90px;
    left:20%;
    overflow: hidden;" runat="server" visible="false">

        <div id="divManual"  runat="server"
            style="position: absolute;
        background-color: white;
        width: 670px;
        height: 650px;
        left: 1px;
        top: 1px;
        padding: 70px;
        overflow-y: scroll;
        box-shadow: 4px 10px 22px #888888;
        border-radius: 4px;
        border-style: solid;"></div>

        <asp:Button id="btnCerrarManual" runat="server" Text="Cerrar" OnClick="CerrarManual" Visible="false" 
            style="padding: 1px;
        position: absolute;
        top: 14px;
        right: 43px;"></asp:Button>
     </div>
    <input type="hidden" id="HDNINPT_MODALSELECT" name="HDNINPT_MODALSELECT" value="">
    <div>
        <center> <asp:Button runat="server" Text="Regresar" id="regresarPrincipalVendedores" visible="false" onclick="regresarControl"/> </center>
    </div>
    </form>
</body>
</html>
