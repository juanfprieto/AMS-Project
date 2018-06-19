<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.DBManager.Reporte.aspx.cs" Inherits="AMS.DBManager.Reporte" %>
<html>
<head>
    <title><% Response.Write("Reporte de "+Request.QueryString["path"]);%></title>
</head>
<body onload="window.print();">
    <link href="../css/AMS.css" type="text/css" rel="stylesheet" />
    <form runat="server">
        <p>
            <asp:Label id="lbInfo" runat="server"></asp:Label>
        </p>
    </form>
</body>
</html>
