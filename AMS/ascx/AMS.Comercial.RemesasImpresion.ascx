<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RemesasImpresion.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RemesasImpresion" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="RemeasLabel" Font-Size="Medium" Font-Bold="True" runat="server">Remesas y Encomiendas</asp:label>
<P></P>
<asp:label id="Label1" runat="server">Numero de Remesa: </asp:label><asp:dropdownlist id="NUMREMESA" runat="server" AutoPostBack="True" OnSelectedIndexChanged="NUMREMESa_proceso"></asp:dropdownlist>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
<table>
	<tbody>
		<tr>
			<td><asp:image id="Image1" runat="server" ImageUrl="../img/bus_scania.jpg" Height="136px" Width="818px"></asp:image></td>
		</tr>
	</tbody>
</table>
<TABLE id="TableRemesa" border="0" runat="server">
	<TBODY>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="NumeroRemesaLabel" runat="server">Numero Remesa</asp:label></td>
			<td><asp:label id="NumeroRemesa" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="FechaLabel" runat="server">Fecha Remesa</asp:label></td>
			<td><asp:label id="Fecha" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="TipoRemesaLabel" runat="server">Tipo de Remesa</asp:label></td>
			<td><asp:label id="TRemesa" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px; HEIGHT: 37px"><asp:label id="OrigenLabel" runat="server">Origen:</asp:label></td>
			<td style="HEIGHT: 37px"><asp:label id="ciudad1" runat="server">--</asp:label></td>
			<td style="WIDTH: 151px; HEIGHT: 37px"></td>
			<td style="HEIGHT: 37px"></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="DestinoLabel" runat="server"> Destino:</asp:label></td>
			<td><asp:label id="ciudad2" runat="server">--</asp:label></td>
			<td style="WIDTH: 151px"></td>
			<td></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px"></td>
		</tr>
		<tr>
			<TD style="WIDTH: 156px"><asp:label id="RutaLabel" runat="server">Ruta: </asp:label></TD>
			<TD><asp:label id="ruta" runat="server">--</asp:label></TD>
			<TD style="WIDTH: 151px" align="left"></TD>
			<TD></TD>
		</tr>
		<TR>
			<TD style="WIDTH: 156px"><asp:label id="BusLabel" runat="server">Bus:</asp:label></TD>
			<TD><asp:label id="bus" runat="server">--</asp:label></TD>
		</TR>
		<TR>
			<td style="WIDTH: 156px; HEIGHT: 27px"><asp:label id="DirOrigenLabel" runat="server">Direccion de Origen: </asp:label></td>
			<td style="HEIGHT: 27px"><asp:label id="dirorigen" runat="server">--</asp:label></td>
			<td style="WIDTH: 151px; HEIGHT: 27px" align="left"><asp:label id="TelOrigenLabel" runat="server">Telefono de Origen: </asp:label></td>
			<td style="HEIGHT: 27px"><asp:label id="telorigen" runat="server">--</asp:label></td>
		</TR>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="NomEmisorLabel" runat="server">Nombre del Emisor: </asp:label></td>
			<td><asp:label id="emisor" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="DirDestinoLabel" runat="server">Direccion de Destino: </asp:label></td>
			<td><asp:label id="dirdestino" runat="server">--</asp:label></td>
			<td style="WIDTH: 151px" align="left"><asp:label id="TelDestinoLabel" runat="server">Telefono de Destino: </asp:label></td>
			<td><asp:label id="teldestino" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="NomDestinoLabel" runat="server">Nombre del Destinatario: </asp:label></td>
			<td><asp:label id="destinatario" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="Label" runat="server">Contenido :</asp:label></td>
			<td><asp:label id="DescripcionLabel" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px; HEIGHT: 25px"><asp:label id="ValoDecLabel" runat="server">Valor Declarado: </asp:label></td>
			<td style="HEIGHT: 25px"><asp:label id="valordeclarado" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px; HEIGHT: 40px"><asp:label id="UnidadesLabel" runat="server">Unidades: </asp:label></td>
			<td style="HEIGHT: 40px"><asp:label id="unidades" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 156px; HEIGHT: 21px"><asp:label id="PesoLabel" runat="server">Peso: </asp:label></td>
			<td style="HEIGHT: 21px"><asp:label id="peso" runat="server">--</asp:label></td>
		</tr>
		<tr>
		</tr>
		<tr>
			<td style="WIDTH: 156px"><asp:label id="VolumenLabe" runat="server">Volumen: </asp:label></td>
			<td><asp:label id="volumen" runat="server">--</asp:label></td>
		</tr>
		<tr>
			<td><asp:label id="ValoFleteLabel" Font-Size="Medium" Font-Bold="True" runat="server" Width="112px">Valor  Flete :</asp:label></td>
			<td><asp:label id="total" runat="server" Font-Size="Medium" ForeColor="Black">--</asp:label><asp:label id="MonedaLabel" Font-Size="X-Small" Font-Bold="True" runat="server" ForeColor="Black">Pesos + I.V.A</asp:label></td>
		</tr>
	</TBODY>
</TABLE>
