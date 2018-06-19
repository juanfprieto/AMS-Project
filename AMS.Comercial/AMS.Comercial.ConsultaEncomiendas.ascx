<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultaEncomiendas.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultaEncomiendas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<TBODY>
			<tr>
				<td style="WIDTH: 258px" colSpan="2"><b> Información de las encomiendas:</b></td>
			</tr>
			<tr>
				<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Clase :</asp:label></td>
				<td style="WIDTH: 144px; HEIGHT: 18px"><asp:dropdownlist id="ddlTipo" runat="server" Font-Size="XX-Small" Width="92px">
						<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
						<asp:ListItem Value="M">Manual</asp:ListItem>
						<asp:ListItem Value="V">Virtual</asp:ListItem>
					</asp:dropdownlist></td>
			</tr>
			<tr>
				<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia Recibe :</asp:label></td>
				<td style="WIDTH: 144px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgenciaO" runat="server" Font-Size="XX-Small" Width="190px"></asp:dropdownlist></td>
				<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Labe13" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia Entrega : </asp:label><asp:dropdownlist id="ddlAgenciaD" runat="server" Font-Size="XX-Small" Width="169px"></asp:dropdownlist></td>
			</tr>
			<tr>
				<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Número desde:</asp:label></td>
				<td style="WIDTH: 144px; HEIGHT: 18px"><asp:textbox id=txtInicioDocumento Font-Size="XX-Small" Width="87px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>" Runat="server"></asp:textbox></td>
				<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Hasta:</asp:label><asp:textbox id=txtFinDocumento Font-Size="XX-Small" Width="86px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>" Runat="server"></asp:textbox></td>
			</tr>
			<tr>
				<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Inicial: </asp:label></td>
				<td style="WIDTH: 144px; HEIGHT: 18px"><asp:textbox id="TextFechaI" onkeyup="DateMask(this)" runat="server" Width="80px"></asp:textbox></td>
				<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Labe44" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Final :</asp:label><asp:textbox id="TextFechaF" onkeyup="DateMask(this)" runat="server" Width="80px"></asp:textbox></td>
				</TD></tr>
			<tr>
				<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa: </asp:label></td>
				<td style="WIDTH: 144px; HEIGHT: 18px"><asp:textbox id="TxtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0', new Array(),1);TraerBus(this.value);"
						runat="server" Font-Size="XX-Small" Width="76px" MaxLength="6" Height="24px"></asp:textbox></td>
				<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Planilla :</asp:label><asp:textbox id="TextPlanilla" runat="server" Width="81px"></asp:textbox></td>
			<tr>
				<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Ruta :</asp:label></td>
				<td style="WIDTH: 144px; HEIGHT: 18px"><asp:dropdownlist id="ddlRutas" runat="server" Font-Size="XX-Small" Width="220px"></asp:dropdownlist></td>
				<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Labe21" runat="server" Font-Size="XX-Small" Font-Bold="True">Entregados :</asp:label>
					<asp:dropdownlist id="ddlEntregados" runat="server" Font-Size="XX-Small" Width="66px">
						<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
						<asp:ListItem Value="S">Si</asp:ListItem>
						<asp:ListItem Value="N">No</asp:ListItem>
					</asp:dropdownlist></td>
			</tr>
		</TBODY>
	</table>
	<table style="WIDTH: 773px" align="center">
		<TR>
			<td style="WIDTH: 120px; HEIGHT: 18px"></td>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<td style="WIDTH: 120px; HEIGHT: 18px"><asp:button id="btnConsultar" Font-Size="XX-Small" Font-Bold="True" Width="86px" Runat="server"
					Text="Consultar"></asp:button></td>
			</TD></TR>
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlPapeleria" Runat="server" Visible="False">
		<TABLE style="WIDTH: 873px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Encomiendas:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrPapeleria" runat="server" PageSize="30" AllowPaging="True" AutoGenerateColumns="False">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="NUM_DOCUMENTO" HeaderText="No." DataFormatString="{0:0000000}"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="TIPO_REMESA" HeaderText="Clase"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MRUT_CODIGO" HeaderText="Ruta"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="AGENCIA_O" HeaderText="AgOrigen"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="AGENCIA_D" HeaderText="AgDestino"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="MPLA_CODIGO" HeaderText="Planilla"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MCAT_PLACA" HeaderText="Placa"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MNIT_RESPONSABLE_RECIBE" HeaderText="NITRspnsble"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_RESPONSABLE" HeaderText="Rspnsble"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MNIT_EMISOR" HeaderText="NITEmisor"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_EMISOR" HeaderText="Emisor"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="TEL_EMISOR" HeaderText="TelEmisor"></asp:BoundColumn>


							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MNIT_DESTINATARIO" HeaderText="NITDestinatario"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_DESTINATARIO" HeaderText="Destinatario"></asp:BoundColumn>

<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="TEL_DESTINATARIO" HeaderText="TelDestinatario"></asp:BoundColumn>

							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_RECIBE" HeaderText="Fecha Rcpcion"
								DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_ENTREGA" HeaderText="Fecha Entrega"
								DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_IVA" HeaderText="IVA"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="COSTO_ENCOMIENDA" HeaderText="Costo"
								DataFormatString="{0:#,#}"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_TOTAL" HeaderText="VlorTtal"
								DataFormatString="{0:#,#}"></asp:BoundColumn>
	<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DESCRIPCION_CONTENIDO" HeaderText="Contenido"></asp:BoundColumn>
						</Columns>
						<PagerStyle Mode="NumericPages"></PagerStyle>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
			</TD></TR></TABLE>
	</asp:panel></DIV>
