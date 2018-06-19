<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.LiquidacionInteresesMora.Impresion.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.ImpresionMora" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<p>
	<asp:Button id="btnImprimir" onclick="btnImprimir_Click" runat="server" Text="Vista Impresión"></asp:Button>
</p>
<p>
	<CR:CrystalReportViewer id="visor" runat="server" Width="350px" Height="50px"></CR:CrystalReportViewer>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
