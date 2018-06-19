<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaGeneral.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaGeneral" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<LINK href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<br>
<p>
	<table class="filters" id="Table2" width="780">
		<tbody>
			<tr>
				<td vAlign="middle" borderColor="gray" width="16" colSpan="0"><IMG height="60" src="../img/AMS.Flyers.Filters.png" border="0">
				</td>
				<td vAlign="middle">
					<p><asp:label id="PlanillaLabel" runat="server">Planilla General</asp:label></p>
					<P>
						<TABLE id="Table4" style="WIDTH: 168px; HEIGHT: 75px" cellSpacing="1" cellPadding="1" width="168"
							border="0">
							<TR>
								<TD style="WIDTH: 51px"><asp:label id="AñoLabel" runat="server">Año</asp:label></TD>
								<td><asp:dropdownlist id="año" runat="server"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 51px"><asp:label id="MesLabel" runat="server">Mes</asp:label></TD>
								<td><asp:dropdownlist id="mes" runat="server"></asp:dropdownlist></TD>
							</TR>
							<tr>
								<td><asp:label id="DiaLabel" runat="server">Dia</asp:label></td>
								<td><asp:textbox id="Dia" runat="server" Width="30px"></asp:textbox></td>
							</tr>
						</TABLE>
					</P>
					<P><asp:button id="Generar" onclick="generar" runat="server" Text="Generar"></asp:button></P>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p></p>
<p><asp:placeholder id="toolsHolder" runat="server" visible="false">
		<TABLE class="tools" width="780">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<td>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<td>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<td>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 188px; POSITION: absolute; TOP: 271px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail.jpg"
						alt="Enviar por email" BorderWidth="0px"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:placeholder></p>
<br>
<table class="reports" id="tablaheader" width="630" align="center" bgColor="gray">
	<tbody>
		<tr>
			<td><asp:table id="tabPreHeader" Width="630px" BorderWidth="0px" CellSpacing="0" CellPadding="0"
					BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="center"></asp:table></td>
		</tr>
	</tbody>
</table>
<table class="reports" id="Table3" cellSpacing="0" cellPadding="0" width="630" align="center"
	bgColor="gray">
	<tbody>
		<tr>
			<td style="WIDTH: 330px" align="left"><label id="remesalabel" style="FONT-WEIGHT: bold; FONT-SIZE: 10pt; COLOR: blue">REMESAS</label>
				<p><asp:datagrid id="Grid" runat="server" Width="300px" HorizontalAlign="left" AutoGenerateColumns="False">
						<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="BUS">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "BUS") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle HorizontalAlign="left" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
					</asp:datagrid></p>
			</td>
			<td style="WIDTH: 330px" align="left"><label id="tiquetelabel" style="FONT-WEIGHT: bold; FONT-SIZE: 10pt; COLOR: blue">TIQUETES</label>
				<p><asp:datagrid id="Grid3" runat="server" Width="310px" HorizontalAlign="left" AutoGenerateColumns="False">
						<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="BUS">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "BUS4") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PASAJEROS">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TOTAL") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR4") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle HorizontalAlign="Left" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
					</asp:datagrid></p>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 330px" align="left"><label id="Anticipolabel" style="FONT-WEIGHT: bold; FONT-SIZE: 10pt; COLOR: blue">ANTICIPOS</label>
				<p><asp:datagrid id="Grid1" runat="server" Width="300px" HorizontalAlign="left" AutoGenerateColumns="False">
						<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="BUS">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "BUS1") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO1") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR1") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle HorizontalAlign="left" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
					</asp:datagrid></p>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 330px" align="left"><label id="Autorilabel" style="FONT-WEIGHT: bold; FONT-SIZE: 10pt; COLOR: blue">AUTORIZACIONES</label>
				<p><asp:datagrid id="Grid2" runat="server" Width="300px" HorizontalAlign="left" AutoGenerateColumns="False">
						<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="BUS">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "BUS2") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO2") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR2") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle HorizontalAlign="left" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
					</asp:datagrid></p>
			</td>
		</tr>
	</tbody>
</table>
<table class="reports" id="tablafooter" width="630" align="center" bgColor="gray">
	<tbody>
		<tr>
			<td><asp:table id="tabFirmas" Width="630px" BorderWidth="0px" CellSpacing="0" CellPadding="0" BackColor="White"
					GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="center"
					EnableViewState="False"></asp:table></td>
		</tr>
	</tbody>
</table>
<BLOCKQUOTE dir="ltr" style="MARGIN-RIGHT: 0px">
	<P><asp:label id="lb" runat="server"></asp:label></P>
</BLOCKQUOTE></TABLE>
