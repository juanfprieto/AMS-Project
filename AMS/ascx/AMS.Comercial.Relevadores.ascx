<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.Relevadores.ascx.cs" Inherits="AMS.Comercial.AMS_Relevadores" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Datos Generales:</b></td>
		</tr>
		<TR>
			<TD colSpan="3">&nbsp;</TD>
		</TR>
		<TR>
			<TD>
				<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Relevador :</asp:label>
			</TD>
			<TD>
				<asp:textbox id="txtEmpleado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					Font-Size="XX-Small" runat="server" ReadOnly="True" Width="80px"></asp:textbox>&nbsp;
				<asp:textbox id="txtEmpleadoa" Font-Size="XX-Small" runat="server" ReadOnly="True" Width="300px"></asp:textbox>&nbsp;
			</TD>
			<TD></td>
		</TR>
		<TR>
			<TD><asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Desde :</asp:label>
			</TD>
			<TD colSpan="2">
				<asp:textbox id="txtFechaDesde" onkeyup="DateMask(this)" Font-Size="XX-Small" runat="server"
					ReadOnly="False" Width="80px"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Hasta :</asp:label></TD>
			<TD colSpan="2">
				<asp:textbox id="txtFechaHasta" onkeyup="DateMask(this)" Font-Size="XX-Small" runat="server"
					ReadOnly="False" Width="80px"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD colSpan="3">
				<asp:button id="btnAgregar" Runat="server" Font-Bold="True" Font-Size="XX-Small" Text="Agregar"></asp:button></TD></TD></TR>
		<TR>
			<TD colSpan="3">
				<asp:ListBox id="lstRelevadores" Font-Size="XX-Small" runat="server" Width="744px" Height="500px"></asp:ListBox></TD>
		</TR>
		<TR>
			<TD colSpan="3">
				<asp:button id="btnEliminar" Runat="server" Font-Bold="True" Font-Size="XX-Small" Text="Eliminar"></asp:button></TD>
		</TR>
		<TR>
			<TD colSpan="3">&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 545px" align="left" colSpan="3">
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>&nbsp;</TD>
		</TR>
		<TR>
			<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
		</TR>
	</table>
</DIV>
<script language="javascript">
</script>

