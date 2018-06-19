<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.EmisionOrdenSalidaVehiculo.Emision.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.EmisionOrden" %>
<P>
<asp:datagrid id="dgVehiculos" runat="server" cssclass="datagrid" ShowFooter="False" AutoGenerateColumns="False" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="CC" HeaderText="Nit o CC del Propietario" />
			<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Propietario" />
			<asp:BoundColumn DataField="CATALOGO" HeaderText="Catalogo del Vehículo" />
			<asp:BoundColumn DataField="VIN" HeaderText="VIN del Vehículo" />
			<asp:BoundColumn DataField="PLACA" HeaderText="Placa del Vehículo" />
			<asp:BoundColumn DataField="MOTOR" HeaderText="Número de Motor del Vehículo" />
			<asp:BoundColumn DataField="COLOR" HeaderText="Color del Vehículo" />
            <asp:BoundColumn DataField="OT" HeaderText="Prefijo Orden de trabajo" />
            <asp:BoundColumn DataField="NUM_OT" HeaderText="Numero Orden de trabajo" />
			<asp:TemplateColumn HeaderText="Agregar a Orden de Salida ?">
				<ItemTemplate>
					<center>
						<asp:CheckBox id="chb" runat="server" />
					</center>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></P>
<P><asp:label id="lbInfo" runat="server"></asp:label></P>
<p><asp:datagrid id="dgFacturas" runat="server" cssclass="datagrid" AutoGenerateColumns="False"	CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="PREFIJO" HeaderText="Prefijo de la Factura"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO" HeaderText="N&#250;mero de la Factura"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" HeaderText="Valor Faltante de la Factura" DataFormatString="{0:C}"></asp:BoundColumn>
		</Columns>
	</asp:datagrid></p>
<p><asp:button id="btnGenerar" onclick="btnGenerar_Click" runat="server" Text="Generar Orden de Sálida"></asp:button>&nbsp;
	<asp:button id="btnCancelar" runat="server" Text="Cancelar" onclick="btnCancelar_Click"></asp:button></p>
<p><asp:label id="lb" runat="server"></asp:label></p>
