<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.EntregaEncomiendas.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_EntregaEncomiendas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 342px" colSpan="2"><b>Información de la encomienda:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 16px"><asp:label id="Label15" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Entrega :</asp:label></td>
			<td style="WIDTH: 184px; HEIGHT: 16px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
			<TD style="HEIGHT: 16px" rowSpan="3">&nbsp;
				<asp:label id="lblRuta" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Ruta :</asp:label></td>
			<td style="WIDTH: 184px; HEIGHT: 18px"><asp:dropdownlist id="ddlRuta" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
			</TD>
		</tr>
		<asp:Panel ID="pnlDocumento" Runat="server" Visible="False">
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 1px">
					<asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Documento :</asp:label></TD>
				<TD style="WIDTH: 184px; HEIGHT: 1px">
					<asp:dropdownlist id="ddlDocumento" runat="server" Font-Size="XX-Small" Width="150px" AutoPostBack="True"></asp:dropdownlist></TD>
			</TR>
		</asp:Panel>
		<TR>
			<TD style="WIDTH: 342px" align="center" colSpan="2">&nbsp;</TD>
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
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos de la Encomienda:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Numero Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblNumDocumento" Font-Size="XX-Small" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Recepcion :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblFechaR" Font-Size="XX-Small" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Entrega :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblFechaE" Font-Size="XX-Small" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Descripcion :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblDescripcion" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Unidades :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblUnidades" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label29" runat="server" Font-Size="XX-Small" Font-Bold="True">Peso Total (lbs) :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblPeso" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label31" runat="server" Font-Size="XX-Small" Font-Bold="True">Volumen Total (mts. cubicos) :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblVolumen" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label33" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Declarado :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblAvaluo" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Total Encomienda :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblCostoEncomienda" runat="server" Font-Size="Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top" align="right"></TD>
				<TD style="WIDTH: 154px" vAlign="top" align="left"><BR>
					<asp:button id="btnRegistrar" Font-Size="XX-Small" Font-Bold="True" Width="126px" Runat="server"
						Text="Entregar Encomienda"></asp:button><BR>
				</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel></DIV>
<br>
<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
