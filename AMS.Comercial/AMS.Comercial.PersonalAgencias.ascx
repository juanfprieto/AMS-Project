<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PersonalAgencias.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PersonalAgencias" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td><h3>Datos Generales:</h3></td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label4" Font-Bold="True" runat="server">Agencia :</asp:label>
            </td>
			<td>
                <asp:dropdownlist id="ddlAgencia" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist>
            </td>
		</tr>
		<asp:panel id="pnlCargo" Runat="server" Visible="False">
			<TR>
				<TD>
					<asp:label id="Label12" runat="server" Font-Bold="True">Cargo :</asp:label>
                </TD>
				<TD>
					<asp:dropdownlist id="ddlCargo" runat="server" AutoPostBack="True"></asp:dropdownlist>
                </TD>
			</TR>
			<asp:Panel id="pnlEmpleados" Visible="False" Runat="server">				
				<TR>
					<TD colSpan="2">
						<asp:label id="Label2" runat="server" Font-Bold="True">Empleados :</asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<asp:textbox id="txtEmpleado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
							runat="server" Width="80px" ReadOnly="True"></asp:textbox>
						<asp:textbox id="txtEmpleadoa" runat="server" Width="300px" ReadOnly="True"></asp:textbox>
						<asp:button id="btnAgregar" Font-Bold="True" Runat="server" Text="Agregar"></asp:button></TD>
					</TD></TR>
				<TR>
					<TD colSpan="2">
						<asp:ListBox id="lstEmpleados" runat="server" Width="420px" Height="300px"></asp:ListBox></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<asp:button id="btnEliminar" Font-Bold="True" Runat="server" Text="Eliminar"></asp:button></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<asp:label id="lblError" runat="server" Font-Bold="True"></asp:label>&nbsp;</TD>
				</TR>
			</asp:Panel>
		</asp:panel>
	</table>
</fieldset>
<script language="javascript">
</script>
