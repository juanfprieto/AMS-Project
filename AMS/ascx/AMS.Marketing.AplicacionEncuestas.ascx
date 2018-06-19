<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Marketing.AplicacionEncuestas.ascx.cs" Inherits="AMS.Marketing.AMS_Marketing_AplicacionEncuestas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<P>
	Escoja la encuesta :
	<asp:DropDownList id="ddlencuestas" class="dmediano" runat="server" AutoPostBack="True" onselectedindexchanged="ddlencuestas_SelectedIndexChanged" onChange="espera();"></asp:DropDownList>
</P>
<asp:PlaceHolder ID="plcEncuesta" Visible="false" runat="server">
    <P>
	    <table>
		    <tr>
			    <td>Formulario Nº :
			    </td>
			    <td><asp:label id="lbnum" Runat="server"></asp:label></td>
		    </tr>
		    <tr>
			    <td>
			    </td>
		    </tr>
		    <tr>
			    <td>
				    Fecha :
			    </td>
			    <td>
				    <asp:Label ID="lbfec" Runat="server"></asp:Label>
			    </td>
		    </tr>
		    <tr>
			    <td>
			    </td>
		    </tr>
		    <tr>
			    <td style="HEIGHT: 22px">
				    Código de la Encuesta :
			    </td>
			    <td style="HEIGHT: 22px">
				    <asp:Label ID="lbcod" Runat="server"></asp:Label>
			    </td>
		    </tr>
		    <tr>
			    <td>
			    </td>
		    </tr>
		    <tr>
			    <td>
				    Nombre de la Encuesta :
			    </td>
			    <td>
				    <asp:Label ID="lbnom" Runat="server"></asp:Label>
			    </td>
		    </tr>
		    <tr>
			    <td>
			    </td>
		    </tr>
		    <tr>
			    <td>
				    Objetivo de la Encuesta :
			    </td>
			    <td>
				    <asp:Label ID="lbobj" Runat="server"></asp:Label>
			    </td>
		    </tr>
	    </table>
    <p>&nbsp;</p>
    
    <P>
    <asp:Label id="LabelEnc" runat="server">Fecha de Encuesta: </asp:Label>
    <asp:textbox id="txtFechaEnc" onkeyup="DateMask(this)" runat="server" Width="80px"></asp:textbox><br><br>
    NIT:&nbsp;<asp:TextBox ReadOnly="true" id="txtNit" runat="server" 
    onclick="ModalDialog(this,'SELECT NIT.mnit_nit AS NIT, NIT.mnit_nombres CONCAT \' \' CONCAT NIT.mnit_apellidos AS NOMBRE FROM mnit NIT ',new Array(),1)" Width="100px">
    </asp:TextBox>&nbsp;<asp:TextBox id="txtNita" runat="server" Width="300px"></asp:TextBox>
    <br><br>
    
    <fieldset style="WIDTH: 270px" bgcolor="FFFFFF"><legend class="Legends">Datos del Encuestado (Opcional)</legend>
    <table  width="650" cellpadding="5" bgcolor="FFFFFF">
        <tr>
            <td>
                Nombre:
            </td>
            <td>
                <asp:textbox id="txtNom" runat="server" Width="200px"></asp:textbox>
            </td>
            <td>
                Apellido:
            </td>
            <td>
                <asp:textbox id="txtApell" runat="server" Width="200px"></asp:textbox>
            </td>
        </tr>
        <tr>
            <td>
                Telefono Fijo:
            </td>
            <td>
                <asp:textbox id="txtTel" runat="server" Width="200px"></asp:textbox>
            </td>
            <td>
                Celular:
            </td>
            <td>
                <asp:textbox id="txtCelular" runat="server" Width="200px"></asp:textbox>
            </td>
        </tr>
        <tr>
            <td>
                E-mail:
            </td>
            <td>
                <asp:textbox id="txtEmail" runat="server" Width="200px"></asp:textbox>
            </td>
    </table>
    </fieldset>
        
    <br><br><br>
 <asp:PlaceHolder id="phForm1" runat="server"></asp:PlaceHolder></P>
   
    <P>
    
	    <asp:Button id="btnGrabar" runat="server" Text="Grabar Encuesta" onclick="btnGrabar_Click" UseSubmitBehavior="false" 
 OnClientClick="clickOnce(this, 'Cargando...')">
	    </asp:Button>&nbsp;
	    <asp:Button id="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False" onclick="btnCancelar_Click"></asp:Button></P>
</asp:PlaceHolder>
 </td></tr></table>
<P>
	<asp:Label id="lb" runat="server"></asp:Label></P>

   </fieldset>


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
                if (!msg || (msg='undefined')) { msg = 'Procesando..'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
        </script>