<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Control Language="c#" codebehind="AMS.Nomina.ImpAcumuladoDeptoCentroCosto.cs" autoeventwireup="false" Inherits="AMS.Nomina.ImpAcumuladoDeptoCentroCosto" %>
<p>
	Porfavor Seleccione&nbsp;las opciones&nbsp;para generar el acumulado 
	correspondiente.
</p>
<p>
</p>
<table style="BACKGROUND-COLOR: white" bordercolor="transparent" bgcolor="white" border="0">
	<tbody>
		<tr>
			<td>
				<table style="BACKGROUND-COLOR: white" border="0">
					<tbody>
						<tr>
							<td>
								Departamento:</td>
							<td>
								<asp:DropDownList id="DDLDEPTO" OnSelectedIndexChanged="cambiodepto" AutoPostBack="True" runat="server"
									class="dmediano">
									<asp:ListItem Value="0">Todos los Departamentos</asp:ListItem>
									<asp:ListItem Value="1">Un Departamento</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>
								Concepto:</td>
							<td>
								<asp:DropDownList id="DDLCONCEPTO" AutoPostBack="True" runat="server" Width="196px" onSelectedIndexChanged="cambioconcepto">
									<asp:ListItem Value="0">Todos los Conceptos</asp:ListItem>
									<asp:ListItem Value="1">Un Concepto</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>
								Año:</td>
							<td>
								<asp:DropDownList id="DDLANO" OnSelectedIndexChanged="cambiodepto" runat="server" Width="196px"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>
								<p>
									Quincena:
								</p>
							</td>
							<td>
								<asp:DropDownList id="DDLQUINCENA" runat="server" Width="196px">
									<asp:ListItem Value="0">Ambas Quincenas</asp:ListItem>
									<asp:ListItem Value="1">Primera Quincena</asp:ListItem>
									<asp:ListItem Value="2">Segunda Quincena</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
					</tbody>
				</table>
			</td>
			<td>
				<table BACKGROUND-COLOR: white" border="0">
					<tbody>
						<tr>
							<td>
								<p>
									<asp:Label id="LBDEPTO" runat="server" visible="False">Seleccione un Departamento: </asp:Label>
								</p>
							</td>
							<td>
								<asp:DropDownList id="DDLDEPTOS" runat="server" class="dmediano" Visible="False"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>
								&nbsp;<asp:Label id="LBCONCEPTO" runat="server" visible="False">Seleccione un Concepto:</asp:Label></td>
							<td>
								<asp:DropDownList id="DDLCONCEPTOS" runat="server" class="dmediano" Visible="False"></asp:DropDownList>
							</td>
						</tr>
					</tbody>
				</table>
			</td>
		</tr>
	</tbody>
</table>
<p>
</p>
<p>
	<asp:Button id="BTNMOSTRAR" onclick="btnmostrar" runat="server" Text="MOSTRAR REPORTE"></asp:Button>
</p>
<p>
</p>
<p>
	<CR:CrystalReportViewer id="visor" runat="server" visible="false" Width="350px" Height="50px"></CR:CrystalReportViewer>
</p>
