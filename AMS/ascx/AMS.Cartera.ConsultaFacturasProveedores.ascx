<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Cartera.ConsultaFacturasProveedores.ascx.cs" Inherits="AMS.Finanzas.AMS_Cartera_ConsultaFacturasProveedores" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script type ='text/javascript'>
    var codFlag = "";
function CargarFacturasNit()
{
    var nitCliente = document.getElementById('<%=tbNit.ClientID%>').value;
    if (nitCliente != '' && codFlag != nitCliente) {
        document.getElementById('<%=hdNitSelec.ClientID%>').value = nitCliente;
        var objPrefijos = AMS_Cartera_ConsultaFacturasProveedores.ConsultarPrefijoFacturasRelacionadasNit(nitCliente).value;
        var ddlPrefijos = document.getElementById('<%=ddlPrefDocumen.ClientID%>');
        ddlPrefijos.options.length = 0;
        ddlPrefijos.options[ddlPrefijos.options.length] = new Option('Seleccione...', '');
        var ddlNumeros = document.getElementById('<%=ddlNumedocu.ClientID%>'); ddlNumeros.options.length = 0;
        ddlNumeros.options[ddlNumeros.options.length] = new Option('Seleccione...','');
        if (objPrefijos.Tables[0].Rows.length > 0) {
            //alert('Se han cargado las facturas del nit escogido!');
            for (var i = 0; i < objPrefijos.Tables[0].Rows.length; ++i)
                ddlPrefijos.options[ddlPrefijos.options.length] = new Option(objPrefijos.Tables[0].Rows[i].NOMBRE_DOC, objPrefijos.Tables[0].Rows[i].PDOC_CODIGO);
        } else {
            alert('El nit ingresado no tiene facturas de cliente relacionadas!');
        }
        
        codFlag = nitCliente;
    }
}

function CargarNumerosFacturaNit()
{
    var objNumeros = AMS_Cartera_ConsultaFacturasProveedores.ConsultarNumerosFacturaRelacionadasPrefijoNit(document.getElementById('<%=hdNitSelec.ClientID%>').value, document.getElementById('<%=ddlPrefDocumen.ClientID%>').value).value;
    var ddlNumeros = document.getElementById('<%=ddlNumedocu.ClientID%>');
    ddlNumeros.options.length = 0;
    ddlNumeros.options[ddlNumeros.options.length] = new Option('Seleccione...', '');

    if (objNumeros.Tables[0].Rows.length > 0) {
        for (var i = 0; i < objNumeros.Tables[0].Rows.length; ++i)
            ddlNumeros.options[ddlNumeros.options.length] = new Option(objNumeros.Tables[0].Rows[i].NUMERO_FACTURA, objNumeros.Tables[0].Rows[i].NUMERO_FACTURA);
    }
}

function CargarFacturaProveedor() {
    var nitCliente = document.getElementById('<%=hdNitSelec.ClientID%>').value;
    var prefDocu = document.getElementById('<%=ddlPrefDocumen.ClientID%>').value;
    var codiDocu = document.getElementById('<%=ddlNumedocu.ClientID%>').value;

    var objNumeros = AMS_Cartera_ConsultaFacturasProveedores.CargarFacturaProveedor(nitCliente, prefDocu, codiDocu).value;

    if (objNumeros.Tables[0].Rows.length > 0) {
        var txtPrefProveedor = document.getElementById('<%=txtPrefProv.ClientID%>');
        var txtCodiProveedor = document.getElementById('<%=txtNumeProv.ClientID%>');
        txtPrefProveedor.value = objNumeros.Tables[0].Rows[0].PREF_PROV;
        txtCodiProveedor.value = objNumeros.Tables[0].Rows[0].NUME_PROV;
    }
}

</script>
<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<fieldset>
<asp:label id="RemeasLabel" runat="server" Font-Bold="True" Font-Size="Medium">Consultar Factura Proveedores</asp:label>

&nbsp;
<TABLE id="Table1" class="filtersIn">
	<tr>
		<TD><asp:label id="Label10" runat="server" Font-Bold="True" ForeColor="Black">Cédula o Nit</asp:label></TD>
		<TD><asp:textbox id="tbNit" runat="server" Width="100%" onblur="CargarFacturasNit()"></asp:textbox></TD>
		<td colspan="2">
        <%--<INPUT id="btnCargarFacturas" onclick="CargarFacturasNit()" type="button" class="noEspera" value="Cargar Facturas">--%>
        </td>
	</tr>
	<TR>
		<TD><asp:label id="Label1" runat="server" Font-Bold="True" ForeColor="Black">Prefijo Documento</asp:label></TD>
		<TD><asp:dropdownlist id="ddlPrefDocumen" runat="server" Width="100%" OnChange="CargarNumerosFacturaNit();"></asp:dropdownlist></TD>
	    <TD><asp:label id="Label12" runat="server" Font-Bold="True" ForeColor="Black">Prefijo Proveedor</asp:label></TD>
		<TD><asp:textbox id="txtPrefProv" runat="server" Width="100%"></asp:textbox></TD>
    </TR>
	<TR>
		<TD><asp:label id="Label2" runat="server" Font-Bold="True" ForeColor="Black">Número Documento</asp:label></TD>
		<TD><asp:dropdownlist id="ddlNumedocu" runat="server" Width="100%" OnChange="CargarFacturaProveedor();" ></asp:dropdownlist></TD>
	    <TD><asp:label id="Label14" runat="server" Font-Bold="True" ForeColor="Black">Número Proveedor</asp:label></TD>
		<TD><asp:textbox id="txtNumeProv" runat="server" Width="100%"></asp:textbox></TD>
    </TR>
	<TR>
		<TD><asp:button id="BuscarEntrada" runat="server" class="noEspera" Text="Buscar por Entrada" onclick="Buscar_Click"></asp:button><input id="hdNitSelec" type="hidden" name="hdNitSelec" runat="server"></TD>
		<TD></TD>
        <TD><asp:button id="BuscarProveedor" runat="server" class="noEspera" Text="Buscar por Proveedor" onclick="Buscar_ClickProve"></asp:button><input id="Hidden1" type="hidden" name="hdNitSelec" runat="server"></TD>
		<TD></TD>
	</TR>
</TABLE>
</fieldset>

<P><asp:panel id="Panel1" runat="server" Width="848px" Height="280px" Visible="False"></P>
<fieldset>

    <TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</TD>
    <asp:PlaceHolder id ="consultasFacturas" runat="server">

<FIELDSET><LEGEND>Detalles Factura</LEGEND>
	<TABLE id="Table2" class="filtersIn">
		<TR>
			<TD><asp:label id="Label3" runat="server" Font-Bold="True" ForeColor="Black">Documento:</asp:label></TD>
			<TD colspan="2"><asp:label id="lbDocumento" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD><asp:label id="detalleLabel" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
            </TR>
		<TR>
			<TD><asp:label id="Label4" runat="server" Font-Bold="True" ForeColor="Black">Fecha Documento</asp:label></TD>
			<TD><asp:label id="lbFecha" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label16" runat="server" Font-Bold="True" ForeColor="Black" Width="112px">Nit</asp:label></TD>
			<TD colSpan="3"><asp:label id="lbNitNombre" runat="server" Font-Bold="True" ForeColor="Red" Width="335px">Label</asp:label></TD>
			<TD></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label5" runat="server" Font-Bold="True" ForeColor="Black">Dirección</asp:label></TD>
			<TD><asp:label id="lbDireccion" runat="server" ForeColor="Red" Visible="False">Label</asp:label></TD>
			<TD>&nbsp;&nbsp;<asp:label id="Label17" runat="server" Font-Bold="True" ForeColor="Black">Telefono</asp:label></TD>
			<TD><asp:label id="lbTelefono" runat="server" ForeColor="Red">Label</asp:label></TD>
			<TD><asp:label id="Label18" runat="server" Font-Bold="True" ForeColor="Black">Celular</asp:label></TD>
			<TD><asp:label id="lbCelular" runat="server" ForeColor="Red">Label</asp:label></TD>
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
			<TD align="right"><asp:label id="lbValorDocumento" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD>&nbsp;&nbsp;<asp:label id="Label6" runat="server" Font-Bold="True" ForeColor="Black">Estado</asp:label></TD>
			<TD><asp:label id="lbEstado" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD><asp:label id="Label7" runat="server" Font-Bold="True" ForeColor="Black">Vencimiento:</asp:label></TD>
			<TD><asp:label id="lbVenc" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label9" runat="server" Font-Bold="True" ForeColor="Black">IVA</asp:label></TD>
			<TD align="right"><asp:label id="lbIva" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD></TD>
			<TD></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label13" runat="server" Font-Bold="True" ForeColor="Black">Retención</asp:label></TD>
			<TD align="right"><asp:label id="lbRetencion" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD>&nbsp;&nbsp;</TD>
			<TD></TD>
			<TD><asp:label id="Label15" runat="server" Font-Bold="True" ForeColor="Black">Almacen</asp:label></TD>
			<TD><asp:label id="lbAlmacen" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label19" runat="server" Font-Bold="True" ForeColor="Black">Total Factura</asp:label></TD>
			<TD align="right"><asp:label id="lbTotFact" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD>&nbsp;&nbsp;<asp:label id="Label21" runat="server" Font-Bold="True" ForeColor="Black">Usuario</asp:label></TD>
			<TD><asp:label id="lbUsuario" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label11" runat="server" Font-Bold="True" ForeColor="Black">Valor Abonado</asp:label></TD>
			<TD align="right"><asp:label id="lbValAbon" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD></TD>
			<TD></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label20" runat="server" Font-Bold="True" ForeColor="Black">Saldo</asp:label></TD>
			<TD align="right"><asp:label id="lbSaldo" runat="server" Font-Bold="True" ForeColor="Red">Label</asp:label></TD>
			<TD></TD>
			<TD></TD>
			<TD></TD>
			<TD></TD>
		</TR>
	</TABLE>
</FIELDSET>
<FIELDSET"><LEGEND>Relación de&nbsp; Pagos</LEGEND>
        <asp:panel id="Panel2" runat="server" Width="592px" Height="66px">
		<asp:datagrid id="dgPagos" runat="server" cssclass="datagrid" HorizontalAlign="Center" AutoGenerateColumns="False">
			<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle HorizontalAlign="Center" cssclass="item"></ItemStyle>
			<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
			<FooterStyle cssclass="footer"></FooterStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Doc">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "DOC") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Numero">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem,"VALOR","{0:C}") %>
				i	</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Fecha">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
						<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FECHA")).ToString("yyyy-MM-dd") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Observaciones">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "OBSERVACIONES") %>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
			<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
	</asp:panel></FIELDSET>
         </asp:PlaceHolder>
</fieldset>
</asp:panel>
