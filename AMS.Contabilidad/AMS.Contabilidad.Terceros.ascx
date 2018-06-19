﻿<%@ Control Language="c#" codebehind="AMS.Contabilidad.Terceros.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.Terceros" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
	</script>
	<table class="filters">
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
						<asp:DropDownList id="year" class="dpequeno" runat="server"></asp:DropDownList>
						 Mes:
						<asp:DropDownList id="month" class="dpequeno" runat="server"></asp:DropDownList>
						<br />
					
						Ordenar por:<br>
						<asp:RadioButtonList id="RadioOpcion" runat="server">
							<asp:ListItem Value="CUE" Selected="True">Cuenta</asp:ListItem>
							<asp:ListItem Value="NIT">NIT</asp:ListItem>
							<asp:ListItem Value="NOM">Nombre</asp:ListItem>
						</asp:RadioButtonList>
					
						<asp:Button id="Consulta" onclick="Consulta_Click" runat="server" Text="Generar"></asp:Button>
                        </td>
                        </tr>
                        </table>
					</p>
				</td>
			</tr>
		</tbody>
	</table>
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
	</p>
	<table class="reports" width="780" align="center" bgcolor="gray">
		<tbody>
			<tr>
				<td>
					<asp:Table id="tabPreHeader" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
						Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"
						EnableViewState="False" Width="100%"></asp:Table>
				</td>
			</tr>
			<tr>
				<td align="center">
					<p>
						<asp:DataGrid id="dg" runat="server" cssclass="datagrid" EnableViewState="False" AutoGenerateColumns="false">
							<FooterStyle cssclass="footer"></FooterStyle>
							<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
							<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
							<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
							<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
							<ItemStyle cssclass="item"></ItemStyle>
						</asp:DataGrid>
					</p>
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
	<p>
	</p>
	<p>
		<asp:Label id="lblAux" runat="server"></asp:Label>
	</p>