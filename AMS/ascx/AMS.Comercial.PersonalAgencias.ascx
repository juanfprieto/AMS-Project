<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PersonalAgencias.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PersonalAgencias" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Datos Generales:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
		</tr>
		<asp:panel id="pnlCargo" Runat="server" Visible="False">
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Cargo :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:dropdownlist id="ddlCargo" runat="server" Font-Size="XX-Small" AutoPostBack="True"></asp:dropdownlist></TD>
			</TR>
			<asp:Panel id="pnlEmpleados" Visible="False" Runat="server">
				<TR>
					<TD colSpan="3">&nbsp;</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 130px; HEIGHT: 18px" colSpan="3">
						<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Empleados :</asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="3">
						<asp:textbox id="txtEmpleado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
							runat="server" Font-Size="XX-Small" Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
						<asp:textbox id="txtEmpleadoa" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox>&nbsp;
						<asp:button id="btnAgregar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Agregar"></asp:button></TD>
					</TD></TR>
				<TR>
					<TD colSpan="3">
						<asp:ListBox id="lstEmpleados" Font-Size="XX-Small" runat="server" Width="420px" Height="500px"></asp:ListBox></TD>
				</TR>
				<TR>
					<TD colSpan="3">
						<asp:button id="btnEliminar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Eliminar"></asp:button></TD>
				</TR>
				<TR>
					<TD colSpan="3">&nbsp;</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 545px" align="left" colSpan="3">
						<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label>&nbsp;</TD>
				</TR>
			</asp:Panel>
		</asp:panel>
		<TR>
			<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
		</TR>
	</table>
</DIV>
<script language="javascript">
</script>
