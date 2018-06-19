<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.PlanificarTaller.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_PlanificarTaller" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
<p>
<table id="Table" class="filtersIn">
<tr>
<td>

	<TR>
		<TD>Fecha de Consulta:
		</TD>
		<TD><asp:label id="Label3" runat="server"></asp:label></TD>
	</TR>
	<TR>
		<TD>Seleccione el Taller</TD>
		<TD><asp:dropdownlist id="Lista" class="dmediano" runat="server"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD>Hora Actual</TD>
		<TD><asp:label id="Label5" runat="server"></asp:label></TD>
	</TR>
    </td>
</tr>
</TABLE>
</p>
<P>

	<asp:button id="Button1" runat="server" OnClick="GenerarTabla" Text="Generar Reporte"></asp:button></P>
<P></P>
</fieldset>
