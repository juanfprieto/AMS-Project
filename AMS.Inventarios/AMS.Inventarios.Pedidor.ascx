<%@ Control Language="c#" codebehind="AMS.Inventarios.Pedidor.ascx.cs" autoeventwireup="false" Inherits="AMS.Inventarios.Pedidor" %>
<script language="javascript" src="../js/AMS.Inventarios.Pedidor.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<p>
    <table id="comp">
        <tbody>
            <tr>
                <td>
                    <fieldset>
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
                        <legend>NIT</legend>
                        <asp:TextBox id="tbNit" onclick="ModalDialog(this, 'SELECT mnit_nit as NIT,mnit_nombres as NOMBRES FROM mnit', new Array())" runat="server"></asp:TextBox>
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
            <asp:TemplateColumn HeaderText="Item">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "item", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit1" onclick="ModalDialog(this, 'SELECT mitems.mite_codigo as CODIGO, mitems.plin_codigo as BODEGA, mitems.mite_nombre as NOMBRE, mitems.puni_codigo as UNIDAD, msaldoitem.msal_cantactual-msaldoitem.msal_cantasig as DISP_EMPRESA, msaldoitemalmacen.msal_cantactual-msaldoitemalmacen.msal_cantasig as DISP_ALMACEN, mprecioitem.mpre_precio as PRECIO FROM mitems,msaldoitem,msaldoitemalmacen,mprecioitem WHERE mitems.mite_codigo=msaldoitem.mite_codigo AND mitems.mite_codigo=msaldoitemalmacen.mite_codigo AND mitems.mite_codigo=mprecioitem.mite_codigo', new Array('edit2', 'edit3', 'edit4', '#tbCantidad', '#tbCAlmacen', 'edit7'))" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "item") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert1" onclick="ModalDialog(this, 'SELECT mitems.mite_codigo as CODIGO, mitems.plin_codigo as BODEGA, mitems.mite_nombre as NOMBRE, mitems.puni_codigo as UNIDAD, msaldoitem.msal_cantactual-msaldoitem.msal_cantasig as DISP_EMPRESA, msaldoitemalmacen.msal_cantactual-msaldoitemalmacen.msal_cantasig as DISP_ALMACEN, mprecioitem.mpre_precio as PRECIO, mitems.piva_porciva as IVA FROM mitems,msaldoitem,msaldoitemalmacen,mprecioitem WHERE mitems.mite_codigo=msaldoitem.mite_codigo AND mitems.mite_codigo=msaldoitemalmacen.mite_codigo AND mitems.mite_codigo=mprecioitem.mite_codigo', new Array('valtoInsert2', 'valToInsert3', 'valToInsert4', '#tbCantidad', '#tbCAlmacen', 'valToInsert7', 'valToInsert9'))" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Bodega">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "bodega", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit2" cssclass="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "bodega") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert2" ReadOnLy="true" runat="server" cssclass="tpequeno"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descripción">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "descripcion", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit3" ReadOnLy="true" cssclass="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "descripcion") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert3" ReadOnLy="true" runat="server" cssclass="tpequeno"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="U. Medida">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "umedida", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit4" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "umedida") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert4" ReadOnLy="true" runat="server" cssclass="tpequeno"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cantidad Pedida">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "cantidadPedida", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit5" cssclass="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "cantidadPedida") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator5" ASPClass="RegularExpressionValidator" ControlToValidate="edit5" ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert5" runat="server" cssclass="tpequeno"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator6" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert5" ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cantidad Asignada">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "cantidadAsignada", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit6" cssclass="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "cantidadAsignada") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator7" ASPClass="RegularExpressionValidator" ControlToValidate="edit6" ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert6" onblur="CheckValue(this)" runat="server" cssclass="tpequeno"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator8" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert6" ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Precio Unitario">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "precio", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit7" cssclass="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "precio") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator9" ASPClass="RegularExpressionValidator" ControlToValidate="edit7" ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert7" runat="server" cssclass="tpequeno"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator9" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert7" ValidationExpression="[0-9,\-\.]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descuento">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "descuento", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_8" cssclass="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "descuento") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator10" ASPClass="RegularExpressionValidator" ControlToValidate="edit_8" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert8" runat="server" cssclass="tpequeno"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator10" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert8" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="IVA">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "iva", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_9" cssclass="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "iva") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator11" ASPClass="RegularExpressionValidator" ControlToValidate="edit_9" ValidationExpression="^[0-9]{2}$" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert9" runat="server" cssclass="tpequeno"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator11" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert9" ValidationExpression="^[0-9]{2}$" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
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
