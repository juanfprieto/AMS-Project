<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.CausacionFacturasAdministrativas.Proveedor.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.CausacionFacturasEncabezadoProveedor" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script type ='text/javascript'>
	function Cambio_Prefijo(/*dropdownlist*/ obj)
	{
		if(obj.options.length>0)
		{
			CausacionFacturasEncabezadoProveedor.Cambio_Numero(obj.value,Cambio_Numero_CallBack);
		}
		else
		{
			alert('No hay prefijos de tipo Factura Proveedor');
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

    function  cargarFech(ob)
        {
            var num = document.getElementById("<%=nit.ClientID%>").value;
            return CausacionFacturasEncabezadoProveedor.ConsultarFecha(ob.value, num,  ConsultarFecha_CallBack);
        }

    function ConsultarFecha_CallBack(ob) 
    {
        if (ob.value != null) 
            {
                $("#<%=tbFecVen.ClientID%>").val(ob.value);
            }
            else 
            {
                alert("Seleccione un nit o verifique que el nit este registrado.");
            }
        }

        function Cargar_Nombre(obj) {
            CausacionFacturasEncabezadoProveedor.Cargar_Nombre(obj.value, Cargar_Nombre_CallBack);
        }

        function Cargar_Nombre_CallBack(response) {
            var respuesta = response.value;
            if (respuesta.Tables[0].Rows.length == 0 || respuesta.Tables[1].Rows.length == 0) {
                var ced = document.getElementById("<%=Nomnit.ClientID%>");
                ced.value = '';
            }
            else {
                var nombre = document.getElementById("<%=Nomnit.ClientID%>");
                if (respuesta.Tables[1].Rows.length != 0) {
                    if (respuesta.Tables[1].Rows[0].NOMBRE != '') {
                        nombre.value = respuesta.Tables[1].Rows[0].NOMBRE;
                    }
                }
            }
        }
</script>
<table id="Table2" class="filtersIn">
	<tbody>
		<tr>
			<td>Prefijo:</td>
			<td><asp:dropdownlist id="prefijoFactura" class="dmediano" runat="server" onChange="Cambio_Prefijo(this)"></asp:dropdownlist></td>
			<td>Número:</td>
			<td><asp:textbox id="numeroFactura" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorNumero" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="numeroFactura">*</asp:requiredfieldvalidator></td>
            <td><asp:Label ID="validaNumero" runat="server" Visible="false">*</asp:Label></td>
            <td></td>
		</tr>
		<tr>
			<td>Nit:</td>
			<td><asp:textbox id="nit" ondblclick="ModalDialog(this, 'SELECT MPRO.mnit_nit, MNIT.mnit_nit CONCAT \' - \' CONCAT MNIT.mnit_apellidos CONCAT \' \' CONCAT COALESCE(MNIT.mnit_apellido2,\'\') CONCAT \' \' CONCAT  MNIT.mnit_nombres CONCAT \' \' CONCAT COALESCE(MNIT.mnit_nombre2,\'\') AS NOMBRE FROM dbxschema.mnit MNIT, dbxschema.mproveedor MPRO WHERE MNIT.mnit_nit = MPRO.mnit_nit;', new Array(),1)"
					 onblur="Cargar_Nombre(this);" class="tpequeno" runat="server" ToolTip="Haga Doble Click para Iniciar la Busqueda"></asp:textbox><asp:requiredfieldvalidator id="validatorNit" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="nit">*</asp:requiredfieldvalidator></td>
			<td>Nombre:</td>
            <td>
            <asp:TextBox id="Nomnit" class="tpequeno" runat="server" ReadOnly="True"></asp:TextBox>
            </td>      
            <td>Almacén:</td>
            <td><asp:dropdownlist id="almacen"  class="dpequeno" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>Prefijo Factura Proveedor:</td>
			<td><asp:textbox id="prefijoProveedor" runat="server" maxlength="6" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorPProveedor" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="prefijoProveedor">*</asp:requiredfieldvalidator></td>
			<td>Número Factura Proveedor:</td>
			<td><asp:textbox id="numeroProveedor" runat="server" maxlength="8" class="tpequeno" ></asp:textbox>
            <asp:requiredfieldvalidator id="validatorNProveedor" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="numeroProveedor">*</asp:requiredfieldvalidator>
            <asp:regularexpressionvalidator id="regValNProveedor" runat="server" ErrorMessage="RegularExpressionValidator" ControlToValidate="numeroProveedor"
					ValidationExpression="\d+">*</asp:regularexpressionvalidator></td>
            <td></td>
            <td></td>
		</tr>
		<tr>
            <td>Fecha:</td>
			<td><asp:textbox id="fecha" onkeyup="DateMask(this)" onblur="cargarFech(this);" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="rfv1" runat="server" ErrorMessage="*" ControlToValidate="fecha" Display="Dynamic"></asp:requiredfieldvalidator></td>
			<td>Fecha de Vencimiento:</td>
			<td><asp:textbox id="tbFecVen" onkeyup="DateMask(this)" class="tpequeno" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv2" runat="server" ErrorMessage="*" ControlToValidate="tbFecVen" Display="Dynamic"></asp:requiredfieldvalidator>
            </td>	
            <td></td>
            <td></td>		
		</tr>
	    <tr>
			<td>Observación:</td>
			<td><asp:textbox id="observacion" class="amediano" runat="server" TextMode="MultiLine" MaxLength="1200"></asp:textbox></td>
			<td>Tipo Detalle:</td>
			<td><asp:dropdownlist id="tipoGasto" runat="server"></asp:dropdownlist></td>
            <td></td>
            <td></td>
		</tr>
		<tr>
			<td colspan="6" align="center" rowspan="3">
				<asp:Button ID="btnAceptar" Runat="server" Text="Aceptar" onclick="Cambiar_Gasto"></asp:Button>
                
			</td>
		</tr>
	</tbody>
</table>
