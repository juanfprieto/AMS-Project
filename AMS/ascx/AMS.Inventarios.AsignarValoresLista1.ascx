<%@ Control Language="c#" codebehind="AMS.Inventarios.AsignarValoresLista1.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ParametrosValorLista" %>

<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<fieldset >
    <legend>Parámetros Para Asignación de precios</legend>
    <table class="fieltersIn">
        <tbody>
            <tr>
                <td>
                    Valor Base Modificación :
                    <asp:DropDownList id="ddlValorBase" runat="server"></asp:DropDownList>
                </td>
                <td>
                    Tipo de Operación :
                    <asp:DropDownList id="ddlTipoOper" runat="server"></asp:DropDownList>
                </td>
                <td>
                    Valor&nbsp;Modificación :
                    <asp:TextBox id="tbValorModificacion" onkeyup="NumericMaskE(this,event)" runat="server" Width="101px"></asp:TextBox>
                    <asp:RequiredFieldValidator id="validatorTbValorModificacion" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic" ControlToValidate="tbValorModificacion">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button id="btnAceptar" onclick="AceptarParametro" runat="server" Text="Aceptar"></asp:Button>
                </td>
            </tr>
        </tbody>
    </table>
</fieldset>
<asp:Label id="lb" runat="server"></asp:Label>
