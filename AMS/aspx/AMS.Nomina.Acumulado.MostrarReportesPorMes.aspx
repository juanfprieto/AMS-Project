<%@ Page Language="c#" codebehind="AMS.Nomina.Acumulado.MostrarReportesPorMes.cs" autoeventwireup="false" Inherits="AMS.Nomina.MostrarReportesPorMes" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<html>
<head>
</head>
<body>
    <form runat="server">
        <asp:Label id="lb" runat="server">Label</asp:Label>
        <CR:CrystalReportViewer id="visor" runat="server"></CR:CrystalReportViewer>
    </form>
</body>
</html>
