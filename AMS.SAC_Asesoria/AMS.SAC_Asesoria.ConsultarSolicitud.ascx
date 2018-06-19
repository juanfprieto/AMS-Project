<%@ Control Language="c#" autoeventwireup="false" Inherits="AMS.SAC_Asesoria.ConsultarSolicitud_SAC" %>
<script language="javascript" src="../js/SAC.Web.Masks.js" type="text/javascript"></script>
<fieldset>
    <asp:Panel id="pnlFilFec" Visible="False" runat="server">
        <table id="Table" class="filtersIn">
            <tbody>
                <tr>
                    <td>
                        Escoja la fecha de inicio de la consulta 
                    </td>
                    <td>
                        <img onmouseover="calendario.style.visibility='visible'" onmouseout="calendario.style.visibility='hidden'" src="../img/SAC.Icon.Calendar.gif" border="0" /> 
                        <table id="calendario" onmouseover="calendario.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendario.style.visibility='hidden'">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:Calendar id="calendarioFecha" runat="server" OnSelectionChanged="Cambiar_Fecha"></asp:Calendar>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:TextBox id="fecha" onkeyup="DateMask(this)" runat="server" Width="76px"></asp:TextBox>
                        <asp:RegularExpressionValidator id="validatorFecha" runat="server" ErrorMessage="RegularExpressionValidator" ControlToValidate="fecha" Text="*" ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Escoja la fecha final de la consulta 
                    </td>
                    <td>
                        <img onmouseover="calendario2.style.visibility='visible'" onmouseout="calendario2.style.visibility='hidden'" src="../img/SAC.Icon.Calendar.gif" border="0" /> 
                        <table id="calendario2" onmouseover="calendario2.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendario2.style.visibility='hidden'">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:Calendar id="calendarioFecha2" runat="server" OnSelectionChanged="Cambiar_Fecha2"></asp:Calendar>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:TextBox id="fechafin" onkeyup="DateMask(this)" runat="server" Width="76px"></asp:TextBox>
                        <asp:RegularExpressionValidator id="validatorFecha2" runat="server" ErrorMessage="RegularExpressionValidator" ControlToValidate="fechafin" Text="*" ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button id="btnconsfec" onclick="btnconsfec_Click" runat="server" Text="Consultar"></asp:Button>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
 
<p>
    <asp:Panel id="pnlFilCon" Visible="False" runat="server">
        <table id="Table1" class="filtersIn">
            <tbody>
                <tr>
                    <td>
                        Escoja el nit del cliente : 
                    </td>
                    <td>
                        <asp:DropDownList id="ddlnitcli" runat="server" AutoPostback="true" onSelectedIndexChanged="ddlnitcli_IndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Escoja el nit del contacto : 
                    </td>
                    <td>
                        <asp:DropDownList id="ddlnitcon" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button id="btnconscon" onclick="btnconscon_Click" runat="server" Text="Consultar"></asp:Button>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</p>
<p>
    <asp:Panel id="pnlFilCli" Visible="False" runat="server">
        <table id="Table2" class="filtersIn">
            <tbody>
                <tr>
                    <td>
                        Escoja el nit del cliente : 
                    </td>
                    <td>
                        <asp:DropDownList id="ddlnitcli2" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button id="btnconscli" onclick="btnconscli_Click" runat="server" Text="Consultar"></asp:Button>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</p>
<asp:PlaceHolder ID="plcExcel" runat="server" visible="false">
    <table id="Table3" class="filtersIn">
        <tr>
            <TD>Exportar Excel&nbsp;&nbsp;</td>
            <td><asp:ImageButton id="ibExcel" onclick="Excel" runat="server" alt="Exportar Excel" ImageUrl="../img/AMS.Icon.Excel.png" BorderWidth="0px"></asp:ImageButton></TD>
        </tr>
    </table>
</asp:PlaceHolder>
<asp:DataGrid id="dgMaeSol" runat="server" onItemCommand="dgMaeSol_ItemCommand" BorderWidth="1px" AutoGenerateColumns="False" Font-Names="Verdana" GridLines="Vertical" BorderStyle="None" BackColor="White" BorderColor="#999999" CellPadding="3" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" PageSize="15">
    <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
    <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
    <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
    <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
    <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
    <Columns>
        <asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="N&#250;mero"></asp:BoundColumn>
        <asp:BoundColumn DataField="CLIENTE" ReadOnly="True" HeaderText="Cliente"></asp:BoundColumn>
        <asp:BoundColumn DataField="CONTACTO" ReadOnly="True" HeaderText="Contacto"></asp:BoundColumn>
        <asp:BoundColumn DataField="FECHOR" ReadOnly="True" HeaderText="Fecha y Hora de Creación"></asp:BoundColumn>
        <asp:BoundColumn DataField="ESTADO" ReadOnly="True" HeaderText="Estado"></asp:BoundColumn>
        <asp:BoundColumn DataField="DESCRIPCION" ReadOnly="True" HeaderText="Descripcion"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Ver Detalles">
            <ItemTemplate>
                <asp:LinkButton id="lnbdetsol" runat="server" CommandName="verdetalles" Text="Ver Detalles" />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Impresión">
            <ItemTemplate>
                <asp:LinkButton id="lnbimp" runat="server" CommandName="impresion" Text="Imprimir" />
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<p>
    <asp:DataGrid id="dgDetSol" runat="server" BorderWidth="1px" AutoGenerateColumns="true" Font-Names="Verdana" GridLines="Vertical" BorderStyle="None" BackColor="White" BorderColor="#999999" CellPadding="3" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" PageSize="15">
        <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
        <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
        <Columns></Columns>
    </asp:DataGrid>
</p>
</fieldset>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>