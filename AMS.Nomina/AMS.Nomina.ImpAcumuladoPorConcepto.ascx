<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Control Language="c#" codebehind="AMS.Nomina.ImpAcumuladoPorConcepto.cs" autoeventwireup="false" Inherits="AMS.Nomina.ImpAcumuladoPorConcepto" %>
<p>
	Porfavor Seleccione&nbsp;las opciones&nbsp;para generar el acumulado 
	correspondiente.
</p>
<p>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					Año:</td>
				<td>
					<asp:DropDownList id="DDLANO" runat="server"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					Mes:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
				</td>
				<td>
					<p>
						<asp:DropDownList id="DDLMES" runat="server"></asp:DropDownList>
					</p>
				</td>
			</tr>
			<tr>
				<td>
					Cod. Concepto:&nbsp;
				</td>
				<td>
					<p>
						<asp:DropDownList id="DDLCONCEPTO" runat="server"></asp:DropDownList>
					</p>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>
	<asp:Button id="BTNMOSTRAR" onclick="btnmostrar" runat="server" Text="MOSTRAR REPORTE"></asp:Button>
	<CR:CrystalReportViewer id="visor" runat="server" visible="false" Width="350px" Height="50px"></CR:CrystalReportViewer>
</p>
