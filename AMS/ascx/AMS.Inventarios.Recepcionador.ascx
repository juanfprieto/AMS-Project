<%@ Control Language="c#" codebehind="AMS.Inventarios.Recepcionador.ascx.cs" autoeventwireup="false" Inherits="AMS.Inventarios.Recepcionador" %>
<script language="javascript" src="../js/AMS.Inventarios.Recepcionador.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<p>
    <table id="comp">
        <tbody>
            <tr>
                <td>
                    <fieldset >
                        <p>
                        </p>
                        <p>
                        </p>
                        <legend>Pedido</legend>
                        <br />
                        <asp:Label id="lbTitle" runat="server"></asp:Label>
                    </fieldset>
                </td>
                <td>
                    <fieldset>

                        <legend>Observacion</legend>
                        <asp:TextBox id="tbDetail" runat="server"></asp:TextBox>
                    </fieldset>
                </td>
                <td>
                    <fieldset>
                        <legend>Fecha</legend>
                        <p>
                            <asp:Label id="lbDate" runat="server"></asp:Label>
                        </p>
                    </fieldset>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>
    <ASP:DataGrid id="dgInserts" runat="server" cssclass="datagrid" OnCancelCommand="DgInserts_Cancel" OnUpdateCommand="DgInserts_Update" OnEditCommand="DgInserts_Edit" OnItemCommand="DgInserts_AddAndDel" OnItemDataBound="DgInserts_ItemDataBound" CellPadding="3" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="false">
        <FooterStyle cssclass="footer"></FooterStyle>
        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
        <PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
        <ItemStyle cssclass="item"></ItemStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Pref. Doc.">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "prefDocumento", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_1" width="50" Text='<%# DataBinder.Eval(Container.DataItem, "prefDocumento") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert1" ReadOnLy="true" runat="server" Width="50px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Num. Doc.">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "numDocumento", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_2" width="50" Text='<%# DataBinder.Eval(Container.DataItem, "numDocumento") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert2" ReadOnLy="true" runat="server" Width="50px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Item">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "item", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_3" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "item") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert3" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Bodega">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "bodega", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_4" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "bodega") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert4" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descripción">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "descripcion", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_5" ReadOnLy="true" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "descripcion") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert5" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="U. Medida">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "umedida", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_6" width="50" Text='<%# DataBinder.Eval(Container.DataItem, "umedida") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert6" ReadOnLy="true" runat="server" Width="50px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cantidad Pedida">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "cantidadPedida", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_7" width="50" Text='<%# DataBinder.Eval(Container.DataItem, "cantidadPedida") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator5" ASPClass="RegularExpressionValidator" ControlToValidate="edit_7" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert7" runat="server" Width="50px"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator6" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert7" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cantidad Entregada">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "cantidadEntregada", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_8" width="50" Text='<%# DataBinder.Eval(Container.DataItem, "cantidadEntregada") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator7" ASPClass="RegularExpressionValidator" ControlToValidate="edit_8" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert8" onblur="CheckValue(this)" runat="server" Width="50px"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator8" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert8" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Precio Unitario">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "precio", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_9" width="70" Text='<%# DataBinder.Eval(Container.DataItem, "precio") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator9" ASPClass="RegularExpressionValidator" ControlToValidate="edit_9" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert9" runat="server" Width="70px"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator9" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert9" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descuento">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "descuento", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_10" width="50" Text='<%# DataBinder.Eval(Container.DataItem, "descuento") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator10" ASPClass="RegularExpressionValidator" ControlToValidate="edit_10" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert10" runat="server" Width="50px"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator10" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert10" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="IVA">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "iva", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_11" width="50" Text='<%# DataBinder.Eval(Container.DataItem, "iva") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator11" ASPClass="RegularExpressionValidator" ControlToValidate="edit_11" ValidationExpression="^[0-9]{2}\.[0-9]{1}$" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert11" runat="server" Width="50px"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator11" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert11" ValidationExpression="^[0-9]{2}\.[0-9]{1}$" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Total">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "total", "{0:N}") %>
                </ItemTemplate>


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
    <asp:Label id="lbInfo" runat="server"></asp:Label>
</p>
</p>
<p>
    <fieldset>
<br>
    &nbsp;&nbsp;Disponible<input type="text" name="tbCantidad" size="8">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Disponible almacen:
    <input type="text" name="tbCAlmacen" size="8">

    </fieldset>
</p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button id="Button1" onclick="RecordProcess" runat="server" Text="Grabar"></asp:Button>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button id="Button2" onclick="CancelProcess" runat="server" Text="Cancelar"></asp:Button>
<p>
