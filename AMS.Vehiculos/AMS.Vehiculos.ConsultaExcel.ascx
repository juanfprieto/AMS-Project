<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.ConsultaExcel.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_ConsultaExcel" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.WizardDirection.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>

<asp:PlaceHolder id="plcSeleccion" runat="server" EnableTheming="True">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Información Liquidación</LEGEND>
					<TABLE class="main" cellSpacing="0">
						<TR>
							<TD>
								<TABLE class="main" cellSpacing="10">
									<TR>
										<TD>
											<asp:Label id="Label1" runat="server" Font-Bold="True" CssClass="Legends">Concesionario: </asp:Label>
</TD>
										<TD>
											<asp:TextBox id="txtConcesionario" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
</TD>
										<TD>
											<asp:TextBox id="txtConcesionarioa" runat="server" class="tmediano" ReadOnly="True"></asp:TextBox>
</TD>
									</TR>
									<TR>
										<TD>
											<asp:Label id="Label7" runat="server" Font-Bold="True" CssClass="Legends">Ejecutivo Garantias: </asp:Label>
</TD>
										<TD colSpan="2">
											<asp:dropdownlist id="ddlVendedor" runat="server"></asp:dropdownlist>
</TD>
									</TR>
								</TABLE>
							</TD>
							<TD>
								<TABLE class="main" cellSpacing="10">
									<TR>
										<TD>
											<asp:Label id="Label2" runat="server" Font-Bold="True" CssClass="Legends">Año vigencia: </asp:Label>
</TD>
										<TD>
											<asp:dropdownlist id="ddlAno" runat="server"></asp:dropdownlist>
</TD>
									</TR>
									<TR>
										<TD>
											<asp:Label id="Label23" runat="server" Font-Bold="True" CssClass="Legends">Mes vigencia: </asp:Label>
</TD>
										<TD>
											<asp:dropdownlist id="ddlMes" runat="server"></asp:dropdownlist>
</TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
						<TR>
							<TD align="right" colSpan="2">
								<asp:button id="btnSeleccionar" runat="server" class="bpequeno" Text="Generar" onclick="btnSeleccionar_Click"></asp:button>
</TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
</asp:PlaceHolder>
<asp:PlaceHolder id="plcFactura" runat="server" Visible="False">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Información Liquidación</LEGEND>
					<TABLE class="main" cellSpacing="0">
						<TR>
							<TD>
								<TABLE class="main" cellSpacing="10">
									<TR>
										<TD>
											<asp:Label id="Label3" runat="server" Font-Bold="True" CssClass="Legends">Concesionario: </asp:Label></TD>
										<TD>
											<asp:Label id="lblConcesionario" runat="server" Font-Bold="True"></asp:Label></TD>
										<TD>
											<asp:Label id="Label16" runat="server" Font-Bold="True" CssClass="Legends">Categoría: </asp:Label></TD>
										<TD>
											<asp:Label id="lblCategoria" runat="server" Font-Bold="True"></asp:Label></TD>
										<TD>
											<asp:Label id="Label5" runat="server" Font-Bold="True" CssClass="Legends">Ejecutivo Garantias: </asp:Label></TD>
										<TD>
											<asp:Label id="lblEjecutivo" runat="server" Font-Bold="True"></asp:Label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
						<TR>
							<TD>
								<TABLE class="main" cellSpacing="10">
									<TR>
										<TD>
											<asp:Label id="Label8" runat="server" Font-Bold="True" CssClass="Legends">Año vigencia: </asp:Label></TD>
										<TD>
											<asp:Label id="lblAno" runat="server" Font-Bold="True"></asp:Label></TD>
										<TD>
											<asp:Label id="Label9" runat="server" Font-Bold="True" CssClass="Legends">Mes vigencia: </asp:Label></TD>
										<TD>
											<asp:Label id="lblMes" runat="server" Font-Bold="True"></asp:Label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
					<asp:Panel ID="pnlAutorizar" Runat="server">
						<TABLE class="main" cellSpacing="10">
							<TR>
								<TD>
									<asp:Label id="Label10" runat="server" Font-Bold="True" CssClass="Legends">Fecha: </asp:Label></TD>
								<TD>
									<asp:textbox id="txtFechaProceso" onkeyup="DateMask(this)" runat="server" class="tpequeno"></asp:textbox></TD>
								<TD>
									<asp:Label id="Label11" runat="server" Font-Bold="True" CssClass="Legends">Aprobación: </asp:Label></TD>
								<TD>
									<asp:textbox id="txtPrefAprobacion" class="tpequeno" Runat="server"></asp:textbox>&nbsp;
									<asp:textbox id="txtNumAprobacion" class="tpequeno" Runat="server"></asp:textbox></TD>
							</TR>
						</TABLE>
					</asp:Panel>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
	<TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Ordenes de Trabajo</LEGEND>
					<asp:Repeater id="rptFactura" runat="server" OnItemDataBound="rptFactura_Bound">
						<ItemTemplate>
							<table cellspacing="0" cellpadding="3" bordercolor="#999999" border="1" style="background-color:White;border-color:#999999;border-width:1px;border-style:None;font-family:Verdana;font-size:8pt;width:700px;border-collapse:collapse;">
								<tr style="color:White;background-color:#000084;font-weight:bold;">
									<td width="100px">ORDEN:&nbsp;<%#DataBinder.Eval(Container.DataItem,"pdoc_codigo") %>-<%#DataBinder.Eval(Container.DataItem,"mord_numeorde")%></td>
									<td width="80px"><%#DataBinder.Eval(Container.DataItem,"mord_entrada","{0:yyyy-MM-dd}")%></td>
									<td width="100px"><%#DataBinder.Eval(Container.DataItem,"tipo_proceso")%></td>
									<td width="70px"><%#DataBinder.Eval(Container.DataItem,"mcat_placa")%></td>
									<td width="70px"><%#DataBinder.Eval(Container.DataItem,"mcat_vin")%></td>
									<td width="70px"><%#DataBinder.Eval(Container.DataItem,"mcat_motor")%></td>
									<td align=right valign=bottom width="200px" bgcolor='<%#DataBinder.Eval(Container.DataItem,"color")%>'><div style="background-color:#DCDCDC;color:Black;width:10;"><%#DataBinder.Eval(Container.DataItem, "strestado")%></div></td>
								</tr>
								<tr>
									<td colspan="7">
										<asp:DataGrid id="dgrOperaciones" runat="server" cssclass="datagrid" AutoGenerateColumns="false">
											 <HeaderStyle CssClass="header"></HeaderStyle>
						                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						                        <ItemStyle CssClass="item"></ItemStyle>
											<Columns>
											    <asp:TemplateColumn HeaderText="">
													<HeaderStyle Width="2%"></HeaderStyle>
													<ItemTemplate>
														<div style='width:100%;height:100%;background-color:<%#DataBinder.Eval(Container.DataItem,"color")%>'>&nbsp;</div>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="NOMBRE">
													<HeaderStyle Width="58%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="TIEMPO">
													<HeaderStyle Width="10%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "TIEMPO") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="VALOR">
													<HeaderStyle Width="10%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "PRECIO", "{0:#,##0}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="FECHA">
													<HeaderStyle Width="15%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "FECHA", "{0:yyyy-MM-dd}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="KMS.">
													<HeaderStyle Width="5%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "KILOMETRAJE", "{0:#,###}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:DataGrid><BR>
										<asp:DataGrid id="dgrRepuestos" runat="server" cssclass="datagrid" AutoGenerateColumns="false" GridLines="Vertical">
										        <HeaderStyle CssClass="header"></HeaderStyle>
						                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						                        <ItemStyle CssClass="item"></ItemStyle>
											<Columns>
											    <asp:TemplateColumn HeaderText="">
													<HeaderStyle Width="2%"></HeaderStyle>
													<ItemTemplate>
														<div style='width:100%;height:100%;background-color:<%#DataBinder.Eval(Container.DataItem,"color")%>'>&nbsp;</div>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="CODIGO">
													<HeaderStyle Width="20%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="NOMBRE">
													<HeaderStyle Width="33%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="CANT.">
													<HeaderStyle Width="5%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "CANTIDAD", "{0:#,##0}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="VALOR UNID.">
													<HeaderStyle Width="15%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "PRECIO", "{0:#,##0}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="FECHA">
													<HeaderStyle Width="20%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "FECHA", "{0:yyyy-MM-dd}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="KMS.">
													<HeaderStyle Width="5%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "KILOMETRAJE", "{0:#,###}") %>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:DataGrid><br>
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:Repeater><BR>
					<TABLE>
						<TR>
							<TD>
								<asp:Label id="Label4" runat="server" Font-Bold="True" CssClass="Legends">Total Alistamientos: </asp:Label></TD>
							<TD>
								<asp:Label id="lblTotalAlistamientos" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label6" runat="server" Font-Bold="True" CssClass="Legends">Total Revisiones: </asp:Label></TD>
							<TD>
								<asp:Label id="lblTotalRevisiones" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label12" runat="server" Font-Bold="True" CssClass="Legends">Total Garantías: </asp:Label></TD>
							<TD>
								<asp:Label id="lblTotalGarantias" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
					</TABLE>
					<BR>
					<TABLE>
						<TR>
							<TD>
								<TABLE>
									<TR>
										<TD>
											<asp:Label id="Label13" runat="server" Font-Bold="True" CssClass="Legends">Subtotal: </asp:Label></TD>
										<TD>
											<asp:Label id="lblSubtotal" runat="server" Font-Bold="True"></asp:Label></TD>
									</TR>
									<TR>
										<TD>
											<asp:Label id="Label14" runat="server" Font-Bold="True" CssClass="Legends">IVA: </asp:Label></TD>
										<TD>
											<asp:Label id="lblIVA" runat="server" Font-Bold="True"></asp:Label></TD>
									</TR>
									<TR>
										<TD>
											<asp:Label id="Label15" runat="server" Font-Bold="True" CssClass="Legends">Total: </asp:Label></TD>
										<TD>
											<asp:Label id="lblTotal" runat="server" Font-Bold="True"></asp:Label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
					<TABLE width="100%">
						<TR>
							<TD align="center"><asp:Button id="btnAceptar" runat="server" Text="Aceptar" Width="200px" onclick="btnAceptar_Click"></asp:Button></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
</asp:PlaceHolder>
<asp:Label id="lblError" runat="server" Font-Bold="True"></asp:Label>

