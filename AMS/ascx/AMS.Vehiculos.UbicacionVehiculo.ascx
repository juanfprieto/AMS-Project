<%@ Control Language="c#" codebehind="AMS.Vehiculos.UbicacionVehiculo.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.UbicacionVehiculo" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type="text/javascript">
function abrirEmergente(obj) {
    var ddlCatalogo = document.getElementById('_ctl1_catalogo');
    var ddlVIN = document.getElementById('_ctl1_' + obj);
    ModalDialog(ddlVIN, 'SELECT mveh_inventario, MV.mcat_vin concat \'   \' concat pc.pcat_descripcion  concat \'   \' concat mcat_motor concat \'   \' concat test_nombesta AS VIN FROM MVEHICULO MV, MCATALOGOVEHICULO MC, pcatalogovehiculo pc, testadovehiculo te WHERE MV.mcat_vin = MC.mcat_vin AND mv.test_tipoesta IN (10,20,30,40) and mc.pcat_codigo = pc.pcat_codigo and mv.test_tipoesta = te.test_tipoesta ORDER BY MV.mcat_vin;', new Array(), 1);
}
</script>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
           <th class="filterHead">
            <IMG height="80" src="../img/AMS.Flyers.Ubicacion.png" border="0"> 
           </th>
           <td>
				<%--<p>Seleccione el Catalogo del Vehiculo : <br /><asp:dropdownlist id="catalogo" AutoPostBack="true" class="dmediano" OnSelectedIndexChanged="Cambio_Catalogo" runat="server"></asp:dropdownlist>
                </p>
                 --%>
				<p>Seleccione el Vin del Vehículo :
                <br> <asp:dropdownlist id="vinVehiculo" AutoPostBack="true" OnSelectedIndexChanged="Cambio_VIN" class="dmediano" runat="server"></asp:dropdownlist>
                <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('vinVehiculo');"></asp:Image>
               </p>
               <p>Ubicacion Actual del Vehículo:&nbsp;
                <br/>
                 <asp:textbox id="ubicacionActual" class="tgrande" runat="server"></asp:textbox>
                </p>
			
				<p>Nueva Ubicación del Vehículo : 
                <br>
                <asp:dropdownlist id="ubicacionVehiculo" class="dgrande" runat="server"></asp:dropdownlist>              
               </p>
				<p>Nombre de la Persona que Autoriza : <br /><asp:textbox id="nombreAutoriza" class="tmediano" runat="server"></asp:textbox><asp:requiredfieldvalidator id="validatorAutoriza" runat="server" ControlToValidate="nombreAutoriza" Display="Dynamic" Font-Name="Arial" Font-Size="11">*
                    </asp:requiredfieldvalidator>
                </p>
				<p>Nombre de la Persona que Transporta : <br /><asp:textbox id="nombreTransporta" class="tmediano" runat="server"></asp:textbox><asp:requiredfieldvalidator id="validatorTransporta" runat="server" ControlToValidate="nombreTransporta" Display="Dynamic" Font-Name="Arial" Font-Size="11">*
                    </asp:requiredfieldvalidator>
                </p>
				<p>Fecha Traslado a Ubicación :&nbsp;
                <br/>
                 <asp:textbox id="calendar" onkeyup="DateMask(this)" class="tmediano" runat="server"></asp:textbox>
                </p>
				<p><asp:button id="btnConfirmar" onclick="Confirmar_Ubicacion" runat="server" Text="Confirmar" ></asp:button>
                </p>
            </td>	
       </tr>
     </tbody>
</table>
<p><asp:label id="lb" runat="server"></asp:label></p>
</fieldset>
<script language = "javascript">
 function clickOnce(btn, msg)
 {
            // Comprobamos si se está haciendo una validación
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Procesando..'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
}
</script>
