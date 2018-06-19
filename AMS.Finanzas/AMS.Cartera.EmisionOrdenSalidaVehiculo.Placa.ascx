<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.EmisionOrdenSalidaVehiculo.Placa.ascx..cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.BusquedaPlaca" %>

<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				Introduzca la placa del vehículo :
			</td>
			<td>
				<asp:TextBox id="tbPlaca" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator id="rfv" runat="server" ControlToValidate="tbPlaca" ErrorMessage="Campo Obligatorio">*</asp:RequiredFieldValidator>
			</td>
		</tr>
	</tbody>
</table>
<p>
</p>
<asp:Button id="btnAceptar" onclick="btnAceptar_Click" runat="server" Text="Aceptar"></asp:Button>
