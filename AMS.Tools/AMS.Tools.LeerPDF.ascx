<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.LeerPDF.ascx.cs" Inherits="AMS.Tools.LeerPDF" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
Tool<TABLE>
	<TR>
		<TD>
			<asp:Label id="Label1" runat="server">PROCEDIMIENTO</asp:Label></TD>
		<TD></TD>
		<TD>
			<asp:Label id="Label2" runat="server">FORMATO</asp:Label></TD>
	</TR>
	<TR>
		<TD>
			<asp:DropDownList id="ddlproc" runat="server" onselectedindexchanged="ddlproc_SelectedIndexChanged"></asp:DropDownList></TD>
		<TD>&nbsp;&nbsp;</TD>
		<TD>&nbsp;
			<asp:DropDownList id="ddlform" runat="server" onselectedindexchanged="ddlform_SelectedIndexChanged"></asp:DropDownList></TD>
	</TR>
</TABLE>
<P></P>
<asp:Button id="Button1" runat="server" Text="Reiniciar" onclick="Button1_Click"></asp:Button>
