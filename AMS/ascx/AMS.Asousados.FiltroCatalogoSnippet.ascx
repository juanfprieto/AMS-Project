<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Asousados.FiltroCatalogoSnippet.ascx.cs" Inherits="AMS.Asousados.FiltroCatalogoSnippet" %>
<table class="tablewhite3">
    <tr>
        <td>
            Clase de Vehículo:
        </td>
        <td>
            <asp:DropDownList ID="ddlClaseVeh" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClaseVeh_OnSelectedIndexChanged" />
        </td>
        <asp:PlaceHolder ID="phMarca" runat="server" Visible="false">
        <td>
            Marca:
        </td>
        <td>
            <asp:DropDownList ID="ddlMarca" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMarca_OnSelectedIndexChanged" />
        </td>
        </asp:PlaceHolder>
    </tr>
    <tr>
        <asp:PlaceHolder ID="phRefPrincipal" runat="server" Visible="false">
        <td>
            Referencia Principal:
        </td>
        <td>
            <asp:DropDownList ID="ddlRefPrincipal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRefPrincipal_OnSelectedIndexChanged" />
        </td>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phRefComplementaria" runat="server" Visible="false">
        <td>
            Referencia Complementaria:
        </td>
        <td>
            <asp:DropDownList ID="ddlRefComplementaria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRefComplementaria_OnSelectedIndexChanged" />
        </td>
        </asp:PlaceHolder>
    </tr>
    <tr>
        <asp:PlaceHolder ID="phCarroceria" runat="server" Visible="false">
        <td>
            Carrocería:
        </td>
        <td>
            <asp:DropDownList ID="ddlCarroceria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCarroceria_OnSelectedIndexChanged" />
        </td>
        </asp:PlaceHolder>
    </tr>
    <asp:PlaceHolder ID="phDatosCatalogo" runat="server" Visible="false">
    <tr>
        <td>
            Cilindraje:
        </td>
        <td>
            <asp:DropDownList ID="ddlCilindraje" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCilindraje_OnSelectedIndexChanged" />
        </td>
        <td>
            Tipo de Combustible:
        </td>
        <td>
            <asp:DropDownList ID="ddlTipoCombustible" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCombustible_OnSelectedIndexChanged" />
        </td>
    </tr>
    <tr>
        <td>
            Aspiración del Motor:
        </td>
        <td>
            <asp:DropDownList ID="ddlAspiracion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAspiracion_OnSelectedIndexChanged" />
        </td>
        <td>
            Tracción:
        </td>
        <td>
            <asp:DropDownList ID="ddlTraccion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTraccion_OnSelectedIndexChanged" />
        </td>
    </tr>
    <tr>
        <td>
            Caja de Cambios:
        </td>
        <td>
            <asp:DropDownList ID="ddlCaja" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DatosCatalogo_OnSelectedIndexChanged" />
        </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phCatalogo" runat="server" Visible="false">
    <tr>
        <td>
            Catálogo:
        </td>
        <td colspan="3">
            <asp:DropDownList ID="ddlCatalogo" runat="server" />
        </td>
    </tr>
    </asp:PlaceHolder>
</table>