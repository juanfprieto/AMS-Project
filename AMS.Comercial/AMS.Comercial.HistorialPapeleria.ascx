<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Comercial.HistorialPapeleria.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_HistorialPapeleria" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información de la papelería:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlTipoDocumento" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
	
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Prefijo:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" Visible="false" runat="server"></asp:dropdownlist>
			</td>
		</tr>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top">
				<asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
				<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small" Width="60px"></asp:textbox></TD>
		</TR>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 12px"><asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Movimiento :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 12px">
				<asp:dropdownlist id="ddlMovimiento" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="R">Recepci&#243;n</asp:ListItem>
					<asp:ListItem Value="D">Despacho</asp:ListItem>
					<asp:ListItem Value="A">Asignaci&#243;n</asp:ListItem>
				</asp:dropdownlist>
			</td>
		</tr>
		<TR>
			<TD align="right" colspan="2"><asp:button id="btnConsultar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Consultar" onclick="btnConsultar_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
		</TR>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<TABLE style="WIDTH: 873px" align="center">
		<TBODY>
			<TR>
				<TD style="WIDTH: 545px" ><B>Papelería:</B></TD>
			</TR>
			<<TR>
				<TD align="center" colspan="3">
					<asp:datagrid id="dgrPapeleria" runat="server" AutoGenerateColumns="False">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="X-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="X-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No."></asp:BoundColumn>
							<asp:BoundColumn DataField="TDOC_NOMBRE" HeaderText="Tipo"></asp:BoundColumn>
							<asp:BoundColumn DataField="NUMERO_TALONARIOS" HeaderText="Talonarios"></asp:BoundColumn>
							<asp:BoundColumn DataField="DOCUMENTO_INICIAL" HeaderText="Desde"></asp:BoundColumn>
							<asp:BoundColumn DataField="DOCUMENTO_FINAL" HeaderText="Hasta"></asp:BoundColumn>
							<asp:BoundColumn DataField="FECHA_REPORTE" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							<asp:BoundColumn DataField="MNIT_RESPONSALE" HeaderText="Nit Responsable"></asp:BoundColumn>
							<asp:BoundColumn DataField="NOMBRE_RESPONSABLE" HeaderText="Nombre Responsable"></asp:BoundColumn>
							<asp:BoundColumn DataField="AGENCIA" HeaderText="Agencia"></asp:BoundColumn>
						</Columns>
					</asp:datagrid><BR>
				</TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<td>&nbsp;
					<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
		</TBODY>
	</TABLE>
</DIV>
