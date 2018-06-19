<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Nomina.AdmonCentroCosto.ascx.cs" Inherits="FabianNomina.AMS_Nomina_AdmonCentroCosto" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>Empleado :</p>
<asp:dropdownlist id="DDLEMPLEADO" AutoPostBack="True" OnSelectedIndexChanged="CargarDatosEmpleado"
	class="dmediano" runat="server"></asp:dropdownlist>
</td></tr>
<br />
<tr>
<td>
<asp:datagrid id="gridRtns" runat="server" Visible="false" showfooter="true" AutoGenerateColumns="False" CellPadding="3" onItemCommand="gridRtns_Item" cssclass="datagrid">
	        <FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Codigo Empleado">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CODEMP") %>
			</ItemTemplate>
			<FooterTemplate>
				<center>
				</center>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="ALMACEN">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "ALMACEN") %>
			</ItemTemplate>
			<FooterTemplate>
				<center>
					<asp:TextBox id="almacenCos" class="tmediano" runat="server" ReadOnly="true" onClick="ModalDialog(this,'SELECT PALM_ALMACEN, PALM_DESCRIPCION FROM PALMACEN ORDER BY PALM_DESCRIPCION',new Array())"></asp:TextBox>
				</center>
			</FooterTemplate>
		</asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Centro de Costo">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CENTROCOSTO") %>
			</ItemTemplate>
			<FooterTemplate>
				<center>
					<asp:TextBox id="codretb" class="tmediano" runat="server" ReadOnly="true" onClick="ModalDialog(this,'SELECT pcen_codigo AS Codigo,pcen_nombre AS Nombre FROM dbxschema.pcentrocosto ORDER BY pcen_codigo',new Array())"></asp:TextBox>
				</center>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Porcentaje">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "PORC", "{0:N}") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:TextBox id="valor" runat="server" class="tmediano" Enabled="true" onkeyup="NumericMask(this)"></asp:TextBox>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Agregar">
			<ItemTemplate>
				<asp:Button id="remRet" runat="server" class="bpequeno" CommandName="RemoverCentroCosto" Text="Remover" />
			</ItemTemplate>
			<FooterTemplate>
				<center>
					<asp:Button id="agRet" runat="server" class="bpequeno" CommandName="AgregarCentroCosto" Text="Agregar" Enabled="true" />
				</center>
			</FooterTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
</asp:datagrid>
</td>
</tr>
<tr>
    <td>
        <asp:Button id="btn_guardar" runat="server" Text="Guardar"></asp:Button><br /><p>Aqui distribuye usted los gastos de los empleados por sedes y centros de costos, la distribucion debe dar 100%</p>
    </td>
</tr>
</table>
<asp:Label id="lb" runat="server"></asp:Label>
</fieldset>
