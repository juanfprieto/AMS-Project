<%@ Control AutoEventWireup="True" CodeBehind="AMS.Vehiculos.PedidoClientesFormOtros Elementos de Venta ulario.ascx.cs"
    Inherits="AMS.Vehiculos.PedidoClientesFormulario" Language="c#" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Vehiculos.Tools.js"></script>
<º1 type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></   º1    
    º
    script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
<script type="text/javascript">
function abrirEmergente(obj) 
{
    var nit = document.getElementById('_ctl1_' + obj);
    ModalDialog(nit, 'SELECT NIT.mnit_nit AS NIT, Nombre AS NOMBRE FROM Vmnit NIT WHERE NIT.mnit_nit NOT IN (SELECT PNI.pnital_nittaller FROM pnittaller PNI) AND NIT.mnit_nit NOT IN (SELECT CEM.mnit_nit FROM cempresa CEM) order by NOMBRE', new Array(), 1);
    //ModalDialog(nit, '**NITS_CLIENTE', new Array(), 1);
}

function clienteAlterno()
{
    document.getElementById("<%=datosClienteAlterno.ClientID%>").value = document.getElementById("<%=datosCliente.ClientID%>").value;
}

function clienteSolicitante(datosCliente)
{
    document.getElementById("<%=datosClienteSolicita.ClientID%>").value = document.getElementById("<%=datosCliente.ClientID%>").value;
}

function mostrarDialogo() 
{
    var catalogo = document.getElementById("<%=catalogoVehiculo.ClientID%>");
    ModalDialog(catalogo, '**VEHICULOS_CATALOGOSNUEVOS', new Array());
}

</script>

<asp:placeholder id="plInfoPed" runat="server">
    <p> 
	    <fieldset>
            <legend >Datos del Pedido</legend>
	        <table class="filtersIn" > 
		        <tr>
		            <td>
                        Prefijo del Pedido :<br />
                        <asp:DropDownList id="prefijoDocumento" runat="server" onChange="cambioTipoDocumento(this);" ></asp:DropDownList>
                    </td>
		            <td style="width:250px;">
                        Número de Pedido : <br />
                        <asp:TextBox id="numeroPedido" class="tpequeno" runat="server" ReadOnly="False"></asp:TextBox>
                        <asp:RegularExpressionValidator id="rev1" runat="server" ControlToValidate="numeroPedido" ErrorMessage="No es un número" ValidationExpression="\d+" Display="Dynamic">*</asp:RegularExpressionValidator>
                        <asp:Label ID="lblCotizacion" runat="server" Visible="false" Text="" style="color: Green;"></asp:Label>
                    </td>
                </tr>
		        <tr>
                    <td>
                        Tipo de Vehículo :<br /> 
                        <asp:DropDownList id="tipoVehiculo" class="dpequeno" runat="server" OnSelectedIndexChanged="Cambio_Tipo_Vehiculo" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td style="width:250px;">
                        Tipo de Servicio :<br /> 
                        <asp:DropDownList id="ddlServicio" class="dpequeno" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Catálogo Vehículo : <br />
                        <asp:DropDownList id="catalogoVehiculo" runat="server" OnSelectedIndexChanged="Cambio_Tipo_Catalogo" AutoPostBack="true" style="width: 92%;"></asp:DropDownList>
                        <asp:Image id="imgLupaCatalogo" runat="server" ImageUrl="../img/AMS.Search.png" onClick="mostrarDialogo();" Visible="false"></asp:Image>
                    </td>
		            <td style="width:250px;">
                        Año Modelo :<br /> 
                        <asp:DropDownList id="anoModelo" class="dpequeno" runat="server" enabled="false" AutoPostBack="true" OnSelectedIndexChanged="cambioAno"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblOpcionV" runat="server" Text="Opcion Vehiculo : " Visible="false"></asp:Label> <br />
                        <asp:DropDownList id="ddlOpcionVeh" runat="server" OnSelectedIndexChanged="Cambio_OpcionVehiculo" AutoPostBack="true" Visible="false"></asp:DropDownList>
                    </td>
                </tr>
		        <tr>
		            <td>
			            Color Primario :<br /> 
                        <asp:DropDownList id="colorPrimario" runat="server"></asp:DropDownList>
                    </td>
		            <td style="width:250px;">
			            Color Opcional :<br /> 
                        <asp:DropDownList id="colorOpcional" runat="server" Width="215px"></asp:DropDownList>
                    </td>
                </tr>
		        <tr>
		            <td>
			            Almacén (Sala): <br />
                        <asp:DropDownList id="almacen" runat="server" OnSelectedIndexChanged="Cambio_Almacen" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
		        <tr>
		            <td>
                        Vendedor : <br />
                        <asp:DropDownList id="vendedor" runat="server"></asp:DropDownList>
                    </td>
		            <td style="width:250px;">
                        Precio de Venta : <br />
                        <asp:TextBox id="txtPrecio" class="tpequeno" runat="server" ReadOnly="False" Enabled="true" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
		                <asp:Label id="disp" Runat="server" Text="Disponibilidad" CssClass="PunteroMano"></asp:Label>
                    </td>
		        </tr>
		        <tr>
                    <td>
                        Medio o Clase de Contacto: : <br />
                        <asp:DropDownList id="claseVenta" runat="server"></asp:DropDownList>
                    </td>
                    <td style="width:250px;">
                        Tipo de Venta : <br />
                        <asp:DropDownList id="tipoVenta" class="dpequeno" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
		            <td>
                        Fecha : <img onmouseover="fecha1.style.visibility='visible'" src="../img/AMS.Icon.Calendar.gif" style="border:1px" alt=""/> 
			            <div id="fecha1" onmouseover="fecha1.style.visibility='visible'" onmouseout="fecha1.style.visibility='hidden'"
			            style="VISIBILITY: hidden; WIDTH: 120px; POSITION: absolute" >
			           
                            <asp:calendar  BackColor="Beige" id="fecha" runat="server"></asp:calendar>
                        
                        </div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
		            <td style="width:250px;">
                        
                          <%--  <IMG  onmouseover="fecha2.style.visibility='visible'" 
			                src="../img/AMS.Icon.Calendar.gif" border:1px> 
			                <div id=fecha2 onmouseover="fecha2.style.visibility='visible'" onmouseout="fecha2.style.visibility='hidden'"
			                style="VISIBILITY: hidden; WIDTH: inherit;  POSITION: absolute"   onmouseout="fecha2.style.visibility='hidden'">
                                <asp:calendar BackColor=Beige id=fechaEntrega runat="server"></asp:Calendar>
                    
                        </div>--%>
                        <asp:PlaceHolder id="PlfechaEntrega" runat="server">
                            <asp:Label id="lbFecha2" Runat="server" > Fecha Aproximada de Entrega :</asp:Label>
                            <img onmouseover="fecha2.style.visibility='visible'" src="../img/AMS.Icon.Calendar.gif" style="border:1px"> 
			                <div id="fecha2" onmouseover="fecha2.style.visibility='visible'" onmouseout="fecha2.style.visibility='hidden'"
			                    style="VISIBILITY: hidden; WIDTH: 120px; POSITION: absolute" >
			           
                                <asp:calendar  BackColor="Beige" id="fechaEntrega" runat="server"></asp:calendar>
                            </div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </asp:PlaceHolder>
                        
                    </td>
                </tr>
            </table>
        </fieldset> 
    </p>
    <p>
	    <fieldset>
            <legend >Datos del Cliente a Facturar</legend>
	        <table class="filtersIn" >
		        <tr>
		            <td>
                        Número de Identificación del Cliente:<br /> 
                        <asp:TextBox id="datosCliente" class="tmediano"         ondblclick="ModalDialog(this,'**NITS_CLIENTE',new Array(),1)" onblur="Cargar_Nombre(this);VerificarCliente(this);" runat="server" ></asp:TextBox>
                        <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('datosCliente');"></asp:Image>
                    </td>
		            <td>
                        Nombre del Cliente a Facturar :<br /> 
                        <asp:TextBox id="datosClientea" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        E-mail del Cliente :<br /> 
                        <asp:TextBox id="txtEmail" runat="server" ></asp:TextBox>
                    </td>
                </tr>
		        <tr>
		            <td>
                        Número de Identificación Cliente Alterno :<br /> 
                        <asp:TextBox id="datosClienteAlterno"  class="tmediano" onClick="clienteAlterno();" ondblclick="ModalDialog(this,'**NITS_CLIENTE',new Array(),1)" onblur="Cargar_Nombrea(this);" runat="server" ></asp:TextBox>
                        <asp:Image id="imglupa2" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('datosClienteAlterno');"></asp:Image>
                    </td>
		            <td>
                        Nombre del Cliente Alterno a Facturar:<br /> 
                        <asp:TextBox id="datosClienteAlternoa" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        E-mail del Cliente Alterno:<br /> 
                        <asp:TextBox id="txtEmaila" runat="server" ></asp:TextBox>
                    </td>
                </tr>
		        <tr>
		            <td>
                        Número de Identificación del Cliente Solicitante :<br /> 
                        <asp:TextBox id="datosClienteSolicita" class="tmediano" onClick="clienteSolicitante();" ondblclick="ModalDialog(this,'**NITS_CLIENTE',new Array(),1)" onblur="Cargar_Nombreb(this);" runat="server" ></asp:TextBox>
                        <asp:Image id="imglupa3" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('datosClienteSolicita');"></asp:Image>
                    </td>
		            <td>
                        Nombre del Cliente :<br /> 
                        <asp:TextBox id="datosClienteSolicitaa" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        E-mail del Cliente Solicitante:<br /> 
                        <asp:TextBox id="txtEmails" runat="server" ></asp:TextBox>
                    </td>
                </tr>
	        </table>
        </fieldset> 
    </p>
    <P><br />
        <asp:LinkButton id="btnSiguiente1" onclick="ConfirmarDatosIniciales" onClientClick="espera();" runat="server" ForeColor="RoyalBlue" style="font-size:16px;">
            Siguiente
        </asp:LinkButton>
        
    </P>
</asp:placeholder>

<asp:placeholder id="plInfoVent" runat="server">
    <fieldset>
        <legend>Datos Sobre la Venta</legend>
        <table class="filtersIn">
            <tr>
                <td>
                    Valor Vehículo : <br />
                    <asp:TextBox id="valorVehiculo" runat="server" ReadOnly="true" class="tpequeno" CssClass="AlineacionDerecha"></asp:TextBox>
                    <input id="hdvalorVehiculo" type="hidden" runat="server" /> 
                </td>
                <td>
                    Valor Base : <br />
                    <asp:TextBox id="valorBase" runat="server" ReadOnly="true"  class="tpequeno" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
                <td>
                    Valor Impuestos : <br />
                    <asp:TextBox id="valorIva" runat="server" ReadOnly="true"  class="tpequeno" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Valor Descuento : <br />
                    <asp:TextBox id="valorDescuento" runat="server"  class="tpequeno" Text="" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
                <td colspan="2">
                    Record Descuentos: <br />
                    <asp:TextBox id="recordDescuento" ReadOnly="true"  class="tpequeno" Runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Valor Vehículo con Descuento :<br /> 
                    <asp:TextBox id="valorVehiculoDescuento" runat="server" ReadOnly="true" Width= class="tpequeno" CssClass="AlineacionDerecha"/>
                </td>
                <td>
                    Valor Base con Descuento :<br /> 
                    <asp:TextBox id="valorBaseDescuento" runat="server" ReadOnly="true"  class="tpequeno" CssClass="AlineacionDerecha"/>
                </td>
                <td>
                    Valor Impuestos con Descuento :<br /> 
                    <asp:TextBox id="valorIvaDescuento" runat="server" ReadOnly="true"  class="tpequeno" CssClass="AlineacionDerecha"/>
                </td>
            </tr>
            <tr>
                <td>
                    Total Valor Vehículo :<br /> 
                    <asp:TextBox id="totalValorVehiculo" runat="server" ReadOnly="true"  class="tpequeno" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                    Valor Oficial Vehículo :<br /> 
                    <asp:Label id="lbPrecioOficial" Runat="server" style="font-weight: bold;"></asp:Label>
                </td>
            </tr>
        </table>
        <p style="FONT-WEIGHT: bold; FONT-STYLE: italic" align="center">Otros 
        Elementos de Venta 
        </p>
        <p>
            <asp:DataGrid id="grillaElementos" runat="server" cssclass="datagrid" OnItemCommand="dgEvento_Grilla" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" OnItemDataBound="dgAccesorioBound">
				<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="CODIGO">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:textbox id="obesequioTextBox"  onclick="ModalDialog(this,'**VEHICULOS_ITEMSVENTAVEHICULOS',new Array())" runat="server" ReadOnly="true"></asp:textbox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="DESCRIPCION">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:textbox id="obesequioTextBoxa" onclick="ModalDialog(this,'**VEHICULOS_ITEMSVENTAVEHICULOS',new Array())" runat="server" ReadOnly="true"></asp:textbox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="COSTO">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "COSTO", "{0:C}") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:textbox id="obesequioTextBoxb" runat="server" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)"></asp:textbox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="PORCENTAJE IVA">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "IVA", "{0:N}%") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:DropDownList id="ddlIVA" runat="server"></asp:DropDownList>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="AGREGAR">
						<ItemTemplate>
							<asp:Button CommandName="QuitarObsequios" Text="Borrar" ID="btnDel" runat="server" Width="55" />
						</ItemTemplate>
						<FooterTemplate>
							<asp:Button CommandName="AgregarObsequios" Text="Agregar" ID="btnAdd" runat="server" Width="55" />
						</FooterTemplate>
					</asp:TemplateColumn>
				</Columns>
				</asp:DataGrid>
        </p>
        <table class="filtersIn">
            <tr>
                <td>Costo Otros Elementos Venta : </td>
                <td align="right">
                    <asp:TextBox id="costoOtrosElementos" runat="server" ReadOnly="True" Text="$0" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Descripción Elementos Obsequiados : </td>
                <td align="right">
                  <%--  FICAR EL CAMPO obsequios a varchar (300)--%>
                    <asp:TextBox id="descripcionObsequios" runat="server" CssClass="AlineacionDerecha" maxlength="299" TextMode="MultiLine" Height="100px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbOpcion" runat="server" Text="Detalle del vehículo" ></asp:Label> <br />
                    <asp:DropDownList id="ddlOpciVehiDetalle" runat="server" class="dmediano" ></asp:DropDownList>
                </td>
                <td>Costo Elementos de Obsequio : </td>
                <td align="right">
                    <asp:TextBox id="costoObsequios" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Valor Total de Venta : </td>
                <td align="right">
                    <asp:TextBox id="totalVenta" runat="server" ReadOnly="True" Text="$0" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
                
            </tr>
        </table>
    </fieldset>

    <p><br />
    <asp:LinkButton id="btnAnterior2" onclick="VolverDatosIniciales" onClientClick="espera();" runat="server" ForeColor="RoyalBlue" style="font-size:16px;">Anterior</asp:LinkButton>&nbsp;&nbsp; 
    <asp:LinkButton id="btnSiguiente2" onclick="ConfirmarDatosVenta" onClientClick="espera();" runat="server" ForeColor="RoyalBlue" style="font-size:16px;">Siguiente</asp:LinkButton>
    </p>
</asp:placeholder>

<asp:placeholder id="plInfoPago" runat="server">
    <fieldset>
        <legend >Valor de los Pagos</legend>
        <table>
            <tr>
                <td>
                    Efectivo :<br />
                    <asp:TextBox id="pagoEfectivo" onkeyup="NumericMaskE(this,event);" runat="server" class="tpequeno" ></asp:TextBox>
                </td>
                <td>
                    <asp:Label id="lbPagoFinanciera" runat="server" Text="Valor Financiado :" Visible="true"></asp:Label> <br>
                    <asp:TextBox id="pagoFinanciera" onkeyup="NumericMaskE(this,event);" runat="server" class="tpequeno" ></asp:TextBox>
                </td>
                <td>
                    <asp:label id="lbNombFinanciera" runat="server" Text="Nombre de la Financiera :" Visible="true"></asp:label> <br>
                    <asp:DropDownList id="ddlFinanciera" runat="server" ></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Cheques Post-Fechados : <br />
                    <asp:TextBox id="pagoCheques" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno" ></asp:TextBox>
                </td>
                <td>
                    Valor Otra Forma Pago : <br />
                    <asp:TextBox id="valorOtroPago" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno" ></asp:TextBox>
                </td>          
                <td>
                    Otra Forma de Pago : <br />
                    <asp:TextBox id="otroPago" runat="server" class="tpequeno"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Valor Parcial Pagos : <br />
                    <asp:TextBox id="valorParcialPagos" runat="server" ReadOnly="True" class="tpequeno" ></asp:TextBox>
                </td>
                <td>
                    Credito Por:
                    <asp:RadioButtonList id="rblcredito" Runat="server" RepeatDirection="Horizontal">
		                <asp:ListItem Value="Cliente" Selected="True">Cliente</asp:ListItem>
		                <asp:ListItem Value="Empresa">Empresa</asp:ListItem>
	                </asp:RadioButtonList>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <!-- -->
            </tr>
            <tr>
                <td colspan="3">
                    <asp:LinkButton id="lnkConsultarAbonos" onclick="ConsultarAbonos" runat="server" Visible="False">Consultar Abonos</asp:LinkButton>
                </td>
            </tr>
        </table>
        <p>
            <asp:RadioButtonList id="retomaVehiculo" runat="server" OnSelectedIndexChanged="Retoma_Vehiculo" AutoPostBack="True" Width="298px" BackColor="#E0E0E0" BorderColor="Silver" RepeatDirection="Horizontal" Height="5px">
                <asp:ListItem Value="SRV" Selected="True">Sin Retoma Vehículo</asp:ListItem>
                <asp:ListItem Value="RV">Retoma Vehículo</asp:ListItem>
            </asp:RadioButtonList>
        </p>
        <p>
            <asp:PlaceHolder id="controlesRetomaVehiculos" runat="server" Visible="False">
                <table class="filtersIn" >
                    <tr>
                        <td>
                            <asp:DataGrid id="grillaRetoma" runat="server" cssclass="datagrid" OnItemCommand="dgEvento_Grilla_Retoma" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True">
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
							                <asp:textbox id="tipoVehiculoRetoma" onclick="ModalDialog(this, '**VEHICULOS_CATALOGOSUSADOS', new Array())" runat="server" ReadOnly="true" Width="100px"></asp:textbox>
						                </FooterTemplate>
					                </asp:TemplateColumn>
					                <asp:TemplateColumn HeaderText="NUMERO CONTRATO RETOMA">
						                <ItemTemplate>
							                <%# DataBinder.Eval(Container.DataItem, "NUMEROCONTRATO") %>
						                </ItemTemplate>
						                <FooterTemplate>
							                <asp:textbox id="numeroContratoRetoma" runat="server" class="tpequeno"></asp:textbox>
						                </FooterTemplate>
					                </asp:TemplateColumn>
					                <asp:TemplateColumn HeaderText="AÑO DE MODELO">
						                <ItemTemplate>
							                <%# DataBinder.Eval(Container.DataItem, "ANOMODELO") %>
						                </ItemTemplate>
						                <FooterTemplate>
							                <asp:textbox id="anoModeloRetoma" onclick="ModalDialog(this,'SELECT pano_ano FROM pano ORDER BY 1 DESC',new Array())" runat="server" ReadOnly="true" Width="100px"></asp:textbox>
						                </FooterTemplate>
					                </asp:TemplateColumn>
					                <asp:TemplateColumn HeaderText="NÚMERO DE PLACA">
						                <ItemTemplate>
							                <%# DataBinder.Eval(Container.DataItem, "NUMEROPLACA") %>
						                </ItemTemplate>
						                <FooterTemplate>
							                <asp:textbox id="numeroPlacaRetoma" runat="server" class="tpequeno"></asp:textbox>
						                </FooterTemplate>
					                </asp:TemplateColumn>
					                <asp:TemplateColumn HeaderText="Ciudad CUENTA de IMPUESTOS">
						                <ItemTemplate>
							                <%# DataBinder.Eval(Container.DataItem, "CUENTAIMPUESTOS") %>
						                </ItemTemplate>
						                <FooterTemplate>
							                <asp:textbox id="cuentaImpuestos" runat="server" class="tpequeno"></asp:textbox>
						                </FooterTemplate>
					                </asp:TemplateColumn>
					                <asp:TemplateColumn HeaderText="VALOR RECIBIDO">
						                <ItemTemplate>
							                <%# DataBinder.Eval(Container.DataItem, "VALORRECIBIDO", "{0:C}") %>
						                </ItemTemplate>
						                <FooterTemplate>
							                <asp:textbox id="valorRecibidoRetoma" runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)"></asp:textbox>
						                </FooterTemplate>
					                </asp:TemplateColumn>
					                <asp:TemplateColumn HeaderText="AGREGAR">
						                <ItemTemplate>
							                <asp:Button CommandName="QuitarRetoma" Text="Borrar" ID="Button1" runat="server" class="bpequeno" />
						                </ItemTemplate>
						                <FooterTemplate>
							                <asp:Button CommandName="AgregarRetoma" Text="Agregar" ID="Button2" runat="server" class="bpequeno" />
						                </FooterTemplate>
					                </asp:TemplateColumn>
				                </Columns>
			                </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        </p>
	    <P>
	        <table class="filtersIn">
		        <tr>
		            <td>
                        Valor Total de Retoma : 
                        <asp:TextBox id="valorTotalRetoma" class="tpequeno" runat="server" ReadOnly="true" Text="$0" ></asp:TextBox>
                    </td>
		            <td rowspan="4" style="vertical-align: top;">
                        Observaciones : <br />
                        <asp:TextBox id="TextObserv" class="amediano" runat="server" TextMode="MultiLine" MaxLength="300" Height="150px" >Placas terminadas en :</asp:TextBox>
                    </td>
		        </tr>
		        <tr>
		            <td>
                        Valor Total de Pagos : 
                        <asp:TextBox id="valorTotalPagos" class="tpequeno" runat="server" ReadOnly="True" Text="$0" ></asp:TextBox>
                    </td>
		        </tr>
		        <tr>
		            <td>
                        Diferencia Venta : 
                        <asp:TextBox id="diferenciaPagos" class="tpequeno" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
		        <tr>
		            <td colspan="2">
                        <asp:Button id="btnAccion" onclick="EjecutarAccion" runat="server" Text="Crear el Pedido"
		                UseSubmitBehavior="false" onClientClick="espera();"> </asp:Button>
                    </td>
                </tr>
	        </table>
	    </P>
    </fieldset>
    <p>
        <asp:LinkButton id="btnAnterior3" onclick="VolverDatosVenta" onClientClick="espera();" runat="server" ForeColor="RoyalBlue">Anterior</asp:LinkButton>
    </p>
</asp:placeholder>

<p><asp:label id="lb" runat="server"></asp:label></p>
<input id="hdValorbase" type="hidden" runat="server" /> <input id="hdDescuentos" type="hidden" runat="server" /> 

<div id="autorizar" runat="server"  visible="false" class="divHabeas">
    <asp:PlaceHolder id="plcAutorizar" runat="server" Visible="true"></asp:PlaceHolder>
</div>

<script language = "javascript" type="text/javascript">
    $(function () {
        var divAutorizar = "<%=autorizar.ClientID%>";
        $("#" + divAutorizar).draggable();
    });
</script>

<script type ="text/javascript">

function clickOnce(btn, msg)
{
			// Comprobamos si se está haciendo una validación
			if (typeof(Page_ClientValidate) == 'function') 
			{
				// Si se está haciendo una validación, volver si ésta da resultado false
				if (Page_ClientValidate() == false) { return false; }
			}
			
			// Asegurarse de que el botón sea del tipo button, nunca del tipo submit
			if (btn.getAttribute('type') == 'button')
			{
				// El atributo msg es totalmente opcional. 
				// Será el texto que muestre el botón mientras esté deshabilitado
				if (!msg || (msg='undefined')) { msg = 'Grabando pedido...'; }
				
				btn.value = msg;

				// La magia verdadera :D
				btn.disabled = true;
			}
			
			return true;
}

function Cargar_Nombre(obj)
{
	PedidoClientesFormulario.Cargar_Nombre(obj.value, Cargar_Nombre_CallBack);
}

function Cargar_Nombre_CallBack(response)
{
	var respuesta=response.value;
	if(respuesta.Tables[0].Rows.length==0 || respuesta.Tables[1].Rows.length==0)
	{
	    var ced = document.getElementById("<%=datosClientea.ClientID%>");
	    var email = document.getElementById("<%=txtEmail.ClientID%>");
	    ced.value = '';
	    email.value = '';
	}
	else
	{
	    var nombre = document.getElementById("<%=datosClientea.ClientID%>");
	    var email = document.getElementById("<%=txtEmail.ClientID%>");
		if(respuesta.Tables[1].Rows.length!=0)
		{
			if(respuesta.Tables[1].Rows[0].NOMBRE!='')
			{
			    nombre.value = respuesta.Tables[1].Rows[0].NOMBRE;
			    email.value = respuesta.Tables[1].Rows[0].EMAIL;
			    if (email.value == "" || email.value == " ") {
			        email.readOnly = false;
			    }
			    else
			    {
			        email.readOnly = true;
			    }
			}
		}
	}
}

function Cargar_Nombrea(obj)
{
	PedidoClientesFormulario.Cargar_Nombre(obj.value,Cargar_Nombrea_CallBack);
}

function Cargar_Nombrea_CallBack(response)
{
	var respuesta=response.value;
	if(respuesta.Tables[0].Rows.length==0 || respuesta.Tables[1].Rows.length==0)
	{
	    var ced = document.getElementById("<%=datosClienteAlternoa.ClientID%>");
	    var emailA = document.getElementById("<%=txtEmail.ClientID%>");
	    ced.value = '';
	    emailA = '';
	}
	else
	{
	    var nombre = document.getElementById("<%=datosClienteAlternoa.ClientID%>");
	    var emailA = document.getElementById("<%=txtEmaila.ClientID%>");
		if(respuesta.Tables[1].Rows.length!=0)
		{
			if(respuesta.Tables[1].Rows[0].NOMBRE!='')
			{
			    nombre.value = respuesta.Tables[1].Rows[0].NOMBRE;
			    emailA.value = respuesta.Tables[1].Rows[0].EMAIL;
			    if (emailA.value == "" || emailA.value == " ") {
			        emailA.readOnly = false;
			    }
			    else {
			        emailA.readOnly = true;
			    }
			}
		}
	}
}

function Cargar_Nombreb(obj)
{
	PedidoClientesFormulario.Cargar_Nombre(obj.value,Cargar_Nombreb_CallBack);
}

function Cargar_Nombreb_CallBack(response)
{
	var respuesta=response.value;
	if(respuesta.Tables[0].Rows.length==0 || respuesta.Tables[1].Rows.length==0)
	{
	    var ced = document.getElementById("<%=datosClienteSolicitaa.ClientID%>");
	    var emailS = document.getElementById("<%=txtEmail.ClientID%>");
	    ced.value = '';
	    emailS.value = '';
	}
	else
	{
	    var nombre = document.getElementById("<%=datosClienteSolicitaa.ClientID%>");
	    var emailS = document.getElementById("<%=txtEmails.ClientID%>");
		if(respuesta.Tables[1].Rows.length!=0)
		{
			if(respuesta.Tables[1].Rows[0].NOMBRE!='')
			{
			    nombre.value = respuesta.Tables[1].Rows[0].NOMBRE;
			    emailS.value = respuesta.Tables[1].Rows[0].EMAIL;
			    if (emailS.value == "" || emailS.value == " ")
			    {
                    emailS.readOnly = false;
			    }
			    else {
			        emailS.readOnly = true;
			    }
			}
		}
	}
}

function RecalculoValorVehiculo(objValBasVeh, objTotalVehiculo, objValIvaVeh, iva, objValorTotalVehiculo, objHdValorTotal, objvalorDescuento, objOtrosElementos, objTotalVenta) {

	if (objTotalVehiculo.value.length == 0)
		objTotalVehiculo.value = "$";
	imp     = objTotalVehiculo.value;
	iva2    = "1." + iva;
	var stringTotalVehiculo = EliminarComas(objTotalVehiculo.value);
	stringTotalVehiculo     = stringTotalVehiculo.substring(1, stringTotalVehiculo.length);
	var TotalVehiculo  = Math.round(parseFloat(stringTotalVehiculo),0);
	resultado          = Math.round(TotalVehiculo / iva2,0);
	objValBasVeh.value = resultado;
	objValIvaVeh.value = Math.round(TotalVehiculo - resultado,0);
	ApplyNumericMask(objValBasVeh);
	if (objValIvaVeh != 0) ApplyNumericMask(objValIvaVeh);
	objValBasVeh.value = "$" + objValBasVeh.value;
	objValIvaVeh.value = "$" + objValIvaVeh.value;
	objHdValorTotal.value = TotalVehiculo;
	objvalorDescuento.value = '0';
	ApplyNumericMask(objHdValorTotal);
	objHdValorTotal.value = "$" + objHdValorTotal.value;
	objValorTotalVehiculo.value = objHdValorTotal.value;
	CalculoTotalVenta(objTotalVehiculo, objOtrosElementos, objTotalVenta);
	objTotalVehiculo.value = stringTotalVehiculo;
	ApplyNumericMask(objTotalVehiculo);
	if (objTotalVehiculo.value.substring(1,0)!='$') objTotalVehiculo.value = '$' + objTotalVehiculo.value;
}

function cambioTipoDocumento(obj) {
    PedidoClientesFormulario.Cambio_Tipo_Documento(obj.value, Cambio_Tipo_Documento_CallBack);
}

function Cambio_Tipo_Documento_CallBack(response) {
    var respuesta = response.value;
    var numPedido = document.getElementById("<%=numeroPedido.ClientID%>");
    numPedido.value = respuesta;
}

function VerificarCliente(obj) {
        PedidoClientesFormulario.Verificar_Cliente(obj.value, Verificar_Cliente_CallBack);
}

function Verificar_Cliente_CallBack(response) {
    var respuesta = response.value;
    var contactos = "";
    var dtsCliente = document.getElementById("<%=datosCliente.ClientID%>");
    var dtsClientea = document.getElementById("<%=datosClientea.ClientID%>");
    var dtsClienteAlt = document.getElementById("<%=datosClienteAlterno.ClientID%>");
    var dtsClienteAlta = document.getElementById("<%=datosClienteAlternoa.ClientID%>");
    var dtsClienteSol = document.getElementById("<%=datosClienteSolicita.ClientID%>");
    var dtsClienteSola = document.getElementById("<%=datosClienteSolicitaa.ClientID%>");
    var ddlVend = document.getElementById("<%=vendedor.ClientID%>");
    
    

    if (respuesta.Tables[1].Rows.length > 0) {
        if (respuesta.Tables[2].Rows[0]["CVEH_COTICLIEEXIS"] == 'S') {
            if (respuesta.Tables[1].Rows[0].PVEN_CODIGO != ddlVend.value) {
                dtsCliente.value = '';
                dtsClientea.value = '';

                dtsClienteAlt.value = '';
                dtsClienteAlta.value = '';
                dtsClienteSol.value = '';
                dtsClienteSola.value = '';

                contactos = "Se han encontrado contactos registrados por este cliente: \n\n";
                for (var i = 0; i < respuesta.Tables[1].Rows.length; i++) {
                    contactos = contactos + respuesta.Tables[1].Rows[i].CONTACTOS + '\n\n'
                }
                alert(contactos);
                alert("Este cliente ya tiene una(s) cotizacion(es) previa(s) con otro vendedor..! ");
            }
            else {
                contactos = "Se han encontrado contactos registrados por este cliente: \n\n";
                for (var i = 0; i < respuesta.Tables[1].Rows.length; i++) {
                    contactos = contactos + respuesta.Tables[1].Rows[i].CONTACTOS + '\n\n'
                }
                email.value = respuesta.Tables[0].Rows[0]["EMAIL"];
                alert(contactos);
            }
            }      
        else {
            contactos = "Se han encontrado contactos registrados por este cliente: \n\n";
            for (var i = 0; i < respuesta.Tables[1].Rows.length; i++) {
                contactos = contactos + respuesta.Tables[1].Rows[i].CONTACTOS + '\n\n'
            }           
            alert(contactos);
            alert("Este cliente ya tiene una(s) cotizacion(es) previa(s) con otro vendedor..! ");
        }
    }
}
</script>
 
