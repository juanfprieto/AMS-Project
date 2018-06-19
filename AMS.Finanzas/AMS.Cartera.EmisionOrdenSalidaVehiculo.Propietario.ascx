<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.EmisionOrdenSalidaVehiculo.Propietario.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.BusquedaPropietario" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>

<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				Identificación del Propietario :
			</td>
			<td>
				<asp:TextBox id="tbNit" onclick="ModalDialog(this, 'SELECT M.mnit_nit AS Nit,M.mnit_nombres CONCAT\' \'CONCAT M.mnit_apellidos AS Nombre,M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M,pciudad P WHERE M.pciu_codigo=P.pciu_codigo ORDER BY mnit_nit',1,new Array())"
					runat="server" ToolTip="Haga Click"></asp:TextBox>
				<asp:RequiredFieldValidator id="rfv" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="tbNit">*</asp:RequiredFieldValidator>
			</td>
		</tr>
	</tbody>
</table>
<p>
	<asp:Button id="btnAceptar" onclick="btnAceptar_Click" runat="server" Text="Aceptar"></asp:Button>
</p>
