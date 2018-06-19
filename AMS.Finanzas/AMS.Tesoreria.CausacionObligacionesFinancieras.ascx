<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tesoreria.CausacionObligacionesFinancieras.ascx.cs" Inherits="AMS.Finanzas.AMS_Tesoreria_CausacionObligacionesFinancieras" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>  
<TABLE>
	<TR>
		<TD>
			<TABLE>
				<TR>
					<TD>Documento :&nbsp;</TD>
					<td><asp:dropdownlist id="ddlTipoDocumento" runat="server" AutoPostBack="True" onselectedindexchanged="ddlCuentaEditar_SelectedIndexChanged"></asp:dropdownlist></td>
					<td>&nbsp;&nbsp;&nbsp;</td>
					<TD>Número :</TD>
					<TD><asp:label id="lblNumDocumento" runat="server"></asp:label></TD>
					<td>&nbsp;&nbsp;&nbsp;</td>
					<td></td>
				</TR>
				<TR>
					<TD>Año :</TD>
					<td><asp:dropdownlist id="ddlAno" runat="server"></asp:dropdownlist></td>
					<td>&nbsp;&nbsp;&nbsp;</td>
					<TD>Mes :</TD>
					<TD><asp:dropdownlist id="ddlMes" runat="server"></asp:dropdownlist></TD>
					<td>&nbsp;&nbsp;&nbsp;</td>
					<td></td>
					<td>&nbsp;&nbsp;&nbsp;</td>
				</TR>
				<TR>
					<TD>Almacén&nbsp;:</TD>
					<td><asp:dropdownlist id="ddlAlmacen" runat="server"></asp:dropdownlist></td>
					<td>&nbsp;&nbsp;&nbsp;</td>
					<td colspan="2"><asp:button id="btnSeleccionar" runat="server" Text="Seleccionar" Width="109px" onclick="btnSeleccionar_Click"></asp:button><asp:button id="btnAtras" runat="server" Text="Atras" Width="109px" Visible="false" onclick="btnAtras_Click"></asp:button></td>
				</TR>
			</TABLE>
		</TD>
	</TR>
</TABLE>
</fieldset>
<asp:panel id="pnlGrilla" Visible="False" Runat="server">
	<TABLE>
		<TR>
			<TD>
				<asp:datagrid id="dgrObligaciones" runat="server" cssclass="datagrid" OnEditCommand="dgrObligaciones_Edit" OnUpdateCommand="dgrObligaciones_Update"
					OnCancelCommand="dgrObligaciones_Cancel" AutoGenerateColumns="False">
					<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
					<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
					<ItemStyle cssclass="item"></ItemStyle>
					<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
					<FooterStyle cssclass="footer"></FooterStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Banco">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PBAN_NOMBRE") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PBAN_NOMBRE") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Cuenta">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PCUE_CODIGO") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PCUE_CODIGO") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Num.">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_NUMERO") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_NUMERO") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Fecha">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_FECHA", "{0:yyyy-MM-dd}") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_FECHA", "{0:yyyy-MM-dd}") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Tasa">
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PTAS_MONTO", "{0:#,##0.##}") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PTAS_MONTO", "{0:#,##0.##}") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Interes">
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_TASAINTERES", "{0:#,##0.##}") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_TASAINTERES", "{0:#,##0.##}") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Monto Ini.">
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTPESOS", "{0:C}") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTPESOS", "{0:C}") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Saldo Capital">
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_SALDO", "{0:C}") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_SALDO", "{0:C}") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Inter&#233;s Causado">
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_INTERESCAUSADO", "{0:C}") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_INTERESCAUSADO", "{0:C}") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Interes Calculado">
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOBL_INTERESCALCULADO", "{0:C}") %>
							</ItemTemplate>
							<EditItemTemplate>
								<asp:TextBox id="txtEdInteresCalc" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "MOBL_INTERESCALCULADO", "{0:#,##0.##}") %>' />
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
					</Columns>
					<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
				</asp:datagrid></TD>
		</TR>
		<TR>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD>Observaciones:</TD>
		</TR>
		<TR>
			<TD>
				<asp:textbox id="txtObservacion" runat="server" Width="600px" TextMode="Multiline" Height="60px"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD>
				<asp:button id="btnAceptar" runat="server" Text="Aceptar" Width="109px" onclick="btnAceptar_Click"></asp:button></TD>
		</TR>
		<TR>
			<TD>&nbsp;
				<asp:Label id="lblError" runat="server"></asp:Label></TD>
		</TR>
	</TABLE>
</asp:panel>
