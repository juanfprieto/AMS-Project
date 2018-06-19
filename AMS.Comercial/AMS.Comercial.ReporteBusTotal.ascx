<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ReporteBusTotal.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ReporteBusTotal" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<LINK href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="JavaScript">

    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<asp:panel id="Panel1" Height="136px" HorizontalAlign="Center" runat="server">
	<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" align="center" border="1">
		<TR>
			<TD width="354">
				<P><STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Seleccione 
						por favor</STRONG></P>
				<P><STRONG>1.Agencia De&nbsp;Buses</STRONG></P>
				<P>&nbsp;</P>
			</TD>
		</TR>
		<TR>
			<TD width="354"><STRONG>Agencia:</STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:dropdownlist id="ddlagencia" runat="server" Width="295px">
					<asp:ListItem Value=" "></asp:ListItem>
				</asp:dropdownlist></TD>
		</TR>
	</TABLE>
</asp:panel><br>
<p>
	<table class="filters" id="Table2" style="WIDTH: 936px; HEIGHT: 441px" width="936" align="center">
		<tbody>
			<tr>
				<td vAlign="middle" borderColor="gray" width="16" colSpan="0"><IMG height="60" src="../img/AMS.Flyers.Filters.png" border="0">
				</td>
				<td vAlign="middle">
					<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						Haga Clic en las Fechas Para Generar el Reporte :</p>
					<p>
						<TABLE id="Table1" style="WIDTH: 546px; HEIGHT: 162px" cellSpacing="1" cellPadding="1"
							width="546" align="center" border="1">
							<TR>
								<TD style="WIDTH: 98px"></TD>
								<TD style="WIDTH: 221px">
									<P align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
										&nbsp;Fecha Desde :</P>
								</TD>
								<TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
									&nbsp; Fecha Hasta :</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 98px" align="center">Fecha :</TD>
								<TD style="WIDTH: 221px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:TextBox id="FechaInicio" onKeyUp="DateMask(this);" runat="server"></asp:TextBox><STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
										(yyyy-mm-dd)</STRONG>
								<TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:TextBox id="FechaFinal" onKeyup="DateMask(this);" runat="server"></asp:TextBox><STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
										(yyyy-mm-dd)</STRONG></TD>
				</td>
			</tr>
			<TR>
			</TR>
	</table>
</p>
<P>&nbsp;</P>
<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="Button1" onclick="generar" runat="server" Width="305px" Text="Generar Reporte"></asp:button>
</P>
</td></tr></tbody></table></p>
<p></p>
<p><asp:placeholder id="toolsHolder" runat="server" visible="false">&lt; 
<TABLE class="tools" width="780">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
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
			<td><asp:table id="tabPreHeader" HorizontalAlign="Center" Width="100%" Font-Size="8pt" BorderWidth="0px"
					CellSpacing="0" CellPadding="1" BackColor="White" GridLines="Both" Runat="server" Font-Name="Verdana"></asp:table></td>
		</TR>
		<tr>
			<td align="center">
				<p><asp:datagrid id="Grid" HorizontalAlign="Center" runat="server" Width="780px" AutoGenerateColumns="False">
						<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="NUMERO DEL BUS" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO BUS") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PLACA DEL BUS" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CIUDAD ORIGEN" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "ORIGEN") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CIUDAD DESTINO" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DESTINO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="VALOR" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="HORA SALIDA" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "HORA SALIDA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FECHA" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="CODIGO RUTA" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODIGO RUTA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO PLANILLA" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO PLANILLA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="RELEVANTE" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "RELEVANTE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="NUMERO DE PASAJEROS" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO DE PASAJEROS") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="SILLAS DISPONIBLES" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DISPONIBLES") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="TOTAL" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TOTAL") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></p>
			</td>
		</tr>
		<tr>
			<td><asp:table id="tabFirmas" HorizontalAlign="Center" Font-Size="8pt" BorderWidth="0px" CellSpacing="0"
					CellPadding="1" BackColor="White" GridLines="Both" Runat="server" Font-Name="Verdana" EnableViewState="False"></asp:table></td>
		</tr>
	</tbody>
</table>
<BLOCKQUOTE dir="ltr" style="MARGIN-RIGHT: 0px">
	<P><asp:label id="lb" runat="server"></asp:label></P>
</BLOCKQUOTE>
