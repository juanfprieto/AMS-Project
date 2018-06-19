<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Web.ModalPDF.aspx.cs" Inherits="AMS.Web.ModalPDF" %>
<%@ Register TagPrefix="uc1" TagName="Email" Src="../ascx/AMS.Tools.Email.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ReImpresión de Documentos</title>
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script>
        $(document).on("ready", inicio);

        function inicio() {
            var url = window.location.href;     // Returns full URL
            url = url.substring(url.indexOf("=") + 1, url.length);
            url = url.replace(/\+/g, "%20");
            //alert(url);
            $("#divPDF").html(
            '<object id="objPDF" data="../rptgen/' + url + ' " type="application/pdf" width="100%" height="100%">' +
            'alt : <a href="../rptgen/' + url + '">test.pdf</a></object>');
            $('.boxSizeCorreo').width(790);
            $(".fontCorreo").css("color", "white");
        }
    </script>
</head>
<body style="overflow: hidden;">
   <form id="form1" runat="server">
         <div id="divPDF" style="position:absolute; width:99%; height:90%;">
        </div>
       <div style="position:absolute; bottom: -3px; background-color: rgb(50, 54, 57);">
         <uc1:Email runat="server" id="opcEnviarMail" style="width: 784px;"></uc1:Email>
       </div>
    </form>
</body>
</html>
