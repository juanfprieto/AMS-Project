<%@ Control Language="c#" codebehind="AMS.Reportes.EliminarReporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Eliminar" %>
<p align="justify">
	A continuacion se muestran los reportes disponibles creados por el usuario 
	mediante
	esta herramienta. Para eliminar simplemente debe dar click en el boton 
	eliminar:
</p>
<p align="justify">
	<asp:DataGrid id="dgReports" runat="server" OnDeleteCommand="DgTable_Delete" BorderStyle="None" Width="700px" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3" BorderColor="#999999" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="false" BackColor="White">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="ID DEL REPORTE">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "IDREPORTE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NOMBRE DEL REPORTE">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMBREREPORTE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ID OPCIÓN DEL MENU">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "IDMENU") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ACCI&#211;N">
				<ItemTemplate>
					<asp:Button CommandName="Delete" Text="Remover" ID="btnDel" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p align="justify">
	<asp:Button id="btnVlr" onclick="Volver" runat="server" Text="Volver"></asp:Button>
</p>
<p align="justify">
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
