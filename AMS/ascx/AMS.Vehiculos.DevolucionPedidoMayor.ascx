<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.DevolucionPedidoMayor.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_DevolucionPedidoMayor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Vehiculos.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
<asp:placeholder id="plcInfoFact" runat="server">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET >
                <LEGEND class="Legends">Datos sobre la 
						Factura de Devolución</LEGEND>
					<TABLE class="main" cellSpacing="10">
						<TR>
							<TD>
								<asp:Label id="Label20" runat="server" forecolor="RoyalBlue">Sede: </asp:Label></TD>
							<TD align="left" colSpan="3">
								<asp:DropDownList id="ddlAlmacen" runat="server" OnSelectedIndexChanged="CambioAlmacen" AutoPostBack="true"></asp:DropDownList></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="lbInfo47" runat="server" forecolor="RoyalBlue">Tipo Devolución : </asp:Label></TD>
							<TD align="right">
								<asp:RadioButtonList id="tipoFacturacion" runat="server" OnSelectedIndexChanged="CambioTipoFacturacion"
									AutoPostBack="true" BorderStyle="None" BackColor="Transparent" RepeatDirection="Horizontal">
									<asp:ListItem Value="A" Selected="True">Automático</asp:ListItem>
									<asp:ListItem Value="M">Manual</asp:ListItem>
								</asp:RadioButtonList></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label19" runat="server" forecolor="RoyalBlue">Fecha Devolución : </asp:Label></TD>
							<TD align="right">
								<asp:TextBox id="txtFechaFac" onkeyup="DateMask(this)" runat="server" class="tpequeno"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="lbInfo44" runat="server" forecolor="RoyalBlue">Prefijo Factura Devolución : </asp:Label></TD>
							<TD align="right">
								<asp:DropDownList id="prefijoFactura" runat="server" OnSelectedIndexChanged="CambioTipoFacturacion"
									AutoPostBack="true"></asp:DropDownList></TD>
							<TD>
								<asp:Label id="lbInfo45" runat="server" forecolor="RoyalBlue">Número Factura Devolución : </asp:Label></TD>
							<TD align="left">
								<asp:TextBox id="numeroFactura" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label1" runat="server" forecolor="RoyalBlue">Código Vendedor : </asp:Label></TD>
							<TD>
								<asp:textbox id="txtCodVendedor" onclick="ModalDialog(this, 'SELECT pven_codigo as codigo,pven_nombre as nombre from pvendedor where pven_vigencia=\'V\' and (tvend_codigo=\'TT\' or tvend_codigo=\'VV\');', new Array());"
									runat="server" class="tpequeno" ReadOnly="True"></asp:textbox></TD>
							<TD>
								<asp:Label id="Label5" runat="server" forecolor="RoyalBlue">Nombre Vendedor : </asp:Label></TD>
							<TD>
								<asp:textbox id="txtCodVendedora" runat="server" class="tgrande" ReadOnly="True"></asp:textbox></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
	<asp:LinkButton id="btnSiguiente1" onclick="MostrarInfoCliente" runat="server" Text="Siguiente"></asp:LinkButton>
</asp:placeholder>
<asp:placeholder id="plcInfoCliente" runat="server">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Factura</LEGEND>
					<TABLE>
						<TR>
							<TD>
								<asp:Label id="Label2" runat="server" forecolor="RoyalBlue">Prefijo Factura Devolución : </asp:Label></TD>
							<TD align="right"><%=ViewState["PREF_FACTURA"]%></TD>
							<TD>
								<asp:Label id="Label3" runat="server" forecolor="RoyalBlue">Número de Factura Devolución : </asp:Label></TD>
							<TD align="right"><%=ViewState["NUM_FACTURA"]%></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label7" runat="server" forecolor="RoyalBlue">Codigo Vendedor : </asp:Label></TD>
							<TD align="right"><%=ViewState["COD_VENDEDOR"]%></TD>
							<TD>
								<asp:Label id="Label9" runat="server" forecolor="RoyalBlue">Nombre Vendedor : </asp:Label></TD>
							<TD align="right"><%=ViewState["NOM_VENDEDOR"]%></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<TABLE id="comp">
					<TR>
						<TD vAlign="top">
							<FIELDSET><LEGEND>Cliente</LEGEND>
								<TABLE>
									<TR>
										<TD>Nit Cliente:</TD>
										<TD>
											<asp:textbox id="nitCliente" onclick="ModalDialog(this, 'SELECT mc.mnit_nit as NIT, mn.mnit_nombres concat \' \' concat mn.mnit_apellidos as Nombre from MNIT as mn, MCLIENTE as mc, MPEDIDOVEHICULOCLIENTEMAYOR MP WHERE mn.mnit_nit=mc.mnit_nit and not mc.mcli_cupocred is null AND MN.MNIT_NIT = MP.MNIT_NIT AND MP.TEST_TIPOESTA IN (20,30)', new Array());ActCliente();"
												runat="server" ReadOnly="True"></asp:textbox></TD>
									<TR>
									<TR>
										<TD>Nombre Cliente:</TD>
										<TD>
											<asp:textbox id="nitClientea" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD>Dirección:</TD>
										<TD>
											<asp:textbox id="nitDireccion" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD>Ciudad:</TD>
										<TD>
											<asp:textbox id="nitCiudad" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD>Teléfono:</TD>
										<TD>
											<asp:textbox id="nitTelefono" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></TD>
									</TR>
								</TABLE>
							</FIELDSET>
						</TD>
						<TD vAlign="top">
							<FIELDSET><LEGEND>Estado</LEGEND>
								<TABLE>
									<TR>
										<TD>Cupo:</TD>
										<TD>
											<asp:TextBox id="txtCupo" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox></TD>
									</TR>
									<TR>
										<TD>Saldo Cartera:</TD>
										<TD>
											<asp:TextBox id="txtSaldoCartera" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox></TD>
									</TR>
									<tr>
										<td>Saldo Cartera Mora:</td>
										<td><asp:TextBox id="txtSaldoCarteraMora" class="tpequeno" ReadOnly="True" runat="server"></asp:TextBox></td>
									</tr>
								</TABLE>
							</FIELDSET>
						</TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
		<TR>
			<TD>
				<TABLE>
					<TR>
						<TD>
							<asp:Label id="lbInfo46" runat="server">Observaciones : </asp:Label></TD>
						<TD>
							<asp:TextBox id="observaciones" runat="server" class="amediano" TextMode="MultiLine"></asp:TextBox></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
	</TABLE>
<asp:LinkButton id="btnAnterior2" onclick="MostrarInfoFactura" runat="server" Text="Anterior"></asp:LinkButton>&nbsp;&nbsp; 
<asp:LinkButton id="btnSiguiente2" onclick="MostrarInfoFacturaD" runat="server" Text="Siguiente"></asp:LinkButton>
</asp:placeholder>
<asp:placeholder id="plcFacturaDev" runat="server">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Factura</LEGEND>
					<TABLE>
						<TR>
							<TD>
								<asp:Label id="Label4" runat="server" forecolor="RoyalBlue">Prefijo Factura Devolución : </asp:Label></TD>
							<TD align="right"><%=ViewState["PREF_FACTURA"]%></TD>
							<TD>
								<asp:Label id="Label6" runat="server" forecolor="RoyalBlue">Número de Factura Devolución : </asp:Label></TD>
							<TD align="right"><%=ViewState["NUM_FACTURA"]%></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label8" runat="server" forecolor="RoyalBlue">Codigo Vendedor : </asp:Label></TD>
							<TD align="right"><%=ViewState["COD_VENDEDOR"]%></TD>
							<TD>
								<asp:Label id="Label10" runat="server" forecolor="RoyalBlue">Nombre Vendedor : </asp:Label></TD>
							<TD align="right"><%=ViewState["NOM_VENDEDOR"]%></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label13" runat="server" forecolor="RoyalBlue">NIT Cliente : </asp:Label></TD>
							<TD align="right"><%=ViewState["NIT_CLIENTE"]%></TD>
							<TD>
								<asp:Label id="Label14" runat="server" forecolor="RoyalBlue">Nombre Cliente : </asp:Label></TD>
							<TD align="right"><%=ViewState["NOM_CLIENTE"]%></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<TABLE>
					<TR>
						<TD vAlign="top">
							<FIELDSET><LEGEND>Factura a Devolver</LEGEND>
								<TABLE>
								<TR>
									<TD>
										<asp:Label id="Label21" runat="server" forecolor="RoyalBlue">Prefijo Factura Devolución : </asp:Label></TD>
									<TD align="right">
										<asp:DropDownList id="ddlFacturaD" runat="server" OnSelectedIndexChanged="CambioTipoFacturacionD"
											AutoPostBack="true"></asp:DropDownList></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="Label22" runat="server" forecolor="RoyalBlue">Número de Factura Devolución : </asp:Label></TD>
									<TD align="left">
										<asp:DropDownList id="ddlNumFacturaD" runat="server"></asp:DropDownList>	
									</TD>
								</TR>
								</TABLE>
							</fieldset>
						</td>
					</tr>
				</TABLE>
			</TD>
		</TR>
	</table>
	<asp:LinkButton id="lnkAnteriorA" onclick="MostrarInfoCliente" runat="server" Text="Anterior"></asp:LinkButton>&nbsp;
	<asp:LinkButton id="linkSiguienteA" onclick="MostrarInfoVehiculos" runat="server" Text="Siguiente"></asp:LinkButton>
</asp:PlaceHolder>
<asp:placeholder id="plcVehiculos" runat="server">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Factura</LEGEND>
					<TABLE>
						<TR>
							<TD>
								<asp:Label id="Label11" runat="server" forecolor="RoyalBlue">Prefijo Factura Devolución : </asp:Label></TD>
							<TD align="right"><%=ViewState["PREF_FACTURA"]%></TD>
							<TD>
								<asp:Label id="Label12" runat="server" forecolor="RoyalBlue">Número de Factura Devolución : </asp:Label></TD>
							<TD align="right"><%=ViewState["NUM_FACTURA"]%></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label15" runat="server" forecolor="RoyalBlue">Codigo Vendedor : </asp:Label></TD>
							<TD align="right"><%=ViewState["COD_VENDEDOR"]%></TD>
							<TD>
								<asp:Label id="Label16" runat="server" forecolor="RoyalBlue">Nombre Vendedor : </asp:Label></TD>
							<TD align="right"><%=ViewState["NOM_VENDEDOR"]%></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label17" runat="server" forecolor="RoyalBlue">NIT Cliente : </asp:Label></TD>
							<TD align="right"><%=ViewState["NIT_CLIENTE"]%></TD>
							<TD>
								<asp:Label id="Label18" runat="server" forecolor="RoyalBlue">Nombre Cliente : </asp:Label></TD>
							<TD align="right"><%=ViewState["NOM_CLIENTE"]%></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<P>&nbsp;
					<asp:datagrid id="dgrVehiculos" runat="server" cssclass="datagrid"
						AutoGenerateColumns="False" GridLines="Vertical" ShowFooter="True">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="">
								<ItemTemplate>
									<asp:CheckBox ID="chkFacturar" Runat="server" AutoPostBack=True OnCheckedChanged="DgInserts_Check" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"USADO")) %>'>
									</asp:CheckBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Factura">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem,"MVEH_FACTURA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Pedido">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem,"MVEH_PEDIDO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cat.">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem,"PCAT_CODIGO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VIN">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem,"MCAT_VIN") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Motor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem,"MCAT_MOTOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Color">
								<ItemTemplate>
									<TABLE width="100%" height="100%">
										<TR height="100%">
											<td width="100%" bgcolor='#<%# DataBinder.Eval(Container.DataItem,"PCOL_CODRGBVEH") %>' onclick="alert('<%# DataBinder.Eval(Container.DataItem,"PCOL_NOMBRE") %> (<%# DataBinder.Eval(Container.DataItem,"PCOL_CODIGO") %>)');"> &nbsp;</td>
										</TR>
									</TABLE>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Año<br>Modelo">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem,"MCAT_ANOMODE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Val.<br>Veh.">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PRECIO", "${0:#,##0}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Val.<br>Desc.">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DESCUENTO", "${0:#,##0}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="IVA">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "IVA", "${0:#,##0}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="TOTAL">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TOTAL", "${0:#,##0}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></P>
			</TD>
		</TR>
		<TR>
			<TD vAlign="bottom">
				<FIELDSET><LEGEND class="Legends">Totales</LEGEND>
					<TABLE>
						<TR>
							<TD>Total Vehículos:</TD>
							<TD>
								<asp:TextBox id="txtTotalVehiculos" runat="server" class="tpequeno"
									ReadOnly="True" Text="0"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>Total Descuentos:</TD>
							<TD>
								<asp:TextBox id="txtTotalDescuentos" runat="server" class="tpequeno"
									ReadOnly="True" Text="0"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>Total IVA:</TD>
							<TD>
								<asp:TextBox id="txtTotalIVA" runat="server" class="tpequeno" ReadOnly="True"
									Text="0"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>Subtotal:</TD>
							<TD>
								<asp:TextBox id="txtSubtotal" runat="server" class="tpequeno" ReadOnly="True"
									Text="0"></asp:TextBox></TD>
						</TR>
					</TABLE>
				</FIELDSET>
				<FIELDSET><LEGEND class="Legends">Adicionales</LEGEND>
					<TABLE>
						<TR>
							<TD>Valor Retenciones:</TD>
							<TD>
								<asp:TextBox id="txtRetenciones" onkeyup="NumericMaskE(this,event);Totales();" runat="server" class="tpequeno" Text="0"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>Valor Fletes:</TD>
							<TD>
								<asp:TextBox id="txtFletes" onkeyup="NumericMaskE(this,event);Totales();" runat="server" class="tpequeno" Text="0"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>IVA Fletes:</TD>
							<TD>
								<asp:DropDownList id="ddlIVAFletes" Runat="server" onchange="Totales();"></asp:DropDownList></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Devolver:</LEGEND>
					<TABLE>
						<TR>
							<TD>TOTAL:&nbsp;&nbsp;</TD>
							<TD>&nbsp;
								<asp:TextBox id="txtTotal" runat="server" class="tpequeno" ReadOnly="True"
									Text="0"></asp:TextBox>&nbsp;</TD>
							<TD>&nbsp;&nbsp;
								<asp:button id="btnFacturar" onclick="Facturar" runat="server" class="tpequeno" Text="Devolver"></asp:button></TD>
						</TR>
						<TR>
							<TD colSpan="3">
								<asp:Label id="lblAdvertencia" runat="server" ForeColor="Red"></asp:Label></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
	<asp:LinkButton id="btnAnterior4" onclick="MostrarInfoFacturaD" runat="server" Text="Anterior"></asp:LinkButton>
</asp:placeholder>
<p><asp:label id="lb" runat="server"></asp:label></p>

<script type ="text/javascript" type="text/javascript">
	var txtCliente=document.getElementById('<%=nitCliente.ClientID%>');
	var txtDireccion=document.getElementById('<%=nitDireccion.ClientID%>');	
	var txtCiudad=document.getElementById('<%=nitCiudad.ClientID%>');	
	var txtTelefono=document.getElementById('<%=nitTelefono.ClientID%>');	
	var txtSaldoCartera=document.getElementById('<%=txtSaldoCartera.ClientID%>');	
	var txtSaldoCarteraMora=document.getElementById('<%=txtSaldoCarteraMora.ClientID%>');	
	var txtCupo=document.getElementById('<%=txtCupo.ClientID%>');
	var txtSubtotal=document.getElementById('<%=txtSubtotal.ClientID%>');
	var txtRetenciones=document.getElementById('<%=txtRetenciones.ClientID%>');
	var txtFletes=document.getElementById('<%=txtFletes.ClientID%>');
	var ddlIVAFletes=document.getElementById('<%=ddlIVAFletes.ClientID%>');
	var txtTotal=document.getElementById('<%=txtTotal.ClientID%>');
	function ActCliente(){
		AMS_Vehiculos_DevolucionPedidoMayor.TraerCliente(txtCliente.value,TraerCliente_Callback);
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
	function Totales(){
		var subtotal=0;
		var retenciones=0;
		var ivaFletes=0;
		var fletes=0;
		try{subtotal=parseInt(txtSubtotal.value.replace(/\,/g,''));}catch(e){subtotal=0;};
		if(isNaN(subtotal))subtotal=0;
		try{retenciones=parseInt(txtRetenciones.value.replace(/\,/g,''));}catch(e){retenciones=0;};
		if(isNaN(retenciones))retenciones=0;
		try{ivaFletes=parseInt(ddlIVAFletes.value.replace(/\,/g,''));}catch(e){ivaFletes=0;};
		try{fletes=parseInt(txtFletes.value.replace(/\,/g,''));
			fletes+=((ivaFletes*fletes)/100);}catch(e){fletes=0;};
		if(isNaN(fletes))fletes=0;
		txtTotal.value=formatoValor(subtotal+retenciones+fletes);
	}
</script>
