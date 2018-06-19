<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.AutorizacionGarantias.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_AutorizacionGarantias" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Vehiculos.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.WizardDirection.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
<asp:placeholder id="plcSeleccion" runat="server">

				<FIELDSET>
                <LEGEND class="Legends">Selección de la 
						Orden</LEGEND>
					<TABLE id="Table1" class="filtersIn" cellSpacing="10">
						<TR>
							<TD>
								<asp:Label id="Label20" runat="server" class="lpequeno" Font-Bold="True">Concesionario: </asp:Label>
                                <br />
								<asp:DropDownList id="ddlConcesionario" class="dmediano" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlConcesionario_Change"></asp:DropDownList>
                            </td>
                        </tr>
                        <TR>
							<TD>
								<asp:Label id="Label1" runat="server" class="lpequeno" Font-Bold="True">Prefijo Orden: </asp:Label>
								<br />
                                <asp:DropDownList id="ddlPrefOrden" class="dpequeno" runat="server"></asp:DropDownList>
						    </td>
                            <TD>
								<asp:Label id="Label2" runat="server" class="lpequeno" Font-Bold="True">Número Orden: </asp:Label>
								<br />
                                <asp:DropDownList id="ddlNumeOrden" class="dpequeno" runat="server"></asp:DropDownList>
                            </TD>
                        </tr>
						<TR>
							<TD align="left" colSpan="1">
								<asp:button id="btnSeleccionar" runat="server" Text="Seleccionar" Width="104px" onclick="btnSeleccionar_Click"></asp:button></TD>
						</TR>
					</TABLE>
				</FIELDSET>

</asp:placeholder>
<asp:placeholder id="plcInformacion" runat="server" Visible="False">
				<FIELDSET><LEGEND class="Legends">Orden de Trabajo</LEGEND>
					<TABLE class="main" cellSpacing="10">
						<TR>
							<TD>
								<asp:Label id="Label3" CssClass="Legends" runat="server" Font-Bold="True">Concesionario: </asp:Label></TD>
							<TD>
								<asp:Label id="lblConcesionario" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label4" CssClass="Legends" runat="server" Font-Bold="True">Orden de Trabajo: </asp:Label></TD>
							<TD>
								<asp:Label id="lblOrden" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label11" CssClass="Legends" runat="server" Font-Bold="True">Factura Concesionario: </asp:Label></TD>
							<TD>
								<asp:Label id="lblFactConc" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label6" CssClass="Legends" runat="server" Font-Bold="True">Placa Vehículo: </asp:Label></TD>
							<TD>
								<asp:Label id="lblVehiculoPlaca" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label10" CssClass="Legends" runat="server" Font-Bold="True">VIN Vehículo: </asp:Label></TD>
							<TD>
								<asp:Label id="lblVehiculoVIN" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label19" CssClass="Legends" runat="server" Font-Bold="True">Año modelo: </asp:Label></TD>
							<TD>
								<asp:Label id="lblModelo" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<tr>
							<TD>
								<asp:Label id="Label17" CssClass="Legends" runat="server" Font-Bold="True">Catalogo: </asp:Label></TD>
							<TD>
								<asp:Label id="lblCatalogo" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label22" CssClass="Legends" runat="server" Font-Bold="True">Kilometraje: </asp:Label></TD>
							<TD>
								<asp:Label id="lblKilometraje" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label24" CssClass="Legends" runat="server" Font-Bold="True">Fecha ultimo kilometraje: </asp:Label></TD>
							<TD>
								<asp:Label id="lblFechKilometraje" runat="server" Font-Bold="True"></asp:Label></TD>
						</tr>
					</TABLE>
				</FIELDSET>


				<FIELDSET><LEGEND class="Legends">Factura de Entrega 
						al Cliente</LEGEND>
					<TABLE class="main" cellSpacing="10">
						<TR>
							<TD vAlign="top">
								<asp:Label id="Label32" CssClass="Legends" runat="server" Font-Bold="True">Factura: </asp:Label></TD>
							<TD vAlign="top">
								<asp:Label id="lblFactIni" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD vAlign="top" rowSpan="2">
								<asp:Label id="Label35" CssClass="Legends" runat="server" Font-Bold="True">Distribuidor: </asp:Label></TD>
							<TD vAlign="top" rowSpan="2">
								<asp:Label id="lblFactDitri" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD vAlign="top">
								<asp:Label id="Label33" CssClass="Legends" runat="server" Font-Bold="True">Fecha Entrega: </asp:Label></TD>
							<TD vAlign="top">
								<asp:Label id="lblFactFecha" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD valign="top">
								<asp:Label id="Label13" CssClass="Legends" runat="server" Font-Bold="True">Meses Garantía: </asp:Label></TD>
								<TD valign="top">
								<asp:Label id="lblMesesGarantia" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD vAlign="top">
								<asp:Label id="Label15" CssClass="Legends" runat="server" Font-Bold="True">Kilómetros Garantía: </asp:Label></TD>
							<TD vAlign="top">
								<asp:Label id="lblKiloGarantia" runat="server" Font-Bold="True"></asp:Label></TD>
								
						</TR>
						<tr>
						    <TD>
								<asp:Label id="Label23" runat="server" CssClass="Legends" Font-Bold="True">Placa: </asp:Label></TD>
							<TD>
								<asp:textbox id="txtPlaca" runat="server" class="tpequeno" MaxLength="10"></asp:textbox></TD>
							<TD>
								<asp:Label id="Label16" runat="server" CssClass="Legends" Font-Bold="True">Motor: </asp:Label></TD>
							<TD>
								<asp:Label id="lblMotorVehiculo" runat="server" Font-Bold="True"></asp:Label></TD>
						</tr>
						<asp:placeholder id="plcFacturaInicial" runat="server" Visible="False">
					<FIELDSET><LEGEND class="Legends">Información 
							Entrega Inicial</LEGEND>
						<TABLE class="main" cellSpacing="10">
							<TR>
								<TD>
									<asp:Label id="Label14" runat="server" CssClass="Legends" Font-Bold="True">Prefijo Factura: </asp:Label></TD>
								<TD>
									<asp:textbox id="txtPrefFactura" runat="server" class="tpequeno"></asp:textbox></TD>
								<TD>
									<asp:Label id="Label18" runat="server" CssClass="Legends" Font-Bold="True">Numero Factura: </asp:Label></TD>
								<TD>
									<asp:textbox id="txtNumFactura" runat="server" class="tpequeno"></asp:textbox></TD>
							</TR>
							<TR>
								<TD>
									<asp:Label id="Label21" runat="server" CssClass="Legends" Font-Bold="True">Fecha Facturación:</asp:Label></TD>
								<TD>
									<asp:textbox id="txtFechaFact" onkeyup="DateMask(this)" runat="server" class="tpequeno"></asp:textbox></TD>
							</TR>
						</TABLE>
					</FIELDSET>
				</asp:placeholder>
				</FIELDSET>


				<FIELDSET><LEGEND class="Legends">Historial</LEGEND>Operaciones:<BR>
					<asp:DataGrid id="dgrOperacionesAnt" runat="server" Width="700px" HeaderStyle-BackColor="#ccccdd"
						Font-Size="8pt" Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" EnableViewState="true"
						BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="false">
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="NOMBRE">
								<HeaderStyle Width="40%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR">
								<HeaderStyle Width="15%"></HeaderStyle>
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
							<asp:TemplateColumn HeaderText="KILOMETRAJE">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "KILOMETRAJE", "{0:#,###}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="INCIDENTE">
								<HeaderStyle Width="15%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "INCIDENTE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CAUSAL">
								<HeaderStyle Width="15%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CAUSAL") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="AUTORIZAR">
								<HeaderStyle Width="2%"></HeaderStyle>
								<ItemTemplate>
									<asp:CheckBox ID="chkUsar" Runat="server">
									</asp:CheckBox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid><BR>
					<BR>
					Repuestos:<BR>
					<asp:DataGrid id="dgrRepuestosAnt" runat="server"  Width="740px" HeaderStyle-BackColor="#ccccdd"
						Font-Size="8pt" Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White"
						BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="false" EnableViewState="true">
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="CODIGO">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NOMBRE">
								<HeaderStyle Width="30%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CANTIDAD">
								<HeaderStyle Width="3%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CANTIDAD", "{0:#,##0}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR UNID.">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PRECIO", "{0:#,##0}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FECHA">
								<HeaderStyle Width="12%"></HeaderStyle>
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
							<asp:TemplateColumn HeaderText="">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemTemplate>
									<asp:DropDownList ID="ddlAprobar" Runat="server">
									    <asp:ListItem Text="Aprobar" Value="A"></asp:ListItem>
									    <asp:ListItem Text="Negar" Value="N"></asp:ListItem>
									    <asp:ListItem Text="Monto" Value="M"></asp:ListItem>
									</asp:DropDownList>
									<asp:TextBox ID="txtMonto" runat="server" Columns="6" onkeyup="NumericMaskE(this,event);"></asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid></FIELDSET>


				<FIELDSET><LEGEND class="Legends">Autorización</LEGEND>
					<TABLE class="main" cellSpacing="10">
						<TR>
							<TD>
								<asp:Label id="Label5" runat="server" Font-Bold="True">Ejecutivo Garantías: </asp:Label></TD>
							<TD>
								<asp:DropDownList id="ddlVendedor" runat="server"></asp:DropDownList></TD>
						</TR>
						<TR>
							<TD vAlign="top">
								<asp:Label id="Label30" runat="server" Font-Bold="True">Usuario que Solicita:</asp:Label></TD>
							<TD vAlign="top">
								<asp:textbox id="txtUsuario" runat="server" Width="400px" MaxLength="50"></asp:textbox></TD>
						</TR>
						
						<TR>
							<TD vAlign="top">
								<asp:Label id="Label9" runat="server" Font-Bold="True">Observación Solicitud de Garantía: </asp:Label></TD>
							<TD colSpan="3">
								<asp:textbox id="txtObsSolicitud" runat="server" Width="500px" Height="200px" TextMode="Multiline"></asp:textbox></TD>
						</TR>
						<TR>
							<TD vAlign="top">
								<asp:Label id="Label8" runat="server" Font-Bold="True">Observación Reposición de Repuestos: </asp:Label></TD>
							<TD colSpan="3">
								<asp:textbox id="txtObsReposicion" runat="server" Width="500px" Height="200px" TextMode="MultiLine"></asp:textbox></TD>
						</TR>
						<TR>
							<TD vAlign="top">
								<asp:Label id="Label12" runat="server" Font-Bold="True">Fecha Proceso:</asp:Label></TD>
							<TD vAlign="top">
								<asp:textbox id="txtFechaProceso" onkeyup="DateMask(this)" runat="server" Width="80px"></asp:textbox></TD>
						</TR>
						<TR>
							<TD align="center" colSpan="6">
								<asp:button id="btnEjecutar" onclick="btnEjecutar_Click" runat="server" Text="Aceptar" Width="200px"></asp:button></TD>
						</TR>
					</TABLE>
				</FIELDSET>

</asp:placeholder>
<asp:Label id="lblError" runat="server" Font-Bold="True"></asp:Label>
