<%@ outputcache duration="10" varybyparam="params" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Web.ModalDialogCont.aspx.cs" Inherits="AMS.Web.ModalDialogCont" %>
<!DOCTYPE html>
<html lang="es">
<head>
	<meta charset="utf-8" />
    <link rel="stylesheet" href="../css/AMS.css" type="text/css"  />
    <script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
</head>
<body>
<form runat="server">
    <table>
        <tr>
            <td>
                <asp:datagrid id="dgTable" runat="server" PageSize="100"
                BorderStyle="Ridge" BorderWidth="2px" BorderColor="White" BackColor="White" CellPadding="3"
                GridLines="None" CellSpacing="1" AllowPaging="True" EnableViewState="True"
                AutoGenerateColumns="True">
                    <FooterStyle forecolor="Black"  font-size="11px" backcolor="#C6C3C6"></FooterStyle>
                    <HeaderStyle font-bold="True" font-size="11px" forecolor="#E7E7FF" backcolor="#4A3C8C"></HeaderStyle>
                    <PagerStyle horizontalalign="Center" font-size="11px" forecolor="Black" position="TopAndBottom" backcolor="#C6C3C6"
                    mode="NumericPages"></PagerStyle>
                    <SelectedItemStyle font-bold="True"  font-size="11px" forecolor="White" backcolor="#9471DE"></SelectedItemStyle>
                    <ItemStyle forecolor="Black" backcolor="#DEDFDE"  font-size="11px"></ItemStyle>
                </asp:datagrid>
            </td>
        </tr>
    </table>
</form>
</body>
</html>
