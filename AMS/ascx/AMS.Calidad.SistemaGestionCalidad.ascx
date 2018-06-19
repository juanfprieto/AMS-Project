<%@ Control Language="c#" AutoEventWireup="True" 
Codebehind="AMS.Calidad.SistemaGestionCalidad.ascx.cs" Inherits="AMS.Calidad.SistemaGestionCalidad" %>
<TABLE>
	<TR>
		<TD>
			<asp:Label id="Label1" runat="server">Procedimiento</asp:Label></TD>
		<TD></TD>
		<TD>
			<asp:Label id="Label2" runat="server">Formato</asp:Label></TD>
	</TR>
	<TR>
		<TD>
			<asp:DropDownList id="ddlproc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlproc_SelectedIndexChanged"></asp:DropDownList></TD>
		<TD>&nbsp;&nbsp;</TD>
		<TD>&nbsp;
			<asp:DropDownList id="ddlform" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlform_SelectedIndexChanged"></asp:DropDownList></TD>
	</TR>
</TABLE>
<P></P>
<asp:Button id="Button1" runat="server" Text="Reiniciar" OnClick="Button1_Click"></asp:Button>
