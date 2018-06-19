<%@ Control Language="c#" codebehind="AMS.Automotriz.GarantiasAprobadasFormato.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.GarantiasAprobadasFormato" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript">
function RevisionTbFactura(objValidacion)
{
	if(objValidacion.value == '')
	{
		alert('- Por favor ingrese alguna factura para agregar.');
		return false;
	}
	else
		return true;
}
</script>

				<fieldset>  <!-- HEIGHT: 60px; -->
					<legend>Datos Generales Factura</legend>
					<table class="filtersIn">
						<tbody>
							<tr>
								<td>
									<p>Prefijo :
										<asp:label id="prefFact" runat="server"></asp:label></p>
								</td>
								<td>
									<p>Número :
										<asp:textbox id="numFact" runat="server" class="tpequeno"></asp:textbox></p>
								</td>
							</tr>
							<tr>
								<td>
									<p>Fecha Factura :
										<asp:label id="fechFactura" class="lpequeno" runat="server"></asp:label></p>
								</td>
								<td>
									<p>Nit CSA :
										<asp:textbox id="nitCSAEX" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox></p>
								</td>
							</tr>
						</tbody></table>
				</fieldset>
			
			
				<fieldset > <!--  HEIGHT: 60px; -->
					<legend>Otros Datos</legend>
					<table>
						<tbody>
							<tr>
								<td>Almacen :
									<asp:dropdownlist id="almacen" class="dpequeno" runat="server"></asp:dropdownlist>&nbsp;</td>
								<td>Centro de Costos :
									<asp:dropdownlist id="centCostos" class="dmediano" runat="server"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td>Dias Plazo :
									<asp:textbox id="diasPlazo" runat="server" class="tpequeno"></asp:textbox></td>
								<td>Observación :
									<asp:textbox id="obsrv" runat="server" class="tpequeno" TextMode="MultiLine"></asp:textbox></td>
							</tr>
						</tbody></table>
				</fieldset>
			
		

<p><asp:datagrid id="dgInserts" runat="server" OnCancelCommand="DgInserts_Cancel" OnUpdateCommand="DgInserts_Update"
		OnEditCommand="DgInserts_Edit" OnItemCommand="DgInserts_AddAndDel" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="false" cssclass="datagrid">
		<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Prefijo Factura">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem,"PREFIJO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="nitCSA" runat="server" onclick="ModalDialogEX(this, 'SELECT pdoc_codigo AS Prefijo, cast(mfac_numedocu as varchar(10)) AS Numero, mfac_valofact AS Valor FROM mfacturacliente WHERE mnit_nit=\'+\' AND (mfac_valofact %2b mfac_valoiva %2b mfac_valoflet %2b mfac_valoivaflet - mfac_valorete - mfac_valoabon) > 0 ORDER BY pdoc_codigo, mfac_numedocu', new Array(),'Nit')"
						ReadOnly="true"></asp:TextBox>
					<!--<asp:RequiredFieldValidator id="validatorNitCSA1" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic"
						ControlToValidate="nitCSA">*</asp:RequiredFieldValidator>-->
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="nitCSA" runat="server" onclick="ModalDialogEX(this, 'SELECT pdoc_codigo AS Prefijo, cast(mfac_numedocu as varchar(10)) AS Numero, mfac_valofact AS Valor FROM mfacturacliente WHERE mnit_nit=\'+\' AND (mfac_valofact %2b mfac_valoiva %2b mfac_valoflet %2b mfac_valoivaflet - mfac_valorete - mfac_valoabon) > 0 ORDER BY pdoc_codigo, mfac_numedocu', new Array(),'Nit')" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem,"PREFIJO") %>'>
					</asp:TextBox>
					<asp:RequiredFieldValidator id="validatorNitCSA2" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic"
						ControlToValidate="nitCSA">*</asp:RequiredFieldValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Número Factura">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem,"NUMERO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="nitCSAa" runat="server" ReadOnly="true"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="nitCSAa" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem,"NUMERO") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Factura">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="nitCSAb" runat="server" ReadOnly="true" ></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="nitCSAb" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:N}") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Aprobado Repuestos">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "APROBADO", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="aprobado" runat="server" Text='0' onkeyup="NumericMaskE(this,event)"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="aprobado" runat="server" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "APROBADO", "{0:G}") %>'>
					</asp:TextBox>
					<asp:RequiredFieldValidator id="validatorAprobado" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic"
						ControlToValidate="aprobado">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorAprob" ASPClass="RegularExpressionValidator" ControlToValidate="aprobado"
						ValidationExpression="[0-9\,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Valor Aprobado Mano de Obra">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "APROBADOMO", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="aprobadoMO" runat="server" Text='0' onkeyup="NumericMaskE(this,event)"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="aprobadoMO" runat="server" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "APROBADOMO", "{0:G}") %>'>
					</asp:TextBox>
					<asp:RequiredFieldValidator id="validatorAprobadoMO" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic"
						ControlToValidate="aprobado">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorAprobMO" ASPClass="RegularExpressionValidator" ControlToValidate="aprobado"
						ValidationExpression="[0-9\,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Valor Aprobado Trabajos de Terceros">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "APROBADOTT", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="aprobadoTT" runat="server" Text='0' onkeyup="NumericMaskE(this,event)"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="aprobadoTT" runat="server" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "APROBADOTT", "{0:G}") %>'>
					</asp:TextBox>
					<asp:RequiredFieldValidator id="validatorAprobadoTT" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic"
						ControlToValidate="aprobado">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorAprobTT" ASPClass="RegularExpressionValidator" ControlToValidate="aprobado"
						ValidationExpression="[0-9\,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>  
			<asp:TemplateColumn HeaderText="Operaciones">
				<ItemTemplate>
					<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
                    <asp:Button CommandName="AddDataAll"  Text="Agregar todo" ID="btnAll" Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"
				HeaderText="Editar Item"></asp:EditCommandColumn>
             
		</Columns>
	</asp:datagrid></p>
<fieldset >
	<p></p>
	<legend>Totales</legend>
	<table>
		<tbody>
			<tr>
				<td>Total Facturas : </td>
				<td><asp:textbox id="totalFacts" runat="server" Width="102px" ReadOnly="True">$0</asp:textbox></td>
                <td>Total Repuestos: </td>
                <td><asp:textbox id="totalRepuestos" runat="server" Width="102px" ReadOnly="True">$0</asp:textbox></td>
			</tr>
			<tr>
				<td>Total Aprobado : </td>
				<td><asp:textbox id="totalApro" runat="server" Width="102px" ReadOnly="True">$0</asp:textbox></td>
                <td>Total Mano de obra: </td>
                <td><asp:textbox id="totalMObra" runat="server" Width="102px" ReadOnly="True">$0</asp:textbox></td>
			</tr>
			<tr>
				<td>Valor Iva : </td>
				<td><asp:textbox id="totalIva" runat="server" Width="102px" ReadOnly="True">$0</asp:textbox></td>
                <td>Total Trabajo Terceros: </td>
                <td><asp:textbox id="totalTerceros" runat="server" Width="102px" ReadOnly="True">$0</asp:textbox></td>
			</tr>
			<tr>
				<td>Total Factura : &nbsp;</td>
				<td><asp:textbox id="totalFact" runat="server" Width="102px" ReadOnly="True">$0</asp:textbox></td>
			</tr>
			<tr>
				<td></td>
                <td></td>
				<td><asp:button id="btnGrabar" onclick="Grabar_Factura" runat="server" Text="Grabar" CausesValidation="False"></asp:button>&nbsp;&nbsp;
					<asp:button id="btnCancelar" onclick="Cancelar_Factura" runat="server" Text="Cancelar" CausesValidation="False"></asp:button></td>
			</tr>

		</tbody></table>
</fieldset>
<p><asp:label id="lb" runat="server"></asp:label></p>
