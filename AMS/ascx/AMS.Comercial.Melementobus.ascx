<%@ Control Language="c#" codebehind="AMS.Comercial.Melementobus.ascx.cs" autoeventwireup="false" Inherits="AMS.Comercial.Melementobus" %>
<p>
	Catalogo del Bus :&nbsp;
	<asp:DropDownList id="catalogoBus" Width="228px" runat="server" OnSelectedIndexChanged="CrearGrafico"
		AutoPostBack="True"></asp:DropDownList>
	&nbsp;&nbsp; Puesto :
	<asp:TextBox id="puesto" runat="server"></asp:TextBox>
</p>
<p>
	<asp:PlaceHolder id="literalsControls" runat="server"></asp:PlaceHolder>
</p>
<p>
</p>
<p>
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p>
