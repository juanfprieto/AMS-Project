<%@ Control Language="c#" codebehind="AMS.Inventarios.FormatoPedido.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.FormatoPedido" %>
<fieldset>
	<table id="Table" class="filtersIn">
    	<legend>Información Cliente</legend>
		<tbody>
			<tr>
				<td>
					Nit Proveedor:
				
					<br><asp:DropDownList id="ddlProveedor" class="dmediano" runat="server"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td align="left" colspan="2">
					<asp:Button id="btnConfirmar" onclick="ConfirmarNit" runat="server" Text="Confirmar"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
    
</fieldset>
<p>
</p>
<p>
	<asp:PlaceHolder id="plProceso" runat="server" Visible="False">
</p>

<FIELDSET>
	<LEGEND>Opciones Información 
Formato</LEGEND>
	<TABLE class="main">
		<TR>
			<TD>Objeto :
				<asp:DropDownList id="ddlObjeto" runat="server" OnSelectedIndexChanged="CambioObjeto" AutoPostBack="True">
					<asp:ListItem Value="P" Selected="True">Pedido</asp:ListItem>
					<asp:ListItem Value="R">Proveedor</asp:ListItem>
					<asp:ListItem Value="I">Items</asp:ListItem>
					<asp:ListItem Value="O">Otro Valor</asp:ListItem>
				</asp:DropDownList>&nbsp;</TD>
			<TD>
				<TABLE class="main">
					<TR>
						<TD>Información :</TD>
						<TD align="right">
							<asp:DropDownList id="ddlInformacion" runat="server"></asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right" colSpan="2">
							<asp:TextBox id="tbInformacion" runat="server" visible="False" Width="84px"></asp:TextBox></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
		<TR>
			<TD align="right" colSpan="2">
				<asp:Button id="btnAgregar" onclick="AgregarElemento" runat="server" Text="Agregar"></asp:Button></TD>
		</TR>
	</TABLE>
</FIELDSET>
<P>
	<ASP:DataGrid id="dgFormato" runat="server" cssclass="datagrid" OnItemCommand="DgFormatoCommand" OnDeleteCommand="DgFormatoDelete"
		BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False">
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="VALORAGREGADO" HeaderText="Valor Agregado"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Ubicar">
				<ItemTemplate>
					<asp:Button CommandName="up" Text="Subir" Runat="server" />
					<asp:Button CommandName="down" Text="Bajar" Runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:ButtonColumn Text="Eliminar" ButtonType="PushButton" HeaderText="Eliminar" CommandName="Delete"></asp:ButtonColumn>
		</Columns>
	</ASP:DataGrid></P>
<P>
	<asp:Button id="btnAceptar" onclick="AceptarElementos" runat="server" Text="Aceptar"></asp:Button>&nbsp;&nbsp;
	<asp:Button id="btnCancelar" onclick="CancelarProceso" runat="server" Text="Cancelar"></asp:Button></P>
</asp:PlaceHolder>
<P></P>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
