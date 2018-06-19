<%@ Control Language="c#" codebehind="AMS.Nomina.LiquidacionIndividual.cs" autoeventwireup="false" Inherits="AMS.Nomina.Liquidacionnomina" %>
<h4>PRELIQUIDACION INFORMATIVA
</h4>
<p>
	<asp:Label id="titulo" runat="server">Ingrese los datos</asp:Label><asp:Label id="prueba" runat="server" width="122px" visible="False"></asp:Label><asp:Label id="lbdocref" runat="server"></asp:Label><asp:Label id="lbmas1" runat="server" visible="False"></asp:Label><asp:Label id="prueba2" runat="server"></asp:Label>
</p>
<p>
</p>
<table class="filtersIn">
	<tbody>
		<tr>
			<td>
				<asp:Label id="Label1" runat="server" class="lmediano" height="21px">Periodo a procesar </asp:Label></td>
			<td>
				<asp:DropDownList id="DDLQUIN" runat="server" class="dmediano"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label3" runat="server">Mes</asp:Label></td>
			<td>
				<asp:DropDownList id="DDLMES" runat="server" class="dmediano"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;<asp:Label id="Label2" runat="server" class="lpequeno">Año</asp:Label>
			</td>
			<td>
				<asp:DropDownList id="DDLANO" runat="server" class="dmediano"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label id="Label4" runat="server">Tipo de pago</asp:Label></td>
			<td>
				<asp:DropDownList id="DDLTIPOPAGO" runat="server" class="dmediano"></asp:DropDownList>
			</td>
		</tr>
		<tr>
		</tr>
		<tr>
			<td>
				<asp:Button id="consulta" onclick="realizar_consulta" runat="server" Text="Enviar"></asp:Button>
			</td>
		</tr>
		<tr>
		</tr>
	</tbody>
</table>
<p>
	<asp:Label id="Label5" runat="server" width="445px" height="59px">El proceso de liquidacion
    de nómina, realiza los pagos de los empleados correspondientes al PERIODO VIGENTE,
    antes de realizar este procedimiento debe haber ingresado las novedades que afecten
    al periodo de pago.</asp:Label>
</p>
<p>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					<p>
						<asp:Label id="Label8" runat="server" class="lmediano">Nombre del Liquidador:</asp:Label>
					</p>
				</td>
				<td>
					<asp:Label id="lbliquidador" runat="server" class="lmediano"></asp:Label></td>
			</tr>
		</tbody>
	</table>
	<asp:DataGrid id="DataGrid2" runat="server" PageSize="40" onPageIndexChanged="Grid_Change" AllowPaging="True">
		<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
	</asp:DataGrid>
	<br>
	<br>
	<asp:Label id="lbperipago" runat="server" visible="False" font-size="Larger">Listado
    Empleados Diferente Periodo de Pago al Seleccionado</asp:Label>
</p>
<p>
</p>
<p>
</p>
<p>
	<asp:DataGrid id="DataGrid1" runat="server" onPageIndexChanged="Grid_Change2" AllowPaging="True"
		ShowFooter="True" AllowCustomPaging="True" >
		<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
	</asp:DataGrid>
</p>
<p>
</p>
<p>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td align="center">
					<asp:Button id="BTNCONFIRMACION" onclick="realizar_liquidacion" runat="server" Text="LIQUIDAR DEFINITIVAMENTE"
						Visible="False"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label><asp:Label id="lb2" runat="server"></asp:Label><asp:Label id="lbpag" runat="server"></asp:Label><asp:Label id="lbpag2" runat="server"></asp:Label><asp:Label id="lb3" runat="server" width="59px"></asp:Label>
</p>
