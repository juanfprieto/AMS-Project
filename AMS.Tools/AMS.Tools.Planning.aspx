<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.Planning.aspx.cs" Inherits="AMS.Tools.Planning" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <link href="../css/cssPlanning.css" type="text/css" rel="stylesheet">
    <script src="../js/jquery.js" type="text/javascript"></script>

    <script>
        var t;
        var speed = 2;
        var moving = true;

        $(document).ready(function () {
            console.log("Inicia Planning!");
            setTimeout(function () {
                if(moving == true)
                    scrollPlanning();
            }, 5000);
        });
        
        function cambiarMovimiento()
        {
            if (moving == true)
            {
                clearTimeout(t);
                $("#btnStop").html('Continuar');
                moving = false;
            }
            else
            {
                $("#btnStop").html('Pausar');
                moving = true;
                scrollPlanning();
            }
        }

        function scrollPlanning()
        {
            t = setTimeout(function () {
                var leftActual = $("#divContenido").position().left - speed;
                $("#divContenido").css({ left: leftActual });

                if (($("#divContenido").width() * -1) <= $("#divContenido").position().left)
                {
                    scrollPlanning();
                }
                else
                {
                    $('#btnCambiarPagina').trigger('click');
                }
            }, 40);
        }

        function tam()
        {
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <div id="divTablero">
            <div id="divEncabezado" runat="server" >
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 32%;" rowspan="2"><asp:Image id="imgLeft" runat="server" width="148"/></td>
                        <td style="width: 25%;"><asp:Image id="imgBanner" runat="server" style="width:100%;"/></td>
                        <td style="width: 40%; text-align:right;"><asp:Image id="imgRight" runat="server" width="160"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label id="lbPagina" runat="server"></asp:Label></td>
                        <td><asp:Button id="btnRetrocederPagina" runat="server" Text="Regresar" onClick="RetrocederPagina"/>
                            <button id="btnStop" type="button" onclick="cambiarMovimiento();">Pausar</button>
                            <asp:Button id="btnCambiarPagina" runat="server" Text="Avanzar" onClick="CambiarPagina"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><div id="divEstados" runat="server" ></div></td>
                        <td><div id="divLineaTiempo" runat="server" ></div></td>
                    </tr>
                </table>
            </div>
            <div id="divDescriptor" runat="server" ></div>
            <div id="divContenido" runat="server" ></div>
        </div>
        
    </form>
</body>
</html>
