<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenesTaller.EstadoOrdenes.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.EstadoOrdenes" %>
<p>
    <asp:DataGrid id="ordenes" OnItemCommand="dgSeleccion_Ordenes" runat="server" HeaderStyle-BackColor="#ccccdd"  Font-Size="8pt" AllowPaging="false" PageSize="100" OnPageIndexChanged="DgUpdate_Page" on
    Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" 
    Font-Names="Verdana" AutoGenerateColumns="false">
        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
        <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
        <AlternatingItemStyle backcolor="#DCDCDC"></AlternatingItemStyle>
        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="PREFIJO">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "PREFIJO") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Nº ORDEN">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "NUMEROORDEN") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Nº ENTRADA">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "NUMEROENTRADA") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="ESTADO ORDEN">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "ESTADOORDEN") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="PLACA">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "PLACA") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="FECHA ENTRADA">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "FECHAENTRADA") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="MODIFICAR">
                <ItemTemplate>
                    <asp:Button CommandName="modificar" Text="Modificar" ID="modifi" runat="server" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="PRELIQUIDAR">
                <ItemTemplate>
                    <asp:Button CommandName="preliquidar" Text="PreLiquidar" ID="preliqui" runat="server" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</p>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>