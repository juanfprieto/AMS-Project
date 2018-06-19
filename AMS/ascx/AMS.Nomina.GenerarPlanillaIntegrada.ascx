<%@ Control Language="c#" codebehind="AMS.Nomina.GenerarPlanillaIntegrada.cs" autoeventwireup="false" Inherits="AMS.Nomina.GenerarPlanillaIntegrada" %>
<table class="filtersIn">
	<tbody>
		<tr>
			<td>
				&nbsp;Forma de&nbsp;Presentacion:&nbsp;</td>
			<td>
				&nbsp;<asp:DropDownList id="ddlFormap" runat="server">
					<asp:ListItem Value="U">Unico</asp:ListItem>
					<asp:ListItem Value="C">Consolidado</asp:ListItem>
					<asp:ListItem Value="S">Sucursal</asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				Código de la Sucursal:</td>
			<td>
				<asp:TextBox id="txtCodSucu" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Nombre de la Sucursal(Si no tiene nombre, coloque su código)</td>
			<td>
				<asp:TextBox id="TextBox2" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Código de la ARP(A la cual el aportante se afilia o cuando se traslada a otra 
				ARP):</td>
			<td>
				<asp:TextBox id="TextBox3" runat="server"></asp:TextBox>
			</td>
		</tr>
	</tbody>
</table>
<p>
</p>
<p>
	&nbsp;<asp:Button id="Button1" onclick="generarArchivoPlano" runat="server" Text="Generar"></asp:Button>
</p>
<p>
</p>
<p>
</p>
