<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="AMS.Produccion.CierreOrdenProduccion.ascx.cs"
    Inherits="AMS.Produccion.AMS_Produccion_CierreOrdenProduccion" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table id="Table" class="filtersIn">
    <tbody>
        <tr>
            <td>
                <fieldset>
                    <legend>Orden de Producción:</legend>
                    <table class="main">
                        <tbody>
                            <tr>
                                <td>
                                    <p>
                                        &nbsp;&nbsp;&nbsp;&nbsp;Prefijo:<br>
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlPrefOrden" runat="server" autopostback="True"
                                            onselectedindexchanged="ddlPrefOrden_SelectedIndexChanged"></asp:dropdownlist></p>
                                </td>
                                <td>
                                    <p>
                                        &nbsp;&nbsp;&nbsp;&nbsp;Numero:<br>
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlNumOrden" runat="server" autopostback="True"
                                            onselectedindexchanged="ddlNumOrden_SelectedIndexChanged"></asp:dropdownlist></p>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </fieldset>
                <asp:panel id="pnlDetalle" runat="server" visible="False">
					<FIELDSET><LEGEND>Detalle:</LEGEND>
						<TABLE class="main">
							<TR>
								<TD>
									<asp:datagrid id="dgrDetalle" runat="server" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
										AlternatingItemStyle-HorizontalAlign="Center" CssClass="datagrid" AutoGenerateColumns="False" ShowFooter="False">
										<FooterStyle CssClass="footer"></FooterStyle>
						                <HeaderStyle CssClass="header"></HeaderStyle>
						                <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						                <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						                <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						                <ItemStyle CssClass="item"></ItemStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Ensamble">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "PENS_CODIGO")%>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Elemento">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "DORD_ELEM")%>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Por Producir">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "DORD_CANTXPROD")%>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Producido">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "DORD_CANTENTR")%>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
                            <tr>
                                <td><br /></td>
                            </tr>
							<tr>
				                <td>
						            Prefijo del Ajuste a Inventario<br>
						            <asp:DropDownList id="ddlPrefijoAjuste" class="dmediano" runat="server" OnSelectedIndexChanged="ddlPrefijoAjuste_OnSelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
				                </td>
				                <td>
					                Número del Ajuste a Inventario:  
						            <asp:Label id="lblNumeroAjuste" runat="server"></asp:Label>
				                </td>
                            </tr>
                            <tr>
                                <td>
                                    <ASP:DataGrid id="dgItems" runat="server" GridLines="Vertical" AutoGenerateColumns="false" CssClass="datagrid">
			                            <HeaderStyle cssclass="header"></HeaderStyle>
		                                <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		                                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						                <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		                                <ItemStyle cssclass="items"></ItemStyle>
			                            <Columns>
				                            <asp:TemplateColumn HeaderText="Código">
					                            <ItemTemplate>
						                            <%# DataBinder.Eval(Container.DataItem, "mite_codigo", "{0:N}") %>
					                            </ItemTemplate>
				                            </asp:TemplateColumn>
				                            <asp:TemplateColumn HeaderText="Nombre">
					                            <ItemTemplate>
						                            <%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
					                            </ItemTemplate>
				                            </asp:TemplateColumn>
				                            <asp:TemplateColumn HeaderText="Unidad de Medida">
					                            <ItemTemplate>
						                            <%# DataBinder.Eval(Container.DataItem, "puni_nombre", "{0:N}") %>
					                            </ItemTemplate>
				                            </asp:TemplateColumn>
				                            <asp:TemplateColumn HeaderText="Cantidad Transferida a Planta">
					                            <ItemTemplate>
						                            <%# DataBinder.Eval(Container.DataItem, "dite_cantidad", "{0:N}") %>
					                            </ItemTemplate>
				                            </asp:TemplateColumn>
				                            <asp:TemplateColumn HeaderText="Cantidad a Devolver">
					                            <ItemTemplate>
						                            <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
					                            </ItemTemplate>
				                            </asp:TemplateColumn>
			                            </Columns>
		                            </ASP:DataGrid>
                                </td>
                            </tr>

							<asp:Panel id="pnlObservacion" Runat="server" Visible="false">
								<TR>
									<TD>Observaciones:</TD>
								</TR>
								<TR>
									<TD>
										<asp:TextBox id="txtObservacion" runat="server" Width="600px" TextMode="MultiLine" Height="72px"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD>&nbsp;</TD>
								</TR>
							</asp:Panel>
							<TR>
								<TD vAlign="bottom" colSpan="4">
									<asp:button id="btnCerrar" runat="server" Enabled="True" Text="Cerrar" onclick="btnCerrar_Click"></asp:button></TD>
							</TR>
						</TABLE>
					</FIELDSET>
				</asp:panel>
            </td>
        </tr>
    </tbody>
</table>
<asp:label id="lbInfo" runat="server"></asp:label>
