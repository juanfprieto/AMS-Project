<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConciliacionManual.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ConciliacionManual" %>
<p>A continuación relacionamos los sobrantes de la conciliación bancaria, realice 
	los cruces que considere hicieron falta, teniendo en cuenta que la sumatoria de 
	los valores cruzados en el Sobrante de Movimiento Bancario, debe ser la misma 
	que en el Sobrante de Movimiento de Tesoreria. Si no aparece nada, entonces 
	solo haga click sobre el botón Ejecutar Cruces.
</p>
<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>Sobrante Movimiento Bancario
			</td>
			<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
			<td>Sobrante Movimiento de Tesoreria
			</td>
		</tr>
		<tr>
			<td>
				<p><asp:datagrid id="dgSobBan" CellPadding="3" cssclass="datagrid" AutoGenerateColumns="False" runat="server">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMEROBAN" HeaderText="N&#250;mero del Documento"></asp:BoundColumn>
							<asp:BoundColumn DataField="VALORBAN" HeaderText="Valor" DataFormatString="{0:C}"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Cruzar">
								<ItemTemplate>
									<center>
										<asp:CheckBox id="chbCB" runat="server" />
									</center>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></p>
			</td>
			<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
			<td>
				<p><asp:datagrid id="dgSobTes" CellPadding="3" cssclass="datagrid" AutoGenerateColumns="False" runat="server">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="PREFTES" HeaderText="Prefijo Tesoreria"></asp:BoundColumn>
							<asp:BoundColumn DataField="NUMTES" HeaderText="N&#250;mero Tesoreria"></asp:BoundColumn>
							<asp:BoundColumn DataField="VALORTES" HeaderText="Valor" DataFormatString="{0:C}"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Cruzar">
								<ItemTemplate>
									<center>
										<asp:CheckBox id="chbCT" runat="server" />
									</center>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></p>
			</td>
		</tr>
	</tbody></table>
<p><asp:button id="btnEjecutar" onclick="btnEjecutar_Click" runat="server" Text="Ejecutar Cruces"></asp:button></p>
<p><asp:label id="lb" runat="server"></asp:label></p>
