<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AnulacionPapeleria.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_AnulacionPapeleria" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="filtersIn">
		<tr>
			<td colSpan="3"><b>Información de la anulación:</b></td>
		</tr>
		<tr>
			<td><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Número de Anulación :</asp:label></td>
			<td><asp:label id="lblNumero" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></td>
			<td><asp:label id="lblFecha" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD colSpan="1">&nbsp;</TD>
		</TD>
		<tr>
			<td colSpan="1">&nbsp;</td>
			<TD vAlign="top"><asp:button id="btnDocumento" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Por Documento"></asp:button>&nbsp;&nbsp;&nbsp;</TD>
			<TD vAlign="top"><asp:button id="btnRango" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Por Rango"
					Width="65px"></asp:button></TD>
		</tr>
	</table>
	<br>
	<asp:panel id="pnlunoauno" runat="server" Visible="false">
		<TABLE id="Table2" class="filtersIn">
			<TR>
				<TD colSpan="3"><B>Documentos:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrDocumentos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" ShowFooter="True">
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" cssclass="item"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" cssclass="header"></HeaderStyle>
						<FooterStyle cssclass="footer"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="Linea"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Doc.">
								<ItemTemplate>
									<asp:dropdownlist id="ddlTipoDocumento" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Numero">
								<ItemTemplate>
									<asp:textbox id="txtNumeroDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'>
									</asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Concepto">
								<ItemTemplate>
									<asp:dropdownlist id="ddlConceptoAnulacion" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:button id="btnAnular" Font-Size="XX-Small" Font-Bold="True" Text="Anular" Runat="server"></asp:button></TD>
			</TR>
		</TABLE>
	</asp:panel><br>
	<asp:panel id="pnlBuscar" runat="server" Visible="false">
		<TABLE id="Table3" class="filtersIn">
			<TR>
				<TD colSpan="8"><B>Por rango :</B></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo Documento :</asp:label></TD>
				<TD>
					<asp:dropdownlist id="ddlTipos" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
				<TD>
					<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero Inicial :</asp:label></TD>
				<TD>
					<asp:textbox id=txtNumeroInicial Font-Size="XX-Small" Runat="server" Width="70px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>">
					</asp:textbox></TD>
				<TD>
					<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero Final :</asp:label></TD>
				<TD>
					<asp:textbox id=txtNumeroFinal Font-Size="XX-Small" Runat="server" Width="70px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>">
					</asp:textbox></TD>
				<TD>
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Concepto Anulacion :</asp:label></TD>
				<TD>
					<asp:dropdownlist id="ddlConceptoAnular" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
				<TD>
					<asp:button id="btnBuscar" Font-Size="XX-Small" Font-Bold="True" Text="Buscar" Runat="server"></asp:button></TD>
			<TR>
				<TD colSpan="2"></TD>
			</TR>
		</TABLE>
	</asp:panel><asp:panel id="pnlRango" runat="server" Visible="false">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD colSpan="3"><B>Documentos a Anular :</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrAnulacion" runat="server" Width="760px" AutoGenerateColumns="False" ShowFooter="False"
						AllowPaging="True" PageSize="30">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="TIPO_DOCUMENTO" HeaderText="Tipo"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="Documento"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_RECEPCION" DataFormatString="{0:yyyy-MM-dd}"
								HeaderText="Fecha Recepcion"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_DESPACHO" DataFormatString="{0:yyyy-MM-dd}"
								HeaderText="Fecha Despacho"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_ASIGNACION" DataFormatString="{0:yyyy-MM-dd}"
								HeaderText="Fecha Asignacion"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MNIT_RESPONSABLE" HeaderText="Nit Responsable"></asp:BoundColumn>
						</Columns>
						<PagerStyle Mode="NumericPages"></PagerStyle>
					</asp:datagrid></TD>
			</TR>
			</TR></TD></TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:button id="btnAnularRango" Font-Size="XX-Small" Font-Bold="True" Text="Anular Rango" Runat="server"></asp:button></TD>
			</TR>
		</TABLE>
	</asp:panel>
	<TABLE align="center">
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
	</TABLE>
</DIV>
<script language="javascript">
<%=strActScript%>
</script>
