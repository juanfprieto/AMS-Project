<%@ Control Language="c#" codebehind="AMS.Inventarios.RecepcionItems.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.RecepcionItems" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>


<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<link href="../css/AMS.css" type="text/css" rel="stylesheet" />
<link rel="stylesheet" href="../css/bootstrap.min.css" />

<script type ="text/javascript">
    $(document).ready(function () {
        if (window.returnValue === 'help')
        {
            window.close();
        }
        });
    function mostarDiv() {
        $("#divHelp").show("slow");
    }
    function cerrarHelp() {
        $("#divHelp").hide(1000);
    }
    function abrirEmergente(obj) {
        var ob = document.getElementById('_ctl1_' + obj);
        ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DIT.dite_cantidad as Q_INGRESO, DIT.dite_cantidad-DIT.dite_cantdevo as Q_A_FACTURAR, DIT.dite_valounit as PRECIO, DIT.dite_porcdesc as DESCUENTO, case when MNIT.TREG_REGIIVA IN(\'N\',\'S\')THEN 0 ELSE DIT.piva_porciva END as IVA FROM mnit MNIT, mitems MIT, ditems DIT, plineaitem PLIN WHERE MNIT.MNIT_NIT=\'' + txtNIT.value + '\' AND MIT.mite_codigo=DIT.mite_codigo AND PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' AND DIT.pdoc_codigo=\'' + splitNumPre[0] + '\' AND DIT.dite_numedocu=' + splitNumPre[1] + ' AND (DIT.dite_cantidad-DIT.dite_cantdevo)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,DIT.mite_codigo', new Array('#tbBase', 'valToInsert10'));
    }
    function CalculoIva(obValFlt, obCmbIVA, obValIVAFlt, obTotRep, obTotal)
    {
        if(obValFlt.value.length > 0)
        {
            //NumericMask(obValFlt);
            var stringValorFletes = EliminarComas(obValFlt.value);
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorFletes = parseFloat(stringValorFletes);
            var valorIva = parseFloat(obCmbIVA.value);
            var valorRepuestos = parseFloat(stringValorRepuestos);
            obValIVAFlt.value = String(valorFletes*(valorIva/100));m
            ApplyNumericMask(obValIVAFlt);
            obValIVAFlt.value = '$'+obValIVAFlt.value;
            document.all[obTotal].value = String(valorRepuestos + (valorFletes + (valorFletes*(valorIva/100))));
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = '$'+document.all[obTotal].value;
        }
        else
        {
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorRepuestos = parseFloat(stringValorRepuestos);
            document.all[obTotal].value = String(valorRepuestos);
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = '$'+document.all[obTotal].value;
            obValIVAFlt.value = '$0';
        }
    }

    function CambioIva(obValFlt, obCmbIVA, obValIVAFlt, obTotRep, obTotal)
    {
        if(obValFlt.value.length > 0)
        {
            var stringValorFletes = EliminarComas(obValFlt.value);
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorFletes = parseFloat(stringValorFletes);
            var valorIva = parseFloat(obCmbIVA.value);
            var valorRepuestos = parseFloat(stringValorRepuestos);
            obValIVAFlt.value = String(valorFletes*(valorIva/100));
            ApplyNumericMask(obValIVAFlt);
            obValIVAFlt.value = '$'+obValIVAFlt.value;
            document.all[obTotal].value = valorRepuestos + (valorFletes + (valorFletes*(valorIva/100)));
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = '$'+document.all[obTotal].value;
        }
        else
        {
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorRepuestos = parseFloat(stringValorRepuestos);
            document.all[obTotal].value = String(valorRepuestos);
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value='$'+document.all[obTotal].value;
            obTotFl.value = '$0';
            obValIVAFlt.value = '$0';
        }
    }

    function CargaNITREPDIR(ob, obSEL, obEXT, obCmbLin) 
    {
        var txtNIT = document.getElementById('<%=txtNIT.ClientID%>');
        if (txtNIT.value.length == 0)
        {
            alert("Debe dar un NIT primero!");
            return;
        }
        if(obEXT == null)
        {
            var numPed = obSEL.value;
            var splitNumPed = numPed.split('-');
			if(splitNumPed.length==2)
			    ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD_INGRESADA,DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD_FACTURADA,DPI.dped_valounit AS VALOR_UNITARIO FROM mitems MIT, dpedidoitem DPI, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' AND DPI.pped_codigo=\'' + splitNumPed[0] + '\' AND DPI.mped_numepedi=' + splitNumPed[1] + ' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase', 'valToInsert10'));
			else
				alert('No hay pedidos para recepcionar');
        }
        else
        {
            if(obEXT.value == '')
            { 
                var numPed = obSEL.value;
                var splitNumPed = numPed.split('-');
                if(splitNumPed.length==2)
                    ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD_INGRESADA,DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD_FACTURADA,DPI.dped_valounit AS VALOR_UNITARIO FROM mitems MIT, dpedidoitem DPI, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' AND DPI.pped_codigo=\'' + splitNumPed[0] + '\' AND DPI.mped_numepedi=' + splitNumPed[1] + ' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo = MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase', 'valToInsert10'));
				else
					alert('No hay pedidos para recepcionar');
            }
            else
            {
                ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE,1 AS CANTIDAD_INGRESADA,1 AS CANTIDAD_FACTURADA, coalesce(MSAL.msal_ulticost,0) AS VALOR,0,case when MNIT.TREG_REGIIVA IN(\'N\',\'S\')THEN 0 ELSE MIT.piva_porciva END as IVA, coalesce(msal_cantactual,0) AS saldo_actual FROM dbxschema.mitems MIT LEFT JOIN dbxschema.MNIT MNIT ON MNIT.MNIT_NIT=\'' + txtNIT.value + '\' LEFT JOIN dbxschema.plineaitem PLIN ON MIT.plin_codigo=PLIN.plin_codigo LEFT OUTER JOIN dbxschema.msaldoitem MSAL inner join dbxschema.cinventario cin on cin.pano_ano = msal.pano_ano ON MSAL.mite_codigo=MIT.mite_codigo WHERE PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase', 'valToInsert10'), 1);
           //     ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE,1 AS CANTIDAD_INGRESADA,1 AS CANTIDAD_FACTURADA, coalesce(MSAL.msal_ulticost,0) AS VALOR,0,PIVA_PORCIVA FROM dbxschema.mitems MIT LEFT JOIN dbxschema.plineaitem PLIN ON MIT.plin_codigo=PLIN.plin_codigo LEFT OUTER JOIN dbxschema.msaldoitem MSAL inner join dbxschema.cinventario cin on cin.pano_ano = msal.pano_ano ON MSAL.mite_codigo=MIT.mite_codigo WHERE PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase', 'valToInsert10'), 1);
                                //'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM mitems MIT, plineaitem PLIN WHERE MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo'
            }
        }
    }

    function CargaNITPRE(ob,obSEL,obEXT,obCmbLin)
    {
        var txtNIT = document.getElementById('<%=txtNIT.ClientID%>');
        if (txtNIT.value.length == 0)
        {
            alert("Debe dar un NIT primero!");
            return;
        }
        if(obEXT == null)
        {
            var numPed = obSEL.value;
            var splitNumPed = numPed.split('-');
            if(splitNumPed.length==2)
                ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD,DPI.dped_valounit AS "VALOR UNITARIO" FROM dbxschema.mitems MIT, dbxschema.dpedidoitem DPI, dbxschema.plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.pped_codigo=\'' + splitNumPed[0] + '\' AND DPI.mped_numepedi=' + splitNumPed[1] + ' AND PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase', 'valToInsert10'));
			else
				alert('No hay pedidos para precepcionar');
            //'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD FROM mitems MIT, dpedidoitem DPI, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.pped_codigo=\''+splitNumPed[0]+'\' AND DPI.mped_numepedi='+splitNumPed[1]+' AND MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo'
        }
        else
        {
            if(obEXT.value == '')
            {
                var numPed = obSEL.value;
                var splitNumPed = numPed.split('-');
                if(splitNumPed.length==2)
                    ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD,DPI.dped_valounit AS "VALOR UNITARIO" FROM dbxschema.mitems MIT, dbxschema.dpedidoitem DPI, dbxschema.plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.pped_codigo=\'' + splitNumPed[0] + '\' AND DPI.mped_numepedi=' + splitNumPed[1] + ' AND PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase', 'valToInsert10'));
				else
					alert('No hay pedidos para precepcionar');
                //'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD FROM mitems MIT, dpedidoitem DPI, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.pped_codigo=\''+splitNumPed[0]+'\' AND DPI.mped_numepedi='+splitNumPed[1]+' AND MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo'
            }
            else
                ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, COALESCE(MSAL.MSAL_CANTACTUAL,0) AS CANTIDAD, CASE WHEN MSAL.msal_ulticost IS NULL THEN MITE_COSTREPO ELSE MSAL.msal_ulticost END AS COSTO FROM dbxschema.plineaitem PLIN, dbxschema.mitems MIT left join dbxschema.msaldoitem MSAL ON MIT.mite_codigo=MSAL.mite_codigo WHERE PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase', 'valToInsert10'), 1);
                //'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM mitems MIT, plineaitem PLIN WHERE MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo'
        }
    }
    function CargaNITPRO(ob) {
        var nombre = cargarNitCli(ob);
        $('#<%=txtNITa.ClientID%>').val(nombre);
    }
    function cargarNitCli(objeto) {
        return RecepcionItems.ConsultarNombreCliente(objeto.value).value;
    }

    function CargaNITLEG(ob, obSEL, obCmbLin) 
    {
        var txtNIT = document.getElementById('<%=txtNIT.ClientID%>');
        if (txtNIT.value.length == 0)
        {
            alert("Debe dar un NIT primero!");
            return;
        }
        if(obSEL.length > 0)
        {
            var numPre = obSEL.value;
            var splitNumPre = numPre.split('-');
            if (splitNumPre.length > 2) {
                splitNumPre[0] += "-" + splitNumPre[1];
                splitNumPre[1] = splitNumPre[2];
            }

            ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DIT.dite_cantidad as Q_INGRESADA, DIT.dite_cantidad-DIT.dite_cantdevo as Q_A_FACTURAR, DIT.dite_valounit as PRECIO, DIT.dite_porcdesc as DESCUENTO, case when MNIT.TREG_REGIIVA IN(\'N\',\'S\')THEN 0 ELSE DIT.piva_porciva END as IVA FROM mnit MNIT, mitems MIT, ditems DIT, plineaitem PLIN WHERE MNIT.MNIT_NIT=\'' + txtNIT.value + '\' AND MIT.mite_codigo=DIT.mite_codigo AND PLIN.plin_tipo=\'' + (obCmbLin.value.split('-'))[1] + '\' AND DIT.pdoc_codigo=\'' + splitNumPre[0] + '\' AND DIT.dite_numedocu=' + splitNumPre[1] + ' AND (DIT.dite_cantidad-DIT.dite_cantdevo)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,DIT.mite_codigo', new Array('#tbBase', 'valToInsert10'));
        }
        else
            alert("No se ha seleccionado ninguna prerecepción para legalziar!");
    }

	function CambioPrefijo(obj)
	{
		RecepcionItems.CargarNumero(obj.value,CargarNumero_CallBack);
	}
	
	function CargarNumero_CallBack(response)
	{
		if (response.error != null)
		{
			alert(response.error);
			return;
		}
		var objNum=document.getElementById("<%=txtNumFacE.ClientID%>");
		objNum.value=response.value;
    }

    function Cargarfacturapre(ob, obSEL, obEXT, obCmbLin) 
    {
    if (response.error != null) {
        alert(response.error);
        return;
    }
    var objNum = document.getElementById("<%=txtNumFacE.ClientID%>");
    objNum.value = response.value;
}
	
    function botonGrabar(obj)
	{
		btnAjus.Enable = false;
		NewAjust ();
	}
	//objSender = id del objeto que hace el llamado a la función
	//idDDLLinea = id del objeto que contiene la linea de bodega
	//idObjNombre = id objeto que tiene el nombre
	//idObjValor = id del objeto que tiene el valor unitario
	//idObjDesc = id del objeto que tiene el descuento
	//idObjIva = id del objeto que tiene el iva
	function ConsultarInfoReferencia(objSender, idDDLLinea, idObjNombre, idDDLalmacen, idObjValor, idObjDesc, idObjIva, nit)
	{
		var codigoRef = objSender.value;
		
		if(codigoRef!="")
		{
			var oItm=objSender.value;
			objSender.value=RecepcionItems.ConsultarSustitucion(objSender.value).value
			if(objSender.value!=oItm)
				alert('El item: '+oItm+' ha sido cambiado por su sustitución: '+objSender.value);
			
			var lineaRef = (document.getElementById(idDDLLinea).value.split('-'))[1];
			var nombreRef = RecepcionItems.ConsultarNombreItem(codigoRef,lineaRef).value;
			var almacen=document.getElementById(idDDLalmacen).value;
			
			if(nombreRef != '')
			{
				document.getElementById(idObjNombre).value = nombreRef;		
				document.getElementById(idObjDesc).value = '0';	
				document.getElementById(idObjValor).value = RecepcionItems.ConsultarPrecioItem(codigoRef,lineaRef,almacen).value;
				document.getElementById(idObjIva).value = RecepcionItems.ConsultarIvaItem(codigoRef, lineaRef, nit).value;
				
				
				if(document.getElementById(idObjValor).value=="0")
					alert('Se ha detectado el valor de 0 en el precio, esto se puede deber a las siguientes causas: \n' 
						+'1. El item '+objSender.value+' no tiene un precio registrado en la lista de precios seleccionada (Clientes)\n'
						+'2. El item aún no tiene movimiento (Clientes-Proveedores)\n'
						+'Le recomendamos revisar su configuración, por favor digite un valor distinto de cero');
			}
			else
			{
				alert('La referencia '+objSender.value+' no esta registrada');
				document.getElementById(idObjNombre).value = '';
				document.getElementById(idIbjValor).value='';
			}
		}
	}

	<%--$(document).ready(function () {
	    alert("Estamos Listos!!");
	    $("#" + "<%=ddlTProc.ClientID%>").change(function () {
	        alert("Cambio la opcion!! " + $("#" + "<%=ddlTProc.ClientID%>" + " option:selected").val());
	    });
	});--%>

</script>
<fieldset>
	<legend>Datos del proceso:</legend>
        <table id="Table4" >
			<tbody>
				<tr>
					<td>
                        <asp:label id="Label10" forecolor="RoyalBlue" runat="server">INFORMACIÓN GENERAL</asp:label>
                        <br />
                        Proceso:<br />
				        <asp:dropdownlist id="ddlTProc" runat="server" class="dmediano" AutoPostBack="true" OnSelectedIndexChanged="CambiaProceso">
                            <asp:ListItem Value="-1" Selected="True" Text="Seleccione.."></asp:ListItem> 
				            <asp:ListItem Value="0" Selected="False" Text="Pre_Recepción"> </asp:ListItem>
				            <asp:ListItem Value="1" Selected="False" Text="Recepción Directa"></asp:ListItem>
				            <asp:ListItem Value="2" Selected="False" Text="Legaliza pre_Recepción"></asp:ListItem>
                            
				        </asp:dropdownlist>
                    </td>
                    <td>
                        Almacén:<br />
						<asp:dropdownlist id="ddlAlmacen" class="dmediano" runat="server" AutoPostBack="True" onselectedindexchanged="ddlAlmacen_SelectedIndexChanged"></asp:dropdownlist>
                    </td>
                    <td>
                        Responsable:<br />
						<asp:dropdownlist id="ddlVendedor" class="dmediano" runat="server"></asp:dropdownlist>
					</td>
					<td>
                        <asp:placeholder id="plhOpLeg" runat="server">
							<p>Medio:<br />
								<asp:DropDownList id="ddlMedio" class="dmediano" runat="server" AutoPostBack="True" EnableViewState="true">
									<asp:ListItem Value="0" Selected="True">Digitada</asp:ListItem>
									<asp:ListItem Value="2">Liquidación Automática</asp:ListItem>
								</asp:DropDownList></p>
						</asp:placeholder>
					</td>
                </tr>
                <asp:placeholder id="plhFacE" runat="server">
								
				<tr>
					<td>
                                   
						<asp:Label id="Label4" runat="server" forecolor="RoyalBlue">ENTRADA ALMACEN </asp:Label><br />
										
                        Prefijo Causación:<br />
											
						<asp:DropDownList id="ddlPrefE" class="dmediano" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefE_SelectedIndexChanged"></asp:DropDownList>
						<asp:RequiredFieldValidator id="rqPrefijoEntrada" runat="server" ControlToValidate="ddlPrefE" Display="None"
								ErrorMessage="Por favor selecciona un prefijo para la entrada"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        Numero Causación:<br />
                                            
						<asp:TextBox id="txtNumFacE" runat="server" class="tpequeno" ReadOnLy="true" MaxLenght="8"></asp:TextBox>
						<asp:RequiredFieldValidator id="rqNumeroEntrada" runat="server" ControlToValidate="txtNumFacE" Display="None"
								ErrorMessage="Por favor selecciona un numero para la entrada"></asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="revTxtNumFacE" runat="server" ControlToValidate="txtNumFacE" Display="None"
								ErrorMessage="Por favor ingresar un numero valido para la entrada" ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>&nbsp;
					</td>
				</tr>
				</asp:placeholder>
                            
                <asp:placeholder id="plhFacF" runat="server">
				<tr>
					<td>
                        <asp:Label id="Label1" runat="server" forecolor="RoyalBlue">FACTURA PROVEEDOR</asp:Label><br />
						NIT Proveedor:<br />
						<asp:textbox id="txtNIT" name="txtNIT" class="tpequeno" ReadOnLy="false" runat="server" style="display:-webkit-inline-box;"></asp:textbox>
                        <asp:Image id="imgLupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image>
                        <asp:requiredfieldvalidator id="rqTxtNIT" runat="server" ErrorMessage="Por favor ingresar el NIT del proveedor" Display="None" ControlToValidate="txtNIT">
                        </asp:requiredfieldvalidator>
					</td>

					<td>
						<asp:textbox id="txtNITa" name="txtNIT" runat="server" class="tmediano" ReadOnLy="true"></asp:textbox>
					</td>

                    <td>
						Días Plazo:<br />
						<asp:TextBox id="txtPlazo" runat="server" class="tpequeno" ReadOnLy="false">0</asp:TextBox>&nbsp;&nbsp;
						<asp:RequiredFieldValidator id="rqTxtPlazo" runat="server" ControlToValidate="txtPlazo" Display="None" ErrorMessage="Por favor ingresar los días de plazo"></asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="revTxtPlazo" runat="server" ControlToValidate="txtPlazo" Display="None" ErrorMessage="Por favor ingresar un numero de días de plazo valido"
							ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>&nbsp;
					</td>
				</tr>

                <tr>
					<td>
			            Prefijo:&nbsp;
						<asp:TextBox id="txtPref" runat="server" class="tpequeno" ReadOnLy="false" MaxLength="6"></asp:TextBox>
						<asp:RequiredFieldValidator id="rqTxtPref" runat="server" ControlToValidate="txtPref" Display="None" ErrorMessage="Por favor ingresar un prefijo para la factura entregada por el proveedor"></asp:RequiredFieldValidator>
					</td>
					<td>
						Numero:&nbsp;
						<asp:TextBox id="txtNumFac" runat="server" class="tpequeno" MaxLength="8"></asp:TextBox>&nbsp;&nbsp;
						<asp:RequiredFieldValidator id="rqTxtNumFac" runat="server" ControlToValidate="txtNumFac" Display="None" ErrorMessage="Por favor seleccionar un numero para la factura entragada por el proveedor"></asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="revTxtNumFac" runat="server" ControlToValidate="txtNumFac" Display="None" ErrorMessage="Por favor ingresar un numero valido para la factura entregada por el proveedor"
							ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>&nbsp;
					</td>
								
					<td>
                        Fecha:<br />
						<asp:TextBox id="tbDate" runat="server" class="tpequeno" Enabled="true"></asp:TextBox>
                        <img onmouseover="calendar.style.visibility='visible'"
							src="../img/AMS.Icon.Calendar.gif" border="0" alt="" />
						<table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
							onmouseout="calendar.style.visibility='hidden'"><tr><td><asp:calendar BackColor="Beige" id="calDate" runat="server" OnSelectionChanged="ChangeDate" enableViewState="true" ></asp:calendar></td></tr></table>
                        
					</td>
				</tr>
				</asp:placeholder>
                       		
                <tr>
					<td>
						<asp:CheckBox ID="chkImportacion" Runat="server"></asp:CheckBox>&nbsp;&nbsp;Importaciones(NIT debe ser extranjero)
					</td>
					<td>
						<p>Observaciones:<br />
						<asp:TextBox id="txtObs" name="txtNIT" runat="server" style="width:260px;" MaxLength="200" Rows="2" TextMode="MultiLine" ></asp:TextBox></p>
					</td>
                    <td>
						<asp:button id="btnSelecNIT" onclick="CambiaNIT" runat="server" Text="Confirmar"></asp:button>
					</td>
				</tr>
                                 
				<tr>
					<td align="center">
						<p>
                            <asp:placeholder id="plhNTPre" runat="server" visible="false">
								<asp:Label id="lblProc" runat="server">Prerecepcion:</asp:Label>
								<br />
								<asp:DropDownList id="ddlTipoPre" class="dpequeno" runat="server"></asp:DropDownList>
								<asp:RequiredFieldValidator id="rqDdlTipoPre" runat="server" ControlToValidate="ddlAlmacen" Display="None" ErrorMessage="Por favor selecciona un documento de prerecepcion"></asp:RequiredFieldValidator>
								<br />
								<asp:TextBox id="txtPreRe" runat="server" Enabled="False" class="tpequeno"></asp:TextBox>
								<asp:RequiredFieldValidator id="rqTxtPreRe" runat="server" ControlToValidate="txtPreRe" Display="None" ErrorMessage="Por favor selecciona un numero de prerecepcion"></asp:RequiredFieldValidator>
							</asp:placeholder>
						</p>
					</td>
                </tr>                            
             </tbody>
		</table>
					
</fieldset>

<br>

<asp:placeholder id="plhFile" runat="server">
	<table style="text-align: center;">
		<tr>
			<td>
				<p>
                    Prefijo Pedido a Cargar:&nbsp;
                    <asp:textbox id="txtPrefijoExcel" class="tpequeno" Visible="false" runat="server"></asp:textbox>
                    Tipo de Carga:&nbsp;
                    <asp:DropDownList id="ddlTipoCargaExcel" class="dmediano" runat="server" onselectedindexchanged="OnChangeTipoCarga" AutoPostBack="true" style="margin-left:auto; margin-right:auto;">
                        <asp:ListItem Text="Carga Directa" Value="D" Selected="true"></asp:ListItem>
                        <%--<asp:ListItem Text="Carga Factura" Value="F"></asp:ListItem>--%>
                    </asp:DropDownList>
                    Archivo:&nbsp; <input id="File1" type="file" name="File1" runat="server" style="margin-left:auto; margin-right:auto;"/>&nbsp;&nbsp;
					<asp:Button id="btnLoadFile" onclick="CargaArchivo" runat="server" Text="Cargar" CausesValidation="false"></asp:Button>
                     <img src="../img/AMS.Help.png"  style="width: 28px; cursor:pointer;" onclick="mostarDiv()" alt=""/>
                </p>               
			</td>
		</tr>
	</table>
    <div style="position:absolute; height: auto; width: 580px; top: 113px; right: 271px; overflow: auto; text-align: left; background-color: gainsboro; display: none;" id="divHelp">
        <fieldset>
            <legend>Ayuda Subir Entradas Excel:</legend>
            <img src="../img/AMS.Icon.Close.png"  style="width: 37px;left: 531px;top: 6px;position: absolute; cursor:pointer;" onclick="cerrarHelp()" alt="" /> 
            <table class="filtersIn">
                <tr>
                    <td> 
                        Pasos para subir las entradas por el método <b>Excel</b>:<br /><br /><br />
                    <b>1)</b> Construir un archivo Excel con las siguientes columnas:<br />
                        Titulo columna A: PEDIDO<br />
                        Titulo columna B: ITEM<br />
                        Titulo columna C: CANTIDAD_FACTURADA<br />
                        Titulo columna D: CANTIDAD_INGRESADA<br />
                        Titulo columna E: PRECIO<br />
                        Titulo columna F: DESCUENTO<br />
                        Titulo columna G: IVA<br />
                        Titulo columna H: LINEA_BODEGA<br /><br />
                    <b>2)</b> Llenar el Excel de acuerdo a su necesidad.<br /><br />
                    <b>3)</b> Debe asignarse un nombre a la Tabla Excel para marcar la región de datos:<br />
                       3.1) Se debe seleccionar todos los campos.<br />
                       3.2) Dar click en la barra de tareas en la pestaña Fórmulas.<br />
                       3.3) Dar click en Asignar nombre. Nos despliega una ventana.
                            En campo de nombre se debe escribir: TABLA. Luego click en Aceptar.<br />
                       3.4) Guardar archivo. <br /><br />
                    <b>4)</b> Subir datos mediante el botón "Seleccionar Archivo" de este menú. Y a continuación hacer click en botón "Cargar".                
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="divErr" style="margin-left: 25%; border-style: solid; width: 45%;  padding: 2%;" runat="server" visible="false">
        <asp:Label ID="lblErr" runat="server" ></asp:Label>
    </div>
</asp:placeholder>
<asp:placeholder id="plhEmbarque" runat="server">
	<table>
		<tr>
			<td align="right">Embarque:</td>
			<td>
				<asp:dropdownlist id="ddlEmbarque" runat="server"></asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td align="right">Tasa de Cambio Declaración Importación:</td>
			<td>
				<asp:TextBox id="txtTasaCambioI" onkeyup="NumericMaskE(this,event)" runat="server" Width="80px"></asp:TextBox>
			</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td align="right">Tasa de Cambio Nacionalización:
			</td>
			<td>
				<asp:TextBox id="txtTasaCambio" onkeyup="NumericMaskE(this,event)" runat="server" Width="80px"></asp:TextBox>
			</td>
			<td>
				<asp:Button id="btnSelEmbarq" onclick="SeleccionarEmbarque" runat="server" Text="Cargar" CausesValidation="false"></asp:Button>
			</td>
		</tr>
	</table>
	<br />
	<asp:datagrid id="dgrGastos" runat="server" EnableViewState="true" 
    GridLines="Vertical" AutoGenerateColumns="false" class="datagrid">
		<HeaderStyle cssclass="header"></HeaderStyle>
		<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="items"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Orden:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PREFO") %>-<%# DataBinder.Eval(Container.DataItem, "NUMO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Gasto:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "GASTO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Total:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nacional:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PGAS_MODENACI") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid>
</asp:placeholder>
<fieldset>
    <asp:DataGrid id="dgItemsRDir" runat="server" enableViewState="true" OnEditCommand="RRDir_Edit"
		OnUpdateCommand="RRDir_Update" OnItemCommand="RRDir_AddAndDel" AutoGenerateColumns="false"
        GridLines="Vertical" ShowFooter="True" OnCancelCommand="RRDir_Cancel" OnItemDataBound="RRDir_ItemDataBound"
        class="datagrid">
        <HeaderStyle cssclass="header"></HeaderStyle>
		<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="items"></ItemStyle>
        <FooterStyle cssclass="footer"></FooterStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Pedido:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "num_ped") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="ddlPedSel" class="dmediano" runat="server"></asp:DropDownList>
					<asp:TextBox id="txtPedido" ReadOnLy="false" runat="server" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Item:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_codigo") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1" runat="server" class="tmediano"></asp:TextBox>
                    <asp:Image id="imglupa2" runat="server" ImageUrl="../img/AMS.Search.png" ></asp:Image>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1a" ReadOnLy="true" runat="server" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Linea de Bodega:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "plin_codigo") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList OnSelectedIndexChanged="ddlLinea_SelectedIndexChanged" id="ddlLinea" class="dmediano" runat="server"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Unidad:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_unid") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cant. Ingresada:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_cantped", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1b" class="tpequeno" onkeyup="NumericMaskE(this,event)" runat="server"
							text="1"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" onkeyup="NumericMaskE(this,event)" id="edit_1" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "mite_cantped") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cant. Facturada:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_cantfac", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1c" onkeyup="NumericMaskE(this,event)" runat="server"
						class="tpequeno" text="1"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" onkeyup="NumericMaskE(this,event)" id="edit_2" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "mite_cantfac") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Precio:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_precio", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1d" onkeyup="NumericMaskE(this,event)" runat="server"
						class="tpequmedi" text="0"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" class="tpequmedi" onkeyup="NumericMaskE(this,event)" id="edit_3" Text='<%# DataBinder.Eval(Container.DataItem, "mite_precio") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="%Descto:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_desc", "{0:N}%") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1e" onkeyup="NumericMaskE(this,event)" runat="server"
						class="tpequeno" text="0"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" onkeyup="NumericMaskE(this,event)" id="edit_5" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "mite_desc") %>' />
					%
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="%IVA:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_iva", "{0:N}%") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1f" onkeyup="NumericMaskE(this,event)" runat="server"
						class="tpequeno" text="0"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_4" onkeyup="NumericMaskE(this,event)" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "mite_iva") %>' />
					%
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Total:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_tot", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Ubicacion:">
				<ItemTemplate>
					<asp:DropDownList id="ddlUbicaciones" runat="server" visible ="True"></asp:DropDownList>
					<asp:Label id="lbNuevaUbi" runat="server" text="Nueva" cssclass="PunteroMano"></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones:">
				<ItemTemplate>
					<asp:Button CommandName="DelDatasRow" Text="Quitar" ID="btnDel" Runat="server" class="bpqueno"
						CausesValidation="False" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar Item a Item" ID="btnAdd" Runat="server"
						CausesValidation="False" class="bmediano" />
					<asp:Button CommandName="AddDataAll" Text="Agregar Pedido Completo" ID="btnAll" Runat="server"
						CausesValidation="False" class="bmediano" />
					<asp:Button CommandName="ClearRows" Text="Reiniciar" ID="btnClear" Runat="server" class="bmediano"
						CausesValidation="False" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
			<asp:TemplateColumn HeaderText="Valor US:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_us", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
        
        
    <asp:DataGrid id="dgItemsPRec" runat="server" enableViewState="true" OnEditCommand="RPRec_Edit"
		OnUpdateCommand="RPRec_Update" OnItemCommand="RPRec_AddAndDel" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True"
		CellPadding="3" OnCancelCommand="RPRec_Cancel" OnItemDataBound="RPRec_ItemDataBound" class="datagrid">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle cssclass="header"></HeaderStyle>
		<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="items"></ItemStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Pedido:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "num_ped") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="DropDownList1" class="dmediano" runat="server"></asp:DropDownList>
					<asp:TextBox id="valToInsert0" ReadOnLy="false" runat="server" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Item:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_codigo") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox1" runat="server" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox2" runat="server" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Linea de Bodega:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "plin_codigo") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="DropDownList2" class="dmediano" runat="server"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Unidad:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_unid") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cant. Ingresada:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_cantped", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox3" onkeyup="NumericMaskE(this,event)" runat="server"
						class="tpequeno" text="1"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)" id="TextBox4" Text='<%# DataBinder.Eval(Container.DataItem, "mite_cantped","{0:N}") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Precio:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_precio", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox5" class="tpequeno" onkeyup="NumericMaskE(this,event)" runat="server"
							text="0"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="TextBox6" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "mite_precio","{0:N}") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Descuento:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_desc", "{0:N}%") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsertDesc" class="tpequeno" onkeyup="NumericMaskE(this,event)" runat="server"
						text="0"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)" id="TextBox7"  Text='<%# DataBinder.Eval(Container.DataItem, "mite_desc","{0:N}") %>' />
					%
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="IVA:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_iva", "{0:N}%") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsertIVA" class="tpequeno" onkeyup="NumericMaskE(this,event)" runat="server"
							text="0"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="TextBox8" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "mite_iva","{0:N}") %>' />
					%
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Total:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_tot", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Ubicacion:">
				<ItemTemplate>
					<asp:DropDownList id="DropDownList3" runat="server" visible ="True"></asp:DropDownList>
					<asp:Label id="Label2" runat="server" text="Nueva" cssclass="PunteroMano"></asp:Label>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList id="ddlUbic" runat="server"></asp:DropDownList>
					<asp:Label runat="server" id="lbUbi" Text="Editar" cssclass="PunteroMano" />
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones:">
				<ItemTemplate>
					<asp:Button CommandName="DelDatasRow" Text="Quitar" ID="Button1" Runat="server" class="bpequeno"
						CausesValidation="False" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar Item a Item" ID="Button2" Runat="server"
						CausesValidation="False" class="bmediano" />
					<asp:Button CommandName="AddDataAll" Text="Agregar Pedido Completo" ID="btnAll1" Runat="server"
						CausesValidation="False" class="bmediano" />
					<asp:Button CommandName="ClearRows" Text="Reiniciar" ID="Button3" Runat="server" class="bmediano"
						CausesValidation="False" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
		</Columns>
	</asp:DataGrid>
 
      
    <asp:DataGrid id="dgItemsLeg" runat="server" enableViewState="true" OnEditCommand="RLeg_Edit"
		OnUpdateCommand="RLeg_Update" OnItemCommand="RLeg_AddAndDel" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True"
		CellPadding="3" OnCancelCommand="RLeg_Cancel" OnItemDataBound="RLeg_ItemDataBound" class="datagrid">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle cssclass="header"></HeaderStyle>
		<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="items"></ItemStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Prerecepcion:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "num_ped") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="ddlPedSelA" class="dmediano" runat="server"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Item:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_codigo") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox9" ReadOnly="true" runat="server" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox10" ReadOnLy="true" runat="server" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Linea de Bodega:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "plin_codigo") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="DropDownList4" class="dmediano" runat="server"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Unidad:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_unid") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cant. Ingresada:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_cantped", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox11" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cant. Facturada:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_cantfac", "{0:N}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" onkeyup="NumericMaskE(this,event)" id="TextBox12" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "mite_cantfac","{0:N}") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox13" class="tpequeno" onkeyup="NumericMaskE(this,event)" runat="server">
                    </asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Precio:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_precio", "{0:C}") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" onkeyup="NumericMaskE(this,event)" onfocus="ApplyNumericMask(this)" id="edit_3A" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "mite_precio","{0:N}") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox14" class="tpequeno" onkeyup="NumericMaskE(this,event)" onfocus="ApplyNumericMask(this)"
						runat="server"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Descuento:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_desc", "{0:N}%") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox15" class="tpequeno" onkeyup="NumericMaskE(this,event)" runat="server">
                    </asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_5A" onkeyup="NumericMaskE(this,event)" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "mite_desc","{0:N}") %>' />
					%
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="IVA:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_iva", "{0:N}%") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="TextBox16" class="tpequeno" onkeyup="NumericMaskE(this,event)" runat="server"
						></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_4A" onkeyup="NumericMaskE(this,event)" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "mite_iva","{0:N}") %>' />
					%
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Total:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "mite_tot", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones:">
				<ItemTemplate>
					<asp:Button CommandName="DelDatasRow" Text="Quitar" ID="btnDelA" Runat="server" class="bpequeno"
						CausesValidation="False" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDataRow" Text="Agregar Item a Item" ID="btnAddA" Runat="server"
						CausesValidation="False" class="bpequeno" />
					<asp:Button CommandName="AddDataRows" Text="Agregar PreRecepción" ID="btnAddT" Runat="server"
						CausesValidation="False" class="bpequeno" />
					<asp:Button CommandName="ClearRows" Text="Reiniciar" ID="btnClearA" Runat="server" class="bpequeno"
						CausesValidation="False" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
		</Columns>
	</asp:DataGrid>
</fieldset>
<fieldset>
	<table id="Table" class="filtersIn">
		<tbody>
			<tr>
				<td>
					<legend>Fletes</legend>
						<tr>
							<td>
								Valor Fletes:
								<br />
									
								<asp:textbox id="txtFlet" runat="server" class="tpequeno">0</asp:textbox>
							</td>
								
							<td>
								%IVA Fletes:
								<br />
									
							<asp:dropdownlist id="ddlPIVA" class="dpequeno" runat="server"></asp:dropdownlist>
							</td>
						</tr>
				</td>
			</tr>
        </tbody>
    </table>
</fieldset>
<fieldset>
<legend>Totales</legend>
	<table id="Table3" class="filtersIn">
		<tbody>
			<tr>
				<td>Subtotal:<br />
                    <asp:textbox id="txtSubTotalManual" runat="server" class="tpequeno" placeholder="valor manual..." onkeyup="NumericMaskE(this,event)"></asp:textbox>
                    <asp:textbox id="txtSubTotal" runat="server" ReadOnly="True" class="tpequeno" Visible="false">$0</asp:textbox>
				</td>
				<td>
					Numero Items:<br />
					<asp:textbox id="txtNumItem" runat="server" ReadOnly="True" class="tpequeno">0</asp:textbox>
				</td>	
				<td>Descuento:<br />
				    <asp:textbox id="txtDesc" runat="server" ReadOnly="True" class="tpequeno">$0</asp:textbox>
				</td>
				<td>
					Numero Unidades:
                    <br />
					<asp:textbox id="txtNumUnid" runat="server" ReadOnly="True" class="tpequeno">0</asp:textbox>
				</td>
			</tr>
			<tr>
				<td>IVA:<br />
					<asp:textbox id="txtIVA" runat="server" ReadOnly="True" class="tpequeno">$0</asp:textbox>
				</td>
				<td>Valor IVA Fletes:<br />
					<asp:textbox id="txtTotIF" runat="server" ReadOnly="True" class="tpequeno">$0</asp:textbox></td>
				<td>Total:<br />
					<asp:textbox id="txtTotal" runat="server" ReadOnly="True" class="tpequeno">$0</asp:textbox>
				</td>
				<td>Gran Total:<br />
					<asp:textbox id="txtGTot" runat="server" ReadOnly="True" class="tpequeno">$0</asp:textbox>
				</td>
			</tr>
            <tr>
                <td>
                    <asp:button id="btnAjus" onclick="NewAjust" runat="server" Enabled="False" Text="Realizar Proceso" ></asp:button>
                </td>
            </tr>
		</tbody>
	</table>
</fieldset>
<p><asp:label id="lbInfo" runat="server"></asp:label></p>
<p></p>
<asp:validationsummary id="vstotal" runat="server" ShowSummary="False" ShowMessageBox="True"></asp:validationsummary>
