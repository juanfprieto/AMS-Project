<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.AnulacionReciboCaja.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.AnulacionReciboCaja" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
<table >
	<tbody>

					<legend>
						<p>
							Escoja el Recibo de Caja&nbsp;o Comprobante de Egreso a&nbsp;Anular:
						</p>
					</legend>
					<table id="Table2" class="filtersIn">
						<tr>
							<td>
								Prefijo del Recibo :
							</td>
							<td>
								<asp:DropDownList id="prefijoRecibo" runat="server" AutoPostBack="True" onSelectedIndexChanged="Escoger_Prefijo"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>
								Número del Recibo :
							</td>
							<td>
								<asp:DropDownList id="numeroRecibo" class="dpequeno" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>
								Prefijo del Documento de Anulación :
							</td>
							<td>
								<asp:DropDownList ID="ddlDocAnu" Runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<asp:Button id="cargarRecibo" onclick="Cargar_Recibo" runat="server" Text="Cargar Recibo"></asp:Button>
							</td>
						</tr>
					</table>
				<p>
				</p>

					<legend>Datos del Recibo</legend>
					<p>
					</p>
					
						<table id="Table1" class="filtersIn">
							
								<tr>
									<td>
										Fecha:
									</td>
									<td>
										<asp:Label id="fecha" runat="server"></asp:Label></td>
								</tr>
								<tr>
									<td>
										Clase de Recibo:&nbsp; &nbsp;</td>
									<td>
										<asp:Label id="claseRecibo" runat="server"></asp:Label></td>
								</tr>
								<tr>
									<td>
										Nit Cliente:
									</td>
									<td>
										<asp:Label id="nitCliente" runat="server"></asp:Label></td>
								</tr>
								<tr>
									<td>
										Nit Beneficiario:
									</td>
									<td>
										<asp:Label id="nitBeneficiario" runat="server"></asp:Label></td>
								</tr>
								<tr>
									<td>
										Concepto:
									</td>
									<td>
										<asp:Label id="concepto" runat="server"></asp:Label></td>
								</tr>
								<tr>
									<td>
										Sede:
									</td>
									<td>
										<asp:Label id="sede" runat="server"></asp:Label></td>
								</tr>
						
						</table>
					
				<p>
				</p>
				 <table id="Table3" class="filtersIn">
                 <tr>
                 <td>
                 
					<p>
					</p>
					<legend>Conceptos</legend>
					<p>
						<asp:Label id="tipoPago" runat="server"></asp:Label>
					</p>
					<asp:DataGrid id="gridPagos_Abonos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Prefijo">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="N&#250;mero Documento">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERODOCUMENTO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
					<p>
						Varios
					</p>
					<asp:DataGrid id="gridVarios" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Descripci&#243;n">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cuenta">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Sede">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "SEDE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Centro de Costo">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CENTROCOSTO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Prefijo Documento">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Numero Documento">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NIT Beneficiario">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NIT") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Naturaleza">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NATURALEZA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor Base">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORBASE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
					<p>
						Pagos
					</p>
					<asp:DataGrid id="gridPagos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Tipo">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TIPO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Banco">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODIGOBANCO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Nombre Banco">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBREBANCO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Documento">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERODOC") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Moneda">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TIPOMONEDA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Tasa Cambio">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORTC") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn Visible="False" HeaderText="Fecha">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False" HeaderText="Estado">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
					<p>
						Retenciones
					</p>
					<asp:DataGrid id="gridRetenciones" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3">
						<FooterStyle cssclass="footer"></FooterStyle>
						<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nombre de Retenci&#243;n">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMRET") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor Retenido">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
					</asp:DataGrid>
                    <p>
                        Obligaciones financieras
					</p>
					<asp:DataGrid id="gridObligaciones_financieras" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="N&#250;mero Obligaci&#250;n">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMEROOBLIGACION") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Consecutivo Pago">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "SECUENCIA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Capital Pagado">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "MONTOPAGO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Valor Interes Pagado">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORINTPAGO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
                    </td>
                    </tr>
				</table>
		<tr>
			<td>
				<asp:Button id="anular" onclick="Anular_Recibo" autopostback = "true" runat="server" Text="Anular" Enabled="False" onClientclick="espera();"></asp:Button>
				<asp:Button ID="cancelar" Runat="server" Text="Cancelar" onclick="cancelar_Click"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
</fieldset>
