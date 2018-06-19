<%@ Register TagPrefix="uc1" TagName="Seleccionar" Src="AMS.Tools.Seleccionar.ascx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Inventarios.Guias.ascx.cs" Inherits="AMS.Inventarios.Guias" %>
<fieldset>
<table id="Table" class="filtersIn">
    <tbody>
        <tr>
            <td>
                <asp:Label id="lblNit" runat="server">Nit:</asp:Label>
            </td>
            <td>
                <asp:DropDownList id="ddlNit" runat="server" OnSelectedIndexChanged="ddlNitIndexChanged" AutoPostBack="True"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDireccion" runat="server">Direcci&oacute;n:</asp:Label>
            </td>
            <td>
                <asp:TextBox id="txtDireccion" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valDireccion" runat="server" ControlToValidate="txtDireccion" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCiudad" runat="server">Ciudad:</asp:Label>
            </td>
            <td>
                <asp:DropDownList id="ddlCiudad" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="valCiudad" runat="server" ControlToValidate="ddlCiudad" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbCajas" runat="server"># Cajas:</asp:Label>
            </td>
            <td>
                <asp:TextBox id="txtCajas" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbPeso" runat="server">Peso:</asp:Label>
            </td>
            <td>
                <asp:TextBox id="txtPeso" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbdespachador" runat="server">Despachador:</asp:Label>
            </td>
            <td>
                <asp:DropDownList id="ddlDespachador" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lbPassword" runat="server">Clave:</asp:Label>
            </td>
            <td>
                <asp:TextBox id="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbObservaciones" runat="server">Observaciones:</asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox TextMode="MultiLine" Columns="40" Rows="4" id="txtObservaciones" runat="server"></asp:TextBox>
            </td>
        </tr>
    </tbody>
</table>

<uc1:seleccionar id="facturas" runat="server"></uc1:seleccionar>

<asp:Label ID="lbInfo" runat="server"></asp:Label>
</fieldset>