<%@ Control Language="c#" codebehind="AMS.Contabilidad.DepreciaActivos.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.DepreciaActivos" %>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<asp:Label id="Label1" runat="server">Este proceso nos permite ir restandole valor
a los activos fijos a medida que se van usando</asp:Label> 
<p>
</p>
<p>
    <asp:Label id="Label3" runat="server">Comprobante: </asp:Label>
    <asp:TextBox ID="txtPrefijoComprobante" runat="server" Enabled="false" CssClass="tmediano"></asp:TextBox>
    <asp:DropDownList id="tComprobante" class="dmediano" runat="server" visible ="false"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="Label2" runat="server"><br />
    Año: </asp:Label> 
    <asp:DropDownList id="ano" class="dpequeno" runat="server" OnSelectedIndexChanged="Cambio_Ano" AutoPostBack="true"></asp:DropDownList>
    &nbsp; &nbsp; <asp:Label id="Label4" runat="server">Mes: </asp:Label>
    <asp:DropDownList id="mes" class="dpequeno" runat="server" OnSelectedIndexChanged="Cambio_Mes" AutoPostBack="true"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</p>
<p>
    <asp:Label id="valorPaag" runat="server" height="15px" width="129px"></asp:Label>
</p>
<p>
    <asp:Label id="LBvalorMinimo" runat="server" height="15px" width="129px"></asp:Label>
</p>

<p>
    <asp:Button id="efectuar" onclick="efectuar_depreciacion" runat="server" Text="Calcular Depreciacion"></asp:Button>
    <%--<asp:Button id="efectuarNIIF" onclick="efectuar_depreciacion" runat="server" Text="Efectuar NIIF"></asp:Button>--%>
</p>
</td>
</tr>
</table>
</fieldset>
<p>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; DEPRECIACIÓN LEGAL Y NIIF  
    <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel" OnClick="ImprimirExcelGrid" runat="server"
                                    alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" Width="27px" Visible="false">
    </asp:ImageButton>
    <asp:Datagrid id="comprobanteG" runat="server" cssclass="datagrid" AutoGenerateColumns="False" align="center">
        <FooterStyle cssclass="footer"></FooterStyle>
        <HeaderStyle font-bold="True" cssclass=""></HeaderStyle>
        <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
        <ItemStyle cssclass="item"></ItemStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="CUENTA NIIF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CUENTA_NIIF") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CUENTA">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CUENTA") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="NIT">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "NIT") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="PREF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "PREF") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="DOCREF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DOC_REF") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CODIGO ACTIVO">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CODIGO_ACTIVO") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="DETALLE">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DETALLE") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="SEDE">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "SEDE") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="C.COSTO">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CENTRO_COSTO") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="DEBITO">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DEBITO","{0:N}") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CREDITO">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CREDITO","{0:N}") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="BASE">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "BASE","{0:N}") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="DEBITO NIIF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DEBITO_NIIFAUX","{0:N}") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CREDITO NIIF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CREDITO_NIIFAUX","{0:N}") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:Datagrid>
    <br />
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  DEPRECIACIÓN NIIF
    <asp:Datagrid id="comprobanteGNiif" runat="server" cssclass="datagrid" AutoGenerateColumns="False" align="center">
        <FooterStyle cssclass="footer"></FooterStyle>
        <HeaderStyle font-bold="True" cssclass=""></HeaderStyle>
        <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
        <ItemStyle cssclass="item"></ItemStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="CUENTA">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CUENTA") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="NIT">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "NIT") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="PREF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "PREF") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="DOCREF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DOC_REF") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CODIGO ACTIVO">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CODIGO_ACTIVO") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="DETALLE">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DETALLE") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="SEDE">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "SEDE") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="C.COSTO">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CENTRO_COSTO") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="DEBITO_NIIF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DEBITO","{0:N}") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CREDITO_NIIF">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CREDITO","{0:N}") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="BASE">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "BASE","{0:N}") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:Datagrid>
</p>
<p>
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  
    <asp:Label id="Lbdebito" Visible="False" runat="server">Total Debito:</asp:Label> 
    <asp:Label id="LbtotalDebito" style="font-weight: 600;"  runat="server"></asp:Label> 
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label id="Lbcredito" Visible="False" runat="server">Total Credito:</asp:Label> 
    <asp:Label id="LbtotalCredito" style="font-weight: 600;" runat="server"></asp:Label> 
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label id="LbdebitoNIIF" Visible="False" runat="server">Total DebitoNIIF:</asp:Label> 
    <asp:Label id="LbtotalDebitoNIIF" style="font-weight: 600;"  runat="server"></asp:Label> 
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label id="LbcreditoNIIF" Visible="False" runat="server">Total CreditoNIIF:</asp:Label> 
    <asp:Label id="LbtotalCreditoNIIF" style="font-weight: 600;"  runat="server"></asp:Label> 
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   
</p>
<p>
     <asp:Button id="guardar" onclick="guardar_comprobanteMixto" runat="server" Text="Guardar Comprobantes LEGAL y NIIF" Visible="False"></asp:Button>
    <asp:Button id="guardar1" onclick="guardar_comprobanteLegal" runat="server" Text="Guardar Comprobantes solo LEGAL" Visible="False"></asp:Button>
    <asp:Button id="guardar2" onclick="guardar_comprobanteNiif" runat="server" Text="Guardar Comprobantes solo NIIF" Visible="False"></asp:Button>
</p>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>