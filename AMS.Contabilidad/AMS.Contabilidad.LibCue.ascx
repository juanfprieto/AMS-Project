<%@ Control Language="c#" codebehind="AMS.Contabilidad.LibCue.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.LibCue" %>
<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<br>
<p>
	<table id="Table" class="filters">
		<tbody>
			<tr>
				<th class="filterHead">
					<img height="60" src="../img/AMS.Flyers.Filters.png" border="0">
				</th>
				<td valign="middle">
					<p>
						Año:
						<asp:DropDownList id="year" runat="server"></asp:DropDownList>
						&nbsp;&nbsp;&nbsp; Mes:
						<asp:DropDownList id="month" runat="server"></asp:DropDownList>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</p>
					<p>
						Ordenar por:<br>
						&nbsp;<asp:RadioButtonList id="RadioOpcion" runat="server">
							<asp:ListItem Value="DOC" Selected="True">Documento</asp:ListItem>
							<asp:ListItem Value="NIT">NIT</asp:ListItem>
							<asp:ListItem Value="NOM">Nombre</asp:ListItem>
							<asp:ListItem Value="SCC">Sede-CC</asp:ListItem>
							<asp:ListItem Value="DEB">Debito</asp:ListItem>
							<asp:ListItem Value="CRE">Credito</asp:ListItem>
						</asp:RadioButtonList>
					</p>
					<p>
						<asp:Button id="Consulta" onclick="Consulta_Click" runat="server" Text="Generar"></asp:Button>
					</p>
					<p>
					</p>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>
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
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 188px; POSITION: absolute; TOP: 271px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail.jpg"
						alt="Enviar por email" BorderWidth="0px"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:PlaceHolder>
</p>
<p>
</p>
<table class="reports" width="780" align="center" bgcolor="gray">
	<tbody>
		<tr>
			<td>
				<asp:Table id="tabPreHeader" BorderWidth="0px" EnableViewState="False" CellSpacing="0" CellPadding="1"
					BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"
					Width="100%"></asp:Table>
			</td>
		</tr>
		<tr>
			<td align="center">
				<p>
					<asp:DataGrid id="dg" runat="server" OnItemDataBound="Report_ItemDataBound" align="center"
						AutoGenerateColumns="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
					</asp:DataGrid>
				</p>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Table id="tabFirmas" BorderWidth="0px" EnableViewState="False" CellSpacing="0" CellPadding="1"
					BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana" HorizontalAlign="Center"
					Width="100%"></asp:Table>
			</td>
		</tr>
	</tbody>
</table>
<asp:Label id="lblAux" runat="server"></asp:Label>
