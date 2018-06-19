<%@ Register TagPrefix="uc1" TagName="Seleccionar" Src="AMS.Tools.Seleccionar.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.CambiarVisibilidad.ascx.cs" Inherits="AMS.Tools.CambiarVisibilidad" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table id="Table" class="filtersIn">
	<TR>
		<TD>
			Seleccione un Perfil:
		</TD>
		<td>
			<asp:DropDownList id="Perfil" runat="server" AutoPostBack="True" onselectedindexchanged="Entidad_SelectedIndexChanged"></asp:DropDownList>
		</td>
	</TR>
</table>
<P>
	<uc1:Seleccionar id="Seleccion" runat="server"></uc1:Seleccionar>
	<br>
</P>
<P>
	<asp:label id="Mensages" Runat="server"></asp:label></P>
    </fieldset>
