<%@ Control Language="c#" codebehind="AMS.Vehiculos.FacturacionFormulario.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.FacturacionFormulario" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<asp:placeholder id="plInfoPed" runat="server">
	<fieldset ><legend class="Legends">Información 
			Pedido</legend>
		<table class="filstersIn">
			<TR>
				<TD>Prefijo Pedido A Proveedor :</TD>
				<TD align="right">
					<asp:Label id="prefPed" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Número Pedido A Proveedor :</TD>
				<TD align="right">
					<asp:Label id="numPed" runat="server"></asp:Label></TD>
			</TR>
		</table>
	</fieldset>
</asp:placeholder>
<p></p>
<P><asp:placeholder id="plInfoVeh" runat="server"></P>

<fieldset>
	<legend class="Legends">
		Información Vehículo</legend>
	<table class="filstersIn">
		<TR>
			<TD>Catálogo del Vehículo :</TD>
			<TD align="right"><asp:label id="catVeh" runat="server"></asp:label></TD>
		</TR>
		<TR>
			<TD>VIN :</TD>
			<TD align="right"><asp:label id="vinVeh" runat="server"></asp:label></TD>
		</TR>
	</table>
</fieldset>
</asp:placeholder>

<P></P>
<fieldset id="fldInfoFactura" runat="server">
	<legend class="Legends">
		Información Factura</legend>
	<asp:checkbox id="chkCst" runat="server" OnCheckedChanged="Generar_Consecutivo" Checked="True"
		TextAlign="Left" AutoPostBack="True" Text="Generar Consecutivo Automatico ?"></asp:checkbox>
	<table class="filstersIn">
		<tbody>
			<tr>
				<td>Prefijo Orden de Pago :
					<asp:dropdownlist id="prefOrdPago" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cambio_Prefijo"></asp:dropdownlist></td>
				<td>Número Orden de Pago :&nbsp;
					<asp:textbox id="numOrdPago" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox><asp:requiredfieldvalidator id="validatorNumOrdPago" runat="server" ControlToValidate="numOrdPago" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorNumOrdPago2" runat="server" ControlToValidate="numOrdPago" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:regularexpressionvalidator></td>
				<td>Nit Proveedor :
					<asp:textbox id="nitProveedor" onclick="ModalDialog(this,'SELECT MPR.mnit_nit AS NIT, MNT.mnit_apellidos CONCAT \' \' CONCAT MNT.mnit_nombres FROM mproveedor MPR, mnit MNT WHERE MPR.mnit_nit = MNT.mnit_nit',new Array())"
						runat="server" class="tpequeno" ReadOnly="True"></asp:textbox><asp:requiredfieldvalidator id="validatorNitProveedor" runat="server" ControlToValidate="nitProveedor" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator></td>
			</tr>
			<tr>
				<td>Prefijo Factura Proveedor :
					<asp:textbox id="prefFacProv" runat="server" class="tpequeno" MaxLength="6"></asp:textbox><asp:requiredfieldvalidator id="validatorPrefFacProv" runat="server" ControlToValidate="prefFacProv" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator></td>
				<td>Número Factura Proveedor :
					<asp:textbox id="numFacProv" runat="server" class="tpequeno" MaxLength="8"></asp:textbox><asp:requiredfieldvalidator id="validatorNumFacProv" runat="server" ControlToValidate="numFacProv" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorNumFacProv2" runat="server" ControlToValidate="numFacProv" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:regularexpressionvalidator></td>
				<td>Fecha de Factura :
					<asp:textbox id="fechFac" onkeyup="DateMask(this)" runat="server" class="tpequeno"
						ReadOnly="False"></asp:textbox><asp:requiredfieldvalidator id="validatorFechFac" runat="server" ControlToValidate="fechFac" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorFechFac2" runat="server" ControlToValidate="fechFac" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator></td>
			</tr>
			<tr>
				<td>Almacén :
					<asp:dropdownlist id="almacen" runat="server"></asp:dropdownlist></td>
				<td>Fecha Ingreso :
					<asp:textbox id="fechIng" onkeyup="DateMask(this)" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorFechIng" runat="server" ControlToValidate="fechIng" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorFechIng2" runat="server" ControlToValidate="fechIng" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator></td>
				<td>Fecha Vencimiento :
					<asp:textbox id="fechVenc" onkeyup="DateMask(this)" runat="server" class="tpequeno"></asp:textbox><asp:requiredfieldvalidator id="validatorFechVenc" runat="server" ControlToValidate="fechVenc" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorFechVenc2" runat="server" ControlToValidate="fechVenc" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator></td>
			</tr>
			<tr>
				<td colSpan="3">Observación :
					<asp:textbox id="obsr" runat="server" class="amediano" TextMode="MultiLine" MaxLength="100"></asp:textbox></td>
			</tr>
		</tbody>
	</table>
</fieldset>
<p></p>
<fieldset id="fldInfoValorFactura" runat="server">
	<legend class="Legends">
		Información Valor Factura</legend>
	<table class="filstersIn">
		<tbody>
			<tr>
				<td width="50%">Valor Neto (Desctos Incluidos):
				</td>
				<td align="right"><asp:requiredfieldvalidator id="validatorValFac" runat="server" ControlToValidate="valFac" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:textbox id="valFac" onkeyup="NumericMaskE(this,event)" runat="server" Width="108px" CssClass="AlineacionDerecha"
						ReadOnly="True"></asp:textbox></td>
			</tr>
			<tr>
				<td>Valor Iva :
				</td>
				<td align="right"><asp:textbox id="tbValIVA" onkeyup="NumericMaskE(this,event)" runat="server" Width="108px" CssClass="AlineacionDerecha"
						ReadOnly="True"></asp:textbox></td>
			</tr>
			<tr>
				<td>Valor Fletes :
				</td>
				<td align="right"><asp:regularexpressionvalidator id="validatorValFlt2" runat="server" ControlToValidate="valFlt" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:regularexpressionvalidator><asp:requiredfieldvalidator id="validatorValFlt" runat="server" ControlToValidate="valFlt" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:textbox id="valFlt" onkeyup="NumericMaskE(this,event)" runat="server" Width="108px" CssClass="AlineacionDerecha">0</asp:textbox></td>
			</tr>
			<tr>
				<td>Porcentaje Iva Fletes :
				</td>
				<td align="right"><asp:dropdownlist id="ddlIvaFlt" runat="server"></asp:dropdownlist></td>
			</tr>
		</tbody>
	</table>
</fieldset>
<fieldset id="fldRets" runat="server" visible="true">
    <asp:PlaceHolder ID="plcRets" Runat="server" ></asp:PlaceHolder>
</fieldset><p>
        <asp:button id="btnAcpt" onclick="Aceptar_Facturacion" runat="server" Text="Facturar" CausesValidation="False" onClientClick="espera();"></asp:button>
        
    </p>

<asp:placeholder id="infVehiculos" runat="server">
	<p>
		<asp:Label id="lbInfo1" runat="server">Información Técnica</asp:Label>
	</p>
	<P>
		<asp:DataGrid id="dgInfoTec" runat="server" cssclass="datagrid" AutoGenerateColumns="true" GridLines="Vertical">
			<HeaderStyle CssClass="header"></HeaderStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
		</asp:DataGrid>
	</P>
	<p>
		<asp:Label id="Label1" runat="server">Información Comercial</asp:Label>
	</p>
	<p>
		<asp:DataGrid id="dgInfoComer" runat="server" cssclass="datagrid" AutoGenerateColumns="true" GridLines="Vertical">
			<HeaderStyle CssClass="header"></HeaderStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
		</asp:DataGrid>
    </p>
	<p>
		<asp:Label id="Label2" runat="server" ForeColor="Red">Estos Vehículos ya fueron adquiridos,revise la siguiente información</asp:Label>
	</p>
	<p>
		<asp:DataGrid id="dgInfoVentas" runat="server" cssclass="datagrid" 
			AutoGenerateColumns="true" GridLines="Vertical"> 
			<HeaderStyle CssClass="header"></HeaderStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
		</asp:DataGrid>
    </p>
</asp:placeholder>
<fieldset id="fldDataGrid" runat="server" visible="false">
    <asp:DataGrid id="dgFacturaExcel" runat="server" GridLines="Vertical" OnCancelCommand="DgInserts_Cancel" OnUpdateCommand="DgInserts_Update" OnEditCommand="DgInserts_Edit"
            cssclass="datagrid" AutoGenerateColumns="false" OnItemCommand="dgExcelDataBound">
			<HeaderStyle CssClass="header"></HeaderStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
            <columns>
			    <asp:TemplateColumn HeaderText="ID">
				    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "ID") %>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="PREFIJO">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>
				    </ItemTemplate>
				    <%--<EditItemTemplate>
                        <asp:DropDownList id="ddlPref" runat="server" class="dmediano" ></asp:DropDownList> 
				    </EditItemTemplate>--%>
			    </asp:TemplateColumn>
			    <%--<asp:TemplateColumn HeaderText="NUMERO">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
				    </ItemTemplate>
				    <EditItemTemplate>
					    <asp:TextBox id="txtNumPref" runat="server" class="tmediano" ></asp:TextBox>
				    </EditItemTemplate>
			    </asp:TemplateColumn>--%>
			    <asp:TemplateColumn HeaderText="PREFIJO FACTURA">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "PREFIJO FACTURA") %>
				    </ItemTemplate>
				    <%--<EditItemTemplate>
					    <asp:TextBox id="txtPrefFact"  runat="server" class="tpequeno" ></asp:TextBox>
				    </EditItemTemplate>--%>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="NUMERO FACTURA">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "NUMERO FACTURA") %>
				    </ItemTemplate>
				    <%--<EditItemTemplate>
					    <asp:TextBox id="txtNumPrefFact"  runat="server" class="tpequeno" ></asp:TextBox>
				    </EditItemTemplate>--%>
			    </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="VIN">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "VIN") %>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="ALMACEN">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "ALMACEN") %>
				    </ItemTemplate>
				    <%--<EditItemTemplate>
					    --<asp:TextBox id="txtAlmacen" runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)"></asp:TextBox>--
                        <asp:DropDownList ID="ddlAlmacen" runat="server" CssClass="dmediano"></asp:DropDownList>
				    </EditItemTemplate>--%>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="FECHA ENTRADA">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "FECHA ENTRADA") %>
				    </ItemTemplate>
				    <%--<EditItemTemplate>
					    <asp:TextBox id="fchEnt" onkeyup="DateMask(this)" runat="server" class="tmediano"></asp:TextBox>
				    </EditItemTemplate>--%>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="FECHA VENC">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "FECHA VENCE") %>
				    </ItemTemplate>
				    <%--<EditItemTemplate>
					    <asp:TextBox id="fchVen" onkeyup="DateMask(this)" runat="server" class="tmediano"></asp:TextBox>
				    </EditItemTemplate>--%>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="VALOR">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:N}") %>
				    </ItemTemplate>
				    <%--<EditItemTemplate>
					    <asp:TextBox id="txtValor" onkeyup="NumericMaskE(this,event)" runat="server" class="tmediano"></asp:TextBox>
				    </EditItemTemplate>--%>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="IVA">
                    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "IVA") %>
				    </ItemTemplate>
				    <EditItemTemplate>
					    <%--<asp:TextBox id="txtIva" runat="server" class="tpequeno"></asp:TextBox>--%>
                        <asp:DropDownList id="ddlIva" runat="server" Width="250px"></asp:DropDownList>
				    </EditItemTemplate>
			    </asp:TemplateColumn>
			    <%--<asp:TemplateColumn HeaderText="RETENCIONES">
				    <ItemTemplate>
                        <asp:TextBox id="txtObs" TextMode="MultiLine" runat="server" width="250px" style="height: 55px;"></asp:TextBox>
                        <asp:PlaceHolder ID="plcRets1" Runat="server" ></asp:PlaceHolder>
				    </ItemTemplate>

			    </asp:TemplateColumn>--%>
                <%--<asp:TemplateColumn HeaderText="Edición:">
				<ItemTemplate>
					<asp:Button id="btnEditar" CommandName="Editar" CausesValidation="False" Runat="server" Text="Editar"></asp:Button>
				</ItemTemplate>
		</asp:TemplateColumn>--%>
                <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
		    </columns>
    </asp:DataGrid><br /><br />
    <p style="font-size: 16px;" >
        <b>Proveedor:</b> <asp:Label ID="txtInfoProovedor" runat="server"></asp:Label><br />
        <b>Ubicación:</b> <asp:Label ID="txtInfoUbicacion" runat="server"></asp:Label><br />
        <b>Valor Total:</b> <asp:Label ID="txtTotalFact" runat="server"></asp:Label><br />
    </p>
        
    <%--Observación: <br />
    <asp:TextBox id="txtObs" TextMode="MultiLine" runat="server" width="350px" style="height: 65px;"></asp:TextBox><br />--%>
    <br />
    <asp:button id="btnAcptExcel" onclick="Aceptar_Facturacion_Excel" runat="server" Text="Facturar Excel" CausesValidation="False" onClientClick="espera();" style="position: relative; margin-left: auto; margin-right: auto; display: -webkit-box;"></asp:button><br />
    <asp:button id="btnVolver" onclick="CargarPrincipal" runat="server" Text="Retornar a la página principal" CausesValidation="False" onClientClick="espera();" Visible="false" style="position: relative; margin-left: auto; margin-right: auto; display: -webkit-box;"></asp:button><br />
    <asp:label id="lb" runat="server"></asp:label> <br />
    <asp:Button id="btnVolverPrincipal" runat="server" Visible="false" OnClick="btn_VolverPrincipal" Text="Regresar"/>
</fieldset>


<script type ="text/javascript">
	function CalcularIva(obj,porcIva)
	{
		var porcReal=parseFloat(porcIva);
		var valor=parseFloat(EliminarComas(obj.value));
		var objIva=document.getElementById("<%=tbValIVA.ClientID%>");
		if(obj.value=='')
		{
			objIva.value="0";
			return;
		}
		NumericMask(obj);
		objIva.value=valor*porcReal/100;
		ApplyNumericMask(objIva);
	}
	
	function RetirarSignoPesos(obj)
	{
		var valor=obj.value;
		if(obj.value.indexOf('$')!=-1)
			valor=valor.substring(1);
		return valor;
	}
	
	function apagarBoton(obj)
	{
		var botonFacturar=document.getElementById("<%=btnAcpt.ClientID%>");
		botonFacturar.enabled=false;
		return;
	}
	
</script>
