<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EmisionRecibos.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.EmisionRecibos" %>
<fieldset>
<legend>
    <div id="descripcionEncabezado" runat="server">
	    Seleccione el tipo de recibo que desea emitir :
    </div>
</legend>
<asp:RadioButtonList id="tipoRecibo" runat="server" AutoPostBack="True" BorderStyle="Solid" BorderWidth="1px"
 onSelectedIndexChanged="tipoRecibo_IndexChanged" RepeatDirection="Horizontal" width="98%">
	<asp:ListItem Value="RC">Recibo de Caja</asp:ListItem>
	<asp:ListItem Value="CE">Comprobante de Egreso</asp:ListItem> 
	<asp:ListItem Value="RP">Recibo de Caja Provisional</asp:ListItem>
	<asp:ListItem Value="RI">Impresion</asp:ListItem>
</asp:RadioButtonList>
</fieldset>

<P><asp:Label id="lb" runat="server"></asp:Label></P>

