<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.AsignarPerfil.ascx.cs" Inherits="AMS.Tools.AsignarPerfil" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="Seleccionar" Src="AMS.Tools.Seleccionar.ascx" %>
<fieldset>
<table id="Table" class="filtersIn">
	<TR>
		<TD>
			Seleccione un Perfil :&nbsp;
		</TD>
		<td>
			<asp:dropdownlist id="Perfil" runat="server" AutoPostBack="True" onselectedindexchanged="Entidad_SelectedIndexChanged"></asp:dropdownlist>
		</td>
	</TR>
</table>
<P>
	<uc1:seleccionar id="Seleccion" runat="server"></uc1:seleccionar></P>
<P>
	<asp:label id="Mensajes" runat="server"></asp:label></P>
    </fieldset>
     