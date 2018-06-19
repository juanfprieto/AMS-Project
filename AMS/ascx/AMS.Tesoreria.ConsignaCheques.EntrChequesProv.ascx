<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.EntrChequesProv.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.EntegaChequesProveedores" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>Código de la Cuenta Corriente :
			</td>
			<td><asp:textbox id="codigoCC" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo',new Array())"
					Width="71px" runat="server" ReadOnly="True" ToolTip="Haga Click"></asp:textbox></td>
			<td>Banco :
			</td>
			<td><asp:textbox id="codigoCCb" Width="71px" runat="server" ReadOnly="True" ToolTip="Haga Click"></asp:textbox></td>
			<td>Fecha :
			</td>
			<td><IMG onmouseover="calendario.style.visibility='visible'" onmouseout="calendario.style.visibility='hidden'"
					src="../img/AMS.Icon.Calendar.gif" border="0">
				<table id="calendario" onmouseover="calendario.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
					onmouseout="calendario.style.visibility='hidden'">
					<tbody>
						<tr>
							<td><asp:calendar BackColor=Beige id="calendarioFecha" runat="server" OnSelectionChanged="Cambiar_Fecha"></asp:calendar></td>
						</tr>
					</tbody></table>
				<asp:textbox id="fecha" onkeyup="DateMask(this)" Width="92px" runat="server"></asp:textbox><asp:regularexpressionvalidator id="validatorFecha" runat="server" ValidationExpression="\d{4}-\d{2}-\d{2}" ErrorMessage="Formato de Fecha Invalido"
					ControlToValidate="fecha">*</asp:regularexpressionvalidator></td>
		</tr>
	</tbody></table>
<p></p>
<table>
	<tbody>
		<tr>
			<td>Prefijo Comprobante de Egreso :
			</td>
			<td><asp:dropdownlist id="prefijoEgreso" runat="server" onSelectedIndexChanged="IndexChanged_prefijoEgreso"
					AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>Número Comprobante :
			</td>
			<td><asp:dropdownlist id="numeroEgreso" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td><asp:button id="aceptarDatos" onclick="aceptarDatos_Click" runat="server" Text="Aceptar"></asp:button></td>
		</tr>
	</tbody></table>
<p></p>
<asp:datagrid id="gridDatos" runat="server" cssclass="datagrid" onItemCommand="gridDatos_Item" AutoGenerateColumns="False" CellPadding="3">
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
	<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<Columns>
		<asp:BoundColumn DataField="PREFIJOCAJA" HeaderText="Prefijo Comprobante Egreso"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMEROCAJA" HeaderText="N&#250;mero Comprobante Egreso"></asp:BoundColumn>
		<asp:BoundColumn DataField="TIPOPAGO" HeaderText="Tipo de Pago"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMERO" HeaderText="N&#250;mero Documento"></asp:BoundColumn>
		<asp:BoundColumn DataField="NOMBREBANCO" HeaderText="Banco"></asp:BoundColumn>
		<asp:BoundColumn DataField="VALOR" HeaderText="Valor" DataFormatString="{0:C}"></asp:BoundColumn>
		<asp:BoundColumn DataField="NITBENEFICIARIO" HeaderText="Nit Beneficiario"></asp:BoundColumn>
		<asp:BoundColumn DataField="FECHA" HeaderText="Fecha"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Agregar/Remover">
			<ItemTemplate>
				<center>
					<asp:Button id="agregar" runat="server" CommandName="agregar" Text="Agregar" />
				</center>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
