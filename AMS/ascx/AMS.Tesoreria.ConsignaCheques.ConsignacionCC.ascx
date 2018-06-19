<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.ConsignacionCC.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ConsignacionChequesCC" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<table id="Table1" class="filtersIn"> 
	<tbody>
		<tr>
			<td>Código Cuenta Corriente :
			</td>
			<td><asp:textbox id="codigoCC" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo',new Array())"
					ReadOnly="True" runat="server" ToolTip="Haga Click" Width="72px"></asp:textbox><asp:requiredfieldvalidator id="validatorCCC" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="codigoCC">*</asp:requiredfieldvalidator></td>
			<td>Banco :
			</td>
			<td><asp:textbox id="codigoCCb" ReadOnly="True" runat="server" Width="72px"></asp:textbox><asp:requiredfieldvalidator id="validatorBanco" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="codigoCCb">*</asp:requiredfieldvalidator></td>
			<td>Fecha :
			</td>
			<td><img onmouseover="calendario.style.visibility='visible'" onmouseout="calendario.style.visibility='hidden'"
					src="../img/AMS.Icon.Calendar.gif" border="0">
				<table id="calendario" onmouseover="calendario.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
					onmouseout="calendario.style.visibility='hidden'">
					<tbody>
						<tr>
							<td><asp:calendar BackColor="Beige" id="calendarioFecha" runat="server" OnSelectionChanged="Cambiar_Fecha"></asp:calendar></td>
						</tr>
					</tbody>
				</table>
				<asp:textbox id="fecha" onkeyup="DateMask(this)" runat="server" Width="92px"></asp:textbox><asp:regularexpressionvalidator id="validatorFecha" runat="server" ErrorMessage="Formato de Fecha Invalido" ControlToValidate="fecha"
					ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:regularexpressionvalidator></td>
		</tr>
		<tr>
			<td>Tipo de Documento :
			</td>
			<td><asp:radiobutton id="rbC" onclick="EstValHid(this)" Text="Cheque" GroupName="docs" value="C" Runat="server"></asp:radiobutton><br>
				<asp:radiobutton id="rbD" onclick="EstValHid(this)" Text="Tarjeta Débito" GroupName="docs" value="D"
					Runat="server"></asp:radiobutton><br>
				<asp:radiobutton id="rbT" onclick="EstValHid(this)" Text="Tarjeta Crédito" GroupName="docs" value="T"
					Runat="server"></asp:radiobutton></td>
			<td>Número :
			</td>
			<td><asp:textbox id="numeroDocumento" onclick="EstabCons(this,GetValHid())" runat="server" ReadOnly="True"
					Width="74px" tooltip="Escoja un tipo de documento"></asp:textbox>
			</td>
			<td></td>
			<td><asp:button id="aceptarDatos" onclick="Aceptar_Datos" runat="server" Text="Aceptar"></asp:button></td>
		</tr>
	</tbody>
</table>
<p></p>
<asp:datagrid id="gridDatos" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False" CellPadding="3" onItemCommand="Manejar_Documentos">
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
	<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternative"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Codigo Recibo de Caja">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CODIGORECIBOCAJA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Número Recibo de Caja">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMERORECIBOCAJA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Tipo de Pago">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "TIPOPAGO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Número del Documento">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Banco">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NOMBREBANCO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Valor">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Nit">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NIT") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Nit Beneficiario">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NITBENEFICIARIO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Fecha">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Remover">
			<ItemTemplate>
				<asp:Button id="remover" runat="server" Text="Remover" CommandName="Remover_Documento" />
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid><input id="hdnChk" type="hidden" runat="server" />

<script type ='text/javascript'>
	function EstabCons(obj,val)
	{
		if (ValidRadio()==false)
			alert('Escoja primero un tipo de documento');
		else
			ModalDialog(obj,'SELECT pban_codigo CONCAT \'-\' CONCAT mcpag_numerodoc AS Numero,mcpag_valor AS Valor FROM dbxschema.mcajapago J,dbxschema.mcaja C,dbxschema.pdocumento P WHERE C.pdoc_codigo=J.pdoc_codigo AND C.mcaj_numero=J.mcaj_numero AND P.pdoc_codigo=C.pdoc_codigo AND J.ttip_codigo=\''+val+'\' AND J.test_estado=\'C\' AND P.tdoc_tipodocu=\'RC\' AND C.test_estadodoc<>\'N\' ORDER BY 1',new Array());
	}
	
	function EstValHid(obj)
	{
		var num=document.getElementById("<%=numeroDocumento.ClientID%>");
		var hdn=document.getElementById("<%=hdnChk.ClientID%>");
		num.title='Haga Click para iniciar la busqueda';
		hdn.value=obj.value;
	}
	
	function GetValHid()
	{
		var hdn=document.getElementById("<%=hdnChk.ClientID%>");
		return hdn.value;
	}
	
	function ValidRadio()
	{
		var rb1=document.getElementById("<%=rbC.ClientID%>");
		var rb2=document.getElementById("<%=rbD.ClientID%>");
		var rb3=document.getElementById("<%=rbT.ClientID%>");
		if(!rb1.checked && !rb2.checked && !rb3.checked)
			return false;
		else
			return true;
	}
</script>
