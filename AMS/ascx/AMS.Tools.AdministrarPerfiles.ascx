<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.AdministrarPerfiles.ascx.cs" Inherits="AMS.Tools.AdministrarPerfiles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table>
	<tr>
		<td vAlign="middle" align="center"><IMG src="../img/AMS.Tools.CrearPerfil.png">
		</td>
		<td>
			<P>Cree un nuevo perfil para manejar grupos de usuarios
			</P>
			<P><asp:button id="btnCrear" Text="Crear Perfil" Runat="server" onclick="btnCrear_Click"></asp:button></P>
		</td>
	</tr>
	<tr>
		<td vAlign="middle" align="center"><IMG src="../img/AMS.Tools.ModificarPerfil.png">
		</td>
		<td>
			<P>Modifique la información de un perfil creado con anterioridad
			</P>
			<P><asp:dropdownlist id="ddlModPerfil" runat="server"></asp:dropdownlist>
				<asp:Button id="btnModificar" Text="Modificar" runat="server" onclick="btnModificar_Click"></asp:Button></P>
		</td>
	</tr>
	<tr>
		<td vAlign="middle" align="center">
			<img src="../img/AMS.Tools.EliminarPerfil.png">
		</td>
		<td>
			<P>Elimine un perfil existente</P>
			<P>
				<asp:DropDownList ID="ddlEliPerfil" Runat="server"></asp:DropDownList>&nbsp;
				<asp:Button id="btnEliminar" Text="Eliminar" runat="server" onclick="btnEliminar_Click"></asp:Button></P>
		</td>
	</tr>
</table>
