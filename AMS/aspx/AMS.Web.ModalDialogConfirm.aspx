<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Web.ModalDialogConfirm.aspx.cs" Inherits="AMS.Web.AMS.Web.ModalDialogConfirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
    <link href="../css/ModalDialogConfirm.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="FrmPrincipal" runat="server">
    <div id="Main">
        <div id="Dialog">
            <h1>..::ADVERTENCIA::..</h1>
            <asp:Label ID="LblMensaje" runat="server"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
