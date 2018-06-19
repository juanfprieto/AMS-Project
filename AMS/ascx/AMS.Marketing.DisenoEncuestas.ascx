<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Marketing.DisenoEncuestas.ascx.cs" Inherits="AMS.Marketing.DisenoEncuestas" %>

<%@ Register TagPrefix="uc1" TagName="Seleccionar" Src="AMS.Tools.Seleccionar.ascx" %>
<TABLE id="Table1" class="filtersIn">
	<TR>
		<TD>
			Seleccione una Encuesta :&nbsp;
		</TD>
		<td>
			<asp:dropdownlist id="Perfil" runat="server" AutoPostBack="True" onselectedindexchanged="Entidad_SelectedIndexChanged"></asp:dropdownlist>
		</td>
	</TR>
</TABLE>
<P>
	<uc1:seleccionar id="Seleccion" runat="server"></uc1:seleccionar></P>
<P>
	<asp:label id="Mensajes" runat="server"></asp:label></P>
