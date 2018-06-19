<%@ Control Language="c#" codebehind="AMS.Contabilidad.CausaDiferidos.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.CausaDiferidos" %>
<fieldset>
<p>
	<asp:Label id="Label4" runat="server">Este proceso genera un comprobante que realiza
    la causacion de los diferidos. Debe realizarse una vez cada mes.</asp:Label>
</p>
<table id="Table" class="filtersIn">
<tbody>
<tr>
<td>

	<asp:Label id="Label1" runat="server">Comprobante: </asp:Label><br />
	<asp:DropDownList id="tComprobante" class="dmediano" runat="server"></asp:DropDownList>
	&nbsp;<br />
	<asp:Label id="Label2" runat="server">Año: </asp:Label><br />
	<asp:DropDownList id="ano" class="dpequeno" runat="server"></asp:DropDownList>
	&nbsp;&nbsp;<br />
	<asp:Label id="Label3" runat="server">Mes: </asp:Label><br />
	<asp:DropDownList id="mes" class="dpequeno" runat="server"></asp:DropDownList>

<p>
	<asp:Button id="efectuar" onclick="efectuar_causacion" runat="server" Text="Efectuar"></asp:Button>
</p>
</td>
</tr>
</tbody>
</table>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
</fieldset>
<p>
	<asp:Datagrid id="comprobanteG" runat="server" cssclass="datagrid" AutoGenerateColumns="False" align="center">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="CUENTA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NIT">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NIT") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="PREF">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PREF") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DOCREF">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DOC_REF") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DETALLE">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DETALLE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="SEDE">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "SEDE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="C.COSTO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CENTRO_COSTO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DEBITO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DEBITO","{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="CREDITO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CREDITO","{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="BASE">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "BASE","{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:Datagrid>
</p>
<p>
	<asp:Button id="guardar" onclick="guardar_comprobante" runat="server" Text="Guardar Comprobante"
		Visible="False"></asp:Button>
</p>
