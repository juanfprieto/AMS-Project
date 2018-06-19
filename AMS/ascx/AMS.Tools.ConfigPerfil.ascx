<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.ConfigPerfil.ascx.cs" Inherits="AMS.Tools.ConfigPerfil"%>
<%@ Register TagPrefix="uc1" TagName="Seleccionar" Src="AMS.Tools.Seleccionar.ascx" %>
<TABLE>
	<TR>
		<TD>
			Seleccione un Perfil:
		</TD>
		<td>
			<asp:DropDownList id="Perfil" runat="server" AutoPostBack="True" onselectedindexchanged="Entidad_SelectedIndexChanged"></asp:DropDownList>
		</td>
	</TR>
</TABLE>
<p><uc1:Seleccionar id="Seleccion" runat="server"></uc1:Seleccionar></p>
<P>
	<asp:Label id="lb" runat="server"></asp:Label></P>
