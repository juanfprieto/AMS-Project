<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.AutorizacionPedidoMayor.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_AutiorizacionPedidoMayor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table style="WIDTH: 800px">
	<tbody id="Table1" class="fieltersIn">
		<tr>
			<td>
				<fieldset style="width:110%">
                    <legend>Listado Autorización Pedidos Cliente Mayor</legend>
					<table id="Table2" class="fieltersIn">
						<tbody>
							<tr>
								<td colSpan="4">
									<p><asp:label id="Label10" runat="server" forecolor="RoyalBlue">PEDIDOS</asp:label></p>
								</td>
							</tr>
							<tr>
								<td>
									<P>
										<ASP:DataGrid id="dgPedidos" runat="server" cssclass="datagrid"  AutoGenerateColumns="false" ShowFooter="True" 
											BorderWidth="1px" GridLines="Vertical" CellPadding="3">
											<FooterStyle cssclass="footer"></FooterStyle>
											<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
											<HeaderStyle cssclass="header"></HeaderStyle>
											<Columns>
												<asp:TemplateColumn HeaderText="Pref.">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "PPED_CODIGO") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Num.">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "MPED_NUMEPEDI") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="NIT">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "MNIT_NIT") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Cliente">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Vendedor">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "VENDEDOR") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Total<br>Cupo">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "CUPO", "{0:N}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Total<br>Cartera">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "CARTERA", "{0:N}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Total<br>Mora">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "MORA", "{0:N}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Total<br>Pedido">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "TOTAL", "{0:N}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Accion">
													<ItemTemplate>
														<asp:DropDownList id="ddlAccion" Runat="server">
															<asp:ListItem Value="" Selected="True">Omitir</asp:ListItem>
															<asp:ListItem Value="S">Autorizar</asp:ListItem>
															<asp:ListItem Value="N">Negar</asp:ListItem>
														</asp:DropDownList>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
											<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
										</ASP:DataGrid>
									</P>
								</td>
							</tr>
							<tr>
								<td colspan="3"><br>
									<asp:button id="btnSeleccionar" OnClick="btnSeleccionar_Click" runat="server" Enabled="True"
										Text="Ejecutar"></asp:button>
								</td>
							</tr>
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<asp:label id="lblInfo" runat="server"></asp:label>
