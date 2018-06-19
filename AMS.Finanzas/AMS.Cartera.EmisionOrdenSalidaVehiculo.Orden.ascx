<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.EmisionOrdenSalidaVehiculo.Orden.ascx..cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.BusquedaOrden" %>
<table id="Table1" class="filtersIn">
    <tbody>
        <tr>
            <td>
                Escoja el prefijo de la Orden de Trabajo : 
            </td>
            <td>
                <asp:DropDownList id="ddlPrefijo" runat="server" onSelectedIndexChanged="ddlPrefijo_IndexChanged" AutoPostBack="True"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Escoja el número de la Orden de Trabajo : 
            </td>
            <td>
                <asp:DropDownList id="ddlNumero" runat="server"></asp:DropDownList>
            </td>
        </tr>
    </tbody>
</table>
<p>
    <asp:Button id="btnAceptar" onclick="btnAceptar_Click" runat="server" Text="Aceptar"></asp:Button>
</p>