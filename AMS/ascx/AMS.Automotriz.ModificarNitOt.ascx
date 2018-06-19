<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.ModificarNitOt.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ModificarNitOt" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<fieldset>
<table id="Table" class="filtersIn">
	<tbody>
		<tr>
			<td>
				Prefijo OT
			</td>
			<td>
				<asp:DropDownList id="ddlPrefijoOt" class="dmediano" runat="server" AutoPostBack="true"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				Numero de Orden
			</td>
			<td><asp:DropDownList id="ddlNumeroOrden" class="dpequeno" runat="server"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				Nuevo Nit
			</td>
			<td>
				<asp:TextBox id="txtNit" runat="server" class="tpequeno" onclick="ModalDialog(this,'SELECT NIT.mnit_nit AS NIT, NIT.mnit_nombres CONCAT \' \' CONCAT NIT.mnit_apellidos AS NOMBRE FROM mnit NIT ',new Array(),1)" ReadOnly="True"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Button Runat="server" ID="btnCambiar" Text="Modificar"  OnClick="CambiarNit"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>

<asp:Label id=lb runat="server"></asp:Label>
</fieldset>
