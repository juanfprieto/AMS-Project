<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.DevolucionPapeleriaAgencia.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_DevolucionPapeleriaAgencia" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información de la devolución:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Número de Anulación :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:label id="lblNumero" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:label id="lblFecha" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<TD style="WIDTH: 545px" colSpan="3"><B>Documentos:</B></TD>
		</TR>
		<TR>
			<TD align="center"><asp:datagrid id="dgrDocumentos" runat="server" ShowFooter="True" AutoGenerateColumns="False">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn DataField="NUMERO" HeaderText="Linea"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Doc.">
							<ItemTemplate>
								<asp:dropdownlist id="ddlTipoDocumento" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Inicio">
							<ItemTemplate>
								<asp:textbox id="txtInicioDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'></asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Fin">
							<ItemTemplate>
								<asp:textbox id="txtFinDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'></asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Concepto">
							<ItemTemplate>
								<asp:dropdownlist id="ddlConceptoDevolucion" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
		<TR>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center"><asp:button id="btnDevolver" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Devolver"></asp:button></TD>
		</TR>
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
	</TABLE>
</DIV>
<script language="javascript">
<%=strActScript%>
</script>
