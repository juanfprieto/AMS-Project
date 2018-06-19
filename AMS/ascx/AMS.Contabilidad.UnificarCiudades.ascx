<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Contabilidad.UnificarCiudades.ascx.cs" Inherits="AMS.Contabilidad.AMS_Contabilidad_UnificarCiudades" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>
<asp:Label id="warning" runat="server"> <b>Advertencia:</b> Este proceso
    reemplazará la ciudad errada por la ciudad correcta en toda la base de datos, por lo tanto
    usted debe tener la absoluta certeza que su desición es la adecuada ya que este proceso
    no es reversible. </asp:Label>
</p>
</td>
</tr>
<P></P>
<p>
<table id="Table" class="filtersIn">
<tr>
<td>
	Ciudad correcta:
    <asp:TextBox id="txtCiudadcorrecta" runat="server" ondblclick="ModalDialog(this,'Select pciu_codigo, pciu_nombre from DBXSCHEMA.PCIUDAD ORDER BY PCIU_CODIGO',new Array(),1)"></asp:TextBox>
	<br>
</td>
<td></td>
</tr>
<tr>
<td>
	Ciudad errada:
    <asp:TextBox id="txtCiudaderrada" runat="server" ondblclick="ModalDialog(this,'Select pciu_codigo, pciu_nombre from DBXSCHEMA.PCIUDAD ORDER BY PCIU_CODIGO',new Array(),1)"></asp:TextBox>
	</td>
<td></td>
</tr>
    </table>
</p>
<p>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button id="unificar" onclick="unificar_Ciudades" runat="server" Text="Unificar Ciudades"></asp:Button>
</p>
<asp:Label id="lb" runat="server"></asp:Label>
</table>
</fieldset>
