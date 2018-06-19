<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page language="c#" Codebehind="AMS.Web.Menu.aspx.cs" AutoEventWireup="True" Inherits="AMS.Web.Menu" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head runat="server">
		<title>Tree View Example</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie8" name="vs_targetSchema">
		<link href="../css/AMS.Menu.css" type="text/css" rel="stylesheet" />
        
        <link rel="stylesheet" href="../css/tabber.css" type="text/css" media="screen" />
        <script type="text/javascript" src="../js/tabber.js"></script>
        <script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
        <script>
        </script>
	</head>
	<body ms_positioning="GridLayout" style="padding: 0px; margin: 0px">
		<form id="frmTVExample" method="post" runat="server">
			<table width="100%">
				<tr>
					<td valign="top">
                        <div class="tabber" id="mytab1">
                            <div class="tabbertab" title="Contenido">
                                <table width="250" border="0">
									<tr>
										<td>
											<asp:Label ID="Label3" Runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
									    <td valign="top"> <div align="left">
											<asp:TreeView ID="trvMenu" EnableClientScript="false" runat="server" ExpandImageUrl="../img/TreeView/Book_Closed.png" CollapseImageUrl="../img/TreeView/Book_Opened.png" 
                                                LeafNodeStyle-CssClass="MenuLeaf" LeafNodeStyle-ImageUrl="../img/TreeView/Item.ico" LeafNodeStyle-NodeSpacing=0 NoExpandImageUrl="../img/TreeView/None.gif" 
                                                SelectedNodeStyle-CssClass="SelectedNode" PopulateNodesFromClient="true">
                                            </asp:TreeView></div>
										</td>
									</tr>
								</table>
                            </div>
                            <div class="tabbertab" title="Buscar...">
                                <table width="250" border="0">
									<tr>
										<td class="Text">>Buscar Para:</td>
									</tr>
									<tr>
										<td valign="top">
											<span class="TextBox">
												<asp:TextBox id="look" runat="server" Width="245px"></asp:TextBox>
											</span>
										</td>
									</tr>
									<tr>
										<td>
											<span class="Button">
												<asp:Button id="Buscar" runat="server" Text="Buscar" OnClick="Buscar_Click"></asp:Button>
											</span>
										</td>
									</tr>
									<tr>
										<td valign="top" width="250" height="200">
											<asp:Label id="Resultado" runat="server"></asp:Label></td>
									</tr>
								</table>
                            </div>
                        </div>
                    </td>
				</tr>
			</table>
			<input id="Sistema" style="Z-INDEX: 1; LEFT: 280px; POSITION: absolute; TOP: 16px" type="hidden" runat="server" />
		</form>
	</body>
</html>
