<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.Actualizaciones.ascx.cs" Inherits="AMS.Tools.AMS_Tools_Actualizaciones" %>
<style>
    .nicEdit-main{
        background-color:white;
        max-height: 600px;
        overflow: scroll !important;
        padding: 30px;
    }
    td img:hover
    {
        transform:none;
    }
</style>
<script type="text/javascript">
    $(document).on("ready", inicio);
    function inicio() {
       
    }

    function carga_modulo(obj)
    {
        //document.getElementById("=btnGuardar.ClientID").disabled = true;
        $("#" + "<%= ddlHijo.ClientID %>").empty();
        $("#" + "<%= ddlMenu.ClientID %>").empty();
        AMS_Tools_Actualizaciones.cargar_Opcion(obj, cargar_Opcion_CallBack);
    }

    function cargar_Opcion_CallBack(response)
    {
        var respuesta = response.value;
        //alert(respuesta.Tables[0].Rows[0].CARPETA); comprobamos que funciona

        if (respuesta != null)
        {
            var mySelect = document.getElementById("<%= ddlHijo.ClientID %>");
            if (mySelect.options.length > 0)
            {
                $("#" + "<%= ddlHijo.ClientID %>").empty();

            }
            if(respuesta.Tables[0].Rows.length > 0)
                mySelect.options[mySelect.options.length] = new Option("Seleccione...", "0");
            for (var i = 0; i < respuesta.Tables[0].Rows.length; i++) {
                mySelect.options[mySelect.options.length] = new Option(respuesta.Tables[0].Rows[i].CARPETA, respuesta.Tables[0].Rows[i].SMEN_CARPETA);
            }
        }
        
    }

    function carga_Menu(obj)
    {
        if (obj === "0")
        {
            $("#" + "<%= ddlMenu.ClientID %>").empty();
        }
        AMS_Tools_Actualizaciones.cargar_Menu(obj, cargar_Menu_CallBack);
    }

    function cargar_Menu_CallBack(response)
    {
        var respuesta = response.value;

        var mySelect = document.getElementById("<%= ddlMenu.ClientID %>");
        if (mySelect.options.length > 0) {
            $("#" + "<%= ddlMenu.ClientID %>").empty();
        }

        if (respuesta.Tables[0].Rows.length > 0)
            mySelect.options[mySelect.options.length] = new Option("Seleccione...", "0");
        for (var i = 0; i < respuesta.Tables[0].Rows.length; i++) {
            mySelect.options[mySelect.options.length] = new Option(respuesta.Tables[0].Rows[i].OPCION, respuesta.Tables[0].Rows[i].SMEN_OPCION);
        }
        document.getElementById("<%= txtMensaje.ClientID %>").disabled = false;
        if(document.getElementById("<%= txtMensaje.ClientID %>").value.length > 19)
            document.getElementById("<%=btnGuardar.ClientID %>").disabled = false;
    }

    function carga_Manual(index)
    {
        var ddlPadreObj = document.getElementById("<%= ddlPadre.ClientID %>");
        var ddlHijo = document.getElementById("<%= ddlHijo.ClientID %>");
        var txtTabla = document.getElementById("<%= txtTabla.ClientID %>");

        AMS_Tools_Actualizaciones.cargar_Manual(ddlPadreObj.value + "." + ddlHijo.value, txtTabla.value, index, cargar_Manual_CallBack);
    }

    function cargar_Manual_CallBack(response)
    {
        var respuesta = response.value;
        
        if (respuesta[0] != "" && respuesta[0] != undefined)
        {
            $(".nicEdit-main").html(respuesta[0]);
            $("#divFecha").text(respuesta[1]);
        }
        else
        {
            var imglupa = document.getElementById("<%= imglupa.ClientID %>");
            if (respuesta[1] == "T")
            {
                imglupa.style.visibility = 'visible';
            }
            else
            {
                imglupa.style.visibility = 'hidden';
            }
            
            $(".nicEdit-main").html("Sin registro...");
            $("#divFecha").text("-");
        }
    }

    function validarText()
    {
        var textArea = document.getElementById("<%= txtMensaje.ClientID %>");
        alert(textArea.value.length);
       
    }

    function grabar_datos(obj)
    {
        //$(obj).prop('disabled', true);
        var ddlModulo = document.getElementById("<%= ddlPadre.ClientID %>");
        var ddlClase = document.getElementById("<%= ddlHijo.ClientID %>");
        var ddlVersion = document.getElementById("<%= ddlVersion.ClientID %>");
        var txtTabla = document.getElementById("<%= txtTabla.ClientID %>");

        //var obj = objuno.options[objuno.selectedIndex].text;
        //var objj = objdos.options[objdos.selectedIndex].text;
        //var codigoMenu = objtres.options[objtres.selectedIndex].value;
        var nombreObjeto = ddlModulo.value + "." + ddlClase.value;
        //var esteSi = obj + " - " + objj + " - " + objjj;

        //alert($(".nicEdit-main").html()); 
        AMS_Tools_Actualizaciones.guardar(nombreObjeto, $(".nicEdit-main").html(), ddlVersion.value, txtTabla.value, cargar_rta_CallBack);
    }

    function cargar_rta_CallBack(response)
    {
        respuesta = response.value;
        if (respuesta != "0")
        {
            document.getElementById("<%= txtMensaje.ClientID %>").value = "";
            $("#" + "<%= ddlMenu.ClientID %>").empty();
            $("#" + "<%= ddlHijo.ClientID %>").empty();
            $("#" + "<%= btnGuardar.ClientID %>").prop('disabled', false);
            //document.getElementById().options[0].value = "0";
            document.getElementById("<%= ddlPadre.ClientID %>").getElementsByTagName('option')[0].selected = 'selected';
            $(".nicEdit-main").html("");
            alert('El registro se modificó correctamente!');
        }
        else {
            alert('Ocurrió un problema al intentar registrar la Actualización');
        }
    }

    function abrirEmergente(obj) {
        var txtTabla = document.getElementById('_ctl1_' + obj);
        ModalDialog(txtTabla, 'select name from SYSIBM.SYSTABLES where creator = \'DBXSCHEMA\' and name not like \'V%\' order by name', new Array());
    }
</script>
<script src="../js/nicEdit-latest.js" type="text/javascript"></script>
<script type="text/javascript">
    //bkLib.onDomLoaded(nicEditors.allTextAreas);
    bkLib.onDomLoaded(function () {
        new nicEditor({
            fullPanel: true, onSave: function (content, id, instance) {
                alert('save button clicked for element ' + id + ' = ' + content);
            }
        }).panelInstance('<%= txtMensaje.ClientID %>');

    });
</script>

<fieldset>
    <asp:Table runat="server" >
        <asp:TableRow>
            <asp:TableCell>
                Versión: &nbsp; <asp:DropDownList runat="server" ID="ddlVersion" Width="90px" ></asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                Modulo: &nbsp; <asp:DropDownList runat="server" ID="ddlPadre" AutoPostBack="false" OnChange="carga_modulo(this.value);" Width="250px"></asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                Clase: &nbsp; <asp:DropDownList runat="server" ID="ddlHijo" Width="250px" AutoPostBack="false" OnChange="carga_Manual(0);"></asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                Tabla: &nbsp; <asp:TextBox runat="server" id="txtTabla" class="tmediano" ReadOnly="true" onBlur="carga_Manual(1);" ></asp:TextBox>
                <asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('txtTabla');" Style ="visibility:hidden"></asp:Image>
            </asp:TableCell>
            <asp:TableCell>
                &nbsp; <asp:DropDownList runat="server" ID="ddlMenu" OnChange="carga_Manual(this.value);" Width="400px" visible="false"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table><br />
    <asp:Table runat="server" style="font-size: large;">
        <asp:TableRow>
            <asp:TableCell>
                Ultima Modificación: <div id="divFecha" style="display: inline-block;"></div><br />
                <asp:TextBox ID="txtMensaje" placeholder="mínimo 20 caracteres" TextMode="MultiLine" runat="server" Rows="7" style="font-family: cursive; font-style: italic; max-width: 1000px; max-height: 800px; font-size: medium" Width="800px" Height="600px" onBlur="validarText();" Enabled="false"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br /> <br />
    <p>
        <asp:Button id="btnGuardar" Text="Guardar" runat="server" style="position:relative; left:50%;" OnClientClick="grabar_datos(this);return false;" CssClass="noEspera"/>
    </p>
</fieldset>
