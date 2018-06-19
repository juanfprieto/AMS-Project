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
	            vehículos a Clientes o de Proveedores, con fecha DIFERENTE al año y mes vigente ó actual, es 
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
            ></asp:Button>
        </p>
        <p>
	        <asp:Label id="lb" runat="server"></asp:Label>
        </p>
             
