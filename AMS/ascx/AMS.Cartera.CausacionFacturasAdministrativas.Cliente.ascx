<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.CausacionFacturasAdministrativas.Cliente.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.CausacionFacturasEncabezadoCliente" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script type ='text/javascript'>
	function Cambio_Prefijo(/*dropdownlist*/obj)
	{
		if(obj.options.length>0)
		{
			CausacionFacturasEncabezadoCliente.Cambio_Numero(obj.value,Cambio_Numero_CallBack);
		}
		else
		{
			alert('No hay prefijos de tipo Factura Cliente');
			return;
		} 
	}
	
	function Cambio_Numero_CallBack(response)
	{
		if (response.error != null)
		{
			alert(response.error);
			return;
		}
		var valor=response.value;
		var objNum=document.getElementById("<%=numeroFactura.ClientID%>");
		objNum.value=valor;
}
function cargar(ob) {
    return CausacionFacturasEncabezadoCliente.Consultar(ob.value, Consultar_CallBack);
}
function Consultar_CallBack(ob) {
    if (ob.value != null) {
        $("#<%=tbdiasPlazo.ClientID%>").val(ob.value);
    }
    else {
        alert("Seleccione un nit");
    }
}

    function cargaNitNombre(obj)
    {
        CausacionFacturasEncabezadoCliente.carga_Nombre(obj.value, cargaNitNombre_callBack);

    }
    function cargaNitNombre_callBack(response)
    {
        var respuesta = response.value;
        var nom = document.getElementById('<%=Nomnit.ClientID%>')
        nom.value = respuesta.Tables[0].Rows[0].NOMBRE
    }
</script>

<table id="Table1" class="filtersIn">
	<tbody>
        <tr>
			<td>Prefijo: </td>
			<td>
                <asp:dropdownlist id="prefijoFactura" class="dpequeno" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CambioPrefijo"></asp:dropdownlist>
			</td>
			<td>Número: </td>
			<td>
                <asp:Label ID="validatorNumero" runat="server" style="color:red;" Visible="false">*</asp:Label>
                <asp:textbox id="numeroFactura" runat="server" class="tpequeno"></asp:textbox>
                
			</td>
			<td>Fecha: </td>
			<td>
                <asp:textbox id="fecha" onkeyup="DateMask(this)" runat="server" Width="92px"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td>Nit: </td>
			<td>
                <asp:Label ID="validatorNit" runat="server" style="color:red;" Visible="false">*</asp:Label>
                <asp:textbox id="nit" onblur="cargaNitNombre(this);cargar(this);" ondblclick="ModalDialog(this, 'SELECT Mnit.mnit_nit, MNIT.mnit_nit CONCAT \' - \' CONCAT MNIT.mnit_apellidos CONCAT \' \' CONCAT COALESCE(MNIT.mnit_apellido2,\'\') CONCAT \' \' CONCAT MNIT.mnit_nombres CONCAT \' \' CONCAT COALESCE(MNIT.mnit_nombre2,\'\') AS NOMBRE FROM dbxschema.mnit MNIT WHERE MNIT.tvig_vigencia = \'V\' ORDER BY MNIT_APELLIDOS;', new Array(),1)"
					runat="server" Width="130px" ToolTip="Haga Doble Click para Iniciar la Busqueda"></asp:textbox>
			</td>
            <td>Nombre: </td>
            <td>
                <asp:TextBox id="Nomnit" style="width:190px" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
			<td>Almacen: </td>
			<td>
                <asp:dropdownlist id="almacen" runat="server"></asp:dropdownlist>
			</td>
			
		</tr>
		<tr>
            <td>Vendedor: </td>
			<td>
                <asp:dropdownlist id="vendedor" class="dpequeno" runat="server"></asp:dropdownlist>
			</td>
			<td>Dias de Plazo: </td>
			<td>
                <asp:textbox id="tbdiasPlazo" class="tpequeno" Runat="server" Text="0" CssClass="AlineacionDerecha">0</asp:textbox>
                <asp:requiredfieldvalidator id="rfv2" runat="server" ControlToValidate="tbdiasPlazo" ErrorMessage="*">

                </asp:requiredfieldvalidator>
                <asp:regularexpressionvalidator id="rev1" runat="server" ControlToValidate="tbdiasPlazo" ErrorMessage="Campo Numérico"
					ValidationExpression="\d+" Display="Dynamic"></asp:regularexpressionvalidator>
			</td>
			<td>Observación: </td>
			<td>
                <asp:textbox id="observacion" runat="server" TextMode="MultiLine" MaxLength="1200"></asp:textbox>
			</td>
			
		</tr>
        <tr>
            <td>Tipo Detalle: </td>
			<td>
                <asp:dropdownlist id="tipoGasto" class="dpequeno" runat="server"></asp:dropdownlist>
			</td>
        </tr>
		<tr>
			<td align="center" colspan="6">
                <asp:button id="btnAceptar" Runat="server" Text="Aceptar" onclick="Cambiar_Gasto"></asp:button>
			</td>
		</tr>
	</tbody>
</table>

