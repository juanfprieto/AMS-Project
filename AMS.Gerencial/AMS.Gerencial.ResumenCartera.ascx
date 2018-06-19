<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="AMS.Gerencial.ResumenCartera.ascx.cs"
    Inherits="AMS.Gerencial.AMS_Gerencial_ResumenCartera" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
    <table id="Table" class="filtersIn">
        <tr>
            <td>
                <asp:Label ID="Label16" runat="server">Fecha:&nbsp;&nbsp;</asp:Label>
                <asp:Label ID="lblFecha" runat="server" Font-Size="10"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="Generar" OnClick="Generar_Click" runat="server" Width="112px" Text="Generar Informe">
                </asp:Button>
            </td>
        </tr>
    </table>
    <br>
    <asp:Panel ID="pnlResultados" runat="server" Visible="False">
        <table id="Table1" class="filtersIn">
            <tr>
                <td style="font-weight: bold; font-size: 10pt">
                    Clientes:
                </td>
            </tr>
            <tr>
                <td class="scrollable">
                    <asp:DataGrid ID="dgrCarteraC" runat="server" CssClass="datagrid" AutoGenerateColumns="False"
                        ShowFooter="True" CellPadding="3">
                        <FooterStyle CssClass="footer"></FooterStyle>
                        <HeaderStyle CssClass="header"></HeaderStyle>
                        <PagerStyle CssClass="pager" Mode="NumericPages"></PagerStyle>
                        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                        <ItemStyle CssClass="item"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="pdoc_codigo" HeaderText="" />
                            <asp:BoundColumn DataField="pdoc_nombre" HeaderText="Documento" FooterText="Totales"
                                FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="VALOR" HeaderText="Total" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="VENCER" HeaderText="A Vencer" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="V30" HeaderText="Venc. 30 Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="V60" HeaderText="Venc. 60 Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="V90" HeaderText="Venc. 90 Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="V120" HeaderText="Venc. 120 Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="VMAX" HeaderText="Venc. Mas Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        <br>
        <table id="Table2" class="filtersIn">
            <tr>
                <td style="font-weight: bold; font-size: 10pt">
                    Proveedores:
                </td>
            </tr>
            <tr>
                <td class="scrollable">
                    <asp:DataGrid ID="dgrCarteraP" runat="server" CssClass="datagrid" AutoGenerateColumns="False"
                        ShowFooter="True" CellPadding="3">
                        <FooterStyle CssClass="footer"></FooterStyle>
                        <HeaderStyle CssClass="header"></HeaderStyle>
                        <PagerStyle CssClass="pager" Mode="NumericPages"></PagerStyle>
                        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                        <ItemStyle CssClass="item"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="pdoc_codigo" HeaderText="" />
                            <asp:BoundColumn DataField="pdoc_nombre" HeaderText="Documento" FooterText="Totales"
                                FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="VALOR" HeaderText="Total" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="VENCER" HeaderText="A Vencer" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="V30" HeaderText="Venc. 30 Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="V60" HeaderText="Venc. 60 Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="V90" HeaderText="Venc. 90 Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="V120" HeaderText="Venc. 120 Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="VMAX" HeaderText="Venc. Mas Dias" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        <br>
        <table id="Table3" class="filtersIn">
            <tr>
                <td style="font-weight: bold; font-size: 10pt">
                    Obligaciones Financieras:
                </td>
            </tr>
            <tr>
                <td class="scrollable">
                    <asp:DataGrid ID="dgrObligaciones" runat="server" CssClass="datagrid" AutoGenerateColumns="False"
                        ShowFooter="True" CellPadding="3">
                        <FooterStyle CssClass="footer"></FooterStyle>
                        <HeaderStyle CssClass="header"></HeaderStyle>
                        <PagerStyle CssClass="pager" Mode="NumericPages"></PagerStyle>
                        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                        <ItemStyle CssClass="item"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="cuenta" HeaderText="Cuenta" FooterText="Totales" />
                            <asp:BoundColumn DataField="mobl_numero" HeaderText="Num." />
                            <asp:BoundColumn DataField="mobl_fechapago" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundColumn DataField="dobl_numepago" HeaderText="Pago" />
                            <asp:BoundColumn DataField="dobl_montpesos" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" HeaderText="Monto<br>Pago" />
                            <asp:BoundColumn DataField="dobl_montinteres" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" HeaderText="Interés<br>Pago" />
                            <asp:BoundColumn DataField="mobl_montpesos" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" HeaderText="Monto" />
                            <asp:BoundColumn DataField="mobl_montpagado" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" HeaderText="Pagado" />
                            <asp:BoundColumn DataField="mobl_interespagado" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" HeaderText="Interés<br>Pagado" />
                            <asp:BoundColumn DataField="saldo" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" HeaderText="Saldo" />
                            <asp:BoundColumn DataField="pcre_nombre" HeaderText="Crédito" />
                            <asp:BoundColumn DataField="dob_tasa" HeaderText="Tasa" />
                            <asp:BoundColumn DataField="tcon_nombre" HeaderText="Condición" />
                            <asp:BoundColumn DataField="mobl_detalle" HeaderText="Detalle" />
                            <asp:BoundColumn DataField="mobl_autoriza" HeaderText="Autoriza" />
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        <br>
        <table id="Table4" class="filtersIn">
            <tr>
                <td style="font-weight: bold; font-size: 10pt">
                    Comparativo Ventas:
                </td>
            </tr>
            <tr>
                <td class="scrollable">
                    <asp:DataGrid ID="dgrComparativos" runat="server" CssClass="datagrid" AutoGenerateColumns="False"
                        ShowFooter="True" CellPadding="3">
                        <FooterStyle CssClass="footer"></FooterStyle>
                        <HeaderStyle CssClass="header"></HeaderStyle>
                        <PagerStyle CssClass="pager" Mode="NumericPages"></PagerStyle>
                        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                        <ItemStyle CssClass="item"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="pdoc_codigo" HeaderText="" />
                            <asp:BoundColumn DataField="pdoc_nombre" HeaderText="Documento" FooterText="Totales"
                                FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="ANO1" HeaderText="" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="ANO2" HeaderText="" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="ANO3" HeaderText="" DataFormatString="${0:#,##0}" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="MES1ANO1" HeaderText="" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="MES2ANO1" HeaderText="" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="MES1ANO2" HeaderText="" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="MES2ANO2" HeaderText="" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="MES1ANO3" HeaderText="" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                            <asp:BoundColumn DataField="MES2ANO3" HeaderText="" DataFormatString="${0:#,##0}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-VerticalAlign="Top" />
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>
</fieldset>
