<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaComparendos.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaComparendos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="RemeasLabel" runat="server" Font-Size="Medium" Font-Bold="True">Relacion de Comparendos</asp:label>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
&nbsp;
<TABLE id="Table1" style="WIDTH: 560px; HEIGHT: 82px" cellSpacing="0" cellPadding="0" width="560"
	border="0">
	<TR>
		<TD style="HEIGHT: 11px"><asp:label id="Label1" runat="server" Font-Bold="True" ForeColor="Red">Fecha Inicio:</asp:label></TD>
		<TD style="HEIGHT: 11px"><asp:label id="Label2" runat="server" Font-Bold="True">Año</asp:label><asp:dropdownlist id="añoI" runat="server"></asp:dropdownlist><asp:label id="Label3" runat="server" Font-Bold="True">Mes</asp:label><asp:dropdownlist id="mesI" runat="server"></asp:dropdownlist><asp:label id="Label4" runat="server" Font-Bold="True">Dia</asp:label><asp:textbox id="diaI" runat="server" Width="30px"></asp:textbox></TD>
		<TD style="HEIGHT: 11px"></TD>
	</TR>
	<TR>
		<td><asp:label id="Label5" runat="server" Font-Bold="True" ForeColor="Red">Fecha Fin:</asp:label></TD>
		<td><asp:label id="Label6" runat="server" Font-Bold="True">Año</asp:label><asp:dropdownlist id="añoF" runat="server"></asp:dropdownlist><asp:label id="Label7" runat="server" Font-Bold="True">Mes</asp:label><asp:dropdownlist id="mesF" runat="server"></asp:dropdownlist><asp:label id="Label8" runat="server" Font-Bold="True">Dia</asp:label><asp:textbox id="diaF" runat="server" Width="30px"></asp:textbox></TD>
		<td></TD>
	</TR>
	<TR>
		<td><asp:label id="Label9" runat="server" Font-Bold="True" ForeColor="Red">Conductor:</asp:label></TD>
		<td><asp:dropdownlist id="conductor" runat="server" Width="250px"></asp:dropdownlist></TD>
		<td></TD>
	</TR>
	<tr>
		<td><asp:label id="Label10" runat="server" Font-Bold="True" ForeColor="Red">Estado:</asp:label></td>
		<td><asp:checkbox id="pago" runat="server" Font-Bold="True" Text="Pago"></asp:checkbox><asp:checkbox id="nopago" runat="server" Font-Bold="True" Text="No Pago"></asp:checkbox><asp:checkbox id="proceso" runat="server" Font-Bold="True" Text="En Proceso"></asp:checkbox><asp:checkbox id="todos" runat="server" Font-Bold="True" Text="Todos"></asp:checkbox></td>
		<td></td>
	</tr>
	<tr>
		<td><asp:button id="Generar" runat="server" Text="Generar" OnClick="generar"></asp:button></td>
	</tr>
</TABLE>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
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
		</TABLE></asp:placeholder></p>
<p></p>
<br>
<table class="reports" id="Table3" width="780" align="center" bgColor="gray">
	<tbody>
		<TR>
			<td><asp:table id="tabPreHeader" Font-Size="8pt" Width="100%" BorderWidth="0px" HorizontalAlign="Center"
					Font-Name="Verdana" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"></asp:table></td>
		</TR>
		<tr>
			<td align="center">
				<p><asp:datagrid id="Grid" runat="server" Width="780px" HorizontalAlign="Center" AutoGenerateColumns="False">
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
							<asp:TemplateColumn HeaderText="NUMERO COMPARENDO" ItemStyle-HorizontalAlign="center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMCOMPARENDO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="INFRACCION" ItemStyle-HorizontalAlign="center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "INFRACCION") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR" ItemStyle-HorizontalAlign="center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CONDUCTOR" ItemStyle-HorizontalAlign="Right">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CONDUCTOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="ESTADO" ItemStyle-HorizontalAlign="Right">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></p>
			</td>
		</tr>
		<tr>
			<td><asp:table id="tabFirmas" Font-Size="8pt" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
					Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0" EnableViewState="False"></asp:table></td>
		</tr>
	</tbody>
</table>
<BLOCKQUOTE dir="ltr" style="MARGIN-RIGHT: 0px">
	<P><asp:label id="lb" runat="server"></asp:label></P>
</BLOCKQUOTE>
