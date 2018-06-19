<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.ReImpresionOT.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ReImpresionOT" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table id="Table" class="filtersIn">
	<tr>
		<td>
			Escoja el prefijo de la Orden de Trabajo
		</td>
		<td>
            <asp:DropDownList ID="ddlPreOT" class="dmediano" Runat="server" AutoPostBack="True"></asp:DropDownList>
			<%--<asp:DropDownList ID="ddlPreOT" class="dmediano" Runat="server" AutoPostBack="True" onselectedindexchanged="ddlPreOT_SelectedIndexChanged"></asp:DropDownList>--%>
		</td>
	</tr>
	<tr>
		<td>
			Escoja o digite el número de la Orden de Trabajo
		</td>
		<td>
            <asp:TextBox id="txtNumOT" runat="server"  class="tpequeno"></asp:TextBox>
			<%--<asp:DropDownList ID="ddlNumOT" class="dpequeno" Runat="server"></asp:DropDownList>--%>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Button ID="btnEnviar" Runat="server" Text="Enviar" onclick="btnEnviar_Click"></asp:Button>
		</td>
	</tr>
</table>

</fieldset>
