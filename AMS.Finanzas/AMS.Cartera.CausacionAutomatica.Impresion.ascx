<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.CausacionAutmatica.Impresion.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.ImpresionCausacionAutomatica" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<p>
    <asp:Button id="btnImprimir" onclick="btnImprimir_Click" Text="Vista Impresión" runat="server"></asp:Button>
    
</p>
<p>
    <CR:CrystalReportViewer id="visor" runat="server"></CR:CrystalReportViewer>
</p>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
    <asp:Label ID="lbInfo" runat="server"></asp:Label>
</p>