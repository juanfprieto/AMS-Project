<%@ Control Language="c#" codebehind="AMS.Inventarios.ListaPrecios.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.AdminListasPrecios" %>


<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
				<img height="60" src="../img/AMS.Flyers.News.png" border="0">
            </th>
			<td>
                <fieldset>
				<p>
					Opción que permite la creación de una lista de precios para items:<br>
					<asp:Button id="btnIngresar1" onclick="IngresarNuevo" Text="Ingresar" runat="server"></asp:Button>
				</p>
			</td>
            </fieldset>
		</tr>
		<tr> 
			<th class="filterHead">
				<img height="80" src="../img/AMS.Flyers.Asignar.png" border="0">
            </th>
			<td>
				<fieldset>
					<legend>Asignación por Formula</legend>
					<p>
						Aqui podemos asignar, aumentar o disminuir los valores de una lista de precios:
					</p>
					<p>
						<asp:DropDownList id="ddlListasEdit" class="dmediano" runat="server"></asp:DropDownList>
						<br />
						<asp:Button id="btnIngresar2" onclick="AsignarPrecios" Text="Ingresar" runat="server"></asp:Button>
					</p>
				</fieldset>
				<fieldset>
                    <p>
					</p>
                    <legend>Asignación Por Archivo</legend>Dentro de esta opción se cargara un archivo
                    de excel donde se encuentren los datos de la lista de precios. 
                    <p>
						Por favor tenga en cuenta la estructura del archivo :&nbsp;&nbsp;&nbsp;&nbsp;<br />
                        <asp:DropDownList id="ddlListasFile" class="dmediano" runat="server"></asp:DropDownList>
						<br />
						<asp:Button id="btnIngresar3" onclick="IngresarArchivo" Text="Ingresar" runat="server"></asp:Button>
					</p>
                </fieldset>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
				<img height="70" src="../img/AMS.Flyers.Eliminar.png" border="0">
            </th>
			<td>
                <fieldset>
				<p>
					Seleccione la lista de precios a eliminar :<br>				
					<asp:DropDownList id="ddlListasDelete" class="dmediano" runat="server"></asp:DropDownList>
					<br />
					<asp:Button id="btnEliminar" onclick="EliminarLista" Text="Eliminar" runat="server"></asp:Button>
				</p>
                </fieldset>
			</td>
		</tr>
	</tbody>
</table>
<asp:Label id="lb" runat="server"></asp:Label>
