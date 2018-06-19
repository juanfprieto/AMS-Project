<%@ Page Language="c#" codebehind="AMS.Nomina.MostrarReportesNomina12meses.cs" autoeventwireup="false" Inherits="AMS.Nomina.MostrarReportes" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<html>
<head>
</head>
<body>
    <form runat="server">
        <CR:CrystalReportViewer id="visor" runat="server"></CR:CrystalReportViewer>
        <p>
            <asp:Label id="lb" runat="server"></asp:Label>
        </p>
    </form>
</body>
</html>
