<%@ Control Language="c#" codebehind="AMS.Contabilidad.BalComp.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.BalComp" %>
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
			<table class="filtersIn">
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
				<TABLE class="filters">
					<TR>
						<th class="filterHead"> 
                        <IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0">
                        </th>

						<th class="filterHead">Imprimir
                         <A href="javascript: Lista()">
                        <IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</th>
						<TD>&nbsp; &nbsp;Enviar por correo
							<asp:TextBox id="tbEmail" runat="server"></asp:TextBox>
                        </TD>
						<TD>
							<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
								ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
							<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"
								BorderWidth="0px"></asp:ImageButton></TD>
						<TD></TD>
					</TR>
				</TABLE>
			</asp:PlaceHolder>
		</p>
		<p>
			<table class="filters">
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
								<FooterStyle  class="footer"></FooterStyle>
								<HeaderStyle  class="header"></HeaderStyle>
								<PagerStyle  class="pager"></PagerStyle>
								<SelectedItemStyle  class="selected"></SelectedItemStyle>
								<AlternatingItemStyle  class="alternate"></AlternatingItemStyle>
								<ItemStyle  class="item"></ItemStyle>
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
