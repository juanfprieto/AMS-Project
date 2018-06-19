<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.LocalizacionBuses.ascx.cs" Inherits="AMS.Comercial.Comercial_LocalizacionBuses" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información del Bus:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" Width="150px" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<asp:panel id="pnlAgencia" Runat="server" Visible="False">
			<TR>
				<td>
					<asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa del Bus :</asp:label></TD>
				<td>
					<asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT distinct mcat_placa AS Placa,mbus_numero as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0;', new Array(),1)"
						runat="server" Font-Size="XX-Small" Width="80px" MaxLength="6"></asp:textbox>&nbsp;
				</TD>
			</TR>
			<TR>
				<td>
					<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Bus :</asp:label></TD>
				<td>
					<asp:textbox id="txtPlacaa" runat="server" Font-Size="XX-Small" Width="80px" MaxLength="6" ReadOnly="True"></asp:textbox>&nbsp;
				</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 17px">
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Estado :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 17px">
					<asp:dropdownlist id="ddlEstado" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label23" runat="server" Font-Size="XX-Small" Font-Bold="True">Comentarios:</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:textbox id="txtComentarios" runat="server" Font-Size="XX-Small" Width="250px" Height="60px"
						TextMode="MultiLine"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px"></TD>
				<TD align="left">
					<asp:button id="btnSeleccionar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Localizar Bus en Agencia"></asp:button></TD>
			</TR>
		</asp:panel>
		<TR>
			<TD style="WIDTH: 545px" align="left" colSpan="3"><asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>&nbsp;</TD>
		</TR>
		<TR>
			<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
		</TR>
	</table>
</DIV>
<script language="javascript">
</script>
