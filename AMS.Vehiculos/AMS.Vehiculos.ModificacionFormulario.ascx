<%@ Control Language="c#" codebehind="AMS.Vehiculos.ModificacionFormulario.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.ModificacionFormulario" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>

<fieldset>
    <center><legend>Información Vehículo</legend><b>Catálogo Vehículo :</b>
		    <asp:label id="cataVehiculo" runat="server"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
		    <b>VIN Vehículo :</b>
		    <asp:label id="vinVehiculo" runat="server"></asp:label><br /><br />
    </center>
	<table style="WIDTH: 80%" >
		<tbody>
            <tr>
                <td>
                    <b>Catálogo :</b><asp:dropdownlist id="ddlCataVehi" runat="server" cssClass="dmediano" enabled="false"></asp:dropdownlist>
                </td>
                <td colspan="2">
                    <b>VIN :</b><asp:textbox id="txtVinVehiculo" runat="server" class="tmediano" MaxLength="17" Enabled="false"></asp:textbox>
                    &nbsp;&nbsp;&nbsp;<asp:RadioButton id="chkVin" runat="server" AutoPostBack="true" OnCheckedChanged="cambioVin" Text="Cambiar únicamente V.I.N" style="height: 15px;width: 30px;" />
                    <br /> <br /> <br />
                </td>
            </tr>
			<tr>
				<td><b>Nº Inventario :</b>
					<asp:label id="numeInventario" runat="server"></asp:label></td>
				<td><b>Estado Vehículo :</b>
					<asp:dropdownlist id="estadoVehiculo" runat="server" class="dpequeno" Enabled="False"></asp:dropdownlist></td>
				<td><b>Nº Recepción :</b>
					<asp:textbox id="numeRecepcion" runat="server" class="tpequeno"></asp:textbox>
                    <asp:requiredfieldvalidator id="validatorNumeRecepcion" runat="server" ControlToValidate="numeRecepcion" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator>
                    <asp:regularexpressionvalidator id="validatornumeRecepcion2" runat="server" ControlToValidate="numeRecepcion" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:regularexpressionvalidator></td>
			</tr>
			<tr>
				<td><b>Pref. Pedido :</b>
					<asp:label id="prefPedido" runat="server"></asp:label></td>
				<td><b>Número Pedido :</b>
					<asp:label id="numePedido" runat="server"></asp:label></td>
				<td><b>Fecha Recepción :</b>
		    			<asp:textbox id="fechRecepcion" runat="server" class="tpequeno" onkeyup="DateMask(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorFechRecepcion" runat="server" ControlToValidate="fechRecepcion" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator>
                        <asp:regularexpressionvalidator id="validatorfechRecepcion2" runat="server" ControlToValidate="fechRecepcion" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator>
				</td>
			</tr>
			<tr>
				<td><b>Fecha Disponibilidad :</b>
					<asp:textbox id="fechDisponible" runat="server" class="tpequeno" onkeyup="DateMask(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorFechDisponible" runat="server" ControlToValidate="fechDisponible" Display="Dynamic"
						Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator>
                        <asp:regularexpressionvalidator id="validatorfechDisponible2" runat="server" ControlToValidate="fechDisponible" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator>
				</td>
				<td><b>Kilometraje Recepción :</b>
					<asp:textbox id="kiloRecepcion" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:textbox>
                    <asp:requiredfieldvalidator id="validatorKiloRecepcion" runat="server" ControlToValidate="kiloRecepcion" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator>
                        <asp:regularexpressionvalidator id="validatorkiloRecepcion2" runat="server" ControlToValidate="kiloRecepcion" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:regularexpressionvalidator>
				</td>
				<td><b>Ubicación :</b>
					<asp:dropdownlist id="ubicacion" runat="server" class="dmediano" Enabled="false"></asp:dropdownlist></td>
			</tr>
			<tr>
				<td><b>Tipo Vehículo :</b>
					<asp:dropdownlist id="tipoVehiculo" runat="server" class="dpequeno"></asp:dropdownlist></td>
				<td><b>Nº Manifiesto :</b>
					<asp:textbox id="numeManifiesto" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorNumeManifiesto" runat="server" ControlToValidate="numeManifiesto" Display="Dynamic"
						Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator></td>
				<td><b>Fecha Manifiesto :</b>
					<asp:textbox id="fechManifiesto" runat="server" class="tpequeno" onkeyup="DateMask(this)"></asp:textbox>
                    <asp:requiredfieldvalidator id="validatorFechManifiesto" runat="server" ControlToValidate="fechManifiesto" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator>
                    <asp:regularexpressionvalidator id="validatorfechManifiesto2" runat="server" ControlToValidate="fechManifiesto" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator>
				</td>
			</tr>
			<tr>
				<td><b>Nº Aduana :</b>
					<asp:textbox id="numeAduana" runat="server" class="tpequeno"></asp:textbox>
                    <asp:requiredfieldvalidator id="validatorNumeAduana" runat="server" ControlToValidate="numeAduana" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator>
				</td>
				<td><b>Nº D.O. :</b>
					<asp:textbox id="numeDO" runat="server" class="tpequeno"></asp:textbox></td>
				<td><b>Nº Levante :</b>
					<asp:textbox id="numeLevante" runat="server" class="tpequeno"></asp:textbox>
                    <asp:requiredfieldvalidator id="validatorNumeLevante" runat="server" ControlToValidate="numeLevante" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator>
				</td>
			</tr>
			<tr>
				<td><b>Valor Gastos :</b>
					<asp:textbox id="valorGastos" onkeyup="NumericMaskE(this,event)" runat="server" Width="106px"></asp:textbox>
                    <asp:regularexpressionvalidator id="validatorValorGastos2" runat="server" ControlToValidate="valorGastos" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:regularexpressionvalidator>
				</td>
				<td><b>Tipo de Compra :</b>
					<asp:dropdownlist id="tipoCompra" runat="server" class="dpequeno"></asp:dropdownlist></td>
				<td><b>Nit Propietario :</b>
					<asp:textbox id="nitPropietario" 
                    onclick="ModalDialog(this,'SELECT MN.mnit_nit AS CEDULA , MN.mnit_nombres AS NOMBRES, MN.mnit_apellidos AS APELLIDOS, MN.mnit_direccion AS DIRECCION, MN.pciu_codigo AS CIUDAD, MN.mnit_telefono AS TELEFONO, MN.mnit_celular AS MOVIL, MN.mnit_email AS EMAIL, MN.mnit_web AS WEBSITE FROM mnit MN ORDER BY MN.mnit_nit', new Array(),1)" runat="server" Width="107px" ReadOnly="True"></asp:textbox>
                    <asp:requiredfieldvalidator id="validatornitPropietario" runat="server" ControlToValidate="nitPropietario" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:requiredfieldvalidator>
				</td>
			</tr>
			<tr>
				<td><b>Fecha Entrega :</b> 
                    <asp:textbox id="fechEntrega" runat="server" class="tpequeno" onkeyup="DateMask(this)"></asp:textbox>
                    <asp:regularexpressionvalidator id="validatorfechEntrega" runat="server" ControlToValidate="fechEntrega" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator>
				</td>
				<td><asp:label id="lblPrecioPublico" runat="server"></asp:label>
					<asp:textbox id="txtPrecioPublico" onkeyup="NumericMaskE(this,event)" runat="server" autopostback="true" class="tpequeno"></asp:textbox>
				</td>
            </tr>
            <tr>
                <td>
                    <b>Placa :</b> <asp:TextBox ID="txtPlaca" runat="server" MaxLength="10" class="tpequeno"></asp:TextBox>
                </td>
                <td>
                    <b>Fecha Matrícula inicial :</b> <asp:TextBox ID="txtFechaMatriInicial" runat="server" class="tpequeno" onkeyup="DateMask(this)"></asp:TextBox>
                </td>
                <%--<td>
                    <b>Núm. Matricula Inicial :</b><asp:TextBox ID="txtNumMatriInicial" runat="server" class="tpequeno"></asp:TextBox>
                </td>--%>
            </tr>
		</tbody>
	</table>
</fieldset>
<p><asp:button id="btnGrabar" onclick="Grabar_Cambio" runat="server" Text="Grabar"></asp:button></p>
<p><asp:label id="lb" runat="server"></asp:label></p>
