<%@ Control Language="c#" codebehind="AMS.Tools.Permisos.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.Permisos" %>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>
	Modifica completamente los permisos para el perfil seleccionado.
</p>
<p>
	Perfiles :
	<asp:DropDownList id="perfiles" runat="server" class="dmediano"></asp:DropDownList>
</p>
</td> 
</tr>
</table>
</fieldset>

	<legend>Opciones de Menu</legend>
	<asp:DataGrid id="opcionesMenu" runat="server" cssclass="datagrid" AutoGenerateColumns="false" GridLines="Vertical">
		<FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Identificador de Opci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "IDENTIFICADOR") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Opci&#243;n de Men&#250;">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OPCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Jerarquia de Opci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "JERARQUIA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="OPCI&#211;N">
				<ItemTemplate>
					<asp:CheckBox id="chkPrm" runat="server" TextAlign="Left" Text="Permitir Acceso?"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>

<p>
	<asp:Button id="btnAceptar" onclick="Generar_Permisos" runat="server" Text="Aceptar"></asp:Button>
</p>

<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>

