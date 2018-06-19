<%@ Control Language="c#" CodeBehind="AMS.Finanzas.Tesoreria.EmisionRecibos.Pagos.ascx.cs"
    AutoEventWireup="True" Inherits="AMS.Finanzas.Tesoreria.Pagos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link href="../css/lightbox.css" type="text/css" rel="stylesheet">
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Vehiculos.Tools.js" type="text/javascript"></script>
<script src="../js/lightbox.js" type="text/javascript"></script>
<script type="text/javascript">
    function CambioTipoPago(/*dropdownlist*/obCmbTipPg, /*textbox*/obBanco, /*textbox*/obNumDocu, /*textbox*/obVal, /*textbox*/obValTasCmb, /*textbox*/obValBas, /*textbox*/obFech, /*textbox*/objBtn, /*label*/objlb, /*string*/consignacion, /*textbox*/obChequera,  /*textbox*/obRazon, /*string*/tipoRecibo) {
        //Si es transferencia bancaria, credito directo o chequegridPagos
        obNumDocu.placeholder = '';
        if (obCmbTipPg.value == 'B' || obCmbTipPg.value == 'CD' || obCmbTipPg.value == 'C' || obCmbTipPg.value == 'CC') {
      //      obChequera.disabled = false;
      //      obChequera.value = '';
            obRazon.disabled = false;
            obBanco.disabled = false;
            obNumDocu.disabled = false;
            obVal.disabled = false;
            obNumDocu.removeAttribute("onblur"); 
            Set_Faltante(obVal);
            obValTasCmb.disabled = false;
            obValBas.disabled = true;
            objBtn.disabled = false;
            if (obCmbTipPg.value == 'B')
                objlb.innerHTML = "Cuenta";
            if (obCmbTipPg.value == 'C' || obCmbTipPg.value == 'CC') {
                objlb.innerHTML = "Chequera";
                obFech.disabled = false;
                alert('Ingrese el banco, número de documento, tipo de moneda valor y fecha');
                if (obCmbTipPg.value == 'CC') {
                    obFech.readOnly = true;
                    obChequera.value = '';
                    obBanco.value = '';
                    obNumDocu.value = '';
                    obChequera.disabled = true;
                    obRazon.disabled = true;
                    obVal.readOnly = true;
                }
            }
            else
            {
                if (obCmbTipPg.value == 'B')
                {
                    obFech.disabled = false;
                    alert('Ingrese la cuenta del banco, número de documento, tipo de moneda valor y fecha');
                }
                else
                {
                    obFech.disabled = true;
                    alert('Ingrese el banco,  número de documento, tipo de moneda y valor');
                }
            }
        }
        //Si es pago con tarjeta debito y credito
        else if (obCmbTipPg.value == 'D' || obCmbTipPg.value == 'T') {
                obBanco.disabled = false;
                obNumDocu.disabled = false;
                obVal.disabled = false;
                Set_Faltante(obVal);
                obValTasCmb.disabled = false;
                obValBas.disabled = false;
                if (tipoRecibo == 'CE' && obCmbTipPg.value == 'T')
                    obChequera.disabled = false;
                else obChequera.disabled = true;
                obFech.disabled = false;
                objBtn.disabled = false;
                objlb.innerHTML = "Cuenta";
                alert('Ingrese el banco, número de documento, tipo de moneda valor y base');
                if (consignacion == 'S')
                    CargarDivConsignacion();
        }
        //Si es pago con efectivo,  o credito directo
        else if (obCmbTipPg.value == 'E' || obCmbTipPg.value == 'X') {
                obBanco.disabled = true;
                obNumDocu.disabled = true;
                obChequera.disabled = true;
                obVal.disabled = false;
                Set_Faltante(obVal);
                obValTasCmb.disabled = false;
                obValBas.disabled = true;
                obFech.disabled = true;
                objBtn.disabled = false;
                objlb.innerHTML = 'Cuenta';
                obChequera.value = '';
                obBanco.value = '';
                obNumDocu.value = '';
                alert('Ingrese el tipo de moneda y el valor');
        }
        //Descuento de ley, descuento comercial
        else if (obCmbTipPg.value == 'DL' || obCmbTipPg.value == 'DC') {
                obBanco.disabled = true;
                obNumDocu.disabled = false;
                obChequera.disabled = false;
                obVal.disabled = false;
                Set_Faltante(obVal);
                obValTasCmb.disabled = false;
                obValBas.disabled = true;
                obFech.disabled = true;
                objBtn.disabled = false;
                objlb.innerHTML = 'Cuenta PUC';
                obNumDocu.placeholder = 'Ingrese NIT...';
                alert('Ingrese el tipo de moneda y el valor');
        }
        //Si no han seleccionado pago
        else if (obCmbTipPg.value == 'ESC') {
                obBanco.disabled = true;
                obChequera.disabled = true;
                obNumDocu.disabled = true;
                obVal.disabled = true;
                Set_Faltante(obVal);
                obValTasCmb.disabled = true;
                obValBas.disabled = true;
                obFech.disabled = true;
                objBtn.disabled = true;
                alert('Escoja un tipo de pago');
        }
    }

    function validaLlave(obj)
    {
        obj.value = obj.value.replace(/[-._/]/,'');
        //if (obj.value.indexOf("-") != -1)
        //{
        //    obj.value = obj.value.substring(0, obj.value.length - 1);
        //}
    }

    function Cargar_CuentasPUC(/*textbox*/obj, /*dropdownlist*/controlTiposPagos) {
        if(controlTiposPagos.value == 'DL') { //descuento de ley
            ModalDialog(obj, 'SELECT mcue_codipuc as CODIGO,mcue_nombre as CUENTA FROM dbxschema.mcuenta', new Array());
        }
        else if (controlTiposPagos.value == 'DC') {
            ModalDialog(obj, 'SELECT mcue_codipuc as CODIGO,mcue_nombre as CUENTA FROM dbxschema.mcuenta where timp_codigo = \'A\' or timp_codigo = \'P\' ', new Array());
        }
    }

    function Cargar_NITS(/*textbox*/obj, /*dropdownlist*/controlTiposPagos, /*textbox*/codigoBanco, objetosModificar) {
        if (controlTiposPagos.value == 'DC' || controlTiposPagos.value == 'DL') { //descuento de ley
            ModalDialog(obj, 'SELECT mnit_nit as NIT, mnit_apellidos concat \' \' concat coalesce(mnit_apellido2, \'\') concat \' \' concat mnit_nombres concat \' \' concat coalesce(mnit_nombre2,\'\') as Nombre FROM mnit where tvig_VIGENCIA = \'V\' order by mnit_nit', new Array());
        }
        else if (controlTiposPagos.value == 'CC') {
            ModalDialogPagos(obj, 'SELECT  mcpag_numerodoc as NUMERO, mcpag_valor as VALOR, mcpag_fecha as FECHA FROM PBANCO PB, MCAJAPAGO MCP, PDOCUMENTO PD WHERE PB.PBAN_CODIGO = MCP.PBAN_CODIGO AND MCP.PDOC_CODIGO = PD.PDOC_CODIGO AND PD.TDOC_TIPODOCU = \'RC\' AND MCP.TTIP_CODIGO = \'C\' AND MCP.TEST_ESTADO = \'C\' AND PB.PBAN_CODIGO = ' + codigoBanco.value + ' order by 1', null, objetosModificar);
        }
    }

    function Cargar_Consulta(/*textbox*/obj, /*dropdownlist*/controlTiposPagos, /*string*/tipoRecibo, /*tetxbox*/campoNumero, /*string*/numeros) {
        if (tipoRecibo == 'RC' || tipoRecibo == 'RP') {
            //Si el pago es con tarjeta debito
            if (controlTiposPagos.value == "D")
                ModalDialog(obj, 'SELECT pban_codigo,pban_nombre FROM pbanco WHERE tban_codigo=\'D\'', new Array());
            //Si el pago es con tarjeta de credito
            else if (controlTiposPagos.value == "T")
                    ModalDialog(obj, 'SELECT pban_codigo,pban_nombre FROM pbanco WHERE tban_codigo=\'T\'', new Array());
            //Si el pago es en cheque
            else if (controlTiposPagos.value == "C")
                    ModalDialog(obj, 'SELECT pban_codigo,pban_nombre FROM pbanco WHERE tban_codigo=\'B\'', new Array());
            //Si el pago es con transferencia bancaria
            else if (controlTiposPagos.value == "B")
                    ModalDialogPagos(obj, 'SELECT PCUE.pban_banco AS BANCO,PBAN.pban_nombre AS NOMBRE,PCUE.pcue_numero AS CUENTA,PCUE.pcue_codigo AS CODIGO FROM dbxschema.pcuentacorriente PCUE,dbxschema.pbanco PBAN WHERE PCUE.pban_banco=PBAN.pban_codigo AND PCUE.tvig_vigencia=\'V\'', null, null);
            //Si es algun otro tipo de pago q se me olvido
            else
                ModalDialog(obj, 'SELECT pban_codigo,pban_nombre FROM pbanco', new Array());
        }
        else if (tipoRecibo == 'CE') {
            //Si el pago es en cheque
        if (controlTiposPagos.value == "C")
            ModalDialogPagos(obj, 'SELECT PBAN.pban_codigo AS Banco,PBAN.pban_nombre AS Nombre,PCHE.pcue_codigo AS Cuenta,PCHE.pche_id AS Chequera,CASE WHEN PCHE.pche_ultche IS NULL THEN PCHE.pche_numeini ELSE PCHE.pche_ultche END AS ULTIMO_CHEQUE, PCHE.tres_manejacons AS Consecutivo FROM dbxschema.pbanco PBAN,dbxschema.pchequera PCHE,dbxschema.pcuentacorriente PCUE WHERE PBAN.pban_codigo=PCUE.pban_banco AND PCUE.pcue_codigo=PCHE.pcue_codigo AND PCHE.tvig_vigencia IN(\'V\') ORDER BY PCHE.pche_id', null, null);
        else if (controlTiposPagos.value == "CC") 
                ModalDialogPagos(obj, 'SELECT DISTINCT PB.PBAN_CODIGO as CODIGO, PBAN_NOMBRE as BANCO FROM PBANCO PB, MCAJAPAGO MCP, PDOCUMENTO PD WHERE PB.PBAN_CODIGO = MCP.PBAN_CODIGO AND MCP.PDOC_CODIGO = PD.PDOC_CODIGO AND PD.TDOC_TIPODOCU = \'RC\' AND MCP.TTIP_CODIGO = \'C\' AND MCP.TEST_ESTADO = \'C\' ORDER BY 2', null, null);
        //Si el pago es con transferencia bancaria, Tarjeta Dedito, Tarjeta Credito
        else if (controlTiposPagos.value == "B" || controlTiposPagos.value == "D" || controlTiposPagos.value == "T")
            ModalDialogPagos(obj, 'SELECT PCUE.pban_banco AS BANCO,PBAN.pban_nombre AS NOMBRE,PCUE.pcue_numero CONCAT \' \' concat ttip_descripcion AS CUENTA,PCUE.pcue_codigo AS CODIGO FROM dbxschema.pcuentacorriente PCUE,dbxschema.pbanco PBAN, dbxschema.TTIPOCUENTA tc WHERE PCUE.pban_banco=PBAN.pban_codigo AND PCUE.TVIG_VIGENCIA=\'V\' and pcue.ttip_tipocuenta = tc.ttip_codigo', null, null);
        //Si el pago es con otro tipo de pago
        else
            ModalDialog(obj, 'SELECT pban_codigo,pban_nombre FROM pbanco', new Array());
        }
    }

    function Set_Faltante(/*textbox**/objVal) {
        //Establezco cuales son los objetos de donde voy a sacar los valores
        /*label*/var lbinfo = document.getElementById("<%=lbInfo.ClientID%>");
        /*label*/var lbdocs = document.getElementById("<%=lbDocs.ClientID%>");
        /*label*/var lbconceptos = document.getElementById("<%=lbConceptos.ClientID%>");
        /*textbox*/var tbvalor = objVal;
        /*hidden*/var hdnVal = document.getElementById("<%=hdnval.ClientID%>");
        var valor = 0, valinfo = 0, valdocs = 0, valconceptos = 0, valHdn;
        var vallbinfo = lbinfo.innerHTML;
        var vallbdocs = lbdocs.innerHTML;
        var vallbconc = lbconceptos.innerHTML;
        if (vallbinfo.toString().indexOf("$") != -1)
            vallbinfo = vallbinfo.toString().substring(vallbinfo.toString().indexOf("$") + 1);
        if (vallbdocs.indexOf("$") != -1)
            vallbdocs = vallbdocs.substring(vallbdocs.indexOf("$") + 1);
        if (vallbconc.indexOf("$") != -1)
            vallbconc = vallbconc.substring(vallbconc.indexOf("$") + 1);
        valinfo = parseFloat(EliminarComas(vallbinfo));
        valdocs = parseFloat(EliminarComas(vallbdocs));
        valconceptos = parseFloat(EliminarComas(vallbconc));
        valHdn = parseFloat(EliminarComas(hdnVal.value));
        if (!isNaN(valinfo))
            valor = valor + valinfo;
        if (!isNaN(valdocs))
            valor = valor + valdocs;
        if (!isNaN(valconceptos))
            valor = valor + valconceptos;
        valor = valor - valHdn;
        if (valor >= 0)
            tbvalor.value = valor.toString();
        else
            tbvalor.value = "0";
        ApplyNumericMask(tbvalor);
    }

    <%--function CargarDivConsignacion()
    {
        var arraySels = new Array('<%=ddlCuentas.ClientID%>');
        showLightbox(arraySels);
    }--%>

    function AsignarValorCuenta() {
        var ddlcue = document.getElementById("<%=ddlCuentas.ClientID%>");
        var hdcue = document.getElementById("<%=hdncue.ClientID%>");
        hdcue.value += ddlcue.value + "@";
        hideLightbox();
    }

    function CerrarVentana() {
        hideLightbox();
    }

    //function validarEdicionNum() {

    //    var codBan = document.getElementById("");
    //    Pagos.validarEdicionNum(codBan.value, validarEdicionNum_CallBack);

    //}

    //function validarEdicionNum_CallBack() {

    //    var codBan = document.getElementById("");
    //    Pagos.validarEdicionNum(codBan.value, validarEdicionNum_CallBack);

    //}

</script>
<p>
    <asp:label id="lbInfo" forecolor="Red" text="" runat="server"></asp:label>
</p>
<p>
    <asp:label id="lbDocs" forecolor="Red" runat="server"></asp:label>
</p>
<p>
    <asp:label id="lbConceptos" forecolor="Red" runat="server"></asp:label>
</p>
<asp:panel id="panelPrefijo" runat="server" visible="False">
Escoja el prefijo con el cual se realizará la factura : 
<asp:DropDownList id="ddlPrefijoFactura" class="dmediano" runat="server"></asp:DropDownList></asp:panel>
<asp:panel id="pnlPrefNot" visible="False" runat="server">
Escoja el prefijo con el cual se realizará la nota devolución al cliente : 
<asp:DropDownList id="ddlPrefNot" class="dmediano" Runat="server"></asp:DropDownList></asp:panel>
<asp:panel id="pnlPrefNotPro" visible="False" runat="server">
Escoja el prefijo con el cual se realizará la nota devolución al proveedor : 
<asp:DropDownList id="ddlPrefNotPro" class="dmediano" Runat="server"></asp:DropDownList></asp:panel>
<p>
    Pagos
</p>
<%--/* se le agrego esto que estaba en produccion y se dejo el q funcionaba en AYCO + onselectedindexchanged
   onselectedindexchanged="gridPagos_SelectedIndexChanged">
 */--%>
 <table id="Table1" class="filtersIn">
    <tr>
     <td class="scrollable">
 <fieldset>
 <div id="divflag_eliminar" runat="server" style="visibility:hidden"></div>
<asp:datagrid id="gridPagos" runat="server" cssclass="datagrid" onitemcommand="gridPagos_Item"
    cellpadding="3" autogeneratecolumns="False" showfooter="True" onitemdatabound="gridPagos_ItemDataBound">  
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
	<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Tipo de Pago">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "TIPO") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:DropDownList id="tipoPagoa" width="130px" runat="server" />
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Banco" FooterStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CODIGOBANCO") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:TextBox ID="codbantxt" Runat="server" Width="50" ReadOnly="true" Enabled="false" ToolTip="Haga Click"></asp:TextBox>
				<asp:Label id="lbch" runat="server" text="Chequera" visible="true" />
				<asp:Textbox id="codbantxt3" runat="server" ReadOnly="false" Width="50" Visible="true" />
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="N&#250;mero del Documento" FooterStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMERODOC") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:TextBox ID="codbantxt4" Runat="server" Width="120"  Enabled="false" onkeyup="validaLlave(this)" placeholder="sólo números" textarea maxlength="10"></asp:TextBox>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Tipo de Moneda">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "TIPOMONEDA") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:RadioButton id="monedaNal" runat="server" GroupName="grupoMoneda" Text="Nacional" Checked="true"></asp:RadioButton>
				<asp:RadioButton id="monedaExt" runat="server" GroupName="grupoMoneda" Text="Extranjera"></asp:RadioButton>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Valor" ItemStyle-HorizontalAlign="Right">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALOR","{0:C}") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:TextBox ID="valortxt" Runat="server" Width="100" ReadOnly="false" Enabled="false" onkeyup="NumericMaskE(this,event)"
					cssclass="AlineacionDerecha"></asp:TextBox>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Valor Tasa Cambio" ItemStyle-HorizontalAlign="Right">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALORTC","{0:C}") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:TextBox id="tasaCambiotxt" runat="server" width="80" ReadOnly="false" Text="1" Enabled="false"
					onkeyup="NumericMaskE(this,event)" cssClass="AlineacionDerecha"></asp:TextBox>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Valor Base" ItemStyle-HorizontalAlign="Right">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALORBASE","{0:C}") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:TextBox id="tbvalBase" runat="server" Width="80" Enabled="false" onkeyup="NumericMaskE(this,event)"
					Text="0" cssClass="AlineacionDerecha"></asp:TextBox>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Fecha">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:TextBox ID="fechatxt" Runat="server" Width="90" ReadOnly="false" Enabled="false" onkeyup="DateMask(this)"></asp:TextBox>
			</FooterTemplate>
		</asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Razon">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "RAZON") %>
			</ItemTemplate>
			<FooterTemplate>
				<asp:TextBox ID="razontxt" Runat="server" Width="150" ReadOnly="false" placeholder="Defina detalle..."></asp:TextBox>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Aceptar Pagos" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<asp:Button CommandName="RemoverPagos" Text="Remover" ID="btnRemPag" runat="server" Width="65" />
			</ItemTemplate>
			<FooterTemplate>
				<asp:Button CommandName="AgregarPagos" Text="Agregar" ID="btnAgrPag" runat="server" Width="65"
					Enabled="false" />
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn Visible="False" HeaderText="Agregar Cheques" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<asp:Button CommandName="AgregarCheques" Text="Agregar" ID="btnAddChq" runat="server" Width="65" />
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
</fieldset>
    </td>
    </tr>
</table>
<p>
    <asp:label id="lbRet" runat="server"></asp:label>
</p>
<p>
</p>
<p>
<table id="Table2" class="filtersIn">
    <tr>
     <td class="scrollable">
<fieldset>
    <asp:datagrid id="gridRtns" runat="server" cssclass="datagrid" visible="false" onitemcommand="gridRtns_Item"
        cellpadding="3" autogeneratecolumns="False" showfooter="true">
		<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
		<FooterStyle cssclass="footer"></FooterStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="C&#243;digo de Retenci&#243;n" FooterStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="codret" runat="server" ReadOnly="true" OnClick="ModalDialog(this,'SELECT P.pret_codigo as Codigo,TRET_NOMBRE AS TIPO,pret_porcendecl as Porc_Declarantes,pret_porcennodecl as Porc_No_Declarantes, pret_nombre as Nombre FROM pretencION P, TRETENCION T WHERE P.TRET_CODIGO = T.TRET_CODIGO ORDER BY 2,4;',new Array())"
						Width="70" ToolTip="Haga Click"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Porcentaje de Retenci&#243;n (%)" ItemStyle-HorizontalAlign="Right">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<TABLE>
						<TR>
							<TD>Porcentaje para declarante
							</TD>
							<TD>
								<asp:TextBox id="codretb" runat="server" ReadOnly="true" Width="60"></asp:TextBox></TD>
							<TD>
								<asp:RadioButton id="rbap1" Text="Aplicar" runat="server" GroupName="rbaplicar"></asp:RadioButton></TD>
						</TR>
						<TR>
							<TD>Porcentaje para no declarante
							</TD>
							<TD>
								<asp:TextBox id="codretc" runat="server" ReadOnly="True" Width="60px"></asp:TextBox></TD>
							<TD>
								<asp:RadioButton id="rbap2" Text="Aplicar" runat="server" GroupName="rbaplicar"></asp:RadioButton></TD>
						</TR>
					</TABLE>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor" ItemStyle-HorizontalAlign="Right">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valor" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Agregar" FooterStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Button id="remRet" runat="server" CommandName="RemoverRetencion" Text="Remover" Width="77px"/>
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button id="agRet" runat="server" CommandName="AgregarRetencion" Text="Agregar" Enabled="true" />
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
		<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </fieldset>
        </td>
    </tr>
</table>
</p>
<table style="color: navy; background-color: transparent">
    <tbody>
        <tr>
            <td>
                Valor Bruto :
            </td>
            <td>
                <asp:textbox id="valorBruto" runat="server" cssclass="AlineacionDerecha" readonly="true"></asp:textbox>
            </td>
            <td>
                Valor Neto :
            </td>
            <td>
                <asp:textbox id="valorNeto" runat="server" cssclass="AlineacionDerecha" readonly="true"></asp:textbox>
                &nbsp;<asp:label id="lblFalta" forecolor="Red" text="" runat="server"></asp:label>
            </td>
        </tr>
    </tbody>
</table>
<input id="hdnval" type="hidden" value="0" runat="server">
<input id="hdncue" type="hidden" runat="server">
<div id="overlay" style="display: none; z-index: 90; left: 0px; width: 100%; position: absolute;
    top: 0px">
</div>
<div id="lightbox" style="display: none; z-index: 100; width: 600px; position: absolute;
    height: 100px">
    <fieldset>

    <legend>Información Tarjeta de Crédito</legend>
    <table>
        <tr>
            <td colspan="2">
                Usted ha seleccionado hacer la consignación automática de pagos con tarjeta débito
                y crédito, por favor proporcione la cuenta a la cual se consignará dicho pago
            </td>
        </tr>
        <tr>
            <td align="right">
                Cuenta Corriente :
            </td>
            <td align="left">
                <asp:dropdownlist id="ddlCuentas" runat="server"></asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td align="right">
                <input id="btnAceptarCons" onclick="AsignarValorCuenta()" type="button" value="Aceptar" class="noEspera">
            </td>
            <td align="left">
                <input type="button" id="btnCancelarCons" value="Cancelar" onclick="CerrarVentana()">
            </td>
        </tr>
    </table>
    </fieldset>
</div>
