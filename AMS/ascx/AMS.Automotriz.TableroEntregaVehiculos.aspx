<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.TableroEntregaVehiculos.aspx.cs" Inherits="AMS.Automotriz.TableroEntregaVehiculos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        function Cargar_Nombre()
        {
            TableroEntregaVehiculos.Hola("123", Cargar_Nombre_CallBack);
        }

        function Cargar_Nombre_CallBack(response) {
            alert(response.value);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="dTablero">
            <img src="../img/Ship.png" style="position: absolute; padding:25px;"><center><h1>TABLERO ENTREGA VEHÍCULOS</h1></center><br /><br /><br /><br /><br /><br />
            <table class="theader">
                <tr>
                    <td style="text-align:left;">
                        <asp:Label runat="server">Sistemas eCas</asp:Label>
                    </td>
                    <td>
                        <asp:Label id="lbPagina" runat="server" style="text-align:center;"> </asp:Label>
                    </td>
                    <td style="text-align:right;">
                        <asp:Label ID="lbFecha" runat="server">2017-05-04</asp:Label>
                    </td>
                </tr>
            </table>
            <div id="divDescriptor" runat="server" ></div>
            <div id="divContenido" runat="server" ></div>
        </div>
        <asp:Button id="btnCambiarPagina" runat="server" Text="Cambiemos página!" OnClick="cambiarPagina" OnClientClick="Cargar_Nombre();"/>
    </form>
</body>
</html>
