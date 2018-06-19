<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaAnticiposExtras.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaAnticiposExtras" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<LINK href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<br>
<p>
	<table class="filters" width="486" id="Table2" style="WIDTH: 486px; HEIGHT: 197px">
		<tbody>
			<tr>
				<td>
					<FIELDSET style="WIDTH: 480px; HEIGHT: 191px"><LEGEND>Filtros</LEGEND>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="472" border="0" style="WIDTH: 472px; HEIGHT: 134px">
							<TR>
								<td>
									<asp:Label id="Label1" runat="server" Font-Bold="True" ForeColor="Black">Placa:</asp:Label></TD>
								<td>
									<asp:DropDownList id="placa" runat="server" Width="104px"></asp:DropDownList></TD>
								<td></TD>
							</TR>
							<TR>
								<td>
									<asp:Label id="Label2" runat="server" Font-Bold="True" ForeColor="Black">Fecha</asp:Label></TD>
								<td>
									<asp:RadioButtonList id="RadioButtonList1" runat="server" Font-Bold="True" ForeColor="Black" AutoPostBack="True">
										<asp:ListItem Value="1">A&#241;o</asp:ListItem>
										<asp:ListItem Value="2">Mes</asp:ListItem>
										<asp:ListItem Value="3">Dia</asp:ListItem>
									</asp:RadioButtonList></TD>
								<td>
									<P>
										<asp:Label id="Label3" runat="server" Visible="False" Font-Bold="True" ForeColor="Black">Año</asp:Label>&nbsp;
										<asp:TextBox id="AñoBox" runat="server" Width="50px" Visible="False"></asp:TextBox></P>
									<P>
										<asp:Label id="Label4" runat="server" Visible="False" Font-Bold="True" ForeColor="Black">Mes</asp:Label>
										<asp:TextBox id="MesBox" runat="server" Width="50px" Visible="False"></asp:TextBox></P>
									<P>
										<asp:Label id="Label5" runat="server" Visible="False" Font-Bold="True" ForeColor="Black">Dia</asp:Label>&nbsp;
										<asp:TextBox id="DiaBox" runat="server" Width="39px" Visible="False"></asp:TextBox></P>
								</TD>
							</TR>
							<TR>
								<td>
									<asp:Button id="generar" runat="server" Text="Generar" OnClick="generar_OnClick"></asp:Button></TD>
								<td></TD>
								<td></TD>
							</TR>
						</TABLE>
					</FIELDSET>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p></p>
<p><asp:placeholder id="toolsHolder" runat="server" visible="false">
		<TABLE class="tools" width="780">
		</TABLE>
	</asp:placeholder></p>
</TABLE>
<br>
<table class="reports" width="780" align="center" bgColor="gray" id="Table3">
	<tbody>
		<tr>
			<td><table class="reports" id="tablaheader" width="780" align="center" bgColor="gray">
					<tbody>
						<tr>
							<td><asp:table id="tabPreHeader" Width="780px" BorderWidth="0px" CellSpacing="0" CellPadding="0"
									BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="center"></asp:table></td>
						</tr>
					</tbody>
			</td>
		</tr>
	</tbody>
</table>
<tr>
	<td align="center">
		<p><asp:datagrid id="Grid" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False" Width="780px">
				<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
				<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
				<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
				<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="DOCUMENTO">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "DOCUMENTO") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="FECHA" ItemStyle-HorizontalAlign="left">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="TIPO ANTICIPO" ItemStyle-HorizontalAlign="left">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "TANTICIPO") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="PLACA" ItemStyle-HorizontalAlign="left">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="A NOMBRE DE:" ItemStyle-HorizontalAlign="left">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="DESCRIPCION" ItemStyle-HorizontalAlign="left">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "DESC") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="VALOR" ItemStyle-HorizontalAlign="left">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid></p>
	</td>
</tr>
<tr>
	<td><asp:table id="tabFirmas" BorderWidth="0px" EnableViewState="False" CellSpacing="0" CellPadding="1"
			BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"></asp:table></td>
</tr>
</TBODY></TABLE> <BLOCKQUOTE dir="ltr" style="MARGIN-RIGHT: 0px">
	<P><asp:label id="lb" runat="server"></asp:label></P>
</BLOCKQUOTE>
