<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.EntregaRemesas.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_EntregaRemesas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="RemeasLabel" runat="server" Font-Bold="True" Font-Size="Medium">Entrega De Encomiendas</asp:label>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
&nbsp;
<TABLE id="Table1" style="WIDTH: 568px; HEIGHT: 134px" cellSpacing="1" cellPadding="1"
	width="568" border="0">
	<TR>
		<TD style="HEIGHT: 24px"><asp:label id="Label2" runat="server">Remesas Pendientes:</asp:label></TD>
		<TD style="HEIGHT: 24px"><asp:dropdownlist id="remesaspendientes" runat="server" AutoPostBack="True" Width="200px"></asp:dropdownlist><asp:label id="Label3" runat="server" Font-Bold="True" Font-Size="XX-Small" Width="111px">(Numero Remesa)</asp:label></TD>
	</TR>
	<TR>
		<TD style="HEIGHT: 11px"><asp:label id="Label5" runat="server">Codigo Remesa :</asp:label></TD>
		<TD style="HEIGHT: 11px"><asp:label id="CodigoLabel" runat="server">Codigo</asp:label></TD>
	</TR>
	<TR>
		<td>
			<asp:Label id="Label8" runat="server">Descripcion</asp:Label></TD>
		<td>
			<asp:Label id="descripcionLabel" runat="server">Descripcion</asp:Label>&nbsp;&nbsp;</TD>
	</TR>
	<tr>
		<td>
			<asp:Label id="Label9" runat="server">Contenido:</asp:Label></td>
		<td>
			<asp:Label id="ContenidoLabel" runat="server">Contenido</asp:Label></td>
	</tr>
	<tr>
		<td>
			<asp:Label id="Label10" runat="server">Unidades :</asp:Label>
		</td>
		<td>
			<asp:Label id="unidadesLabel" runat="server">Unidades</asp:Label></td>
	</tr>
	<tr>
		<td><asp:label id="Label1" runat="server">Nombre Destinatario:</asp:label>
		</td>
		<td><asp:label id="NombreLabel" runat="server">Nombre</asp:label></td>
	</tr>
	<tr>
		<td><asp:label id="Label4" runat="server">Nombre Emisor:</asp:label></td>
		<td>
			<asp:Label id="NombreLabel2" runat="server">Nombre</asp:Label></td>
	</tr>
	<tr>
		<td><asp:label id="Label7" runat="server">Origen:</asp:label></td>
		<td>
			<asp:Label id="OrigenLabel" runat="server">Origen</asp:Label></td>
	</tr>
	<tr>
		<td>
			<asp:Label id="Label6" runat="server">Destino:</asp:Label>
		</td>
		<td>
			<asp:Label id="destinoLabel" runat="server">Destino</asp:Label></td>
	</tr>
</TABLE>
<asp:button id="Entregar" onclick="Entregar_click" runat="server" Text="Entregar"></asp:button>
