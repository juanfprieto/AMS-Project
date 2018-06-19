<%@ Control Language="c#" codebehind="AMS.Inventarios.ReversarPreRecepcion.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ReversarPreRecepcion" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
    <fieldset>
	<table id="Table2" class="filtersIn">
    <legend>Datos del proceso:</legend>
		<tbody>
			<tr>
				<td>
					<p>
						<asp:Label id="Label5" runat="server" forecolor="RoyalBlue">NIT PROVEEDOR</asp:Label>
					</p>
				</td>
			</tr>
			<tr>
				
							<tr>
								<td>
									Nit :<br />
									<asp:TextBox id="tbNit" ondblclick="ModalDialog(this, 'Select t1.mnit_nit as NIT, t1.mnit_nombres concat \' \' concat t1.mnit_apellidos as Nombre from MNIT as t1,MPROVEEDOR as t2 where t1.mnit_nit=t2.mnit_nit', new Array());"
										style="name: txtNIT" runat="server" ReadOnLy="false" Width="100px"></asp:TextBox>
								</td>
                                </tr>
							<tr>
								<td>
									<asp:TextBox id="tbNita" style="name: txtNIT" runat="server" ReadOnLy="true" Width="150px"></asp:TextBox>
								</td>
							</tr>
					
				<td valign="bottom" align="right">
					<asp:Button id="btnSelecNIT" onclick="CargarNit" runat="server" Text="Confirmar"></asp:Button>
				</td>
			</tr>
			<asp:PlaceHolder id="plPreRecep" runat="server" visible="false">
				<TR>
					<TD>PreRecepciones Disponibles :</TD>
					<TD align="right">
						<asp:DropDownList id="ddlPreRecep" runat="server"></asp:DropDownList></TD>
				</TR>
				<TR>
					<TD align="right" colSpan="2">
						<asp:Button id="btnCargar" onclick="CargarPreRecepcion" runat="server" Text="Cargar"></asp:Button></TD>
				</TR>
			</asp:PlaceHolder>
		</tbody>
       
	</table>
     </fieldset>
<p>
	<ASP:DataGrid id="dgItemsRDir" runat="server" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="false" enableViewState="true">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
		<Columns>
			<asp:BoundColumn DataField="mite_codigo" ReadOnly="True" HeaderText="Item :"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_nombre" ReadOnly="True" HeaderText="Nombre :"></asp:BoundColumn>
			<asp:BoundColumn DataField="plin_codigo" ReadOnly="True" HeaderText="Linea de Bodega :"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_canting" ReadOnly="True" HeaderText="Cantidad Ingresada :" DataFormatString="{0:N}"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_precio" ReadOnly="True" HeaderText="Precio :" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_desc" ReadOnly="True" HeaderText="Descuento :" DataFormatString="{0:N}%"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_iva" ReadOnly="True" HeaderText="IVA :" DataFormatString="{0:N}%"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_tot" ReadOnly="True" HeaderText="Total :" DataFormatString="{0:C}"></asp:BoundColumn>
		    <asp:BoundColumn DataField="mite_codigo" ReadOnly="True" HeaderText="Item :"></asp:BoundColumn>
			<asp:BoundColumn DataField="palm_almacen" ReadOnly="True" HeaderText="Sede :"></asp:BoundColumn>
		    <asp:BoundColumn DataField="pped_codigo" ReadOnly="True" HeaderText="Pedido :"></asp:BoundColumn>
			<asp:BoundColumn DataField="mped_numepedi" ReadOnly="True" HeaderText="Numero :"></asp:BoundColumn>
			</Columns>
	</ASP:DataGrid>
</p>
<p>
	<asp:Button id="btnReversar" onclick="ReversarPreRecep" runat="server" Text="Reversar PreRecepción"
		Visible="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
