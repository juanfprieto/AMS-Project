<%@ Control Language="c#" codebehind="AMS.Tools.PrePrintedForms.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.PrePrintedForms" %>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
				<img height="70" src="../img/AMS.Flyers.News.png" border="0">
			</th>
			<td>
				<p>
					Tipo de Documento Generico :
					<asp:DropDownList id="ddlDocGen" runat="server" OnSelectedIndexChanged="Cambio_Documento" AutoPostBack="True" class="dmediano">
						<asp:ListItem Value="F" Selected="True">Facturas</asp:ListItem>
						<asp:ListItem Value="C">Cheques</asp:ListItem>
					</asp:DropDownList>
					&nbsp;&nbsp;
				</p>
				<p>
					Documento Especifico :
					<asp:DropDownList id="ddlDocEsp" runat="server" class="dmediano"></asp:DropDownList>
				</p>
				<p>
					<asp:Button id="btnDis" onclick="Go_Diseno1" runat="server" Text="Diseñar"></asp:Button>
				</p>
			</td>
		</tr>
	</tbody>
</table>
<asp:Label id="lb" runat="server"></asp:Label>
</fieldset>