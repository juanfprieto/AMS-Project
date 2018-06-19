<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Nomina.ActualizarSueldos.ascx.cs" Inherits="AMS.Nomina.AMS_Nomina_ActualizarSueldos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<fieldset>
Proceso General
<table id="Table" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<p>Sueldo Mínimo Actual:
				</p>
			</td>
			<td><asp:label id="lbSalMin" Runat="server"></asp:label></td>
            <td>Subsidio Transporte Actual: <asp:label id="lbSubsidioTra" Runat="server"></asp:label></td>
            <TD>Fecha Modificación: 
                <asp:textbox id="fechaModificacion" onkeyup="DateMask(this)" class="tmediano" runat="server"></asp:textbox>
            </TD>
	    </tr>
		<tr>
			<td>Sueldo Mínimo Nuevo:
			</td>
			<td><asp:textbox id="txtSalNew" Runat="server"></asp:textbox></td>
            <td>Subsidio de Transporte Nuevo:
			</td>
			<td><asp:textbox id="txtSubTraNew" Runat="server"></asp:textbox></td>
       		<td vAlign="middle" rowSpan="2"><asp:button id="btnGuardarAll" onclick="ActTodos" Runat="server" text="Enviar"></asp:button></td>
   	    </tr>
	</tbody>
</table>
<p></p>
Proceso Individual
<table id="Table1" class="filtersIn">
	<tbody>
            <td colspan= "2"><asp:dropdownlist OnChange="cambioEmpleadoJS(this);" id="ddlEmpleados" class="dgrande" Runat="server"></asp:dropdownlist></td>
		  <tr>
            <td><asp:label id="lbSalminEmpl" class="lpequeno" Runat="server"></asp:label></td>
			<td><asp:textbox id="txtSalNewEmpl" class="tpequeno" Runat="server"></asp:textbox></td>
             <TD>Fecha Modificacion: 
                <asp:textbox id="fechaModificacion2" onkeyup="DateMask(this)" class="tmediano" runat="server"></asp:textbox>
            </TD>
			<td><asp:button id="btnGuardar" onclick="ActEmpleado" Runat="server" text="Enviar"></asp:button></td>
		</tr>
	</tbody>
</table>
</fieldset>

<asp:Label id="lb" runat="server"></asp:Label>
<script language="javascript">
function cambioEmpleado_CallBack(response)
 {
	if(response.error!=null)
	{
		alert(response.error);
		return;
	}
	var sal=document.getElementById("<%=lbSalminEmpl.ClientID%>");
	sal.innerHTML=response.value;
 }

function cambioEmpleadoJS(obj)
{
	AMS_Nomina_ActualizarSueldos.cambioEmpleado(obj.value,cambioEmpleado_CallBack);
	
}

</script>
