<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.AnulacionPedidoClientes.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_AnulacionPedidoClientes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<p>
 <fieldset>
   <legend>Seleccione el pedido a Anular:</legend>
        <TABLE id="Table1" class="filtersIn">
        <tr>
        <td>
            <p>            
            Prefijo del Pedido: <br />
	            <asp:dropdownlist id="prefijoDocumento" runat="server" class="dmediano" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Prefijo"></asp:dropdownlist>
	             <p></p>
                Número de Pedido :<br />
                <asp:dropdownlist id="numeroPedido" runat="server" class="dmediano"></asp:dropdownlist>
 
            <p></p>
              Observación o Causa de la Anulación del Pedido: <br />
              <asp:TextBox ID="TextObserv" runat="server" TextMode="MultiLine" MaxLength="300" class="tgrande" Rows="4" style="margin-top: 0px"></asp:TextBox>
             
            <tr>
             <td>  
              <asp:button id="btnCancelar" onclick="CancelarPedido" Text="Cancelar Pedido" Runat="server" UseSubmitBehavior="false"></asp:button>
             </td> 
            </tr>
            </p>
            </td>
        </tr>
         </TABLE>
    </fieldset>
</p>


