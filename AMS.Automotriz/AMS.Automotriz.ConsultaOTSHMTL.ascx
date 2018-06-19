<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.ConsultaOTSHMTL.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ConsultaOTSHMTL" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
    <table id="Table" class="filtersIn">
	    <tr>
		    <td>
			    Escoja el prefijo de la Orden de Trabajo
		    </td>
		    <td>
			    <asp:DropDownList ID="ddlPreOT" Runat="server" AutoPostBack="True" onselectedindexchanged="ddlPreOT_SelectedIndexChanged"></asp:DropDownList>
		    </td>
	    </tr>
	    <tr>
		    <td>
			    Escoja o digite el número de la Orden de Trabajo
		    </td>
		    <td>
			    <asp:DropDownList ID="ddlNumOT" Runat="server"></asp:DropDownList>
		    </td>
	    </tr>
	    <tr>
		    <td>
			    <asp:Button ID="btnEnviar" Runat="server" Text="Enviar" onclick="btnEnviar_Click"></asp:Button>
		    </td>
	    </tr>
    </table>
</fieldset>