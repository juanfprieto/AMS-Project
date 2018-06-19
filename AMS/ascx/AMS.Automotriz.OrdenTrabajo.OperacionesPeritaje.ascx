<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenesTaller.OperacionesPeritaje.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.OperacionesPeritaje" %>
<p>
	<asp:PlaceHolder id="gruposPeritaje" runat="server"></asp:PlaceHolder>
</p>
<p>
	<asp:Button id="aceptar" onclick="Validar_Formulario" runat="server" Width="142px" Text="Aceptar"></asp:Button>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</p>
<p>
	<asp:Label id="costoPeritaje" runat="server"></asp:Label>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
