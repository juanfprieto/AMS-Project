<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Marketing.SeguimientoClienteEspecifico.ascx.cs" Inherits="AMS.Marketing.SeguimientoClienteEspecifico" %>
<table>
    <tbody>
        <tr>
            <td>
                Nit del Cliente:
            </td>
            <td>
                <asp:TextBox ID="txtNit" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                Placa del Vehículo:
            </td>
            <td>
                <asp:TextBox ID="txtPlaca" runat="server"></asp:TextBox>
            </td>
        </tr>
    </tbody>
</table>
