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
                N�mero de Pedido :<br />
                <asp:dropdownlist id="numeroPedido" runat="server" class="dmediano"></asp:dropdownlist>
 
            <p></p>
              Observaci�n o Causa de la Anulaci�n del Pedido: <br />
              <asp:TextBox ID="TextObserv" runat="server" TextMode="MultiLine" MaxLength="300" class="tgrande" Rows="4" style="margin-top: 0px"></asp:TextBox>
             
            <tr>
             <td>  
              <asp:button id="btnCancelar" onclick="CancelarPedido" Text="Cancelar Pedido" Runat="server" UseSubmitBehavior="false" 
              OnClientClick="clickOnce(this, 'Cargando...')"></asp:button>
             </td> 
            </tr>
            </p>
            </td>
        </tr>
         </TABLE>
    </fieldset>
</p>


<script language = "javascript">
 function clickOnce(btn, msg)
 {
            // Comprobamos si se est� haciendo una validaci�n
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se est� haciendo una validaci�n, volver si �sta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el bot�n sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Ser� el texto que muestre el bot�n mientras est� deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Procesando..'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
}
</script>
