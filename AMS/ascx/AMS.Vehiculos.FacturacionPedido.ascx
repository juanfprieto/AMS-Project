<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.FacturacionPedido.ascx.cs" Inherits="AMS.Vehiculos.FacturacionPedido" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Vehiculos.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/jquery"></script>

<script type ="text/javascript">
    function Facturar_Pedido()
    {
	    document.getElementById('botonAccion').disabled = "locked";
	    document.getElementById('<%=oprimioBotonAccion.ClientID%>').value = "true";
		
	    document.getElementById('Form').submit();
    }
    function habilitaAlmacen()
    {
        if (document.getElementById('<%=chkAlmacen.ClientID%>').checked)
        {
            document.getElementById('<%=ddlAlmacen.ClientID%>').disabled = false;
        } else {
            document.getElementById("<%=ddlAlmacen.ClientID%>").disabled = true;
        }
    }
    function CerrarDiv1()
    {
        $('#<%=divMensaje.ClientID%>').hide('slow');
    }
</script>

<asp:placeholder id="plInfoFact" runat="server">
	<fieldset>
		<legend class="Legends">Datos sobre la Factura</LEGEND>
		<table class="filtersIn" >
			<tr>
				<td>
					<asp:Label id="lbInfo47" runat="server" forecolor="RoyalBlue">Tipo Facturación : </asp:Label>
					<asp:RadioButtonList id="tipoFacturacion" runat="server" BorderStyle="None" BackColor="Transparent" RepeatDirection="Horizontal" OnSelectedIndexChanged="CambioTipoFacturacion" AutoPostBack="true" >
						<asp:ListItem Value="A" Selected="True">Automatico</asp:ListItem>
						<asp:ListItem Value="M">Manual</asp:ListItem>
					</asp:RadioButtonList>
                </td>
                <td></td>
			</tr>
			<tr>
				<td>
					<asp:Label id="lbInfo44" runat="server" forecolor="RoyalBlue">Prefijo Factura : </asp:Label>
                    <br><asp:DropDownList id="prefijoFactura" runat="server" OnSelectedIndexChanged="CambioPrefijoFactura" AutoPostBack="true"></asp:DropDownList>
                </TD>
				<TD><asp:Label id="lbInfo45" runat="server" forecolor="RoyalBlue">Número de Factura : </asp:Label>
                    <br><asp:TextBox id="numeroFactura" runat="server" class="tpequeno" Enabled="false" ></asp:TextBox>
                </TD>
				<TD>
					<asp:Label id="lbFechaFac" runat="server" forecolor="RoyalBlue">Fecha Factura :</asp:Label></TD>
		        <TD align="right">
			        <asp:TextBox id="txtFechaFac" onkeyup="DateMask(this)" runat="server" ReadOnly="false" CssClass="tpequeno"></asp:TextBox></TD>
		        <td></td>
            </TR>
		
            <TR>
				<TD>
					<asp:Label id="Label4" runat="server" forecolor="RoyalBlue">Prefijo Factura Administrativa: </asp:Label>
                    <br ><asp:DropDownList id="ddlPrefijoFacAcce" runat="server" OnSelectedIndexChanged="CambioPrefijoFacturaAcces" AutoPostBack="true"></asp:DropDownList></TD>
				<TD>
					<asp:Label id="Label5" runat="server" forecolor="RoyalBlue">Número de Factura Administrativa: </asp:Label>
				    <br><asp:TextBox id="txtNumeroFacAcce" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
                </TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo46" runat="server" forecolor="RoyalBlue">Observaciones : </asp:Label>
				    <br><asp:TextBox id="observaciones" runat="server" Width="380px" Height="110px" TextMode="MultiLine"></asp:TextBox>
                </TD>
                <td></td>
			</TR>
		</table>
	</fieldset>
	<p>
    <asp:LinkButton id="btnSiguiente1" onclick="MostrarInfoPedVeh" runat="server" Text="Siguiente"></asp:LinkButton>
    </p>
</asp:placeholder>

<asp:placeholder id="plInfoPedVeh" runat="server">
<p>
	<FIELDSET>
		<LEGEND class="Legends">Datos Sobre el Pedido</LEGEND>
		<TABLE class="filtersIn">
			<TR>
				<TD>
					<asp:Label id="lbInfo1" runat="server" forecolor="RoyalBlue">Prefijo Pedido :</asp:Label>
                    <br><asp:Label id="prefijoPedido" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo2" runat="server" forecolor="RoyalBlue">Número del Pedido :</asp:Label>
					<br><asp:Label id="numeroPedido" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo3" runat="server" forecolor="RoyalBlue">Tipo de Vehiculo :</asp:Label>
					<br><asp:Label id="catalogo" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo4" runat="server" forecolor="RoyalBlue">Color Primario :</asp:Label>
					<br><asp:Label id="colorPrimario" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo5" runat="server" forecolor="RoyalBlue">Color Opcional :</asp:Label>
					<br><asp:Label id="colorOpcional" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo6" runat="server" forecolor="RoyalBlue">Clase Vehiculo :</asp:Label>
					<br><asp:Label id="claseVehiculo" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo7" runat="server" forecolor="RoyalBlue">Año Modelo :</asp:Label>
					<br><asp:Label id="anoModelo" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo8" runat="server" forecolor="RoyalBlue">Fecha de Pedido :</asp:Label>
					<br><asp:Label id="fechaPedido" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo9" runat="server" forecolor="RoyalBlue">Fecha de Entrega :</asp:Label>
					<br><asp:Label id="fechaEntrega" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo10" runat="server" forecolor="RoyalBlue">Vendedor :</asp:Label>
					<br><asp:DropDownList id="vendedor" runat="server"></asp:DropDownList></TD>
				<TD>
					<asp:Label id="lbInfo11" runat="server" forecolor="RoyalBlue">Almacen de Venta :</asp:Label>
					<br><asp:DropDownList ID="ddlAlmacen" runat="server" width="100%" ></asp:DropDownList>
                        
                    <%--<asp:Label id="sedeVenta" runat="server"></asp:Label>--%>

				</TD>
                <td>
                    <asp:checkbox id="chkAlmacen" text=" Cambiar Almacen" runat="server" onchange="habilitaAlmacen();" />
                </td>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo12" runat="server" forecolor="RoyalBlue">Nit Cliente Principal
                            :</asp:Label>&nbsp;
					<asp:Label id="nitPrincipal" runat="server"></asp:Label></TD>
				<TD colSpan="2">
					<asp:Label id="lbInfo13" runat="server" forecolor="RoyalBlue">Nombre Cliente Principal
                            :</asp:Label>&nbsp;
					<asp:Label id="nombrePrincipal" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo14" runat="server" forecolor="RoyalBlue">Nit Cliente Alterno
                            :</asp:Label>&nbsp;
					<asp:Label id="nitAlterno" runat="server"></asp:Label></TD>
				<TD colSpan="2">
					<asp:Label id="lbInfo16" runat="server" forecolor="RoyalBlue">Nombre Cliente Alterno
                            :</asp:Label>&nbsp;
					<asp:Label id="nombreAlterno" runat="server"></asp:Label></TD>
			</TR>
		</TABLE>
	</FIELDSET>
</p>
<p> 			
	<FIELDSET>
		<LEGEND class="Legends">Datos del Vehículo Asignado</LEGEND>
		<TABLE class="filtersIn" >
			<TR>
				<TD>
					<asp:Label id="lbInfo17" runat="server" forecolor="RoyalBlue">Número de Inventario:</asp:Label>
					<br><asp:Label id="numeroInventario" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo18" runat="server" forecolor="RoyalBlue">Tipo de Vehículo :</asp:Label>
					<br><asp:Label id="catalogoAsignado" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo19" runat="server" forecolor="RoyalBlue">VIN :</asp:Label>
					<br><asp:Label id="vin" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo20" runat="server" forecolor="RoyalBlue">Motor : </asp:Label>
					<br><asp:Label id="numeroMotor" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo21" runat="server" forecolor="RoyalBlue">Serie :</asp:Label>
					<br><asp:Label id="numeroSerie" runat="server"></asp:Label>
				</TD>
				<TD>
					<asp:Label id="lbInfo22" runat="server" forecolor="RoyalBlue">Año de Modelo :</asp:Label>
					<br><asp:Label id="anoModeloAsignado" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo23" runat="server" forecolor="RoyalBlue">Tipo de Servicio :</asp:Label>
					<br><asp:Label id="tipoServicio" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo24" runat="server" forecolor="RoyalBlue">Color :</asp:Label>
					<br><asp:Label id="colorAsignado" runat="server"></asp:Label></TD>
				<TD>
					<asp:Label id="lbInfo25" runat="server" forecolor="RoyalBlue">Kilometraje :</asp:Label>
					<br><asp:Label id="kilometrajeAsignado" runat="server"></asp:Label></TD>
			</TR>
		</TABLE>
	</FIELDSET>
</p>
<p>
    <asp:LinkButton id="btnAnterior2" onclick="RetornarInfoFac" runat="server" Text="Anterior"></asp:LinkButton>&nbsp;&nbsp; 
    <asp:LinkButton id="btnSiguiente2" onclick="MostrarInfoVent" runat="server" Text="Siguiente"></asp:LinkButton>

</p>
</asp:placeholder>
<asp:placeholder id="plInfoVenta" runat="server">

<FIELDSET>
	<LEGEND>Datos sobre la Venta</LEGEND>
	<TABLE class="filtersIn">
		<TR>
			<TD>
				<asp:Label id="lbInfo26" runat="server" forecolor="RoyalBlue">Valor Vehiculo :</asp:Label>
				<br><asp:TextBox id="valorAutomovil" runat="server" class="tpequeno" ReadOnly="true" CssClass="AlineacionDerecha"></asp:TextBox></TD>
			<TD>
				<asp:Label id="lblDescuento" runat="server" forecolor="RoyalBlue">Valor Descuento :</asp:Label>
				<br><asp:TextBox id="valorDescuento" runat="server" class="tpequeno" ReadOnly="true" CssClass="AlineacionDerecha"></asp:TextBox></TD>
			<TD>
				<asp:Label id="Label1" runat="server" forecolor="RoyalBlue">Valor Venta :</asp:Label>
				<br><asp:TextBox id="valorVentaVeh" runat="server" class="tpequeno" ReadOnly="true" CssClass="AlineacionDerecha"></asp:TextBox></TD>
        </TR>
		<TR>
			<TD>
				<asp:Label id="Label2" runat="server" forecolor="RoyalBlue">Valor Base :</asp:Label>
				<br><asp:TextBox id="valorBaseVeh" runat="server" class="tpequeno" ReadOnly="true" CssClass="AlineacionDerecha"></asp:TextBox></TD>
            <TD>
				<asp:Label id="lbInfo50" runat="server" forecolor="RoyalBlue">Valor Impuestos :</asp:Label>
				<br><asp:TextBox id="valorIVAautomovil" runat="server" class="tpequeno" ReadOnly="true" CssClass="AlineacionDerecha"></asp:TextBox></TD>
            <TD>
				<asp:Label id="Label3" runat="server" forecolor="RoyalBlue">Total Venta :</asp:Label>
				<br><asp:TextBox id="valorTotalVeh" runat="server" class="tpequeno" ReadOnly="true" CssClass="AlineacionDerecha"></asp:TextBox></TD>
        </TR>
		<TR>
			<TD colspan="2">
				<asp:Label id="lbInfo28" runat="server" forecolor="RoyalBlue">Detalle Obsequios :</asp:Label>
				<br><asp:TextBox id="detalleObsequios" runat="server" class="tmediano"></asp:TextBox>
            </TD>
			<TD>
				<asp:Label id="lbInfo29" runat="server" forecolor="RoyalBlue">Valor Obsequios :</asp:Label>  
				<br><asp:TextBox id="valorObsequios" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno">
                </asp:TextBox>
            </TD>
		</TR>
        </TABLE>
</FIELDSET>
		<TR>
			<TD colSpan="3">
				<P style="FONT-WEIGHT: bold; FONT-STYLE: italic" align=center>
					<asp:Label id="lbInfo30" runat="server" forecolor="RoyalBlue">Otros Elementos de Venta</asp:Label>&nbsp;
				</P>
				<P>
					<asp:DataGrid id="grillaElementos" runat="server" cssclass="datagrid" OnItemCommand="dgEvento_Grilla_Elementos" AutoGenerateColumns="false" ShowFooter="True" OnItemDataBound="dgAccesorioBound" Enabled="true" >
                        <FooterStyle CssClass="footer" ></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="DESCRIPCION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="obesequioTextBox" onclick="ModalDialog(this,'SELECT pite_nombre,pite_costo FROM DBXSCHEMA.pitemventavehiculo',new Array())" runat="server" ReadOnly="true"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="COSTO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "COSTO", "{0:C}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="obesequioTextBoxa" runat="server"  onkeyup="NumericMaskE(this,event)" onfocus="ApplyNumericMask(this)"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PORCENTAJE IVA" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "IVA", "{0:N}%") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList id="ddlIVA" runat="server"></asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FACTURAR" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate > 
                                    <asp:CheckBox id="chkFacturar" Text="" runat="server" Visible="true" />
	                            </ItemTemplate> 
                                <FooterTemplate>
                                </FooterTemplate>
							</asp:TemplateColumn>
                            
						</Columns>
					</asp:DataGrid>
                    <center>
                <asp:Label id="Label8" runat="server" forecolor="Red" Font-Size="Smaller" >
                *Los elementos seleccionados se facturaran por medio de una factura administrativa y NO se descontaran de inventario.
                </asp:Label></center>
                </P>
			</TD>
		</TR>
		<TR>
			<TD colspan="2">
				<asp:Label id="lbInfo31" runat="server" forecolor="RoyalBlue">Valor Otros Elementos:</asp:Label>
				<br><asp:TextBox id="valorElementosVenta" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox></TD>
			<TD>
				<asp:Label id="lbInfo51" runat="server" forecolor="RoyalBlue">Valor IVA Otros Elementos:</asp:Label>
				<br><asp:TextBox id="valorIVAElementosVenta" runat="server" class="tpequeno" ReadOnly="True" ></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="lbInfo32" runat="server" forecolor="RoyalBlue">Valor Total Venta :</asp:Label>
				<br><asp:TextBox id="valorTotalVenta" runat="server" class="tpequeno" ReadOnly="True" ></asp:TextBox></TD>
		</TR>
	
<p>
    <asp:LinkButton id="btnAnterior3" onclick="RetornarInfoPedVeh" runat="server" Text="Anterior"></asp:LinkButton>&nbsp;&nbsp; 
    <asp:LinkButton id="btnSiguiente3" onclick="MostrarInfoPagos" runat="server" Text="Siguiente"></asp:LinkButton>
</p>

</asp:placeholder>
<asp:placeholder id="plInfoPagos" runat="server">

<FIELDSET>
	<LEGEND>Datos sobre el Pago</LEGEND>
    <p>
	<FIELDSET>
		<LEGEND>Abonos Realizados al Pedido</LEGEND>
		<asp:DataGrid id="grillaAnticipos" runat="server" AutoGenerateColumns="false" GridLines="Vertical" cssclass="datagrid">
			<HeaderStyle CssClass="header"></HeaderStyle>
			<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
			<Columns>
				<asp:BoundColumn DataField="PREFIJOANTICIPO" HeaderText="PREFIJO ANTICIPO"></asp:BoundColumn>
				<asp:BoundColumn DataField="NUMEROANTICIPO" HeaderText="NÚMERO ANTICIPO"></asp:BoundColumn>
				<asp:BoundColumn DataField="FECHAANTICIPO" HeaderText="FECHA ANTICIPO"></asp:BoundColumn>
				<asp:BoundColumn DataField="VALORANTICIPO" HeaderText="VALOR DEL ANTICIPO" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:BoundColumn DataField="TIPODOCUMENTO" HeaderText="TIPO DOCUMENTO"></asp:BoundColumn>
			</Columns>
		</asp:DataGrid>
	</FIELDSET>
    </p>
    <div id="divMensaje" runat="server" visible="false" style="position:absolute; top:260px; left:27%; background: #aee4e2; border-style:solid; border-width:3px; width:501px; box-shadow: 4px 7px 13px #888888; padding-bottom: 10px; border-radius: 31px; padding-top: 20px; padding-left: 5px;">Por favor, verfique que todos los recibos de caja que se encuentran aqui relacionados coincidan exactamente con los que estan soportados fisicamente en la carpeta del negocio.
     <asp:Button id="btnCerrar" runat="server" Text="Aceptar" style="margin: auto; display: -webkit-box;" onClientClick="CerrarDiv1(); return false" class="noEspera"></asp:Button>
    </div>
	<TABLE class="filtersIn">
		<TR>
			<TD colSpan="2">
				<asp:Label id="lbInfo33" runat="server" forecolor="RoyalBlue">Valor Anticipos :</asp:Label>
				<br><asp:TextBox id="valorTotalAnticipos" runat="server" class="tpequeno" ReadOnly="True" ></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="lbInfo34" runat="server" forecolor="RoyalBlue">Nit Real Financiera:</asp:Label>
				<br><asp:TextBox id="nitRealFinanciera" Columns="15" runat="server" ReadOnly="True"></asp:TextBox>
                <br><asp:TextBox id="nitRealFinancieraa" runat="server" ReadOnly="True"></asp:TextBox>
				</TD>
			<TD>
				<asp:Label id="lbInfo35" runat="server" forecolor="RoyalBlue" ReadOnly="True">Valor Financiado :</asp:Label>
				<br><asp:TextBox id="valorFinanciado" ReadOnly="True" runat="server" class="tpequeno"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="lbInfo37" runat="server" forecolor="RoyalBlue">Detalle otros Pagos:</asp:Label>
				<br><asp:TextBox id="detalleOtrosPagos" runat="server" ></asp:TextBox></TD>
			<TD>
				<asp:Label id="lbInfo38" runat="server" forecolor="RoyalBlue">Valor otros Pagos :</asp:Label>
				<br><asp:TextBox id="valorOtrosPagos" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:TextBox></TD>
			<TD>
				<asp:CheckBox id="chkPrenda" Text="Con Prenda" Runat="server" Checked="true"></asp:CheckBox></TD>
		</TR>
	</TABLE>
</FIELDSET>
<p>
    <asp:LinkButton id="btnAnterior4" onclick="RetornarInfoVenta" runat="server" Text="Anterior"></asp:LinkButton>&nbsp;&nbsp; 
    <asp:LinkButton id="btnSiguiente4" onclick="MostrarInfoRetoma" runat="server" Text="Siguiente"></asp:LinkButton>
    
</p>
</asp:placeholder>
<asp:placeholder id="plInfoRetoma" runat="server">
<FIELDSET>
	<LEGEND>Automoviles de Retoma</LEGEND>
	
					<asp:DataGrid id="grillaRetoma" runat="server" OnItemCommand="dgEvento_Grilla_Retoma" AutoGenerateColumns="false" GridLines="Vertical" cssclass="datagrid" ShowFooter="True">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="TIPO VEHICULO RETOMA">
								<ItemTemplate>
<%# DataBinder.Eval(Container.DataItem, "TIPOVEHICULO") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="tipoVehiculoRetoma" onclick="ModalDialog(this,'SELECT pcat_codigo codigo, \'[\' concat pcat_codigo concat \'] - [\' concat pcat_descripcion concat \']\' descripcion FROM dbxschema.pcatalogovehiculo order by pcat_descripcion,pcat_codigo',new Array())" runat="server" ReadOnly="true" ></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO CONTRATO RETOMA">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMEROCONTRATO") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="numeroContratoRetoma" runat="server" ></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="AÑO DE MODELO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "ANOMODELO") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="anoModeloRetoma" onclick="ModalDialog(this,'SELECT pano_ano ano FROM dbxschema.pano ORDER BY pano_ano DESC',new Array())" runat="server" ReadOnly="true" ></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO DE PLACA">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMEROPLACA") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="numeroPlacaRetoma" runat="server" ></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="COLOR VEHICULO">
								<ItemTemplate>
									<asp:DropDownList id="colorVehiculoRetoma" runat="server"></asp:DropDownList>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CUENTA DE IMPUESTOS EN">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CUENTAIMPUESTOS") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="cuentaImpuestos" runat="server" ></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR RECIBIDO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORRECIBIDO", "{0:C}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="valorRecibidoRetoma" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)" runat="server" ></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="AGREGAR">
								<ItemTemplate>
									<asp:Button CommandName="QuitarRetoma" Text="Borrar" ID="btnDel" runat="server" Width="60" />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button CommandName="AgregarRetoma" Text="Agregar" ID="btnAdd" runat="server" Width="60" />
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
<FIELDSET>
    <p>
	<table class="filtersIn">
		<tbody>
			<TR>
				<TD>
					<asp:Label id="lbInfo40" runat="server" forecolor="RoyalBlue">Valor Total de Retoma:</asp:Label></TD>
				<TD align="right">
					<asp:TextBox id="valorTotalRetoma" runat="server" ReadOnly="True" CssClass="AlineacionDerecha">$0</asp:TextBox></TD>
                <td></td>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo41" runat="server" forecolor="RoyalBlue">Valor Total Pagos :</asp:Label></TD>
				<TD align="right">
					<asp:TextBox id="valorTotalPagos" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:TextBox></TD>
			    <td></td>
            </TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo43" runat="server" forecolor="RoyalBlue">Valor Venta :</asp:Label></TD>
				<TD align="right">
					<asp:TextBox id="vlrVenta" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:TextBox></TD>
			    <td></td>
            </TR>
			<TR>
				<TD>
					<asp:Label id="lbInfo42" runat="server" forecolor="RoyalBlue">Diferencia de Pagos
                        :</asp:Label></TD>
				<TD align="right">
					<asp:TextBox id="diferencia" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:TextBox></TD>
			    <td></td>
            </TR>
			<TR>
			    <TD>
				    <asp:Label id="LabelInt" runat="server" forecolor="RoyalBlue" Visible="False">Valor Intermediación:</asp:Label></TD>
			    <TD align="right">
				    <asp:TextBox id="TxtInt" runat="server" ReadOnly="False" CssClass="AlineacionDerecha" Visible="False"></asp:TextBox></TD>
		        <td></td>
            </TR>
            
            <TR>
			    <TD>
				    <asp:Label id="lblIva" runat="server" forecolor="RoyalBlue" Visible="False">Iva :</asp:Label></TD>
			    <TD align="right">
				    <asp:TextBox id="txtIva" runat="server" ReadOnly="False" CssClass="AlineacionDerecha" Visible="False"></asp:TextBox></TD>
		        <td></td>
            </TR>
            <TR>
			    <TD>
				    <asp:Label id="lblImpo" runat="server" forecolor="RoyalBlue" Visible="False">Impoconsumo :</asp:Label></TD>
			    <TD align="right">
				    <asp:TextBox id="txtImpo" runat="server" ReadOnly="False" CssClass="AlineacionDerecha" Visible="False"></asp:TextBox></TD>
		        <td></td>
            </TR>

		    <TR>
			    <TD>
				    <asp:Label id="LabelIvaInt" runat="server" forecolor="RoyalBlue" Visible="False">Iva :</asp:Label></TD>
			    <TD align="right">
				    <asp:TextBox id="TxtIvaInt" runat="server" ReadOnly="False" CssClass="AlineacionDerecha" Visible="False"></asp:TextBox></TD>
		        <td></td>
            </TR>

             <TR>
			    <TD>
				    <asp:Label id="lbelemevent" runat="server" forecolor="RoyalBlue" Visible="True">Otros Elementos de venta :</asp:Label></TD>
			    <TD align="right">
				    <asp:TextBox id="Txtelemevent" runat="server" ReadOnly="True" CssClass="AlineacionDerecha" Visible="True"></asp:TextBox></TD>
		        <td></td>
            </TR>

              <TR>
			    <TD>
				    <asp:Label id="lbivaeleme" runat="server" forecolor="RoyalBlue" Visible="True">Iva Otros Elementos de venta :</asp:Label></TD>
			    <TD align="right">
				    <asp:TextBox id="txtivaeleme" runat="server" ReadOnly="True" CssClass="AlineacionDerecha" Visible="True"></asp:TextBox></TD>
		        <td></td>
            </TR>
		    <TR>
			    <TD colSpan="2">
			        <asp:PlaceHolder runat="server" ID="plcFacturar">
		                <input id="botonAccion" onclick="this.disabled=true;Facturar_Pedido();" 
                        type="button" value="Facturar Vehículo" name="botonAccion" />
                        <INPUT id="oprimioBotonAccion" type="hidden" value="false" name="oprimioBotonAccion" runat="server">
			            <asp:HyperLink id="hl1" runat="server" Visible="False" Target="_blank">Ver Factura</asp:HyperLink>
                        
                    </asp:PlaceHolder>
                </TD>
	        </TR>
            <tr>
                <td colspan="3"><asp:Label ID="lblErrorFact" runat="server"></asp:Label></td>
            </tr>
	    </tbody>
	</table>
    </p>
    <asp:PlaceHolder runat="server" ID="plcNotaCred" Visible="false">
        <table class="filtersIn">
            <TR>
                <td colspan="3">Seleccione una prefijo de nota crédito para retornar el excedente a favor del cliente:</td>
            </tr>
            <tr>
			    <TD>
				    <asp:Label id="lblNota" runat="server" forecolor="RoyalBlue" Visible="true">Nota a favor del Cliente :</asp:Label></TD>
			    <TD align="left" colspan="2">
				    <asp:DropDownList id="ddlNotaCredito" class="dmediano" runat="server" OnSelectedIndexChanged="CambioNotaNumero" AutoPostBack="true" ></asp:DropDownList></TD>
            </tr>
            <tr>
                <TD>
				    <asp:Label id="Label6" runat="server" forecolor="RoyalBlue" Visible="true">Número Nota :</asp:Label></TD>
                <TD align="left">
                    <asp:TextBox id="txtNumeNota" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>
                </TD>
                <td></td>
            </TR>
            <tr>
                <td>
                    <asp:Label id="Label7" runat="server" forecolor="RoyalBlue" Visible="true">Valor devolución :</asp:Label>
                </td>
                <TD align="left">
				    
                    <asp:TextBox id="txtNotaCred" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>
                </TD>
                <td></td>
            </tr>
        </table>
    </asp:PlaceHolder>
</FIELDSET>

<asp:LinkButton id="btnAnterior5" onclick="RetornarInfoPagos" runat="server" Text="Anterior"></asp:LinkButton>
</asp:placeholder>
<p><asp:label id="lb" runat="server"></asp:label></p>