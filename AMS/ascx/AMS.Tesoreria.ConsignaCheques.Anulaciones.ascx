<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.Anulaciones.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.Anulaciones" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<table id="Table1" class="filters">
	<tbody>
		<tr>
			<td>Tipo de Documento a Anular :
			</td>
			<td><asp:dropdownlist id="tipoDocAnular" runat="server" AutoPostBack="True" onSelectedIndexChanged="tipoDocAnular_Changed"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>Número del Documento :
			</td>
			<td><asp:dropdownlist id="numeroDocumento" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td align="center" colSpan="2"><asp:button id="aceptar" runat="server" Text="Aceptar" onclick="aceptar_Click"></asp:button>&nbsp;
				<asp:button id="btCan" Text="Cancelar" Runat="server" onclick="btCan_Click"></asp:button></td>
		</tr>
	</tbody>
</table>
<p></p>
<p><asp:label id="lbInfo" runat="server"></asp:label></p>
<P><asp:label id="lbTitulo" runat="server" Visible="False"></asp:label><asp:dropdownlist id="ddlPrefDev" runat="server" Visible="False"></asp:dropdownlist>
	<asp:DropDownList id="ddlPrefDevProv" runat="server" Visible="False"></asp:DropDownList></P>
<p><asp:datagrid id="gridDatos" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
	</asp:datagrid></p>
<P><asp:datagrid id="dgDev" runat="server" Visible="False" AutoGenerateColumns="False">
		<Columns>
			<asp:BoundColumn DataField="CUENTA" ReadOnly="True" HeaderText="Cuenta Corriente"></asp:BoundColumn>
			<asp:BoundColumn DataField="FECHA" ReadOnly="True" HeaderText="Fecha" DataFormatString="{0:yyyy'-'MM'-'dd}"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO FACTURA" ReadOnly="True" HeaderText="Prefijo Factura"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO FACTURA" ReadOnly="True" HeaderText="N&#250;mero Factura"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO CONSIGNACION" ReadOnly="True" HeaderText="Prefijo Consignaci&#243;n"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO CONSIGNACION" ReadOnly="True" HeaderText="N&#250;mero Consignaci&#243;n"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO RECIBO CAJA" ReadOnly="True" HeaderText="Prefijo Recibo Caja"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO RECIBO CAJA" ReadOnly="True" HeaderText="N&#250;mero Recibo Caja"></asp:BoundColumn>
			<asp:BoundColumn DataField="BANCO" ReadOnly="True" HeaderText="Banco"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO DOCUMENTO" ReadOnly="True" HeaderText="N&#250;mero del Documento"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Prefijo Nota Devoluci&#243;n">
				<ItemTemplate>
					<asp:DropDownList id="ddlPrefNot" runat="server"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></P>
<P><asp:datagrid id="dgDevRem" runat="server" Visible="False" AutoGenerateColumns="False">
		<Columns>
			<asp:BoundColumn DataField="PREFIJO REMISION" ReadOnly="True" HeaderText="Prefijo Remisi&#243;n"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO REMISION" ReadOnly="True" HeaderText="N&#250;mero Remisi&#243;n"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO CHEQUE" ReadOnly="True" HeaderText="N&#250;mero Cheque"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO FACTURA CLIENTE" ReadOnly="True" HeaderText="Prefijo Factura Cliente"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO FACTURA CLIENTE" ReadOnly="True" HeaderText="N&#250;mero Factura Cliente"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO RECIBO CAJA" ReadOnly="True" HeaderText="Prefijo Recibo Caja"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO RECIBO CAJA" ReadOnly="True" HeaderText="N&#250;mero Recibo Caja"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO DEVOLUCION FINANCIERA" ReadOnly="True" HeaderText="Prefijo Devoluci&#243;n Financiera"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO DEVOLUCION FINANCIERA" ReadOnly="True" HeaderText="N&#250;mero Devoluci&#243;n Financiera"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Prefijo Devoluci&#243;n Factura Cliente">
				<ItemTemplate>
					<asp:DropDownList id="ddlPrefNotRem" runat="server"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></P>
<p><asp:label id="lbEfectivo" runat="server"></asp:label></p>
<input id="hdnTotal" type ="hidden"  runat="server" >
<input id="hdnTip" type ="hidden" runat="server">

