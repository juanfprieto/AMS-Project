<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Nomina.GeneradorPlanillaPila.ascx.cs" Inherits="AMS.Nomina.GeneradorPlanillaPila" %>

<p>
	<table id="Table1" class="filtersIn">
		<tbody>
			<tr>
				<td>AÑO</td>
				<td><asp:dropdownlist id="DDLANO" runat="server"></asp:dropdownlist></td>
			</tr>
			<tr>
				<td>MES</td>
				<td><asp:dropdownlist id="DDLMES" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
			</tr>
		</tbody>
	</table>
</p>
<br>
<P>
<asp:Label id="info" runat="server" Visible="false">Para descargar el archivo haga Click derecho sobre el Link abajo y seleccione "Guardar como..." para determinar la ruta de almacenamiento del archivo de texto.</asp:Label>
<asp:hyperlink download id="hl" runat="server" Visible="False">HyperLink</asp:hyperlink></P>
<asp:button id="btnGenerar" runat="server" Text="Generar"></asp:button>