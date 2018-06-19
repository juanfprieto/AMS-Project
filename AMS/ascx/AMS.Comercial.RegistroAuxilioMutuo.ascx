<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RegistroAuxilioMutuo.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RegistroAuxilioMutuo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Información del Acta:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 14px"><asp:label id="Label15" Font-Bold="True" Font-Size="XX-Small" runat="server">No. Acta :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 14px"><asp:textbox id="txtNumActa" Font-Size="XX-Small" runat="server" Width="80px"></asp:textbox></td>
			<TD style="HEIGHT: 14px">&nbsp;</TD>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">No. Siniestro :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:textbox id="txtNumSiniestro" Font-Size="XX-Small" runat="server" Width="80px"></asp:textbox></td>
			</TD>
			<TD>&nbsp;</TD>
		</tr>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa del bus :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px"><asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0', new Array(),1);"
					runat="server" Font-Size="XX-Small" Width="80px" MaxLength="6"></asp:textbox>&nbsp;</TD>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD>
				<asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Responsable :</asp:label></TD>
			<TD>
				<asp:textbox id="txtResponsable" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" Font-Size="XX-Small" Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
				<asp:textbox id="txtResponsablea" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top">
				<asp:label id="Label14" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Siniestro :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtValorSiniestro" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
					Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top">
				<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Auxilio :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtValorAuxilio" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
					Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Siniestro :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtFechaSiniestro" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server"
					Width="60px"></asp:textbox></TD>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Acta :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtFechaActa" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server" Width="60px"></asp:textbox></TD>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD></TD>
			<TD align="left">
				<asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Width="87px"
					Text="Crear"></asp:button></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 545px" align="center" colSpan="3">&nbsp;</TD>
		</TR>
	</table>
	<br>
</DIV>
<br>
<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
