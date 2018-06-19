<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.VIP.EstadosTesoreria.ascx.cs" Inherits="AMS.VIP.EstadosTesoreria" %>

<p>
<asp:Label runat="server">Facturas</asp:Label>
<asp:DataGrid id="dgFacturas" runat="server" CssClass="datagrid" AutoGenerateColumns="false" GridLines="Vertical"
		ShowFooter="True">
		<HeaderStyle CssClass="header"></HeaderStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<Columns>
            <asp:TemplateColumn HeaderText="Código de Producto Padre">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODPADRE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre Padre">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMPADRE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cédula Padre">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CEDPADRE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Teléfono">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TELEFONO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Código de Producto Cliente">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODCLI") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cliente">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMCLI") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cédula Cliente">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CEDCLI") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Código de Factura">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODFAC") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Tipo de Venta">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TIPOFAC") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Fecha de la Transacción">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "FECHTRANS") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Fecha de Vencimiento">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "FECHVENC") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Valor">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Aval">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "AVAL") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Estado">
				<ItemTemplate>
					<asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
</asp:DataGrid>
</p>
<p>
<asp:Button id="btnGuardar" runat="server" OnClick="guardar" Text="Guardar Cambios"/>
</p>