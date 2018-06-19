<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.DevolucionFacturaCliente.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_DevolucionFacturaCliente" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type="text/javascript" src="../js/jquery.js"></script>
<script type="text/javascript" src="../js/jquery-ui.js"></script>
<script type="text/javascript" src="../js/ui/jquery.ui.datepicker.js" ></script>
<script type ="text/javascript" src="../js/generales.js"></script>
<script type="text/javascript" src="../js/modernizr.js"></script>

<script type ="text/javascript">
	function RealizarDevolucion()
	{
		document.getElementById('btnDevPed').disabled = "locked";
		document.getElementById('<%=oprimioBtnDevPed.ClientID%>').value = "true";
		document.getElementById('Form').submit();
	}
    $(function () {
        //mirar documentacion JQueryUI sobre datepicker
        var fechaVal = $(".calendario").val();
        $(".calendario").datepicker();
        $(".calendario").datepicker("option", "dateFormat", "yy-mm-dd");
        $(".calendario").datepicker("option", "showAnim", "slideDown");
        $(".calendario").val(fechaVal);
    });


</script>
<fieldset>        
	    <TABLE id="Table1" class="filtersIn">
		    <TR>
              <td>       
              <br>    
                    <TABLE id="Table2" class="filtersIn">
                <tbody>
                <tr>
                <td>Prefijo Pedido : <br />
                <asp:dropdownlist id="prefijoPedidoOtro" runat="server" class="dmediano" OnSelectedIndexChanged="Cambio_Tipo_Documento_Otros"
                AutoPostBack="true"></asp:dropdownlist></td>
                <td>Número del Pedido :<br />
<asp:dropdownlist id="numeroPedidoOtro" class="dpequeno" runat="server"></asp:dropdownlist><asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image></td>
		</tr>
		<tr>
			<td>VIN:<br />
				<asp:TextBox ID="txtVIN" class="tmediano" Runat="server"></asp:TextBox>
			</td>
			<td>
				<asp:Button ID="btnSeleccionar" Runat="server" Text="Seleccionar" onclick="btnSeleccionar_Click"></asp:Button>
			</td>
            </td>
		</tr>            
	    </TABLE>
</fieldset>

<br>
<asp:Panel ID="pnlDevolver" Visible="False" Runat="server">
	<TABLE id="Table3" class="filtersIn">
		<tbody>
			<tr><td><asp:Label ID="lblDatosPedido" Runat="server"></asp:Label></td></tr>
            <tr><td><textarea  id="TextArea1" class="tmediano" runat="server" ></textarea></td></tr>
		</tbody>
	</table>
	<br>
	<TABLE id="Table4" class="filtersIn">
		<TR>
			<TD>Prefijo Nota Devolución Cliente :</TD>
			<TD>
				<asp:dropdownlist id="prefijoDevoluciones" runat="server"></asp:dropdownlist></TD>
		</TR>
        <TR>
			<TD>Prefijo Nota Débito Cliente :</TD>
			<TD>
				<asp:dropdownlist id="ddlNotaDebito" runat="server"></asp:dropdownlist></TD>
		</TR>
        
        <tr>
        		<td>Fecha de la Devolución :
					<asp:textbox id="fechNota" runat="server" class="calendario" Width="85px"
						ReadOnly="true"></asp:textbox><asp:requiredfieldvalidator id="validatorFechFac" runat="server" ControlToValidate="fechNota" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorFechFac2" runat="server" ControlToValidate="fechNota" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator></td>
		</tr>

         <p align="justify">
	            <i><strong>NOTA</strong></i>: Para realizar este proceso, usted DEBE TENER fisicamente todos los formularios originales
                emitidos y con la observación de la anulación escrita con su puño y letra sobre esos formularios.
                De presentarse la opción que alguno de estos formularios aún esté en poder de terceros, usted 
                y la empresa pueden contraer grandes problemas si el vehículo tiene algún trámite con esos formularios.
         </p> 

		<TR>
			<TD>
				<P><INPUT id="btnDevPed" onclick="this.disabled=true;RealizarDevolucion();" type="button" value="Realizar Devolución"
						name="btnDevPed"> <INPUT id="oprimioBtnDevPed" type="hidden" value="false" name="oprimioBtnDevPed" runat="server"></P>
			</TD>
			<TD></TD>
		</TR>
	</TABLE>
</asp:Panel>
<asp:label id="lb" runat="server"></asp:label>
