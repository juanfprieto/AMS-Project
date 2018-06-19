<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.Exporter.ascx.cs" Inherits="AMS.Tools.Exporter" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table id="Table" class="filtersIn">
<tbody>
<tr>
<td>

<p>
	<asp:Label id="Label1" runat="server">Esta es una herramienta creada para exportar
    tablas completas de la base de datos:</asp:Label>
</p>
<p>
	Formato :&nbsp;<asp:DropDownList id="ddlFormato" runat="server" 
        onselectedindexchanged="ddlFormato_SelectedIndexChanged">
		<asp:ListItem Value="E">Excel</asp:ListItem>
	</asp:DropDownList>&nbsp;&nbsp;&nbsp; Tabla a Exportar :
	<asp:DropDownList id="tablaAct" runat="server" AutoPostBack="True" onselectedindexchanged="tablaAct_SelectedIndexChanged"></asp:DropDownList>
</p>
<asp:Panel ID="pnlFecha" Runat="server" Visible="False">
	<p>
		Año :&nbsp;<asp:DropDownList id="ddlAno" runat="server" AutoPostBack="True" onselectedindexchanged="ddlAno_SelectedIndexChanged"></asp:DropDownList>&nbsp;&nbsp;&nbsp; 
		Mes :&nbsp;<asp:DropDownList id="ddlMes" runat="server" AutoPostBack="True" onselectedindexchanged="ddlMes_SelectedIndexChanged"></asp:DropDownList>
	</p>
</asp:Panel>
<p>
	Registros :&nbsp;<asp:DropDownList id="ddlRegistros" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;Total 
	:&nbsp;<asp:Label id="lblTotal" runat="server"></asp:Label>
</p>
<p>
	<asp:DataGrid id="dgTabla" runat="server" CssClass="datagrid" ShowFooter="False" AutoGenerateColumns="False">
		<HeaderStyle CssClass="header"></HeaderStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
	</asp:DataGrid>
</p>
<p>
	<asp:Button id="actual" onclick="actual_Click" runat="server" Text="Generar"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
<font color="#009900"></font>
</td>
</tr>
</tbody>
</table>
</fieldset>
