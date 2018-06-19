<%@ Control Language="c#" codebehind="AMS.Reportes.FinalReporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Final" %>

<p>
	Aqui Terminamos la configuración del reporte, preparando el texto&nbsp;que se 
	mostrara en el footer del reporte :
</p>
<p>
	Nombre del reporte :
	<asp:TextBox id="nombReporte" Width="434px" runat="server"></asp:TextBox>
</p>
<p>
</p>
<fieldset style="WIDTH: 598px; HEIGHT: 60px">
	<legend>Elementos del Footer</legend>
	<table class="main">
		<tbody>
			<tr>
				<td>
					Texto a Agregar :&nbsp;
				</td>
				<td>
					<asp:TextBox id="tbTxtFooter" runat="server"></asp:TextBox>
				</td>
				<td>
					<asp:Button id="btnAgrVlTx" onclick="Agregar_ValorTx" runat="server" Text="Agregar Valor Textual"></asp:Button>
				</td>
			</tr>
			<tr>
				<td>
					Tabla :
					<asp:DropDownList id="ddlTablas" runat="server" OnSelectedIndexChanged="Cambio_Tabla" AutoPostBack="True"></asp:DropDownList>
				</td>
				<td>
					Campo :
					<asp:DropDownList id="ddlCampos" runat="server"></asp:DropDownList>
				</td>
				<td>
					<asp:Button id="btnAgrVl" onclick="Agregar_Valor" runat="server" Text="Agregar Valor "></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>
<p>
</p>
<p>
	<asp:DataGrid id="dgFooter" Width="465px" runat="server" Font-Size="8pt" Font-Name="Verdana" HeaderStyle-BackColor="#ccccdd"
		CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical"
		BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="False">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="VALOR">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Button id="btnAcpt" onclick="Guardar_Reporte" runat="server" Text="Guardar Reporte"></asp:Button>
</p>
<p>
	Estos son los elementos configurados para el reporte :
</p>
<p>
	Tabla Principal del Reporte :
	<asp:Label id="lbTabla" runat="server" font-bold="True"></asp:Label>
</p>
<p>
	Consulta :
	<asp:Label id="lbConsulta" runat="server" font-bold="True"></asp:Label>
</p>
<p>
	Filtros :
</p>
<p>
	<asp:DataGrid id="dgFltrRpt" Width="650px" runat="server" Font-Size="8pt" Font-Name="Verdana"
		HeaderStyle-BackColor="#ccccdd" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None"
		GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="false">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="TABLA" HeaderText="TABLA"></asp:BoundColumn>
			<asp:BoundColumn DataField="CAMPO" HeaderText="CAMPO"></asp:BoundColumn>
			<asp:BoundColumn DataField="ETIQUETA" HeaderText="ETIQUETA"></asp:BoundColumn>
			<asp:BoundColumn DataField="TIPOCOMPARACION" HeaderText="TIPO DE COMPARACIÓN"></asp:BoundColumn>
			<asp:BoundColumn DataField="TIPODATO" HeaderText="TIPO DE DATO"></asp:BoundColumn>
			<asp:BoundColumn DataField="CONTROLASOCIADO" HeaderText="CONTROL ASOCIADO"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALORINTERPRETADO" HeaderText="VALOR INTERPRETADO"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	Filas Especiales :
</p>
<p>
	<asp:DataGrid id="dgFilas" Width="551px" runat="server" Font-Size="8pt" Font-Name="Verdana" HeaderStyle-BackColor="#ccccdd"
		CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical"
		BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="False">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="POSICI&#211;N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "POSICION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ORDEN PRESENTACI&#211;N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ORDEN") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ALINEACI&#211;N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ALINEACION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="VALOR">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="OPCI&#211;N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OPCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="TABLAS ASOCIADAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TABLAS") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
