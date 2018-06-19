<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Cartera.ConsultaFacturasClientes.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ConsultaFacturasClientes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script type ='text/javascript'>
function CargarFacturasNit()
{
	if(document.getElementById('<%=tbNit.ClientID%>').value == ''){
	    alert('Por favor ingrese un nit');
	    return false;
	}
	document.getElementById('<%=hdNitSelec.ClientID%>').value = document.getElementById('<%=tbNit.ClientID%>').value;

	var existeNit = "";
//	existeNit = AMS_Automotriz_ConsultaFacturasClientes.validarNitConsulta(document.getElementById('<%=tbNit.ClientID%>').value).value;

    if(existeNit == "NO")
        alert('El nit ingresado NO E X I S T E ...!');

	var objPrefijos = AMS_Automotriz_ConsultaFacturasClientes.ConsultarPrefijoFacturasRelacionadasNit(document.getElementById('<%=tbNit.ClientID%>').value).value;
	
	var ddlPrefijos = document.getElementById('<%=ddlPrefDocumen.ClientID%>');
//	ddlPrefijos.options.length = 0;
	ddlPrefijos.options[0] = new Option('Seleccione...','');
	
	var ddlNumeros = document.getElementById('<%=ddlNumedocu.ClientID%>');
//	ddlNumeros.options.length = 0;
	ddlNumeros.options[0] = new Option('Seleccione...','');
	
	if(objPrefijos.Tables[0].Rows.length > 0){
	    alert('Se han cargado las facturas del nit escogido!');
	    for (var i = 0; i < objPrefijos.Tables[0].Rows.length; ++i)
	        ddlPrefijos.options[i+1] = new Option(objPrefijos.Tables[0].Rows[i].NOMBRE_DOC,objPrefijos.Tables[0].Rows[i].PDOC_CODIGO);
	}
	else{
	    alert('El nit ingresado no tiene facturas de cliente relacionadas!');
	}
}

function CargarNumerosFacturaNit()
{
	var objNumeros = AMS_Automotriz_ConsultaFacturasClientes.ConsultarNumerosFacturaRelacionadasPrefijoNit(document.getElementById('<%=hdNitSelec.ClientID%>').value,document.getElementById('<%=ddlPrefDocumen.ClientID%>').value).value;var ddlNumeros = document.getElementById('<%=ddlNumedocu.ClientID%>');ddlNumeros.options.length = 0;ddlNumeros.options[ddlNumeros.options.length] = new Option('Seleccione...','');
	if(objNumeros.Tables[0].Rows.length > 0){
	    for (var i = 0; i < objNumeros.Tables[0].Rows.length; ++i)
	        ddlNumeros.options[i+1] = new Option(objNumeros.Tables[0].Rows[i].NUMERO_FACTURA,objNumeros.Tables[0].Rows[i].NUMERO_FACTURA);
	}
}
</script>
<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>

<fieldset>
<asp:label id="RemeasLabel" runat="server" Font-Bold="True" Font-Size="Medium">Consultar Factura Clientes</asp:label>
&nbsp;
<TABLE id="Table" class="filtersIn">
	<tr>
		<TD><asp:label id="Label10" runat="server" Font-Bold="True" ForeColor="Black">Cédula o Nit</asp:label></TD>
		<TD><asp:textbox id="tbNit" runat="server"></asp:textbox></TD>
		<td><INPUT id="btnCargarFacturas" onclick="CargarFacturasNit();" type="button" value="Cargar Facturas" class="noEspera"></td>
	</tr>
	<TR>
		<TD><asp:label id="Label1" runat="server" Font-Bold="True" ForeColor="Black">Prefijo Documento</asp:label></TD>
		<TD><asp:dropdownlist id="ddlPrefDocumen" runat="server" Width="100%" OnChange="CargarNumerosFacturaNit();"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD><asp:label id="Label2" runat="server" Font-Bold="True" ForeColor="Black">Numero Documento</asp:label></TD>
		<TD><asp:dropdownlist id="ddlNumedocu" runat="server" Width="100%"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD><asp:button id="Buscar" onclick="consultar" runat="server" Text="Buscar"></asp:button><input id="hdNitSelec" runat="server" type="hidden"></TD>
		<TD></TD>
	</TR>
</TABLE>
</fieldset>
<asp:panel id="Panel1" runat="server" Width="848px" Height="280px" Visible="False">

<fieldset>
    
    <TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</TD>
    <asp:PlaceHolder id ="consultasFacturas" runat="server">
    <FIELDSET>
    <LEGEND>Detalles Factura</LEGEND>
	    <TABLE id="Table2" class="filtersIn">
		    <TR>
			    <TD><asp:label id="Label3" runat="server" Font-Bold="True" ForeColor="Black">Documento:</asp:label></TD>
			    <TD><asp:label id="docLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
		        <TD><asp:label id="detalleLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label4" runat="server" Font-Bold="True" ForeColor="Black">Fecha Documento</asp:label></TD>
			    <TD><asp:label id="fechadocLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label16" runat="server" Font-Bold="True" ForeColor="Black" Width="112px">Nit</asp:label></TD>
			    <TD colSpan="3"><asp:label id="nitLabel" runat="server" Font-Bold="True" ForeColor="Red" Width="335px"></asp:label></TD>
			    <TD></TD>
			    <TD></TD>
			    <TD></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label5" runat="server" Font-Bold="True" ForeColor="Black">Dirección</asp:label></TD>
			    <TD><asp:label id="DirLabel" runat="server" ForeColor="Red" Visible="False"></asp:label></TD>
			    <TD>&nbsp;&nbsp;<asp:label id="Label17" runat="server" Font-Bold="True" ForeColor="Black">Telefono</asp:label></TD>
			    <TD><asp:label id="TelLabel" runat="server" ForeColor="Red"></asp:label></TD>
			    <TD><asp:label id="Label18" runat="server" Font-Bold="True" ForeColor="Black">Celular</asp:label></TD>
			    <TD><asp:label id="CeluLabel" runat="server" ForeColor="Red"></asp:label></TD>
		    </TR>
		    <TR>
			    <TD></TD>
			    <TD></TD>
                <TD></TD>
                <TD></TD>
                <TD></TD>
                <TD></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label8" runat="server" Font-Bold="True" ForeColor="Black">Valor Documento</asp:label></TD>
			    <TD align="right"><asp:label id="valodocLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD>&nbsp;&nbsp;<asp:label id="Label6" runat="server" Font-Bold="True" ForeColor="Black">Estado</asp:label></TD>
			    <TD><asp:label id="estadoLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD><asp:label id="Label7" runat="server" Font-Bold="True" ForeColor="Black">Vencimiento:</asp:label></TD>
			    <TD><asp:label id="vencLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label9" runat="server" Font-Bold="True" ForeColor="Black">IVA</asp:label></TD>
			    <TD align="right"><asp:label id="ivaLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD></TD>
			    <TD></TD>
			    <TD><asp:label id="Label12" runat="server" Font-Bold="True" ForeColor="Black">Recibo de Caja</asp:label></TD>
			    <TD><asp:label id="rcajaLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label13" runat="server" Font-Bold="True" ForeColor="Black">Retención</asp:label></TD>
			    <TD align="right"><asp:label id="RetLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD>&nbsp;&nbsp;<asp:label id="Label14" runat="server" Font-Bold="True" ForeColor="Black">Vendedor:</asp:label></TD>
			    <TD><asp:label id="vendLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD><asp:label id="Label15" runat="server" Font-Bold="True" ForeColor="Black">Almacén</asp:label></TD>
			    <TD><asp:label id="almaLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label19" runat="server" Font-Bold="True" ForeColor="Black">Total Factura</asp:label></TD>
			    <TD align="right"><asp:label id="totalLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD>&nbsp;&nbsp;<asp:label id="Label21" runat="server" Font-Bold="True" ForeColor="Black">Usuario</asp:label></TD>
			    <TD><asp:label id="UsuLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD></TD>
			    <TD></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label11" runat="server" Font-Bold="True" ForeColor="Black">Valor Abonado</asp:label></TD>
			    <TD align="right"><asp:label id="valopaLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD></TD>
			    <TD></TD>
			    <TD></TD>
			    <TD></TD>
		    </TR>
		    <TR>
			    <TD><asp:label id="Label20" runat="server" Font-Bold="True" ForeColor="Black">Saldo</asp:label></TD>
			    <TD align="right"><asp:label id="SaldoLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></TD>
			    <TD></TD>
			    <TD></TD>
			    <TD></TD>
			    <TD></TD>
		    </TR>
	    </TABLE>
    </FIELDSET>
    <FIELDSET>
        <LEGEND>Relación de&nbsp; Pagos</LEGEND>
        <asp:panel id="Panel2" runat="server" Width="592px" Height="66px">
        <asp:datagrid id=Grid runat="server" cssclass="datagrid" HorizontalAlign="Center" AutoGenerateColumns="False">
		   	<FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
			<ItemStyle HorizontalAlign="Center"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Doc" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "DOC") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Numero" ItemStyle-HorizontalAlign="center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Fecha" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Estado" ItemStyle-HorizontalAlign="center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
					</ItemTemplate>
				</asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Observaciones" ItemStyle-HorizontalAlign="center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "OBSERVACIONES") %>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid>
	</asp:panel></FIELDSET> 
        </asp:PlaceHolder>
</fieldset>
    </asp:panel>


