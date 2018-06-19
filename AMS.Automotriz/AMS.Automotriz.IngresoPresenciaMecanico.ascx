<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.IngresoPresenciaMecanico.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_IngresoPresenciaMecanico" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript">
function CambioMecanico(objDdlMecanico){var spanInfoEstado = document.getElementById('<%=lbUltimoEstado.ClientID%>');var hdCodEstadoJS = document.getElementById('<%=hdCodEstado.ClientID%>');if(objDdlMecanico.value == ''){spanInfoEstado.innerText = hdCodEstadoJS.value = '';return;}var salidaConsulta = AMS_Automotriz_IngresoPresenciaMecanico.ConsultaUltimoEstadoMecanico(objDdlMecanico.value).value;var splitConsulta = salidaConsulta.split('@');spanInfoEstado.innerText = splitConsulta[1];hdCodEstadoJS.value = splitConsulta[0];if(splitConsulta[0] == 'E')document.getElementById('<%=ddlRazonSalida.ClientID%>').disabled = false;else document.getElementById('<%=ddlRazonSalida.ClientID%>').disabled = true;}
function ValidarEnvio(){if(document.getElementById('<%=ddlMecanico.ClientID%>').value == ''){alert('Seleccione un tecnico para realizar el proceso!');return false;}if(document.getElementById('<%=hdCodEstado.ClientID%>').value == 'E'){if(document.getElementById('<%=ddlRazonSalida.ClientID%>').value == ''){alert('Seleccione una razon para el detalle de la salida!');return false;}}}
</script>
<fieldset>
	<legend>
		Ingreso Presencia Mecanicos</legend>

	<table id="Table" class="filtersIn">
		<tr>
			<td>Fecha y Hora de Sistema :</td>
			<td><asp:Label id="lbFechHorSis" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td>Mecanico Seleccionado :</td>
			<td>
				<asp:DropDownList id="ddlMecanico" class="dmediano" runat="server" OnChange="CambioMecanico(this);"></asp:DropDownList></td>
		</tr>
		<tr>
			<td>Ultimo Estado Almacenado :
			</td>
			<td>
				<input id="hdCodEstado" runat="server" type="hidden">
				<asp:Label id="lbUltimoEstado" class="lpequeno" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td>Razón de Salida (solo aplica para salidas y no entradas):</td>
			<td>
				<asp:DropDownList id="ddlRazonSalida" class="dmediano" runat="server" Enabled="False"></asp:DropDownList></td>
		</tr>
		<tr>
			<td>Ingrese su clave :
			</td>
			<td>
				<asp:TextBox id="tbPass" runat="server" class="tpequeno" TextMode="Password"></asp:TextBox>&nbsp;
				<asp:Button id="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click"></asp:Button></td>
		</tr>
	</table>
</fieldset>