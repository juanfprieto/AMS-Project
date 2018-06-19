<%@ outputcache duration="10" varybyparam="params" %>
<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ModalDialogPagos.aspx.cs" Inherits="AMS.Web.ModalDialogPagos" %>

<!DOCTYPE html>
<HTML lang="es">
	<HEAD>
		<meta charset="utf-8" />
		<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
		<script language="javascript">
        <%if(Request.QueryString["Vals"]!=null)Response.Write("GetValueA('"+Request.QueryString["Vals"]+"');");%>
		</script>
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
	</HEAD>
	 
	<body class="mainApp">
        <form runat="server">
		    <table class="help">
	            <tbody>
		 			<tr>
						<td>
							<table class="head" bgcolor="#e0e0e0">
								<tbody>
									<tr>
										<td>
											Buscar:
											<asp:TextBox id="tbWord" class="tpequeno" runat="server"></asp:TextBox>
											en:
											<asp:DropDownList id="ddlCols" class="dpequeno" runat="server"></asp:DropDownList>
											<br>
											<br>
											<asp:Button id="btSearch" onclick="Search" runat="server" Text="Buscar"></asp:Button>
											<asp:Label id="lbTitle" runat="server"></asp:Label></td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<p style="TEXT-ALIGN: center">
								<asp:DataGrid id="dgTable" runat="server" PageSize="100" OnItemDataBound="DgHelp_ItemDataBound"
									BorderStyle="Ridge" BorderWidth="2px" BorderColor="White" BackColor="White" CellPadding="3"
									GridLines="None" CellSpacing="1" OnPageIndexChanged="dgHelp_Page" AllowPaging="True" EnableViewState="True"
									AutoGenerateColumns="True">
									<FooterStyle forecolor="Black" backcolor="#C6C3C6"></FooterStyle>
									<HeaderStyle font-bold="True" forecolor="#E7E7FF" backcolor="#4A3C8C"></HeaderStyle>
									<PagerStyle horizontalalign="Center" forecolor="Black" position="TopAndBottom" backcolor="#C6C3C6"
										mode="NumericPages"></PagerStyle>
									<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#9471DE"></SelectedItemStyle>
									<ItemStyle forecolor="Black" backcolor="#DEDFDE"></ItemStyle>
								</asp:DataGrid>
							</p>
						</td>
					</tr>
					<tr>
						<td>
							<p style="TEXT-ALIGN: center">
								<asp:Label id="lb" runat="server"></asp:Label>
							</p>
						</td>
					</tr>
				</tbody>
			</table>
      	</form>
	</body>
</HTML>
