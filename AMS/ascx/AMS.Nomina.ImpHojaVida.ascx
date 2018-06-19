<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Control Language="c#" codebehind="AMS.Nomina.ImpHojaVida.cs" autoeventwireup="false" Inherits="AMS.Nomina.ImpHojaVida" %>
<script runat="server">

    // Insert user control code here
    //

</script>
<!-- Insert content here -->
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>
	Porfavor escoja el nombre del empleado, y el tipo de reporte para la Hoja de 
	Vida.
</p>
<p>
	<asp:DropDownList id="DDLEMPLEADO" runat="server" class="dmediano"></asp:DropDownList>
</p>
<p>
	<table>
		<tbody>
			<tr>
				<td>
					<asp:Button id="BTNDETALLADA" runat="server" Visible="false" onclick="generarrptdetallado" Width="95px" Text="Detallada" ></asp:Button>
				</td>
				<td>
					<asp:Button id="BTNSIMPLE" cssClass="noEspera" onclick="generarrpt" runat="server" Width="96px" Text="Simple"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
	<br>
</p>

<CR:CrystalReportViewer id="visor" runat="server" Width="350px" Height="50px"></CR:CrystalReportViewer>
</td></tr></table></fieldset>
