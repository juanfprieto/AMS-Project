<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AgregarBusAfiliado.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_AgregarBusAfiliado" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="filtersIn">
		<tr>
			<td colSpan="2"><b>Por Favor Seleccione el Vehículo a Afiliar:</b>-
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa del Vehículo:</asp:Label>
			</td>
			<td ><asp:dropdownlist id="ddlplaca" Runat="server" Onchange="cargarPlacaDB(this)" Font-Size="XX-Small"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Vin Vehículo:</asp:Label>
			</td>
			<td>
				<asp:TextBox id="txtVin" runat="server" Font-Size="XX-Small"></asp:TextBox></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label14" runat="server" Font-Size="XX-Small" Font-Bold="True">Catálogo del Vehículo:</asp:Label>
			</td>
			<td>
				<asp:TextBox id="txtCatalogo" runat="server" Font-Size="XX-Small"></asp:TextBox></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label15" runat="server" Font-Size="XX-Small" Font-Bold="True">Propietario del Vehículo:</asp:Label>
			</td>
			<td>
				<asp:TextBox ReadOnly="True" id="ddlpropietario" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" class="tpequeno" Font-Size="XX-Small"></asp:TextBox>
				<asp:textbox id="ddlpropietarioa" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></td>
			<td></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label16" runat="server" Font-Size="XX-Small" Font-Bold="True">Asociado Cooperativa:</asp:Label>
			</td>
			<td>
				<asp:TextBox ReadOnly="True" id="ddlasociado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" Width="80px" Font-Size="XX-Small"></asp:TextBox>
				<asp:textbox id="ddlasociadoa" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></td>
			<td></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label17" runat="server" Font-Size="XX-Small" Font-Bold="True">Conductor Principal:</asp:Label>
			</td>
			<td>
				<asp:TextBox ReadOnly="True" id="ddlconductor" onclick="MostrarConductor(this);" runat="server"
					Width="80px" Font-Size="XX-Small"></asp:TextBox>
				<asp:textbox id="ddlconductora" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></td>
			<td></td>
		</tr>
		</TR>
		<tr>
			<td>
				<asp:Label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">2ª  Condcutor Principal:</asp:Label></td>
			<td>
				<asp:TextBox Font-Size="XX-Small" ReadOnly="True" id="SegundoConductor" runat="server" Width="80px"
					onclick="MostrarConductor(this);"></asp:TextBox>
				<asp:textbox id="SegundoConductora" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha de Ingreso:</asp:Label>
			</td>
			<td><asp:textbox id="FechaIngreso" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small"></asp:textbox></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label19" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero del Vehículo:</asp:Label>
			</td>
			<td><asp:textbox id="NumBus" Runat="server" Font-Size="XX-Small"></asp:textbox></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label20" runat="server" Font-Size="XX-Small" Font-Bold="True">Estado del Vehículo:</asp:Label>
			</td>
			<td><asp:dropdownlist id="ddlestado" Width="204px" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Comercial Vehiculo:</asp:Label></td>
			<td>
				<asp:TextBox id="ValVehiculo" runat="server" Font-Size="XX-Small" onKeyUp="NumericMask(this);"></asp:TextBox></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Categoria:</asp:Label></td>
			<td>
				<asp:DropDownList id="categoria" runat="server" Width="100px" Font-Size="XX-Small"></asp:DropDownList></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Capacidad Pasajeros:</asp:Label></td>
			<td>
				<asp:TextBox id="txtCapacidad" runat="server" Font-Size="XX-Small" onKeyUp="NumericMask(this);"
					MaxLength="3" Width="50px"></asp:TextBox>
            </td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label22" runat="server" Font-Size="XX-Small" Font-Bold="True">Potencia:</asp:Label></td>
			<td>
				<asp:TextBox id="txtPotencia" runat="server" Font-Size="XX-Small"></asp:TextBox>
            </td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Capacidad Combustible:</asp:Label></td>
			<td>
				<asp:TextBox id="txtCapacidadC" runat="server" Font-Size="XX-Small" onKeyUp="NumericMask(this);"></asp:TextBox></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Configuración asociada:</asp:Label>
			</td>
			<td><asp:dropdownlist id="ddlConfiguracion" Runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label21" runat="server" Font-Size="XX-Small" Font-Bold="True">Reposición del vehículo:</asp:Label>
			</td>
			<td>
				<asp:TextBox ReadOnly="True" id="txtReposicion" onclick="ModalDialog(this,'SELECT MBUSAFILIADO.MCAT_PLACA AS PLACA,MBUSAFILIADO.MBUS_NUMERO AS NUMERO from DBXSCHEMA.MBUSAFILIADO MBUSAFILIADO where MBUSAFILIADO.TESTA_CODIGO NOT IN (-1,0,3)', new Array(),1)"
					runat="server" Width="50px" Font-Size="XX-Small"></asp:TextBox></td>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label23" runat="server" Font-Size="XX-Small" Font-Bold="True">Observaciones:</asp:Label>
			</td>
			<td>
				<asp:TextBox id="txtObservaciones" runat="server" Width="250px" TextMode="MultiLine" Height="60px"
					Font-Size="XX-Small"></asp:TextBox></td>
			<td></td>
		</tr>
		<TR>
			<td align="center" colSpan="2" style="WIDTH: 545px"><asp:button id="btnGuardar" Runat="server" Text="Guardar" Font-Size="XX-Small" Font-Bold="True"
			UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
			</asp:button></td>
		</TR>
	</table>
</DIV>
<asp:Label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:Label>
<script language:javascript>
      function clickOnce(btn, msg)
        {
            // Comprobamos si se está haciendo una validación
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Loading...'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }

function cargarPlacaDB(Obj){
	var ddlplaca= document.getElementById("<%=ddlplaca.ClientID%>");	
	AMS_Comercial_AgregarBusAfiliado.CargarPlaca(Obj.value,CargarPlaca_Callback);
}
function CargarPlaca_Callback(response)
{
	var txtVin=document.getElementById("<%=txtVin.ClientID%>");
	var txtCatalogo=document.getElementById("<%=txtCatalogo.ClientID%>");
	var ddlpropietario=document.getElementById("<%=ddlpropietario.ClientID%>");
	var ddlasociado=document.getElementById("<%=ddlasociado.ClientID%>");
	var ddlconductor=document.getElementById("<%=ddlconductor.ClientID%>");
	var nomPropietario=document.getElementById("<%=ddlpropietarioa.ClientID%>");
	var nomAsociado=document.getElementById("<%=ddlasociadoa.ClientID%>");
	var nomConductor1=document.getElementById("<%=ddlconductora.ClientID%>");
	var nomConductor2=document.getElementById("<%=SegundoConductora.ClientID%>");
		var respuesta=response.value;
		if(respuesta.Tables[0].Rows.length>0)
		{
			txtVin.value=respuesta.Tables[0].Rows[0].VIN;
			txtCatalogo.value=respuesta.Tables[0].Rows[0].CATALOGO;
			ddlpropietario.value=respuesta.Tables[0].Rows[0].NIT;
			ddlasociado.value=respuesta.Tables[0].Rows[0].NIT;
			ddlconductor.value=respuesta.Tables[0].Rows[0].NIT;
			nomPropietario.value=respuesta.Tables[0].Rows[0].NOMNIT;
			nomAsociado.value=respuesta.Tables[0].Rows[0].NOMNIT;
			nomConductor1.value=respuesta.Tables[0].Rows[0].NOMNIT;
		}
		else
		{
			txtVin.value="";
			txtCatalogo.value="";
			ddlpropietario.value="";
			ddlasociado.value="";
			ddlconductor.value="";
			nomPropietario.value="";
			nomAsociado.value="";
			nomConductor1.value="";
			nomConductor2.value="";
			return;
		}	
		
}
function MostrarPersonal(obj,flt){
	var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
	ModalDialog(obj,sqlDsp, new Array(),1)
}
function MostrarConductor(obj){
	var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODICARGO=\'CO\';';
	ModalDialog(obj,sqlDsp, new Array(),1)
}
</script>
