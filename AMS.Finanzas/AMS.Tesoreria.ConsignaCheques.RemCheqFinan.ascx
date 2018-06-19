<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.RemCheqFinan.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.RemisionChequeFinanciera" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				Código Cuenta Financiera :
			</td>
			<td>
				<asp:TextBox id="codigoCF" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo AND P.ttip_tipocuenta=\'F\'',new Array())"
					ReadOnly="True" ToolTip="Haga Click" runat="server" Width="73px"></asp:TextBox>
				<asp:RequiredFieldValidator id="validatorCCF" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="codigoCF">*</asp:RequiredFieldValidator>
			</td>
			<td>
				Fecha :
			</td>
			<td>
				<img onmouseover="calendario.style.visibility='visible'" onmouseout="calendario.style.visibility='hidden'"
					src="../img/AMS.Icon.Calendar.gif" border="0">
				<table id="calendario" onmouseover="calendario.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
					onmouseout="calendario.style.visibility='hidden'">
					<tbody>
						<tr>
							<td>
								<asp:calendar BackColor=Beige id="calendarioFecha" runat="server" OnSelectionChanged="Cambiar_Fecha"></asp:Calendar>
							</td>
						</tr>
					</tbody>
				</table>
				<asp:TextBox id="fecha" onkeyup="DateMask(this)" runat="server" Width="92px"></asp:TextBox>
				<asp:RegularExpressionValidator id="validatorFecha" runat="server" ErrorMessage="Formato de Fecha Invalido" ControlToValidate="fecha"
					ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:RegularExpressionValidator>
			</td>
		</tr>
		<tr>
			<td>
				Nit de la Financiera :
			</td>
			<td>
				<asp:TextBox id="nitFinanciera" onclick="ModalDialog(this,'SELECT a.mnit_nit AS Nit,a.mnit_nombres CONCAT \' \' CONCAT a.mnit_apellidos AS Razon_Social FROM mnit a,pfinanciera b where a.mnit_nit = b.mnit_nit ORDER BY a.mnit_nit',new Array())"
					ReadOnly="true" ToolTip="Haga Click" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator id="validatorNit" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="nitFinanciera">*</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>
				Prefijo Documento de Cobro :
			</td>
			<td>
				<asp:DropDownList id="prefijoFactura" runat="server"></asp:DropDownList>
			</td>
		</tr>
	</tbody>
</table>
<p>
</p>
<table>
	<tbody>
		<tr>
			<td>
				Recibo de Caja :
			</td>
			<td>
				<asp:DropDownList id="prefijoCaja" runat="server" onSelectedIndexChanged="prefijoCaja_IndexChanged"
					AutoPostBack="True"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				Número Recibo Caja :
			</td>
			<td>
				<asp:DropDownList id="numeroCaja" runat="server"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Button id="aceptar" onclick="aceptar_Click" runat="server" Text="Aceptar"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
<p>
</p>
<asp:Panel id="panelGrillas" runat="server" Visible="False">
<TABLE>
  <TR>
    <TD>
<asp:DataGrid id=gridDatos runat="server" cssclass="datagrid" onItemCommand="gridDatos_Item" ShowFooter="true" AutoGenerateColumns="False" CellPadding="3">
					<FooterStyle cssclass="footer"></FooterStyle>
					<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
					<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
					<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
					<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
					<ItemStyle cssclass="item"></ItemStyle>
					<Columns>
						<asp:BoundColumn DataField="CODIGORECIBOCAJA" ReadOnly="True" HeaderText="Codigo Recibo de Caja"></asp:BoundColumn>
						<asp:BoundColumn DataField="NUMERORECIBOCAJA" ReadOnly="True" HeaderText="N&#250;mero Recibo de Caja"></asp:BoundColumn>
						<asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="N&#250;mero del Documento"></asp:BoundColumn>
						<asp:BoundColumn DataField="NOMBREBANCO" ReadOnly="True" HeaderText="Banco"></asp:BoundColumn>
						<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor" DataFormatString="{0:c}"></asp:BoundColumn>
						<asp:BoundColumn DataField="NIT" ReadOnly="True" HeaderText="Nit"></asp:BoundColumn>
						<asp:BoundColumn DataField="FECHA" ReadOnly="True" HeaderText="Fecha"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Agregar Cheques">
							<ItemTemplate>
								<center>
									<asp:CheckBox id="chkCheque" runat="server" />
								</center>
							</ItemTemplate>
							<FooterTemplate>
								<center>
									<asp:Button id="btnAdd" runat="server" Text=">>" CommandName="agregar" Width="50" />
								</center>
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid></TD></TR>
  <TR></TR>
  <TR>
    <TD>
<asp:DataGrid id=gridCheques runat="server" cssclass="datagid" onItemCommand="gridCheques_Item" AutoGenerateColumns="False" CellPadding="3" onEditCommand="gridCheques_Edit" onUpdateCommand="gridCheques_Update" onCancelCommand="gridCheques_Cancel">
					<FooterStyle cssclass="footer"></FooterStyle>
					<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
					<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
					<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
					<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
					<ItemStyle cssclass="item"></ItemStyle>
					<Columns>
						<asp:BoundColumn DataField="CODIGORECIBOCAJA" ReadOnly="True" HeaderText="Codigo Recibo de Caja"></asp:BoundColumn>
						<asp:BoundColumn DataField="NUMERORECIBOCAJA" ReadOnly="True" HeaderText="N&#250;mero Recibo de Caja"></asp:BoundColumn>
						<asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="N&#250;mero del Documento"></asp:BoundColumn>
						<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor" DataFormatString="{0:c}"></asp:BoundColumn>
						<asp:BoundColumn DataField="COMISION" HeaderText="Valor Comisi&#243;n" DataFormatString="{0:C}"></asp:BoundColumn>
						<asp:BoundColumn DataField="IVA" HeaderText="Valor IVA" DataFormatString="{0:C}"></asp:BoundColumn>
						<asp:BoundColumn DataField="RETENCION" HeaderText="Valor Retenci&#243;n" DataFormatString="{0:C}"></asp:BoundColumn>
						<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
						<asp:ButtonColumn Text="Borrar" ButtonType="PushButton" CommandName="Delete"></asp:ButtonColumn>
					</Columns>
				</asp:DataGrid></TD></TR>
  <TR>
    <TD>
      <CENTER>
<asp:Button id=aceptarCheques onclick=aceptarCheques_Click runat="server" Text="Aceptar" Enabled="False"></asp:Button></CENTER></TD></TR></TABLE>
</asp:Panel>
<asp:Label id="lb" runat="server"></asp:Label>
