<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="AMS.Documentos.CausalDevolucion.ascx.cs"
    Inherits="AMS.Documentos.CausalDevolucion" %>
<table style="background-color: transparent" border="1">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblVendedor" Text="Vendedor" />
        </td>
        <td>
            <asp:Label runat="server" ID="lblClave" Text="Clave" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlVendedor" runat="server" />
        </td>
        <td>
            <asp:TextBox ID="txtClaveVendedor" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblCausa" Text="Causa" />
        </td>
        <td>
            <asp:Label runat="server" ID="lblDetalleCausa" Text="¿Cuál?" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlCausa" runat="server" OnSelectedIndexChanged="ddlCausa_indexChanged" />
        </td>
        <td>
            <asp:TextBox ID="txtDetalleCausa" runat="server" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblAccion" Text="Acción" />
        </td>
        <td>
            <asp:Label runat="server" ID="lblProveedor" Text="Proveedor" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlAccion" runat="server" OnSelectedIndexChanged="ddlAccion_indexChanged" />
        </td>
        <td>
            <asp:TextBox ID="txtProveedor" runat="server" Visible="false" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_click" Text="Guardar"
                CausesValidation="true" />
        </td>
    </tr>
</table>
