<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.PedidoClienteMayor.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_PedidoClienteMayor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
				<img height="50" src="../img/AMS.Flyers.Nueva.png" border="0"></td>
			<th>
            <td>
				<p>
					Tipo de Documento Prefijo :
					<asp:DropDownList id="tipoDocumento" runat="server" class="dmediano"></asp:DropDownList>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:Button id="btnNuevo" onclick="Nuevo_Pedido" runat="server" class="bpequeno" Text="Nuevo"></asp:Button>
				</p>
				<p>
					<font style="BACKGROUND-COLOR: #f2f2f2">Generacion de Consecutivo&nbsp;: </font>
					&nbsp;
					<asp:RadioButtonList id="opcion" runat="server" RepeatDirection="Horizontal">
						<asp:ListItem Value="A" Selected="True">Autom&#225;tico</asp:ListItem>
						<asp:ListItem Value="M">Manual</asp:ListItem>
					</asp:RadioButtonList>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				</p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
				<img height="50" src="../img/AMS.Flyers.Edits.png" border="0"></td>
			<th>
            <td>
				<p>
					Tipo de Documento :
					<asp:DropDownList id="tipoDocumentoEdit" runat="server" class="dmediano" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Documento"></asp:DropDownList>
					&nbsp;&nbsp; <br />Número :
					<asp:DropDownList id="numeroEdicion" runat="server" class="dmediano"></asp:DropDownList>
				</p>
				<p>
					<asp:Button id="btnEdit" onclick="Editar_Pedido" runat="server" Text="Editar"></asp:Button>
					&nbsp;
					<asp:Button id="btnDel" onclick="Eliminar_Pedido" runat="server" Text="Borrar"></asp:Button>
				</p>
			</td>
		</tr>
	</tbody>
</table>
</fieldset>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
