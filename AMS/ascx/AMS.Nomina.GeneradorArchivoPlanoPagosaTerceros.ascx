<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Nomina.GeneradorArchivoPlanoPagosaTerceros.ascx.cs" Inherits="AMS.Nomina.GeneradorArchivoPlanoPagosaTerceros" %>

<p>
	<table id="Table1" class="filtersIn">
		<tbody>
			<tr>
				<td>PREFIJO</td>
				<td><asp:TextBox id="TXTPREFIJO" runat="server"></asp:TextBox></td>
			</tr>
			<tr>
				<td>NUMERO</td>
				<td><asp:TextBox id="TXTNUMERO" runat="server" ></asp:TextBox></td>
			</tr>

		</tbody>
	</table>
</p>
<br>
<P>
<asp:Label id="info" runat="server" Visible="false">Para descargar el archivo haga Click derecho sobre el Link abajo y seleccione "Guardar como..." para determinar la ruta de almacenamiento del archivo de texto.</asp:Label>
<asp:hyperlink download id="hl" runat="server" Visible="False">HyperLink</asp:hyperlink></P>
<asp:button id="btnGenerar1" runat="server" onClick="IniciarProceso" Text="Generar"></asp:button>