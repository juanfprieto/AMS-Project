<%@ Control Language="c#" codebehind="AMS.Tools.CrearUsuarios.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.CrearUsuario" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script type="text/javascript">
    function validaLlave(obj)
    {
        var llave = document.getElementById('<%=loginUsuario.ClientID%>');
        var letra = obj.keyCode;
        if (letra == 190 || letra == 110)
        {
            llave.value = llave.value.substring(0, llave.value.length - 1);
        } else {
            return;
        }
        
    }
</script>
<fieldset>
	<p>
	</p>
    
	<legend>Datos del Nuevo Usuario</legend>
	<table class="main">
		<tbody>
			<tr>
				<td>
					Digite el Nombre del Nuevo Usuario :&nbsp;&nbsp;
				</td>
				<td>
					<asp:TextBox id="nombreUsuario" runat="server" class="tmediano"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Digite el Login del Nuevo Usuario :</td>
				<td>
					<asp:TextBox id="loginUsuario" runat="server" class="tmediano" onkeyup="validaLlave(event)"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Digite la Constraseña del Nuevo Usuario :&nbsp;
				</td>
				<td>
					<asp:TextBox id="contrasenaUsuario" runat="server" class="tmediano" TextMode="Password"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Confirme la Constraseña :&nbsp;&nbsp;</td>
				<td>
					<asp:TextBox id="verificarContrasena" runat="server" class="mediano" TextMode="Password"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Seleccione el Tipo de Perfil para este Usuario :</td>
				<td>
					<asp:DropDownList id="tipoPerfil" runat="server" class="dmediano"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					Introduzca el Nit de la empresa externa con acceso a nuestro sistema (si Aplica) para este Usuario :</td>
				<td>
					<asp:TextBox id="txtNit" class="tmediano" runat="server" ondblclick="ModalDialog(this,'SELECT NIT.mnit_nit AS NIT, NIT.mnit_nombres CONCAT \' \' CONCAT NIT.mnit_apellidos AS NOMBRE FROM mnit NIT',new Array(),1)"></asp:TextBox>
				</td>
			</tr>
            <div id="seguridadIP" runat="server">
                <tr>
				    <td>
					    IPs de acceso validas:</td>
				    <td>
					    <asp:TextBox id="txtIPs"  class="tgrande" runat="server" placeholder="###.###.###.###"></asp:TextBox>
                        <helpinfo info="*Dejar vacio para acceso libre. &#xa;&#xa;*Ingrese mas de una IP separandolas &#xa;con (punto y coma), Ej:  &#xa;192.168.1.3 ; 190.50.50.50 ; 192.255.0.20">
                        </helpinfo>
				    </td>
			    </tr>
            </div>
			<tr>
				<td>
					<asp:Button id="Aceptar" onclick="Aceptar_Nuevo_Usuario" runat="server" class="bmediano" Text="Aceptar"></asp:Button>
				</td>
				<td>
					<asp:Button id="cancelar" onclick="Cancelar" runat="server" class="bpequeno" Text="Cancelar"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
