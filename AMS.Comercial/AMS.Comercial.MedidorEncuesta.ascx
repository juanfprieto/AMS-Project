<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.MedidorEncuesta.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_MedidorEncuesta" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="web" Namespace="WebChart" Assembly="WebChart" %>
<asp:label id="RemeasLabel" Font-Bold="True" Font-Size="Medium" runat="server">Resumen Encuestas</asp:label>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
&nbsp;
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
	<TR>
		<td><asp:label id="Label1" runat="server">Servicio</asp:label></TD>
		<td><asp:dropdownlist id="servicio" runat="server" Width="150px"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<td><asp:button id="Generar" runat="server" Text="Generar" OnClick="generar"></asp:button></TD>
		<td></TD>
	</TR>
</TABLE>
<asp:panel id="Panel1" runat="server" Height="208px" Visible="False">
	<TABLE>
		<TR>
			<td>
				<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
					<TR>
						<td>
							<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
							&nbsp;</TD>
						<td></TD>
						<td></TD>
					</TR>
					<TR>
						<td>
							<asp:Label id="Label5" runat="server" Font-Bold="True" ForeColor="Black">Servicio:</asp:Label></TD>
						<td>
							<asp:Label id="serv" runat="server" Font-Bold="True" ForeColor="Blue">Label</asp:Label></TD>
						<td></TD>
					</TR>
					<TR>
						<td>
							<asp:Label id="Label4" runat="server" Font-Bold="True" ForeColor="Black">Calificacion Maxima</asp:Label></TD>
						<td>
							<asp:Label id="maxcal" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
						<td></TD>
					</TR>
					<TR>
						<td>
							<asp:Label id="Label3" runat="server" Font-Bold="True" ForeColor="Black">Calificacion Minima:</asp:Label></TD>
						<td>
							<asp:Label id="mincal" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
						<td></TD>
					</TR>
					<TR>
						<td>
							<asp:Label id="Label2" runat="server" Font-Bold="True" ForeColor="Black">Promedio Calificacion:</asp:Label></TD>
						<td>
							<asp:Label id="promcal" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
					</TR>
					<TR>
						<td>
							<asp:Label id="Label8" runat="server" Font-Size="Small" Font-Bold="True">Porcentaje de Satisfaccion:</asp:Label></TD>
						<td>
							<asp:Label id="promedio" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label>
							<asp:Label id="Label10" runat="server" Font-Bold="True" ForeColor="Black">%</asp:Label></TD>
					</TR>
					<TR>
						<td>
							<asp:Label id="Label6" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Blue">Total Encuestas:</asp:Label></TD>
						<td>
							<asp:Label id="toten" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
					</TR>
					<TR>
						<td>
							<asp:Label id="Label7" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Blue">Total Encuestas Servicio:</asp:Label></TD>
						<td>
							<asp:Label id="totser" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
					</TR>
					<TR>
						<td>
							<asp:Button id="detalles" onclick="detalles_Click" runat="server" Text="Ver Detalles"></asp:Button></TD>
						<td></TD>
					</TR>
				</TABLE>
			</TD>
			<td>
				<Web:ChartControl id="chart1" runat="server" Width="268px" Visible="False" Height="194px" ShowYValues="False"
					GridLines="Both" ShowXValues="False" HasChartLegend="False" BorderColor="Black" BorderStyle="Outset"
					ChartFormat="Jpg" YCustomEnd="0" YValuesInterval="0" Padding="12" YCustomStart="0" ImageID="1713e042-d2ce-4fc1-8bc9-03f5c455740f"
					ChartPadding="30" ShowTitlesOnBackground="False">
					<XTitle ForeColor="White" StringFormat="Center,Far,Character,LineLimit"></XTitle>
					<YAxisFont ForeColor="White" StringFormat="Far,Near,Character,LineLimit" Font="Tahoma, 8pt, style=Bold"></YAxisFont>
					<ChartTitle ForeColor="White" StringFormat="Near,Near,Character,LineLimit" Text="Estaditicas"
						Font="Tahoma, 12pt, style=Bold"></ChartTitle>
					<XAxisFont ForeColor="White" StringFormat="Center,Near,Character,LineLimit" Font="Tahoma, 8pt, style=Bold"></XAxisFont>
					<Legend Font="Tahoma, 6pt">
					</Legend>
					<Background Angle="90" EndPoint="100, 400" Color="SlateGray" HatchStyle="DiagonalBrick"></Background>
					<YTitle ForeColor="White" StringFormat="Near,Near,Character,DirectionVertical"></YTitle>
					<Border EndCap="Round" Color="LightGray" Width="2"></Border>
					<PlotBackground Color="Gray"></PlotBackground>
				</Web:ChartControl></TD>
		</TR>
	</TABLE>
</asp:panel><asp:panel id="Panel2" runat="server" Height="168px" Visible="False">
	<DIV style="OVERFLOW: auto; HEIGHT: 200px">
		<asp:datagrid id="Grid" runat="server" Width="556px" AutoGenerateColumns="False" HorizontalAlign="Center">
			<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
			<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
			<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
			<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
			<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
			<ItemStyle HorizontalAlign="Center"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="OBSERVACIONES SERVICIO">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "OBSERVICIO") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="OBSERVACIONES GENERALES" ItemStyle-HorizontalAlign="center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "OBGENERALES") %>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid></DIV>
</asp:panel>
