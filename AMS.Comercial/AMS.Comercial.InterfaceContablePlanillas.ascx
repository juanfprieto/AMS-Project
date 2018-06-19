<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.InterfaceContablePlanillas.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_InterfaceContablePlanillas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<DIV align="center">
	<table style="WIDTH: 868px" align="center">
		<tr>
			<td style="WIDTH: 868px"><b>Información de las planillas:</b></td>
		</tr>
		<TR>
			<td>
				<TABLE style="WIDTH: 868px; HEIGHT: 40px">
					<TBODY>
						<tr>
							<td>Año :
							</td>
							<td><asp:dropdownlist id="ddlAno" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
							<td>Mes :
							</td>
							<td><asp:dropdownlist id="ddlMes" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
							<td>Dia Inicial :
							</td>
							<td><asp:dropdownlist id="ddlDiaInicial" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
							<td>Dia Final :
							</td>
							<td><asp:dropdownlist id="ddlDiaFinal" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
							<TD align="center"><asp:button id="btnConsultar" Font-Size="XX-Small" Width="100px" Text="Consultar" Runat="server"
									Font-Bold="True"></asp:button></TD>
							<TD align="center">&nbsp;&nbsp;&nbsp;</TD>
						</tr>
						<tr>
							<td colSpan="10"><asp:datagrid id="dgMovs" runat="server" Width="800" AutoGenerateColumns="True"></asp:datagrid></td>
						</tr>
					</TBODY>
				</TABLE>
				<br>
			</TD>
		</TR>
		<tr>
			<td style="WIDTH: 868px" colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<asp:button id="CmdContabilizar" runat="server" Text="Contabilizar" Visible="False"></asp:button>&nbsp;
	<asp:button id="btnCancelar" Width="184px" Text="Cancelar o Reiniciar Proceso" Runat="server"
		CausesValidation="False" Visible="False"></asp:button>&nbsp;
	<asp:linkbutton id="lnkExportarExcel" runat="server" Visible="False">Exportar Excel</asp:linkbutton><INPUT id="hdSvrErrores" type="hidden" name="hdSvrErrores" runat="server">
	<input id="hdnCont" type="hidden" name="hdnCont" runat="server"><br>
	<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
</DIV>
