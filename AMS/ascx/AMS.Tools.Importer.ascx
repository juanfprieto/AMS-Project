<%@ Control Language="c#" codebehind="AMS.Tools.Importer.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.Importer" %>
<fieldset>
<p>
	<asp:Label id="Label1" runat="server">Esta es una herramienta creada para importar
    tablas completas de Archivos PLANOS con campos separados por coma (,). 
    Se grabarán los registros que esten correctos, 
    los registros que tengan algun error NO se grabarán en la tabla que usted seleccione para actualizar:</asp:Label>
</p>
<p>
	Archivo a Utilizar :&nbsp;<input id="fDocument" type="file" runat="server">&nbsp;&nbsp;&nbsp; 
	Tabla a Actualizar :
	<asp:DropDownList id="tablaAct" runat="server"></asp:DropDownList>
</p>
<p>
	<asp:Button id="actual" onclick="actualizar" runat="server" Text="Actualizar" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
<font color='#009900'></font>
</fieldset>
<script language=javascript>
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
                if (!msg || (msg='undefined')) { msg = 'Procesando...'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }

            return true;
        }	
</script>		
