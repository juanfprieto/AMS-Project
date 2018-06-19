<%@ Control Language="C#" CodeBehind="AMS.Asousados.AsociacionVendedores.ascx.cs" AutoEventWireup="true" Inherits="AMS.Asousados.AsociacionVendedores" %>
<fieldset>
<table id="Table" class="filtersIn">
    <tbody>

        <tr>
            <td>
                Asociado:
            </td>
            <td>
                <asp:dropdownlist id="ddlAsociado" cssclass="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAsociado_SelectedIndexChanged">
                </asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td>
                Desea asignar vendedores automaticamente?
            </td>
            <td>
                <asp:Button ID="btnAsignarVends" runat="server" Text="Asignar" OnClick="btnAsignarVends_Click" Enabled="false"/>
                <asp:Button ID="btnDesasignarVends" runat="server" Text="Desasignar" OnClick="btnDesasignarVends_Click" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lblMensajeAsignacion" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Sede:
            </td>
            <td>
                <asp:DropDownList id="ddlSede" cssclass="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSede_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="scrollable">
                <asp:DataGrid ID="dgVendedores" runat="server" CssClass="datagrid" 
                showfooter="true" AutoGenerateColumns="false" DataKeyField="CODIGO"
                OnItemDataBound="dgVendedores_Bound"
                OnItemCommand="dgVendedores_ItemCommand" 
                OnUpdateCommand="dgVendedores_Update"
				OnEditCommand="dgVendedores_Edit"
                OnCancelCommand="dgVendedores_Cancel">
                    <FooterStyle cssclass="footer"></FooterStyle>
	                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	                <ItemStyle cssclass="item"></ItemStyle>
	                <HeaderStyle cssclass="header"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="Nombre">
                            <ItemTemplate>
				                <%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
			                </ItemTemplate>
							<EditItemTemplate>
								<asp:TextBox runat="server" id="txtNomEdit" Text='<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>' />
							</EditItemTemplate>
			                <FooterTemplate>
					            <asp:TextBox id="txtNom" class="tmediano" runat="server"></asp:TextBox>
			                </FooterTemplate>
		                </asp:TemplateColumn>
                        <asp:BoundColumn HeaderText="Id Ubicación" Visible="false" DataField="IDUBICACION">
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Ubicación">
                            <ItemTemplate>
				                <%# DataBinder.Eval(Container.DataItem, "UBICACION") %>
			                </ItemTemplate>
							<EditItemTemplate>
								<asp:DropDownList runat="server" id="ddlUbicacionEdit" />
							</EditItemTemplate>
			                <FooterTemplate>
					           <asp:DropDownList id="ddlUbicacion" class="dmediano" runat="server"></asp:DropDownList>
			                </FooterTemplate>
		                </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Teléfono">
                            <ItemTemplate>
				                <%# DataBinder.Eval(Container.DataItem, "TELEFONO") %>
			                </ItemTemplate>
							<EditItemTemplate>
								<asp:TextBox runat="server" id="txtTelEdit" Text='<%# DataBinder.Eval(Container.DataItem, "TELEFONO") %>' />
							</EditItemTemplate>
			                <FooterTemplate>
					                <asp:TextBox id="txtTel" class="tmediano" runat="server"></asp:TextBox>
			                </FooterTemplate>
		                </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="e-Mail">
                            <ItemTemplate>
				                <%# DataBinder.Eval(Container.DataItem, "MAIL") %>
			                </ItemTemplate>
							<EditItemTemplate>
								<asp:TextBox runat="server" id="txtEmailEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MAIL") %>' />
							</EditItemTemplate>
			                <FooterTemplate>
					                <asp:TextBox id="txtEmail" class="tmediano" runat="server"></asp:TextBox>
			                </FooterTemplate>
		                </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="fotografia">
                            <ItemTemplate>
                                <asp:Image runat="server" ID="imgVend" Width="50px" Height="50px" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "FOTO") %>'/>
			                </ItemTemplate>
							<EditItemTemplate>
                                <asp:FileUpload id="fuVendEdit" runat="server"></asp:FileUpload>
							</EditItemTemplate>
			                <FooterTemplate>
                                <asp:FileUpload id="fuVend" runat="server"></asp:FileUpload>
			                </FooterTemplate>
		                </asp:TemplateColumn>
			            <asp:TemplateColumn HeaderText="">
				            <ItemTemplate>
					            <asp:Button CommandName="remover" Text="Remover" ID="btnEliminar" CausesValidation="False" runat="server" />
				            </ItemTemplate>
				            <FooterTemplate>
					            <asp:Button CommandName="agregar" Text="Agregar" ID="btnAgregar" runat="server" />
				            </FooterTemplate>
			            </asp:TemplateColumn>
						<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar">
                        </asp:EditCommandColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </tbody>
</table>
</fieldset>
