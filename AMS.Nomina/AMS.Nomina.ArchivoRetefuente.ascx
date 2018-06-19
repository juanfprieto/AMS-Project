<%@ Control Language="c#" codebehind="AMS.Nomina.ArchivoRetefuente.cs" autoeventwireup="false" Inherits="AMS.Nomina.ArchivoRetefuente" %>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p align="justify">
	El Archivo de Excel que esta subiendo debe tener las siguientes caracteristicas 
	:
</p>
<p align="justify">
	a) Los campos deben ir en el siguiente orden :
</p>
<p align="center">
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					1.</td>
				<td>
					Inicio</td>
			</tr>
			<tr>
				<td>
					2.</td>
				<td>
					A</td>
			</tr>
			<tr>
				<td>
					3.</td>
				<td>
					Final</td>
			</tr>
			<tr>
				<td>
					4.</td>
				<td>
					Porcentaje</td>
			</tr>
			<tr>
				<td>
					5.</td>
				<td>
					Valor a Retener</td>
			</tr>
		</tbody>
	</table>
</p>
<p align="justify">
	b) El rango de celdas debe estar definido con el nombre de&nbsp;TablaRetefuente 
	(Menu Insertar/Nombre Opción Definir), ademas debe tener en cuenta que la 
	primer fila de su selección es el titulo del campo ej. Inicio,A,Final, etc; Si 
	no existe esa fila, definala o seleccione una fila por arriba vacia.
</p>
<p align="justify">
	Mayor informacion : <a href="http://www.ecasltda.com" target="blank">Visite Nuestra 
		Pagina</a>
</p>
<p>
	Archivo A Subir : <input id="fDocument" type="file" runat="server">&nbsp;&nbsp;&nbsp;
	<asp:Button id="btnAceptar" onclick="AceptarArchivo" runat="server" Text="Aceptar"></asp:Button>
</p>
<p>
</p>
<p>
	<asp:DataGrid id="DataGridRfte" runat="server"></asp:DataGrid>
</p>
</td></tr></table>
</fieldset>
