<%@ Control Language="c#" codebehind="AMS.Nomina.EliminarAcumulados.cs" autoeventwireup="false" Inherits="AMS.Nomina.EliminarAcumulados" %>
<fieldset>

<table id="Table" class="filtersIn">
<tr>
<td>
<p>
    Si reliquido alguna quincena y esta tuvo incidencia en alguno de los siguientes procesos
    porfavor elimine dicho proceso y generelo nuevamente. 
</p>
<p>
    <asp:DropDownList id="DDLPROCESO" class="dpequeno" AutoPostBack="True" OnSelectedIndexChanged="CargarGrilla" runat="server">
        <asp:ListItem Value="Cesantias">Cesantias</asp:ListItem>
        <asp:ListItem Value="Primas">Primas</asp:ListItem>
    </asp:DropDownList>
</p>
<p>
    <asp:DataGrid id="DataGrid1" runat="server" OnItemCommand="BorrarDatos" AutoGenerateColumns="False">
        <FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
        <Columns>
            <asp:BoundColumn DataField="SECUENCIA" HeaderText="SECUENCIA"></asp:BoundColumn>
            <asp:BoundColumn DataField="FECHA INICIO" HeaderText="FECHA INICIO"></asp:BoundColumn>
            <asp:BoundColumn DataField="FECHA FINAL" HeaderText="FECHA FINAL"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="ELIMINAR">
                <ItemTemplate>
                    <asp:Button id="Button1" runat="server" Text="Elim." enabled="false" ></asp:Button>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</p>
<p>
</p>
</td></tr></table></fieldset>