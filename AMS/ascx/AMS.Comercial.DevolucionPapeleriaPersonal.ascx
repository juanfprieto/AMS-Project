<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.DevolucionPapeleriaPersonal.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_DevolucionPapeleriaPersonal" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
			<td>
				<asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Asignada a :</asp:label></TD>
			<td>
				<asp:textbox id="txtEncargado" onclick="MostrarPersonal(this,'D');"
					Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
				<asp:textbox id="txtEncargadoa" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></TD>
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
		<asp:Panel Runat="server" ID="pnlDocumentos" Visible="True">
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrDocumentos" runat="server" AutoGenerateColumns="False" ShowFooter="True">
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
				<TD align="center">
					<asp:button id="btnDevolver" Font-Size="XX-Small" Font-Bold="True" Text="Devolver" Runat="server"></asp:button></TD>
			</TR>
		</asp:Panel>
	</TABLE>
	<table style="WIDTH: 773px" align="center">
		<TR>
			<TD align="center"><b><asp:label id="lblResultadoDespacho" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></b></TD>
		</TR>
		<TR>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center">
				<asp:Panel Runat="server" ID="pnlDetallesPapeleria" Visible="False">
					<asp:datagrid id="dgrVerDetalles" runat="server" AutoGenerateColumns="False" ShowFooter="False"
						DataKeyField="TDOC_CODIGO" ShowHeader="False">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="BUS">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									Tiene&nbsp;<%# DataBinder.Eval(Container.DataItem, "TOTAL") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "TDOC_NOMBRE") %>(s)&nbsp;asignad@s&nbsp;sin&nbsp;devolver&nbsp;entre&nbsp;<%# DataBinder.Eval(Container.DataItem, "MIN") %>&nbsp;y&nbsp;<%# DataBinder.Eval(Container.DataItem, "MAX") %>.
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<asp:Button OnClick="btnVerDetalle" Runat=server ID="btnDetalle" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TDOC_CODIGO") %>' Enabled='<%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "TOTAL"))>0) %>' Font-Size="XX-Small" Font-Bold="True" Text="Detalle">
									</asp:Button>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid>
					<BR>
					<asp:Panel id="pnlDetallePapeleria" Runat="server" Visible="False">
						<asp:datagrid id="dgrDetalle" runat="server" AutoGenerateColumns="False">
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<FooterStyle BackColor="#CCCCCC"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="NUM_DOCUMENTO" HeaderText="No."></asp:BoundColumn>
								<asp:BoundColumn DataField="TIPO_DOCUMENTO" HeaderText="Clase"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_RECEPCION" HeaderText="Recepci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_DESPACHO" HeaderText="Despacho" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="AGENCIA" HeaderText="Agencia"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_ASIGNACION" HeaderText="Asignaci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</asp:Panel><br>
					<asp:button id="btnVolver" Font-Size="XX-Small" Font-Bold="True" Text="Volver" Runat="server"></asp:button>
				</asp:Panel>
			</TD>
		</TR>
		<TR>
			<td>&nbsp;</TD>
		</TR>
	</table>
</DIV>
<script language="javascript">
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	function MostrarPersonal(obj,flt){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
<%=strActScript%>
</script>
<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
