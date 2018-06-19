<%@ Control Language="c#" codebehind="AMS.Vehiculos.EntregaVehiculosProceso.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.EntregaVehiculosProceso" %>
<p>
	Por Favor Digite la Fecha de Entrega del Vehículo
	<asp:Label id="vehiculo" runat="server"></asp:Label>&nbsp;con el formato 
	año-mes-dia :
</p>
<p>
	<asp:TextBox id="fechaEntrega" runat="server"></asp:TextBox>
	&nbsp;&nbsp;
	<asp:Button id="btnEntrega" onclick="Efectuar_Entrega" runat="server" Text="Efectuar Entrega"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>


