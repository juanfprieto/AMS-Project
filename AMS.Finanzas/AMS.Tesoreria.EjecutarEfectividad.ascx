<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EjecutarEfectivivad.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.EjecutarEfectividad" %>
<fieldset>
<p>
	Por favor escoja la fecha sobre la cual desea realizar la tarea :
</p>
<table id="table" class="filtersIn">
	<tbody>
		<tr>
			<td>
				Fecha de Ejecución :
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
				<asp:TextBox id="fecha" runat="server" Width="92px"></asp:TextBox>
				<asp:RegularExpressionValidator id="validatorFecha" runat="server" ControlToValidate="fecha" ErrorMessage="Formato de Fecha Invalido"
					ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:RegularExpressionValidator>
			</td>
			<td>
				<center>
					<asp:Button id="btnAceptar" onclick="btnAceptar_Click" runat="server" Text="Aceptar"></asp:Button>
				</center>
			</td>
		</tr>
	</tbody>
</table>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
</fieldset>
