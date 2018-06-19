<%@ Control Language="c#" codebehind="AMS.Inventarios.ConsPedidos.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ConsultaPedido" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript">
</script>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
				<img height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
			<td>
				<table class="filtersIn">
					<tbody>
						<tr>
                       
							<td>
                            <p>
								Nit del&nbsp;
                                
								<asp:Label id="lbTipCli" runat="server"></asp:Label>:
                                <br />
								<asp:TextBox id="tbNit" class="tpequeno" runat="server"></asp:TextBox>
                                <br />
							<br/>
								<asp:Button id="btnCargar" onclick="CargarPedidos" runat="server" Text="Cargar Pedidos "></asp:Button>
                                </p>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label id="lbPedidos" runat="server" visible="False">Pedidos Relacionados :</asp:Label>&nbsp;<asp:DropDownList id="ddlPedidos" runat="server" Visible="False"></asp:DropDownList>
							</td>
							<td>
								<asp:Button id="btnListar" onclick="MostrarPedido" runat="server" Text="Mostrar Estado Items"
									Visible="False"></asp:Button>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:CheckBox id="chkItemPendi" runat="server" Text="Mostrar Unicamente Items Pendientes" Visible="False"
									TextAlign="Left"></asp:CheckBox>
							</td>
						</tr>
					</tbody>
				</table>
			</td>
		</tr>
	</tbody>
</table>
</fieldset>
<p>

	<asp:PlaceHolder id="plInfoPedido" runat="server"></asp:PlaceHolder>
	<asp:DataGrid id="dgPedido" runat="server" cssclass="datagrid" AutoGenerateColumns="False" GridLines="Vertical" >
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="REFERENCIA" HeaderText="REFERENCIA"></asp:BoundColumn>
			<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCION"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDAD PEDIDA" HeaderText="CANTIDAD PEDIDA"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDAD ASIGNADA" HeaderText="CANTIDAD ASIGNADA"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDAD FACTURADA" HeaderText="CANTIDAD FACTURADA"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDAD PENDIENTE" HeaderText="CANTIDAD PENDIENTE"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR UNITARIO" HeaderText="VALOR UNITARIO" DataFormatString="{0:c}"></asp:BoundColumn>
			<asp:BoundColumn DataField="DESCTO" HeaderText="% DESCTO"></asp:BoundColumn>
			<asp:BoundColumn DataField="IVA" HeaderText="% IVA"></asp:BoundColumn>
            <asp:BoundColumn DataField="SALDO BODEGAS" HeaderText="SALDO BODEGAS"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
