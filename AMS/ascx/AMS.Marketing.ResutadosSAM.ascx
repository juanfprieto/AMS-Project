<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Marketing.ResutadosSAM.ascx.cs" Inherits="AMS.Marketing.AMS_Marketing_ResutadosSAM" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<p></p>
<p>
<fieldset>
	<table>
		<tbody>
			<tr>
				<td>Escoja la fecha inicial
				</td>
				<td></td>
				<td>Escoja la fecha final
				</td>
			</tr>
			<tr>
				<td><asp:calendar BackColor=Beige id="fechaInicial" runat="server" SelectedDate="2005-11-20"></asp:calendar></td>
				<td>&nbsp;&nbsp;&nbsp;
				</td>
				<td><asp:calendar BackColor=Beige id="fechaFinal" runat="server" SelectedDate="2005-11-20"></asp:calendar></td>
			</tr>
			<tr>
				<td>Resultado
				</td>
				<td>&nbsp;&nbsp;&nbsp;
				</td>
				<td><asp:dropdownlist id="ddlResultado" runat="server" Visible="True"></asp:dropdownlist></td>
			</tr>
		</tbody>
	</table>
</p>
<p><asp:button id="btnGenerar" runat="server" Text="Generar Reporte" onclick="btnGenerar_Click"></asp:button></p>
<asp:placeholder id="phReporte" runat="server" Visible="False">
	<P>
		<TABLE>
			<TR>
				<TD>
					<CENTER>
						<asp:Label id="lbRep" runat="server" text="REPORTE DE RESULTADOS DEL S.A.M."></asp:Label></CENTER>
				</TD>
			</TR>
			<TR>
				<TD></TD>
			</TR>
			<TR>
				<TD>
					<asp:DataGrid id="dgReporte" runat="server" ShowFooter="False" AutoGenerateColumns="False" HeaderStyle-BackColor="#ccccdd"
						Font-Size="8pt" Font-Name="Verdana" CellPadding="3" Font-Names="Verdana" Width="650px">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="CC" HeaderText="Nit o CC del Cliente" />
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Fecha" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
							<asp:BoundColumn DataField="PACT_NOMBMARK" HeaderText="Actividad" />
							<asp:BoundColumn DataField="PRES_DESCRIPCION" HeaderText="Resultado" />
							<asp:BoundColumn DataField="DMAR_DETALLE" HeaderText="Detalle" />
						</Columns>
					</asp:DataGrid></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
		</TABLE>
	</P>
</asp:placeholder>
<asp:Panel ID="pnlExcel" Runat="server" Visible="False">
	<TABLE>
		<TR>
			<TD>
				<asp:button id="btnGenerarExcel" runat="server" Text="Descargar Excel" onclick="btnGenerarExcel_Click"></asp:button></TD>
		</TR>
	</TABLE>
</asp:Panel>
<p><asp:label id="lb" runat="server"></asp:label></p>
</fieldset>
