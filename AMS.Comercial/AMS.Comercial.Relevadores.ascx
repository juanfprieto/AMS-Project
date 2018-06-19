<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.Relevadores.ascx.cs" Inherits="AMS.Comercial.AMS_Relevadores" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td colSpan="3"><h3>Datos Generales:</h3></td>
		</tr>
		<TR>
			<TD>
				<asp:label id="Label2" Font-Bold="True"  runat="server">Relevador :</asp:label>
			</TD>
			<TD colspan="2">
				<asp:textbox id="txtEmpleado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					 runat="server" ReadOnly="True" Width="80px"></asp:textbox>&nbsp;
				<asp:textbox id="txtEmpleadoa"  runat="server" ReadOnly="True" Width="300px"></asp:textbox>
			</TD>
		</TR>
		<TR>
			<TD>
                <asp:label id="Label3" Font-Bold="True"  runat="server">Fecha Desde :</asp:label>
			</TD>
			<TD colSpan="2">
				<asp:textbox id="txtFechaDesde" onkeyup="DateMask(this)"  runat="server" ReadOnly="False" Width="80px"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD>
                <asp:label id="Label1" Font-Bold="True"  runat="server">Fecha Hasta :</asp:label>
            </TD>
			<TD colSpan="2">
				<asp:textbox id="txtFechaHasta" onkeyup="DateMask(this)"  runat="server" ReadOnly="False" Width="80px"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD colSpan="3">
				<asp:button id="btnAgregar" Runat="server" Font-Bold="True"  Text="Agregar"></asp:button>
            </TD>
        </TR>
		<TR>
			<TD colSpan="3">
				<asp:ListBox id="lstRelevadores"  runat="server" Width="744px" Height="500px"></asp:ListBox></TD>
		</TR>
		<TR>
			<TD colSpan="3">
				<asp:button id="btnEliminar" Runat="server" Font-Bold="True"  Text="Eliminar"></asp:button></TD>
		</TR>
		<TR>
			<TD  align="left" colSpan="3">
				<asp:label id="lblError" Font-Bold="True"  runat="server"></asp:label>
            </TD>
		</TR>
	</table>
</fieldset>
<script language="javascript">
</script>

