<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.VIP.IngresoCliente.ascx.cs" Inherits="AMS.VIP.IngresoCliente" %>
<table>
    <tbody>
        <tr>
            <td>
                Tarjeta:
            </td>
            <td>
                <asp:TextBox ID="txtTarjeta" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNit" runat="server" Visible="false" Text="Cédula:"/>
            </td>
            <td>
                <asp:TextBox ID="txtNit" runat="server" Visible="false"></asp:TextBox>
            </td>
        </tr>
    </tbody>
</table>