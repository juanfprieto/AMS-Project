<%@ Page language="c#" Codebehind="AMS.Web.MobileIndex.aspx.cs" AutoEventWireup="True" Inherits="AMS.Web.MobileIndex" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AMS - Sistemas eCAS Ltda</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../img/AMS.ico" type="image/ico" rel="icon">
		<LINK href="../css/AMS.css" type="text/css" rel="stylesheet">
		<LINK href="../css/ajax.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
		<script language="javascript">
			<%if(Request.QueryString["eds"]!=null)
				Response.Write("alert('AMS se cerrara para actualizar los cambios realizados,\\n por favor inicie la aplicacin de nuevo. Gracias.');window.close();");%>
		</script>
	</HEAD>
	<body>
	<form id="Form" method="post" runat="server">
		<table class="main" width="100%" align="center" bgColor="white">
			<tbody>
				<tr>
					<td width="100%">
						<p><asp:label id="infoProcess" runat="server" font-names="Arial" font-size="12px" forecolor="RoyalBlue"
								font-bold="True" backcolor="#F2F2F2" cssclass="infoProcess" Width="100%"></asp:label></p>
					</td>
				</tr>
				<tr>
					<td>
						<p><asp:placeholder id="gridHolder" runat="server"></asp:placeholder></p>
					</td>
				</tr>
				<tr>
					<td>
						<asp:datagrid id="dgrMenu" runat="server" AutoGenerateColumns="False" ShowHeader="False" ShowFooter="False" OnItemDataBound="dgrMenu_ItemDataBound">
							<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#FFFFFF"></ItemStyle>
							<Columns>
								<asp:TemplateColumn ItemStyle-HorizontalAlign=Left>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "TEXTO") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						<p>
							<asp:label id="lbSystemName" runat="server" font-names="Tahoma" font-size="4pt" forecolor="#424242">AMS</asp:label>&nbsp;<asp:label id="lbCompanyName" runat="server" font-names="Tahoma" font-size="4pt" forecolor="#424242">Sistema eCAS</asp:label></FONT>
							<font color="#2a2a2a">&nbsp;&nbsp;
								<asp:label id="lblUsuario" runat="server" font-names="Tahoma" font-size="2pt" forecolor="#424242">Usuario</asp:label>
							</font>
						</p>
					</td>
				</tr>
			</tbody>
		</table>
		<asp:Label id="lb" runat="server"></asp:Label>
	</form>
	</body>
</HTML>
