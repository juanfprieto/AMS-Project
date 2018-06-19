<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.DiccionarioPalabras.ascx.cs" Inherits="AMS.Tools.DiccionarioPalabras" %>

<FIELDSET>
    <LEGEND>Diccionario de Palabras</LEGEND>
	<TABLE class="filtersIn" >
		<TR>
		    <TD>
                Palabra Clave :<br> 
                <asp:TextBox id="txtPalabraClave" class="tmediano" ondblclick="ModalDialog(this,sqlModalDiccionario,new Array(),1)" onBlur="onChange_CargaDiccionario(this);" runat="server" ReadOnly="true"></asp:TextBox> <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('txtPalabraClave');" style="cursor:pointer"></asp:Image>
            </TD>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                Palabras Asociadas :<br>
                <asp:TextBox id="txtPalabrasAso" class="amediano" runat="server" TextMode="MultiLine" MaxLength="200" Height="150px" ReadOnly="true"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox id="txtNuevaPalabra" class="tmediano" runat="server" placeholder="Escriba una nueva palabra..."></asp:TextBox><asp:Button id="btnAgregar" runat="server" Text="Agregar Palabra" onClientClick="press_AgregarPalabra(this); return false;" class="noEspera"></asp:Button><br>
                <asp:DropDownList id="ddlListaPalabrasAso" class="dmediano" runat="server"></asp:DropDownList><asp:Button id="btnEliminar" runat="server" Text="Eliminar Palabra" onClientClick="press_EliminarPalabra(this); return false;" class="noEspera"></asp:Button>
            </td>
        </tr>
    </TABLE>
</FIELDSET>

<script type="text/javascript">
    var sqlModalDiccionario = "SELECT DISTINCT UPPER(PCL.PCLA_NOMBRE) as nombre " +
                                "FROM DBXSCHEMA.PCLASEVEHICULO PCL "+
                                "RIGHT JOIN DBXSCHEMA.PCATALOGOVEHICULO PC ON PC.PCLA_CODIGO = PCL.PCLA_CODIGO  union "+
                                "SELECT DISTINCT UPPER(PMV.PMAR_NOMBRE) " +
                                "FROM DBXSCHEMA.PMARCA PMV "+
                                "RIGHT JOIN DBXSCHEMA.PCATALOGOVEHICULO PC ON PMV.PMAR_CODIGO = PC.PMAR_CODIGO union "+
                                "SELECT DISTINCT UPPER(PGC.PGRU_NOMBRE) " +
                                "FROM DBXSCHEMA.PGRUPOCATALOGO PGC "+
                                "RIGHT JOIN  DBXSCHEMA.PCATALOGOVEHICULO PC ON PGC.PGRU_GRUPO = PC.PGRU_GRUPO union "+
                                "SELECT DISTINCT UPPER(PSU.PSGRU_NOMBRE) " +
                                "FROM DBXSCHEMA.PSUBGRUPOVEHICULO PSU "+
                                "RIGHT JOIN  DBXSCHEMA.PCATALOGOVEHICULO PC ON PSU.PSGRU_GRUPO = PC.PSGRU_CODIGO union "+
                                "SELECT DISTINCT UPPER(PCA.PCAR_NOMBRE) " +
                                "FROM  DBXSCHEMA.PCARROCERIA PCA " +
                                "RIGHT JOIN DBXSCHEMA.PCATALOGOVEHICULO PC ON PC.PCAR_CODIGO = PCA.PCAR_CODIGO  order by NOMBRE";
    
    function abrirEmergente(obj) {
        var textoPalabraClave = document.getElementById('_ctl1_' + obj);
        ModalDialog(textoPalabraClave, sqlModalDiccionario, new Array(), 1);
    }

    function onChange_CargaDiccionario(obj) {
        DiccionarioPalabras.CargaDiccionario(obj.value, CargaDiccionario_CallBack);
    }
    function CargaDiccionario_CallBack(response) {
        var respuesta = response.value;
        var areaPalabras = document.getElementById("<%=txtPalabrasAso.ClientID%>");
        areaPalabras.value = respuesta;

        llenarDDLListaPalabrasAso(respuesta);
    }

    function press_AgregarPalabra(obj) {
        var palabraClave = document.getElementById("<%=txtPalabraClave.ClientID%>");
        var palabraNueva = document.getElementById("<%=txtNuevaPalabra.ClientID%>");
        var resultadoCallBack = DiccionarioPalabras.AgregarPalabra(palabraClave.value + "*" + palabraNueva.value, AgregarPalabras_CallBack);
    }
    function AgregarPalabras_CallBack(response) {
        var respuesta = response.value;
        var palabraClave = document.getElementById("<%=txtPalabraClave.ClientID%>");
        var palabraNueva = document.getElementById("<%=txtNuevaPalabra.ClientID%>");

        if (respuesta == "A")
            alert("Antes de agregar seleccione una palabra clave!");
        else if (respuesta == "B")
            alert("Antes de agregar escriba la palabra que desea relacionar.");
        else if (respuesta == "C")
            alert("La palabra que esta ingresando ya ha sido relacionada!");
        else if (respuesta == "D")
            alert("Error al almacenar en Base de Datos!");
        else {
            var areaPalabras = document.getElementById("<%=txtPalabrasAso.ClientID%>");
            areaPalabras.value = respuesta;
            llenarDDLListaPalabrasAso(respuesta);
            alert("La palabra: " + palabraNueva.value + " - Ha sigo relacionada correctamente con: " + palabraClave.value);
            palabraNueva.value = "";
        }
    }

    function press_EliminarPalabra(obj) {
        var palabraClave = document.getElementById("<%=txtPalabraClave.ClientID%>");
        var ddlPalabras = document.getElementById("<%=ddlListaPalabrasAso.ClientID%>");
        if (palabraClave.value != "")
        {
            DiccionarioPalabras.EliminarPalabra(palabraClave.value + "*" + ddlPalabras.options[ddlPalabras.selectedIndex].value, EliminarPalabras_CallBack);
        }
    }
    function EliminarPalabras_CallBack(response) {
        var respuesta = response.value;
        var palabraClave = document.getElementById("<%=txtPalabraClave.ClientID%>");
        var palabraNueva = document.getElementById("<%=txtNuevaPalabra.ClientID%>");

        if (respuesta == "A")
            alert("Antes de eliminar seleccione una palabra clave!");
        else if (respuesta == "B")
            alert("Seleccione una palabra en la lista antes de eliminar.");
        else if (respuesta == "C")
            alert("Error en Base de Datos");
        else {
            var areaPalabras = document.getElementById("<%=txtPalabrasAso.ClientID%>");
            areaPalabras.value = respuesta;
            llenarDDLListaPalabrasAso(respuesta);
            alert("Se ha eliminado la palabra seleccionada");
        }
    }

    function llenarDDLListaPalabrasAso(contenido) {
        var idSelect = "<%=ddlListaPalabrasAso.ClientID%>";
        $('#' + idSelect + ' option').remove();
        var arrayContenido = contenido.split(",");

        for (var i = 0; i < arrayContenido.length - 1; i++) {
            $('#' + idSelect).append('<option>' + arrayContenido[i] + '</option>');
        }
    }

</script>