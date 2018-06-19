<%@ Control Language="c#" codebehind="AMS.Contabilidad.LibDia.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.LibDia" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
	</script>
	<br>
	<div class="header">
		<p>
			<asp:Label id="reportInfo" runat="server"></asp:Label>
		</p>
		<p>
			<table id="Table1" class="filtersIn">
				<tbody>
					<tr>
						<th class="filterHead">
							<img height="60" src="../img/AMS.Flyers.Filters.png" border="0">
						</th>
						<td>
							<p>
								<asp:PlaceHolder id="filterHolder" runat="server"></asp:PlaceHolder>
							</p>
						</td>
					</tr>
				</tbody>
			</table>
		</p>
		<p>
			<asp:PlaceHolder id="toolsHolder" runat="server" visible="false">
				<TABLE class="tools" width="780">
					<TR>
						<th class="filterHead">
                        <IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0">
                        </th>
						<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</TD>
						<TD>&nbsp; &nbsp;Enviar por correo
							<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
						<TD>
							<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
								ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
							<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" ImageUrl="../img/AMS.Icon.Mail.jpg"
								alt="Enviar por email"></asp:ImageButton></TD>
						<TD width="380"></TD>
					</TR>
				</TABLE>
			</asp:PlaceHolder>
		</p>
		<p>
			<table class="reports" width="780" bgcolor="gray">
				<tbody>
					<tr>
						<td>
							<asp:Table id="tabPreHeader" BorderWidth="0px" CellSpacing="0" CellPadding="1" BackColor="White"
								GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"
								Width="100%"></asp:Table>
						</td>
					</tr>
					<tr>
						<td>
							<ASP:DataGrid id="report" runat="server" cssclass="datagid" CellSpacing="1" CellPadding="3"
								AllowSorting="true" autogeneratecolumns="false"
								OnItemDataBound="Report_ItemDataBound">
								<FooterStyle cssclass="footer""></FooterStyle>
								<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
								<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
								<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
								<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
								<ItemStyle cssclass="item"></ItemStyle>
							</ASP:DataGrid>
						</td>
					</tr>
					<tr>
						<td align="center">
							<asp:Table id="tabFirmas" BorderWidth="0px" CellSpacing="0" CellPadding="1" BackColor="White"
								GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"
								EnableViewState="False" Width="100%"></asp:Table>
						</td>
					</tr>
				</tbody>
			</table>
		</p>
	</div>
