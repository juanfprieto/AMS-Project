<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ModalDialogAyuda.aspx.cs" Inherits="AMS.Web.ModalDialogAyuda" %>
<html>
<head>
    <script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
    <link href="../css/AMS.css" type="text/css" rel="stylesheet" />
</head>
<body bgcolor="123456">
    <form runat="server">
        <table width="300" border="0">
            <tbody>
                <tr>
                    <td align="middle" bgcolor="#0066cc">
                        <asp:Label id="lbCampo" runat="server" font-bold="True" forecolor="White"></asp:Label></td>
                </tr>
                <tr>
                    <td bgcolor="#dddddd">
                        <asp:Label id="lbAyuda" runat="server" font-bold="True"></asp:Label></td>
                </tr>
            </tbody>
        </table>
        <p>
            <asp:Label id="lb" runat="server"></asp:Label>
        </p>
    </form>
</body>
</html>
