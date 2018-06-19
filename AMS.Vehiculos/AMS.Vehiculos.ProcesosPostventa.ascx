<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.ProcesosPostventa.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_ProcesosPostventa" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Vehiculos.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.WizardDirection.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
<script type ="text/javascript">
function FiltroVehiculos(txtObj){
	<%=ViewState["FILTRO_VEHICULOS"]%>
}
function VerProcedimientos(txtObj){
	ModalDialog(txtObj,"SELECT PT.PTEM_OPERACION CODIGO, PT.PTEM_DESCRIPCION NOMBRE FROM PTEMPARIO PT, ptiempotaller ptt where ptt.ptie_tempario=pt.ptem_operacion and ptt.ptie_grupcata='<%=ViewState["GRUPO_CATALOGO"]%>';",new Array(),1);
}
function VerRepuestos(txtObj){
	ModalDialog(txtObj,'SELECT mi.mite_codigo, mite_nombre FROM mitems mi, mprecioitem mp, ctaller ct where mi.mite_codigo = mp.mite_codigo and mp.ppre_codigo = ct.ppre_codigo;',new Array(),1);
}
</script>
<asp:placeholder id="plcSeleccion" runat="server">
	<TABLE>
		<TR>
			<TD>
				<asp:placeholder id="plcDistribuidorV" runat="server" Visible="False">
					<FIELDSET><LEGEND class="Legends">Selección del 
							Concesionario</LEGEND>
						<TABLE class="filstersIn" cellSpacing="10">
							<TR>
								<TD>
									<asp:Label id="Label25" runat="server" CssClass="Legends" wide="300px" Font-Bold="True">NIT: </asp:Label></TD>
								<TD>
									<asp:TextBox id="txtNitDistribuidor" class="tpequeno" onclick="ModalDialog(this,'SELECT MN.MNIT_NIT NIT, MN.MNIT_APELLIDOS concat \' \' concat COALESCE(MN.MNIT_APELLIDO2, \'\') concat \' \' concat MN.MNIT_nombres concat \' \' concat COALESCE(MN.MNIT_NOMBRE2, \'\') concat \' \' concat COALESCE(MN.MNIT_ESTABLECIMIENTO, \'\') NOMBRE, MN.MNIT_DIRECCION DIRECCION, MN.MNIT_TELEFONO TELEFONO FROM MNIT MN, MCONCESIONARIO MC,PCIUDAD PC WHERE MC.MNIT_NIT=MN.MNIT_NIT AND PC.PCIU_CODIGO=MN.PCIU_CODIGO;',new Array(),1)"
										runat="server" ReadOnly="True"></asp:TextBox></TD>
                                        </TR>
                                        <tr>
								<TD>
									<asp:Label id="Label26" runat="server" CssClass="Legends" Font-Bold="True">Nombre: </asp:Label></TD>
								<TD>
									<asp:TextBox id="txtNitDistribuidora" class="tpequeno" runat="server" ReadOnly="True"></asp:TextBox></TD>
							    </tr>
                            <TR>
								<TD>
									<asp:Label id="Label28" runat="server" CssClass="Legends" Font-Bold="True">Teléfono: </asp:Label></TD>
								<TD>
									<asp:TextBox id="txtNitDistribuidorc" class="tpequeno" runat="server" ReadOnly="True"></asp:TextBox></TD>
                                    </tr>
                                    <tr>
                                <TD>
									<asp:Label id="Label27" runat="server" CssClass="Legends" Font-Bold="True">Dirección: </asp:Label></TD>
								<TD>
									<asp:TextBox id="txtNitDistribuidorb" class="tpequeno" runat="server" ReadOnly="True" ></asp:TextBox></TD>
							</TR>
						</TABLE>
					</FIELDSET>
					<BR>
				</asp:placeholder>
				<FIELDSET><LEGEND class="Legends">Selección del 
						Vehículo</LEGEND>
					<TABLE class="filstersIn" cellSpacing="10">
						<TR>
							<TD>
								<asp:Label id="Label20" runat="server" CssClass="Legends" Font-Bold="True">VIN: </asp:Label></TD>
							<TD align="left" colSpan="3">
								<asp:TextBox id="txtVINVehiculo" onclick="FiltroVehiculos(this);" runat="server" CssClass="AlineacionDerecha"
									ReadOnly="True" class="tmediano"></asp:TextBox></TD>
							<TD>&nbsp;</TD>
							<TD>
								<asp:button id="btnSeleccionar" runat="server" class="bpequeno" Text="Seleccionar" onclick="btnSeleccionar_Click"></asp:button></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
</asp:placeholder><asp:placeholder id="plcProceso" runat="server" Visible="False">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Información 
						Vehículo</LEGEND>
					<TABLE class="filstersIn" cellSpacing="10">
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
								<asp:textbox id="txtPlaca" runat="server" class="lpequeno" MaxLength="10"></asp:textbox></TD>
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
								<asp:textbox id="txtKilometraje" onkeyup="NumericMaskE(this,event)" runat="server" Width="80px" MaxLength="10"></asp:textbox></TD>
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
				<asp:placeholder id="plcInfoInicial" runat="server" Visible="False">
					<FIELDSET><LEGEND class="Legends">Factura de Entrega 
							al Cliente</LEGEND>
						<TABLE class="filstersIn" cellSpacing="10">
							<TR>
								<TD vAlign="top">
									<asp:Label id="Label32" runat="server" CssClass="Legends" Font-Bold="True">Factura: </asp:Label></TD>
								<TD vAlign="top">
									<asp:Label id="lblFactIni" runat="server" Font-Bold="True"></asp:Label></TD>
								<TD vAlign="top" rowSpan="2">
									<asp:Label id="Label35" runat="server" CssClass="Legends" Font-Bold="True">Distribuidor: </asp:Label></TD>
								<TD vAlign="top" rowSpan="2">
									<asp:Label id="lblFactDitri" runat="server" Font-Bold="True"></asp:Label></TD>
							</TR>
							<TR>
								<TD vAlign="top">
									<asp:Label id="Label33" runat="server" CssClass="Legends" Font-Bold="True">Fecha Entrega: </asp:Label></TD>
								<TD vAlign="top">
									<asp:Label id="lblFactFecha" runat="server" Font-Bold="True"></asp:Label></TD>
							</TR>
						</TABLE>
					</FIELDSET>
				</asp:placeholder></TD>
		</TR>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Información 
						Distribuidor o Taller Que Ejecuta La Acción</LEGEND>
					<TABLE class="filstersIn" cellSpacing="10">
						<TR>
							<TD>
								<asp:Label id="Label4" runat="server" CssClass="Legends" Font-Bold="True">NIT: </asp:Label></TD>
							<TD>
								<asp:Label id="lblNITDistribuidor" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label8" runat="server" CssClass="Legends" Font-Bold="True">Nombre: </asp:Label></TD>
							<TD>
								<asp:Label id="lblNombreDistribuidor" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label12" runat="server" CssClass="Legends" Font-Bold="True">Teléfono: </asp:Label></TD>
							<TD>
								<asp:Label id="lblTelefonoDistribuidor" runat="server" Font-Bold="True"></asp:Label></TD>
							<TD>
								<asp:Label id="Label10" runat="server" CssClass="Legends" Font-Bold="True">Dirección: </asp:Label></TD>
							<TD>
								<asp:Label id="lblDireccionDistribuidor" runat="server" Font-Bold="True"></asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label37" runat="server" CssClass="Legends" Font-Bold="True">Categoría: </asp:Label></TD>
							<TD>
								<asp:Label id="lblCategoria" runat="server" Font-Bold="True"></asp:Label></TD>
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
								<asp:Label id="Label6" runat="server" CssClass="Legends" class="tmediano" Font-Bold="True">Documento: </asp:Label></TD>
							<TD>
								<asp:TextBox id="txtNITPropietario" runat="server" ReadOnly="False" MaxLength=15></asp:TextBox></TD>
							<TD>
								<asp:Label id="Label11" runat="server" CssClass="Legends" Font-Bold="True">Nombre: </asp:Label></TD>
							<TD>
								<asp:TextBox id="txtNITPropietarioa" runat="server" ReadOnly="False" Width="400px" MaxLength=200></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label9" runat="server" CssClass="Legends" Font-Bold="True">Teléfono: </asp:Label></TD>
							<TD>
								<asp:TextBox id="txtNITPropietarioc" runat="server" ReadOnly="False" MaxLength=40></asp:TextBox></TD>
							<TD>
								<asp:Label id="Label14" runat="server" CssClass="Legends" Font-Bold="True">Dirección: </asp:Label></TD>
							<TD>
								<asp:TextBox id="txtNITPropietariob" onclick="WizardDirection(this);" runat="server" ReadOnly="True"
									Width="400px"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label15" runat="server" CssClass="Legends" Font-Bold="True">Celular: </asp:Label></TD>
							<TD>
								<asp:TextBox id="txtNITPropietariod" runat="server" ReadOnly="False" MaxLength=12></asp:TextBox></TD>
							<TD>
								<asp:Label id="Label16" runat="server" CssClass="Legends" Font-Bold="True">Ciudad: </asp:Label></TD>
							<TD>
								<asp:DropDownList id="txtNITPropietarioe" runat="server"></asp:DropDownList></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:placeholder id="plcFacturaInicial" runat="server" Visible="False">
					<FIELDSET><LEGEND class="Legends">Información 
							Entrega Inicial</LEGEND>
						<TABLE class="filstersIn" cellSpacing="10">
							<TR>
								<TD>
									<asp:Label id="Label13" runat="server" CssClass="Legends" Font-Bold="True">Prefijo Factura: </asp:Label></TD>
								<TD>
									<asp:textbox id="txtPrefFactura" runat="server" class="tpequeno"></asp:textbox></TD>
								<TD>
									<asp:Label id="Label18" runat="server" CssClass="Legends" Font-Bold="True">Numero Factura: </asp:Label></TD>
								<TD>
									<asp:textbox id="txtNumFactura" runat="server" class="lpequeno"></asp:textbox></TD>
							</TR>
							<TR>
								<TD>
									<asp:Label id="Label21" runat="server" CssClass="Legends" Font-Bold="True">Fecha Facturación:</asp:Label></TD>
								<TD>
									<asp:textbox id="txtFechaFact" onkeyup="DateMask(this)" runat="server" Width="80px"></asp:textbox></TD>
							</TR>
						</TABLE>
					</FIELDSET>
				</asp:placeholder>
				<FIELDSET><LEGEND class="Legends">Información 
						Proceso</LEGEND>
					<TABLE class="filstersIn">
						<TR>
							<TD vAlign="top">
								<TABLE class="main" cellSpacing="10">
									<TR>
										<TD vAlign="top">
											<asp:Label id="Label30" runat="server" CssClass="Legends" Font-Bold="True">Fecha Proceso:</asp:Label></TD>
										<TD vAlign="top">
											<asp:textbox id="txtFechaProceso" onkeyup="DateMask(this)" runat="server" class="tpequeno"></asp:textbox></TD>
									</TR>
								</TABLE>
							</TD>
							<TD vAlign="top">
								<asp:placeholder id="plcOrdenProceso" runat="server" Visible="False">
									<TABLE class="main" cellSpacing="10">
										<TR>
											<TD vAlign="top">
												<asp:Label id="Label34" runat="server" CssClass="Legends" Font-Bold="True">Prefijo Factura Concesionario:</asp:Label></TD>
											<TD vAlign="top">
												<asp:textbox id="txtPrefOrdenProc" runat="server" class="tpequeno" MaxLength="6"></asp:textbox></TD>
											<TD vAlign="top">
												<asp:Label id="Label36" runat="server" CssClass="Legends" Font-Bold="True">Número Factura Concesionario:</asp:Label></TD>
											<TD vAlign="top">
												<asp:textbox id="txtNumOrdenProc" runat="server" class="tpequeno" MaxLength="10"></asp:textbox></TD>
										</TR>
									</TABLE>
								</asp:placeholder></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Observaciones</LEGEND>
					<TABLE class="filstersIn" cellSpacing="10">
						<TR>
							<TD vAlign="top">
								<asp:Label id="Label17" runat="server" CssClass="Legends" Font-Bold="True">Cliente: </asp:Label></TD>
							<TD>
								<asp:textbox id="txtObservacionesCliente" runat="server" class="amediano" MaxLength="400" TextMode="MultiLine"
									Height="80px"></asp:textbox></TD>
						</TR>
						<TR>
							<TD vAlign="top">
								<asp:Label id="Label22" runat="server" CssClass="Legends" Font-Bold="True">Taller:</asp:Label></TD>
							<TD>
								<asp:textbox id="txtObservacionesTaller" runat="server" class="amediano" MaxLength="400" TextMode="MultiLine"
									Height="80px"></asp:textbox></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Lista de Chequeo</LEGEND>
					<asp:placeholder id="plcManteniProg" runat="server" Visible="False">
						<TABLE class="filstersIn" cellSpacing="10">
							<TR>
								<TD>
									<asp:Label id="Label19" runat="server" CssClass="Legends" Font-Bold="True">Mantenimiento Programado: </asp:Label></TD>
								<TD>
									<asp:DropDownList id="ddlMantenimiento" runat="server" OnSelectedIndexChanged="Cambia_MantenimientoProg"
										AutoPostBack="true"></asp:DropDownList></TD>
							</TR>
						</TABLE>
					</asp:placeholder>
					<asp:placeholder id="plcOperaciones" runat="server" Visible="False">
						<TABLE class="main" cellSpacing="10">
							<TR>
								<TD>Operaciones:<BR>
									<asp:DataGrid id="dgrKitsMantenimientoOperaciones" runat="server" cssclass="datagrid" OnItemDataBound="dgrOpers_Bound"
										AutoGenerateColumns="false" GridLines="Vertical"
										HeaderStyle-BackColor="#ccccdd">
										<HeaderStyle CssClass="header"></HeaderStyle>
						                <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						                <ItemStyle CssClass="item"></ItemStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="CODIGO">
												<HeaderStyle Width="20%"></HeaderStyle>
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="NOMBRE">
												<HeaderStyle Width="45%"></HeaderStyle>
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
											<asp:TemplateColumn HeaderText="TIEMPO">
												<HeaderStyle Width="8%"></HeaderStyle>
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "TIEMPO") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Incidente">
												<HeaderStyle Width="2%"></HeaderStyle>
												<ItemTemplate>
													<asp:DropDownList ID="ddlIncidente" Runat="server"></asp:DropDownList>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Causal">
												<HeaderStyle Width="2%"></HeaderStyle>
												<ItemTemplate>
													<asp:DropDownList ID="ddlCausal" Runat="server"></asp:DropDownList>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="">
												<HeaderStyle Width="2%"></HeaderStyle>
												<ItemTemplate>
													<asp:CheckBox ID="chkUsar" Runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"USAR")) %>'>
													</asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:DataGrid></TD>
							</TR>
							<TR>
								<TD>
									<asp:placeholder id="plcSelProcedimiento" runat="server" Visible="False">
										<TABLE>
											<TR>
												<TD>
													<asp:TextBox id="txtProcedimiento" onclick="VerProcedimientos(this);" ReadOnly="True" Width="80px"
														Runat="server"></asp:TextBox></TD>
												<TD>
													<asp:TextBox id="txtProcedimientoa" ReadOnly="True" Width="160px" Runat="server"></asp:TextBox></TD>
												<TD>
													<asp:Button id="btnProcedimiento" onclick="AgregarProcedimiento" runat="server" Text="Agregar Operacion"
														width="160px"></asp:Button><BR>
												</TD>
											</TR>
										</TABLE>
									</asp:placeholder></TD>
							</TR>
						</TABLE>
					</asp:placeholder>
					<asp:placeholder id="plcRepuestos" runat="server" Visible="False">
						<TABLE class="filstersIn" cellSpacing="10">
							<TR>
								<TD>Repuestos:<BR>
									<asp:DataGrid id="dgrKitsMantenimientoRepuestos" runat="server" cssclass="datagrid" AutoGenerateColumns="false" GridLines="Vertical">
										<HeaderStyle CssClass="header"></HeaderStyle>
						                <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						                <ItemStyle CssClass="item"></ItemStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="CODIGO">
												<HeaderStyle Width="18%"></HeaderStyle>
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="NOMBRE">
												<HeaderStyle Width="70%"></HeaderStyle>
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="CANTIDAD">
												<HeaderStyle Width="5%"></HeaderStyle>
												<ItemTemplate>
													<asp:TextBox id="txtCantidad" runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>'>
													</asp:TextBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="VALOR<BR>UNIDAD">
												<HeaderStyle Width="5%"></HeaderStyle>
												<ItemTemplate>
													<asp:TextBox id="txtValor" runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "PRECIO", "{0:#,##0}") %>'>
													</asp:TextBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="">
												<HeaderStyle Width="2%"></HeaderStyle>
												<ItemTemplate>
													<asp:CheckBox ID="chkUsarR" Runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"USAR")) %>'>
													</asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:DataGrid></TD>
							</TR>
							<TR>
								<TD>
									<asp:placeholder id="plcSelRepuestos" runat="server" Visible="False">
										<TABLE>
											<TR>
												<TD>
													<asp:TextBox id="txtRepuesto" onclick="VerRepuestos(this);" ReadOnly="True" class="tpequeno" Runat="server"></asp:TextBox></TD>
												<TD>
													<asp:TextBox id="txtRepuestoa" ReadOnly="True" class="tmediano" Runat="server"></asp:TextBox></TD>
												<TD>
													<asp:Button id="btnRepuestos" onclick="AgregarRepuesto" runat="server" Text="Agregar Repuesto"
														class="bmediano"></asp:Button><BR>
												</TD>
											</TR>
										</TABLE>
									</asp:placeholder></TD>
							</TR>
						</TABLE>
					</asp:placeholder></FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:placeholder id="plcAutorizacionesGarantia" runat="server" Visible="False">
					<FIELDSET><LEGEND class="Legends">Autorizaciones</LEGEND>
						<asp:DataGrid id="dgrAutorizacionesGarantia" runat="server" cssclass="datagrid" OnItemDataBound="dgrAutorizaciones_Bound"
							AutoGenerateColumns="false" GridLines="Vertical">
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
								<asp:TemplateColumn HeaderText="FECHA">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "FECHA", "{0:yyyy-MM-dd}") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="OBSERVACION">
									<HeaderStyle Width="75%"></HeaderStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "OBSERVACION") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid></FIELDSET>
				</asp:placeholder></TD>
		</TR>
		<TR>
			<TD>
				<asp:placeholder id="plcMantenimientosAnt" runat="server" Visible="False">
					<FIELDSET><LEGEND class="Legends">Historial</LEGEND>Kits:<BR>
						<asp:DataGrid id="dgrMantenimientosAnt" runat="server" cssclass="datagrid" AutoGenerateColumns="false" GridLines="Vertical">
							<HeaderStyle CssClass="header"></HeaderStyle>
						    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						    <ItemStyle CssClass="item"></ItemStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="NOMBRE">
									<HeaderStyle Width="20%"></HeaderStyle>
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
							</Columns>
						</asp:DataGrid><BR>
						<BR>
						Repuestos:<BR>
						<asp:DataGrid id="dgrRepuestosAnt" runat="server" cssclass="datagrid" AutoGenerateColumns="false" GridLines="Vertical">
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
							</Columns>
						</asp:DataGrid></FIELDSET>
				</asp:placeholder></TD>
		</TR>
		<TR>
			<TD align="center"><BR>
				<asp:Button id="btnAceptar" onclick="Aceptar" runat="server" Text="Aceptar"></asp:Button><BR>
				<BR>
			</TD>
		</TR>
	</TABLE>
</asp:placeholder>
<p><asp:label id="lblError" runat="server"></asp:label></p>
