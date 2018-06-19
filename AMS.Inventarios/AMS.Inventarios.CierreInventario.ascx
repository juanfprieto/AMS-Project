<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.CierreInventario.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_CierreInventario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>

<FIELDSET>
<LEGEND>Cierre Inventario Fisicio</LEGEND>
	<TABLE id="Table1" class="filtersIn">
		<TR>
			<TD>Prefijo Inventario Físico a Cerrar:&nbsp;</TD>
			<TD align="left">
				<asp:dropdownlist id="ddlPrefInventario" AutoPostBack="True" runat="server" onselectedindexchanged="ddlPrefInventario_SelectedIndexChanged"></asp:dropdownlist></TD>
			<TD>&nbsp;&nbsp;</TD>
			<TD>Número Inventario Físico a Cerrar:&nbsp;</TD>
			<TD align="left">
				<asp:dropdownlist id="ddlNumInventario" AutoPostBack="True" runat="server" onselectedindexchanged="ddlNumeroInventario_SelectedIndexChanged"></asp:dropdownlist></TD>
			<TD>&nbsp;&nbsp;&nbsp;</TD>
		</TR>
	</TABLE>
	<TABLE class="tablewhite" id="Table2" cellSpacing="1" cellPadding="1" border="0">
		<TR>
			<TD>Prefijo Documento Ref :
				<asp:TextBox id="tbPrefDocRef" runat="server" Width="60px" MaxLength="6"></asp:TextBox>&nbsp;&nbsp;</TD>
			<TD>Número Documento Ref :
				<asp:TextBox id="tbPrefNumRef" runat="server" Width="100px"></asp:TextBox></TD>
		</TR>
	</TABLE>
	<TABLE class="tablewhite" id="Table3" cellSpacing="1" cellPadding="1" border="0">
		<tr>
			<td>Centro de Costo:&nbsp;&nbsp;</td>
			<td>Responsable:&nbsp;&nbsp;</td>
			<td>Fecha:&nbsp;&nbsp;</td>
		</tr>
		<tr>
			<td><asp:DropDownList id="ddlCentro" runat="server"></asp:DropDownList></td>
			<td><asp:DropDownList id="ddlVendedor" runat="server"></asp:DropDownList>
			<td><asp:textbox id="txtFechaProceso" onkeyup="DateMask(this)" runat="server" Width="80px"></asp:textbox></td>
		</tr>
	</TABLE>
	<TABLE class="tablewhite" id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
		<TR>
			<TD>
				<asp:DataGrid id="dgrItems" runat="server" cssclass="datagrid" AutoGenerateColumns="false" Font-Names="Verdana"
					BorderWidth="1px" GridLines="Vertical"
					CellPadding="3"  HeaderStyle-BackColor="#ccccdd">
					<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
					<ItemStyle cssclass="item"></ItemStyle>
					<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="CODIGO">
							<HeaderStyle Width="10%"></HeaderStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "DINV_MITE_CODIGO") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="NOMBRE">
							<HeaderStyle Width="50%"></HeaderStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "DINV_MITE_NOMBRE") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="ALMCN">
							<HeaderStyle Width="10%"></HeaderStyle>
							<ItemTemplate>
					NIMDA			<%# DataBinder.Eval(Container.DataItem, "DINV_PALM_ALMACEN") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="TARJETA">
							<HeaderStyle Width="10%"></HeaderStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "DINV_TARJETA") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="DIFERENCIA">
							<HeaderStyle Width="10%"></HeaderStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "DINV_DIFERENCIA") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="COSTO">
							<HeaderStyle Width="10%"></HeaderStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "DINV_COSTDIFERENCIA", "{0:#,##0}") %>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid></TD>
		</TR>
		<TR>
			<TD>
				<asp:button id="btnCerrar" runat="server" CausesValidation="False" Text="Cerrar Inventario Físico" onclick="btnCerrar_Click" onclientclick="espera();" ></asp:button></TD>
		</TR>
	</TABLE>
</FIELDSET>
<p>
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p>
