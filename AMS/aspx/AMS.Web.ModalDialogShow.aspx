<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Web.ModalDialogShow.aspx.cs" Inherits="AMS.Web.ModalDialogShow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Modal Dialog Show</title>
    <script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
    <script language="javascript">
        <%
            
                string parametros = Request.QueryString["Vals"];
                Response.Write("parent.terminarDialogoU( '" + parametros + "');");
            
        %>
	</script>
    <link href="../css/normalize.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="../css/bootstrap.min.css"/>
    <link rel="stylesheet" href="../css/bootstrap-theme.min.css"/>

    <link href="../css/AMS.css" type="text/css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" type="text/css" rel="stylesheet"  />
    <link rel="stylesheet" href="../css/jquery.ui.theme.css">
    <script type="text/javascript" src="../js/modernizr.js"></script>
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>
    <script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
    <script type="text/javascript" src="../js/generales.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.core.js" ></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.widget.js" ></script>
    <script type="text/javascript" src="../js/ui/jquery.ui.datepicker.js" ></script>
    <script type="text/javascript" src="../js/bootstrap.min.js"></script>

    <link rel="stylesheet" href="../css/fecha.css"/>
    <link rel="stylesheet" href="../css/jquery.ui.all.css"/>
    <link rel="stylesheet" href="../css/jquery.ui.theme.css"/>
    <link rel="stylesheet" href="../css/AMS.GridView.css"/>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        
    </div>
    </form>
</body>
</html>
