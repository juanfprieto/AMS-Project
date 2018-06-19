<%@ Page Language="c#" codebehind="AMS.Nomina.Acumulado.MostrarReportesProvision.cs" autoeventwireup="false" Inherits="AMS.Nomina.MostrarReportesProvision" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<html>
<head>
</head>
<body>
    <form runat="server">
        <p>
            &nbsp;<CR:CrystalReportViewer id="visor" runat="server"></CR:CrystalReportViewer>
        </p>

    </form>
</body>
</html>
