<%@ Control Language="c#" codebehind="AMS.Automotriz.Categorizacion.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.Categorizacion" %>
<p>
</p>
<fieldset>
    <table id="Table" class="filtersIn">
    <tr>
    <td>
    <p>
	    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	    <asp:Label id="titulo1" class="lmediano" runat="server">Categorización
        de Clientes del Taller</asp:Label>
    </p>
    <p>
    </p>
    <p>
	    <asp:Label id="titulo2" runat="server">Permite MARCAR los CLIENTES del Taller por
        CATEGORÍAS de la A hasta la Q, dependiendo de la frecuencia de visitas al taller en
        los últimos CINCO (5) PERÍODOS</asp:Label>
    </p>
    <p>
    </p>

	<legend>Períodos de Tiempo</legend>
	<asp:RadioButtonList id="opcionesTiempo" runat="server" RepeatDirection="Horizontal" BorderStyle="None">
		<asp:ListItem Value="anos" Selected="True">Ultimos 5 a&#241;os</asp:ListItem>
		<asp:ListItem Value="semestres">Ultimos 5 Semestres</asp:ListItem>
	</asp:RadioButtonList>
    </td>
</tr>

    </table>
<p>
	<asp:Button id="categorizar" onclick="Categorizar_Clientes" runat="server" class="bpequeno" Text="Categorizar"></asp:Button>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button id="mostrarLista" onclick="Mostrar_Lista" runat="server" class="bmediano" Text="Mostrar Lista Completa"></asp:Button>
</p>
</fieldset>
<p>
	<asp:DataGrid id="grillaCategorizacion" runat="server" cssclass="datagrid" ShowFooter="false" GridLines="Vertical" AutoGenerateColumns="true"
		Visible="False">
		<FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
	</asp:DataGrid>
</p>
<p>
	<asp:DataGrid id="grillaResultado" runat="server" ShowFooter="false" AutoGenerateColumns="true" cssclass="datagrid">
		<FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
	</asp:DataGrid>
</p>
<p>
	<asp:Label id="labelInformativo" runat="server"></asp:Label>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
