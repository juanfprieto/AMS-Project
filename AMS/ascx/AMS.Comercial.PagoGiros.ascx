<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PagoGiros.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PagoGiros" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Información del giro:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 14px"><asp:label id="Label15" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Destino :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 14px"><asp:dropdownlist id="ddlAgenciaD" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
			<TD style="HEIGHT: 14px">&nbsp;</TD>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Origen :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgenciaO" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
			</TD>
			<td>&nbsp;</TD>
		</tr>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Número de Documento :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlDocumento" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></TD>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlPago" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Emisor:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label17" runat="server" Font-Size="XX-Small" Font-Bold="True">Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblDocEmisor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Nombre :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblNombreEmisor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label19" runat="server" Font-Size="XX-Small" Font-Bold="True">Apellido :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblApellidoEmisor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label20" runat="server" Font-Size="XX-Small" Font-Bold="True">Telefono :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblTelefonoEmisor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label21" runat="server" Font-Size="XX-Small" Font-Bold="True">Direccion :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblDireccionEmisor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Receptor:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblDocReceptor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Nombre :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblNombreReceptor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Apellido :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblApellidoReceptor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Telefono :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblTelefonoReceptor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Direccion :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:label id="lblDireccionReceptor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Giro:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblNumDocumento" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Recepcion :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblFechaR" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Entrega :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblFechaE" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Giro :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblValorGiro" runat="server" Font-Size="Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top" align="right"></TD>
				<TD style="WIDTH: 154px" vAlign="top" align="left"><BR>
					<asp:button id="btnRegistrar" Font-Size="XX-Small" Font-Bold="True" Width="100px" Runat="server"
						Text="Pagar Giro"></asp:button><BR>
				</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel></DIV>
<br>
<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
