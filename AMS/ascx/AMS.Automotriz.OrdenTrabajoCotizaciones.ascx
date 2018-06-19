<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenTrabajoCotizaciones.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.OrdenesTallerCotizaciones" %>
<fieldset>
<p>  
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					<asp:Label id="Label1" runat="server"><b>Datos de la Orden</b></asp:Label></td>
				<td align="center">
					<asp:ImageButton id="origen" onclick="Cargar_DatosOrden" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
						CausesValidation="False" ></asp:ImageButton>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:PlaceHolder id="datosOrigen" runat="server" Visible="False"></asp:PlaceHolder>
				</td>
			</tr>
			<tr> 
				<td>
					<asp:Label id="Label2" runat="server"><b>Datos Propietario</b></asp:Label></td>
				<td align="center">
					<asp:ImageButton id="propietario" onclick="Cargar_DatosPropietario" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
						CausesValidation="False"></asp:ImageButton>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:PlaceHolder id="datosPropietario" runat="server" Visible="False"></asp:PlaceHolder>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label id="Label3" runat="server"><b>Datos Vehiculo</b></asp:Label></td>
				<td align="center">
					<asp:ImageButton id="vehiculo" onclick="Cargar_DatosVehiculo" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
						CausesValidation="False"></asp:ImageButton>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:PlaceHolder id="datosVehiculo" runat="server" Visible="False"></asp:PlaceHolder>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label id="Label5" runat="server"><b>Kits o Combos</b></asp:Label></td>
				<td align="center">
					<asp:ImageButton id="botonKits" onclick="Cargar_KitsCombos" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
						CausesValidation="False"></asp:ImageButton>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:PlaceHolder id="kitsCombos" runat="server" Visible="False"></asp:PlaceHolder>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label id="Label7" runat="server"><b>Peritaje</b></asp:Label></td>
				<td align="center">
					<asp:ImageButton id="opPeritaje" onclick="Cargar_Operaciones_Peritaje" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
						CausesValidation="False"></asp:ImageButton>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:PlaceHolder id="operacionesPeritaje" runat="server" Visible="False"></asp:PlaceHolder>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>
</p>
<p>
    <asp:Button id="grabar" onclick="Grabar_Orden" runat="server" Text="Grabar Orden de Trabajo"
		Enabled="False"></asp:Button>&nbsp;
	<asp:Button id="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
</fieldset>
