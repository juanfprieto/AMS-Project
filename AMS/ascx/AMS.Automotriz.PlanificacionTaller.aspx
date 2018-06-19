<%@ Page language="c#" Codebehind="AMS.Automotriz.PlanificacionTaller.aspx.cs" AutoEventWireup="True" Inherits="AMS.Automotriz.AMS_Automotriz_PlanificacionTaller" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AMS - Planning de Taller</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../css/AMS.css" type="text/css" rel="stylesheet">
		<script language="javascript" type="text/javascript">
			var idInt;
			function Cargar()
			{
				clearInterval(idInt);
				idInt=setInterval('SetURL()',30000);
			}
			
			function SetURL()
			{
				//Tomo la url completa
				var url=document.location.href.toString();
				//Saco las variables get
				var gets=url.substring((url.indexOf("?"))+1,url.length);
				//Divido las variables get
				var arrGets=gets.split('&');
				//En la var get de la pagina saco el valor con el que viene y le sumo uno
				var nPag=arrGets[1].substring(arrGets[1].indexOf("=")+1,arrGets[1].length)
				var numPag=parseInt(nPag)+1;
				//Redirecciono a la nueva pagina
				document.location.href='AMS.Automotriz.PlanificacionTaller.aspx?'+arrGets[0]+'&pag='+numPag.toString();
			}
		</script>
	</HEAD>
	<BODY onload="Cargar()">
		<form id="Form1" method="post" runat="server">
			<P>&nbsp;</P>
			<p style="FONT-SIZE: 10pt; COLOR: lime; FONT-FAMILY: Arial; TEXT-DECORATION: none"></p>
			<P>
				<TABLE id="Table1" style="WIDTH: 898px; HEIGHT: 139px" cellSpacing="0" cellPadding="0"
					width="898" bgColor="white" border="0">
					<TR>
						<TD bgColor="#ffffff"><asp:imagebutton id="ImageButton2" runat="server" ImageUrl="../img/logo.jpg" Width="160px" Height="140px"></asp:imagebutton><asp:imagebutton id="ImageButton3" runat="server" ImageUrl="../img/planiing.png" Height="138px" Width="716px"></asp:imagebutton></TD>
					</TR>
				</TABLE>
			</P>
			<asp:label id="Label10" runat="server" Font-Bold="True" ForeColor="Black">OPERACIONES</asp:label>
			<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
			<asp:datagrid id="DataGrid1" runat="server" Width="900px" HorizontalAlign="Center" AllowPaging="True"
				PageSize="5">
				<FooterStyle HorizontalAlign="Center"></FooterStyle>
				<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
			<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
			<P>
				<TABLE id="Table3" style="WIDTH: 216px; HEIGHT: 44px" cellSpacing="0" cellPadding="0" width="216"
					bgColor="white" border="0">
					<TR>
						<TD>
							<asp:Label id="Label3" runat="server" Font-Bold="True" ForeColor="Black">Numero de Entrada</asp:Label></TD>
						<TD>
							<asp:TextBox id="TextBox3" runat="server" Height="20px" Width="60px" Font-Bold="True" ForeColor="Black"
								BackColor="Yellow" BorderColor="Black" BorderWidth="2px" ReadOnly="True">    XN </asp:TextBox></TD>
					</TR>
				</TABLE>
			</P>
			<P>&nbsp;</P>
			<P><asp:label id="Label11" runat="server" Font-Bold="True" ForeColor="Black">OT Sin Asignar</asp:label></P>
			<P><asp:datagrid id="Grid" runat="server" Width="600px" ForeColor="Gainsboro" HorizontalAlign="Center"
					AllowPaging="True" BackColor="LightGray" AutoGenerateColumns="False">
					<FooterStyle ForeColor="Black" BackColor="Transparent"></FooterStyle>
					<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
					<AlternatingItemStyle ForeColor="Red" BackColor="LightGray"></AlternatingItemStyle>
					<ItemStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="Red" BackColor="LightGray"></ItemStyle>
					<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="Blue"></HeaderStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Numero Orden">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "NUMORDEN") %>
							</ItemTemplate>
						</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Placa">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Fecha de Entrada">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "FECHAENTRADA") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Hora de Entrada">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "HORAENTRADA") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Fecha a Entregar">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "FECHAENTREGA") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Hora a Entregar">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "HORAENTREGA") %>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
				</asp:datagrid></P>
			<P>&nbsp;</P>
			<P><asp:imagebutton id="ImageButton1" runat="server" ImageUrl="file:///C:\Inetpub\wwwroot\AMS\img\powered.png"
					Width="104px"></asp:imagebutton></P>
		</form>
	</BODY>
</HTML>
