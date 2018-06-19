<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.DevolucionCC.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.DevolucionChequesCC" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				
			</td>
			<td>
				<asp:DropDownList id="prefijoConsignacion" runat="server" AutoPostBack="True" onSelectedIndexChanged="Cambiar_PrefijoConsignacion"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				
			</td>
			<td>
				<asp:DropDownList id="numeroConsignacion" runat="server"></asp:DropDownList>
			</td>
		</tr>
        <tr>
            <td>
                Cuenta corriente:
            </td>
            <td>
                <asp:DropDownList ID="ddlCuentaCorriente" runat="server"></asp:DropDownList>
            </td>
        </tr>
		<tr>
			<td>
				Fecha devolución:
			</td>
			<td>
				<img onmouseover="calendario.style.visibility='visible'" onmouseout="calendario.style.visibility='hidden'"
					src="../img/AMS.Icon.Calendar.gif" border="0">
				<table id="calendario" onmouseover="calendario.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
					onmouseout="calendario.style.visibility='hidden'">
					<tbody>
						<tr>
							<td>
								<asp:calendar BackColor=Beige id="calendarioFecha" runat="server" OnSelectionChanged="Cambiar_Fecha"></asp:Calendar>
							</td>
						</tr>
					</tbody>
				</table>
				<asp:TextBox id="fecha" runat="server" Width="92px" onkeyup="DateMask(this)"></asp:TextBox>
				<asp:RegularExpressionValidator id="validatorFecha" runat="server" ErrorMessage="Formato de Fecha Invalido" ControlToValidate="fecha"
					ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:RegularExpressionValidator>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Button id="aceptarDatos" onclick="Aceptar_Datos" runat="server" Text="Aceptar"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
<p>
</p>
<asp:DataGrid id="gridDatos" runat="server" Visible="True" AutoGenerateColumns="False" CellPadding="3" onItemCommand="Manejar_Documentos" >
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
	<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Prefijo Recibo de Caja">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "PREFIJORECIBOCAJA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Número Recibo de Caja">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMERORECIBOCAJA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Tipo de Pago">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "TIPOPAGO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Número del Documento">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Banco">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NOMBREBANCO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Valor">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Nit Beneficiario">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NITBENEFICIARIO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Fecha">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Prefijo Factura de Devolución">
			<ItemTemplate>
				<asp:DropDownList id="prefijoDevolucion" runat="server" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Devolver">
			<ItemTemplate>
				<center>
					<asp:Button id="btnDevolver" runat="server" Text="Devolver" CommandName="Devolver_Documento" />
				</center>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
