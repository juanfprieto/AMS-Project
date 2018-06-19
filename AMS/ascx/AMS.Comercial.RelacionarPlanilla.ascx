<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RelacionarPlanilla.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RelacionarPlanilla" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Información general:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" Width="150px" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<asp:panel id="pnlTipoAsociar" Runat="server" Visible="False">
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:dropdownlist id="ddlTipoAsociar" runat="server" Font-Size="XX-Small" AutoPostBack="True" Width="150px">
						<asp:ListItem Value="---seleccione---">---seleccione---</asp:ListItem>
						<asp:ListItem Value="Giros">Giros</asp:ListItem>
						<asp:ListItem Value="Encomiendas">Encomiendas</asp:ListItem>
						<asp:ListItem Value="Anticipos/servicios">Anticipos/servicios</asp:ListItem>
					</asp:dropdownlist></TD>
			</TR>
			<asp:panel id="pnlPlanilla" Visible="False" Runat="server">
				<TR>
					<TD>
						<asp:label id="Label35" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero de Planilla :</asp:label></TD>
					<TD style="WIDTH: 281px">
						<asp:textbox id="txtNumeroDocumento" runat="server" Font-Size="XX-Small" Width="80px" MaxLength="7"></asp:textbox></TD>
				</TR>
			</asp:panel>
		</asp:panel>
		<TR>
			<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
		</TR>
	</table>
	<br>
	<asp:Panel ID="pnlElemento" Runat="server" Visible="false">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Información del
						<asp:label id="lblTipo" runat="server"></asp:label>:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="lblTipoDesc" runat="server"></asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:Label id="lblTipoExp" Font-Size="XX-Small" Runat="server"></asp:Label><BR>
					<asp:dropdownlist id="ddlNumeroTipo" runat="server" Font-Size="XX-Small" AutoPostBack="True"></asp:dropdownlist></TD>
			</TR>
			<asp:Panel id="pnlInfoElemento" Visible="false" Runat="server">
				<TR>
					<TD colSpan="2">
						<asp:label id="lblTipoInfo" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<asp:button id="btnAsociar" Font-Size="XX-Small" Font-Bold="True" Width="100px" Runat="server"
							Text="Asociar"></asp:button></TD>
				</TR>
			</asp:Panel>
			<TR>
				<TD style="WIDTH: 545px" align="left" colSpan="2">
					<asp:label id="lblError" runat="server"></asp:label>&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:Panel>
</DIV>
