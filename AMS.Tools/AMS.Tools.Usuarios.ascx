<%@ Control Language="c#" codebehind="AMS.Tools.Usuarios.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.Usuarios" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Avisos.Nuevo.png" border="0">
			</th>
			<td> 
            <p>
            <table id="Table" class="filtersIn">
            <tr>
            <td>
            Esta opción nos permite crear usuarios nuevos para nuestro sistema
                <asp:button id="crear" onclick="Ingresar_Crear" runat="server" Text="Crear" Width="131px"></asp:button>
                </td>
                </tr>
                </table>
                </p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Avisos.Modificar.png" border="0">
			</th>
			<td>
            <p>
            <table id="Table" class="filtersIn">
            <tr>
            <td>
				Escoja el Usuario que desea Modificar :&nbsp;&nbsp;
					<asp:dropdownlist id="ddlmodificar" class="dmediano" runat="server"></asp:dropdownlist>
				<asp:button id="btnModificar" runat="server" Text="Modificar" Width="122px" onclick="btnModificar_Click"></asp:button>
                </td>
                </tr>
                </table>
                </p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Avisos.Eliminar.png" border="0">
			</th>
			<td>
				<p>
            <table id="Table1" class="filtersIn">
            <tr>
            <td>
                Escoja el Usuario que desea Eliminar :&nbsp;&nbsp;
					<asp:dropdownlist id="usuario" class="dmediano" runat="server"></asp:dropdownlist>
				<asp:button id="Button1" onclick="Eliminar_Usuario" runat="server" Text="Eliminar" Width="122px"></asp:button>
                </td>
                </tr>
                </table>
                </p>
			</td>
		</tr>
	</tbody>
</table>
<p><asp:label id="lb" runat="server"></asp:label></p>
</fieldset>