<%@ Control Language="c#" codebehind="AMS.Inventarios.ModificadorListaEmpaque.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ModificadorListaEmpaque" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript">
    function CargaItem(/*textbox*/ob,/*dropdownlist*/obCmbLin,/*string*/ano)
    {
		ModalDialogInventarios(ob,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE,DBXSCHEMA.CANTACTL(MIT.mite_codigo,'+ano+') AS SALDO FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE PLIN.plin_tipo=\''+(obCmbLin.value.split('-'))[1]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER By MIT.mite_codigo', new Array(),1,obCmbLin.value);
    }
</script>
<table>
	<tbody>
		<tr>
			<td>
				<fieldset>
                    <legend>Lista de Empaque</legend>Numero Lista Empaque : <asp:Label id="lbNumLista" runat="server"></asp:Label> 
                    <p>
						Almacén :
						<asp:Label id="lbAlmacen" runat="server"></asp:Label>
					</p>
                </fieldset>
			</td>
			<td>
				<fieldset>
                    <legend>Información Cliente</legend>Nit Cliente : <asp:Label id="lbNitCliente" runat="server"></asp:Label> 
                    <p>
						Nombre Cliente :
						<asp:Label id="lbNombreCliente" runat="server"></asp:Label>
					</p>
                </fieldset>
			</td>
			<td>
				<fieldset>
					<legend>Fecha</legend>
					<p>
						&nbsp;<asp:Label id="lbDate" runat="server"></asp:Label>
					</p>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<p>
	&nbsp;<asp:DataGrid id="dgListaEmpaque" runat="server" cssclass="datagrid" OnCancelCommand="DgListaEmpaqueCancel" OnUpdateCommand="DgListaEmpaqueUpdate"
		OnEditCommand="DgListaEmpaqueEdit" OnDeleteCommand="DgListaEmpaqueDelete" 
		CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="altermate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="CODIGO" ReadOnly="True" HeaderText="Codigo Item"></asp:BoundColumn>
			<asp:BoundColumn DataField="LINEA" ReadOnly="True" HeaderText="Linea"></asp:BoundColumn>
			<asp:BoundColumn DataField="PEDIDO" ReadOnly="True" HeaderText="Pedido Relacionado"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDADASIGNADA" HeaderText="Cantidad Asignada"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDADPENDIENTE" ReadOnly="True" HeaderText="Cantidad Pendiente Pedido"></asp:BoundColumn>
			<asp:ButtonColumn Text="Eliminar" ButtonType="PushButton" HeaderText="Eliminar" CommandName="Delete"></asp:ButtonColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" HeaderText="Editar" CancelText="Cancelar"
				EditText="Editar"></asp:EditCommandColumn>
		</Columns>
		<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	</asp:DataGrid><br>
</p>
<fieldset>
	<legend>Agregar Items</legend>
	<P><ASP:DATAGRID id="dgItemsNuevos" runat="server" cssclass="datagrid" enableViewState="true" AutoGenerateColumns="false"
			 GridLines="Vertical" CellPadding="3" OnDeleteCommand="DgInserts_Delete">
			<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
			<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Codigo:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_codigo", "{0:N}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Nombre:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Linea de Bodega:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "plin_codigo") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Cantidad:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_cantidad", "{0:N}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Cant Asig:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_cantasig", "{0:N}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Precio Inicial :">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_precioinicial", "{0:C}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Precio Final:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_precio", "{0:C}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="IVA:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_iva", "{0:N}%") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Descuento:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_desc", "{0:N}%") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Total:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_tot", "{0:C}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Operaciones:">
					<ItemTemplate>
						<asp:Button CommandName="Delete" Text="Quitar" ID="btnDel" Runat="server" width="80px" />
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
			<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
		</ASP:DATAGRID><br>
		<table class="main">
			<tbody>
				<tr>
					<td>Item:&nbsp;</td>
					<td><asp:textbox id="txtItem" runat="server" Width="100px" ReadOnly="True"></asp:textbox></td>
					<td>Línea:&nbsp;</td>
					<td><asp:DropDownList id="ddlLineas" runat="server"></asp:DropDownList></td>
					<td>Cantidad:&nbsp;</td>
					<td><asp:TextBox id="txtCantidad" runat="server" Width="60px" text="1"></asp:TextBox></td>
					<td>Precio Final:&nbsp;</td>
					<td><asp:TextBox runat="server" onkeyup="NumericMaskE(this,event)" id="txtPrecioF" Width="90px" /></td>
					<td>Descuento:&nbsp;</td>
					<td><asp:TextBox ID="txtDescuento" Runat="server" onkeyup="NumericMaskE(this,event)" Width="60"></asp:TextBox></td>
				</tr>
				<tr>
					<td colspan="2"><asp:Button id="btnAgregar" onclick="AgregarItem" runat="server" Text="Agregar"></asp:Button></td>
				</tr>
			</tbody>
		</table>
	</P>
</fieldset>
<p>
	<asp:Button id="btnReiniciar" onclick="ReiniciarGrilla" runat="server" Text="Recargar"></asp:Button>
	&nbsp;&nbsp;&nbsp;
	<asp:Button id="btnAceptar" onclick="AceptarModificacion" runat="server" Text="Aceptar"></asp:Button>
	&nbsp;&nbsp;&nbsp;
	<asp:Button id="btnCancelar" onclick="CancelarModificacion" runat="server" Text="Cancelar"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
