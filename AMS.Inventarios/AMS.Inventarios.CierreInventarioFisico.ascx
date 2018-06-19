<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.CierreInventarioFisico.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_CierreInventarioFisico" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<script type ="text/javascript">

function ValSelInvFis(){if(document.getElementById('<%=ddlInventarios.ClientID%>').value == ''){alert('Seleccione un inventario físico para mirar la observación!');return false;return confirm('Desea realizar el cierre de este inventario físico?');}}

</script>

<fieldset>
    <legend>Inventario Físico</legend>
	<table id="Table1" class="filtersIn">
    
		<tr>        
			<td>Seleccione el Inventario:   <br />
                    <asp:dropdownlist id="ddlInventarios" class="dmediano" runat="server"></asp:dropdownlist>
            </td>            
            
		</tr>
        
		<tr>
			<td colspan="2"><asp:button id="btnCerrar" runat="server" Text="Cerrar Inventario Físico" onclick="btnCerrar_Click" ></asp:button>&nbsp;
				<asp:button id="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" onclick="btnCancelar_Click"></asp:button>
            </td>
		</tr>
	</table>
    <p>
		<asp:Label id="lbInfo" runat="server"></asp:Label>
	</p>
</fieldset>



