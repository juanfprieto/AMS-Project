<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultaServiciosAnticipos.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultaServiciosAnticipos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información de los anticipos/servicios:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Clase :</asp:label></td>
			<td style="WIDTH: 160px; HEIGHT: 18px"><asp:dropdownlist id="ddlTipo" runat="server" Font-Size="XX-Small" Width="92px">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="M">Manual</asp:ListItem>
					<asp:ListItem Value="V">Virtual</asp:ListItem>
				</asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia :</asp:label></td>
			<td style="WIDTH: 160px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small" Width="190px"></asp:dropdownlist></td>
			<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Concepto :</asp:label><asp:dropdownlist id="ddlConcepto"  runat="server" Font-Size="XX-Small" Width="190px"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Número desde:</asp:label></td>
			<td style="WIDTH: 160px; HEIGHT: 18px"><asp:textbox id=txtInicioDocumento Font-Size="XX-Small" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>" Width="87px" Runat="server"></asp:textbox></td>
			<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Hasta : </asp:label><asp:textbox id=txtFinDocumento Font-Size="XX-Small" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>" Width="86px" Runat="server"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Inicial : </asp:label></td>
			<td style="WIDTH: 160px; HEIGHT: 18px"><asp:textbox id="TextFechaI" onkeyup="DateMask(this)" runat="server" Width="80px"></asp:textbox></td>
			<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Labe44" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Final : </asp:label><asp:textbox id="TextFechaF" onkeyup="DateMask(this)" runat="server" Width="80px"></asp:textbox></td>
			</TD></tr>
		<tr>
			<td style="WIDTH: 112px; HEIGHT: 18px"><asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa : </asp:label></td>
			<td style="WIDTH: 160px; HEIGHT: 18px"><asp:textbox id="TxtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0', new Array(),1);TraerBus(this.value);"
					runat="server" Font-Size="XX-Small" MaxLength="6" Width="76px" Height="18px"></asp:textbox></td>
			<td style="WIDTH: 348px; HEIGHT: 18px"><asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Planilla : </asp:label><asp:textbox id="TextPlanilla" runat="server" Width="81px"></asp:textbox></td>
		</tr>
	</table>
	<table style="WIDTH: 773px" align="center">
		<TR>
			<td style="WIDTH: 112px; HEIGHT: 18px"></td>
			<td style="WIDTH: 120px; HEIGHT: 18px"><asp:button id="btnConsultar" Font-Size="XX-Small" Font-Bold="True" Width="86px" Runat="server"
					Text="Consultar"></asp:button></td>
			</TD></TR>
		<TR>
			<td><asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
	</table>
	<br>
	<asp:Panel Runat="server" Visible="False" ID="pnlAnticipos">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Anticipos / Servicios:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrAnticipos" runat="server" Width="760px" AutoGenerateColumns="False" AllowPaging="True"
						PageSize="30" ShowFooter="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="DOCUMENTO"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="TIPO_DOCUMENTO" HeaderText="Tipo"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_DOCUMENTO" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"
								HeaderText="FECHA"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_AGENCIA" HeaderText="CDGO"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_AGENCIA" HeaderText="AGENCIA"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MPLA_CODIGO" HeaderText="PLANILLA"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MCAT_PLACA" HeaderText="PLACA"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="BUS" HeaderText="BUS"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NIT_RESPONSABLE" HeaderText="NIT RESPNSBLE"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_RESPONSABLE" HeaderText="Responsable"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MNIT_RESPONSABLE_RECIBE" HeaderText="NIT RECIBE"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_CONCEPTO" HeaderText="CDGO"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_CONCEPTO" HeaderText="CONCEPTO"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_GASTO"
								HeaderText="VALOR"></asp:BoundColumn>
						</Columns>
						<PagerStyle Mode="NumericPages"></PagerStyle>
					</asp:datagrid></TD>
			</TR>
			</TR></TD></TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:Panel></DIV>
