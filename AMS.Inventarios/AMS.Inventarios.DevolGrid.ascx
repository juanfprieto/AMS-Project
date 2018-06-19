<%@ Control Language="c#" codebehind="AMS.Inventarios.DevolGrid.ascx.cs" autoeventwireup="false" Inherits="AMS.Inventarios.DevolGrid" %>
<script language="javascript" src="../js/AMS.Inventarios.ModificadorLE.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<p>
    <table id="Table" class="filtersIn">
        <tbody>
            <tr>
                <td>
                    <fieldset>
                        <p>
                        </p>
                        <p>
                        </p>
                        <legend>Devolución</legend>
                        <br />
                        <asp:Label id="lbTitle" runat="server"></asp:Label>
                    </fieldset>
                </td>
                <td>
                    <fieldset>
                        <legend>NIT</legend>
                        <asp:TextBox id="tbNit" runat="server" ReadOnLy="true"></asp:TextBox>
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
    <ASP:DataGrid id="dgInserts" runat="server" AutoGenerateColumns="false" ShowFooter="True" cssclass="datagrid" OnItemDataBound="DgInserts_ItemDataBound" OnEditCommand="DgInserts_Edit" OnUpdateCommand="DgInserts_Update" OnCancelCommand="DgInserts_Cancel">
        <FooterStyle cssclass="footer"></FooterStyle>
        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
        <PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
        <ItemStyle cssclass="item"></ItemStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Item">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "item", "{0:N}") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Bodega">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "bodega", "{0:N}") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descripción">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "descripcion", "{0:N}") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="U. Medida">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "umedida", "{0:N}") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cantidad Facturada">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "cantidadPedida", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit5" width="100" ReadOnLy="true" OnClick="KeepValue(this)" OnBlur="CheckValue(this)" Text='<%# DataBinder.Eval(Container.DataItem, "cantidadPedida") %>' />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cantidad Devuelta">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "cantidadDevuelta", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit6" width="100" OnClick="KeepValue(this)" OnBlur="CheckValue(this)" Text='<%# DataBinder.Eval(Container.DataItem, "cantidadDevuelta") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator7" ASPClass="RegularExpressionValidator" ControlToValidate="edit6" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Precio Unitario">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "precio", "{0:N}") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descuento">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "descuento", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit7" width="100" ReadOnLy="true" OnClick="KeepValue(this)" OnBlur="CheckValue(this)" Text='<%# DataBinder.Eval(Container.DataItem, "descuento") %>' />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="IVA">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "iva", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit8" width="100" ReadOnLy="true" OnClick="KeepValue(this)" OnBlur="CheckValue(this)" Text='<%# DataBinder.Eval(Container.DataItem, "iva") %>' />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Total Factura">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "totalFactura", "{0:N}") %>
                </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Total Devolucion">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "totalDevolucion", "{0:N}") %>
                </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Operaciones">
                <ItemTemplate>
                    <asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
        </Columns>
    </ASP:DataGrid>
</p>
<p>
    <asp:Label id="lbInfo" runat="server"></asp:Label>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button id="Button1" onclick="RecordProcess" runat="server" Text="Grabar Devolución"></asp:Button>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button id="Button2" onclick="CancelProcess" runat="server" Text="Cancelar"></asp:Button>
<p>
</p>
