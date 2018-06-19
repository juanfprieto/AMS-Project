<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tesoreria.ObligacionesFinancieras.ascx.cs" Inherits="AMS.Finanzas.AMS_Tesoreria_ObligacionesFinancieras" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<asp:panel id="pnlSeleccion" Visible="True" Runat="server">
<fieldset>
	<TABLE class="filters">
		<TR>
			<th class="filterHead">
            <img height="50" src="../img/AMS.Flyers.Nueva.png" border="0">
            </th>
			<td>
				<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:Button id="btnNuevo" Text="Nuevo" Width="109px" runat="server" onclick="btnNuevo_Click"></asp:Button></P>
			</td>
		</TR>
		<TR>
			<th class="filterHead">
            <img height="50" src="../img/AMS.Flyers.Edits.png" border="0">
            </th>
			<td>
				<P>Cuenta :
					<asp:DropDownList id="ddlCuentaEditar" runat="server" AutoPostBack="True" onselectedindexchanged="ddlCuentaEditar_SelectedIndexChanged"></asp:DropDownList></P>
				<P>Número :
					<asp:DropDownList id="ddlCreditoEditar" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:Button id="btnEdit" Text="Editar" Width="109px" runat="server" onclick="btnEdit_Click"></asp:Button></P>
			</td>
		</TR>
	</TABLE>
</fieldset>
</asp:panel><asp:panel id="pnlCredito" Visible="False" Runat="server">
	<TABLE>
		<TR>
			<TD>Cuenta Bancaria Pasivo :
			</TD>
			<TD>
				<asp:dropdownlist id="ddlCuenta" runat="server"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD>Número Pagaré :
			</TD>
			<TD>
				<asp:TextBox id="txtNumero" Width="100px" runat="server" MaxLength="15"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Fecha Desembolso :
			</TD>
			<TD>
				<asp:TextBox id="txtFechaDesemb" onkeyup="DateMask(this)" Width="92px" runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Documento :</TD>
			<TD>
				<asp:dropdownlist id="ddlDocumento" runat="server" AutoPostBack="True" onselectedindexchanged="ddlDocumento_SelectedIndexChanged"></asp:dropdownlist>&nbsp;&nbsp;
				<asp:Label id="lblNumDocumento" runat="server"></asp:Label></TD>
		</TR>
		<TR>
			<TD>Almacén :</TD>
			<TD>
				<asp:dropdownlist id="ddlAlmacen" runat="server"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD>Monto Pesos :</TD>
			<TD>
				<asp:TextBox id="txtMontoPesos" onkeyup="NumericMaskE(this,event)" Width="100px" runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Monto Dólares :</TD>
			<TD>
				<asp:TextBox id="txtMontoDolares" onkeyup="NumericMaskE(this,event)" Width="100px" runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Número Cuotas :</TD>
			<TD>
				<asp:TextBox id="txtNumCuotas" onkeyup="NumericMaskE(this,event)" Width="100px" runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Tipo Crédito :</TD>
			<TD>
				<asp:dropdownlist id="ddlTipoCredito" runat="server"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 23px">Tipo Tasa :</TD>
			<TD style="HEIGHT: 23px">
				<asp:dropdownlist id="ddlTipoTasa" runat="server"></asp:dropdownlist></TD>
			<TD>Monto Pagado :</TD>
			<TD>
				<asp:TextBox id="txtMontoPagado" Width="100px" runat="server" ReadOnly="True"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>% Interes del Crédito :</TD>
			<TD>
				<asp:TextBox id="txtInteresCredito" onkeyup="NumericMaskE(this,event)" Width="100px" runat="server"></asp:TextBox></TD>
			<TD>Intereses Pagados :</TD>
			<TD>
				<asp:TextBox id="txtInteresPagado" Width="100px" runat="server" ReadOnly="True"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Condición :</TD>
			<TD>
				<asp:dropdownlist id="ddlCondicion" runat="server"></asp:dropdownlist></TD>
			<TD>Intereses Causados :</TD>
			<TD>
				<asp:TextBox id="txtInteresCausado" Width="100px" runat="server" ReadOnly="True"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD vAlign="top">Observación :</TD>
			<TD colSpan="3">
				<asp:TextBox id="txtDetalle" Width="500px" runat="server" Height="120px" TextMode="MultiLine"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD vAlign="top">Autoriza :</TD>
			<TD colSpan="3">
				<asp:TextBox id="txtAutoriza" Width="300px" runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD colSpan="2"></TD>
		</TR>
	</TABLE>
	<BR>
	<TABLE>
		<TR>
			<TD vAlign="top">
				<TABLE>
					<TR>
						<TD>Plan de 
							Pagos&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Button id="btnPagos" Text="Generar" Width="109px" runat="server" onclick="btnPagos_Click"></asp:Button></TD>
					</TR>
					<TR>
						<TD>
							<asp:DataGrid id="dgrPagos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" ShowFooter="True" OnCancelCommand="dgrPagos_Cancel"
								OnUpdateCommand="dgrPagos_Update" OnEditCommand="dgrPagos_Edit">
								<FooterStyle cssclass="footer"></FooterStyle>
								<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
								<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
								<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
								<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
								<ItemStyle cssclass="item"></ItemStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="&nbsp;&nbsp;">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "DOBL_NUMEPAGO") %>
										</ItemTemplate>
										<EditItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "DOBL_NUMEPAGO") %>
										</EditItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Fecha" ItemStyle-HorizontalAlign="Center">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "MOBL_FECHA", "{0:yyyy-MM-dd}") %>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:TextBox id="txtEdFechaPago" runat="server" Enabled="true" onkeyup="DateMask(this)" CssClass="AlineacionDerecha" Width="80px" Text='<%# DataBinder.Eval(Container.DataItem, "MOBL_FECHA", "{0:yyyy-MM-dd}") %>' />
										</EditItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtFechaPago" runat="server" Enabled="true" onkeyup="DateMask(this)" CssClass="AlineacionDerecha"
												Width="80px"></asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Monto Capital" ItemStyle-HorizontalAlign="Right">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTCAPI", "{0:C}") %>
										</ItemTemplate>
										<EditItemTemplate>
										</EditItemTemplate>
										<FooterTemplate>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Capital" ItemStyle-HorizontalAlign="Right">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTPESOS", "{0:C}") %>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:TextBox id="txtEdCapitalPago" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTPESOS", "{0:#,##0.##}") %>' />
										</EditItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtCapitalPago" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha"
												Width="100px"></asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Interes" ItemStyle-HorizontalAlign="Right">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTINTERES", "{0:C}") %>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:TextBox id="txtEdInteresPago" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTINTERES", "{0:#,##0.##}") %>' />
										</EditItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtInteresPago" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha"
												Width="100px"></asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="" FooterStyle-HorizontalAlign="Center">
										<ItemTemplate>
											<asp:Button id="remPago" runat="server" CommandName="RemoverPago" Text="Remover" />
										</ItemTemplate>
										<FooterTemplate>
											<asp:Button id="agPago" runat="server" CommandName="AgregarPago" Text="Agregar" Enabled="true" />
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
								</Columns>
							</asp:DataGrid></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
	</TABLE>
	<BR>
	<TABLE>
		<TR>
			<TD vAlign="top">
				<TABLE>
					<TR>
						<TD>Monto Desembolsado</TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 18px">a la Cuenta :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:TextBox id="txtCuentaDesemP" onkeyup="NumericMaskE(this,event)" Width="100px" runat="server"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD>
							<asp:DataGrid id="dgrDesembolsos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" ShowFooter="True">
								<FooterStyle cssclass="footer"></FooterStyle>
								<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
								<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
								<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
								<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
								<ItemStyle cssclass="item"></ItemStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="Detalle" ItemStyle-HorizontalAlign="Left">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "DOBL_RAZON") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtDetalleDesem" runat="server" Enabled="true" Width="200px"></asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Cuenta" ItemStyle-HorizontalAlign="Left">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "MCUE_CODIPUC") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:textbox id="txtCuentaDesem" onclick="ModalDialog(this,'SELECT MCUE_CODIPUC, MCUE_NOMBRE FROM MCUENTA WHERE TIMP_CODIGO<>\'N\' ORDER BY MCUE_CODIPUC;',new Array())"
												Runat="server" ReadOnly="True" Width="80px"></asp:textbox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Pref. Doc." ItemStyle-HorizontalAlign="Left">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "PDOC_CODIDOCUREFE") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtPrefDocDesem" runat="server" Enabled="true" CssClass="AlineacionDerecha"
												Width="50px"></asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Num. Doc." ItemStyle-HorizontalAlign="Left">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "DOBL_NUMEDOCUREFE") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtNumDocDesem" runat="server" Enabled="true" CssClass="AlineacionDerecha" Width="100px"></asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Beneficiario" ItemStyle-HorizontalAlign="Left">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "MNIT_NIT") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:textbox id="txtNitDesem" onclick="ModalDialog(this,'SELECT mnit_nit AS NIT,mnit_apellidos || \' \' || COALESCE(mnit_apellido2,\'\') || \' \' || mnit_nombres || \' \' || COALESCE(mnit_nombre2,\'\')  AS NOMBRE FROM dbxschema.mnit ORDER BY mnit_nit;',new Array())"
												Runat="server" ReadOnly="True" Width="80px"></asp:textbox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Almacén" ItemStyle-HorizontalAlign="Left">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "PALM_ALMACEN") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:textbox id="txtAlmacenDesem" onclick="ModalDialog(this,'SELECT PALM_ALMACEN, PALM_DESCRIPCION FROM PALMACEN where pcen_centcart is not null or pcen_centteso is not null order by palm_descripcion;',new Array())"
												Runat="server" ReadOnly="True" Width="50px"></asp:textbox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Centro Costo" ItemStyle-HorizontalAlign="Left">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "PCEN_CODIGO") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:textbox id="txtCentroDesem" onclick="ModalDialog(this,'SELECT PCEN_CODIGO, PCEN_NOMBRE FROM PCENTROCOSTO where timp_codigo <> \'N\' ORDER BY PCEN_NOMBRE;',new Array())"
												Runat="server" ReadOnly="True" Width="50px"></asp:textbox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Débito" ItemStyle-HorizontalAlign="Right">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "DOBL_VALODEBI", "{0:C}") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtDebitoDesem" runat="server" Enabled="true" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)"
												Width="80px">0</asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Crédito" ItemStyle-HorizontalAlign="Right">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "DOBL_VALOCRED", "{0:C}") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtCreditoDesem" runat="server" Enabled="true" CssClass="AlineacionDerecha"
												onkeyup="NumericMaskE(this,event)" Width="80px">0</asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Base" ItemStyle-HorizontalAlign="Right">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "DOBL_VALOBASE", "{0:C}") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:TextBox id="txtBaseDesem" runat="server" Enabled="true" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)"
												Width="80px">0</asp:TextBox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="" FooterStyle-HorizontalAlign="Center">
										<ItemTemplate>
											<asp:Button id="remDesem" runat="server" CommandName="RemoverDesem" Text="Remover" />
										</ItemTemplate>
										<FooterTemplate>
											<asp:Button id="agrDesem" runat="server" CommandName="AgregarDesem" Text="Agregar" Enabled="true" />
										</FooterTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:DataGrid></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
	</TABLE>
	<BR>
	<TABLE>
		<TR>
			<TD>
				<asp:Button id="btnAceptar" Text="Aceptar" Width="109px" runat="server" onclick="btnAceptar_Click"></asp:Button></TD>
		</TR>
		<TR>
			<TD>
				<asp:label id="lblInfo" runat="server"></asp:label></TD>
		</TR>
	</TABLE>
</asp:panel>
