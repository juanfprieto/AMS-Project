<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.HVPostventa.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_HVPostventa" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:placeholder id="plcSeleccion" runat="server">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET>
                <LEGEND class="Legends">Selección del 
						Vehículo</LEGEND>
					<TABLE class="filstersIn" cellSpacing="10">
						<TR>
							<TD>
								<asp:Label id="Label20" runat="server" Font-Bold="True" CssClass="Legends">VIN: </asp:Label></TD>
							<TD align="left" colSpan="3">
								<asp:TextBox id="txtVINVehiculo" onclick="ModalDialog(this,'SELECT mv.mcat_vin VIN,mc.mcat_motor MOTOR, mc.pcat_codigo catalogo, mc.mcat_placa as placa FROM mvehiculo mv, mcatalogovehiculo mc, mvehiculopostventa mp WHERE mv.mcat_vin=mc.mcat_vin and mp.mcat_vin=mc.mcat_vin;',new Array(),1);"
									runat="server" ReadOnly="True" class="tmediano"></asp:TextBox></TD>
							<TD>&nbsp;</TD>
							<TD>
								<asp:button id="btnSeleccionar" runat="server" class="bpequeno" Text="Consultar" OnClick="btnSeleccionar_Click"></asp:button></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
</asp:placeholder>
<asp:placeholder id="plcResultados" runat="server" Visible="False">
    <TABLE>
		<TR>
			<TD>
				<FIELDSET>
                <LEGEND class="Legends">Información Vehículo</LEGEND>
					<TABLE class="filstersIn" cellspacing="10">
						<TR>
							<TD>
								<asp:Label id="Label1" runat="server" CssClass="Legends" Font-Bold="True">VIN: </asp:Label></TD>
							<TD>
								<asp:Label id="lblVINVehiculo" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label2" runat="server" CssClass="Legends" Font-Bold="True">Motor: </asp:Label></TD>
							<TD>
								<asp:Label id="lblMotorVehiculo" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label23" runat="server" CssClass="Legends" Font-Bold="True">Placa: </asp:Label></TD>
							<TD>
								<asp:Label id="lblPlacaVehiculo" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label3" runat="server" CssClass="Legends" Font-Bold="True">Catálogo: </asp:Label></TD>
							<TD>
								<asp:Label id="lblCatalogoVehiculo" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label5" runat="server" CssClass="Legends" Font-Bold="True">Color: </asp:Label></TD>
							<TD>
								<asp:Label id="lblColorVehiculo" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label24" runat="server" CssClass="Legends" Font-Bold="True">Kilometraje: </asp:Label></TD>
							<TD>
								<asp:Label id="lblKilometraje" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label29" runat="server" CssClass="Legends" Font-Bold="True">Meses garantía: </asp:Label></TD>
							<TD>
								<asp:Label id="lblMesesGarantia" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label31" runat="server" CssClass="Legends" Font-Bold="True">Kilómetros garantía: </asp:Label></TD>
							<TD>
								<asp:Label id="lblKiloGarantia" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label7" runat="server" CssClass="Legends" Font-Bold="True">Año Catálogo: </asp:Label></TD>
							<TD>
								<asp:Label id="lblAnoCatalogo" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Información 
						Propietario</LEGEND>
					<TABLE class="filstersIn" cellSpacing="10">
						<TR>
							<TD>
								<asp:Label id="Label6" runat="server" CssClass="Legends" Font-Bold="True">Documento: </asp:Label></TD>
							<TD>
								<asp:Label id="lblNITPropietario" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label11" runat="server" CssClass="Legends" Font-Bold="True">Nombre: </asp:Label></TD>
							<TD>
								<asp:Label id="lblNITPropietarioa" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label9" runat="server" CssClass="Legends" Font-Bold="True">Teléfono: </asp:Label></TD>
							<TD>
								<asp:Label id="lblNITPropietarioc" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label14" runat="server" CssClass="Legends" Font-Bold="True">Dirección: </asp:Label></TD>
							<TD>
								<asp:Label id="lblNITPropietariob" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label15" runat="server" CssClass="Legends" Font-Bold="True">Celular: </asp:Label></TD>
							<TD>
								<asp:Label id="lblNITPropietariod" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label16" runat="server" CssClass="Legends" Font-Bold="True">Ciudad: </asp:Label></TD>
							<TD>
								<asp:Label id="lblNITPropietarioe" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
    </TABLE>
	<TABLE>
		<TR>
			<TD>
				<FIELDSET>
                <LEGEND class="Legends">Historial</LEGEND>Kits:<BR>
					<asp:DataGrid id="dgrMantenimientosAnt" runat="server" cssclass="datagrid" AutoGenerateColumns="false" GridLines="Vertical">
						<HeaderStyle CssClass="header"></HeaderStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="NOMBRE">
								<HeaderStyle Width="15%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FECHA">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHA", "{0:yyyy-MM-dd}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="KILOMETRAJE">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "KILOMETRAJE", "{0:#,###}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CONCESIONARIO">
								<HeaderStyle Width="25%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CONCESIONARIO")%>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="">
								<HeaderStyle></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TIPO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="OBSERVACIONES CLIENTE">
								<HeaderStyle Width="20%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "OBSECLIENTE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="OBSERVACIONES TALLER">
								<HeaderStyle Width="20%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "OBSETALLER") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid><BR>
					<BR>
					Operaciones:<BR>
					<asp:DataGrid id="dgrOperacionesAnt" runat="server" cssclass="datagrid" AutoGenerateColumns="false" GridLines="Vertical">
						<HeaderStyle CssClass="header"></HeaderStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="NOMBRE">
								<HeaderStyle Width="60%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FECHA">
								<HeaderStyle Width="20%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHA", "{0:yyyy-MM-dd}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="KILOMETRAJE">
								<HeaderStyle Width="20%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "KILOMETRAJE", "{0:#,###}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CONCESIONARIO">
								<HeaderStyle Width="20%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CONCESIONARIO")%>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="">
								<HeaderStyle></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TIPO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid><BR>
					<BR>
					Repuestos:<BR>
					<asp:DataGrid id="dgrRepuestosAnt" runat="server" cssclass="datagrid" GridLines="Vertical" AutoGenerateColumns="false">
						<HeaderStyle CssClass="header"></HeaderStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="CODIGO">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NOMBRE">
								<HeaderStyle Width="40%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CANTIDAD">
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
							<asp:TemplateColumn HeaderText="KILOMETRAJE">
								<HeaderStyle Width="5%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "KILOMETRAJE", "{0:#,###}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CONCESIONARIO">
								<HeaderStyle Width="20%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CONCESIONARIO")%>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="">
								<HeaderStyle></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TIPO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
				</FIELDSET>
			</td>
		</tr>
	</table>
</asp:placeholder>
