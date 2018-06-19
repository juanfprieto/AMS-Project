<%@ Control Language="c#" codebehind="AMS.Contabilidad.LisCon.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.LisCon" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>

    <table class="filters">
            <tbody>
            <tr>
                <th class="filterHead">
                    <img height="60" src="../img/AMS.Flyers.Filters.png" border="0" /> 
                </th>
                <td valign="middle">
                   
                        Año: 
                        <asp:DropDownList id="year" class="dpequeno" runat="server"></asp:DropDownList>
                         Mes: 
                        <asp:DropDownList id="month" class="dpequeno" runat="server"></asp:DropDownList>
                        
                   
                       <asp:RadioButtonList id="RadioOpcion" runat="server" AutoPostBack="True" Onselectedindexchanged="opcion_cambio">
                            <asp:ListItem Value="TC" >Todos los comprobantes</asp:ListItem>
                            <asp:ListItem Value="UTC">Un tipo de comprobante</asp:ListItem>
                        </asp:RadioButtonList>
                   
   <asp:Label id="lblTipoComp" runat="server">Tipo de Comprobante:</asp:Label>&nbsp;
   <asp:DropDownList id="typeDoc" class="dmediano" runat="server"></asp:DropDownList>
                      
                        <asp:Button id="Consulta" onclick="Consulta_Click" runat="server" Text="Generar"></asp:Button>
                       
                        </td>
            </tr>
            </tbody>
    </table>
<p>
</p>
<p>
    <asp:PlaceHolder id="toolsHolder" runat="server" visible="false">
        <table class="tools">
            <tbody>
                <tr>
                    <td>
                        <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel" OnClick="ImprimirExcelGrid" runat="server"
                            alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" Width="60px">
                        </asp:ImageButton>
                    </td>
                    <td>
                        <img height="30" src="../img/AMS.Flyers.Tools.png" border="0" /></td>
                    <td>
                        Imprimir <a href="javascript: Lista()"><img height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0" /> </a></td>
                    <td>
                        &nbsp; &nbsp;Enviar por correo 
                        <asp:TextBox id="tbEmail" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 188px; POSITION: absolute; TOP: 271px" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
                        <asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"></asp:ImageButton>
                    </td>
                    <td></td>
                </tr>
            </tbody>
        </table>
    </asp:PlaceHolder>
</p>
<p>
</p>
<br />
<table class="reports" align="center" bgcolor="gray">
    <tbody>
        <tr>
            <td>
                <asp:Table id="tabPreHeader" BorderWidth="0px" EnableViewState="False" CellSpacing="0" CellPadding="1" BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center" Width="100%"></asp:Table>
            </td>
        </tr>
        <tr>
            <td align="middle">
                <p>
                    <asp:DataGrid id="dg" runat="server" cssclass="datagrid" OnItemDataBound="Report_ItemDataBound" AutoGenerateColumns="False">
                        <FooterStyle cssclass="footer"></FooterStyle>
                        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
                        <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
                        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
                        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
                        <ItemStyle cssclass="item"></ItemStyle>
                    </asp:DataGrid>
                </p>
            </td>
        </tr>
        <tr>
         <p>
        <asp:Label ID="lblResult" runat="server"></asp:Label></p>
         <p>
         <asp:TextBox ID="txtSort" runat="server" Visible="False"></asp:TextBox><asp:TextBox
            <td>
                <asp:Table id="tabFirmas" BorderWidth="0px" EnableViewState="False" CellSpacing="0" CellPadding="1" BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center" Width="100%"></asp:Table>
            </td>
        </tr>
    </tbody>
</table>
<asp:Label id="lblAux" runat="server"></asp:Label>
