<%@ Control Language="c#" codebehind="AMS.Nomina.ImpAcumuladoProvisiones.cs" autoeventwireup="false" Inherits="AMS.Nomina.ImpAcumuladoProvisiones" %>
<script runat="server">

    // Insert user control code here
    //

</script>
<p>
	<!-- Insert content here --> Porfavor Seleccione&nbsp;las opciones&nbsp;para 
	generar el acumulado correspondiente del año en curso.
</p>
<p>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					Concepto:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
				</td>
				<td>
					<p>
						<asp:DropDownList id="DDLCONCEPTO" runat="server">
							<asp:ListItem Value="0">Todos los Conceptos</asp:ListItem>
						</asp:DropDownList>
					</p>
				</td>
			</tr>
			<tr>
				<td>
					Empleado:&nbsp;
				</td>
				<td>
					<p>
						<asp:DropDownList id="DDLEMPLEADO" runat="server" OnSelectedIndexChanged="cambioaempleados" AutoPostBack="True">
							<asp:ListItem Value="0">Todo el Archivo</asp:ListItem>
							<asp:ListItem Value="1">Empleado</asp:ListItem>
						</asp:DropDownList>
					</p>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>
	<asp:DropDownList id="DDLEMPLEADOS" runat="server" class="dmediano" Visible="False"></asp:DropDownList>
</p>
<p>
	<asp:Button id="BTNMOSTRAR" onclick="btnmostrar" runat="server" Text="MOSTRAR REPORTE"></asp:Button>
</p>
