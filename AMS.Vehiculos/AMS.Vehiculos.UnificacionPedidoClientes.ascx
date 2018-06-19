<%@ Control Language="c#" codebehind="AMS.Vehiculos.UnificacionPedidoClientes.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.UnificacionPedidoClientes" %>

<fieldset>        
	    <TABLE id="Table1" class="filtersIn">
		    <TR>
              <td>

            <p align="justify">
	           Durante este proceso tomara un pedido de cliente que se encuentre en estado 
	creado y pasara todos los anticipos de dinero a otro pedido en estado creado o 
	asignado. El pedido de origen se colocara en estado cancelado.
            </p>
               </td>
             </TR> 
	    </TABLE>
</fieldset>
<p>
</p>

<fieldset>        
	    <TABLE id="Table2" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<fieldset>
                    <p>
					</p>
                    <legend>Pedido de Origen</legend>Prefijo del Pedido : 
                    <asp:DropDownList id="prefijoPedidoOrigen" runat="server" class="dmediano" OnSelectedIndexChanged="Cambio_Tipo_Documento_Origen"
						AutoPostBack="true"></asp:DropDownList>
                    <p>
						Número del Pedido :
						<asp:DropDownList id="numeroPedidoOrigen" runat="server" class="dmediano" OnSelectedIndexChanged="Cambio_Numero_Documento_Origen"
							AutoPostBack="true"></asp:DropDownList>
					</p>
                    <p>
						Nit Cliente Principal :&nbsp;
						<asp:Label id="nitPrincipalOrigen" class="lmediano" runat="server"></asp:Label>
					</p>
                    <p>
						Nombre Cliente Principal :
						<asp:Label id="nombrePrincipalOrigen" class="lmediano" runat="server"></asp:Label>
					</p>
                    <p>
						Nit Cliente Opcional :
						<asp:Label id="nitOpcionalOrigen" class="lmediano" runat="server"></asp:Label>
					</p>
                    <p>
						Nombre Cliente Opcional :
						<asp:Label id="nombreOpcionalOrigen" class="lmediano" runat="server"></asp:Label>
					</p>
                    <p>
						Catalogo Automovil :
						<asp:Label id="catalogoOrigen" class="lmediano" runat="server"></asp:Label>
					</p>
                </fieldset>
			</td>


			<td>
				<fieldset>
                    <legend>Pedido de Destino</legend>Prefijo del Pedido :&nbsp; &nbsp;<asp:DropDownList id="prefijoPedidoDestino" runat="server" class="dmediano" OnSelectedIndexChanged="Cambio_Tipo_Documento_Destino"
						AutoPostBack="true"></asp:DropDownList>
                    <p>
						Número del Pedido :&nbsp;
						<asp:DropDownList id="numeroPedidoDestino" runat="server" class="dmediano" OnSelectedIndexChanged="Cambio_Numero_Documento_Destino"
							AutoPostBack="true"></asp:DropDownList>
					</p>
                    <p>
						Nit Cliente Principal :
						<asp:Label id="nitPrincipalDestino" class="lmediano" runat="server"></asp:Label>
					</p>
                    <p>
						Nombre Cliente Principal :
						<asp:Label id="nombreClientePrincipal" class="lmediano" runat="server"></asp:Label>
					</p>
                    <p>
						Nit Cliente Opcional :
						<asp:Label id="nitOpcionalDestino" class="lmediano" runat="server"></asp:Label>
					</p>
                    <p>
						Nombre Cliente Opcional :
						<asp:Label id="nombreOpcionalDestino" class="lmediano" runat="server"></asp:Label>
					</p>
                    <p>
						Catalogo Automovil :
						<asp:Label id="catalogoDestino" class="lmediano" runat="server"></asp:Label>
					</p>
                </fieldset>
			</td>
		</tr>
	</tbody>
</table>
<p>
	<asp:Button id="botonUnificar" onclick="Unificar_Pedidos" runat="server" Text="Unificar Pedidos"></asp:Button>
</p>
</fieldset>  

<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
