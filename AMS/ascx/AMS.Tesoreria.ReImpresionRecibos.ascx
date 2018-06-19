<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EmisionRecibos.ReImpresionRecibos.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ReImpresionRecibos" %>
<table id="Table" class="filtersIn">
    <tbody>
        <tr>
            <td>
                Escoja el prefijo del Recibo o Comprobante : 
            </td>
            <td>
                <asp:DropDownList id="ddlPrefijo" onSelectedIndexChanged="ddlPrefijo_IndexChanged" AutoPostBack="true" class="dmediano" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Escoja el número del Recibo o Comprobante : 
            </td>
            <td>
                <asp:DropDownList id="ddlNumero" class="dpequeno" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button id="btnAceptar" onclick="btnAceptar_Click" runat="server" Text="Aceptar"></asp:Button>
            </td>
        </tr>
    </tbody>
</table>