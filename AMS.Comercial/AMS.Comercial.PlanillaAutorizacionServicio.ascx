<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaAutorizacionServicio.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaAutorizacionServicio" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<LINK href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<br>
<p>
	<table class="filters" width="780" id="Table2">
		<tbody>
			<tr>
				<td vAlign="middle" borderColor="gray" width="16" colSpan="0"><IMG height="60" src="../img/AMS.Flyers.Filters.png" border="0">
				</td>
				<td vAlign="middle">
					<p>&nbsp;</p>
					<p>
						<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="0">
							<TR>
								<TD style="WIDTH: 98px">
									<asp:Label id="PlacaLabel" runat="server">Placa</asp:Label></TD>
								<td>
									<asp:DropDownList id="Placa" runat="server" Width="136px" OnSelectedIndexChanged="generar"></asp:DropDownList></TD>
							</TR>
							<TR>
							</TR>
						</TABLE>
					</p>
					<P>
						<TABLE id="Table4" style="WIDTH: 168px; HEIGHT: 75px" cellSpacing="1" cellPadding="1" width="168"
							border="0">
							<TR>
								<TD style="WIDTH: 51px"><asp:label id="Label1" runat="server">A�o</asp:label></TD>
								<td><asp:dropdownlist id="a�o" runat="server"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 51px"><asp:label id="Label2" runat="server">Mes</asp:label></TD>
								<td><asp:dropdownlist id="mes" runat="server"></asp:dropdownlist></TD>
							</TR>
						</TABLE>
					</P>
					<p><asp:button id="Button1" onclick="generar" runat="server" Text="Generar"></asp:button></p>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p></p>
<p><asp:placeholder id="toolsHolder" runat="server" visible="false">&lt; 
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
<p></p>
<br>
<table class="reports" width="780" align="center" bgColor="gray" id="Table3">
	<tbody>
		<tr>
			<td><asp:Table id="tabPreHeader" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
					Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"
					Width="100%"></asp:Table></td>
		</tr>
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
							<asp:TemplateColumn HeaderText="FECHA">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO AUTORIZACION " ItemStyle-HorizontalAlign="center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMAUTO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CODIGO AUTORIZACION " ItemStyle-HorizontalAlign="center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="DESCRIPCION SERVICIO" ItemStyle-HorizontalAlign="left">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR SERVICIO" ItemStyle-HorizontalAlign="Right">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORSERV") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CANTIDAD SERVICIO" ItemStyle-HorizontalAlign="center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR TOTAL SERVICIO" ItemStyle-HorizontalAlign="Right">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORTOTAL") %>
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
	</tbody>
</table>
<BLOCKQUOTE dir="ltr" style="MARGIN-RIGHT: 0px">
	<P><asp:label id="lb" runat="server"></asp:label></P>
</BLOCKQUOTE>
