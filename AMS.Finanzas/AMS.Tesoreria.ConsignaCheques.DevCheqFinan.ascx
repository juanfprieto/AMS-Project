<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.DevCheqFinan.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.DevolucionChequesFinanciera" %>
<p>
	<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
	<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
</p>
<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				Nit de la Financiera :
			</td>
			<td>
				<asp:TextBox id="tbNitFin" onclick="ModalDialog(this,'SELECT a.mnit_nit AS Nit,a.mnit_nombres CONCAT \' \' CONCAT a.mnit_apellidos AS Razon_Social FROM mnit a,pfinanciera b where a.mnit_nit = b.mnit_nit ORDER BY a.mnit_nit',new Array())"
					ReadOnly="True" runat="server" Width="122px"></asp:TextBox>
				<asp:RequiredFieldValidator id="rfvNit" runat="server" ControlToValidate="tbNitFin" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>
				Número de Cuenta Corriente :
			</td>
			<td>
				<asp:TextBox id="tbCuenta" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo AND P.ttip_tipocuenta=\'F\'',new Array())"
					ReadOnly="True" runat="server" Width="122px"></asp:TextBox>
				<asp:RequiredFieldValidator id="rfvCuenta" runat="server" ControlToValidate="tbCuenta" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>
				<p>
					Escoja el prefijo para la factura por la devolución :
				</p>
			</td>
			<td>
				<asp:DropDownList id="ddlPrefijo" runat="server"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				Fecha Actual :
			</td>
			<td>
				<asp:TextBox id="fecha" onkeyup="DateMask(this)" runat="server" Width="92px"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:Button id="aceptar" onclick="aceptar_Click" runat="server" Text="Aceptar"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
<p>
</p>
<asp:DataGrid id="gridDatos" runat="server" cssclass="datagrid" ShowFooter="True" CellPadding="3" AutoGenerateColumns="False" onItemCommand="gridDatos_Item">
	<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
	<FooterStyle cssclass="footer"></FooterStyle>
	<Columns>
		<asp:BoundColumn DataField="CODIGOREM" ReadOnly="True" HeaderText="C&#243;digo Remisi&#243;n"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMEROREM" ReadOnly="True" HeaderText="N&#250;mero Remisi&#243;n"></asp:BoundColumn>
		<asp:BoundColumn DataField="CODIGOFAC" ReadOnly="True" HeaderText="C&#243;digo F&#225;ctura Cobro"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMEROFAC" ReadOnly="True" HeaderText="N&#250;mero F&#225;ctura Cobro"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="N&#250;mero del Documento"></asp:BoundColumn>
		<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor" DataFormatString="{0:c}"></asp:BoundColumn>
		<asp:BoundColumn DataField="FECHA" ReadOnly="True" HeaderText="Fecha Remisi&#243;n"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Prefijo Nota Devoluci&#243;n Cliente">
			<ItemTemplate>
				<asp:DropDownList id="ddlPrefNot" runat="server"></asp:DropDownList>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Devolver Cheques">
			<ItemTemplate>
				<center>
					<asp:CheckBox id="ckbDev" runat="server" />
				</center>
			</ItemTemplate>
			<FooterTemplate>
				<center>
					<asp:Button id="devChq" runat="server" Text="Devolver" CommandName="devolver" Width="70" />
				</center>
			</FooterTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
