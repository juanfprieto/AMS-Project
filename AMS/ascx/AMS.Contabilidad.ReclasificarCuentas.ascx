<%@ Control Language="c#" codebehind="AMS.Contabilidad.ReclasificarCuentas.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.ReclasificarCuentas" %>

<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>
	<asp:Label id="warning" runat="server"> <b>Advertencia:</b> Este proceso
    reemplazará la cuenta errada por la cuenta correcta en toda la base de datos, por
    lo tanto usted debe tener la absoluta certeza que su desición es la adecuada ya que
    este proceso no es reversible. </asp:Label>
</p>
</td>
</tr>
<p>
<table id="Table1" class="filtersIn">
<tr>
<td>
	Cuenta correcta:
	<asp:DropDownList id="rightAccount" runat="server"></asp:DropDownList>
	<br>
</td>
<td></td>
</tr>
<tr>
<td>
	Cuenta errada: &nbsp;&nbsp;
	<asp:DropDownList id="errorAccount" runat="server"></asp:DropDownList>
</td>
<td></td>
</tr>
</table>
</p>
<p>
	<asp:Button id="reclasificarButton" onclick="ReclasificarButton_onclick" runat="server" Text="Reclasificar cuentas ahora"></asp:Button>
</p>
<asp:Label id="lb" runat="server"></asp:Label>
</table>
</fieldset>
