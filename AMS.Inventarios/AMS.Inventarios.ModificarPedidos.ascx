<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Inventarios.ModificarPedidos.ascx.cs" Inherits="AMS.Inventarios.ModificarPedidos" %>

<fieldset> 
    <TABLE id="Table1" class="filtersIn">
        <tr>
            <td>
                Código Pedido:<br />
                <asp:DropDownList id="ddlCodPedido" OnSelectedIndexChanged="CambioPrefijo" AutoPostBack="True" class="dmediano" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Número Pedido:<br />
                <asp:DropDownList id="ddlNumPedido" class="dpequeno" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Button id="btnModificar" onclick="ModificarPedido" runat="server" Text="Modificar Pedido" ></asp:Button>
            </td>
        </tr>
    </TABLE>
</fieldset>