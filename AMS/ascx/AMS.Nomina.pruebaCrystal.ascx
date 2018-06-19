<%@ Control Language="c#" codebehind="AMS.Nomina.pruebaCrystal.cs" autoeventwireup="false" Inherits="AMS.Nomina.pruebaCrystal" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>
    Porfavor Seleccione la Quincena para generar los recibos de pago. 
</p>
<p>
    <asp:DropDownList id="DDLQUINCENA" class="dmediano" runat="server"></asp:DropDownList>
</p>
<p>
    &nbsp;<asp:Button id="BTNMOSTRAR" onclick="btnmostrar" runat="server" Text="MOSTRAR REPORTE"></asp:Button>
</p>
<CR:CrystalReportViewer id="visor" runat="server" visible="false"></CR:CrystalReportViewer>
</td></tr>
</table>
</fieldset>