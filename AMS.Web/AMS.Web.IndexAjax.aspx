   <%@ page Language="c#" Debug="true" AutoEventWireup="True" CodeBehind="AMS.Web.IndexAjax.aspx.cs" 
   Inherits="AMS.Web.IndexAjax" EnableEventValidation="false" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>


<!DOCTYPE html>
<html lang="es">
  <head runat="server">
    <meta charset="utf-8" />
    <title>AMS - Sistemas eCAS Ltda</title>
    <meta name="viewport" content="width=device-width", initial-scale=1, maximum-scale=1.5/>
    <link href="../img/AMS.ico" type="image/ico" rel="icon" />

    <link href="../css/normalize.css" type="text/css" rel="stylesheet" />
    <link href="../css/AMS.css" type="text/css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" type="text/css" rel="stylesheet"  />
   
    <script type="text/javascript" src="../js/modernizr.js"></script>
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>
    <script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
    <script type="text/javascript" src="../js/generales.js"></script>
    <script type="text/javascript" src="../js/prefixfree.min.js"></script>
  <script language='javascript' src='../ui/jquery.ui.core.js' type='text/javascript'></script>
    <script language='javascript' src='../ui/jquery.ui.widget.js' type='text/javascript'></script>
    <script language='javascript' src='../ui/jquery.ui.datepicker.js' type='text/javascript'></script>

    <link rel="stylesheet" href="../css/fecha.css"/>
    <link rel="stylesheet" href="../css/jquery.ui.all.css"/>
    <link rel="stylesheet" href="../css/jquery.ui.theme.css"/>

    <script language='javascript'>
        $(function () {
            $(".datepicker").datepicker({
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
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
</head>
<body>
    <form id="Form" runat="server">
    <div id="dialog" title="Basic dialog" style="visibility:hidden"></div>
    <table>
        <tbody>
            <tr>
                <td>
                    <h2>
                        <font color="#2a2a2a">&nbsp;
                            <input type="image" src="../img/marcos.jpg" onclick="cambiar(this);" value="Quitar" style="width: 16px; cursor: pointer">&nbsp;&nbsp;
                            <asp:Label ID="lbSystemName" runat="server" Font-Names="Tahoma" Font-Size="16pt" ForeColor="#424242">
                                AMS - 
                            </asp:Label>
                            <asp:Label ID="lbCompanyName" runat="server" Font-Names="Tahoma" Font-Size="16pt" ForeColor="#424242">
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
                <td colspan="2">
                    <asp:Label ID="infoProcess" runat="server" Font-Names="Arial" Font-Size="12px" ForeColor="RoyalBlue"
                        Font-Bold="True" BackColor="#F2F2F2" CssClass="infoProcess" Width="100%"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="mainIframe">
        <div id="contImg">
            <img id="imagenCarga" src="../img/Send3.gif" alt="Cargando..." /><br />
            <label>
                <font size="4"><i>e</i></font>cas Procesando...
            </label>
        </div>
        <p>
            <asp:PlaceHolder ID="gridHolder" runat="server"></asp:PlaceHolder>
        </p>
    </div>
    <br>
    <asp:Label ID="lb" runat="server"></asp:Label><input type="hidden" id="HDNINPT_MODALSELECT"
        name="HDNINPT_MODALSELECT" value="">
    </form>
</body>
</html>
