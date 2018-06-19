<%@ Page language="c#" Codebehind="AMS.Garantias.VerKit.aspx.cs" AutoEventWireup="True" Inherits="AMS.Garantias.AMS_Garantias_VerKit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AMS</title>
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout" bgColor="#006699">
		<form id="Form1" method="post" runat="server">
			<asp:Panel id="Panel1" style="Z-INDEX: 102; LEFT: 24px; POSITION: absolute; TOP: 40px" runat="server"
				Height="72px" Width="272px">
				<asp:Label id="Label2" runat="server" Width="456px" ForeColor="DarkBlue" Font-Bold="True" BackColor="WhiteSmoke">Operaciones</asp:Label>
				<asp:DataGrid id="dgOperaciones" runat="server" Width="457px" AutoGenerateColumns="False">
					<Columns>
						<asp:BoundColumn DataField="ptem_operacion" HeaderText="C&#243;digo"></asp:BoundColumn>
						<asp:BoundColumn DataField="ptem_descripcion" HeaderText="Operaci&#243;n"></asp:BoundColumn>
					</Columns>
				</asp:DataGrid>
			</asp:Panel>
			<asp:Panel id="Panel2" style="Z-INDEX: 103; LEFT: 24px; POSITION: absolute; TOP: 176px" runat="server"
				Height="112px" Width="224px">
				<asp:Label id="Label3" runat="server" Width="456px" ForeColor="DarkBlue" Font-Bold="True" BackColor="WhiteSmoke">Items</asp:Label>
				<asp:DataGrid id="dgItems" runat="server" Width="456px" AutoGenerateColumns="False">
					<Columns>
						<asp:BoundColumn DataField="Codigo" HeaderText="C&#243;digo"></asp:BoundColumn>
						<asp:BoundColumn DataField="Item" HeaderText="Item"></asp:BoundColumn>
					</Columns>
				</asp:DataGrid>
			</asp:Panel>
			<P>
				<asp:Label id="Label1" style="Z-INDEX: 104; LEFT: 184px; POSITION: absolute; TOP: 16px" runat="server"
					Height="16px" Width="128px" BackColor="Transparent" Font-Bold="True" Font-Size="Small"
					ForeColor="White">Datos del Kit</asp:Label></P>
		</form>
	</body>
</HTML>
