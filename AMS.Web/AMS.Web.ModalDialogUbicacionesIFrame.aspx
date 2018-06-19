<%@ Page Language="C#" Debug="true" autoeventwireup="True"%>
<HTML>
<head>
    <meta charset ="UTF-8" />
    <script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
     <script type="text/javascript" src="../js/jquery.js"></script>
      <script language="javascript" type="text/javascript">
          $(document).ready(function () {

              if (document.baseURI.split('&')[1] === "cierre=1") {
                  var valor = document.activeElement.localName;
              }
          });
          //$(window).load(function () {
          //    alert('window load')
          //});
  </script>
    <link href="../style/AMS.css" type="text/css" rel="stylesheet" /> 
</head>
<body class="mainApp" onload="loadSrc()">
    <form runat="server">
    <iframe name="iFrameModalDialog" src="" frameborder="0" width="100%" height="100%">
    </iframe>
    </form>
</body>
</html>
