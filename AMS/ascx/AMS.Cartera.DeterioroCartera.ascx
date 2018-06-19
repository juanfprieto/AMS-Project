<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Cartera.DeterioroCartera.ascx.cs" Inherits="AMS.Cartera.DeterioroCartera" %>
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
    <legend>Rango días de mora NIIF</legend>

    <asp:DataGrid id="dgDiasMoraNIIF" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" 
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

<fieldset>
    <legend>Rango días de mora LEGAL</legend>

    <asp:DataGrid id="dgDiasMoraLEGAL" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" 
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
<asp:Button ID="btnCalcular" runat="server" Text="Calcular Deterioro y Provision" onClick="CalcularDeterioro_Click" style="margin: 0% 0% 0% 5%;"/>
<br /><br />
<asp:PlaceHolder ID="plcDeterioro" runat="server" Visible="false">
    
    <fieldset>
        <legend>Deterioro de Cartera</legend>
        <br />
        <b>Valor Total Deterioro Niif Mes Actual: </b><asp:Label ID="lbValorDeterioroNiif" runat="server" text="0"></asp:Label><br />
        <b>Valor Total Provision Legal Mes Actual: </b><asp:Label ID="lbValorDeterioroLegal" runat="server" text="0"></asp:Label>
        <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel" OnClick="ImprimirExcelGrid" runat="server"
                                    alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" Width="27px">
        </asp:ImageButton>
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
                <asp:TemplateColumn HeaderText="Nit Cliente">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "NIT") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Nombre Cliente">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "NOMBRE") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Prefijo Factura">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "PREFIJO") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Número Factura">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "NUMERO") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Monto" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "MONTO","{0:N0}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Días de mora">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DIASMORA") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Porcentaje Deterioro NIIF">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "TASANIIF") %> 
                    </ItemTemplate>
                    <EditItemTemplate>
					    <asp:TextBox style="width: 45px;" id="txtEditTasaNiif" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TASANIIF") %>' />
				    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Valor Deterioro Niif" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "VALORNIIF","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Valor Anterior Niif" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "VALORANTERIORNIIF","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Fecha vencimiento">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "FECHAVENC", "{0:yyyy-MM-dd}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Porcentaje Provision Legal">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "TASALEGAL") %> 
                    </ItemTemplate>
                    <EditItemTemplate>
					    <asp:TextBox style="width: 45px;" id="txtEditTasaLegal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TASALEGAL") %>' />
				    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Valor Provision Legal" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "VALORLEGAL","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Valor Anterior Provision" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "VALORANTERIORLEGAL","{0:N}") %> 
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn HeaderText="-" HeaderStyle-HorizontalAlign="Center" ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Tasa Manual"></asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
    <br />
    <br />
    <asp:Button ID="btnGenerarComprobanteNiif" runat="server" Text="Generar Comprobante Deterioro NIIF" onClick="GenerarComprobante_Click" style="margin: 0% 0% 0% 5%;"/>
    <asp:Button ID="btnGenerarComprobanteLegal" runat="server" Text="Generar Comprobante Provision Legal" onClick="GenerarComprobante_Click" style="margin: 0% 0% 0% 5%;"/>
</asp:PlaceHolder>
<asp:Label ID="lb" runat="server"></asp:Label>