<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.ModificarUsuario.ascx.cs" Inherits="AMS.Tools.ModificarUsuario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table id="Table" class="">
		<TR>
			<TD>Login :
			</TD>
			<TD>
				<asp:Label id="lbLogin" runat="server"></asp:Label></TD>
		</TR>
		<TR>
			<TD>Nombre : 
			</TD>
			<TD>
				<asp:textbox id="tbnombre" class="tgrande" Runat="server" MaxLength="100"></asp:textbox>
				<asp:RequiredFieldValidator id="rfv1" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="tbnombre"
					Display="Dynamic"></asp:RequiredFieldValidator></TD>
		</TR>
		<TR>
			<TD>Perfil de Usuario :
			</TD>
			<TD>
				<asp:dropdownlist id="ddlperfil" class="dmediano" Runat="server"></asp:dropdownlist></TD>
		</TR>
        <asp:placeholder id="plcClaves" runat="server" visible="true" >
		<div  runat="server" id="txtPassActual" >
            <TR>
			    <TD>Contraseña Actual :
			    </TD>
			    <TD>
				    <asp:textbox id="tbcont" Runat="server" TextMode="Password"></asp:textbox>
				    <asp:RequiredFieldValidator id="rfv2" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="tbcont"
					    Display="Dynamic"></asp:RequiredFieldValidator></TD>
		    </TR>
        </div>
		<TR>
			<TD>Digite su nueva contraseña :
			</TD>
			<TD>
				<asp:textbox id="tbcontn" Runat="server" TextMode="Password"></asp:textbox>
				<asp:RequiredFieldValidator id="rfv3" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="tbcontn"
					Display="Dynamic"></asp:RequiredFieldValidator></TD>
		</TR>
		<TR>
			<TD>Confirme su contraseña nueva :
			</TD>
			<TD>
				<asp:textbox id="tbcontnc" Runat="server" TextMode="Password"></asp:textbox>
				<asp:RequiredFieldValidator id="rfv4" runat="server" ErrorMessage="Campo Obligatorio. " ControlToValidate="tbcontnc"
					Display="Dynamic"></asp:RequiredFieldValidator>
				<asp:CompareValidator id="cv1" runat="server" ErrorMessage="La contraseña es distinta" ControlToValidate="tbcontnc"
					Display="Dynamic" ControlToCompare="tbcontn"></asp:CompareValidator></TD>
		</TR>
        </asp:placeholder>
		<TR>
			<TD>Nuevo Nit Asignado de la empresa externa con acceso a nuestro sistema (si Aplica) :
			</TD>
			<TD>
				<asp:textbox id="txtNit" ondblclick="ModalDialog(this,'SELECT NIT.mnit_nit AS NIT, NIT.mnit_nombres CONCAT \' \' CONCAT NIT.mnit_apellidos AS NOMBRE FROM mnit NIT',new Array(),1)"
					Runat="server" class="tmediano"></asp:textbox></TD>
		</TR>
        <div id="seguridadIP" runat="server">
            <tr>
				<td>
					IPs de acceso validas:</td>
				<td>
					<asp:TextBox id="txtIPs"  class="tgrande" runat="server" placeholder="###.###.###.###" Enabled="false"></asp:TextBox>
                    <helpinfo info="*Dejar vacio para acceso libre. &#xa;&#xa;*Ingrese mas de una IP separandolas &#xa;con (punto y coma), Ej:  &#xa;192.168.1.3 ; 190.50.50.50 ; 192.255.0.20">
                    </helpinfo>
				</td>
			</tr>
        </div>
	</TABLE>

<P><asp:button id="btnConfirmar" runat="server" Text="Confirmar Cambios" onclick="btnConfirmar_Click" UseSubmitBehavior="false" 
 OnClientClick="clickOnce(this, 'Cargando...')">
</asp:button>&nbsp;<asp:button id="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False" onclick="btnCancelar_Click"></asp:button></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
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
