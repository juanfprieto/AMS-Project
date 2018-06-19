<%@ Control Language="c#" codebehind="AMS.Vehiculos.ModificacionFormulario.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.ModificacionFormulario" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<table bgColor="#f2f2f2" border="0">
	<tbody>valorGastos
		<tr>
			<td>
				<fieldset align="center">
					<P><legend>Información Vehículo</legend>Cat Vehículo :
						<asp:label id="cataVehiculo" runat="server"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						VIN Vehículo :
						<asp:label id="vinVehiculo" runat="server"></asp:label></P>
					<P>Cat Nuevo:&nbsp;&nbsp;&nbsp; &nbsp;<asp:dropdownlist id="ddlCataVehi" runat="server"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;VIN 
						Nuevo:&nbsp;&nbsp;&nbsp;&nbsp;<asp:textbox id="txtVinVehiculo" runat="server" class="tmediano" MaxLength="17"></asp:textbox></P>
					<table style="WIDTH: 695px" bgColor="#f2f2f2" border="0">
						<tbody>
							<tr>
								<td>Nº Inventario :
									<asp:label id="numeInventario" runat="server"></asp:label></td>
								<td>Estado Vehículo :
									<asp:dropdownlist id="estadoVehiculo" runat="server" class="dpequeno" Enabled="False"></asp:dropdownlist></td>
								<td>Nº Recepción :
									<asp:textbox id="numeRecepcion" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorNumeRecepcion" runat="server" ControlToValidate="numeRecepcion" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
        </asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatornumeRecepcion2" runat="server" ControlToValidate="numeRecepcion" Display="Dynamic"
										Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td>Pref. Pedido :
									<asp:label id="prefPedido" runat="server"></asp:label></td>
								<td>Número Pedido :
									<asp:label id="numePedido" runat="server"></asp:label></td>
								<td>Fecha Recepción :
									<asp:textbox id="fechRecepcion" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorFechRecepcion" runat="server" ControlToValidate="fechRecepcion" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
        </asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorfechRecepcion2" runat="server" ControlToValidate="fechRecepcion" Display="Dynamic"
										Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td>Fecha Disponibilidad :
									<asp:textbox id="fechDisponible" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorFechDisponible" runat="server" ControlToValidate="fechDisponible" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
        </asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorfechDisponible2" runat="server" ControlToValidate="fechDisponible"
										Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator></td>
								<td>Kilometraje Recepción :
									<asp:textbox id="kiloRecepcion" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorKiloRecepcion" runat="server" ControlToValidate="kiloRecepcion" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
        </asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorkiloRecepcion2" runat="server" ControlToValidate="kiloRecepcion" Display="Dynamic"
										Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:regularexpressionvalidator></td>
								<td>Ubicación :
									<asp:dropdownlist id="ubicacion" runat="server" class="dmediano"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td>Tipo Vehículo :
									<asp:dropdownlist id="tipoVehiculo" runat="server" class="dpequeno"></asp:dropdownlist></td>
								<td>Nº Manifiesto :
									<asp:textbox id="numeManifiesto" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorNumeManifiesto" runat="server" ControlToValidate="numeManifiesto" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
        </asp:requiredfieldvalidator></td>
								<td>Fecha Manifiesto :
									<asp:textbox id="fechManifiesto" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorFechManifiesto" runat="server" ControlToValidate="fechManifiesto" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
        </asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorfechManifiesto2" runat="server" ControlToValidate="fechManifiesto"
										Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td>Nº Aduana :
									<asp:textbox id="numeAduana" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorNumeAduana" runat="server" ControlToValidate="numeAduana" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
        </asp:requiredfieldvalidator></td>
								<td>Nº D.O. :
									<asp:textbox id="numeDO" runat="server" class="tpequeno"></asp:textbox></td>
								<td>Nº Levante :
									<asp:textbox id="numeLevante" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorNumeLevante" runat="server" ControlToValidate="numeLevante" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
        </asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td>Valor Gastos :
									<asp:textbox id="valorGastos" onkeyup="NumericMaskE(this,event)" runat="server" Width="106px"></asp:textbox><asp:regularexpressionvalidator id="validatorValorGastos2" runat="server" ControlToValidate="valorGastos" Display="Dynamic"
										Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:regularexpressionvalidator></td>
								<td>Tipo de Compra :
									<asp:dropdownlist id="tipoCompra" runat="server" class="dpequeno"></asp:dropdownlist></td>
								<td>Nit Propietario :
									<asp:textbox id="nitPropietario" onclick="ModalDialog(this,'SELECT MN.mnit_nit AS CEDULA , MN.mnit_nombres AS NOMBRES, MN.mnit_apellidos AS APELLIDOS, MN.mnit_direccion AS DIRECCION, MN.pciu_codigo AS CIUDAD, MN.mnit_telefono AS TELEFONO, MN.mnit_celular AS MOVIL, MN.mnit_email AS EMAIL, MN.mnit_web AS WEBSITE FROM mnit MN ORDER BY MN.mnit_nit', new Array(),1)"
										runat="server" Width="107px" ReadOnly="True"></asp:textbox><asp:requiredfieldvalidator id="validatornitPropietario" runat="server" ControlToValidate="nitPropietario" Display="Dynamic"
										Font-Name="Arial" Font-Size="11">*
                                    </asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td>Fecha Entrega : <asp:textbox id="fechEntrega" runat="server" class="tpequeno"></asp:textbox><asp:regularexpressionvalidator id="validatorfechEntrega" runat="server" ControlToValidate="fechEntrega" Display="Dynamic"
										Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator></td>
							    <td><asp:label id="lblPrecioPublico" runat="server"></asp:label>
									<asp:textbox id="txtPrecioPublico" onkeyup="NumericMaskE(this,event)" runat="server" autopostback="true" class="tpequeno"></asp:textbox></td>
                            </tr>
						</tbody></table>
				</fieldset>
			</td>
		</tr>
	</tbody></table>
<p><asp:button id="btnGrabar" onclick="Grabar_Cambio" runat="server" Text="Grabar"></asp:button></p>
<p><asp:label id="lb" runat="server"></asp:label></p>
