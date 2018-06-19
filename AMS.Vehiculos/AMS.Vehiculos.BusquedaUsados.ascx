<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.BusquedaUsados.ascx.cs" Inherits="AMS.Vehiculos.BusquedaUsados" %>

<p>
    <table>
        <tbody>
            <tr>
                <td>
                    Búsqueda:
                </td>
                <td>
                    <asp:TextBox ID="txtBusqueda" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click">
                    </asp:Button>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>

<asp:DataGrid id="dgListaVehiculos" cssclass="datagrid" runat="server" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" OnItemCommand="dgListaClientes_Command">
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		
		<Columns>
            <asp:TemplateColumn HeaderText="Código de Producto">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cliente">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Código Afiliación" Visible="false">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODAFIL") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Afiliación">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "AFILIACION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="No. de Tarjeta">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TARJETA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="">
				<ItemTemplate>
					<asp:Button CommandName="ver" Text="Detalle" ID="btnVer" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
		
</asp:DataGrid>
</p>