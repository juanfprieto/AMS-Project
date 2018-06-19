<%@ Control Language="c#" codebehind="AMS.Contabilidad.DefCueAnexos.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.DefCueAnexos" %>
<LINK href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
	</script>
	<br>
	<p>
		<table id="table1" class="filters">
			<tbody>
				<tr>
					<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
					<td>
						<p>
                        <table id="Table" class="filtersIn">
                        <tr>
                        <td>
                        Año:
							<asp:dropdownlist id="year" class="dpequeno" runat="server"></asp:dropdownlist>&nbsp;&nbsp;&nbsp; 
							Mes:
							<asp:dropdownlist id="month" class="dpequeno" runat="server"></asp:dropdownlist>&nbsp;&nbsp; 
							<br />Anexo:
							<asp:dropdownlist id="anexo" class="dmediano" runat="server"></asp:dropdownlist>
                            <br />
                            <asp:radiobuttonlist id="opciones" runat="server">
								<asp:ListItem Selected="True">Mes</asp:ListItem>
						        <asp:ListItem >Bimestre</asp:ListItem>
								<asp:ListItem >Acumulado Año</asp:ListItem>
                      	</asp:radiobuttonlist>
                        <br />
                        <asp:button id="Button1" onclick="generar" runat="server" Text="Generar"></asp:button>
                        </td>
                        </tr>
                        </table>
                        </p>
					</td>
				</tr>
			</tbody></table>
	</p>
	<p></p>
	<p><asp:placeholder id="toolsHolder" runat="server" visible="false">
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
			</TABLE>
		</asp:placeholder></p>
	<p></p>
	<br>
	<table class="reports" width="780" align="center" bgColor="gray">
		<tbody>
			<tr>
				<td><asp:Table id="tabPreHeader" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
						Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"
						Width="100%"></asp:Table></td>
			</tr>
			<tr>
				<td align="center">
					<p><asp:datagrid id="Grid" runat="server" cssclass="datagrid" AutoGenerateColumns="False" onselectedindexchanged="Grid_SelectedIndexChanged">
							<FooterStyle cssclass="footer"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
							<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
							<ItemStyle cssclass="item"></ItemStyle>
							<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="CODIGO DEL RENGLON O LINEA" ItemStyle-HorizontalAlign="Left">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="NOMBRE DEL RENGLON O LINEA">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="TOTAL DEL RENGLON O LINEA" ItemStyle-HorizontalAlign="Right">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "TOTAL") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></p>
				</td>
			</tr>
			<tr>
				<td><asp:table id="tabFirmas" BorderWidth="0px" EnableViewState="False" CellSpacing="0" CellPadding="1"
						BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"></asp:table></td>
			</tr>
		</tbody></table>
	<BLOCKQUOTE dir="ltr" style="MARGIN-RIGHT: 0px">
		<p><asp:label id="lb" runat="server"></asp:label></p>
	</BLOCKQUOTE>
