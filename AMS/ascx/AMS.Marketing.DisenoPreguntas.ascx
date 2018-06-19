<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Marketing.DisenoPreguntas.ascx.cs" Inherits="AMS.Marketing.DisenoPreguntas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<%@ Register TagPrefix="uc1" TagName="Seleccionar" Src="AMS.Tools.Seleccionar.ascx" %>
<TABLE id="Table1" class="filtersIn">
	<TR>
		<TD>
			Seleccione una Pregunta :&nbsp;
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
