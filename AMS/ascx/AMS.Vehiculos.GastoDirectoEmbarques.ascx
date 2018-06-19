<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.GastoDirectoEmbarques.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_GastoDirectoEmbarques" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script language='javascript' src='../js/AMS.Tools.js'></script>
<table bgcolor="#f2f2f2" border="0">
	<tbody>
		<tr>
			<td>
				<fieldset align="center">
					<legend>
						Información Factura</legend>
					<table class="filstersIn" bgcolor="#f2f2f2" border="0">
						<tbody>
							<tr>
								<td>
									Prefijo Factura :<br>
									<asp:DropDownList id="prefijoFactura" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Documento"
										runat="server"></asp:DropDownList>
								</td>
								<td>
									Número Factura :<br>
									<asp:TextBox id="numeroFactura" class="tpequeno" runat="server" ReadOnly="True"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatornumeroFactura" runat="server" ControlToValidate="numeroFactura" Display="Dynamic"
										Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="validatornumeroFactura2" runat="server" ControlToValidate="numeroFactura" Display="Dynamic"
										Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:RegularExpressionValidator>
								</td>
								<td>
									Nit Proveedor :<br>
									<asp:TextBox id="nitProveedor" onclick="ModalDialog(this, 'SELECT mnit_nit as NIT, mnit_nombres concat \' \' concat mnit_apellidos as Nombre FROM mnit', new Array(),1)"
										class="tpequeno" runat="server" ReadOnly="True"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatornitProveedor" runat="server" ControlToValidate="nitProveedor" Display="Dynamic"
										Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									Prefijo Fact. Proveedor:<br>
									<asp:TextBox id="prefFactProveedor" class="tpequeno" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatorprefFactProveedor" runat="server" ControlToValidate="prefFactProveedor"
										Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
								</td>
								<td>
									Número Fact. Proveedor:<br>
									<asp:TextBox id="numeFactProveedor" class="tpequeno" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="numeFactProveedor"
										Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="numeFactProveedor"
										Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:RegularExpressionValidator>
								</td>
								<td>
									Fecha :<br>
									<asp:TextBox id="fechaFact" class="tpequeno" runat="server" onkeyup="DateMask(this)"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatorfechaFact" runat="server" ControlToValidate="fechaFact" Display="Dynamic"
										Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="validatorfechaFact2" runat="server" ControlToValidate="fechaFact" Display="Dynamic"
										Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td>
									Almacén :<br>
									<asp:DropDownList id="almacen" runat="server"></asp:DropDownList>
								</td>
								<td>
									Fecha Venc. :<br>
									<asp:TextBox id="fechaVencimiento" onkeyup="DateMask(this)" runat="server" class="tpequeno"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatorfechaVencimiento" runat="server" ControlToValidate="fechaVencimiento"
										Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="validatorfechaVencimiento2" runat="server" ControlToValidate="fechaVencimiento"
										Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:RegularExpressionValidator>
								</td>
								<td>
									Valor Factura :<br>
									<asp:TextBox id="valorFactura" onkeyup="NumericMaskE(this,event)" class="tpequeno" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatorvalorFactura" runat="server" ControlToValidate="valorFactura" Display="Dynamic"
										Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="validatorvalorFactura2" runat="server" ControlToValidate="valorFactura" Display="Dynamic"
										Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td rowspan="2">
									Observación :<br>
									<asp:TextBox id="observacion" class="amediano" runat="server" TextMode="MultiLine"></asp:TextBox>
								</td>
								<td>
									Valor IVA :<br>
									<asp:TextBox id="txtIVA" onkeyup="NumericMaskE(this,event)" class="tpequeno" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txtIVA" Display="Dynamic"
										Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="Regularexpressionvalidator2" runat="server" ControlToValidate="txtIVA" Display="Dynamic"
										Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:RegularExpressionValidator>
								</td>
								<td>
									Valor Retenciones :<br>
									<asp:TextBox id="txtRetenciones" onkeyup="NumericMaskE(this,event)" class="tpequeno" runat="server" ReadOnly="True">0</asp:TextBox>
									<asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ControlToValidate="txtRetenciones" Display="Dynamic"
										Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="Regularexpressionvalidator4" runat="server" ControlToValidate="txtRetenciones"
										Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td>
									Valor Total :<br>
									<asp:TextBox id="txtTotal" onkeyup="NumericMaskE(this,event)" class="tpequeno" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="txtTotal" Display="Dynamic"
										Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator id="Regularexpressionvalidator3" runat="server" ControlToValidate="txtTotal" Display="Dynamic"
										Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:RegularExpressionValidator>
								</td>
								<td><asp:Button id="btnSeleccionar" onclick="Seleccionar" runat="server" Text="Seleccionar" CausesValidation="False"></asp:Button></td>
							</tr>
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<br>
<br>
<asp:Panel ID="pnlDetalles" Runat="server" Visible="False">
	<TABLE class="filstersIn" border="1">
		<TR>
			<TD class="SubTitulos" align="center">Embarques Relacionados</TD>
			<TD class="SubTitulos" align="center">Gastos Relacionados</TD>
		</TR>
		<TR>
			<TD>
				<asp:DataGrid id="dgEmbarques" runat="server" cssclass="datagrid"
					AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True"
					OnItemCommand="DgEmbarques_AddAndDel">
					<FooterStyle CssClass="footer"></FooterStyle>
					<HeaderStyle CssClass="header"></HeaderStyle>
					<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
					<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
					<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Secuencia">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"SECUENCIA") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="vehInserts" runat="server" ReadOnly="true" onclick="ModalDialog(this, 'SELECT MEMB_SECUENCIA SECUENCIA, MEMB_NUMEEMBA NUMERO, mlic_valoemba valor FROM MEMBARQUE WHERE PEST_ESTADO<>\'R\' ORDER BY 2',new Array())"
									Width="100px"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Número">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"NUMERO") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="vehInsertsa" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Valor Embarque">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"VALOR", "{0:C}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="vehInsertsb" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Porcentaje">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Valor de Factura">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "VALORFACT" , "{0:C}") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Operaciones">
							<ItemTemplate>
								<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid></TD>
			<TD>
				<asp:DataGrid id="dgGastos" runat="server" cssclass="datagrid" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" OnItemCommand="DgGastos_AddAndDel"
					OnEditCommand="DgGastos_Edit" OnUpdateCommand="DgGastos_Update" OnCancelCommand="DgGastos_Cancel"
					OnItemDataBound="dgGastos_ItemDataBound">
					<FooterStyle CssClass="footer"></FooterStyle>
				    <HeaderStyle CssClass="header"></HeaderStyle>
				    <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
				    <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
				    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
				    <ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Código Gasto">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"CODIGO") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="gastInserts" runat="server" ReadOnly="true" class="tpequeno" onclick="ModalDialog(this,'SELECT pgas_codigo AS CODIGO, CASE WHEN pgas_modenaci = \'S\' THEN pgas_nombre CONCAT \' \' CONCAT \' - Pesos\' ELSE pgas_nombre CONCAT \' \' CONCAT \' - Dolares\' END AS NOMBRE, pgas_modenaci AS NACIONAL FROM pgastodirecto order by codigo', new Array())"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Nombre Gasto">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"NOMBRE") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="gastInsertsa" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Valor Gasto">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"VALOR", "{0:C}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="valorInserts" runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Operaciones">
							<ItemTemplate>
								<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="Button1" Runat="server" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="Button2" Runat="server" />
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid><BR>
				Tasa de Cambio:&nbsp;
				<asp:TextBox id="txtTasa" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD class="SubTitulos" align="center">Retenciones</TD>
			<TD class="SubTitulos" align="center"></TD>
		</TR>
		<TR>
			<TD vAlign="top" align="center">
				<asp:DataGrid id="gridRtns" runat="server" cssclass="datagrid" Visible="True"
					AutoGenerateColumns="False" OnItemDataBound="gridRtns_ItemDataBound" showfooter="True" onItemCommand="gridRtns_Item">
					    <FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="C&#243;digo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
							FooterStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
							</ItemTemplate>
							<FooterTemplate>
								<center>
									<asp:TextBox id="codret" runat="server" ReadOnly="true" class="tpequeno" ToolTip="Haga Click"></asp:TextBox>
								</center>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Porcentaje de Retenci&#243;n (%)" HeaderStyle-HorizontalAlign="Center"
							FooterStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="codretb" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Base">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="base" runat="server" Enabled="true" Text="0" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Valor">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="valor" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0" class="tpequeno" ReadOnly="True"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Agregar" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Button id="remRet" runat="server" CommandName="RemoverRetencion" Text="Remover" CausesValidation="false" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:Button id="agRet" runat="server" CommandName="AgregarRetencion" Text="Agregar" Enabled="true"
									CausesValidation="false" />
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid></TD>
			<TD vAlign="top">
				<FIELDSET><LEGEND>Totales</LEGEND>
					<TABLE class="main">
						<TR>
							<TD>Embarques:
							</TD>
							<TD>
								<asp:TextBox id="tlEmbarques" runat="server" class="tpequeno" readonly="True">0</asp:TextBox></TD>
						</TR>
						<TR>
							<TD>Gastos:
							</TD>
							<TD vAlign="middle">
								<asp:TextBox id="tlGastos" runat="server" class="tpequeno" readonly="true">0</asp:TextBox></TD>
						</TR>
						<TR>
							<TD>Diferencia Gastos - Total Factura:&nbsp;&nbsp;
							</TD>
							<TD>
								<asp:TextBox id="tlDiferencia" runat="server" class="tpequeno" readonly="true">0</asp:TextBox></TD>
						</TR>
						<TR>
							<TD></TD>
							<TD align="right">
								<asp:Button id="btnGrabar" onclick="Realizar_Proceso" runat="server" CausesValidation="False"
									Text="Grabar" onClientclick="espera();"></asp:Button>&nbsp;&nbsp;
								<asp:Button id="btnCancelar" onclick="Cancelar_Proceso" runat="server" CausesValidation="False"
									Text="Cancelar"></asp:Button></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
</asp:Panel>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
<script type ="text/javascript">
	function PorcentajeVal(tPorcentaje,tBase,tTotal){
		var txtT=document.getElementById(tTotal);
		try{
			var prct=parseFloat(document.getElementById(tPorcentaje).value.replace(/\,/g,''));
			var bse=parseFloat(document.getElementById(tBase).value.replace(/\,/g,''));
			var pt=Math.round((prct*bse)/100);
			txtT.value=formatoValor(pt);
		}
		catch(err){
			txtT.value="";
		}
	}
</script>
