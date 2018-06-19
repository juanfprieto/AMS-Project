<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ModalDialogCache.aspx.cs" Inherits="AMS.Web.ModalDialogCache" %>
<%@ outputcache duration="10" varybyparam="params" %>
<HTML>
	<HEAD>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
		<script language="javascript">
        <%if(Request.QueryString["Vals"]!=null)Response.Write("GetValueA('"+Request.QueryString["Vals"]+"');");%>
		</script>
		<LINK href="../css/AMS.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="mainApp">
		<form runat="server">
			<table class="help" style="WIDTH: 344px; HEIGHT: 108px">
				<tbody>
					<tr>
						<td>
							<table class="head" bgColor="#e0e0e0">
								<tbody>
									<tr>
										<td>Buscar:
											<asp:textbox id="tbWord" runat="server" Width="100"></asp:textbox>en:
											<asp:dropdownlist id="ddlCols" runat="server"></asp:dropdownlist><br>
											<br>
											<asp:button id="btSearch" runat="server" Text="Buscar"></asp:button><asp:button id="btInserta" runat="server" Text="Insertar"></asp:button><asp:label id="lbTitle" runat="server"></asp:label></td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<p style="TEXT-ALIGN: center"><asp:datagrid id="dgTable" runat="server" PageSize="100"
									BorderStyle="Ridge" BorderWidth="2px" BorderColor="White" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1"
									AllowPaging="True" EnableViewState="True" AutoGenerateColumns="True">
									<FooterStyle forecolor="Black" backcolor="#C6C3C6"></FooterStyle>
									<HeaderStyle font-bold="True" forecolor="#E7E7FF" backcolor="#4A3C8C"></HeaderStyle>
									<PagerStyle horizontalalign="Center" forecolor="Black" position="TopAndBottom" backcolor="#C6C3C6"
										mode="NumericPages"></PagerStyle>
									<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#9471DE"></SelectedItemStyle>
									<ItemStyle forecolor="Black" backcolor="#DEDFDE"></ItemStyle>
								</asp:datagrid></p>
						</td>
					</tr>
					<tr>
						<td>
							<p style="TEXT-ALIGN: center"><asp:label id="lb" runat="server"></asp:label></p>
						</td>
					</tr>
				</tbody>
			</table>
			<br>
			<p style="TEXT-ALIGN: center"><asp:textbox id="insTabla" runat="server" Visible="False"></asp:textbox><asp:textbox id="insSQL" runat="server" Visible="False"></asp:textbox></p>
			<p style="TEXT-ALIGN: center"></p>
			<!-- Insert content here --></form>
	</body>
</HTML>
