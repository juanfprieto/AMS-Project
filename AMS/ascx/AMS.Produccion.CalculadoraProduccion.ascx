<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Produccion.CalculadoraProduccion.ascx.cs" Inherits="AMS.Produccion.CalculadoraProduccion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:ScriptManager ID="ScriptManager" runat="server" />
<link href="../css/AjaxTabControl.css" type="text/css" rel="stylesheet">

<br />
<ajaxToolkit:TabContainer id="tabContainer" runat="server" 
                          activetabindex="0" 
                          AutoPostBack="false"
                          CSSClass="ajax__tab_darkblue-theme">
       
    <ajaxToolkit:TabPanel ID="pnlCalc" runat="server" 
                           HeaderText="Calculadora">
        <ContentTemplate>
            <table>
                <tbody>
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Items:</legend>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlEnsambles" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnAgregarEnsamble" runat="server" Text="Agregar Item" OnClick="btnAgregarEnsamble_Click">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlLotes" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnAgregarLote" runat="server" Text="Agregar Programa" OnClick="btnAgregarLote_Click">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    </tbody>
            </table>
            <asp:DataGrid ID="dgEnsambles" runat="server" Visible="False" FooterStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center" AlternatingItemStyle-HorizontalAlign="Center"
                AutoGenerateColumns="False" CssClass="datagrid" OnItemCommand="dgEnsambles_ItemCommand">
                <FooterStyle CssClass="footer"></FooterStyle>
				<HeaderStyle CssClass="header"></HeaderStyle>
				<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
				<ItemStyle CssClass="item"></ItemStyle>
                <Columns>
                    <asp:TemplateColumn HeaderText="Item">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ITEM") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Nombre">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem,"NOMBRE") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Cantidad">
                        <ItemTemplate>
                            <asp:TextBox ID="txtCantidad" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CANTIDAD") %>' />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Eliminar">
                        <ItemTemplate>
                            <asp:Button ID="btnDel" CommandName="del" Text="Remover" runat="server"></asp:Button>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnCalc" OnClick="btnCalc_Click" Text="Calcular" runat="server"></asp:Button>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkRecursivo" runat="server" checked="false" Text="Mostrar sub-ensambles"></asp:CheckBox>
                    </td>
                </tr>
            </table>
            <br />
                 
            <asp:DataGrid ID="dgItems" runat="server" Visible="False" FooterStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center" AlternatingItemStyle-HorizontalAlign="Center"
                AutoGenerateColumns="False" CssClass="datagrid" OnItemDataBound="dgItems_OnItemDataBound">
                <FooterStyle CssClass="footer"></FooterStyle>
				    <HeaderStyle CssClass="header"></HeaderStyle>
				    <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
				    <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
				    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
				    <ItemStyle CssClass="item"></ItemStyle>
                <Columns>
                    <asp:TemplateColumn HeaderText="Padre - Item" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ITEM") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Cantidad Necesaria">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem,"CANTIDAD", "{0:N}") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Bodega">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ALMACEN") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Disponibilidad">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "DISPONIBLE", "{0:N}") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Intercambiables">
                        <ItemTemplate>
                            <asp:Image ID="imgIntercam" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel ID="pnlOrd" runat="server" HeaderText="Orden de Produccion">
        <ContentTemplate>
            <p>
                <asp:PlaceHolder ID="creacionOrden" runat="server"></asp:PlaceHolder>
            </p>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>
