<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Produccion.ProcesoNacionalizacion.ascx.cs" Inherits="AMS.Produccion.AMS_Produccion_ProcesoNacionalizacion" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table style="WIDTH: 600px">
	<tbody>
		<tr>
			<td>
				<fieldset><legend>Vehículos:</legend>
					<table class="main">
						<tbody>
							<tr>
								<td colSpan="4">
									<p><asp:label id="Label10" forecolor="RoyalBlue" runat="server">VEHÍCULOS POR NACIONALIZAR</asp:label></p>
								</td>
							</tr>
							<tr>
								<td>
									<P>
										<ASP:DataGrid id="dgEnsambles" runat="server" AutoGenerateColumns="false" ShowFooter="True" cssclass="datagrid" GridLines="Vertical">
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
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<br>
<table style="WIDTH: 600px">
	<tbody>
		<tr>
			<td>
				<fieldset><legend>Datos Nacionalización:</legend>
					<table class="main">
						<tbody>
							<tr>
								<td>
									<table class="main">
										<tbody>
											<tr>
												<td colSpan="4">
													<p><asp:label id="Label1" forecolor="RoyalBlue" runat="server">INFORMACIÓN VEHÍCULO</asp:label></p>
												</td>
											</tr>
											<tr>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Prefijo Documento:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlPrefDoc" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefDoc_SelectedIndexChanged"></asp:dropdownlist></p>
												</td>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Número Documento:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lblNumDoc" runat="server"></asp:label></p>
												</td>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Manifiesto:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:textbox id="txtManifiesto" runat="server" MaxLength="20" ReadOnly="True"></asp:textbox></p>
												</td>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;D.O.:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:textbox id="txtDO" runat="server" MaxLength="20"></asp:textbox></p>
												</td>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Levante:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:textbox id="txtLevante" runat="server" MaxLength="20" ReadOnly="True"></asp:textbox></p>
												</td>
											</tr>
											<tr>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Aduana:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:textbox id="txtAduana" runat="server" MaxLength="20"></asp:textbox></p>
												</td>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Fecha:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:textbox id="txtFecha" onkeyup="DateMask(this)" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox></p>
												</td>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Embarque:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlEmbarque" runat="server" AutoPostBack="True" onselectedindexchanged="ddlEmbarque_SelectedIndexChanged"></asp:dropdownlist></p>
												</td>
											</tr>
											<tr>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Tasa Cambio<br>
														&nbsp;&nbsp;&nbsp;&nbsp;Declaración Importación:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtTasaCambioI" runat="server" onkeyup="NumericMaskE(this,event)" class="tpequeno"></asp:TextBox></p>
												</td>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;Tasa Cambio<br>
														&nbsp;&nbsp;&nbsp;&nbsp;Nacionalización:<br>
														&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtTasaCambio" runat="server" onkeyup="NumericMaskE(this,event)" class="tpequeno"></asp:TextBox></p>
												</td>
												<td>
													<p>&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="btnSeleccionar" runat="server" Text="Seleccionar" Enabled="True" onclick="btnSeleccionar_Click"></asp:button></p>
												</td>
												<td>&nbsp;</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<br>
<asp:PlaceHolder ID="plcNacionalizar" Runat="server" Visible="False">
	<TABLE style="WIDTH: 600px">
		<TR>
			<TD>
				<FIELDSET><LEGEND>Gastos:</LEGEND>
					<TABLE class="main">
						<TR>
							<TD><BR>
								<ASP:DATAGRID id="dgrGastos" runat="server" cssclass="datagrid" GridLines="Vertical" AutoGenerateColumns="false" EnableViewState="true">
						            <HeaderStyle CssClass="header"></HeaderStyle>
						            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						            <ItemStyle CssClass="item"></ItemStyle>
									<Columns>
										<asp:TemplateColumn HeaderText="Orden:">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "PREFO") %>-<%# DataBinder.Eval(Container.DataItem, "NUMO") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="Gasto:">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "GASTO") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="Total:">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:N}") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="Nacional:">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "PGAS_MODENACI") %>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</ASP:DATAGRID><BR>
							</TD>
						</TR>
						<TR>
							<TD>
								<TABLE class="main">
									<TR>
										<TD>Total Embarque:&nbsp;</TD>
										<TD>
											<asp:TextBox id="txtTotalEmbarque" runat="server" ReadOnly="True" class="tmediano"></asp:TextBox></TD>
									</TR>
									<TR>
										<TD>Total Unidades:&nbsp;</TD>
										<TD>
											<asp:TextBox id="txtTotalUnidades" runat="server" ReadOnly="True" class="tmediano"></asp:TextBox></TD>
									</TR>
									<TR>
										<TD>Total Gastos Moneda Extranjera:&nbsp;</TD>
										<TD>
											<asp:TextBox id="txtTotalGastosE" runat="server" ReadOnly="True" class="tmediano"></asp:TextBox></TD>
									</TR>
									<TR>
										<TD>Total Gastos Moneda Nacional:&nbsp;</TD>
										<TD>
											<asp:TextBox id="txtTotalGastosN" runat="server" ReadOnly="True" class="tmediano"></asp:TextBox></TD>
									</TR>
									<TR>
										<TD>Subtotal:&nbsp;</TD>
										<TD>
											<asp:TextBox id="txtSubtotal" runat="server" ReadOnly="True" class="tmediano"></asp:TextBox></TD>
									</TR>
									<TR>
										<TD>IVA:&nbsp;</TD>
										<TD>
											<asp:TextBox id="txtIVA" runat="server" ReadOnly="True" class="tmediano"></asp:TextBox></TD>
									</TR>
									<TR>
										<TD>Total:&nbsp;</TD>
										<TD>
											<asp:TextBox id="txtTotal" runat="server" ReadOnly="True" Width="200px"></asp:TextBox></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
		<TR>
			<TD><BR>
				&nbsp;&nbsp;
				<asp:button id="btEjecutar" runat="server" Enabled="True" Text="Ejecutar" onclick="btEjecutar_Click"></asp:button><BR>
			</TD>
		</TR>
	</TABLE>
</asp:PlaceHolder>
<asp:label id="lblInfo" runat="server"></asp:label>
<%=scrFocus%>
