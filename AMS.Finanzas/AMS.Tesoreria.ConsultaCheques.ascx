<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsultaCheques.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ConsultaCheques" targetSchema="http://schemas.microsoft.com/intellisense/ie5" enableViewState="True"%>
<fieldset>
<p>Que filtro desea usar :
</p>
<table id="Table" class="filtersIn">
	<tr>
		<td><asp:radiobutton id="rbNit" Runat="server" GroupName="rb" Text="Nit"></asp:radiobutton></td>
	</tr>
	<tr>
		<td><asp:radiobutton id="rbCheque" Runat="server" GroupName="rb" Text="Número del Cheque"></asp:radiobutton></td>
	</tr>
	<tr>
		<td><asp:radiobutton id="rbRC" Runat="server" GroupName="rb" Text="Recibo de Caja"></asp:radiobutton></td>
	</tr>
</table>
<P></P>
<table id="tblNit" runat="server" style="DISPLAY: none">
	<tr>
		<td>Seleccione o digite el nit :
		</td>
		<td><asp:textbox id="tbNit" Runat="server" Width="136px" ToolTip="Digite el nit o de doble click para iniciar la busqueda"></asp:textbox></td>
	</tr>
</table>
<table id="tblCheque" runat="server" style="DISPLAY: none">
	<tr>
		<td>Seleccione el banco :
		</td>
		<td><asp:dropdownlist id="ddlBanco" Runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Seleccione o digite el número del cheque :
		</td>
		<td><asp:textbox id="tbCheque" Runat="server" Width="88px" ToolTip="Digite el número del cheque o de doble click para iniciar la busqueda"></asp:textbox></td>
	</tr>
</table>
<table id="tblRC" runat="server" style="DISPLAY: none">
	<tr>
		<td>Prefijo Recibo de Caja :
		</td>
		<td><asp:dropdownlist id="ddlPrefRC" Runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefRC_SelectedIndexChanged"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Número del Recibo de Caja :
		</td>
		<td><asp:dropdownlist id="ddlNumRC" Runat="server"></asp:dropdownlist></td>
	</tr>
</table>
<p><asp:button id="btnBuscar" onclick="btnBuscar_Click" Text="Buscar" runat="server" Enabled="False"></asp:button></p>
<p><asp:datagrid id="dgBusqueda" runat="server" cssclass="datagrid" CellPadding="3" AutoGenerateColumns="False">
		<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
		<FooterStyle cssclass="footer"></FooterStyle>
		<Columns>
			<asp:BoundColumn DataField="NIT" ReadOnly="True" HeaderText="Nit"></asp:BoundColumn>
			<asp:BoundColumn DataField="NOMBRE" ReadOnly="True" HeaderText="Nombre del Beneficiario"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO" ReadOnly="True" HeaderText="Prefijo del RC &#243; CE"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="N&#250;mero del RC &#243; CE">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="BANCO" ReadOnly="True" HeaderText="Banco"></asp:BoundColumn>
			<asp:BoundColumn DataField="CHEQUE" ReadOnly="True" HeaderText="N&#250;mero Cheque"></asp:BoundColumn>
			<asp:BoundColumn DataField="ESTADO" ReadOnly="True" HeaderText="Estado"></asp:BoundColumn>
			<asp:BoundColumn DataField="FECHA" ReadOnly="True" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" ReadOnly="True" HeaderText="Valor" DataFormatString="{0:C}">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DEVOLUCIONES" ReadOnly="True" HeaderText="Devoluciones">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="PRORROGAS" ReadOnly="True" HeaderText="Prorrogas">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
			</asp:BoundColumn>
		</Columns>
		<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
	</asp:datagrid></p>
    </fieldset>
<script type ='text/javascript'>
	function MostrarTabla(idTabla1,idTabla2,idTabla3)
	{
		var tab1=document.getElementById(idTabla1);
		var tab2=document.getElementById(idTabla2);
		var tab3=document.getElementById(idTabla3);
		var rb1=document.getElementById("<%=rbNit.ClientID%>");
		var rb2=document.getElementById("<%=rbCheque.ClientID%>");
		var rb3=document.getElementById("<%=rbRC.ClientID%>");
		var btn=document.getElementById("<%=btnBuscar.ClientID%>");
		if(rb1.checked)
		{
			tab1.style.display='';
			tab2.style.display='none';
			tab3.style.display='none';
			btn.disabled=false;
		}
		else if(rb2.checked)
		{
			tab1.style.display='none';
			tab2.style.display='';
			tab3.style.display='none';
			btn.disabled=false;
		}
		else if(rb3.checked)
		{
			tab1.style.display='none';
			tab2.style.display='none';
			tab3.style.display='';
			btn.disabled=false;
		}
	}
	
	function CargarCheques(sender,obj)
	{
		var objeto=document.getElementById(obj);
		var valor=objeto.value;
		ModalDialog(sender,'SELECT mcpag_numerodoc AS Cheque FROM mcajapago WHERE pban_codigo=\''+valor+'\' ORDER BY mcpag_numerodoc ASC',new Array());
	}
</script>
