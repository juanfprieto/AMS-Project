<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.CapPedCliMayor.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_CapPedCliMayor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>

<table>
	<tr>
		<td>
			<table id="comp" >
				<tbody>
					<tr>
						<td rowspan="2" valign="top">
							<fieldset >
								<legend>Comprobante</legend>
								<p>&nbsp;<asp:label id="tipo" runat="server">Tipo :</asp:label>&nbsp;
									<asp:label id="tipoDocu" runat="server"></asp:label></p>
								<p>Número:
									<asp:textbox id="idPedido" runat="server" class="tpequeno"></asp:textbox></p>
								<br>
							</fieldset>
						</td>
						<td valign="top">
							<fieldset >
								<legend>Fecha</legend>
								<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="fechaPedido" runat="server"></asp:label></p>
							</fieldset>
						</td>
						<td valign="top">
							<fieldset >
								<legend>Observación</legend>
								<asp:textbox id="observacion" runat="server" class="tgrande" TextMode="MultiLine" Height="40"></asp:textbox>
							</fieldset>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<fieldset >
								<legend>Vendedor</legend>
								<table>
									<tr>
										<td>Código:</td>
										<td><asp:textbox id="txtCodVendedor" ondblclick="ModalDialog(this, 'SELECT pven_codigo as codigo,pven_nombre as nombre from pvendedor where pven_vigencia=\'V\' and (tvend_codigo=\'TT\' or tvend_codigo=\'VV\');', new Array());"
												runat="server" ReadOnly="True" class="tpequeno"></asp:textbox></td>
									</tr>
									<tr>
										<td>Nombre:</td>
										<td><asp:textbox id="txtCodVendedora" runat="server" class="tgrande" ReadOnly="True"></asp:textbox></td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
				</tbody>
			</table>
			<table id="comp" >
				<tbody>
					<tr>
						<td valign="top">
							<fieldset >
								<legend>
									Cliente</legend>
								<table>
									<tr>
										<td>Nit Cliente:</td>
										<td><asp:textbox id="nitCliente" onBlur="ActCliente();" ondblclick="ModalDialog(this, 'SELECT mc.mnit_nit as NIT, mn.mnit_nombres concat \' \' concat mn.mnit_apellidos as Nombre from MNIT as mn, MCLIENTE as mc WHERE mn.mnit_nit=mc.mnit_nit and not mc.mcli_cupocred is null', new Array());"
												runat="server" ReadOnly="True"></asp:textbox></td>
									<tr>
									<tr>
										<td>Nombre Cliente:</td>
										<td><asp:textbox id="nitClientea" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></td>
									</tr>
									<tr>
										<td>Dirección:</td>
										<td><asp:textbox id="nitDireccion" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></td>
									</tr>
									<tr>
										<td>Ciudad:</td>
										<td><asp:textbox id="nitCiudad" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></td>
									</tr>
									<tr>
										<td>Telefono:</td>
										<td><asp:textbox id="nitTelefono" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></td>
									</tr>
								</table>
							</fieldset>
						</td>
						<td valign="top">
							<fieldset >
								<legend>
									Estado</legend>
								<table>
									<tr>
										<td>No. Pedido:</td>
										<td><asp:textbox id="txtNoPedido" runat="server" MaxLength="10" class="tpequeno"></asp:textbox></td>
									</tr>
									<tr>
										<td>Cupo:</td>
										<td><asp:TextBox id="txtCupo" class="tpequeno" ReadOnly="True" runat="server"></asp:TextBox></td>
									</tr>
									<tr>
										<td>Saldo Cartera:</td>
										<td><asp:TextBox id="txtSaldoCartera" class="tpequeno" ReadOnly="True" runat="server"></asp:TextBox></td>
									</tr>
									<tr>
										<td>Saldo Cartera Mora:</td>
										<td><asp:TextBox id="txtSaldoCarteraMora" class="tpequeno" ReadOnly="True" runat="server"></asp:TextBox></td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
				</tbody>
			</table>
		</td>
	</tr>
</table>
<p>&nbsp;<asp:datagrid id="dgInserts" runat="server" cssclass="datagrid" AutoGenerateColumns="False" GridLines="Vertical" ShowFooter="True" OnItemDataBound="DgInserts_ItemDataBound" OnItemCommand="DgInserts_AddAndDel" OnEditCommand="DgInserts_Edit"
		OnUpdateCommand="DgInserts_Update" OnCancelCommand="DgInserts_Cancel">
		<FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Catalogo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem,"CATALOGO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox Runat="server" id="catalogo" onClick="ModalDialog(this, 'Select a.pcat_codigo as catalogo, a.pcat_descripcion as descripcion,b.ppre_precio as precio from DBXSCHEMA.PCATALOGOVEHICULO a,DBXSCHEMA.PPRECIOVEHICULO b where a.pcat_codigo=b.pcat_codigo', new Array())"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="catalogoEdit" runat="server" onclick="ModalDialog(this, 'SELECT a.pcat_codigo AS Catalogo, a.pcat_descripcion as descripcion FROM DBXSCHEMA.PCATALOGOVEHICULO a,DBXSCHEMA.PPRECIOVEHICULO b where a.pcat_codigo=b.pcat_codigo', new Array())" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem,"CATALOGO") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Color">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "COLOR") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="colorInsert" runat="server"></asp:DropDownList>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="colorEdit" onclick="ModalDialog(this, 'SELECT pcol_codigo AS CODIGO ,pcol_descripcion AS COLOR FROM pcolor order by pcol_descripcion', new Array())" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "COLOR") %> '>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Color Alterno">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "COLORALTER") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="colorAlterInsert" runat="server"></asp:DropDownList>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="colorAlterEdit" onclick="ModalDialog(this, 'SELECT pcol_codigo AS CODIGO ,pcol_descripcion AS COLOR FROM pcolor order by pcol_descripcion', new Array())" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "COLORALTER") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad Pedida">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CANTPED", "{0:G}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="cantidadPedidaInserts" CssClass="AlineacionDerecha" runat="server" Width="50px"></asp:TextBox>
					<asp:RegularExpressionValidator id="validator1" ASPClass="RegularExpressionValidator" ControlToValidate="cantidadPedidaInserts"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="cantidadPedidaEdit" CssClass="AlineacionDerecha" runat="server" width="50px" Text='<%# DataBinder.Eval(Container.DataItem, "CANTPED", "{0:G}") %>' />
					<asp:RegularExpressionValidator id="validator2" ASPClass="RegularExpressionValidator" ControlToValidate="cantidadPedidaEdit"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Unitario">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALUNIT", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="catalogob" CssClass="AlineacionDerecha" runat="server" Width="100px" ReadOnly=True></asp:TextBox>
					<asp:RegularExpressionValidator id="validator3" ASPClass="RegularExpressionValidator" ControlToValidate="catalogob"
						ValidationExpression="[0-9\,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="valorUnitarioEdit" CssClass="AlineacionDerecha" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "VALUNIT", "{0:N}") %>' ReadOnly=True>
					</asp:TextBox>
					<asp:RegularExpressionValidator id="validator4" ASPClass="RegularExpressionValidator" ControlToValidate="valorUnitarioEdit"
						ValidationExpression="[0-9\,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Fecha Llegada">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "FECHALLEGADA", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox Runat="server" onkeyup="DateMask(this)" ID="txtFechaLlegada" Width="80px"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox Runat="server" onkeyup="DateMask(this)" ID="txtFechaEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FECHALLEGADA", "{0:N}") %>' Width="80px">
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones">
				<ItemTemplate>
					<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" HeaderText="Editar Item" CancelText="Cancelar"
				EditText="Actualizar"></asp:EditCommandColumn>
		</Columns>
	</asp:datagrid>
</p>
<p>Total Pedido :
	<asp:textbox id="totalPedido" runat="server" ReadOnly="True" class="tpequeno">$0</asp:textbox></p>
<p><asp:button id="btnGuardar" onclick="Guardar_Pedido" runat="server" Width="73px" Enabled="False"
		Text="Guardar" Visible="False"></asp:button>&nbsp;<asp:button id="btnEditar" onclick="Editar_Pedido" runat="server" Text="Editar" Visible="False"></asp:button>
</p>
<p><asp:label id="lb" runat="server"></asp:label><asp:hyperlink id="hl1" runat="server" Visible="False" Target="_blank">Descargar Pedido</asp:hyperlink></p>
<script type ="text/javascript" type="text/javascript">
	var txtCliente=document.getElementById('<%=nitCliente.ClientID%>');
	var txtDireccion=document.getElementById('<%=nitDireccion.ClientID%>');	
	var txtCiudad=document.getElementById('<%=nitCiudad.ClientID%>');	
	var txtTelefono=document.getElementById('<%=nitTelefono.ClientID%>');	
	var txtSaldoCartera=document.getElementById('<%=txtSaldoCartera.ClientID%>');	
	var txtSaldoCarteraMora=document.getElementById('<%=txtSaldoCarteraMora.ClientID%>');	
	var txtCupo=document.getElementById('<%=txtCupo.ClientID%>');	
	function ActCliente(){
		AMS_Vehiculos_CapPedCliMayor.TraerCliente(txtCliente.value,TraerCliente_Callback);
	}
	function TraerCliente_Callback(response){
		var respuesta=response.value;
		if(respuesta.Tables[0].Rows.length>0)
		{
			txtDireccion.value=respuesta.Tables[0].Rows[0].DIRECCION;
			txtCiudad.value=respuesta.Tables[0].Rows[0].CIUDAD;
			txtTelefono.value=respuesta.Tables[0].Rows[0].TELEFONO;
			txtSaldoCartera.value=respuesta.Tables[0].Rows[0].SALDO;
			txtSaldoCarteraMora.value=respuesta.Tables[0].Rows[0].SALDOMORA;
			txtCupo.value=respuesta.Tables[0].Rows[0].CUPO;
		}
		else
		{
			txtDireccion.value="";
			txtCiudad.value="";
			txtTelefono.value="";
			txtSaldoCartera.value="";
			txtSaldoCarteraMora.value="";
			txtCupo.value="";
		}
	}
</script>
