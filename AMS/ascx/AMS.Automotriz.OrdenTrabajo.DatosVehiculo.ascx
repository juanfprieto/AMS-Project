<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenesTaller.DatosVehiculo.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.DatosVehiculo" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type = "text/javascript">
    function abrirCatalogoVehiculo() {
    var catVeh = document.getElementById("<%=modelo.ClientID%>");
    ModalDialog(catVeh, "SELECT pcat_codigo,pcat_descripcion CONCAT ' -  Catálogo: ' CONCAT pcat_codigo CONCAT '  -  Vin ' CONCAT pcat_vinbasico FROM pcatalogovehiculo ORDER BY 2", new Array(), 1);
    }
</script>
<table class="filtersIn">
	<tbody>
		<tr>
			<td>
				<fieldset class="infield">
					<legend class="Legends">Datos Generales Vehículo</legend>
					<table class="filtersIn">
						<tbody>
							<tr>
								<td>
									Catálogo Modelo:</td>
								<td align="right">
									<asp:DropDownList id="modelo" AutoPostBack="true" OnSelectedIndexChanged="CambioCatalogo" Enabled="False" runat="server" Width="180px"></asp:DropDownList>
                                    <asp:Image id="imglupaCatalogo" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirCatalogoVehiculo();" Visible="false"></asp:Image>
								</td>
							</tr>
							<tr>
								<td>
									Identificación (VIN):</td>
								<td align="right">
									<asp:TextBox id="identificacion" Enabled="False"  runat="server" MaxLength="18" onkeyup="aMayusculas(this)"  Width="180px"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatorIdentificacion" runat="server" ControlToValidate="identificacion" Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									Motor :</td>
								<td align="right">
									<asp:TextBox id="motor" Enabled="False" runat="server" MaxLength="20" onkeyup="aMayusculas(this)"></asp:TextBox>
									<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="motor" Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									Serie&nbsp; :</td>
								<td align="right">
									<asp:TextBox id="serie" Enabled="False" runat="server" MaxLength="20" onkeyup="aMayusculas(this)"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>
									Año Modelo:</td>
								<td align="right">
									<asp:TextBox id="anoModelo" Enabled="False" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatorAnoModelo" runat="server" ControlToValidate="anoModelo" Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="validatorAnoModelo2" runat="server" ControlToValidate="anoModelo" Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}">*</asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td>
									Kilometraje :</td>
								<td align="right">
									<asp:TextBox id="kilometraje" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha">0</asp:TextBox>
									<asp:RequiredFieldValidator id="validatorKilometraje" runat="server" ControlToValidate="kilometraje" Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="validatorKilometraje2" runat="server" ControlToValidate="kilometraje" Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td>
									Color :</td>
								<td align="right">
									<asp:DropDownList id="color" Enabled="False"  runat="server"></asp:DropDownList>
								</td>
							</tr>
							<tr>
								<td>
									Tipo :</td>
								<td align="right">
									<asp:DropDownList id="tipo" Enabled="False"  runat="server"></asp:DropDownList>
								</td>
							</tr>
							<tr>
								<td>
									Concesionario Vendedor/ Ultimo concesionario:</td>
								<td align="right">
									<asp:TextBox id="consVendedor" Enabled="False" runat="server" onkeyup="aMayusculas(this)" ></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>
									Fecha Compra (AAAA-MM-DD):</td>
								<td align="right">
									<asp:TextBox id="fechaCompra" onkeyup="DateMask(this)" Enabled="False" class="tpequeno" runat="server" ></asp:TextBox>
									<asp:RequiredFieldValidator id="validatorFechaCompra" runat="server" ControlToValidate="fechaCompra" Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="validatorFechaCompra2" runat="server" ControlToValidate="fechaCompra" Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td>
									Kilometraje de Compra :</td>
								<td align="right">
									<asp:TextBox id="kilometrajeCompra" onkeyup="NumericMaskE(this,event)" Enabled="False" class="tpequeno" runat="server" CssClass="AlineacionDerecha">0</asp:TextBox>
									<asp:RequiredFieldValidator id="validatorKilometrajeCompra" runat="server" ControlToValidate="kilometrajeCompra" Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="validatorKilometrajeCompra2" runat="server" ControlToValidate="kilometrajeCompra" Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td>
									<p>
										Código de Encendido ó Radio :
									</p>
								</td>
								<td align="right">
									<asp:TextBox id="codRadio" runat="server"></asp:TextBox>
								</td>
							</tr>
						</tbody>
					</table>
				</fieldset>
			</td>

			<td valign="top">
				<fieldset class="infield">
					<legend class="Legends">Datos Aseguradora</legend>
					<table class="filtersIn">
						<tbody>
							<tr>
								<td>
									Nit :</td>
								<td align="right">
									<asp:TextBox id="nitAseguradora" onclick="ModalDialog(this, 'SELECT MN.mnit_nit AS NIT, MN.mnit_nombres AS RAZON, MN.mnit_apellidos AS SIGLA, MN.mnit_direccion AS DIRECCION, MN.pciu_codigo AS CIUDAD, MN.mnit_telefono AS TELEFONO, MN.mnit_celular AS MOVIL, MN.mnit_email AS EMAIL, MN.mnit_web AS WEBSITE FROM mnit MN, PASEGURADORA PA WHERE PA.MNIT_NIT=MN.MNIT_NIT ORDER BY MN.mnit_nit', new Array());" Enabled="False" runat="server" ReadOnly="True"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>
									Siniestro :</td>
								<td align="right">
									<asp:TextBox id="siniestro" Enabled="False" runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>
									Porcentaje Deducible:</td>
								<td align="right">
									<asp:TextBox id="porcentajeDeducible" onkeyup="NumericMaskE(this,event)" Enabled="False" class="tpequeno" runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>
									Valor Mínimo Deducible :</td>
								<td align="right">
									<asp:TextBox id="valorMinDeducible" onkeyup="NumericMaskE(this,event)" Enabled="False" class="tpequeno" runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>
									Número de Autorización:</td>
								<td align="right">
									<asp:TextBox id="numeroAutorizacionAsegura" Enabled="False" class="tpequeno" runat="server"></asp:TextBox>
								</td>
							</tr>
						</tbody>
					</table>
				</fieldset>

				<fieldset class="infield">
					<legend class="Legends">Datos Compañia Garantía</legend>
					<table class="filtersIn">
						<tbody>
							<tr>
								<td>
									Nit :</td>
								<td align="right">
									<asp:TextBox id="nitCompania" onclick="ModalDialog(this, 'SELECT MN.mnit_nit AS NIT , MN.mnit_nombres AS RAZON, MN.mnit_apellidos AS SIGLA, MN.mnit_direccion AS DIRECCION, MN.pciu_codigo AS CIUDAD, MN.mnit_telefono AS TELEFONO, MN.mnit_celular AS MOVIL, MN.mnit_email AS EMAIL, MN.mnit_web AS WEBSITE FROM mnit MN, PCASAMATRIZ PC WHERE PC.MNIT_NIT=MN.MNIT_NIT ORDER BY MN.mnit_nit', new Array())" Enabled="False" runat="server" ReadOnly="True"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>
									Número de Autorización:</td>
								<td align="right">
									<asp:TextBox id="numeroAutorizacionGarant" Enabled="False" class="tpequeno" runat="server"></asp:TextBox>
								</td>
							</tr>
						</tbody>
					</table>
				</fieldset>
                <fieldset>
					<legend class="Legends">Documentos</legend>
					<asp:DataGrid id="dgrDocumentos" runat="server" cssclass="datagrid" OnItemCommand="dgEvento_Grilla" AutoGenerateColumns="false" ShowFooter="True">
						<FooterStyle  cssclass="footer"></FooterStyle>
						<HeaderStyle  cssclass="header"></HeaderStyle>
						<PagerStyle  cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle  cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle  cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="DOCUMENTO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PDOC_NOMBRE") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtDocVehiCod" onclick="ModalDialog(this,'SELECT pdoc_codigo,pdoc_nombre FROM DBXSCHEMA.pdocumentovehiculo',new Array())" runat="server" ReadOnly="true" Columns="4"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "MVEH_NUMEDOCU") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtDocVehiNum" runat="server" Columns="10"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VENCE (AAAA-MM-DD)">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "MVEH_FECHENTREGA", "{0:yyyy-MM-dd}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtDocVehiVence" onkeyup="DateMask(this);" runat="server" Columns="10"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="EXPEDICION (AAAA-MM-DD)">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "MVEH_FECHINGRESO", "{0:yyyy-MM-dd}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtDocVehiExpe" onkeyup="DateMask(this);" runat="server" Columns="10"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="AGREGAR">
								<ItemTemplate>
									<asp:Button CommandName="QuitarDocumento" Text="Borrar" ID="btnDel" runat="server" class="bpequeno" CausesValidation=false />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button CommandName="AgregarDocumento" Text="Agregar" ID="btnAdd" runat="server" class="bpequeno" CausesValidation=false/>
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>					
					</asp:DataGrid>
                </fieldset>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:Button id="confirmar" onclick="Confirmar" Enabled="true" class="bpequeno" runat="server" text="Validar"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
<asp:Label id="lb" runat="server"></asp:Label>
