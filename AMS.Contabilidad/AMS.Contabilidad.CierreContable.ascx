<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Contabilidad.CierreContable.ascx.cs" Inherits="AMS.Contabilidad.CierreContable" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<P>
	<asp:Label id="Label1" runat="server" Width="22px" Height="16px">Mes Cierre</asp:Label>&nbsp;
	<asp:TextBox id="TxBMes" class="tpequeno" runat="server" ToolTip="mes de cierre"></asp:TextBox></P>
<P>
	<asp:Label id="Label2" runat="server" Width="22px" Height="16px">A�o Cierre</asp:Label>&nbsp;
	<asp:TextBox id="TxBAno" class="tpequeno" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button id="BtnCierre" runat="server" Text="Generar Cierre" 
        onclick="BtnCierre_Click" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')"></asp:Button></P>
        </td>
        </tr>
        </table>
        </fieldset>

			 
<script type="text/javascript">
 function clickOnce(btn, msg)
        {
            // Comprobamos si se est� haciendo una validaci�n
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se est� haciendo una validaci�n, volver si �sta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el bot�n sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Ser� el texto que muestre el bot�n mientras est� deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Cerrando Mes...'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }	
</script>		                        