<%@ Control Language="c#" codebehind="AMS.Contabilidad.EstRes.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.EstRes" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
	</script>
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
				<TABLE id="Table2" class="filtersIn">
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
							<asp:Table id="tabPreHeader" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
								Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"
								Width="100%"></asp:Table>
						</td>
					</tr>
					<tr>
						<td>
							<ASP:DataGrid id="report" runat="server" cssclass="datagrid" GridLines="None" 
								CellPadding="3" CellSpacing="1" OnItemDataBound="Report_ItemDataBound" autogeneratecolumns="false"
								AllowSorting="true">
								<FooterStyle cssclass="footer"></FooterStyle>
								<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
								<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
								<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
								<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
								<ItemStyle cssclass="item"></ItemStyle>
							</ASP:DataGrid>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Table id="tabFirmas" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana" Font-Size="8pt"
								Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0" Width="100%"></asp:Table>
						</td>
					</tr>
				</tbody>
			</table>
		</p>
	</div>
