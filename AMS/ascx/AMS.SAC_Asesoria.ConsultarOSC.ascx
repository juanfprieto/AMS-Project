<%@ Control Language="c#" autoeventwireup="false" Inherits="AMS.SAC_Asesoria.ConsultarOSC" %>

<script language="javascript" src="../js/SAC.Web.Masks.js" type="text/javascript"></script>
<fieldset>
    <asp:Panel id="pnlFilFec" Visible="False" runat="server">
        <table>
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
    <asp:Panel id="pnlFilEst" Visible="False" runat="server">
        <table>
            <tbody>
                <tr>
                    <td>
                        Escoja el estado : 
                    </td>
                    <td>
                        <asp:DropDownList id="ddlestosc" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button id="btnconsest" onclick="btnconsest_Click" runat="server" Text="Consultar"></asp:Button>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</p>
<p>
    <asp:Panel id="pnlFilCli" Visible="False" runat="server">
        <table>
            <tbody>
                <tr>
                    <td>
                        Escoja el nit del cliente : 
                    </td>
                    <td>
                        <asp:DropDownList id="ddlnitcli" runat="server"></asp:DropDownList>
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
    <table>
        <tr>
            <TD>Exportar Excel&nbsp;&nbsp;</td>
            <td><asp:ImageButton id="ibExcel" onclick="Excel" runat="server" alt="Exportar Excel" ImageUrl="../img/AMS.Icon.Excel.png" BorderWidth="0px"></asp:ImageButton></TD>
        </tr>
    </table>
</asp:PlaceHolder>
    <asp:DataGrid id="dgOSC" runat="server" onItemCommand="dgOSC_ItemCommand" CssClass="datagrid" GridLines="Vertical" PageSize="15" AutoGenerateColumns="False">
        <FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
        <Columns>
            <asp:BoundColumn DataField="NUMOSC" ReadOnly="True" HeaderText="N&#250;mero de la OSC"></asp:BoundColumn>
            <asp:BoundColumn DataField="NUMSOL" ReadOnly="True" HeaderText="N&#250;mero de la Solicitud"></asp:BoundColumn>
            <asp:BoundColumn DataField="CLIENTE" ReadOnly="True" HeaderText="Cliente"></asp:BoundColumn>
            <asp:BoundColumn DataField="CONTACTO" ReadOnly="True" HeaderText="Contacto"></asp:BoundColumn>
            <asp:BoundColumn DataField="ESTADO" ReadOnly="True" HeaderText="Estado de la OSC"></asp:BoundColumn>
            <asp:BoundColumn DataField="FECHOR" ReadOnly="True" HeaderText="Fecha y Hora de Creaci&#243;n"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Ver Solicitud Enlazada">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" commandname="vse" runat="server" Text="Ver Solicitud" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Ver Orden Completa">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton2" commandname="voc" runat="server" Text="Ver Orden" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    </fieldset>