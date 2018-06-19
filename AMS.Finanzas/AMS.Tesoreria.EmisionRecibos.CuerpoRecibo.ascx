<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EmisionRecibos.CuerpoRecibo.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.CuerpoRecibo" %>
<fieldset>
<table id="Table" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<b><asp:Label id="lbEnc" runat="server"></asp:Label></b></td>
			<td align="center">
				<asp:ImageButton id="btnEncabezado" onclick="btnEncabezado_Click" runat="server" Text="Encabezado del Recibo"
					Width="30px" ImageUrl="../img/AMS.BotonExpandir.png" Height="30px"></asp:ImageButton>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:PlaceHolder id="phEncabezado" runat="server"></asp:PlaceHolder>
			</td>
		</tr>
       
		<tr>
			<td>
				<b>Documentos a Pagar</b>
			</td>
			<td align="center">
				<asp:ImageButton id="btnDocumentos" onclick="btnDocumentos_Click" runat="server" Text="Documentos a Pagar"
					Width="30px" ImageUrl="../img/AMS.BotonExpandir.png" Height="30px"></asp:ImageButton>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:PlaceHolder id="phDocumentos" runat="server"></asp:PlaceHolder>
			</td>
		</tr>

		<tr>
			<td>
				<b><asp:Label id="lbVarios" runat="server"></asp:Label></td></b>
			<td align="center">
				<asp:ImageButton id="btnVarios" onclick="btnVarios_Click" runat="server" Text="Varios" Width="30px"
					ImageUrl="../img/AMS.BotonExpandir.png" Height="30px"></asp:ImageButton>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:PlaceHolder id="phVarios" runat="server"></asp:PlaceHolder>
			</td>
		</tr>

		<tr>
			<td>
				<b>Relación de Conceptos Ingresos / Egresos No Causados</b>
			</td>
			<td align="center">
				<asp:ImageButton id="btnNoCausados" onclick="btnNoCausados_Click" runat="server" Text="Conceptos No Causados"
					Width="30px" ImageUrl="../img/AMS.BotonExpandir.png" Height="30px"></asp:ImageButton>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:PlaceHolder id="phCancelacionObligFin" runat="server"></asp:PlaceHolder><asp:PlaceHolder id="phNoCausados" runat="server"></asp:PlaceHolder>
			</td>
		</tr>

		<tr>
			<td>
				<b>Relación de Pagos</b>
			</td>
			<td align="center">
				<asp:ImageButton id="btnPagos" onclick="btnPagos_Click" runat="server" Text="Pagos" Width="30px"
					ImageUrl="../img/AMS.BotonExpandir.png" Height="30px"></asp:ImageButton>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:PlaceHolder id="phPagos" runat="server"></asp:PlaceHolder>
			</td>
		</tr>

	</tbody>
</table>

<p>
	<asp:Button id="guardar" onclick="guardar_Click" runat="server" Text="Grabar" Enabled="False" ></asp:Button>
	<asp:Button id="cancelar" onclick="cancelar_click" runat="server" Text="Cancelar" CausesValidation="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>

<P>&nbsp;</P>
<P>&nbsp;</P>
</fieldset>

