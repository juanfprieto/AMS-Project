<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Inventarios.CierrePedidos.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_CierrePedidos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<Table id="Table2" class="filtersIn">  
<tbody>
<tr>
<td>
    El Cierre Mensual o Administrativo de Pedidos, es el procedimiento para eliminar 
	los pedidos vencidos que no tengan listas de empaque en proceso de alistamiento en la bodega y que la fecha del pedido
    sea menor a la fecha de hoy menos los meses de estadia parametrizado en los tipos de pedido del sistema.
    <br />
    <br />
    Por favor, asegurese que los meses de vigencia estén actualizados en el parámetro de Tipo de Pedido
    <br />
    <br />
     Usted puede ejecutar este proceso en cualquier momento, se recomienda que se ejecuta obligatoriamente al cierre del mes
        y siempre antes de realizar la Asignación Automática de Back_Order a los pedidos pendientes de los clientes
</td>
</tr>
</tbody>
</Table>
    <p> </p>
<TABLE id="Table1" class="filtersIn">
	<TR>
		<TD>Mes Vigente:
			<asp:Label id="mesVigente" runat="server"></asp:Label>&nbsp; Año Vigente:&nbsp;
			<asp:Label id="anoVigente" runat="server"></asp:Label>&nbsp;</TD>
	</TR>
</TABLE>
<p></p>
<P>
	<asp:Button id="btnProceso" runat="server" Text="Realizar Proceso" onClick="btnProceso_Click" OnClientClick="espera();"></asp:Button></P>
<P>
	<asp:Label id="lb" runat="server"></asp:Label></P>
</fieldset>