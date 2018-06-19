<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.DialogBox.aspx.cs" Inherits="AMS.Web.DialogBox" %>
<HTML>
	<HEAD>
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="mainApp">
		<form runat="server">
			<center>
				<table class="main" cellpadding="13">
					<tbody>
						<tr>
							<td align="center" colspan="2">
								<asp:PlaceHolder id="infoDialog" runat="server"></asp:PlaceHolder>
							</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Button id="btnAcp" runat="server" Text="Aceptar"></asp:Button>
							</td>
							<td align="center">
								<asp:Button id="btnCnl" runat="server" Text="Cancelar"></asp:Button>
							</td>
						</tr>
					</tbody>
				</table>
				<p>
					&nbsp;<asp:Label id="lb" runat="server"></asp:Label>
				</p>
			</center>
		</form>
	</body>
</HTML>
