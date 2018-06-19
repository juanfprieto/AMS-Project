<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.DevolucionGastoDirectoVehiculos.ascx.cs" Inherits="AMS.Vehiculos.DevolucionGastoDirectoVehiculos" %>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script language='javascript' src='../js/AMS.Tools.js'></script>

<script type ='text/javascript'>
    function abrirEmergente(obj) {
        var nit = document.getElementById('_ctl1_' + obj);
        ModalDialog(nit, 'SELECT mnit_nit as NIT, nombre  as Nombre FROM vmnit', new Array());
    }

    function cargarFech(ob) {
        return DevolucionGastoDirectoVehiculos.ConsultarFecha(ob.value, ConsultarFecha_CallBack);
    }

    function ConsultarFecha_CallBack(ob) {
        if (ob.value != null) {
            $("#<%=fechaVencimiento.ClientID%>").val(ob.value);
        }
    }

    function Cargar_Nombre(obj) {
        DevolucionGastoDirectoVehiculos.Cargar_Nombre(obj.value, Cargar_Nombre_CallBack);
    }

    function Cargar_Nombre_CallBack(response) {
        var respuesta = response.value;
        if (respuesta.Tables[0].Rows.length == 0 || respuesta.Tables[1].Rows.length == 0) {
            var ced = document.getElementById("<%=Nompro.ClientID%>");
            ced.value = '';
        }
        else {
            var nombre = document.getElementById("<%=Nompro.ClientID%>");
            if (respuesta.Tables[1].Rows.length != 0) {
                if (respuesta.Tables[1].Rows[0].NOMBRE != '') {
                    nombre.value = respuesta.Tables[1].Rows[0].NOMBRE;
                }
            }
        }
    }


    </script>
<fieldset>

    <table id="Table1" class="filtersIn">
                    <p>
                        Escoja el tipo de Inclusión en Gastos Directos a Realizar :
                        <asp:DropDownList id="tipoInclusion" class="dmediano" runat="server">
                        <asp:ListItem Value="V" Selected="True">Veh&#237;culos</asp:ListItem>
                        <asp:ListItem Value="E">Embarques</asp:ListItem>
                        </asp:DropDownList>
                    </p>

	<legend>Información Factura</legend>
	<table id="Table2" class="filtersIn">
		<tbody>
			<tr>
				<td>
					Prefijo Devolución de Factura :<br>
					<asp:DropDownList id="prefijoFactura" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Documento"
						runat="server"></asp:DropDownList>
				</td>
				<td>
					Número Devolución :<br>
					<asp:TextBox id="numeroFactura" class="tpequeno" runat="server" ReadOnly="True"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatornumeroFactura" runat="server" ControlToValidate="numeroFactura" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatornumeroFactura2" runat="server" ControlToValidate="numeroFactura" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:RegularExpressionValidator>
				</td>
				<td>
					Nit Proveedor :<br>
                    <asp:TextBox id="nitProveedor" ondblclick="ModalDialog(this,'SELECT mnit_nit as NIT, nombre  as Nombre FROM vmnit', new Array())"  onblur="Cargar_Nombre(this);" OnTextChanged="Cargue_PrefijoDocumento"
                    class="tpequeno" runat="server" AutoPostBack="true"></asp:TextBox>
                    <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png"  onClick="abrirEmergente('nitProveedor')"></asp:Image>
					<asp:RequiredFieldValidator id="validatornitProveedor" runat="server" ControlToValidate="nitProveedor" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
				</td>
                <td>
					Nombre Proveedor :<br>
                    <asp:TextBox id="Nompro" class="tmediano" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
			</tr>
			<tr>
				<td>
					Prefijo Entrada Almacén:<br>
					<asp:DropDownList id="prefFactProveedor" AutoPostBack="true" OnSelectedIndexChanged="Cargue_Documento" runat="server"></asp:DropDownList>
					<asp:RequiredFieldValidator id="validatorprefFactProveedor" runat="server" ControlToValidate="prefFactProveedor"
						Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
				</td>
				<td>
					Número Entrada Almacén:<br>
					<asp:DropDownList id="numeFactProveedor" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="Cargar_Datos"></asp:DropDownList>
					<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="numeFactProveedor"
						Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="numeFactProveedor"
						Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:RegularExpressionValidator>
				</td>
                <td>
					Fecha :<br>
					<asp:TextBox id="fechaFact" class="tpequeno"  runat="server" onblur="cargarFech(this)" onkeyup="DateMask(this)"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorfechaFact" runat="server" ControlToValidate="fechaFact" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorfechaFact2" runat="server" ControlToValidate="fechaFact" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:RegularExpressionValidator>
				</td>
			</tr>
			<tr>
				<td>
					Almacén :<br>
					<asp:DropDownList id="almacen" class="dpequeno" runat="server"></asp:DropDownList>
				</td>
				<td>
					
		Valor Factura :<br>
					<asp:TextBox id="valorFactura" onkeyup="NumericMaskE(this,event)" class="tpequeno" ReadOnly = "true" runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorvalorFactura" runat="server" ControlToValidate="valorFactura" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorvalorFactura2" runat="server" ControlToValidate="valorFactura" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:RegularExpressionValidator>
				</td>
				<td>
			Fecha Venc. :<br>
					<asp:TextBox id="fechaVencimiento" class="tpequeno" onkeyup="DateMask(this)"  runat="server" ></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorfechaVencimiento" runat="server" ControlToValidate="fechaVencimiento"
						Display="Dynamic" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorfechaVencimiento2" runat="server" ControlToValidate="fechaVencimiento"
						Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:RegularExpressionValidator>
				</td>
			</tr>
			<tr>
				<td rowspan="2">
					Observación :<br>
					<asp:TextBox id="observacion" class="amediano" runat="server" TextMode="MultiLine"></asp:TextBox>
				</td>
				<td>
					Valor IVA :<br>
					<asp:TextBox id="txtIVA" onkeyup="NumericMaskE(this,event)" class="tpequeno" ReadOnly = "true" runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txtIVA" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="Regularexpressionvalidator2" runat="server" ControlToValidate="txtIVA" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:RegularExpressionValidator>
				</td>
				<td>
					Valor Retenciones:<br>
					<asp:TextBox id="txtRetenciones" onkeyup="NumericMaskE(this,event)" class="tpequeno"  runat="server" ReadOnly="True">0</asp:TextBox>
					<asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ControlToValidate="txtRetenciones" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="Regularexpressionvalidator4" runat="server" ControlToValidate="txtRetenciones"
						Display="Dynamic" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:RegularExpressionValidator>
				</td>
			</tr>
			<tr>
				<td>
					Valor Total :<br>
					<asp:TextBox id="txtTotal" onkeyup="NumericMaskE(this,event)" class="tpequeno" ReadOnly = "true" runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="txtTotal" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="Regularexpressionvalidator3" runat="server" ControlToValidate="txtTotal" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\-\.]+">*</asp:RegularExpressionValidator>
				</td>
				<td><asp:Button id="btnSeleccionar" onclick="Seleccionar" runat="server" Text="Seleccionar" CausesValidation="False"></asp:Button></td>
			</tr>
		</tbody>
	</table>
</fieldset>
		
<br>
<br>
<asp:Panel ID="pnlDetalles" Runat="server" Visible="False">
	<TABLE class="main" style="WIDTH: 85%; HEIGHT: 46px" border="1">
		<TR>
			<TD class="SubTitulos" align="center">Vehículos Relacionados</TD>
			<TD class="SubTitulos" align="center">Gastos Relacionados</TD>
		</TR>
		<TR>
			<TD>
				<asp:DataGrid id="dgVehiculos" runat="server" cssclass="datagrid" OnEditCommand="gridNC_Edit" OnUpdateCommand="gridNC_Update" OnCancelCommand="gridNC_Cancel"
					OnItemCommand="DgVehiculos_AddAndDel" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="false">
					<FooterStyle CssClass="footer"></FooterStyle>
					<HeaderStyle CssClass="header"></HeaderStyle>
					<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
					<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
					<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Catalogo">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"CATALOGO") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="vehInserts" runat="server" ReadOnly="true"  onclick="ModalDialog(this, 'SELECT MC.pcat_codigo AS CODIGO, MVEH.mcat_vin AS VIN, varchar_format(MFAC.mfac_valofact,\'999999999.00\') AS VALOR_COMPRA, test_nombesta as estado, cast(mveh_fechrece as varchar(10)) as entrada FROM mvehiculo MVEH, MCATALOGOVEHICULO MC, mfacturaproveedor MFAC, testadovehiculo te WHERE MVEH.pdoc_codiordepago = MFAC.pdoc_codiordepago AND MVEH.mfac_numeordepago=MFAC.mfac_numeordepago AND MC.MCAT_VIN = MVEH.MCat_VIN and mveh.test_tipoesta < 60 and mveh.test_tipoesta = te.test_tipoesta',new Array())"
									class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="VIN">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"VIN") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="vehInsertsa" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Valor Vehiculo" >
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"VALOR", "{0:C}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="vehInsertsb" runat="server" ReadOnly="true" class="tpequeno" Enabled="true"></asp:TextBox>
							</FooterTemplate>
                            <EditItemTemplate>
				                <asp:TextBox runat="server" id="valorVehi"  onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "VALOR" , "{0:C}") %>' />
			                </EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Porcentaje">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE") %>
							</ItemTemplate>
                            <FooterTemplate>
								<asp:TextBox id="porcentaje" runat="server" ReadOnly = "true"  class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Valor de Factura">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "VALORFACT" , "{0:C}") %>
							</ItemTemplate>
                            	<FooterTemplate>
								<asp:TextBox id="valorFac" runat="server" ReadOnly = "true" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
                            <EditItemTemplate>
				                <asp:TextBox runat="server" id="valorFaca"  onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "VALORFACT" , "{0:C}") %>' />
			                </EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Operaciones">
							<ItemTemplate>
								<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
							</FooterTemplate>
						</asp:TemplateColumn>
                        <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
					</Columns>
				</asp:DataGrid></TD>
			<TD>
				<asp:DataGrid id="dgGastos" runat="server" cssclass="datagrid" OnItemCommand="DgGastos_AddAndDel" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="false"
					 OnEditCommand="DgGastos_Edit"
					>
					    <FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Código Gasto">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"CODIGO") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="gastInserts" runat="server" ReadOnly="true" class="tpequeno" onclick="ModalDialog(this,'SELECT pgas_codigo AS CODIGO, pgas_nombre AS NOMBRE, pgas_modenaci AS NACIONAL FROM pgastodirecto order by codigo', new Array())"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Nombre Gasto">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"NOMBRE") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="gastInsertsa" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Valor Gasto">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem,"VALOR", "{0:C}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="valorInserts" runat="server" ReadOnly = "true" class="tmediano" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText ="Operaciones">
							<ItemTemplate>
								<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid><BR>
				Tasa de Cambio:&nbsp;
				<asp:TextBox id="txtTasa" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD class="SubTitulos" align="center">Retenciones</TD>
			<TD class="SubTitulos" align="center"></TD>
		</TR>
		<TR>
			<TD vAlign="top" align="center">
				<asp:DataGrid id="gridRtns" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False"
					 showfooter="True" onItemCommand="gridRtns_Item">
					    <FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
                        <asp:TemplateColumn HeaderText="Tipo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
							FooterStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "TIPORETE") %>
							</ItemTemplate>
							<FooterTemplate>
								<center>
									<asp:DropDownList id="ddlTiporet"  class="dmediano" runat="server"></asp:DropDownList>
                                    <%--<asp: id="codret" runat="server" ReadOnly="true" class="tpequeno" ToolTip="Haga Click"     OnSelectedIndexChanged="Cambio_Documento"></asp:TextBox>--%>
								</center>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="C&#243;digo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
							FooterStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "CODRET" ,"{0:N}") %>
							</ItemTemplate>
							<FooterTemplate>
								<center>
									<asp:TextBox id="codret" runat="server" ReadOnly="true" class="tpequeno" ToolTip="Haga Click"></asp:TextBox>
								</center>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Porcentaje de Retenci&#243;n (%)" HeaderStyle-HorizontalAlign="Center"
							FooterStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="codretb" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Base">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox id="base" runat="server" ReadOnly = "true" Enabled="true" Text="0" class="tpequeno"></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Valor">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
							</ItemTemplate>
							<FooterTemplate>
							<asp:TextBox id="valor" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0" class="tpequeno" ReadOnly="true" ></asp:TextBox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Agregar" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Button id="remRet" runat="server" CommandName="RemoverRetencion" Text="Remover" CausesValidation="false" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:Button id="agRet" runat="server" CommandName="AgregarRetencion" Text="Agregar" Enabled="true"
									CausesValidation="false" />
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid></TD>
			<TD vAlign="top">
				<FIELDSET>
					<LEGEND>Totales</LEGEND>
					<TABLE class="main">
						<TR>
							<TD>Vehículos:
							</TD>
							<TD>
								<asp:TextBox id="tlVehiculos" onkeyup="NumericMaskE(this,event)"  runat="server" readonly="True">0</asp:TextBox></TD>
                            <td></td>
                            <td></td>
						</TR>
						<TR>
							<TD>Gastos:
							</TD>
							<TD vAlign="middle">
								<asp:TextBox id="tlGastos"  onkeyup="NumericMaskE(this,event)"  runat="server" readonly="true">0</asp:TextBox></TD>
                            <td><asp:TextBox id="txtGastosVeh" onkeyup="NumericMaskE(this,event)" runat="server" readonly="true">0</asp:TextBox></td>
                            <td>Gastos Vehículos</td>
						</TR>
						<TR>
							<TD>Diferencia Gastos - Total Factura:&nbsp;&nbsp;
							</TD>
							<TD>
								<asp:TextBox id="tlDiferencia" runat="server" readonly="true">0</asp:TextBox></TD>
                            <td><asp:TextBox id="txtDiferenciaVeh" runat="server" readonly="true">0</asp:TextBox></td>
                            <td>Diferencias Gastos - Gastos Vehículos</td>
						</TR>
						<TR>
							<TD></TD>
							<TD align="right">
								<asp:Button id="btnGrabar" onclick="Realizar_Proceso" runat="server" Text="Grabar" onClientclick="espera();"></asp:Button>&nbsp;&nbsp;
								<asp:Button id="btnCancelar" onclick="Cancelar_Proceso" runat="server" CausesValidation="False"
									Text="Cancelar"></asp:Button></TD>
                            <td></td>
                            <td></td>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
	<P></P>
</asp:Panel>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>

<script type ="text/javascript">

    function Cambio_Retencion(objDdlRete, objTxtcodret, tipoSociedad, porcentaje, base, valor) {

        $("#" + objTxtcodret.id).click(function () {

            if (tipoSociedad == "N" || tipoSociedad == "U") {
                ModalDialog(objTxtcodret, 'SELECT PR.PRET_CODIGO CODIGO,PR.PRET_NOMBRE NOMBRE,PR.PRET_PORCENNODECL PORCENTAJE,PR.TTIP_PROCESO PROCESO, ' +
                                  'PR.TRET_CODIGO TIPO, PR.MCUE_CODIPUCPROV CUENTA FROM PRETENCION PR WHERE PR.TRET_CODIGO=\'' + objDdlRete.value + '\' AND PR.TTIP_CODIGO IN (\'N\',\'T\') ' +
                                  ' ORDER BY TIPO;', new Array());
            }
            else {
                ModalDialog(objTxtcodret, 'SELECT PR.PRET_CODIGO CODIGO,PR.PRET_NOMBRE NOMBRE,PR.PRET_PORCENNODECL PORCENTAJE,PR.TTIP_PROCESO PROCESO, ' +
                                  'PR.TRET_CODIGO TIPO, PR.MCUE_CODIPUCPROV CUENTA FROM PRETENCION PR WHERE PR.TRET_CODIGO=\'' + objDdlRete.value + '\' AND PR.TTIP_CODIGO IN (\'J\',\'T\') ' +
                                  ' ORDER BY TIPO;', new Array());
            }
        });


        $("#" + objTxtcodret.id).change(function () {

            PorcentajeVal(porcentaje, base, valor);
        });
    }


    function PorcentajeVal(tPorcentaje, tBase, tTotal) {
        var txtT = document.getElementById(tTotal);
        try {
            var prct = parseFloat(document.getElementById(tPorcentaje).value.replace(/\,/g, ''));
            var bse = parseFloat(document.getElementById(tBase).value.replace(/\,/g, ''));
            var pt = Math.round((prct * bse) / 100);
            txtT.value = formatoValor(pt);
        }
        catch (err) {
            txtT.value = "";
        }
    }

   

</script>

