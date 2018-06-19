<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.PrintBox.aspx.cs" Inherits="AMS.Web.PrintBox" %>
<HTML>
	<HEAD>
		<title>Aplicación de Impresión</title>
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form runat="server">
			<p>
				<asp:PlaceHolder id="plcObjCOM" runat="server"></asp:PlaceHolder>
			</p>
			<p>
				&nbsp;<asp:Label id="lb" runat="server"></asp:Label>
			</p>
			<p>
				<asp:TextBox id="TextBox1" runat="server" Width="231px"></asp:TextBox>
			</p>
		</form>
	</body>
</HTML>
