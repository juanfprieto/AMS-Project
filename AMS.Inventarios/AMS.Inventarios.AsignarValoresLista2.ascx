<%@ Control Language="c#" codebehind="AMS.Inventarios.AsignarValoresLista2.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.AsignaValoresLista" %>
<asp:PlaceHolder id="plProceso" runat="server">
    <fieldset >
        <legend>Items a Actualizar</legend>
        <asp:RadioButtonList id="rblTipoItems" runat="server" RepeatDirection="Horizontal" CellSpacing="5" OnSelectedIndexChanged="CambioGrupoItems" AutoPostBack="True" CssClass="main">
            <asp:ListItem Value="T" Selected="True">Todos los Items</asp:ListItem>
            <asp:ListItem Value="F">Filtrar Items</asp:ListItem>
        </asp:RadioButtonList>
    </fieldset>
    <p></p>
    <asp:PlaceHolder id="plFiltros" runat="server" Visible="False">
        <fieldset >
            <legend>Filtros para Items</legend>
            <table class="fieltersIn">
                <tbody>
                    <tr>
                        <td>
                            Parametro de Filtro :
                            <asp:DropDownList id="ddlParametro" runat="server" OnSelectedIndexChanged="CambioParametro" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td>
                            Valor de Filtro :
                            <asp:DropDownList id="ddlValor" runat="server"></asp:DropDownList>
                            &nbsp;</td>
                        <td>
                            <asp:Button id="btnAgregar" onclick="AgregarFiltro" runat="server" Text="Agregar Filtro"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:DataGrid id="dgFiltrosValor" runat="server" cssclass="datagrid" CellPadding="3" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="False" Width="700px" OnDeleteCommand="DgFiltroValorDelete">
                                <FooterStyle cssclass="footer"></FooterStyle>
                                <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
                                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
                                <ItemStyle cssclass="item"></ItemStyle>
                                <HeaderStyle cssclass="header"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="FILTRO" ReadOnly="True" HeaderText="Filtro"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor"></asp:BoundColumn>
                                    <asp:ButtonColumn Text="Eliminar" ButtonType="PushButton" HeaderText="Eliminar" CommandName="Delete"></asp:ButtonColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </tbody>
            </table>
        </fieldset>
    </asp:PlaceHolder>
    <p>
        <asp:Button id="btnActualizar" onclick="ActualizarListaPrecios" runat="server" Text="Realizar Actualización"></asp:Button>
    </p>
</asp:PlaceHolder>
<p>
</p>
<asp:PlaceHolder id="plResultado" runat="server" Visible="False">
    <fieldset>
        <legend>Resultado Actualización</legend>
            <asp:DataGrid id="dgResultado" runat="server" cssclass="datgrid" CellPadding="3" AutoGenerateColumns="true">
                <FooterStyle cssclass="footer"></FooterStyle>
                <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
                <ItemStyle cssclass="item"></ItemStyle>
                <HeaderStyle cssclass="header"></HeaderStyle>
            </asp:DataGrid>
    </fieldset>
    <p>
        <asp:Button id="btnVolver" onclick="VolverAdmin" runat="server" Text="Volver"></asp:Button>
    </p>
</asp:PlaceHolder>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>
