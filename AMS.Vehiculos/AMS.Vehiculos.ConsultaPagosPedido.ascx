<%@ Control Language="c#" codebehind="AMS.Vehiculos.ConsultaPagosPedido.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.ConsultaPagosPedido" %>
<fieldset>
	<legend>Información Pedido</legend>
	<table class="main" cellspacing="10">
		<tbody>
			<tr>
				<td>
					<asp:Label id="lbInfo1" runat="server" forecolor="RoyalBlue">Prefijo Pedido :</asp:Label>&nbsp;
					<asp:Label id="prefijoPedido" runat="server"></asp:Label></td>
				<td>
					<asp:Label id="lbInfo2" runat="server" forecolor="RoyalBlue">Número del Pedido :</asp:Label><asp:Label id="numeroPedido" runat="server"></asp:Label></td>
				<td>
					<asp:Label id="lbInfo3" runat="server" forecolor="RoyalBlue">Tipo de Vehículo :</asp:Label>&nbsp;<asp:Label id="catalogo" runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td>
					<asp:Label id="lbInfo4" runat="server" forecolor="RoyalBlue">Color Primario :</asp:Label>&nbsp;
					<asp:Label id="colorPrimario" runat="server"></asp:Label></td>
				<td>
					<asp:Label id="lbInfo5" runat="server" forecolor="RoyalBlue">Color Opcional :</asp:Label>&nbsp;<asp:Label id="colorOpcional" runat="server"></asp:Label></td>
				<td>
					<asp:Label id="lbInfo6" runat="server" forecolor="RoyalBlue">Clase Vehículo :</asp:Label>&nbsp;<asp:Label id="claseVehiculo" runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td>
					<asp:Label id="lbInfo7" runat="server" forecolor="RoyalBlue">Año Modelo :</asp:Label>&nbsp;<asp:Label id="anoModelo" runat="server"></asp:Label></td>
				<td>
					<asp:Label id="lbInfo8" runat="server" forecolor="RoyalBlue">Fecha de Pedido :</asp:Label>&nbsp;<asp:Label id="fechaPedido" runat="server"></asp:Label></td>
				<td>
					<asp:Label id="lbInfo9" runat="server" forecolor="RoyalBlue">Fecha de Entrega :</asp:Label>&nbsp;<asp:Label id="fechaEntrega" runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td>
					<asp:Label id="lbInfo10" runat="server" forecolor="RoyalBlue">Vendedor :</asp:Label>&nbsp;
					<asp:Label id="lbVendedor" runat="server"></asp:Label></td>
				<td colspan="2">
					<asp:Label id="lbInfo11" runat="server" forecolor="RoyalBlue">Almacén de Venta :</asp:Label>&nbsp;
					<asp:Label id="sedeVenta" runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td>
					<asp:Label id="lbInfo12" runat="server" forecolor="RoyalBlue">Nit Cliente Principal
                    :</asp:Label>&nbsp;<asp:Label id="nitPrincipal" runat="server"></asp:Label></td>
				<td colspan="2">
					<asp:Label id="lbInfo13" runat="server" forecolor="RoyalBlue">Nombre Cliente Principal
                    :</asp:Label>&nbsp;<asp:Label id="nombrePrincipal" runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td>
					<asp:Label id="lbInfo14" runat="server" forecolor="RoyalBlue">Nit Cliente Alterno
                    :</asp:Label>&nbsp;<asp:Label id="nitAlterno" runat="server"></asp:Label></td>
				<td colspan="2">
					<asp:Label id="lbInfo16" runat="server" forecolor="RoyalBlue">Nombre Cliente Alterno
                    :</asp:Label>&nbsp;<asp:Label id="nombreAlterno" runat="server"></asp:Label></td>
			</tr>
		</tbody>
	</table>
</fieldset>
<fieldset>
	<legend>Anticipos Realizados a Pedido</legend>
	<asp:DataGrid id="dgAnticipos" runat="server" cssclass="datagrid" GridLines="Vertical" AutoGenerateColumns="False">
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="PREFIJOANTICIPO" HeaderText="PREFIJO ANTICIPO"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMEROANTICIPO" HeaderText="N&#218;MERO ANTICIPO"></asp:BoundColumn>
			<asp:BoundColumn DataField="FECHAANTICIPO" HeaderText="FECHA ANTICIPO"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALORANTICIPO" HeaderText="VALOR DEL ANTICIPO" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="TIPODOCUMENTO" HeaderText="TIPO DOCUMENTO"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>
</fieldset>
<fieldset>
	<legend>Información Retoma</legend>
	<asp:DataGrid id="dgRetoma" runat="server" GridLines="Vertical"
		AutoGenerateColumns="False" cssclass="datagrid">
		<FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="TIPOVEHICULO" HeaderText="TIPO VEHICULO RETOMA"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMEROCONTRATO" HeaderText="NUMERO CONTRATO RETOMA"></asp:BoundColumn>
			<asp:BoundColumn DataField="ANOMODELO" HeaderText="A&#209;O DE MODELO"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMEROPLACA" HeaderText="NUMERO DE PLACA"></asp:BoundColumn>
			<asp:BoundColumn DataField="CUENTAIMPUESTOS" HeaderText="CUENTA DE IMPUESTOS EN"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALORRECIBIDO" HeaderText="VALOR RECIBIDO" DataFormatString="{0:C}"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>
</fieldset>
<p>
	<asp:LinkButton id="lnkVolver" onclick="VolverOrigen" runat="server">Volver</asp:LinkButton>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
<p>
</p>
