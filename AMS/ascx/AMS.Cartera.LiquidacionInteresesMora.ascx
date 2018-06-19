<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.LiquidacionInteresesMora.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.LiquidacionMora" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ='text/javascript'>
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<fieldset>
<TABLE id="Tabl" class="filtersIn">
	<tbody>
		<tr>
			<td>Digite la&nbsp; tasa de&nbsp; interes :
			<br /><asp:textbox id="tbTasa" class="tpequeno" runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="tbTasa">*</asp:requiredfieldvalidator>
            </td>
            </tr>
            <tr>
			<td>Digite los dias de gracia :
			<br /><asp:textbox id="tbGracia" class="tpequeno" runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfv2" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="tbGracia">*</asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td>Escoja el año a liquidar :
            <br /><asp:dropdownlist id="ddlAno" class="dpequeno" runat="server"></asp:dropdownlist></td>
            </tr>
            <tr>
			<td>Escoja el mes a liquidar :
            <br /><asp:dropdownlist id="ddlMes" class="dpequeno" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>
				Escoja el prefijo para las facturas a generar :	
			<br /><asp:dropdownlist id="ddlPrefijo" class="dmediano" runat="server"></asp:dropdownlist></td>
            </tr>
            <tr>
			<td>Seleccione el almacen :
			<br /><asp:dropdownlist id="ddlAlmacen" class="dpequeno" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td><asp:checkbox id="cbNit" onclick="MostrarDiv('divNit')" Runat="server" Checked="False" Text="Un nit específico"></asp:checkbox></td>
			<td>
				<div id="divNit" style="DISPLAY: none"><asp:textbox id="tbNit" ondblclick="ModalDialog(this,'SELECT mnit_nit AS NIT,mnit_apellidos || \' \' || COALESCE(mnit_apellido2,\'\') || \' \' || mnit_nombres || \' \' || COALESCE(mnit_nombre2,\'\')  AS NOMBRE FROM dbxschema.mnit ORDER BY mnit_nit ASC',new Array())"
						Runat="server" ToolTip="Haga doble click para iniciar la busqueda"></asp:textbox></div>
			</td>
		</tr>
	</tbody>
</table>
<p><asp:button id="btnPreLiquidar" onclick="btnPreLiquidar_Click" runat="server" Text="Preliquidar Intereses"></asp:button></p>
<p><asp:linkbutton id="lnbReporte" runat="server" Visible="False" onclick="lnbReporte_Click">Ver Cruces</asp:linkbutton></p>
<p><asp:datagrid id="dgPreLiq" runat="server" cssclass="datagrid" onItemCommand="dgPreLiq_ItemCommand" onItemDataBound="dgPreLiq_DataBound"
		onPageIndexChanged="dgPreLiq_PageIndexChanged" CellPadding="3" AutoGenerateColumns="False" ShowFooter="True"
		AllowPaging="True" PageSize="5">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="NIT" ReadOnly="True" HeaderText="Nit"></asp:BoundColumn>
			<asp:BoundColumn DataField="FACREL" ReadOnly="True" HeaderText="Facturas Relacionadas"></asp:BoundColumn>
			<asp:BoundColumn DataField="FALTANTE" ReadOnly="True" HeaderText="Valor Faltante por Pagar" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="INTERESES" ReadOnly="True" HeaderText="Intereses a Cobrar" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Liquidar S/N">
				<ItemTemplate>
					<center>
						<asp:CheckBox id="chbLiq" runat="server" />
					</center>
				</ItemTemplate>
				<FooterTemplate>
					<center>
						<asp:Button id="btnSelAll" runat="server" Text="Liquidar Todos" CommandName="Liquidar_Todos" />
					</center>
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></p>
<p><asp:button id="btnLiquidar" onclick="btnLiquidar_Click" runat="server" Text="Liquidar Intereses"
		Visible="False"></asp:button>&nbsp;
	<asp:button id="btnCancelar" runat="server" Text="Cancelar Proceso" CausesValidation="False" onclick="btnCancelar_Click"></asp:button></p>
<p><asp:label id="lb" runat="server"></asp:label></p>
</fieldset>
<script type ='text/javascript'>
	function MostrarDiv(idDiv)
	{
		var dv=document.getElementById(idDiv);
		if(dv.style.display=='none')
			dv.style.display='';
		else
			dv.style.display='none';
	}
</script>
