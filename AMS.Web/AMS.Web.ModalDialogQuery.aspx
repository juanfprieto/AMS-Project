<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ModalDialogQuery.aspx.cs" Inherits="AMS.Web.ModalDialogQuery" %>
<%@ outputcache duration="10" varybyparam="params" %>
<HTML>
	<HEAD>
		<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form runat="server">
			<table class="query" style="WIDTH: 344px; HEIGHT: 108px">
				<tbody>
					<tr>
						<td>
							<p style="TEXT-ALIGN: center">
								<asp:Label id="titulo" runat="server"></asp:Label>
							</p>
						</td>
					</tr>
					<tr>
						<td>
							<p style="TEXT-ALIGN: center">
								<asp:DataGrid id="dgTable" runat="server" PageSize="100" BorderStyle="Ridge" BorderWidth="2px"
									BorderColor="White" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1" EnableViewState="True"
									AutoGenerateColumns="True">
									<HeaderStyle font-bold="True" forecolor="#E7E7FF" backcolor="#4A3C8C"></HeaderStyle>
									<PagerStyle horizontalalign="Center" forecolor="Black" position="TopAndBottom" backcolor="#C6C3C6"></PagerStyle>
									<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#9471DE"></SelectedItemStyle>
									<ItemStyle forecolor="Black" backcolor="#DEDFDE"></ItemStyle>
								</asp:DataGrid>
							</p>
						</td>
					</tr>
					<tr>
						<td>
							<p style="TEXT-ALIGN: center">
								<input id="cerrar" onclick="javascript:window.close();" type="button" value="Cerrar">
							</p>
						</td>
					</tr>
				</tbody>
			</table>
		</form>
		<asp:Label id="lb" runat="server"></asp:Label>
	</body>
</HTML>
