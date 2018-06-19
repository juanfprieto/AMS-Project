<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AnulacionTiquetes.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_AnulacionTiquetes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="filtersIn">
		<tr>
			<td colSpan="3"><b>Información del Tiquete:</b></td>
		</tr>
		<TR>
			<td><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
		</TR>
		<tr>
			<td><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Número Tiquete :</asp:label></td>
			<td><asp:textbox id="txtTiquete" runat="server" Font-Size="XX-Small" Width="80px" MaxLength="7"></asp:textbox></td>
		</tr>
		<TR>
			<td><asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Motivo Anulación :</asp:label></td>
			<td><asp:dropdownlist id="ddlMotivo" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
		</TR>
		<TR>
			<TD></TD>
			<TD>
				<asp:button id="btnAnular" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Anular"></asp:button></TD>
		</TR>
		<TR>
			<td align="center" colSpan="2">&nbsp;</td>
		</TR>
	</table>
</DIV>
<script language="javascript">
</script>
<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
