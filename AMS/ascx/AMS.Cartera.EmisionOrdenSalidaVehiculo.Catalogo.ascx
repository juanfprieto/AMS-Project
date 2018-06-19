<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.EmisionOrdenSalidaVehiculo.Catalogo.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.BusquedaCatalogo" %>

<table>
    <tbody>
        <tr>
            <td>
                Catálogo del Vehículo : 
            </td>
            <td>
                <asp:DropDownList id="ddlCatalogo" runat="server" AutoPostBack="True" onSelectedIndexChanged="ddlCatalogo_IndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                VIN del Vehículo : 
            </td>
            <td>
                <asp:DropDownList id="ddlVIN" runat="server"></asp:DropDownList>
            </td>
        </tr>
    </tbody>
</table>
<p>
    <asp:Button id="btnAceptar" onclick="btnAceptar_Click" runat="server" Text="Aceptar"></asp:Button>
</p>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>