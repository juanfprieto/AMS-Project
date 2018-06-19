<%@ Control Language="c#" Debug="true" codebehind="AMS.Inventarios.Facturador.ascx.cs" autoeventwireup="false" Inherits="AMS.Inventarios.Facturador" %>
<script language="javascript" src="../js/AMS.Inventarios.Facturador.js" type="text/javascript"></script>
<p>
	<table id="comp" >
		<tbody>
			<tr>
				<td>
					<fieldset>
						<p>
						</p>
						<p>
						</p>
						<legend>Factura</legend>
						<p>
						</p>
					</fieldset>
				</td>
				<td>
					<fieldset >
						<legend>Razón</legend>
						<asp:TextBox id="detail" TextMode="MultiLine" Height="57px" runat="server" Width="385px"></asp:TextBox>
					</fieldset>
				</td>
				<td>
					<fieldset>
						<legend>Fecha</legend>
						<p>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</p>
					</fieldset>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>
	<ASP:DataGrid id="dgInserts" runat="server" cssclass="datagrid" AutoGenerateColumns="false"	BorderWidth="1px" GridLines="Vertical" 
		ShowFooter="True" CellPadding="3" OnItemDataBound="DgInserts_ItemDataBound" OnItemCommand="DgInserts_AddAndDel" OnEditCommand="DgInserts_Edit"
		OnUpdateCommand="DgInserts_Update" OnCancelCommand="DgInserts_Cancel">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Bodega">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "bodega", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_1" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "bodega") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator1" ASPClass="RegularExpressionValidator" ControlToValidate="edit_1"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1" onclick="PutDatas(this, 'bodega')" ReadOnLy="true" runat="server"
						Width="100px"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Item">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "item", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_2" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "item") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator2" ASPClass="RegularExpressionValidator" ControlToValidate="edit_2"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert2" onBlur="PutDatas(this, 'item')" ReadOnLy="true" runat="server"
						Width="100px"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Descripción">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "descripcion", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_3" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "descripcion") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator3" ASPClass="RegularExpressionValidator" ControlToValidate="edit_3"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert3" onclick="PutDatas(this, 'descripcion')" ReadOnLy="true" runat="server"
						Width="100px"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="U. Medida">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "umedida", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_4" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "umedida") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator4" ASPClass="RegularExpressionValidator" ControlToValidate="edit_4"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert4" onclick="PutDatas(this, 'umedida')" ReadOnLy="true" runat="server"
						Width="100px"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "cantidad", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_5" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "cantidad") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator5" ASPClass="RegularExpressionValidator" ControlToValidate="edit_5"
						ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert5" runat="server" Width="100px"></asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator5" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert5"
						ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Unitario">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "valorUnitario", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_6" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "valorUnitario") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator6" ASPClass="RegularExpressionValidator" ControlToValidate="edit_6"
						ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert6" runat="server" Width="100px"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="% Descuento">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "descuento", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_7" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "descuento") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator6" ASPClass="RegularExpressionValidator" ControlToValidate="edit_7"
						ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert7" runat="server" Width="100px"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="% IVA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "iva", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_8" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "IVA") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator6" ASPClass="RegularExpressionValidator" ControlToValidate="edit_8"
						ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Total">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "total", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_9" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "total") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator7" ASPClass="RegularExpressionValidator" ControlToValidate="edit_9"
						ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
				</EditItemTemplate>
				<FooterTemplate>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones">
				<ItemTemplate>
					<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
		</Columns>
	</ASP:DataGrid>
</p>
<p>
	<asp:Button id="Button1" runat="server" Text="Devolver"></asp:Button>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button id="Button2" runat="server" Text="Anular"></asp:Button>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button id="Button3" runat="server" Text="Grabar Factura"></asp:Button>
</p>
<p>
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p>
<p>
	<table>
		<tbody>
			<tr>
				<td>
					<fieldset>
						<p>
						</p>
						<p>
						</p>
						<legend>Bodega(plineaitem)
                        </legend>
						<p>
							&nbsp;&nbsp;
							<asp:ListBox id="accounts" Height="60px" runat="server" Width="224px"></asp:ListBox>
						</p>
					</fieldset>
				</td>
				<td>
					<fieldset>
						<legend>Items(mitems)</legend>
						<asp:ListBox id="nits" runat="server" Width="240px"></asp:ListBox>
					</fieldset>
				</td>
				<td>
					<fieldset>
						<legend>Costo Promedio (msaldoitem)</legend>
						<p>
							&nbsp;
							<asp:ListBox id="centros" Height="56px" runat="server" Width="223px"></asp:ListBox>
						</p>
					</fieldset>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>
</p>
