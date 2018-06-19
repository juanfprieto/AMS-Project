<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Inventarios.importarCCA.ascx.cs" Inherits="AMS.Inventarios.importarCCA" %>

<fieldset>
    <table id="table" class="filters">
        <tbody>
            <tr>
            <td>
                <p>
                Proceso de ACTUALIZACION de PRECIOS, CREACION de ITEMS y realización de 
                SUSTITUCIONES a partir del archivo CAREDISK2 suministrado por la C.C.A.</p>
                <p>
                <asp:Label id="Label3" runat="server">Por favor indique la ruta del archivo para 
                importar la lista con nombre CAREDISK2 suministradas por la CCA:</asp:Label>
                </p>
                <p>
                Archivo a Utilizar :&nbsp;<input id="fDocument" type="file" runat="server">&nbsp;&nbsp;&nbsp;
	
                <asp:DropDownList id="tablaAct" runat="server" Visible="False"></asp:DropDownList>
                </p>

                <p>&nbsp;</p>
                <p style="width: 266px">
                Línea de los Items a realizar la actualización: &nbsp;&nbsp;
                <asp:DropDownList ID="DdlLineaItems" runat="server">
                </asp:DropDownList></p>

                <p style="width: 266px">
                Lista de Precios de los Items a Actualizar : &nbsp;&nbsp;
                <asp:DropDownList ID="DdlListaPrecios" runat="server">
                </asp:DropDownList></p>
 
                <p style="width: 266px">
                Documento para realizar las sustituciones : &nbsp;&nbsp;
                <asp:DropDownList ID="DdlAjusteSustitucion" runat="server">
                </asp:DropDownList></p>

                <p>&nbsp;</p>

                <p>
                Por favor, asegurese de haber seleccionado correctamente las opciones, de tener 
                Back-Up de la Base de Datos y de haber ELIMINADO la última línea del archivo que viene en BLANCO.
                El tiempo del proceso es 10 a 20 minutos aprox. dependiendo del performance de su servidor.</p>

                <p>
                <asp:Button id="actual3" onclick="actualizar" runat="server" Text="Actualizar" UseSubmitBehavior="false" 
                OnClientClick="clickOnce(this, 'Procesando Archivo de CCA...')"> </asp:Button>
                </p>

                <p>
                <asp:Label id="lb" runat="server"></asp:Label>
                </p>
                </td>
                </tr>
        </tbody>
    </table>
</fieldset>

<script type = 'text/javascript'>
    function clickOnce(btn, msg) {
        // Comprobamos si se está haciendo una validación
        if (typeof (Page_ClientValidate) == 'function') {
            // Si se está haciendo una validación, volver si ésta da resultado false
            if (Page_ClientValidate() == false) { return false; }
        }

        // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
        if (btn.getAttribute('type') == 'button') {
            // El atributo msg es totalmente opcional. 
            // Será el texto que muestre el botón mientras esté deshabilitado
            if (!msg || (msg = 'undefined')) { msg = 'Procesando Archivo de CCA...'; }

            btn.value = msg;

            // La magia verdadera :D
            btn.disabled = true;
        }

        return true;
    }
        </script>
