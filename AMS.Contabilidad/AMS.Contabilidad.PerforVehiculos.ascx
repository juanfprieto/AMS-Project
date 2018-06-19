<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Contabilidad.PerforVehiculos.ascx.cs" Inherits="AMS.Contabilidad.AMS_Contabilidad_PerforVehiculos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<P><asp:label id="Label1" Width="264px" runat="server">Performance Venta de Vehiculos</asp:label></P>
<HR width="100%" color="#003399" SIZE="3">
<P></P>
<p><asp:button id="Button1" onclick="generar" runat="server" Text="Generar"></asp:button></p>
<table class="reports" width="780" align="center" bgColor="gray">
	<tbody>
		<tr>
			<td><asp:table id="tabPreHeader" Width="100%" CellSpacing="0" CellPadding="1" BackColor="White"
					GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"
					BorderWidth="0px"></asp:table></td>
		</tr>
		<tr>
			<td align="center">
				<p><asp:datagrid id="Grid" runat="server" cssclass="datagrid" HorizontalAlign="Center" AutoGenerateColumns="False" onselectedindexchanged="Grid_SelectedIndexChanged">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<ItemStyle HorizontalAlign="left" cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="MODELO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "modelo") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR UNITARIO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "valorUnitario") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR TOTAL">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "valorTotal") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR IVA">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "porcentaje") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CANTIDAD VENDIDA">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "cantidad") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></p>
			</td>
		</tr>
		<tr>
			<td><asp:table id="tabFirmas" CellSpacing="0" CellPadding="1" BackColor="White" GridLines="Both"
					Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center" BorderWidth="0px"
					EnableViewState="False"></asp:table></td>
		</tr>
	</tbody>
</table>
<BLOCKQUOTE dir="ltr" style="MARGIN-RIGHT: 0px">
	<p><asp:label id="lb" runat="server"></asp:label></p>
</BLOCKQUOTE>
