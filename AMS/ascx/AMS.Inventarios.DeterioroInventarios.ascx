<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Inventarios.DeterioroInventarios.ascx.cs" Inherits="AMS.Inventarios.DeterioroInventarios" %>
<script>
    function abrirEmergente() {
        var objFamilia = document.getElementById("<%=txtFamilia.ClientID%>");
        ModalDialog(objFamilia, 'select pfam_codigo, pfam_nombre from PFAMILIAITEM order by  pfam_nombre;', new Array());
    }

    function CheckAll(Checkbox, idGrid) {
        //Posiblemente se debe eliminar este codigo... amenos que funcione bien.
        var gridItems;
        if (idGrid == 1)
            gridItems = document.getElementById("<%=dgItems.ClientID %>");
        else if (idGrid == 2)
            gridItems = document.getElementById("<%=dgItemsSeleccion.ClientID %>");

        for (i = 1; i < gridItems.rows.length; i++) {
            if(gridItems.rows[i].cells[6].getElementsByTagName("INPUT")[0].disabled == false)
                gridItems.rows[i].cells[6].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
        }
    }
</script>
<fieldset>
    <legend>Filtro items</legend>
    <TABLE id="Tabl" class="filtersIn">
	    <tbody>
		    <tr>
                <td>
                    Línea:
                </td>
                <td>
                    <asp:DropDownList ID="ddlLinea" runat="server"></asp:DropDownList>
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    Familia:
                </td>
                <td>
                    <asp:TextBox ID="txtFamilia" runat="server" ></asp:TextBox>
                </td>
                <td>
                    <asp:Image id="imgLupaFamilia" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente();" style="margin-left: -12px"></asp:Image>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    Almacén:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAlmacen" runat="server"></asp:DropDownList>
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    Año:
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </tbody>
    </TABLE>
</fieldset>

<br />
<asp:Button ID="btnCargarItems" runat="server" Text="Cargar Items" onClick="CargarItems_Click" style="margin: 0% 0% 0% 5%;"/>
<br /><br />

<asp:PlaceHolder ID="plcItems" runat="server" Visible="false">
    <fieldset>
        <legend>Items obtenidos</legend>
        <div style="color:Green">Items agregados:<asp:Label ID="lblContadorItems" runat="server" Text="0" ></asp:Label></div>
        <br />
        <asp:DataGrid id="dgItems" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" 
        Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" 
        Font-Names="Verdana" AutoGenerateColumns="false" style="margin: 1px;" ShowFooter="True" OnItemCommand="DgItems_onCommand" 
        OnItemDataBound="DgItems_DataBound">
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
            <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
            <AlternatingItemStyle backcolor="#DCDCDC"></AlternatingItemStyle>
            <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
            <FooterStyle CssClass="footer"></FooterStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Año">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ANO") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Almacen">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ALMACEN") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Código">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CODIGO") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Descripción">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Cantidad actual" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CANT_ACTUAL","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Costo promedio" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "COST_PROMEDIO","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn> 
                <asp:TemplateColumn HeaderText="Días ultima rotacion" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DIAS_ROTACION","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn> 
                <asp:TemplateColumn HeaderText="Porcentaje deterioro" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DETERIORO","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn> 
                <asp:TemplateColumn HeaderText="Selección" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <center>
                            <asp:Label ID="lblChk" runat="server" Text="Selección"></asp:Label><br />
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAll(this,1);" />
                        </center>
                    </HeaderTemplate>
			        <ItemTemplate>
                        <asp:CheckBox ID="cbRows" runat="server"/>
                    </ItemTemplate>
                    <FooterTemplate>
                        <center>
                            <asp:Button ID="btnAgregarItems" runat="server" Text ="Agregar items" CommandName="AgrearItems"/>
                        </center>
                    </FooterTemplate>
                </asp:TemplateColumn>               
            </Columns>
        </asp:DataGrid>
    </fieldset>
    
    <br />
    <asp:Button ID="btnConfirmarSeleccion" runat="server" Text="Confirmar Selección" onClick="ConfirmarSelecccion_Click" style="margin: 0% 0% 0% 5%;"/>
</asp:PlaceHolder>

<asp:PlaceHolder ID="plcSeleccion" runat="server" Visible="false">
    <fieldset>
        <legend>Items seleccionados</legend>
        <div style="color:Green">Cantidad: <asp:Label ID="lblSeleccionItems" runat="server" Text="0" ></asp:Label></div>
        <br />
        <asp:DataGrid id="dgItemsSeleccion" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" 
        Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" 
        Font-Names="Verdana" AutoGenerateColumns="false" style="margin: 1px;" ShowFooter="True" OnItemCommand="DgItSeleccion_onCommand">
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
            <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
            <AlternatingItemStyle backcolor="#DCDCDC"></AlternatingItemStyle>
            <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
            <FooterStyle CssClass="footer"></FooterStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Año">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ANO") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Almacen">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ALMACEN") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Código">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CODIGO") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Descripción">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Cantidad actual" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CANT_ACTUAL","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Costo promedio" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "COST_PROMEDIO","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn> 
                <asp:TemplateColumn HeaderText="Selección" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <center>
                            <asp:Label ID="lblChk" runat="server" Text="Selección"></asp:Label><br />
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAll(this,2);" />
                        </center>
                    </HeaderTemplate>
			        <ItemTemplate>
                        <asp:CheckBox ID="cbRows" runat="server"/>
                    </ItemTemplate>
                    <FooterTemplate>
                        <center>
                            <asp:Button ID="btnEliminarItems" runat="server" Text ="Eliminar items" CommandName="EliminarItems"/>
                        </center>
                    </FooterTemplate>
                </asp:TemplateColumn>               
            </Columns>
        </asp:DataGrid>
        <br />
        Centro de costo: <asp:DropDownList ID="ddlCentroCosto" runat="server" class="dmediano"></asp:DropDownList>
    </fieldset>

    <br />
    <asp:Button ID="btnGenerarComprobante" runat="server" Text="Generar Comprobante NIIF"   onClick="GenerarComprobante_Click" style="margin: 0% 0% 0% 5%;"/>
</asp:PlaceHolder>

<asp:Label ID="lb" runat="server"></asp:Label>