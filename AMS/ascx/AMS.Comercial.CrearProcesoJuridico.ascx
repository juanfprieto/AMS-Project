<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.CrearProcesoJuridico.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_CrearProcesoJuridico" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información del proceso jurídico:</b></td>
		</tr>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Número :</asp:label></td>
			<td style="WIDTH: 386px"><asp:label id="lblNumero" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></td>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Juzgado :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlJuzgado" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</TR>
		<TR>
			<td><asp:Label id="Label12" runat="server" Font-Bold="True" Font-Size="XX-Small">Código Radicación:</asp:Label></TD>
			<td><asp:TextBox id="txtRadicacion" runat="server" Font-Size="XX-Small" MaxLength="10"></asp:TextBox></TD>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo Proceso Jurídico :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlProceso" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo Subproceso Jurídico :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlSubproceso" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Clase :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlClase" Font-Size="XX-Small" runat="server">
					<asp:ListItem Selected="True">--seleccione--</asp:ListItem>
					<asp:ListItem Value="O">Ordinario</asp:ListItem>
					<asp:ListItem Value="E">Ejecutivo</asp:ListItem>
				</asp:dropdownlist></td>
		</TR>
		<TR>
			<td style="WIDTH: 154px" vAlign="top"><asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Descripción :</asp:label></td>
			<td><asp:TextBox id="txtDescripcion" runat="server" Font-Size="XX-Small" MaxLength="4000" TextMode="MultiLine"
					Width="570px" Height="200"></asp:TextBox></TD>
		</TR>
		<TR>
			<td style="WIDTH: 154px" vAlign="top"><asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Origen :</asp:label></td>
			<td><asp:TextBox id="txtOrigen" runat="server" Font-Size="XX-Small" MaxLength="100" Width="570px"></asp:TextBox></TD>
		</TR>
		<tr>
			<td style="WIDTH: 107px"><asp:Label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Demandante :</asp:Label></td>
			<td><asp:TextBox ReadOnly="True" id="txtDemandante" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" Width="80px" Font-Size="XX-Small"></asp:TextBox>&nbsp;
				<asp:textbox id="txtDemandantea" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:Label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Demandado :</asp:Label></td>
			<td><asp:TextBox ReadOnly="True" id="txtDemandado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" Width="80px" Font-Size="XX-Small"></asp:TextBox>&nbsp;
				<asp:textbox id="txtDemandadoa" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:Label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Abogado Demandante :</asp:Label></td>
			<td><asp:TextBox ReadOnly="True" id="txtAboDemandante" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT IN (SELECT MNIT_NIT FROM DBXSCHEMA.MABOGADOS)', new Array(),1)"
					runat="server" Width="80px" Font-Size="XX-Small"></asp:TextBox>&nbsp;
				<asp:textbox id="txtAboDemandantea" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:Label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Abogado Demandado :</asp:Label></td>
			<td><asp:TextBox ReadOnly="True" id="txtAboDemandado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT IN (SELECT MNIT_NIT FROM DBXSCHEMA.MABOGADOS)', new Array(),1)"
					runat="server" Width="80px" Font-Size="XX-Small"></asp:TextBox>&nbsp;
				<asp:textbox id="txtAboDemandadoa" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></td>
		</tr>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label14" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Prox. Actuación :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtFechaProxAct" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server"
					Width="60px"></asp:textbox></TD>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<td style="WIDTH: 154px" vAlign="top"><asp:label id="Label15" runat="server" Font-Size="XX-Small" Font-Bold="True">Normatividad :</asp:label></td>
			<td><asp:TextBox id="txtNormatividad" runat="server" Font-Size="XX-Small" MaxLength="400" TextMode="MultiLine"
					Width="570px" Height="100"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label16" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha ProcesoJurídico :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtFechaProceso" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server"
					Width="60px"></asp:textbox></TD>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label17" runat="server" Font-Size="XX-Small" Font-Bold="True">Estado :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlEstado" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</TR>
		<TR>
			<td style="WIDTH: 154px"><asp:label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Archivo :</asp:label></td>
			<td style="WIDTH: 386px"><input type="file" id="txtArchivo" runat="server" NAME="txtArchivo" width="40px" Font-Size="XX-Small"></td>
		</TR>
		<TR>
			<TD align="right" colspan="2"><asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Crear"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
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
