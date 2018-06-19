<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.TrasFondosCCCheque.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.TrasladoFondosCheque" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				Código Cuenta Corriente Origen :
			</td>
			<td>
				<asp:TextBox id="codigoCCO" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo',new Array())"
					Width="72px" ToolTip="Haga Click" runat="server" ReadOnly="True"></asp:TextBox>
				<asp:RequiredFieldValidator id="validatorCCO" runat="server" ControlToValidate="codigoCCO" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
			</td>
			<td>
				Código Banco :
			</td>
			<td>
				<asp:TextBox id="codigoCCOb" Width="72px" runat="server" ReadOnly="true"></asp:TextBox>
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
				<asp:TextBox id="fecha" onkeyup="DateMask(this)" Width="92px" runat="server"></asp:TextBox>
				<asp:RegularExpressionValidator id="validatorFecha" runat="server" ControlToValidate="fecha" ErrorMessage="Formato de Fecha Invalido"
					ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:RegularExpressionValidator>
			</td>
		</tr>
		<tr>
			<td>
				Código Cuenta Corriente Destino :
			</td>
			<td>
				<asp:TextBox id="codigoCCD" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo',new Array())"
					Width="72px" ToolTip="Haga Click" runat="server" ReadOnly="True"></asp:TextBox>
				<asp:RequiredFieldValidator id="validatorCCD" runat="server" ControlToValidate="codigoCCD" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
			</td>
			<td>
				Código Banco :
			</td>
			<td>
				<asp:TextBox id="codigoCCDb" Width="72px" runat="server" ReadOnly="true"></asp:TextBox>
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
				Prefijo Comprobante de Egreso :
			</td>
			<td>
				<asp:DropDownList id="prefijoEgreso" runat="server" onSelectedIndexChanged="Escoger_Egreso" AutoPostBack="True"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				Número Comprobante de Egreso :
			</td>
			<td>
				<asp:DropDownList id="numeroEgreso" runat="server"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Button id="aceptarDatos" onclick="Aceptar_Datos" runat="server" Text="Aceptar"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
<p>
</p>
<asp:DataGrid id="gridDatos" runat="server" cssclass="datagrid" onItemCommand="gridDatos_Item" CellPadding="3" AutoGenerateColumns="False">
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
	<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<Columns>
		<asp:BoundColumn DataField="CODIGORECIBOCAJA" HeaderText="Codigo Comprobante Egreso"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMERORECIBOCAJA" HeaderText="N&#250;mero Comprobante Egreso"></asp:BoundColumn>
		<asp:BoundColumn DataField="TIPOPAGO" HeaderText="Tipo de Pago"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMERO" HeaderText="N&#250;mero del Documento"></asp:BoundColumn>
		<asp:BoundColumn DataField="NOMBREBANCO" HeaderText="Banco"></asp:BoundColumn>
		<asp:BoundColumn DataField="VALOR" HeaderText="Valor" DataFormatString="{0:c}"></asp:BoundColumn>
		<asp:BoundColumn DataField="NIT" HeaderText="Nit"></asp:BoundColumn>
		<asp:BoundColumn DataField="NITBENEFICIARIO" HeaderText="Nit Beneficiario"></asp:BoundColumn>
		<asp:BoundColumn DataField="FECHA" HeaderText="Fecha"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Agregar">
			<ItemTemplate>
				<asp:Button Text="Agregar" CommandName="Agregar" id="agregar" runat="server" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Remover">
			<ItemTemplate>
				<asp:Button Text="Remover" CommandName="Remover" id="remover" runat="server" Enabled="false" />
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
