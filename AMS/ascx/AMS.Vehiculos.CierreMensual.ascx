<%@ Control Language="c#" codebehind="AMS.Vehiculos.CierreMensual.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.CierreMensual" %>
<
<fieldset>        
	    <TABLE id="Table1" class="filtersIn">
		    <TR>
              <td>
            <p align="center">
	            Cierre Mensual de Vehiculos
            </p>
            <p align="justify">
	            El Cierre Mensual o Administrativo de Vehículos, es el procedimiento para 
	            finalizar la actividad comercial del mes vigente y NO PERMITIR FACTURAR 
	            vehículos a clientes, con fecha DIFERENTE al año y mes vigente ó actual; es 
	            importantísimo que usted haya realizado TODOS los PROCESOS del MES 
	            antes de ejecutar este proceso
            </p> 
               </td>
             </TR>
	    </TABLE>
</fieldset>

<fieldset>        
	    <TABLE id="Table2" class="filtersIn">
		    <TR>
               <td>
				   Mes Vigente:
	          <asp:Label id="mesVigente" runat="server"></asp:Label>&nbsp; Año Vigente:&nbsp;<asp:Label id="anoVigente" runat="server"></asp:Label>&nbsp;</td>
                       </td>               
            </TR>
	    </TABLE>
</fieldset>
        <p>
            &nbsp;<asp:Button id="btnProceso" OnClick="Realizar_Proceso" runat="server" Text="Realizar Proceso" UseSubmitBehavior="false" 
            OnClientClick="clickOnce(this, 'Cargando...')"></asp:Button>
        </p>
        <p>
	        <asp:Label id="lb" runat="server"></asp:Label>
        </p>
             
<script language:javascript>
    function clickOnce(btn, msg) {
        // Comprobamos si se está haciendo una validación
        if (typeof (Page_ClientValidate) == 'function') {
            // Si se está haciendo una validación, volver si ésta da resultado false
            if (Page_ClientValidate() == false) { return false; }
        }

        // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
        if (btn.getAttribute('type') == 'button') {
            // El atributo msg es totalmente opcional. 
            // Será el texto que muestre el botón mientras esté deshabilitado
            if (!msg || (msg = 'undefined')) { msg = 'Procesando..'; }

            btn.value = msg;

            // La magia verdadera :D
            btn.disabled = true;
        }

        return true;
    }
</script>