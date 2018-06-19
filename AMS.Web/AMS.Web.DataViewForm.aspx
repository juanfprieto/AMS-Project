<%@ Page language="c#" Codebehind="AMS.Web.DataViewForm.aspx.cs" AutoEventWireup="True" Inherits="AMS.Web.DataViewForm" %>
<HTML>
	<HEAD>
		<title>AMS - Sistemas eCAS Ltda</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../css/AMS.css" type="text/css" rel="stylesheet">
		<link rel="icon" type="image/ico" href="../img/AMS.ico">
		<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
		<script language="javascript">
			<%if(Request.QueryString["eds"]!=null)
				Response.Write("alert('AMS se cerrara para actualizar los cambios realizados,\\n por favor inicie la aplicación de nuevo. Gracias.');window.close();");%>
		</script>
	</HEAD>
	<body>
		<table class="main" width="780" align="center" bgColor="white">
			<tbody>
				<tr>
					<td>
						<p><asp:label id="infoProcess" runat="server" font-names="Arial" font-size="12px" forecolor="RoyalBlue"
								font-bold="True" backcolor="#F2F2F2" cssclass="infoProcess"></asp:label></p>
					</td>
				</tr>
				<tr>
					<td>
						<div id="mainIframe">
							<form id="Form" method="post" runat="server">
								<p><asp:placeholder id="gridHolder" runat="server"></asp:placeholder></p>
							</form>
						</div>
					</td>
				</tr>
			</tbody>
		</table>
		<br>
		<asp:label id="lb" runat="server"></asp:label>
	</body>
</HTML>
