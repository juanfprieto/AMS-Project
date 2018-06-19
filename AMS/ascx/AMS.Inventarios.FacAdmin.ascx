<%@ Control Language="c#" codebehind="AMS.Inventarios.FacAdmin.ascx.cs" autoeventwireup="false" Inherits="AMS.Inventarios.FacAdmin" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<table>
    <tbody>
        <tr>
            <td>
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <p>
                                    NIT:<br />
                                    <asp:TextBox id="txtNIT" style="name: txtNIT" runat="server" Width="100px" ReadOnLy="true"></asp:TextBox>
                                    <asp:TextBox id="txtNITa" style="name: txtNIT" runat="server" Width="150px" ReadOnLy="true"></asp:TextBox>
                                    <asp:TextBox id="txtAlm" style="name: txtNIT" runat="server" Width="0px" ReadOnLy="true" Height="0px"></asp:TextBox>
                                </p>
                            </td>
                            <td>
                                &nbsp; &nbsp;Responsable:<br />
                                &nbsp; &nbsp;<asp:DropDownList id="ddlVendedor" runat="server"></asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;&nbsp; Fecha:<br />
                                &nbsp; &nbsp;<asp:TextBox id="tbDate" runat="server" Width="78px" ReadOnly="True"></asp:TextBox>
                                <img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0" />
                                <table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendar.style.visibility='hidden'">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:calendar BackColor=Beige id="calDate" runat="server" OnSelectionChanged="ChangeDate" enableViewState="true"></asp:Calendar>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="center">
                                <asp:PlaceHolder id="plhFacProv" runat="server">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <p>
                                                        Prefijo:<br />
                                                        <asp:TextBox id="txtPref" runat="server" Width="100px" ReadOnLy="false"></asp:TextBox>
                                                    </p>
                                                </td>
                                                <td>
                                                    <p>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;Numero:<br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtNumFac" runat="server" Width="100px" ReadOnLy="false"></asp:TextBox>
                                                        &nbsp;&nbsp; &nbsp;
                                                    </p>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </asp:PlaceHolder>
                            </td>
                            <td>
                                &nbsp; &nbsp;Sede:<br />
                                &nbsp; &nbsp;<asp:DropDownList id="ddlAlmacen" runat="server"></asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;&nbsp; Centro Costo:<br />
                                &nbsp; &nbsp;<asp:DropDownList id="ddlCentro" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p align="left">
                                    Cod. Documento:<br />
                                    <asp:DropDownList id="ddlCodDoc" runat="server" AutoPostBack="True"></asp:DropDownList>
                                    <br />
                                    Num. Documento:<br />
                                    <asp:Label id="lblNumDoc" runat="server">0</asp:Label>
                                </p>
                            </td>
                            <td valign="top">
                                <p align="left">
                                    &nbsp; Fecha Limite:<br />
                                    &nbsp; &nbsp;<asp:TextBox id="tbDateV" runat="server" Width="78px" ReadOnly="True"></asp:TextBox>
                                    <img onmouseover="calendar1.style.visibility='visible'" onmouseout="calendar1.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0" />
                                    <table id="calendar1" onmouseover="calendar1.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendar1.style.visibility='hidden'">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:calendar BackColor=Beige id="calDate1" runat="server" OnSelectionChanged="ChangeDate1" enableViewState="true"></asp:Calendar>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </p>
                            </td>
                            <td>
                                <p align="right">
                                </p>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                Detalle:<br />
                <asp:TextBox id="txtObs" style="name: txtNIT" runat="server" Width="525px" MaxLength="100" Rows="5" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </tbody>
</table>
<form name="dgFrm">
    <p>
        <ASP:DataGrid id="dgItems" runat="server" cssclass="datagrid" enableViewState="true" OnCancelCommand="DgInserts_Cancel" ShowFooter="True"   AutoGenerateColumns="false" OnUpdateCommand="DgInserts_Update" OnEditCommand="DgInserts_Edit" OnItemCommand="DgInserts_AddAndDel">
            <FooterStyle cssclass="footer"></FooterStyle>
            <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
            <PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
            <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
            <ItemStyle cssclass="item"></ItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Factura:">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "mite_pfact", "{0:N}") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox id="valToInsert1" onclick="ModalDialog(this, 'SELECT pfac_codigo as CODIGO, pfac_detalle as DETALLE FROM pfactura ORDER BY pfac_codigo', new Array('#tbBase','valToInsert1'));" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Detalle:">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "mite_detalle", "{0:N}") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox id="valToInsert1a" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Cantidad:">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" id="edit_0" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem, "mite_cant") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "mite_cant", "{0}") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox id="valToInsertCant" runat="server" Width="60px" text="1"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Valor:">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" id="edit_1" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem, "mite_precio") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "mite_precio", "{0}") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox id="valToInsertVal" runat="server" Width="60px" text="0"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="% IVA:">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" id="edit_2" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem, "mite_iva") %>' />
                        %
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "mite_iva", "{0:N}%") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <p align="right">
                            $<asp:TextBox id="valToInsertIvaV" runat="server" Width="60px" text=""></asp:TextBox>
                            <br />
                            %<asp:TextBox id="valToInsertIvaP" runat="server" Width="60px" text=""></asp:TextBox>
                        </p>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Total:">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "mite_tot", "{0:N}") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Operaciones:">
                    <ItemTemplate>
                        <asp:Button CommandName="DelDatasRow" Text="Quitar" ID="btnDel" Runat="server" width="80px" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" width="80px" />
                        <br />
                        <asp:Button CommandName="ClearRows" Text="Reiniciar" ID="btnClear" Runat="server" width="80px" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
            </Columns>
        </ASP:DataGrid>
    </p>
    <p>
    </p>
    <p>
        <table>
            <tbody>
                <tr>
                    <td>
                        <p>
                            Subtotal:
                        </p>
                    </td>
                    <td>
                        <asp:TextBox id="txtSubTotal" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            IVA:
                        </p>
                    </td>
                    <td>
                        <asp:TextBox id="txtIVA" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            Total:
                        </p>
                    </td>
                    <td>
                        <asp:TextBox id="txtTotal" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </p>
    <p>
        <asp:Button id="btnAjus" onclick="NewAjust" runat="server" Text="Facturar" Enabled="False"></asp:Button>
        &nbsp;&nbsp;
    </p>
    <p>
        <asp:Label id="lbInfo" runat="server"></asp:Label>
    </p>
</form>
