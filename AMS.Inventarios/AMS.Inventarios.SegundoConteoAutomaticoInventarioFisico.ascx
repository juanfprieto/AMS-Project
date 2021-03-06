<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.SegundoConteoAutomaticoInventarioFisico.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_SegundoConteoAutomaticoInventarioFisico" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<script type ="text/javascript">

function ValSelInvFis(){if(document.getElementById('<%=ddlInventarios.ClientID%>').value == ''){alert('Seleccione un inventario f�sico para mirar la observaci�n!');return false;return confirm('Desea realizar el cierre de este inventario f�sico?');}}

</script>

<fieldset>
    <legend>Inventario F�sico</legend>
	<table id="Table1" class="filtersIn">
    
		<tr>        
			<td>Seleccione el Inventario:   <br />
                    <asp:dropdownlist id="ddlInventarios" class="dmediano" runat="server"></asp:dropdownlist>
            </td>            
        </tr>
        
        <tr>         
			<td colspan="2">El proceso de Segundo Conteo Autom�tico de inventario f�sico genera el SEGUNDO CONTEO IGUAL al PRIMER CONTEO y CALCULA las DIFERENCIAS del Inventario F�sico:   <br />
            </td>            
      	</tr>
		
        <tr>
			<td colspan="2"><asp:button id="btnCerrar" runat="server" Text="Realizar Segundo Conteo Autom�tico Inventario F�sico" onclick="btnCerrar_Click" ></asp:button>&nbsp;
				<asp:button id="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" onclick="btnCancelar_Click"></asp:button>
            </td>
		</tr>
	</table>
    <p>
		<asp:Label id="lbInfo" runat="server"></asp:Label>
	</p>
</fieldset>



