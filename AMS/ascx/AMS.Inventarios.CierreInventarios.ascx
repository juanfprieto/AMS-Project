<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.CierreInventarios.ascx.cs" Inherits="AMS.Inventarios.CierreInventarios" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<fieldset> 
<table id="Table" class="filtersIn">
	<tbody>
    <tr>
    <td>        
                    <p>
                        El Cierre Mensual o Administrativo de Inventarios, es el procedimiento para 
	                    finalizar la actividad de los Items en el mes vigente 
	                    y NO permitir la creación de pedidos ni 
	                    facturación con fecha diferente al año y mes vigente.</p>

	                <p> Usted debe también ejecutar el proceso de Cierre de Pedidos por la opción indicada.</p>
      </td> 
      </tr>
      <tr>     
		       <br>
               <td>
                        Mes Vigente:
			            <asp:Label id="mesVigente" runat="server"></asp:Label>&nbsp; Año Vigente:&nbsp;
			            <asp:Label id="anoVigente" runat="server"></asp:Label><br><br>
                        <asp:Button id="btnProceso" runat="server" Text="Realizar Proceso" onclick="btnProceso_Click" onClientclick="espera();"></asp:Button>
                         </td>
   </tr>
    </tbody>
  </table>
</fieldset>
     
<p><asp:Label id="lb" runat="server"></asp:Label></p>

