<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Asousados.ListaUsadosEditar.ascx.cs" Inherits="AMS.Asousados.ListaUsadosEditar" %>

<p>
<asp:Label ID="Label1" runat="server">Vehículos</asp:Label>
<asp:DataGrid id="dgListaVehiculos" cssclass="datagrid" runat="server" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" OnItemCommand="dgListaVehiculos_Command">
		<HeaderStyle CssClass="header"></HeaderStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		
		<Columns>
            <asp:TemplateColumn HeaderText="Id" Visible="false">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ID") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Asociado / Dueño">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ASOCIADO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Placa">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Catálogo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CATALOGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operación">
				<ItemTemplate>
					<asp:Button CommandName="editar" Text="Editar" ID="btnEdit" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
				<ItemTemplate>
					<asp:Button CommandName="remover" Text="Remover" ID="btnEliminar" CausesValidation="False" runat="server" AutoPostBack="true"/>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
		<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</p>