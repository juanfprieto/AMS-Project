<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EmisionRecibos.Encabezados.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.EncabezadoRecibo" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>

<table class="filtersInAuto">
	<tbody>
		<tr>
			<td>Sede:&nbsp;<asp:dropdownlist id="almacen" class="dmediano" onSelectedIndexChanged="Cambiar_Almacen" AutoPostBack="true" runat="server"></asp:dropdownlist>
            </td>
			<td>Prefijo:&nbsp;<asp:dropdownlist id="prefijoRecibo" class="dmediano" onChange="Cambiar_Prefijo(this);" runat="server"></asp:dropdownlist>
            </td>
        </tr>
        <tr>
			<td>Número:&nbsp;<asp:textbox id="numeroRecibo" readonly="true" class="tpequeno" runat="server"></asp:textbox><asp:requiredfieldvalidator id="validatorNumero" 
                runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="numeroRecibo">*</asp:requiredfieldvalidator>
				<asp:RegularExpressionValidator id="rev1" runat="server" ControlToValidate="numeroRecibo" ErrorMessage="*" ValidationExpression="\d+"
					Display="Dynamic">*</asp:RegularExpressionValidator>
            </td>     
			<td>Fecha:&nbsp;<asp:textbox id="fecha" onkeyup="DateMask(this)" class="tpequeno" runat="server" ReadOnly="True"></asp:textbox><asp:regularexpressionvalidator id="validatorFecha" runat="server" ErrorMessage="RegularExpressionValidator" ControlToValidate="fecha"
					Text="*" ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:regularexpressionvalidator>
            </td>
		</tr>
		<tr>
			<td><div id="divTipo" runat="server">Tipo:&nbsp;</div><asp:dropdownlist id="tipoRecibo" class="dmediano" runat="server" onchange="javascript:MostrarCreditos(this);"></asp:dropdownlist>
            </td>
			<td>Concepto:<br><asp:textbox id="concepto" runat="server" Width="250px" Height="35px" TextMode="MultiLine"></asp:textbox>
				<asp:RequiredFieldValidator id="rfv3" runat="server" ControlToValidate="concepto" ErrorMessage="*" Display="Dynamic">*</asp:RequiredFieldValidator>&nbsp;
	        </td>
        </tr>
        <tr>
			<td><div id="lblFlujo" Runat="server" style="display: inline-block">Flujo de Caja:</div>&nbsp;<asp:dropdownlist id="ddlFlujo" class="dpequeno" runat="server"></asp:dropdownlist>
            </td>
            <td></td>
		</tr>
		<tr>
			<td><asp:label id="lbCli" Runat="server"></asp:label>&nbsp;<asp:textbox id="datCli" class="tpequeno" onblur="Cargar_Datos(this);" ondblclick="SelCliente(this);"
				Runat="server" ToolTip="Haga Doble Click para iniciar la busqueda o ingresar un nit nuevo" MaxLength="15"></asp:textbox>
                <asp:requiredfieldvalidator id="rfv1" runat="server" ErrorMessage="*" ControlToValidate="datCli"></asp:requiredfieldvalidator>
            </td>
            <td><asp:textbox id="datClia" Runat="server" ReadOnly="True"></asp:textbox>
            </td>
        </tr>
        <tr>
			<td><asp:label id="lbBen" Runat="server"></asp:label>&nbsp;<asp:textbox id="datBen" class="tpequeno" onblur="Cargar_Datos2(this);" ondblclick="ModalDialog(this, 'SELECT M.mnit_nit AS Nit, CASE WHEN M.TNIT_TIPONIT = \'N\' THEN M.mnit_apellidos ELSE M.mnit_apellidos CONCAT \' \' CONCAT COALESCE(M.mnit_apellido2,\'\') CONCAT \' \' CONCAT M.mnit_nombres CONCAT \' \' CONCAT COALESCE(M.mnit_NOMBRE2,\'\') END AS Nombre,M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M,pciudad P WHERE M.pciu_codigo=P.pciu_codigo ORDER BY mnit_nit',1,new Array())"
					Runat="server" ToolTip="Haga Doble Click para iniciar la busqueda o ingresar un nit nuevo" MaxLength="15"></asp:textbox>
            <asp:requiredfieldvalidator id="rfv2" runat="server" ErrorMessage="*" ControlToValidate="datBen"></asp:requiredfieldvalidator>
            </td>
            <td>
            <asp:textbox id="datBena" Runat="server" ReadOnly="True"></asp:textbox>
            </td>    
		</tr>
        <tr>
            <td colspan="2">
			<asp:button id="aceptar" onclick="aceptar_Click" runat="server" Text="Aceptar"></asp:button>
            </td>
		</tr>
	</tbody>
</table>

<div id="dvCredito" style="VISIBILITY: <%=verCredito%>;display:inline">
    <table id="Table" class="filtersIn">
	    <tbody>
		    <tr>
			    <td>Crédito:</td>
			    <td><asp:dropdownlist id="ddlCredito" runat="server" onchange="javascript:TraerCreditoA();"></asp:dropdownlist>
                </td>
	        </tr>
        </tbody>
    </table>
</div>

<script type ='text/javascript'>
    var obj1b  = document.getElementById("<%=datCli.ClientID%>").onblur;
    var obj1db = document.getElementById("<%=datCli.ClientID%>").ondblclick;
    var obj2b  = document.getElementById("<%=datBen.ClientID%>").onblur;
    var obj2db = document.getElementById("<%=datBen.ClientID%>").ondblclick;
    function Retornar_Nombre_CallBack(response) {
        if (response.error != null) {
            alert(response.error);
            return;
        }
        if (response.value == "Error") {
            alert('Nit Inexistente');
            var nit = document.getElementById("<%=datCli.ClientID%>");
            var desc = document.getElementById("<%=datClia.ClientID%>");
            nit.value = "";
            desc.value = "";
        }
        else {
            var resultado = response.value;
            var obj = document.getElementById("<%=datClia.ClientID%>");
            var obj1 = document.getElementById("<%=datBena.ClientID%>");
            var obj2 = document.getElementById("<%=datCli.ClientID%>");
            var obj3 = document.getElementById("<%=datBen.ClientID%>");
            obj.value = resultado;
            if (obj3.value == "") {
                obj1.value = resultado;
                obj3.value = obj2.value;
            }
        }
    }

    function Cargar_Datos(val) {
        if (val.value == "")
            return;
        else
            EncabezadoRecibo.Retornar_Nombre(val.value, Retornar_Nombre_CallBack);
    }

    function Retornar_Nombre2_CallBack(response) {
        if (response.error != null) {
            alert(response.error);
            return;
        }
        if (response.value == "Error") {
            alert('Nit Inexistente');
            var nit = document.getElementById("<%=datBen.ClientID%>");
            nit.value = "";
        }
        else {
            var resultado = response.value;
            var obj = document.getElementById("<%=datBena.ClientID%>");
            obj.value = resultado;
        }
    }

    function Cargar_Datos2(val) {
        if (val.value == "")
            return;
        else
            EncabezadoRecibo.Retornar_Nombre2(val.value, Retornar_Nombre2_CallBack);
    }

    function Cambiar_Prefijo(obj) {
        if (obj.options.length > 0)
            EncabezadoRecibo.Cambiar_Numero(obj.value, Cambiar_Numero_RollBack);
        else
            return;
    }

    function Cambiar_Numero_RollBack(response) {
        if (response.error != null) {
            alert(response.error);
            return;
        }
        var valor = response.value;
        var objNum = document.getElementById("<%=numeroRecibo.ClientID%>");
        objNum.value = valor;
    }

    function SelCliente(obj) {
        var tpR = document.getElementById("<%=tipoRecibo.ClientID%>").value;
        if (tpR == 'V')
            ModalDialog(obj, 'SELECT M.mnit_nit AS Nit, CASE WHEN M.TNIT_TIPONIT = \'N\' THEN M.mnit_apellidos ELSE M.mnit_apellidos CONCAT \' \' CONCAT COALESCE(M.mnit_apellido2,\'\') CONCAT \' \' CONCAT M.mnit_nombres CONCAT \' \' CONCAT COALESCE(M.mnit_NOMBRE2,\'\') END AS Nombre,M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M,pciudad P WHERE M.pciu_codigo=P.pciu_codigo AND M.mnit_nit IN (SELECT MNIT_NIT FROM MPEDIDOVEHICULO WHERE TEST_TIPOESTA IN(10,20)) ORDER BY mnit_nit', 1, new Array())
        else 
        {
            if (tpR == 'O')
                ModalDialog(obj, 'SELECT M.mnit_nit AS Nit, CASE WHEN M.TNIT_TIPONIT = \'N\' THEN M.mnit_apellidos ELSE M.mnit_apellidos CONCAT \' \' CONCAT COALESCE(M.mnit_apellido2,\'\') CONCAT \' \' CONCAT M.mnit_nombres CONCAT \' \' CONCAT COALESCE(M.mnit_NOMBRE2,\'\') END AS Nombre,M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M,pciudad P WHERE M.pciu_codigo=P.pciu_codigo AND M.mnit_nit IN (select distinct mnit_Nit from dbxschema.MOBLIGACIONFINANCIERa m, dbxschema.pcuentacorriente pcc,  dbxschema.pbanco pc where m.pcue_codigo = pcc.pcue_codigo and pcc.pban_banco = pc.pban_codigo) ORDER BY mnit_nit', 1, new Array())
            else
                ModalDialog(obj, 'SELECT M.mnit_nit AS Nit, CASE WHEN M.TNIT_TIPONIT = \'N\' THEN M.mnit_apellidos ELSE M.mnit_apellidos CONCAT \' \' CONCAT COALESCE(M.mnit_apellido2,\'\') CONCAT \' \' CONCAT M.mnit_nombres CONCAT \' \' CONCAT COALESCE(M.mnit_NOMBRE2,\'\') END AS Nombre,M.mnit_direccion AS Direccion,P.pciu_nombre AS Ciudad,M.mnit_telefono AS Telefono FROM mnit M,pciudad P WHERE M.pciu_codigo=P.pciu_codigo ORDER BY mnit_nit', 1, new Array());
        }
    }


    function MostrarCreditos(obj) {
        var obj1 = document.getElementById("<%=datCli.ClientID%>");
        var obj2 = document.getElementById("<%=datBen.ClientID%>");
        var objC = document.getElementById("<%=ddlCredito.ClientID%>");
        var obj3 = document.getElementById("<%=concepto.ClientID%>");
        if (obj.value == 'F') {
            if (objC.value.length > 0)
                TraerCredito(objC.value);
            dvCredito.style.visibility = 'visible';
            obj1.readOnly = true;
            obj2.readOnly = true;
            obj1.onblur = "return;";
            obj1.dblclick = "return;";
            obj2.onblur = "return;";
            obj2.dblclick = "return;";
        }
        else {
            dvCredito.style.visibility = 'hidden';
            obj1.readOnly = false;
            obj2.readOnly = false;
            obj1.onblur = obj1b;
            obj1.dblclick = obj1db;
            obj2.onblur = obj2b;
            obj2.dblclick = obj2db;
            obj3.value = '';
        }
    }

    function MostrarCreditos_CallBack(response) {
        var obj1 = document.getElementById("<%=datCli.ClientID%>");
        var obj1a = document.getElementById("<%=datClia.ClientID%>");
        var obj2 = document.getElementById("<%=datBen.ClientID%>");
        var obj2a = document.getElementById("<%=datBena.ClientID%>");
        var obj3 = document.getElementById("<%=concepto.ClientID%>");
        var objC = document.getElementById("<%=ddlCredito.ClientID%>");
        var ds = response.value;
        if (ds != null && typeof (ds) == "object" && ds.Tables != null) {
            obj1.value = ds.Tables[0].Rows[0].NITFINANCIERA;
            obj2.value = ds.Tables[0].Rows[0].NITCLIENTE;
            obj1a.value = ds.Tables[0].Rows[0].NOMBREFINANCIERA;
            obj2a.value = ds.Tables[0].Rows[0].NOMBRECLIENTE;
            obj3.value = 'Cancela credito: ' + objC.value + ' ' + obj1a.value + ' - ' + obj2a.value;
        }
    }

    function TraerCredito(numC) {
        EncabezadoRecibo.TraerCredito(numC, MostrarCreditos_CallBack);
    }

    function TraerCreditoA() {
        var objC = document.getElementById("<%=ddlCredito.ClientID%>");
        TraerCredito(objC.value);
    }

    MostrarCreditos(document.getElementById("<%=tipoRecibo.ClientID%>"));

</script>
