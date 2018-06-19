<%@ Control Language="c#" codebehind="AMS.Nomina.ActivarQuincena.cs" autoeventwireup="false" Inherits="AMS.Nomina.ActivarQuincena" %>
<fieldset>

<table id="Table" class="filtersIn">
<tr>
<td>
<p>
	Escoja la Quincena a Activar
</p>
<p>
	<asp:DataGrid id="DATAGRIDACTIVARQ" CssClass="datagrid" OnItemCommand="ActivaQuincena" AutoGenerateColumns="False"
		runat="server">
		<FooterStyle CssClass="footer"></FooterStyle>
	    <HeaderStyle CssClass="header"></HeaderStyle>
	    <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
	    <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
	    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						
		<Columns>
			<asp:BoundColumn DataField="CODIGO QUINCENA" HeaderText="CODIGO QUINCENA"></asp:BoundColumn>
			<asp:BoundColumn DataField="A&#209;O" HeaderText="A&#209;O"></asp:BoundColumn>
			<asp:BoundColumn DataField="MES" HeaderText="MES"></asp:BoundColumn>
			<asp:BoundColumn DataField="PERIODO NOMINA" HeaderText="PERIODO NOMINA"></asp:BoundColumn>
			<asp:BoundColumn DataField="ESTADO" HeaderText="ESTADO"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="ACTIVAR">
				<ItemTemplate>
					<asp:Button id="Button1" runat="server" Text="Act." enabled="false"></asp:Button>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	&nbsp;&nbsp;
	<asp:Panel id="PANELBORRARDATOS" runat="server" Visible="False" >
		<P>Ademas de activar la quincena ud tiene que borrar los datos almacenados para 
			volver a liquidar definitivamente&nbsp;la misma.
		</P>
		<P>ATENCION:Este proceso es irreversible.
		</P>
		<P></P>
		<CENTER>
			<asp:Button id="Button1" onclick="BorrarDatos" runat="server" class="tpequeno" Text="BORRAR DATOS"
				align="center"></asp:Button></CENTER>
		<P></P>
	</asp:Panel>
<P></P>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
	<asp:Label id="LBPRUEBA" runat="server"></asp:Label>
</p>
</td></tr></table></fieldset>