<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.CrearLlamada.ascx.cs" Inherits="AMS.Tools.CrearLlamada" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<P>En este modulo usted puede ingresar las llamadas personales que se reciben para 
	cualquiera de los empleados de
	<asp:Label ID="lbEmpresa" Runat="server"></asp:Label>
</P>
<table id="Table" class="filtersIn">
	<tr>
		<td>Acción a tomar :
		</td>
		<td><asp:dropdownlist id="ddlaccion" Runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Fecha de la Llamada :
		</td>
		<td><asp:textbox id="tbfecha" Runat="server" onkeyup="DateMask(this)"></asp:textbox><asp:requiredfieldvalidator id="rfv1" runat="server" ControlToValidate="tbfecha">*</asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td>Hora de la Llamada :
		</td>
		<td><asp:textbox id="tbhora" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv2" runat="server" ControlToValidate="tbhora">*</asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td>Persona que Llama (DE) :
		</td>
		<td><asp:textbox id="tbpersona" Runat="server" MaxLength="100" class="tgrande"></asp:textbox><asp:requiredfieldvalidator id="rfv3" runat="server" ControlToValidate="tbpersona">*</asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td>
			Mensaje :
		</td>
		<td>
			<asp:TextBox ID="tbmensaje" Runat="server" MaxLength="250" TextMode="MultiLine" class="tgrande"></asp:TextBox>
			<asp:RequiredFieldValidator id="rfv4" runat="server" ControlToValidate="tbmensaje">*</asp:RequiredFieldValidator>
		</td>
	</tr>
</table>
<P>
	<asp:Button id="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click"></asp:Button>&nbsp;
	<asp:Button id="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="False"></asp:Button></P>
<P>
	<asp:Label id="lb" runat="server"></asp:Label></P>
