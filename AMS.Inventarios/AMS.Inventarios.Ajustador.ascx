<%@ Control Language="c#" codebehind="AMS.Inventarios.Ajustador.ascx.cs" autoeventwireup="false" Inherits="AMS.Inventarios.Ajustador" %>
<script language="javascript" src="../js/AMS.Inventarios.Ajustador.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<p>
    <table id="Table1" class="filtersIn" >
        <tbody>
            <tr>
                <td>
                    <fieldset >
                        <p>
                        </p>
                        <p>
                        </p>
                        <legend>Ajuste</legend>
                        <p>
                            <asp:Label id="lbHead"  runat="server"></asp:Label>
                        </p>
                    </fieldset>
                </td>
                <td>
                    <fieldset >
                        <legend>Razón</legend>
                        <asp:TextBox id="detail" runat="server" Height="57px" TextMode="MultiLine" Width="327px"></asp:TextBox>
                    </fieldset>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>
    <ASP:DataGrid id="dgInserts" runat="server" cssclass="datagrid" OnCancelCommand="DgInserts_Cancel" OnUpdateCommand="DgInserts_Update" OnEditCommand="DgInserts_Edit" OnItemCommand="DgInserts_AddAndDel" OnItemDataBound="DgInserts_ItemDataBound" CellPadding="3" ShowFooter="True" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" AutoGenerateColumns="false">
        <FooterStyle cssclass="footer"></FooterStyle>
        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
        <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
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
                    <asp:RegularExpressionValidator id="RegularExpressionValidator1" ASPClass="RegularExpressionValidator" ControlToValidate="edit_1" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert1" clientId="valToInsert1" onclick="PutDatas(this, 'bodega')" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Item">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "item", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_2" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "item") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator2" ASPClass="RegularExpressionValidator" ControlToValidate="edit_2" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert2" onclick="ModalDialog(this, 'mitems,mite_codigo,mite_nombre,puni_codigo', new Array('valToInsert3','valToInsert4'))" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descripción">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "descripcion", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_3" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "descripcion") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator3" ASPClass="RegularExpressionValidator" ControlToValidate="edit_3" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert3" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="U. Medida">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "umedida", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_4" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "umedida") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator4" ASPClass="RegularExpressionValidator" ControlToValidate="edit_4" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert4" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Cantidad">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "cantidad", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_5" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "cantidad") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator5" ASPClass="RegularExpressionValidator" ControlToValidate="edit_5" ValidationExpression="[0-9,\-]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert5" runat="server" Width="100px"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator5" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert5" ValidationExpression="[0-9\-]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Costo Promedio">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "costoPromedio", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_6" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "costoPromedio") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator6" ASPClass="RegularExpressionValidator" ControlToValidate="edit_6" ValidationExpression="[0-9\-]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox id="valToInsert6" runat="server" Width="100px"></asp:TextBox>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator6" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert6" ValidationExpression="[0-9\-]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Total">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "total", "{0:N}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="edit_7" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "total") %>' />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator7" ASPClass="RegularExpressionValidator" ControlToValidate="edit_7" ValidationExpression="[0-9]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">
                    *
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate></FooterTemplate>
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
<p>
    <table >
        <tbody>
            <tr>
                <td>
                    <fieldset >
                        <p>
                        </p>
                        <p>
                        </p>
                        <legend>Bodega
                        </legend>
                        <p>
                            &nbsp;&nbsp;
                            <asp:ListBox id="bodega" runat="server" Height="60px" Width="224px"></asp:ListBox>
                        </p>
                    </fieldset>
                </td>
                <td>
                    <fieldset >
                        <legend>Costo Promedio (msaldoitem)</legend>
                        <p>
                            &nbsp;
                            <asp:ListBox id="costoProm" runat="server" Height="56px" Width="223px"></asp:ListBox>
                        </p>
                    </fieldset>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>

</p>
