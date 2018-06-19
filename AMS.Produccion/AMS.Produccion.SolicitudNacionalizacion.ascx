<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Produccion.SolicitudNacionalizacion.ascx.cs" Inherits="AMS.Produccion.AMS_Produccion_SolicitudNacionalizacion" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table style="WIDTH: 600px">
	<tbody>
		<tr>
			<td>
				<fieldset><legend>Listado Solicitud Nacionalización</legend>
					<table class="main">
						<tbody>
							<tr>
								<td colSpan="2">
									<p><asp:label id="Label10" runat="server" forecolor="RoyalBlue">VEHICULOS</asp:label></p>
								</td>
							</tr>
							<tr>
								<td>
									<p>Prefijo Documento:<br>
										<asp:dropdownlist id="ddlPrefDoc" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefDoc_SelectedIndexChanged"></asp:dropdownlist></p>
								</td>
								<td>
									<p>Número Documento:<br>
										<asp:label id="lblNumDoc" runat="server"></asp:label></p>
								</td>
							</tr>
							<tr>
								<td colSpan="2">
									<P>
										<ASP:DataGrid id="dgEnsambles" runat="server" AutoGenerateColumns="false" ShowFooter="True" CssClass="datagrid" GridLines="Vertical">
											<FooterStyle CssClass="footer"></FooterStyle>
						                    <HeaderStyle CssClass="header"></HeaderStyle>
						                    <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						                    <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						                    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						                    <ItemStyle CssClass="item"></ItemStyle>
											<Columns>
												<asp:TemplateColumn HeaderText="Catálogo">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "PCAT_CODIGO") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="VIN">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "MCAT_VIN") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Motor">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "MCAT_MOTOR") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Color">
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "PCOL_DESCRIPCION") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="">
													<ItemTemplate>
														<asp:CheckBox Runat="server" ID="chkUsarE" Checked="True"></asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</ASP:DataGrid>
									</P>
								</td>
							</tr>
							<tr>
								<td colspan="3"><br>
									<asp:button id="btnSeleccionar" runat="server" Enabled="True" Text="Ejecutar" onclick="btnSeleccionar_Click"></asp:button>
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
