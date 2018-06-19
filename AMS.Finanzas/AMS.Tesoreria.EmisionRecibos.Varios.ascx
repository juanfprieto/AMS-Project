<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EmisionRecibos.Varios.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ManejoVarios" %>
<p>
	<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
	<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
</p>
<p><asp:panel id="panelAbonos" Width="600px" runat="server" Visible="False">
		<P></P>
		<asp:DataGrid id="gridAbonos" Visible="true" runat="server" cssclass="datagrid" CellPadding="3" AutoGenerateColumns="False"	onUpdateCommand="gridAbonos_Update" onCancelCommand="gridAbonos_Cancel" onEditCommand="gridAbonos_Edit">
			<FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass=""></ItemStyle>
			<Columns>
				<asp:BoundColumn DataField="PREFIJO" ReadOnly="True" HeaderText="Prefijo del Pedido"></asp:BoundColumn>
				<asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="N&#250;mero del Pedido"></asp:BoundColumn>
				<asp:BoundColumn DataField="VALORPEDIDO" ReadOnly="True" HeaderText="Valor Total Pedido" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:BoundColumn DataField="VALORABONADO" ReadOnly="True" HeaderText="Valor Abonado" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Faltante">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem,"ABONO","{0:C}") %>
					</ItemTemplate>
					<EditItemTemplate>
						<asp:TextBox id="tbabono" width=100 runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ABONO","{0:N}") %>' onkeyup="NumericMaskE(this,event)" class="AlineacionDerecha" />
					</EditItemTemplate>
				</asp:TemplateColumn>
                <asp:BoundColumn DataField="RETOMAS" ReadOnly="True" HeaderText="Valor Retomas" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"
					ItemStyle-HorizontalAlign="Center"></asp:EditCommandColumn>

			</Columns>
		</asp:DataGrid>
        <p>
        <asp:Button ID="bntContinuar" runat="server" OnClick="CargarPanel" Text="Continuar"/>
        </p>
	</asp:panel>
<P></P>
<p></p>
<p></p>
<p><asp:panel id="panelPost" runat="server" Visible="False">
		<P></P>
		<asp:DataGrid id="gridPost" Visible="true" runat="server" cssclass="dategrid" CellPadding="3" AutoGenerateColumns="False"
			onUpdateCommand="gridPost_Update" onCancelCommand="gridPost_Cancel" onEditCommand="gridPost_Edit">
			<FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
			<Columns>
				<asp:BoundColumn DataField="TIPO" ReadOnly="True" HeaderText="Tipo"></asp:BoundColumn>
				<asp:BoundColumn DataField="CODIGOBANCO" ReadOnly="True" HeaderText="Banco"></asp:BoundColumn>
				<asp:BoundColumn DataField="NUMERODOCUMENTO" ReadOnly="True" HeaderText="N&#250;mero de Documento"></asp:BoundColumn>
				<asp:BoundColumn DataField="TIPOMONEDA" ReadOnly="True" HeaderText="Tipo de Moneda"></asp:BoundColumn>
				<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor ($)" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:BoundColumn DataField="VALORTC" ReadOnly="True" HeaderText="Tasa de Cambio" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:BoundColumn DataField="FECHA" ReadOnly="True" HeaderText="Fecha"></asp:BoundColumn>
				<asp:BoundColumn DataField="ESTADO" ReadOnly="True" HeaderText="Estado"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Nueva Fecha de Prorroga">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem,"FECPRO") %>
					</ItemTemplate>
					<EditItemTemplate>
						<asp:Textbox id="tbfec" width=100 runat="server" onkeyup="DateMask(this)" Text='<%# DataBinder.Eval(Container.DataItem,"FECPRO") %>' />
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor Intereses Cobrados">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem,"INTERES","{0:C}") %>
					</ItemTemplate>
					<EditItemTemplate>
						<asp:Textbox id="tbint" width=100 runat="server" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem,"INTERES","{0:N}") %>' class="AlineacionDerecha" />
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
			</Columns>
		</asp:DataGrid>
		<P>Total Intereses por Prorrogas :
			<asp:Label id="lbProrroga" runat="server" text="$0.00" visible="true"></asp:Label>&nbsp;&nbsp;
			<asp:Button id="btnAceptar" onclick="btnAceptar_Click" runat="server" Text="Aceptar" Enabled="False"></asp:Button></P>
	</asp:panel>
<P></P>
<p></p>
<p></p>
<p><asp:panel id="panelDevPed" Width="360px" runat="server" Visible="False">
		<P>
			<asp:DataGrid id="gridAbonosDev" Visible="true" runat="server" CellPadding="3" AutoGenerateColumns="False">
				<FooterStyle cssclass="footer"></FooterStyle>
				<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
				<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
				<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
				<ItemStyle cssclass="item"></ItemStyle>
				<Columns>
					<asp:BoundColumn DataField="PREFIJO" ReadOnly="True" HeaderText="Prefjijo Pedido"></asp:BoundColumn>
					<asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="Número Pedido"></asp:BoundColumn>
					<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor Total" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Valor a Devolver" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<asp:TextBox ID=tbdev Runat=server Text='<%# DataBinder.Eval(Container.DataItem, "ABONADO", "{0:N}") %>' Width=90 onKeyUp=NumericMaskE(this,event) CssClass=AlineacionDerecha>
							</asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Devolver" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<asp:CheckBox id="chbDevolver" runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid></P>
		<P>
			<asp:Button id="btnDevolver" onclick="btnDevolver_Click" runat="server" Text="Aceptar"></asp:Button></P>
	</asp:panel>
<P></P>
<input id="hdncli" type="hidden" runat="server"><input id="hdnben" type="hidden" name="hdnben" runat="server">
