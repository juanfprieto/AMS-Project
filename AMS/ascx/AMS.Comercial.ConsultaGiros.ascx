<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultaGiros.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultaGiros" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información de los giros:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Clase :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlTipo" Font-Size="XX-Small" runat="server">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="M">Manual</asp:ListItem>
					<asp:ListItem Value="V">Virtual</asp:ListItem>
				</asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Número :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">desde:<asp:textbox id="txtInicioDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'></asp:textbox>&nbsp;&nbsp;hasta:<asp:textbox id="txtFinDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Origen :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlAgenciaO" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Destino :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlAgenciaD" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Entregados :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:dropdownlist id="ddlEntregados" Font-Size="XX-Small" runat="server">
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
					<TD style="WIDTH: 545px" colSpan="3"><B>Giros:</B></TD>
				</TR>
				<TR>
					<TD align="center">
						<asp:datagrid id="dgrPapeleria" runat="server" AutoGenerateColumns="False" AllowPaging="True"
							PageSize="30">
							<FooterStyle BackColor="#CCCCCC"></FooterStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="NUM_DOCUMENTO" HeaderText="No." DataFormatString="{0:000000}"></asp:BoundColumn>
								<asp:BoundColumn DataField="TIPO_DOCUMENTO" HeaderText="Clase"></asp:BoundColumn>
								<asp:BoundColumn DataField="AGENCIA_O" HeaderText="Origen"></asp:BoundColumn>
								<asp:BoundColumn DataField="AGENCIA_D" HeaderText="Destino"></asp:BoundColumn>
								<asp:BoundColumn DataField="MRUT_CODIGO" HeaderText="Ruta"></asp:BoundColumn>
								<asp:BoundColumn DataField="MPLA_CODIGO" HeaderText="Planilla" ItemStyle-HorizontalAlign=Left></asp:BoundColumn>
								<asp:BoundColumn DataField="MNIT_EMISOR" HeaderText="Emisor" ItemStyle-HorizontalAlign=Left></asp:BoundColumn>
						     	<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_EMISOR" HeaderText="Emisor"></asp:BoundColumn>
								<asp:BoundColumn DataField="MNIT_DESTINATARIO" HeaderText="Destinatario" ItemStyle-HorizontalAlign=Left></asp:BoundColumn>
						        <asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_DESTINATARIO" HeaderText="Destinatario"></asp:BoundColumn>
								<asp:BoundColumn DataField="VALOR_IVA" HeaderText="IVA" ItemStyle-HorizontalAlign=Left></asp:BoundColumn>
								<asp:BoundColumn DataField="COSTO_GIRO" HeaderText="Costo" DataFormatString="{0:#,#}" ItemStyle-HorizontalAlign=Left></asp:BoundColumn>
								<asp:BoundColumn DataField="VALOR_GIRO" HeaderText="Valor" DataFormatString="{0:#,#}" ItemStyle-HorizontalAlign=Left></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_RECIBE" HeaderText="Recepcion" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FECHA_ENTREGA" HeaderText="Entrega" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							</Columns>
							<PagerStyle Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
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