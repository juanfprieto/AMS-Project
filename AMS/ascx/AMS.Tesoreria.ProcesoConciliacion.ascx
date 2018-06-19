<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ProcesoConciliacion.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ProcesoConciliacion" %>
<p>Este es su archivo de movimientos bancarios :
</p>
<p><asp:datagrid id="dgConciliacion" onPageIndexChanged="dgConciliacion_PageChanged" AllowPaging="True"
		AutoGenerateColumns="False" runat="server" cssclass="datagrid" PageSize="10" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
	</asp:datagrid></p>
<p>Estos son los movimientos que se encuentran registrados en&nbsp;tesoreria para 
	el mes elegido (los egresos NO ENTREGADOS NO se incluyen en este 
	proceso),&nbsp;junto con los movimientos pendientes del mes anterior :
</p>
<p><asp:datagrid id="dgMovimiento" onPageIndexChanged="dgMovimiento_PageChanged" AllowPaging="True"
		AutoGenerateColumns="False" runat="server" cssclass="datagrid" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="FECHA" HeaderText="Fecha"></asp:BoundColumn>
			<asp:BoundColumn DataField="CODIGO" HeaderText="Prefijo"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO" HeaderText="N&#250;mero"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" HeaderText="Valor" DataFormatString="{0:C}"></asp:BoundColumn>
		</Columns>
	</asp:datagrid></p>
<p><asp:button id="btnConciliar" onclick="btnConciliar_Click" runat="server" Text="Ejecutar Conciliación"></asp:button></p>
<p><asp:label id="lb" runat="server"></asp:label></p>
