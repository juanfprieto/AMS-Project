<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarRecorrido.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarRecorrido" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table align="center" style="WIDTH: 773px; HEIGHT: 50px">
		<tr>
			<td colspan="2"><b>Información de la ruta:</b>
			</td>
		</tr>
		<tr>
			<td width="84" style="WIDTH: 84px">
				<asp:Label runat="server" Font-Size="XX-Small" Font-Bold="True" id="Label1">Ruta Principal:</asp:Label>
			</td>
			<td>
				<asp:dropdownlist id="ddlRuta" Font-Size="XX-Small" runat="server" Width="150px" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<asp:Panel Runat="server" Visible="False" ID="pnlRuta">
			<TR>
				<td>&nbsp;</TD>
				<td></TD>
			</TR>
			<TR>
				<TD width="84">&nbsp;&nbsp;&nbsp;
					<asp:Label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Descripción:</asp:Label></TD>
				<TD width="84">
					<asp:Label id="lblDesc" Font-Size="XX-Small" runat="server" Width="500px"></asp:Label></TD>
			</TR>
			<TR>
				<TD width="84">&nbsp;&nbsp;&nbsp;
					<asp:Label id="Label21" Font-Bold="True" Font-Size="XX-Small" runat="server">Origen:</asp:Label></TD>
				<TD width="84">
					<asp:Label id="lblOrigen" Font-Size="XX-Small" runat="server" Width="500px"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 14px" width="84">&nbsp;&nbsp;&nbsp;
					<asp:Label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Destino:</asp:Label></TD>
				<TD style="HEIGHT: 14px" width="84">
					<asp:Label id="lblDestino" Font-Bold="False" Font-Size="XX-Small" runat="server" Width="500px"></asp:Label></TD>
			</TR>
		</asp:Panel>
		<tr>
			<td style="WIDTH: 84px">&nbsp;</td>
			<td></td>
		</tr>
	</table>
	<br>
	<asp:Panel Runat="server" Visible="False" ID="pnlSubrutas">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 247px"><B>Ciudades&nbsp;intermedias:</B>
				</TD>
				<td></TD>
			</TR>
			<TR>
				<td>&nbsp;
					<asp:dropdownlist id="ddlCiudad" Font-Size="XX-Small" runat="server"></asp:dropdownlist>&nbsp;
					<asp:button id="btnAgregar" Font-Bold="True" Font-Size="XX-Small" Width="81px" Runat="server"
						Text="Agregar"></asp:button></TD>
				<td></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
				<td></TD>
			</TR>
			<TR>
				<td>Origen:&nbsp;
					<asp:Label id="lblOrigenR" Font-Size="XX-Small" runat="server"></asp:Label></TD>
				<td></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:ListBox id="lstSubRutas" Font-Size="XX-Small" Width="647px" Runat="server" DataValueField="COD"
						DataTextField="NOM" Height="268px"></asp:ListBox></TD>
				<TD vAlign="middle">
					<asp:button id="btnSubir" Font-Bold="True" Font-Size="XX-Small" Width="81px" Runat="server"
						Text="Subir"></asp:button><BR>
					<BR>
					<asp:button id="btnBajar" Font-Bold="True" Font-Size="XX-Small" Width="81px" Runat="server"
						Text="Bajar"></asp:button><BR>
					<BR>
					<asp:button id="btnQuitar" Font-Bold="True" Font-Size="XX-Small" Width="81px" Runat="server"
						Text="Borrar"></asp:button></TD>
			</TR>
			<TR>
				<td>Destino:&nbsp;
					<asp:Label id="lblDestinoR" Font-Size="XX-Small" runat="server"></asp:Label></TD>
				<td></TD>
			</TR>
			<TR>
				<td>
					<asp:CheckBox id="chkInversa" Runat="server" Text="&amp;nbsp;Crear/modificar ruta inversa."></asp:CheckBox></TD>
				<td></TD>
			</TR>
			<TR>
				<td>
					<asp:CheckBox Checked=False id="chkRutas" Runat="server" Text="&amp;nbsp;Asociar rutas al recorrido (elimina actuales)."></asp:CheckBox></TD>
				<td></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2"><BR>
					<asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Width="111px" Runat="server"
						Text="Guardar Recorrido"></asp:button></TD>
				<td></TD>
			<TR>
				<TD style="WIDTH: 247px">&nbsp;
					<asp:Label id="lblError" Font-Size="XX-Small" runat="server"></asp:Label></TD>
				<td></TD>
			</TR>
		</TABLE>
	</asp:Panel>
</DIV>
