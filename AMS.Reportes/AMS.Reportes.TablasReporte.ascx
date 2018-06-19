<%@ Control Language="c#" codebehind="AMS.Reportes.TablasReporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Tablas" %>
<p>
	A continuación se presentan las tablas&nbsp;disponibles para generar reportes. 
	Por Favor seleccione una tabla para realizar el reporte :
</p>
<p>
	Tabla a Seleccionar :
	<asp:DropDownList id="ddlTablas" runat="server"></asp:DropDownList>
</p>
<p>
	<asp:Button id="btnAgr" onclick="Agregar_Tabla" runat="server" Text="Agregar"></asp:Button>
</p>
<p>
	<asp:DataGrid id="dgTablas" runat="server" OnDeleteCommand="DgTable_Delete" BorderStyle="None"
		Width="700px" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3"
		BorderColor="#999999" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="false"
		BackColor="White">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE DE LA TABLA"></asp:BoundColumn>
			<asp:BoundColumn DataField="COMENTARIO" HeaderText="COMENTARIO DE LA TABLA"></asp:BoundColumn>
			<asp:BoundColumn DataField="RELACIONADAS" HeaderText="TABLAS RELACIONADAS"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="ACCI&#211;N">
				<ItemTemplate>
					<asp:Button CommandName="Delete" Text="Remover" ID="btnDel" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Button id="btnAcpt" onclick="Aceptar_Tablas" runat="server" Text="Aceptar" Visible="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
