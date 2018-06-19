<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.Documentos.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_Documentos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<table>
	<tr>
		<td colSpan="2">Por Favor Seleccione el Vehículo:
		</td>
	</tr>
	<tr>
		<td>Catalogo
		</td>
		<td><asp:dropdownlist id="ddlcatalogo" Runat="server" Onchange="cargarVinsJS(this)"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Vin
		</td>
		<td><asp:dropdownlist id="ddlVin" Runat="server" Onchange="cargarDatosJS2(this)"></asp:dropdownlist>
			<asp:Button Runat="server" ID="btn_reporte" OnClick="gen_rpt" Text="Generar Acta"></asp:Button>
		</td>
	</tr>
	<tr>
		<td>Codigo del Documento
		</td>
		<td><asp:dropdownlist id="ddlCodDoc" Runat="server" Onchange="cargarDatosJS(this) "></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Número del Documento
		</td>
		<td><asp:textbox id="txtNumDoc" Runat="server"></asp:textbox></td>
	</tr>
	<tr>
		<td>Fecha del Documento
		</td>
		<td><asp:textbox id="txtFechaDoc" onkeyup="DateMask(this)" Runat="server"></asp:textbox>
            <asp:RequiredFieldValidator id="rfvFechaDoc" runat="server" ControlToValidate="txtFechaDoc" ErrorMessage="La fecha del Doc. es un dato necesario"></asp:RequiredFieldValidator>
    </td>
	</tr>
	<tr>
		<td>Valor del Documento
		</td>
		<td><asp:textbox id="txtValoDoc" Runat="server"></asp:textbox></td>
	</tr>
	</TR>
	<tr>
		<td>Fecha de Vencimiento Documento
		</td>
		<td><asp:textbox id="txtFechaVencDoc" onkeyup="DateMask(this)" Runat="server"></asp:textbox>
            <asp:RequiredFieldValidator id="rfvFechaVencDoc" runat="server" ControlToValidate="txtFechaVencDoc" ErrorMessage="La fecha de Venc. del Doc. es un dato necesario"></asp:RequiredFieldValidator>
        </td>
	</tr>
	<tr>
		<td>Observaciones
		</td>
		<td><asp:textbox id="txtObserva" Runat="server"></asp:textbox></td>
	</tr>
	<tr>
		<td>Nombre Tramitador
		</td>
		<td><asp:textbox id="txtTramitador" Runat="server"></asp:textbox></td>
	</tr>
	<tr>
		<td>Entregado al Cliente S/N
		</td>
		<td><asp:dropdownlist id="ddlEntreCli" Runat="server">
				<asp:ListItem Value="N" Selected="True">No</asp:ListItem>
				<asp:ListItem Value="S">Si</asp:ListItem>
				<asp:ListItem Value="ND">ND</asp:ListItem>
			</asp:dropdownlist></td>
	</tr>
	<tr>
		<td align="center" colSpan="2"><asp:button id="btnGuardar" onclick="GuardarDoc" Runat="server" Text="Guardar"></asp:button></td>
	</tr>
</table>

<script language:javascript>

function cargarVinsJS(Obj)
{
	AMS_Vehiculos_Documentos.CargarVin(Obj.value,CargarVin_Callback);
	//probar();	

}

function CargarVin_Callback(response) {
    var ddlVin = document.getElementById("<%=ddlVin.ClientID%>");
    var respuesta = response.value;
    if (respuesta.Tables[0].Rows.length > 0) {
        ddlVin.options.length = 0;
        for (var i = 0; i < respuesta.Tables[0].Rows.length; i++) {
            ddlVin.options[ddlVin.options.length] = new Option(respuesta.Tables[0].Rows[i].VIN, respuesta.Tables[0].Rows[i].VIN);
        }
    }
    else {
        ddlnum.options.length = 0;
        alert('No hay Vins disponibles');
        return;
    }
    probar();
}

function probar()
{
	var catalogo= document.getElementById("<%=ddlcatalogo.ClientID%>");
	//alert(catalogo.value);
	var ddlCodDoc = document.getElementById("<%=ddlCodDoc.ClientID%>");
	//alert(ddlCodDoc.value);
	var ddlVin=document.getElementById("<%=ddlVin.ClientID%>");
	//alert(ddlVin.value);

	alert("Catálogo: " + catalogo.value + "\n" +
          "Código Documento: " + ddlCodDoc.value + "\n" +
          "VIN: " + ddlVin.value + "\n");

	AMS_Vehiculos_Documentos.CargarDatos2(ddlVin.value,catalogo.value,ddlCodDoc.value,cargarDatosJS2_Callback);
}

function cargarDatosJS2(Obj)
{
	var catalogo = document.getElementById("<%=ddlcatalogo.ClientID%>");
	var ddlCodDoc = document.getElementById("<%=ddlCodDoc.ClientID%>");
	AMS_Vehiculos_Documentos.CargarDatos2(Obj.value,catalogo.value,ddlCodDoc.value,cargarDatosJS2_Callback);
}

function cargarDatosJS(Obj)
{
	var catalogo=document.getElementById("<%=ddlcatalogo.ClientID%>");
	var vin=document.getElementById("<%=ddlVin.ClientID%>");
	AMS_Vehiculos_Documentos.CargarDatos(Obj.value,catalogo.value,vin.value,cargarDatosJS_Callback);
}

function cargarDatosJS2_Callback(response)
{
	var respuestaDatos= response.value;
	var vin=document.getElementById("<%=ddlVin.ClientID%>");
	//alert(vin.value);
	
	if (respuestaDatos.Tables[0].Rows.length>0)
	{
	    var tres=document.getElementById("<%=ddlEntreCli.ClientID%>");
	    if(respuestaDatos.Tables[0].Rows[0].TRES_SINO=="S")
		    tres.options[1].selected = true ;
	    else
		    tres.options[0].selected = true ;
		
	    var txtNum=document.getElementById("<%=txtNumDoc.ClientID%>");
	    txtNum.value=respuestaDatos.Tables[0].Rows[0].MVEH_NUMEDOCU;
	    var txtFecha = document.getElementById("<%=txtFechaDoc.ClientID%>");
	    var someD = new Date();
	    someD = respuestaDatos.Tables[0].Rows[0].MVEH_FECHINGRESO;
	    var un = someD.getMonth() + 1;

	    if (un<9)
	    {
	        un= '0'+un;
	    }
	
	    un = un.toString();
	    var dosF = someD.getDate();
	    
        if (dosF<9)
	    {
	        dosF= '0'+dosF;
	    }
	
	    dosF = dosF.toString();
	    var tresF = someD.getFullYear();
	    tresF=tresF.toString();
	    txtFecha.value=tresF+'-'+un+'-'+dosF;
	
	    ///////////////////////////////////////////////////////////////////
	    var txtValo = document.getElementById("<%=txtValoDoc.ClientID%>");
	    txtValo.value= respuestaDatos.Tables[0].Rows[0].MVEH_VALODOCU;
	
	    var txtFechaVenc = document.getElementById("<%=txtFechaVencDoc.ClientID%>");
	    var someDate = new Date();
	    someDate = respuestaDatos.Tables[0].Rows[0].MVEH_FECHENTREGA;
	    var uno = someDate.getMonth()+1;
	    
        if (uno<9)
	    {
	        uno= '0'+uno;
	    }
	
	    uno = uno.toString();
	    var dos = someDate.getDate();
	    
        if (dos<9)
	    {
	        dos= '0'+dos;
	    }
	
	    dos = dos.toString();
	    var tres = someDate.getFullYear();
	    tres=tres.toString();
	    txtFechaVenc.value=tres+'-'+uno+'-'+dos;
	    var txtObs = document.getElementById("<%=txtObserva.ClientID%>")
	    txtObs.value = respuestaDatos.Tables[0].Rows[0].MVEH_OBSERVAC;
	    var txtTramita = document.getElementById("<%=txtTramitador.ClientID%>")
	    txtTramita.value=respuestaDatos.Tables[0].Rows[0].MVEH_NOMBRETRAMITA;
	}
	else
	{
		alert('Sin datos');
		var txtNum=document.getElementById("<%=txtNumDoc.ClientID%>");
		txtNum.value='';
		
		var txtFecha = document.getElementById("<%=txtFechaDoc.ClientID%>");
		txtFecha.value ='';
		
		var txtValo = document.getElementById("<%=txtValoDoc.ClientID%>");
	    txtValo.value= '';
		
		var txtFechaVenc = document.getElementById("<%=txtFechaVencDoc.ClientID%>");
		txtFechaVenc.value = '';
		
		var txtObs = document.getElementById("<%=txtObserva.ClientID%>")
	    txtObs.value = '';
	    
	    var txtTramita = document.getElementById("<%=txtTramitador.ClientID%>")
	    txtTramita.value='';
	    
	    var tres=document.getElementById("<%=ddlEntreCli.ClientID%>");
		tres.options[2].selected = true ;
	}	
}

function cargarDatosJS_Callback(response)
{
	var respuestaDatos= response.value;

	if (respuestaDatos.Tables[0].Rows.length>0)
	{
	    var tres=document.getElementById("<%=ddlEntreCli.ClientID%>");
	    if(respuestaDatos.Tables[0].Rows[0].TRES_SINO=="S")
		    tres.options[1].selected = true ;
	    else
		    tres.options[0].selected = true ;
		
	    var txtNum=document.getElementById("<%=txtNumDoc.ClientID%>");
	    txtNum.value=respuestaDatos.Tables[0].Rows[0].MVEH_NUMEDOCU;
	
	    var txtFecha = document.getElementById("<%=txtFechaDoc.ClientID%>");
	    var someD = new Date();
	    someD = respuestaDatos.Tables[0].Rows[0].MVEH_FECHINGRESO;
	
	    var un = someD.getMonth()+1;
	    if (un<9)
	    {
	        un= '0'+un;
	    }
	
	    un = un.toString();
	    var dosF = someD.getDate();

	    if (dosF<9)
	    {
	        dosF= '0'+dosF;
	    }
	
	    dosF = dosF.toString();
	    var tresF = someD.getFullYear();
	    tresF=tresF.toString();
	    txtFecha.value=tresF+'-'+un+'-'+dosF;
	
	    ///////////////////////////////////////////////////////////////////
	    var txtValo = document.getElementById("<%=txtValoDoc.ClientID%>");
	    txtValo.value= respuestaDatos.Tables[0].Rows[0].MVEH_VALODOCU;
	
	    var txtFechaVenc = document.getElementById("<%=txtFechaVencDoc.ClientID%>");
	    var someDate = new Date();
	    someDate = respuestaDatos.Tables[0].Rows[0].MVEH_FECHENTREGA;
	    var uno = someDate.getMonth()+1;
	    
        if (uno<9)
	    {
	        uno= '0'+uno;
	    }
	
	    uno = uno.toString();
	    var dos = someDate.getDate();

	    if (dos<9)
	    {
	        dos= '0'+dos;
	    }
	
	    dos = dos.toString();
	    var tres = someDate.getFullYear();
	    tres=tres.toString();
	    txtFechaVenc.value=tres+'-'+uno+'-'+dos;
	
	    var txtObs = document.getElementById("<%=txtObserva.ClientID%>")
	    txtObs.value = respuestaDatos.Tables[0].Rows[0].MVEH_OBSERVAC;
	
	    var txtTramita = document.getElementById("<%=txtTramitador.ClientID%>")
	    txtTramita.value=respuestaDatos.Tables[0].Rows[0].MVEH_NOMBRETRAMITA;
	}
	else
	{
		alert('Sin datos');
		var txtNum=document.getElementById("<%=txtNumDoc.ClientID%>");
		txtNum.value='';
		
		var txtFecha = document.getElementById("<%=txtFechaDoc.ClientID%>");
		txtFecha.value ='';
		
		var txtValo = document.getElementById("<%=txtValoDoc.ClientID%>");
	    txtValo.value= '';
		
		var txtFechaVenc = document.getElementById("<%=txtFechaVencDoc.ClientID%>");
		txtFechaVenc.value = '';
		
		var txtObs = document.getElementById("<%=txtObserva.ClientID%>")
	    txtObs.value = '';
	    
	    var txtTramita = document.getElementById("<%=txtTramitador.ClientID%>")
	    txtTramita.value='';
	    
	    var tres=document.getElementById("<%=ddlEntreCli.ClientID%>");
		tres.options[2].selected = true ;
	}
}
</script>
