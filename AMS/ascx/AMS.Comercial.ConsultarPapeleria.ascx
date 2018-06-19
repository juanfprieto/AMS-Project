<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultarPapeleria.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultarPapeleria" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Número :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">desde:<asp:textbox id="txtInicioDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'></asp:textbox>&nbsp;&nbsp;hasta:<asp:textbox id="txtFinDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Clase :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlClaseDocumento" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="M" Selected="True">Manual</asp:ListItem>
					<asp:ListItem Value="V">Virtual</asp:ListItem>
				</asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Despachados :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlDespachados" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Asignados :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlAsignados" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:dropdownlist>&nbsp;&nbsp;Nit
				<asp:textbox id="txtResponsable" ondblclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="False"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Devueltos :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlDevueltos" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Anulados :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlAnulados" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label10" Font-Bold="True" Font-Size="XX-Small" runat="server">Planillados :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlPlanillados" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:dropdownlist>&nbsp;&nbsp;No.<asp:textbox id="txtPlanilla" Font-Size="XX-Small" runat="server" Width="80px"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Usados :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlUsados" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:dropdownlist>
			</td>
		</tr>
		<TR>
			<TD align="right" colspan="2"><asp:button id="btnConsultar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Consultar"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
		</TR>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<asp:Panel Runat="server" Visible="False" ID="pnlPapeleria">
		<TABLE style="WIDTH: 773px" align="center">
			<TBODY>
				<TR>
					<TD style="WIDTH: 545px" colSpan="3"><B>Papelería:</B></TD>
				</TR>
				<TR>
					<TD align="center">
						<asp:datagrid id="dgrPapeleria" runat="server" PageSize="30" AllowPaging="True" AutoGenerateColumns="False">
							<FooterStyle BackColor="#CCCCCC"></FooterStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="TDOC_CODIGO" HeaderText="Tipo"></asp:BoundColumn>
								<asp:BoundColumn DataField="NUM_DOCUMENTO" HeaderText="No."></asp:BoundColumn>
								<asp:BoundColumn DataField="TIPO_DOCUMENTO" HeaderText="Clase"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_RECEPCION" HeaderText="Recepci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_DESPACHO" HeaderText="Despacho" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="AGENCIA" HeaderText="Agencia"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_ASIGNACION" HeaderText="Asignaci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="MNIT_RESPONSABLE" HeaderText="Responsable"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_DEVOLUCION" HeaderText="Devoluci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_ANULACION" HeaderText="Anulaci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="MPLA_CODIGO" HeaderText="Planilla"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_USO" HeaderText="Uso" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="MRUT_CODIGO" HeaderText="Ruta"></asp:BoundColumn>
							</Columns>
							<PagerStyle Mode="NumericPages"></PagerStyle>
						</asp:datagrid><BR>
						*Agencia preasignada</TD>
				</TR>
				<TR>
					<td>&nbsp;</TD>
				</TR>
	</asp:Panel>
	<TR>
		<td>&nbsp;
			<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
	</TR>
	</TBODY></TABLE>
</DIV>
<script language="javascript">
<%=strActScript%>
</script>
