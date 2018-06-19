<%@ Control Language="c#" codebehind="AMS.Vehiculos.PedidoProveedor.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.CPedidoProveedor" %>
<table class="filters">
	<tbody>
		<tr>
			<th   class="filterHead">
				<img height="50" src="../img/AMS.Flyers.Nueva.png" border="0">
            </th>
			   <td>
                 <fieldset> 
				<p>
					Tipo de Documento Prefijo:
					<asp:DropDownList id="tipoDocumento" runat="server" class="dmediano"></asp:DropDownList>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:Button id="btnNuevo" onclick="Nuevo_Pedido" runat="server" Width="109px" Text="Nuevo"></asp:Button>
				</p>
				<p>
					Generacion de Consecutivo:
					&nbsp;
					<asp:RadioButtonList id="opcion" runat="server" RepeatDirection="Horizontal">
						<asp:ListItem Value="A" Selected="True">Autom&#225;tico</asp:ListItem>
						<asp:ListItem Value="M">Manual</asp:ListItem>
					</asp:RadioButtonList>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				</p>
                </fieldset>
			</td>
		</tr>
		<tr>
			<th  class="filterHead">
				<img height="50" src="../img/AMS.Flyers.Edits.png" border="0">
             </th>
			<td>
            <fieldset>
				<p>
					Tipo de Documento:<br>
					<asp:DropDownList id="tipoDocumentoEdit" runat="server" class="dmediano" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Documento"></asp:DropDownList>
					<br>
                     Número:<br>
					<asp:DropDownList id="numeroEdicion" runat="server" class="dpequeno"></asp:DropDownList>
				</p>
				<p>
					<asp:Button id="btnEdit" onclick="Editar_Pedido" runat="server" Text="Editar"></asp:Button>
					&nbsp;
					<asp:Button id="btnDel" onclick="Eliminar_Pedido" runat="server" Text="Borrar"></asp:Button>
				</p>
                </fieldset>
			</td>
		</tr>
	</tbody>
</table>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
