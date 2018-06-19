<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Inventarios.DeterioroInventario.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_DeterioroInventario" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script>
    $(function () {
        var fechaVal = $("#<%=txtFechaCorte.ClientID%>").val();
        $("#<%=txtFechaCorte.ClientID%>").datepicker();
        $("#<%=txtFechaCorte.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=txtFechaCorte.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFechaCorte.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFechaCorte.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=txtFechaCorte.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFechaCorte.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFechaCorte.ClientID%>").val(fechaVal);
    });

</script>
<fieldset>
    <TABLE id="Tabl" class="filtersIn">
	    <tbody>
		    <tr>
                <td>
                    Fecha de Corte:
                </td>
                <td>
                    <asp:TextBox ID="txtFechaCorte" runat="server" class="tpequeno" onkeyup="DateMask(this);"></asp:TextBox>
                </td>
                <td>
                    <asp:Label id="Label3" runat="server">Comprobante: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPrefijoComprobante" runat="server" Enabled="false" CssClass="tmediano"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    Año a liquidar:
                </td>
                <td>
                    <asp:TextBox ID="txtLiquidaYear" runat="server" class="tpequeno" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    Mes a liquidar:
                </td>
                <td>
                    <asp:TextBox ID="txtLiquidaMonth" runat="server" class="tpequeno" Enabled="false"></asp:TextBox>
                </td>
                <td></td>
            </tr>
        </tbody>
    </TABLE>
</fieldset>
<br />
<fieldset>
    <legend>Rango días de mora</legend>

    <asp:DataGrid id="dgDiasMora" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" 
    Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" 
    Font-Names="Verdana" AutoGenerateColumns="false" style="    width: 60%;margin: 1px;">
        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
        <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
        <AlternatingItemStyle backcolor="#DCDCDC"></AlternatingItemStyle>
        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Día inicio">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DIAINI") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Día fin">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DIAFIN") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Porcentaje Deterioro">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "TASADIARIA") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</fieldset>
<br />
<asp:Button ID="btnCalcular" runat="server" Text="Calcular Deterioro" onClick="CalcularDeterioro_Click" style="margin: 0% 0% 0% 5%;"/>

<br /><br />
<asp:PlaceHolder ID="plcDeterioro" runat="server" Visible="false">
    <fieldset>
        <legend>Deterioro</legend>
        <br />
        <asp:DataGrid id="dgDeterioro" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" 
        Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" 
        Font-Names="Verdana" AutoGenerateColumns="false" style="margin: 1px;"
        OnCancelCommand="DgInserts_Cancel"  OnEditCommand="DgInserts_Edit" OnUpdateCommand="DgInserts_Update">
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
            <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
            <AlternatingItemStyle backcolor="#DCDCDC"></AlternatingItemStyle>
            <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Codigo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CODIGO") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Nombre">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "NOMBRE") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Cantidad Actual">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CANT_ACTUAL", "{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Costo Promedio">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "COST_PROMEDIO") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ultima Venta">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ULTIMA_VENTA", "{0:yyyy-MM-dd}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ultimo Ingreso">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ULTIMO_INGRESO", "{0:yyyy-MM-dd}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Dias Ultima Venta">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DIAS_MORAVENT") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Dias Ultimo Ingreso">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DIAS_MORAINGR") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Porcentaje Deterioro">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "PORCENTAJE") %> 
                    </ItemTemplate>
                    <EditItemTemplate>
					    <asp:TextBox style="width: 45px;" id="txtEditTasa" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE") %>' />
				    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Deterioro">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DETERIORO", "{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn HeaderText="-" HeaderStyle-HorizontalAlign="Center" ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Tasa Manual"></asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
    <br />
    <asp:Button ID="btnGenerarComprobante" runat="server" Text="Generar Comprobante NIIF" onClick="GenerarComprobante_Click"  style="margin: 0% 0% 0% 5%;"/>
</asp:PlaceHolder>
<asp:Label ID="lb" runat="server"></asp:Label>
