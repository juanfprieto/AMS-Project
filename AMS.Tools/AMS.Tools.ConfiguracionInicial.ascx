<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.ConfiguracionInicial.ascx.cs" Inherits="AMS.Tools.ConfiguracionInicial" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript">
	function Mostrar_Div(/*string*/obj)
	{
		var objeto=document.getElementById(obj);
		if(objeto!=null)
		{
			if(objeto.style.display=='none')
				objeto.style.display='';
			else if(objeto.style.display=='')
				objeto.style.display='none';
		}
	}
</script>
<P>Algunos de los siguientes valores aun no han sido configurados, por favor 
	realice dicha configuración para que el sistema funcione correctamente</P>
<table id="Table" class="filtersIn">
	<tr>
		<td>Configuración de Envio de Correo
		</td>
	</tr>
	<tr>
		<td></td>
	</tr>
	<tr>
		<td>E-mail desde el cual se enviaran los correos generados por 
			las aplicaciones del sistema :
		</td>
		<td><asp:textbox id="tbcorreo" MaxLength="256" class="tmediano" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv1" Display="Dynamic" ControlToValidate="tbcorreo" runat="server" ErrorMessage="Campo Obligatorio"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="rev1" Display="Dynamic" ControlToValidate="tbcorreo" runat="server" ErrorMessage="Correo Inválido"
				ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator></td>
	</tr>
	<tr>
		<td>Contraseña del correo :
		</td>
		<td>
			<a href="javascript:Mostrar_Div('divContCor');">Haga click si desea cambiar o 
				asignar la contraseña del correo</a>
			<div id="divContCor" style="DISPLAY: none">
				Digite la contraseña :
				<asp:textbox id="tbcont" Runat="server" TextMode="Password" />
			</div>
		</td>
	</tr>
	<tr>
		<td>Servidor de correo (Ej. hotmail.com, miempresa.com) :
		</td>
		<td><asp:textbox id="tbservidor" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv3" Display="Dynamic" ControlToValidate="tbservidor" runat="server" ErrorMessage="Campo Obligatorio"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td></td>
	</tr>
	<tr>
		<td>Configuración Regional
		</td>
	</tr>
	<tr>
		<td></td>
	</tr>
	<tr>
		<td>Su empresa maneja centavos? :
		</td>
		<td><asp:dropdownlist id="ddlcentavos" Runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Nombre de su moneda nacional
		</td>
		<td><asp:textbox id="tbmoneda" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv4" Display="Dynamic" ControlToValidate="tbmoneda" runat="server" ErrorMessage="Campo Obligatorio"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td></td>
	</tr>
	<tr>
		<td>Configuración de la Base de Datos</td>
	</tr>
	<tr>
		<td></td>
	</tr>
	<tr>
		<td>Adaptador de la Base de Datos :
		</td>
		<td><asp:dropdownlist id="ddlbd" Runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Nombre de su servidor de Base de Datos :
		</td>
		<td><asp:textbox id="tbserver" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv5" Display="Dynamic" ControlToValidate="tbserver" runat="server" ErrorMessage="Campo Obligatorio"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td>Nombre de la Base de Datos :
		</td>
		<td><asp:textbox id="tbbd" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv6" Display="Dynamic" ControlToValidate="tbbd" runat="server" ErrorMessage="Campo Obligatorio"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td>Usuario de la Base de Datos :
		</td>
		<td><asp:textbox id="tbusuario" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv7" Display="Dynamic" ControlToValidate="tbusuario" runat="server" ErrorMessage="Campo Obligatorio"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td>
			Contraseña de la Base de Datos :
		</td>
		<td>
			<a href="javascript:Mostrar_Div('divContBD')">Haga click si desea asignar o cambiar 
				la contraseña de la base de datos</a>
			<div id="divContBD" style="DISPLAY: none">
				Digite la contraseña :
				<asp:textbox id="tbcontbd" Runat="server" TextMode="Password"></asp:textbox>
			</div>
		</td>
	</tr>
	<tr>
		<td>Esquema :
		</td>
		<td><asp:textbox id="tbesq" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv9" Display="Dynamic" ControlToValidate="tbesq" runat="server" ErrorMessage="Campo Obligatorio"></asp:requiredfieldvalidator></td>
	</tr>
</table>
<P><asp:button id="btnGuardar" runat="server" Text="Guardar" OnClick="Guardar"></asp:button>&nbsp;&nbsp;
	<asp:button id="btnCancelar" runat="server" Text="Cancelar" OnClick="Cancelar"></asp:button></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
