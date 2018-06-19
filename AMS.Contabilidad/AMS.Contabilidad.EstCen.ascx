<%@ Control Language="c#" codebehind="AMS.Contabilidad.EstCen.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.EstCen" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<div class="header">
    <p>
        <asp:Label id="reportInfo" runat="server"></asp:Label>
    </p>
    <p>
        <table id="table1" class="filtersIn">
            <tbody>
                <tr>
                    <th class="filterHead">
                        <img height="60" src="../img/AMS.Flyers.Filters.png" border="0" />
                    </th>
                    <td>
                        <p>
                            <asp:PlaceHolder id="filterHolder" runat="server"></asp:PlaceHolder>
                        </p>
                    </td>
                </tr>
            </tbody>
        </table>
    </p>
    <p>
        <asp:PlaceHolder id="toolsHolder" runat="server" visible="false">
            <table class="tools" width="780">
                <tbody>
                    <tr>
                        <th class="filterHead">
                            <img height="30" src="../img/AMS.Flyers.Tools.png" border="0"/></th>
                        <td>
                            Imprimir <a href="javascript: this.print()"><img height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0" /> </a></td>
                        <td>
                            &nbsp; &nbsp;Enviar por correo
                            <asp:TextBox id="tbEmail" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
                            <asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.gif" BorderWidth="0px"></asp:ImageButton>
                        </td>
                        <td width="380"></td>
                    </tr>
                </tbody>
            </table>
        </asp:PlaceHolder>
    </p>
    <p>
        <table class="reports" width="780" bgcolor="gray">
            <tbody>
                <tr>
                    <td>
                        <asp:Table id="tabPreHeader" BorderWidth="0px" CellSpacing="0" CellPadding="1" BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"></asp:Table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <ASP:DataGrid id="report" runat="server" cssclass="datagrid" CellSpacing="1" CellPadding="3" AllowSorting="true" autogeneratecolumns="false" OnItemDataBound="Report_ItemDataBound">
                            <FooterStyle cssclass="footer"></FooterStyle>
                            <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
                            <PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
                            <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
                            <ItemStyle cssclass="item"></ItemStyle>
                            <AlternatingItemStyle cssclass="alternate" />
                        </ASP:DataGrid>
                    </td>
                </tr>
            </tbody>
        </table>
    </p>
</div>
