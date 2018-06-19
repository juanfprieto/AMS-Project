<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tesoreria.RegistroConsignacion.ascx.cs" Inherits="AMS.Finanzas.AMS_Tesoreria_RegistroConsignacion" %>

<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>


<fieldset>
    <table>
        <tr>
            <td>
                Código de la cuenta:
            </td>
            <td>
                <asp:DropDownList ID="ddlCodigoCuenta" runat="server" cssClass="dmediano"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Número Consignación:
            </td>
            <td>
                <asp:TextBox ID="txtNumero" runat="server" cssClass="tmediano"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Pedido del cliente:
            </td>
            <td>
                <asp:TextBox ID="txtPedido" runat="server" cssClass="tmediano"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Estado de la consignación:
            </td>
            <td>
                <asp:DropDownList ID="ddlEstadoConsignacion" runat="server" cssClass="dmediano"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo de venta:
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoVenta" runat="server" cssClass="dmediano"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo de pago:
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoCompra" runat="server" cssClass="dmediano"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Asesor:
            </td>
            <td>
                <asp:DropDownList ID="ddlUsuario" runat="server" cssClass="dmediano"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Fecha de la consignación:
            </td>
            <td>
                <asp:TextBox ID="txtFecha" runat="server" cssClass="tmediano" onKeyUp="DateMask(this)" placeholder="aaaa-mm-dd"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Observaciones:
            </td>
            <td>
                <asp:TextBox ID="txtObervaciones" runat="server" TextMode="MultiLine" Width="35%" Height="100px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br /> <br />
    <asp:Button id="btnGuardar" runat="server" Text="Grabar" OnClick="guardarCondignacion"/>
</fieldset>
