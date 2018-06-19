<%@ Control Language="c#" codebehind="AMS.Contabilidad.UnificarNits.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.UnificarNits" %>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>
	<asp:Label id="warning" runat="server"> <b>Advertencia:</b> Este proceso
    reemplazará el nit errado por el nit correcto en toda la base de datos, por lo tanto
    usted debe tener la absoluta certeza que su desición es la adecuada ya que este proceso
    no es reversible. </asp:Label>
</p>
</td>
</tr>
<p>
<table id="Table" class="filtersIn">
<tr>
<td>
	Nit correcto:
	<asp:TextBox id="txtNitcorrecto" runat="server" ondblclick="ModalDialog(this,'SELECT NIT.mnit_nit AS NIT, NIT.mnit_nombres CONCAT \' \' CONCAT NIT.mnit_apellidos AS NOMBRE FROM mnit NIT ',new Array(),1)"></asp:TextBox>
</td>
<td></td>
</tr>
<tr>
<td>
	Nit errado:
	<asp:TextBox id="txtNiterrado" runat="server" ondblclick="ModalDialog(this,'SELECT NIT.mnit_nit AS NIT, NIT.mnit_nombres CONCAT \' \' CONCAT NIT.mnit_apellidos AS NOMBRE FROM mnit NIT ',new Array(),1)"></asp:TextBox>
</td>
<td></td>
</tr>
</table>
</p>
<p>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="reclasificarButton" onclick="ReclasificarButton_onclick" runat="server" Text="Unificar Nit" OnClientClick="espera();"></asp:Button>
</p>
<asp:Label id="lb" runat="server"></asp:Label>
</table>
</fieldset>
