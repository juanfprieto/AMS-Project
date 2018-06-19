<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.ConsultaFacturas.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.ConsultaFacturas" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("[src*=minus]").each(function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999' align='left' bgcolor= '#A1DCF2'>" + $(this).next().html() + "</td></tr>");
            $(this).next().remove()
        });
    });
    </script>
     <style type="text/css">
    .ChildGrid
    {
        text-align:left;
    }
    .ChildGrid td
        {
            background-color: #eee !important;
            color: black;
        }
        .ChildGrid th
        {
            background-color: #6C6C6C !important;
            color: White;
        }
    </style>
<fieldset>
<p>Seleccione el filtro :
</p>
<table id="Table" class="filtersIn">
<tbody>
<tr>
<td>
<p><asp:radiobuttonlist id="rblFiltro" AutoPostBack="true" onSelectedIndexChanged="rblFiltro_IndexChanged"
		runat="server">
		<asp:ListItem Value="N">Nit</asp:ListItem>
		<asp:ListItem Value="F">Factura</asp:ListItem>
	</asp:radiobuttonlist></p>
<p><asp:panel id="pnl1" runat="server">
		<P>
			<asp:Label id="lb1" runat="server"></asp:Label>&nbsp;
			<asp:DropDownList id="ddl1" runat="server" onSelectedIndexChanged="ddl1_IndexChanged" Visible="false" class="dmediano"></asp:DropDownList>&nbsp;
			<asp:TextBox id="TextBox1" ondblclick="ModalDialog(this,'SELECT DISTINCT mn.mnit_nit concat\' - \'concat mn.mnit_nombres concat\' \'concat coalesce(mn.mnit_nombre2,\'\') concat\' \'concat mn.mnit_apellidos concat\' \'concat coalesce(mn.mnit_apellido2,\'\')FROM mnit mn,mfacturacliente mf where mf.mnit_nit=mn.mnit_nit', new Array(),1)"
				runat="server" Visible="False" Width="320px"></asp:TextBox>
        </P>
		<P>
			<asp:Label id="lb2" runat="server"></asp:Label>&nbsp;
			<asp:DropDownList id="ddl2" runat="server" Visible="false" class="dpequeno"></asp:DropDownList>&nbsp;
		</P>
	</asp:panel>

<p><asp:button id="btnBuscar" onclick="btnBuscar_Click" runat="server" Text="Buscar"></asp:button></p>
</td>
</tr>
</tbody>
</table>
<p><asp:GridView id="dgBusqueda" runat="server" cssclass="datagrid" onItemCommand="dgBusqueda_Item" CellPadding="3" DataKeyNames="PREFIJO, NUMERO" >
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="imgShow" runat="server" OnClick="Show_Hide_ChildGrid" ImageUrl="../img/plus.png"
                        CommandArgument="Show" />
                    <asp:Panel ID="pnlOrders" runat="server" Visible="false" Style="position: relative">
                        <asp:GridView ID="gvOrders" runat="server"   CssClass="ChildGrid">
                        </asp:GridView>
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
	</asp:GridView></p>
<p><asp:datagrid id="dgAbonos" runat="server" cssclass="datagrid" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns></Columns>
	</asp:datagrid></p>
    </fieldset>

