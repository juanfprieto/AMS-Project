<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Nomina.RegistroAsistencia.ascx.cs" Inherits="AMS.Nomina.AMS_Nomina_RegistroAsistencia" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table class="filtersIn">
	<tr>
		<td style="HEIGHT: 88px">Empleado:&nbsp;
		</td>
		<td style="HEIGHT: 88px"><asp:dropdownlist id="ddlTarjetasEmpleados" AutoPostBack="false" Runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Clave:
		</td>
		<td><asp:textbox id="txtClave" Runat="server" TextMode="Password"></asp:textbox></td>
	</tr>
	<tr>
		<td align="center" colSpan="2"></td>
	</tr>
</table>
<P>Si el&nbsp;Trabajador buscado no aparece , no esta debidamente enlazado a los 
	empleados.
</P>
<P><asp:button id="Btn_Entrada" Text="Entrada" Height="64px" runat="server" class="bmediano"></asp:button>&nbsp;
	<asp:button id="Btn_salida" Text="Salida" Height="64px" runat="server" class="bmediano"></asp:button></P>
<P>&nbsp;</P>
<P></P>
<P>&nbsp;</P>
