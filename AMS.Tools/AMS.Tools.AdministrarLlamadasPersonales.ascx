<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.AdministrarLlamadasPersonales.ascx.cs" Inherits="AMS.Tools.AdministrarLlamadasPersonales" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="filters">
	<tbody>
		<tr>
        
			<th class="filterHead">
				<img height="70" src="..\img\AMS.Avisos.IngresarLlamada.png" border="0">
			</th>
			<td>
				<p>
					Seleccione el empleado&nbsp;para quien entró la llamada:<br />
					<asp:DropDownList id="ddlasesor" class="dmediano" Runat="server"></asp:DropDownList>
				
					<asp:Button id="btnAceptar" onclick="btnAceptar_Click" Runat="server" Text="Aceptar"></asp:Button>
				</p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
				<img height="70" src="..\img\AMS.Avisos.ConsultarLlamadas.png" border="0">
			</th>
			<td>
				<asp:Button id="btnConsultar" onclick="btnConsultar_Click" Runat="server" Text="Consultar mis Llamadas"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
