<%@ Control Language="c#" codebehind="AMS.Nomina.ImpAcumuladoPorMes.cs" autoeventwireup="false" Inherits="AMS.Nomina.ImpAcumuladoPorMes" %>
<p>
	Porfavor Seleccione&nbsp;las opciones&nbsp;para generar el acumulado 
	correspondiente del año en curso.
</p>
<table class="filtersIn">
	<tbody>
		<tr>
			<td>
				Mes:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
			</td>
			<td>
				<p>
					<asp:DropDownList id="DDLMES" class="dmediano" runat="server"></asp:DropDownList>
				</p>
			</td>
		</tr>
		<tr>
			<td>
				Empleado:&nbsp;
			</td>
			<td>
				<p>
					<asp:DropDownList id="DDLEMPLEADO" class="dmediano" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cambioaempleados">
						<asp:ListItem Value="0">Todo el Archivo</asp:ListItem>
						<asp:ListItem Value="1">Empleado</asp:ListItem>
					</asp:DropDownList>
				</p>
			</td>
		</tr>
	</tbody>
</table>
<p>
	<asp:DropDownList id="DDLEMPLEADOS" class="dmediano" runat="server" Visible="False"></asp:DropDownList>
</p>
<p>
	<asp:Button id="BTNMOSTRAR" onclick="btnmostrar" runat="server" Text="MOSTRAR REPORTE"></asp:Button>
</p>
