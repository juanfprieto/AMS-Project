<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.VIP.Redenciones.ascx.cs" Inherits="AMS.VIP.Redenciones" %>

<asp:Label ID="lblUsrNoAutorizado" runat="server" Visible="false" ForeColor="Red" Font-Size="Large"/>
<p>
<asp:Label runat="server">Items por Redimir</asp:Label>
<asp:DataGrid id="dgPorRedimir" cssclass="datagrid" runat="server" AutoGenerateColumns="false" GridLines="Vertical"
		ShowFooter="True" OnItemCommand="redimir">
		
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		
		<Columns>
            <asp:TemplateColumn HeaderText="Código" Visible="false">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cliente">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CLIENTE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Estado de la Redención">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Item">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ITEM") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operación">
				<ItemTemplate>
					<asp:Button CommandName="redimir" Text="Redimir" ID="btnRdn" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
        </asp:DataGrid>
</p>
<p>
<asp:Label ID="Label1" runat="server">Items por Redimidos</asp:Label>
<asp:DataGrid id="dgRedimidos" cssclass="datagrid" runat="server" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" OnItemCommand="imprimir">
		
		<HeaderStyle CssClass="header"></HeaderStyle>
        <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		
		<Columns>
            <asp:TemplateColumn HeaderText="Código" Visible="false">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cliente">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CLIENTE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Estado de la Redención">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Item">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ITEM") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operación">
				<ItemTemplate>
					<asp:Button CommandName="imprimir" Text="Imprimir" ID="btnImpr" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
</asp:DataGrid>
</p>