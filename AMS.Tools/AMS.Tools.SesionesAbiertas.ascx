<%@ Control Language="c#" codebehind="AMS.Tools.SesionesAbiertas.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.SesionesAbiertas" %>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>
	A Continuación se muestra una lista con todas las sesiones de usuario abiertas, 
	si 
	es necesario se pueden cerrar desde aca:
</p>
<p>
	<asp:DataGrid id="sesionesAbiertas" runat="server" CellPadding="3"  GridLines="Vertical" AutoGenerateColumns="false" OnItemCommand="dgCerrar_Sesion_Usuario" class="datagrid">
		    <FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
		    <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		
		<Columns>
			<asp:TemplateColumn HeaderText="NOMBRE DEL USUARIO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMBUSUARIO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="LOGIN">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "LOGIN") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DIRECCIÓN IP">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DIRECCIONIP") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="OPCIÓN">
				<ItemTemplate>
					<asp:Button CommandName="cerrarSesion" Text="Cerrar Sesión" ID="crrSes" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Button id="actualizar" onclick="Actualizar_Grilla" runat="server" class="bmediano" Text="Actualizar"></asp:Button>
</p>
</td>
</tr>
</table>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
</fieldset>
