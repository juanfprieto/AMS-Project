<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.CrearComparendo.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_CrearComparendo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información del comparendo:</b></td>
		</tr>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Número :</asp:label></td>
			<td style="WIDTH: 386px"><asp:TextBox id="txtNumero" runat="server" Font-Size="XX-Small" MaxLength="10"></asp:TextBox></td>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Infracción :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlInfraccion" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</TR>
		<tr>
			<td style="WIDTH: 107px"><asp:Label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Infractor :</asp:Label></td>
			<td><asp:TextBox ReadOnly="True" id="txtInfractor" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" Width="80px" Font-Size="XX-Small"></asp:TextBox>&nbsp;
				<asp:textbox id="txtInfractora" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></td>
		</tr>
		<TR>
			<td style="WIDTH: 154px" vAlign="top"><asp:label id="Label19" runat="server" Font-Size="XX-Small" Font-Bold="True">Dirección :</asp:label></td>
			<td><asp:TextBox id="txtDireccion" runat="server" Font-Size="XX-Small" MaxLength="100" Width="570px"></asp:TextBox></TD>
		</TR>
		<TR>
			<td>
				<asp:label id="Label20" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa del Bus :</asp:label></TD>
			<td>
				<asp:textbox id="txtPlaca" onclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0;', new Array(),1)"
					Font-Size="XX-Small" runat="server" Width="80px" MaxLength="6" ReadOnly="True"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top">
				<asp:label id="Label14" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Comparendo :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtFechaComparendo" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small"
					Width="60px"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top">
				<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Radicación :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtRadicacion" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small"
					Width="60px"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top">
				<asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Pago :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtFechaPago" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small" Width="60px"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top">
				<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Pagado :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtValor" onkeyup="NumericMask(this)" runat="server" Font-Size="XX-Small" Width="100px"
					ReadOnly="False" MaxLength="11"></asp:textbox></TD>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Autoridad :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlAutoridad" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Estado :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlEstado" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</TR>
		<TR>
			<td style="WIDTH: 154px" vAlign="top"><asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Observación :</asp:label></td>
			<td><asp:TextBox id="txtObservacion" runat="server" Font-Size="XX-Small" MaxLength="4000" TextMode="MultiLine"
					Width="570px" Height="200"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD align="center" colspan="2"><asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Crear"
					Width="84px"></asp:button></TD>
		</TR>
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
