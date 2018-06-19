<%@ Control Language="c#" codebehind="AMS.Vehiculos.CapPedPro.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.CadPedPro" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<fieldset>
<TABLE id="Tale1" class="filtersIn">
	<tbody>
		<tr>
			<td>
                <fieldset> 
					<legend>Comprobante</legend>

					<p>&nbsp;<asp:label id="tipo" runat="server">Tipo :</asp:label>&nbsp;
						<asp:label id="tipoDocu" runat="server"></asp:label>
                    </p>
					<p>Número:
						<asp:textbox id="idPedido" runat="server" class="tpequeno"></asp:textbox>
                    </p>
				</fieldset>
			</td>
            <td>
			    <fieldset>
					<legend>Observación</legend>
					<p><asp:textbox id="observacion" runat="server" class="tgrande"></asp:textbox>
                    </p>
				</fieldset>
			</td>
        </tr>
		<tr>
			<td>
				<fieldset>
                   <p></p>
                      <legend>Proveedor</legend>
                      Nit Proveedor : <asp:textbox id="nitProveedor" onclick="ModalDialog(this, 'SELECT t1.mnit_nit as NIT, t1.mnit_nombres CONCAT \' \' CONCAT t1.mnit_apellidos as NOMBRE from MNIT as t1, MPROVEEDOR as t2 WHERE t1.mnit_nit=t2.mnit_nit', new Array())"
						runat="server" ReadOnly="True"></asp:textbox>
                  <p>Nombre Proveedor :&nbsp;
	              <asp:textbox id="nitProveedora" runat="server" class="tmediano" ReadOnly="True"></asp:textbox></p>
                </fieldset>
			        </td>
                     <td>
				<fieldset>
					<legend>Fecha</legend>
					<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:label id="fechaPedido" runat="server"></asp:label></p>
				</fieldset>
			</td>
		</tr>
	</tbody>
  </table>
</fieldset>

<p>&nbsp;<asp:datagrid id="dgInserts" runat="server" cssclass="datagrid" AutoGenerateColumns="False" GridLines="Vertical" ShowFooter="True" OnItemDataBound="DgInserts_ItemDataBound" OnItemCommand="DgInserts_AddAndDel" OnEditCommand="DgInserts_Edit"
		OnUpdateCommand="DgInserts_Update" OnCancelCommand="DgInserts_Cancel">
		<FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Catalogo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem,"CATALOGO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox Runat="server" id="catalogo" onClick="ModalDialog(this, 'Select a.pcat_codigo,b.ppre_costo from DBXSCHEMA.PCATALOGOVEHICULO a,DBXSCHEMA.PPRECIOVEHICULO b where a.pcat_codigo=b.pcat_codigo', new Array())"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="catalogoEdit" runat="server" onclick="ModalDialog(this, 'SELECT pcat_codigo AS Catalogo FROM pcatalogovehiculo', new Array())" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem,"CATALOGO") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Color">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "COLOR") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="colorInsert" runat="server"></asp:DropDownList>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="colorEdit" onclick="ModalDialog(this, 'SELECT pcol_codigo AS CODIGO ,pcol_descripcion AS COLOR FROM pcolor order by pcol_descripcion', new Array())" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "COLOR") %> '>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Color Alterno">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "COLORALTER") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="colorAlterInsert" runat="server"></asp:DropDownList>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="colorAlterEdit" onclick="ModalDialog(this, 'SELECT pcol_codigo AS CODIGO ,pcol_descripcion AS COLOR FROM pcolor order by pcol_descripcion', new Array())" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "COLORALTER") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad Pedida">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CANTPED", "{0:G}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="cantidadPedidaInserts" CssClass="AlineacionDerecha" runat="server" Width="50px"></asp:TextBox>
					<asp:RegularExpressionValidator id="validator1" ASPClass="RegularExpressionValidator" ControlToValidate="cantidadPedidaInserts"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="cantidadPedidaEdit" CssClass="AlineacionDerecha" runat="server" width="50px" Text='<%# DataBinder.Eval(Container.DataItem, "CANTPED", "{0:G}") %>' />
					<asp:RegularExpressionValidator id="validator2" ASPClass="RegularExpressionValidator" ControlToValidate="cantidadPedidaEdit"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Unitario">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALUNIT", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="catalogoa" CssClass="AlineacionDerecha" runat="server" Width="100px" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
					<asp:RegularExpressionValidator id="validator3" ASPClass="RegularExpressionValidator" ControlToValidate="catalogoa"
						ValidationExpression="[0-9\,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id="valorUnitarioEdit" CssClass="AlineacionDerecha" runat="server" onkeyup="NumericMaskE(this,event)" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "VALUNIT", "{0:N}") %>'>
					</asp:TextBox>
					<asp:RegularExpressionValidator id="validator4" ASPClass="RegularExpressionValidator" ControlToValidate="valorUnitarioEdit"
						ValidationExpression="[0-9\,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Fecha Llegada">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "FECHALLEGADA", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox Runat="server" onkeyup="DateMask(this)" ID="txtFechaLlegada"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox Runat=server onkeyup="DateMask(this)" ID="txtFechaEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FECHALLEGADA", "{0:N}") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones">
				<ItemTemplate>
					<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" HeaderText="Editar Item" CancelText="Cancelar"
				EditText="Actualizar"></asp:EditCommandColumn>
		</Columns>
	</asp:datagrid>
</p>
<fieldset>
<p>Total Pedido :
	<asp:textbox id="totalPedido" runat="server" ReadOnly="True" CssClass="AlineacionDerecha">$0</asp:textbox></p>
<p><asp:button id="btnGuardar" onclick="Guardar_Pedido" runat="server" Width="73px" Enabled="False"
		Text="Guardar" Visible="False"></asp:button>&nbsp;<asp:button id="btnEditar" onclick="Editar_Pedido" runat="server" Text="Editar" Visible="False"></asp:button>
</p>
<p><asp:label id="lb" runat="server"></asp:label><asp:hyperlink id="hl1" runat="server" Visible="False" Target="_blank">Descargar Pedido</asp:hyperlink></p>
</fieldset>