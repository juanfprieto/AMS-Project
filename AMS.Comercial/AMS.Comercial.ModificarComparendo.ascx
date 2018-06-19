<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarComparendo.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarComparendo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información del comparendo:</b></td>
		</tr>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Número :</asp:label></td>
			<td style="WIDTH: 386px"><asp:TextBox id="txtNumero" runat="server" Font-Size="XX-Small" MaxLength="10" onclick="ModalDialog(this,'SELECT rtrim(char(MC.MCOM_NUMERO)) AS NUMERO,PI.DESCRIPCION AS INFRACCION, MC.MNIT_INFRACTOR AS INFRACTOR, MC.MCAT_PLACA AS PLACA, FECHA_COMPARENDO AS FECHA, VALOR_PAGADO AS VALOR from DBXSCHEMA.MCOMPARENDO MC, DBXSCHEMA.PINFRACCIONES_TRANSITO AS PI WHERE PI.PINF_CODIGO=MC.PINF_CODIGO;', new Array(),1)"
			ReadOnly=False></asp:TextBox></td>
		</TR>
		<TR>
			<TD align="right" colSpan="2">
				<asp:button id="btnSeleccionar" Runat="server" Font-Size="XX-Small" Font-Bold="True" Text="Seleccionar"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
		</TR>
		<asp:Panel Visible="False" Runat="server" ID="pnlDetalle">
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Infracción :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlInfraccion" Font-Size="XX-Small" runat="server"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 107px">
					<asp:Label id="Label13" Font-Bold="True" Font-Size="XX-Small" runat="server">Infractor :</asp:Label></TD>
				<td>
					<asp:TextBox id="txtInfractor" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT', new Array(),1)"
						Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>&nbsp;
					<asp:textbox id="txtInfractora" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label19" Font-Bold="True" Font-Size="XX-Small" runat="server">Dirección :</asp:label></TD>
				<td>
					<asp:TextBox id="txtDireccion" Font-Size="XX-Small" runat="server" MaxLength="100" Width="570px"></asp:TextBox></TD>
			</TR>
			<TR>
				<td>
					<asp:label id="Label20" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa del Bus :</asp:label></TD>
				<td>
					<asp:textbox id="txtPlaca" onclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado', new Array(),1)"
						Font-Size="XX-Small" runat="server" MaxLength="6" Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
				</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label14" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Comparendo :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtFechaComparendo" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="60px"
						Runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Radicación :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtRadicacion" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="60px" Runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Pago :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtFechaPago" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="60px" Runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Pagado :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtValor" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server" MaxLength="11"
						Width="100px" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Autoridad :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlAutoridad" Font-Size="XX-Small" runat="server"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Estado :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlEstado" Font-Size="XX-Small" runat="server"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Observación :</asp:label></TD>
				<td>
					<asp:TextBox id="txtObservacion" Font-Size="XX-Small" runat="server" MaxLength="4000" Width="570px"
						Height="200" TextMode="MultiLine"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD align="center" colspan="2"><asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Modificar"
						Width="84px"></asp:button></TD>
			</TR>
			</asp:Panel>
			<tr>
				<td colSpan="2">&nbsp;</td>
			</tr>
	</table>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
	</TABLE>
</DIV>
