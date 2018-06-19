<%@ Control Language="c#" codebehind="AMS.Inventarios.NuevaLista.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.NuevaLista" %>

<fieldset>
    <legend>Datos Nueva Lista de Precio</legend>
    <table class="main">
        <tbody>
            <tr>
                <td>
                    Codigo Lista de Precios : 
                    <asp:TextBox id="tbCodigoLista" runat="server" Width="30px" MaxLength="2"></asp:TextBox>
                    <asp:RequiredFieldValidator id="validatorTbCodigoLista" runat="server" ControlToValidate="tbCodigoLista" Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    Nombre Lista de Precios : 
                    <asp:TextBox id="tbNombreLista" runat="server" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator id="validatorTbNombreLista" runat="server" ControlToValidate="tbNombreLista" Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    Moneda : 
                    <asp:DropDownList id="ddlMoneda" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button id="tbCrear" onclick="CrearLista" runat="server" Text="Crear"></asp:Button>
                </td>
            </tr>
        </tbody>
    </table>
</fieldset>
<asp:Label id="lb" runat="server"></asp:Label>