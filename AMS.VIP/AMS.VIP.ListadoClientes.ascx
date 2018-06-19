<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.VIP.ListadoClientes.ascx.cs" Inherits="AMS.VIP.ListadoClientes" %>

<p>
    <table>
        <tbody>
            <tr>
                <td>
                    Nombre:
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" OnClick="filtrar">
                    </asp:Button>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>
<asp:Label runat="server">Clientes</asp:Label>
<asp:DataGrid id="dgListaClientes" cssclass="datagrid" runat="server" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" OnItemCommand="dgListaClientes_Command">
		
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
			<asp:TemplateColumn HeaderText="Operación">
				<ItemTemplate>
					<asp:Button CommandName="editar" Text="Editar" ID="btnEdit" runat="server" />
                    <asp:Button CommandName="tarjeta" Text="Solicitar Nueva Tarjeta" ID="btnTarjeta" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
</asp:DataGrid>
</p>