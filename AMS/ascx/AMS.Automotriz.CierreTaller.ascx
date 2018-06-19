<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.CierreTaller.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_CierreTaller" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<P>El Cierre Mensual o Administrativo de Taller, es el procedimiento para finalizar 
	la actividad de servicio del mes vigente y&nbsp;no&nbsp;permitir la creación de 
	ordenes de trabajo&nbsp;ni facturación&nbsp;a clientes con fecha diferente al 
	año y mes vigente.</P>
<TABLE id="Table1">
	<TR>
		<TD>Mes Vigente:
			<asp:Label id="mesVigente" runat="server"></asp:Label>&nbsp; Año Vigente:&nbsp;
			<asp:Label id="anoVigente" runat="server"></asp:Label>&nbsp;</TD>
	</TR>
</TABLE>
</td>
</tr>
</table>

<P>&nbsp;
	<asp:Button id="btnProceso" runat="server" Text="Realizar Proceso" onclick="btnProceso_Click"></asp:Button>
</P>
<P>
	<asp:Label id="lb" runat="server"></asp:Label></P>
    </fieldset>
