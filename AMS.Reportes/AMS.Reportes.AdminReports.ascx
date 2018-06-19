<%@ Control Language="c#" codebehind="AMS.Reportes.AdminReports.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Admin" %>
<fieldset>
<table class="filters">
	
		
        <tr>
			<th class="filterHead">
				<img height="70" src="../img/AMS.Flyers.News.png" border="0">
			</th>
            <td>
				<p>
					Esta opción nos permite crear nuevos reportes y agregarlos al menu de usuario
				</p>
				<p>
					<asp:Button id="btnIngr1" onclick="Ingresar_Nuevo" runat="server" Text="Ingresar"></asp:Button>
				</p>
                </td>
                </tr>
		<tr>
		
			<th class="filterHead">
				<img height="70" src="../img/AMS.Flyers.Eliminar.png" border="0">
			</th>
            <td>
				<p>
					Mediante esta opción se nos permite eliminar reportes existentes dentro del 
					programa
				</p>
				<p>
					<asp:Button id="btnIngr2" onclick="Ingresar_Eliminar" runat="server" Text="Ingresar"></asp:Button>
				</p>
			</td>
            </tr>
	</table>
<asp:Label id="lb" runat="server"></asp:Label>
</fieldset>