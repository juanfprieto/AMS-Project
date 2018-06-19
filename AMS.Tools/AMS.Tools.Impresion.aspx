<%@ Page language="c#" Codebehind="AMS.Tools.Impresion.aspx.cs" AutoEventWireup="True" Inherits="AMS.Tools.Impresion" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AMS</title>
		<meta name="vs_snapToGrid" content="False">
		<meta name="vs_showGrid" content="True">
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout" onload="window.print();">
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
		<form id="Form1" method="post" runat="server">
			<center><asp:Panel id="pnl1" runat="server" Width="224px" Visible="False">
					<TABLE id="Table1" style="WIDTH: 600px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 77px; BACKGROUND-COLOR: white; BORDER-BOTTOM-STYLE: none"
						cellSpacing="1" cellPadding="1" border="1">
						<TR>
							<TD>Tabla :
							</TD>
							<TD>
								<asp:Label id="lbtabla" runat="server"></asp:Label></TD>
						</TR>
						<TR>
							<TD>Campo :
							</TD>
							<TD>
								<asp:Label id="lbcampo" runat="server"></asp:Label></TD>
						</TR>
						<TR>
							<TD>Comentario :
							</TD>
							<TD>
								<asp:Label id="lbcomentario" runat="server"></asp:Label></TD>
						</TR>
						<TR>
							<TD>Ayuda Contextual :
							</TD>
							<TD>
								<asp:Label id="lbayuda" runat="server"></asp:Label></TD>
						</TR>
					</TABLE>
				</asp:Panel>
			</center>
			<p></p>
			<center>
				<asp:Panel id="pnl2" runat="server" Width="116px" Visible="False">
					<asp:DataGrid id="dg" runat="server" cssclass="datagrid" PageSize="25">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
					</asp:DataGrid>
				</asp:Panel>
			</center>
		</form>
	</body>
</HTML>
