<%@ Control Language="c#" codebehind="AMS.Inventarios.VistaImpresion.cs" autoeventwireup="True" Inherits="AMS.Inventarios.VistaImpresion" %>
<table class="reports" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<p>
					<asp:Table id="tabHeaderEmpresa" BorderWidth="0px" CellSpacing="5" CellPadding="1" BackColor="White"
						GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"
						Width="100%"></asp:Table>
				</p>
				<p>
					<asp:Table id="tabHeaderCliente" BorderWidth="0px" CellSpacing="0" CellPadding="1" BackColor="White"
						GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"
						Width="100%"></asp:Table>
				</p>
				<p>
					<ASP:DataGrid id="dgRepuestos" BorderWidth="2px" cssclass="datagrid" CellSpacing="1" CellPadding="3"
						runat="server" AutoGenerateColumns="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="REFERENCIA" ReadOnly="True" HeaderText="Referencia "></asp:BoundColumn>
							<asp:BoundColumn DataField="NOMBRE" ReadOnly="True" HeaderText="Nombre"></asp:BoundColumn>
							<asp:BoundColumn DataField="CANTIDAD" ReadOnly="True" HeaderText="Cantidad">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="VALOR_UNIDAD" ReadOnly="True" HeaderText="Valor Unidad" DataFormatString="{0:C}">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundColumn>
							<asp:TemplateColumn HeaderText="IVA">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE_IVA", "{0:N}") %>
									&nbsp %
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="VALOR_IVA" ReadOnly="True" HeaderText="Valor IVA" DataFormatString="{0:c}">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor Total" DataFormatString="{0:c}">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundColumn>
						</Columns>
						<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6"></PagerStyle>
					</ASP:DataGrid>
				</p>
				<p>
					<asp:Table id="tabHeaderFactura" BorderWidth="0px" CellSpacing="0" CellPadding="1" BackColor="White"
						GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"
						Width="100%"></asp:Table>
				</p>
			</td>
		</tr>
	</tbody>
</table>
<asp:Label id="lb" runat="server"></asp:Label>
