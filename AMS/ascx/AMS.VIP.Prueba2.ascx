<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.VIP.Prueba2.ascx.cs" Inherits="AMS.VIP.Prueba2" %>

<style>
    #despues {
    font-family: Arial
    }
</style>

<fieldset>
    <table>
        <td>
            <tr><asp:Label ID="nombre"  Text="Selecione un usuario:" runat="server"></asp:Label></tr>
            <tr><asp:DropDownList ID="listanombre" AutoPostBack="True" OnSelectedIndexChanged="Selection_Change" runat="server">
        <%--<asp:ListItem>Juan Valdez</asp:ListItem>
        <asp:ListItem>Item 2</asp:ListItem>
        <asp:ListItem>Item 3</asp:ListItem>--%>
    </asp:DropDownList></tr>
        </td>
        <td>
            <tr><asp:Label ID="nit" Text="NIT Usuario:" runat="server"></asp:Label></tr>
            <tr><asp:TextBox ID="nituser" Text="" runat="server"></asp:TextBox></tr>
        </td>
    <asp:Label ID="clave" Text="Digite Clave:" runat="server"></asp:Label>   
    <asp:TextBox ID="inclave" TextMode="Password" runat="server"/>
        
    <asp:Button Text="Validar" runat="server"/>
    </table>
</fieldset>