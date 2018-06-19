<%@ Control Language="c#" codebehind="AMS.Nomina.ImpAcumuladoDoceMeses.cs" autoeventwireup="false" Inherits="AMS.Nomina.ImpAcumuladoDoceMeses" %>
<p>
	Porfavor Seleccione&nbsp;las opciones&nbsp;para generar el acumulado 
	correspondiente.
</p>
<p>
	Año:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
	&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:DropDownList id="DDLANO" class="dmediano" runat="server"></asp:DropDownList>
</p>
<p>
	Concepto :&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:DropDownList id="DDLCONCEPTO" class="dmediano" runat="server">
		<asp:ListItem Value="0">Todos los Conceptos</asp:ListItem>
		<asp:ListItem Value="1">Devengados</asp:ListItem>
		<asp:ListItem Value="2">Deducciones</asp:ListItem>
	</asp:DropDownList>
</p>
<p>
	Empleado:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:DropDownList id="DDLEMPLEADO" class="dmediano" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlempleado">
		<asp:ListItem Value="0">Todo el Archivo</asp:ListItem>
		<asp:ListItem Value="1">Empleado</asp:ListItem>
	</asp:DropDownList>
</p>
<p>
	<asp:DropDownList id="DDLEMPLEADOS" class="dmediano" runat="server" Visible="False"></asp:DropDownList>
</p>
<p>
	&nbsp;<asp:Button id="BTNMOSTRAR" onclick="btnmostrar" runat="server" Text="MOSTRAR REPORTE"></asp:Button>
</p>
