<%@ Control Language="c#" codebehind="AMS.Reportes.MenuReporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Menu" %>
<p>
	Ahora configuraremos en que opcion del menu vamos a colocar este reporte. A 
	continuacion
	se presentan las opciones del menu donde usted podra colocar su reporte :
</p>
<p>
	Opciones del Menu Disponibles :
	<asp:DropDownList id="opcionesMenu" runat="server"></asp:DropDownList>
</p>
<p>
	<asp:Button id="btnAcept" onclick="Agregar_OpcionMenu" runat="server" Text="Aceptar"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
