<%@ Control Language="c#" codebehind="AMS.Inventarios.Sustitucion.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ControlSustitucion" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript">
function MostrarRefs1(obTex,obCmbLin)
{
    ModalDialog(obTex,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER By mite_codigo', new Array());
}
</script>
<script language="javascript">
function MostrarRefs2(obTex,obCmbLin,obCmbPr)
{
    if(obCmbPr.value == 'U')
        ModalDialog(obTex,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER By mite_codigo', new Array());
}
</script>
<fieldset>
	<legend>Información Sustitución</legend>
	<table class="table1" class="filtersIn">
		<tbody>
			<tr>
				<td>
					Prefijo Sustitución :
					<asp:Label id="lbPrefijo" runat="server"></asp:Label></td>
				<td>
					Número Sustitución :
					<asp:Label id="lbNumero" runat="server"></asp:Label></td>
				<td>
					Fecha Sustitución :&nbsp;<asp:Label id="lbFecha" runat="server"></asp:Label>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>
<p>
</p>
<fieldset>
	<legend>Información Sustitución</legend>
	<table class="table2" class="filtersIn">
		<tbody>
			<tr>
				<td>
					Tipo de Sustitución&nbsp; :
				</td>
				<td align="right">
					<asp:DropDownList id="ddlTipoSust" runat="server">
						<asp:ListItem Value="A" Selected="True">Anterior</asp:ListItem>
						<asp:ListItem Value="U">Unificaci&#243;n</asp:ListItem>
						<asp:ListItem Value="P">Posterior</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					Linea de Bodega :
				</td>
				<td align="right">
					<asp:DropDownList id="ddlLinea" runat="server"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					Codigo Item Origen :
				</td>
				<td align="right">
					<asp:TextBox id="tbCodOrigen" runat="server" Width="186px"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Codigo Item Sustitución :
				</td>
				<td align="right">
					<asp:TextBox id="tbCodSust" runat="server" Width="186px"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td align="right" colspan="2">
					<asp:Button id="btnAgregar" onclick="AgregarSustitucion" runat="server" Text="Agregar"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>
<p>
	<asp:DataGrid id="dgSustitucion" runat="server"	cssclass="datagrid" OnDeleteCommand="EliminarRegistro" AutoGenerateColumns="False" Font-Names="Verdana" BorderWidth="1px"
		GridLines="Vertical" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="CLASE" HeaderText="CLASE SUSTITUCI&#211;N"></asp:BoundColumn>
			<asp:BoundColumn DataField="CODORIGEN" HeaderText="CODIGO ORIGEN"></asp:BoundColumn>
			<asp:BoundColumn DataField="CODSUSTI" HeaderText="CODIGO SUSTITUCI&#211;N"></asp:BoundColumn>
			<asp:BoundColumn DataField="LINEA" HeaderText="LINEA"></asp:BoundColumn>
			<asp:ButtonColumn Text="Eliminar" ButtonType="PushButton" HeaderText="ELIMINAR REGISTRO" CommandName="Delete"></asp:ButtonColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Button id="btnCrear" onclick="CrearSustitucion" runat="server" Text="Crear Sustitución"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
