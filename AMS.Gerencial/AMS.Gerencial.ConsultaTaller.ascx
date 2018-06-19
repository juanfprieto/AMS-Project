<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Gerencial.ConsultaTaller.ascx.cs" Inherits="AMS.Gerencial.AMS_Gerencial_ConsultaTaller" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
<TABLE id="Table" class="filtersIn">
	<TR> 
		<TD><asp:label id="Label16" runat="server">Fecha Inicio:</asp:label></TD>
		<td><asp:label id="Label14" runat="server">Año:</asp:label>
        <asp:dropdownlist id="ano1" runat="server" class="dpequeno" ></asp:dropdownlist></td>
		<td><asp:label id="Label18" runat="server">Mes:</asp:label>
        <asp:dropdownlist id="mes1" runat="server" class="dpequeno"></asp:dropdownlist></td>
		<td><asp:label id="Label20" runat="server">Dia</asp:label>
        <asp:textbox id="DiaInicio" runat="server" class="tpequeno">1</asp:textbox></td>
	</TR>
	<tr>
		<td><asp:label id="Label15" runat="server">Fecha Finalización:</asp:label></td>
		<td><asp:label id="Label17" runat="server">Año:</asp:label>
        <asp:dropdownlist id="ano2" runat="server" class="dpequeno"></asp:dropdownlist></td>
		<td><asp:label id="Label19" runat="server">Mes:</asp:label>
        <asp:dropdownlist id="mes2" runat="server" class="dpequeno"></asp:dropdownlist></td>
		<td><asp:label id="Label21" runat="server">Dia</asp:label>
        <asp:textbox id="DiaFin" runat="server" class="tpequeno"></asp:textbox></td>
	</tr>
	<tr>
		<td><asp:button id="Generar" onclick="Generar_Click" runat="server" cssclass="datagrid" Text="Generar Informe"></asp:button></td>
	</tr>
</TABLE>
<BR>
<asp:panel id="pnlResultados" runat="server" Width="100%" Height="368px" Visible="False">
	<TABLE id="Table1" class="FiltersIn">
		<TR>
			<TD>Operaciones:</TD>
		</TR>
		<TR>
			<TD>
				<asp:DataGrid id="dgrTotalO" runat="server"  AutoGenerateColumns="False"
					ShowFooter="True">
					<FooterStyle CssClass="footer"></FooterStyle>
					<HeaderStyle CssClass="header"></HeaderStyle>
					<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
					<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
					<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:BoundColumn DataField="TOPE_NOMBRE" HeaderText="TIPO" FooterText="Totales" />
                        <asp:BoundColumn DataField="CARGO" HeaderText="Cargo" />
						<asp:BoundColumn DataField="LIQUIDADAS" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="LIQUIDADAS" />
						<asp:BoundColumn DataField="VALOLIQUIDADAS" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="TOTAL" DataFormatString="{0:C}" />
						<asp:BoundColumn DataField="PROCESO" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="EN PROCESO" />
						<asp:BoundColumn DataField="VALOPROCESO" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="TOTAL" DataFormatString="{0:C}" />
					</Columns>
				</asp:DataGrid></TD>
		</TR>
	</TABLE><BR>
	<TABLE id="Table2" class="filtersIn">
		<TR>
			<TD style="FONT-WEIGHT: bold; FONT-SIZE: 10pt">Repuestos:</TD>
		</TR>
		<TR>
			<TD>
				<asp:DataGrid id="dgrRepuestos" runat="server" CssClass="datagrid" AutoGenerateColumns="False"
					ShowFooter="True">
					<FooterStyle CssClass="footer"></FooterStyle>
					<HeaderStyle CssClass="header"></HeaderStyle>
					<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
					<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
					<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:BoundColumn DataField="TCAR_NOMBRE" HeaderText="CARGO" FooterText="Totales" />
						<asp:BoundColumn DataField="VALORLIQU" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="VALOR<BR>LIQUIDADO" DataFormatString="{0:C}" />
						<asp:BoundColumn DataField="VALORPROC" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="VALOR<BR>PROCESO" DataFormatString="{0:C}" />
					</Columns>
				</asp:DataGrid></TD>
		</TR>
	</TABLE><BR>
	<TABLE id="Table3" class="filtersIn">
		<TR>
			<TD style="FONT-WEIGHT: bold; FONT-SIZE: 10pt">Recepcionistas:</TD>
		</TR>
		<TR>
			<TD>
				<asp:DataGrid id="dgrRecepcionistas" runat="server" cssclass="datagrid" AutoGenerateColumns="False"
					ShowFooter="True">
					    <FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO" FooterText="Totales" />
						<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE" />
						<asp:BoundColumn DataField="ORDSLIQU" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="ORDENES<BR>LIQUIDADAS" />
						<asp:BoundColumn DataField="VENDLIQU" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="VENDIDO<BR>LIQUIDADO" DataFormatString="{0:C}" />
						<asp:BoundColumn DataField="ORDSPROC" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="ORDENES<BR>PROCESO" />
						<asp:BoundColumn DataField="ORDSPROC10" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="ORDENES<BR>PROCESO<BR>10 dias" />
						<asp:BoundColumn DataField="ORDSPROC20" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="ORDENES<BR>PROCESO<BR>20 dias" />
						<asp:BoundColumn DataField="ORDSPROCM" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="ORDENES<BR>PROCESO<BR>Mas dias" />
						<asp:BoundColumn DataField="VENDPROC" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="VENDIDO<BR>PROCESO" DataFormatString="{0:C}" />
					</Columns>
				</asp:DataGrid></TD>
		</TR>
	</TABLE><BR>
	<TABLE id="Table4" class="filtersIn">
		<TR>
			<TD style="FONT-WEIGHT: bold; FONT-SIZE: 10pt">Mecánicos:</TD>
		</TR>
		<TR>
			<TD>
				<asp:DataGrid id="dgrMecanicos" runat="server" CssClass="datagrid" AutoGenerateColumns="False"
					ShowFooter="True">
					<FooterStyle CssClass="footer"></FooterStyle>
					<HeaderStyle CssClass="header"></HeaderStyle>
					<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
					<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
					<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO" FooterText="Totales" />
						<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE" />
						<asp:BoundColumn DataField="HORASLIQU" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="HORAS<BR>LIQUIDADAS" />
						<asp:BoundColumn DataField="VENDLIQU" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="VENDIDO<BR>LIQUIDADO" DataFormatString="{0:C}" />
						<asp:BoundColumn DataField="HORASPROC" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="HORAS<BR>PROCESO" />
						<asp:BoundColumn DataField="VENDPROC" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
							HeaderText="VENDIDO<BR>PROCESO" DataFormatString="{0:C}" />
					</Columns>
				</asp:DataGrid></TD>
		</TR>
	</TABLE>
</asp:panel>
</fieldset>
