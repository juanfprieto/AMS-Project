<%@ Page Language="C#" Debug="true" autoeventwireup="True" codebehind="AMS.Finanzas.Cartera.ReporteCruces.aspx.cs" Inherits="AMS.Finanzas.Cartera.ReporteCruces" %>
<HTML>
	<HEAD>
	</HEAD>
	<body onload="window.print();">
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
			<form runat="server">
				<p>
					<asp:Label id="lbInfo" runat="server"></asp:Label>
				</p>
				<center>
					<p>
						<asp:DataGrid id="dgCruces" runat="server" Width="650px" PageSize="10" Font-Names="Verdana" CellPadding="3"
							Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" AutoGenerateColumns="False">
							<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
							<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
							<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
							<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
							<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
							<Columns>
								<asp:BoundColumn DataField="NIT" HeaderText="Nit"></asp:BoundColumn>
								<asp:BoundColumn DataField="PREFIJO" HeaderText="Prefijo Factura"></asp:BoundColumn>
								<asp:BoundColumn DataField="NUMERO" HeaderText="N&#250;mero Factura"></asp:BoundColumn>
								<asp:BoundColumn DataField="VALOR" HeaderText="Valor Factura" DataFormatString="{0:C}"></asp:BoundColumn>
								<asp:BoundColumn DataField="CRUCE" HeaderText="Cruzada con"></asp:BoundColumn>
								<asp:BoundColumn DataField="PREFIJON" HeaderText="Prefijo Nota"></asp:BoundColumn>
								<asp:BoundColumn DataField="NUMERON" HeaderText="N&#250;mero Nota"></asp:BoundColumn>
								<asp:BoundColumn DataField="VALORN" HeaderText="Valor Cruce" DataFormatString="{0:C}"></asp:BoundColumn>
							</Columns>
						</asp:DataGrid>
					</p>
				</center>
			</form>
	</body>
</HTML>
