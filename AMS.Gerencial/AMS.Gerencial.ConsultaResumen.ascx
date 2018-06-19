<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Gerencial.ConsultaResumen.ascx.cs" Inherits="AMS.Gerencial.AMS_Gerencial_ConsultaResumen" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<P>
<P><asp:panel id="Panel1" runat="server" Visible="False">
		<TABLE id="RESUMEN" class="filtersIn">
			<tr>
				<td></td>
				<td borderColor="#0000ff" align="center" bgColor="#0000ff">
					<asp:label id="Label2" runat="server" Font-Italic="True" Font-Size="Medium" ForeColor="White"
						Font-Bold="True">AÑO</asp:label></td>
				<td align="right" bgColor="#0000ff">
					<asp:label id="Label3" runat="server" Font-Italic="True" Font-Size="Medium" ForeColor="White"
						Font-Bold="True">MES</asp:label></td>
				<td bgColor="#0000ff"></td>
			</tr>
			<tr>
				<td align="left">
					<asp:label id="Label1" runat="server" Font-Size="XX-Small">Ventas por Mostrador</asp:label></td>
				<td align="right">
					<asp:label id="ventasano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="ventasmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label4" runat="server" Font-Size="XX-Small">Transferencias a Taller</asp:label></td>
				<td align="right">
					<asp:label id="transano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="transmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" ForeColor="Red" Font-Bold="True">Utilidad Bruta</asp:label></td>
				<td align="right">
					<asp:label id="utilano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="utilmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label6" runat="server" Font-Size="XX-Small">% Margen Utilidad Financiero</asp:label></td>
				<td align="right">
					<asp:label id="utilfinano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="utilfinmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label7" runat="server" Font-Size="XX-Small">% Margen Utilidad Real</asp:label></td>
				<td align="right">
					<asp:label id="utilrealano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="utilrealmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<HR width="100%" color="#0000ff" SIZE="3">
					&nbsp;</td>
				<td>
					<HR width="100%" color="#0000ff" SIZE="3">
					&nbsp;</td>
				<td>
					<HR width="100%" color="#0000ff" SIZE="3">
					&nbsp;</td>
				<td>
					<HR width="100%" color="#0000ff" SIZE="3">
					&nbsp;</td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label9" runat="server" Font-Size="XX-Small" ForeColor="Red" Font-Bold="True">INVENTARIO INICIAL</asp:label></td>
				<td align="right">
					<asp:label id="InvInicial" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="InvinicialMes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label10" runat="server" Font-Size="XX-Small">Compras</asp:label></td>
				<td align="right">
					<asp:label id="compano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="compmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label11" runat="server" Font-Size="XX-Small">Ajustes</asp:label></td>
				<td align="right">
					<asp:label id="ajusano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="ajusmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label12" runat="server" Font-Size="XX-Small">Costos Ventas Mostrador</asp:label></td>
				<td align="right">
					<asp:label id="cosvenano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="cosvenmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="label" runat="server" Font-Size="XX-Small">Costos Consumos Internos</asp:label></td>
				<td align="right">
					<asp:label id="coninano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="coninmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label13" runat="server" Font-Size="XX-Small">Costo Transferencia a Taller</asp:label></td>
				<td align="right">
					<asp:label id="trantano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="trantmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label8" runat="server" Font-Size="XX-Small" ForeColor="Red" Font-Bold="True">INVENTARIO FINAL</asp:label></td>
				<td align="right">
					<asp:label id="invfinano" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td align="right">
					<asp:label id="invfinalmes" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
				<td></td>
			</tr>
			<tr>
				<td>
					<HR width="100%" color="#0000ff" SIZE="3">
					&nbsp;</td>
				<td>
					<HR width="100%" color="#0000ff" SIZE="3">
					&nbsp;</td>
				<td>
					<HR width="100%" color="#0000ff" SIZE="3">
					&nbsp;</td>
				<td>
					<HR width="100%" color="#0000ff" SIZE="3">
					&nbsp;</td>
			</tr>
			<tr>
				<td></td>
				<td></td>
				<td align="left">
					<asp:label id="Label15" runat="server" Font-Size="XX-Small" Font-Bold="True">Inventario Costo Promedio</asp:label></td>
				<td align="right">
					<asp:label id="invcostp" runat="server" Font-Size="XX-Small" Font-Bold="True">0</asp:label></td>
			</tr>
			<tr>
				<td></td>
				<td></td>
				<td>
					<asp:Label id="Label19" runat="server" Font-Size="XX-Small" Font-Bold="True" Width="146px">Inventario Con Venta Ultimo Semestre:</asp:Label></td>
				<td align="right">
					<asp:TextBox id="invalro" runat="server" Font-Bold="True" Width="134px" BackColor="Lime" align="right"></asp:TextBox></td>
			</tr>
			<tr>
				<td>
					<asp:label id="Label14" runat="server" Font-Size="X-Small" ForeColor="Red" Font-Bold="True">Inventario Sin Venta Mas</asp:label></td>
				<td align="right">
					<asp:textbox id="inv2anos" runat="server" Font-Size="X-Small" Font-Bold="True" Width="120px"
						BackColor="DeepPink" ReadOnly="True"></asp:textbox></td>
				<td align="right">
					<asp:textbox id="inv1ano" runat="server" Font-Size="X-Small" Font-Bold="True" Width="120px" BackColor="#FF8000"
						ReadOnly="True"></asp:textbox></td>
				<td align="right">
					<asp:textbox id="inv6meses" runat="server" Font-Size="X-Small" Font-Bold="True" Width="120px"
						BackColor="Yellow" ReadOnly="True"></asp:textbox></td>
			</tr>
			<tr>
				<td></td>
				<td align="center">
					<asp:label id="Label16" runat="server" Font-Size="XX-Small" Font-Bold="True">Sin Venta Mas De 2 Años </asp:label></td>
				<td align="center">
					<asp:label id="Label17" runat="server" Font-Size="XX-Small" Font-Bold="True">Sin Venta Mas De 1 Año</asp:label></td>
				<td align="center">
					<asp:label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Sin Venta Mas De 6 Meses</asp:label></td>
			</tr>
		</TABLE>
	</asp:panel></P>
