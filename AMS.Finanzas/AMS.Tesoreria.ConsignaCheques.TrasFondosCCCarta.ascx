<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.TrasFondosCCCarta.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.TrasladoFondosCarta" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>Código Cuenta Corriente Origen :
			</td>
			<td><asp:textbox id="codigoCCO" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo',new Array())"
					Width="72px" ToolTip="Haga Click" runat="server" ReadOnly="True"></asp:textbox><asp:requiredfieldvalidator id="validatorCCO" runat="server" ControlToValidate="codigoCCO" ErrorMessage="RequiredFieldValidator">*</asp:requiredfieldvalidator></td>
			<td>Código del Banco :
			</td>
			<td><asp:textbox id="codigoCCOb" Width="72px" runat="server" ReadOnly="true"></asp:textbox></td>
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
					</tbody>
				</table>
				<asp:textbox id="fecha" onkeyup="DateMask(this)" Width="92px" runat="server"></asp:textbox><asp:regularexpressionvalidator id="validatorFecha" runat="server" ControlToValidate="fecha" ErrorMessage="Formato de Fecha Invalido"
					ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:regularexpressionvalidator></td>
		</tr>
		<tr>
			<td>Código Cuenta Corriente Destino :
			</td>
			<td><asp:textbox id="codigoCCD" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo',new Array())"
					Width="72px" ToolTip="Haga Click" runat="server" ReadOnly="True"></asp:textbox><asp:requiredfieldvalidator id="validatorCCD" runat="server" ControlToValidate="codigoCCD" ErrorMessage="RequiredFieldValidator">*</asp:requiredfieldvalidator></td>
			<td>Código del Banco :
			</td>
			<td><asp:textbox id="codigoCCDb" Width="72px" runat="server" ReadOnly="true"></asp:textbox></td>
			<td><asp:button id="aceptarDatos" onclick="Aceptar_Valores" runat="server" Text="Aceptar"></asp:button></td>
		</tr>
	</tbody>
</table>
<p></p>
<table>
	<tbody>
		<tr>
			<td>Saldo Disponible :
			</td>
			<td><asp:label id="lbSaldo" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td>Valor a Transferir :
			</td>
			<td><asp:textbox id="valorTransferencia" onkeyup="NumericMaskE(this,event)" Width="112px" runat="server"
					CssClass="AlineacionDerecha"></asp:textbox></td>
		</tr>
		<tr>
			<td><asp:label id="lbInfoAutorizacion" Text="Número de Autorización del Sobregiro : " Visible="False"
					Runat="server"></asp:label></td>
			<td>
				<asp:TextBox ID="tbSobregiro" Runat="server" Visible="False" Width="144px" MaxLength="20"></asp:TextBox>
				<asp:RequiredFieldValidator id="rfv1" runat="server" ControlToValidate="tbSobregiro" ErrorMessage="Campo Obligatorio"
					Visible="False" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Button id="aceptar" onclick="Aceptar_Transferencia" runat="server" Text="Aceptar" Enabled="False"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
