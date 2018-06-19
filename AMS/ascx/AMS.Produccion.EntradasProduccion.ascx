<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Produccion.EntradasProduccion.ascx.cs" Inherits="AMS.Produccion.AMS_Produccion_EntradasProduccion" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table style="WIDTH: 600px">
	<tbody>
		<tr>
			<td>
				<fieldset><legend>Datos del proceso:</legend>
					<table class="main">
						<tbody>
							<tr>
								<td colSpan="3">
									<p><asp:label id="Label10" runat="server" forecolor="RoyalBlue">INFORMACIÓN GENERAL</asp:label></p>
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;&nbsp;&nbsp;&nbsp;Proceso:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlTProc" runat="server" OnSelectedIndexChanged="CambiaProceso" AutoPostBack="True">
											<asp:ListItem Value="1" Selected="True">Entradas de Producción Planta Propia</asp:ListItem>
											<asp:ListItem Value="2">Entradas de Producción Externas</asp:ListItem>
										</asp:dropdownlist> 
								</td>
								<td colSpan="2">
									 &nbsp;&nbsp;&nbsp;&nbsp;Almacén:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlAlmacen" runat="server" AutoPostBack="True" onselectedindexchanged="ddlAlmacen_SelectedIndexChanged"></asp:dropdownlist> 
								</td>
							</tr>
							<tr>
								<td>
									 &nbsp;&nbsp;&nbsp;&nbsp;Responsable:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlVendedor" runat="server"></asp:dropdownlist> 
								</td>
							</tr>
						</tbody>
					</table>
					<table class="main">
						<tbody>
							<TR>
								<TD>
									<P><asp:Label id="Label4" runat="server" forecolor="RoyalBlue">ENTRADA ALMACEN </asp:Label></P>
								</TD>
							</TR>
							<TR>
								<TD>
									&nbsp;&nbsp;&nbsp;&nbsp; Prefijo:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:DropDownList id="ddlPrefE" runat="server" AutoPostBack="true" onselectedindexchanged="ddlPrefE_SelectedIndexChanged"></asp:DropDownList>
								</TD>
								<TD>
									&nbsp;&nbsp;&nbsp;&nbsp;Numero:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtNumFacE" runat="server" class="tpequeno" ReadOnLy="false"></asp:TextBox>
									
								</TD>
							</TR>
						</tbody>
					</table>
					<table class="main">
						<tbody>
							<TR>
								<TD>
									<P>
										<asp:Label id="Label1" runat="server" forecolor="RoyalBlue">ORDEN DE PRODUCCIÓN</asp:Label></P>
								</TD>
							</TR>
							<TR>
								<TD>
									 &nbsp;&nbsp;&nbsp;&nbsp;Prefijo:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList id="ddlPrefOrden" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefOrden_SelectedIndexChanged"></asp:DropDownList> 
								</TD>
								<TD>
									 &nbsp;&nbsp;&nbsp;&nbsp;Numero:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList id="ddlNumOrden" runat="server"></asp:DropDownList> 
								</TD>
							</TR>
							<TR>
								<TD>&nbsp;&nbsp;&nbsp;&nbsp; Fecha:<BR>
									&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:TextBox id="tbDate" runat="server" class="tpequeno" onkeyup="DateMask(this)"></asp:TextBox>
								</TD>
								<TD>
									&nbsp;&nbsp;&nbsp;&nbsp;Días Plazo:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtPlazo" runat="server" class="tpequeno" ReadOnLy="false">0</asp:TextBox>
								</TD>
							</TR>
							<tr>
								<td>
									<p><asp:label id="Label5" runat="server" forecolor="RoyalBlue">NIT PROVEEDOR</asp:label></p>
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;&nbsp;&nbsp;&nbsp; NIT:&nbsp;&nbsp;
										<asp:textbox id="txtNIT" ondblclick="ModalDialog(this, 'Select t1.mnit_nit as NIT, t1.mnit_nombres concat \' \' concat t1.mnit_apellidos as Nombre from MNIT as t1,MPROVEEDOR as t2 where t1.mnit_nit=t2.mnit_nit ORDER BY Nombre', new Array());"
											style="name: txtNIT" runat="server" class="tpequeno" ReadOnLy="false"></asp:textbox>
								</td>
								<td>
									<asp:textbox id="txtNITa" style="name: txtNIT" runat="server" class="tpequeno" ReadOnLy="true"></asp:textbox>
								</td>
							</tr>
                            <tr>
                                <td>
                                    <br>
                                    &nbsp;&nbsp;&nbsp;&nbsp; Cerrar Orden: <asp:CheckBox id="chkCerrarO" Text="" runat="server" Checked="False" Visible="true" />
                                </td>
                                <td></td>
                            </tr>
							<tr>
								<td colspan="2">
									<p>Observaciones:
										<asp:textbox id="txtObs" runat="server" class="tgrande" MaxLength="100" Rows="5"></asp:textbox></p>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:button id="btnSeleccionar" onclick="Seleccionar" runat="server" Enabled="True" Text="Seleccionar"></asp:button>
								</td>
							</tr>
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>

			<p><ASP:DATAGRID id="dgrItems" runat="server" CssClass="datagrid" enableViewState="true" OnCancelCommand="dgrItems_Cancel" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="false" OnUpdateCommand="dgrItems_Update" OnEditCommand="dgrItems_Edit">
					<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="CODIGO">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="NOMBRE">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
							</ItemTemplate>
							<EditItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="CANTIDAD">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
							</ItemTemplate>
							<EditItemTemplate>
								<asp:TextBox runat="server" id="txtCantidad" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>' />
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
					</Columns>
				</ASP:DATAGRID>
			</p>

<table style="WIDTH: 600px">
<tbody>
<tr>
<td>
<asp:panel id="pnlDetalle" runat="server" visible="false">
	<FIELDSET><LEGEND>Cierre de orden</LEGEND>
		<TABLE class="main">
			<tr>
				<td>
					Prefijo del Ajuste a Inventario<br>
					<asp:DropDownList id="ddlPrefijoAjuste" class="dmediano" runat="server" OnSelectedIndexChanged="ddlPrefijoAjuste_OnSelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
				</td>
				<td align="right">
					Número del Ajuste a Inventario:<br>
					<asp:Label id="lblNumeroAjuste" Font-Bold="true" runat="server">...</asp:Label>
				</td>
            </tr>
            <tr>
                <td colspan="2">
                    <br>
                    <ASP:DataGrid id="dgItems" runat="server" GridLines="Vertical" AutoGenerateColumns="false" CssClass="datagrid">
			            <HeaderStyle cssclass="header"></HeaderStyle>
		                <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		                <ItemStyle cssclass="items"></ItemStyle>
			            <Columns>
				            <asp:TemplateColumn HeaderText="Código">
					            <ItemTemplate>
						            <%# DataBinder.Eval(Container.DataItem, "mite_codigo", "{0:N}") %>
					            </ItemTemplate>
				            </asp:TemplateColumn>
				            <asp:TemplateColumn HeaderText="Nombre">
					            <ItemTemplate>
						            <%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
					            </ItemTemplate>
				            </asp:TemplateColumn>
				            <asp:TemplateColumn HeaderText="Unidad de Medida">
					            <ItemTemplate>
						            <%# DataBinder.Eval(Container.DataItem, "puni_nombre", "{0:N}") %>
					            </ItemTemplate>
				            </asp:TemplateColumn>
				            <asp:TemplateColumn HeaderText="Cantidad Transferida a Planta">
					            <ItemTemplate>
						            <%# DataBinder.Eval(Container.DataItem, "dite_cantidad", "{0:N}") %>
					            </ItemTemplate>
				            </asp:TemplateColumn>
				            <asp:TemplateColumn HeaderText="Cantidad a Devolver">
					            <ItemTemplate>
						            <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
					            </ItemTemplate>
				            </asp:TemplateColumn>
			            </Columns>
		            </ASP:DataGrid>
                </td>
            </tr>

			<asp:Panel id="pnlObservacion" Runat="server" Visible="false">
			<TR>
				<TD colspan="2">
                    <br>
                    Observaciones:<br>
					<asp:TextBox id="txtObservacion" runat="server" Width="400px" TextMode="MultiLine" Height="72px"></asp:TextBox>
                </TD>
			</TR>
			</asp:Panel>
		</TABLE>
	</FIELDSET>
</asp:panel>
</td>
</tr>
</tbody>
</table>

<asp:PlaceHolder ID="plcTotales" Runat="server">
<TABLE style="WIDTH: 600px">
  <TR>
    <TD>
      <FIELDSET>
      <P></P><LEGEND>Fletes</LEGEND>
      <TABLE class="main" style="WIDTH: 387px; HEIGHT: 56px" border=0>
        <TR>
          <TD>
            <P>Valor 
            Fletes:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; 
            </P></TD>
          <TD>
            <P align="right">
<asp:textbox id=txtFlet onkeyup=NumericMaskE(this,event) runat="server" Width="100px" CssClass="AlineacionDerecha">0</asp:textbox></P></TD></TR>
        <TR>
          <TD>
            <P>%IVA 
            Fletes:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</P></TD>
          <TD>
            <P align="right">
<asp:dropdownlist id="ddlPIVA" runat="server"></asp:dropdownlist></P></TD></TR></TABLE></FIELDSET> 
    </TD></TR>
  <TR>
    <TD>
      <FIELDSET><LEGEND>Totales</LEGEND>
      <TABLE class="main">
        <TR>
          <TD>
            <P>Total Externos: </P></TD>
          <TD>
<asp:textbox id=txtTotalExternos onkeyup=NumericMaskE(this,event) runat="server" CssClass="AlineacionDerecha" ReadOnly="False"></asp:textbox></TD>
          <TD>
            <P>Total Ensamble: </P></TD>
          <TD>
<asp:textbox id=txtSubTotal runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:textbox></TD></TR>
        <TR>
          <TD height=26>
            <P>Descuento: </P></TD>
          <TD height=26>
<asp:textbox id=txtDesc onkeyup=NumericMaskE(this,event) runat="server" CssClass="AlineacionDerecha"></asp:textbox></TD>
          <TD>
            <P>Numero Items: </P></TD>
          <TD>
<asp:textbox id=txtNumItem runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:textbox></TD></TR>
        <TR>
          <TD>
            <P>IVA: </P></TD>
          <TD>
<asp:textbox id=txtIVA runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:textbox></TD>
          <TD height=26>
            <P>Numero Unidades: </P></TD>
          <TD height=26>
<asp:textbox id=txtNumUnid runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:textbox></TD></TR>
        <TR>
          <TD>
            <P>Total: </P></TD>
          <TD>
<asp:textbox id=txtTotal runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:textbox></TD>
          <TD>
            <P>Valor IVA Fletes: </P></TD>
          <TD>
<asp:textbox id=txtTotIF runat="server" CssClass="AlineacionDerecha" ReadOnly="True">0</asp:textbox></TD></TR></TABLE></FIELDSET> 
    </TD></TR></TABLE>
<P>
<center>
<asp:button id=btnAjus onclick=NewAjust runat="server" Text="Realizar Proceso" Enabled="False"></asp:button>
</center>
</P>
<P>
<asp:label id=lbInfo runat="server"></asp:label></P>
<P></P></TR></TBODY></TABLE>

<script type = "text/javascript">
/*
    function CalculoIva(obValFlt, obCmbIVA, obValIVAFlt, obTotRep, obTotal)
    {
        if(obValFlt.value.length > 0)
        {
            NumericMask(obValFlt);
            var stringValorFletes = EliminarComas(obValFlt.value);
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorFletes = parseFloat(stringValorFletes);
            var valorIva = parseFloat(obCmbIVA.value);
            var valorRepuestos = parseFloat(stringValorRepuestos);
            obValIVAFlt.value = String(valorFletes*(valorIva/100));
            ApplyNumericMask(obValIVAFlt);
            obValIVAFlt.value = obValIVAFlt.value;
            document.all[obTotal].value = String(valorRepuestos + (valorFletes + (valorFletes*(valorIva/100))));
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = document.all[obTotal].value;
        }
        else
        {
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorRepuestos = parseFloat(stringValorRepuestos);
            document.all[obTotal].value = String(valorRepuestos);
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = document.all[obTotal].value;
            obValIVAFlt.value = '0';
        }
    }

    function CambioIva(obValFlt, obCmbIVA, obValIVAFlt, obTotRep, obTotal)
    {
        if(obValFlt.value.length > 0)
        {
            var stringValorFletes = EliminarComas(obValFlt.value);
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorFletes = parseFloat(stringValorFletes);
            var valorIva = parseFloat(obCmbIVA.value);
            var valorRepuestos = parseFloat(stringValorRepuestos);
            obValIVAFlt.value = String(valorFletes*(valorIva/100));
            ApplyNumericMask(obValIVAFlt);
            obValIVAFlt.value = obValIVAFlt.value;
            document.all[obTotal].value = valorRepuestos + (valorFletes + (valorFletes*(valorIva/100)));
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = document.all[obTotal].value;
        }
        else
        {
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorRepuestos = parseFloat(stringValorRepuestos);
            document.all[obTotal].value = String(valorRepuestos);
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = document.all[obTotal].value;
            obTotFl.value = '0';
            obValIVAFlt.value = '0';
        }
    }
*/
	var txtTotalExternos=document.getElementById("<%=txtTotalExternos.ClientID%>");
	var txtFlet=document.getElementById("<%=txtFlet.ClientID%>");
	var ddlPIVA=document.getElementById("<%=ddlPIVA.ClientID%>");
	var txtSubTotal=document.getElementById("<%=txtSubTotal.ClientID%>");
	var txtDesc=document.getElementById("<%=txtDesc.ClientID%>");
	var txtIVA=document.getElementById("<%=txtIVA.ClientID%>");
	var txtTotIF=document.getElementById("<%=txtTotIF.ClientID%>");
	var txtTotal=document.getElementById("<%=txtTotal.ClientID%>");
	
	function Totales(){
		var fletes=parseFloat(EliminarComas(txtFlet.value));
		var ivaFletes=parseFloat(EliminarComas(ddlPIVA.value))*fletes/100;
		txtTotIF.value=ivaFletes;
		ApplyNumericMask(txtTotIF);
	}
    function CargaNITREPDIR(ob,obSEL,obEXT,obCmbLin)
    {
        if(document.all["_ctl1:txtNIT"].value.length==0)
        {
            alert("Debe dar un NIT primero!");
            return;
        }
        if(obEXT == null)
        {
            var numPed = obSEL.value;
            var splitNumPed = numPed.split('-');
			if(splitNumPed.length==2)
				ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as "CANTIDAD INGRESADA",DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as "CANTIDAD FACTURADA",DPI.dped_valounit AS "VALOR UNITARIO" FROM mitems MIT, dpedidoitem DPI, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND DPI.pped_codigo=\''+splitNumPed[0]+'\' AND DPI.mped_numepedi='+splitNumPed[1]+' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase','valToInsert10'));
			else
				alert('No hay pedidos para recepcionar');
        }
        else
        {
            if(obEXT.value == '')
            {
                var numPed = obSEL.value;
                var splitNumPed = numPed.split('-');
                if(splitNumPed.length==2)
					ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as "CANTIDAD INGRESADA",DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as "CANTIDAD FACTURADA",DPI.dped_valounit AS "VALOR UNITARIO" FROM mitems MIT, dpedidoitem DPI, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND DPI.pped_codigo=\''+splitNumPed[0]+'\' AND DPI.mped_numepedi='+splitNumPed[1]+' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo = MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase','valToInsert10'));
				else
					alert('No hay pedidos para recepcionar');
            }
            else
            {
				ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE,1 AS "CANTIDAD INGRESADA",1 AS "CANTIDAD FACTURADA",MSAL.msal_ulticost AS "VALOR" FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN,dbxschema.msaldoitem MSAL WHERE MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo AND MSAL.mite_codigo=MIT.mite_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase','valToInsert10'),1);
                //'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM mitems MIT, plineaitem PLIN WHERE MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo'
            }
        }
    }

    function CargaNITPRE(ob,obSEL,obEXT,obCmbLin)
    {
        if(document.all["_ctl1:txtNIT"].value.length==0)
        {
            alert("Debe dar un NIT primero!");
            return;
        }
        if(obEXT == null)
        {
            var numPed = obSEL.value;
            var splitNumPed = numPed.split('-');
            if(splitNumPed.length==2)
				ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD,DPI.dped_valounit AS "VALOR UNITARIO" FROM dbxschema.mitems MIT, dbxschema.dpedidoitem DPI, dbxschema.plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.pped_codigo=\''+splitNumPed[0]+'\' AND DPI.mped_numepedi='+splitNumPed[1]+' AND MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase','valToInsert10'));
			else
				alert('No hay pedidos para precepcionar');
            //'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD FROM mitems MIT, dpedidoitem DPI, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.pped_codigo=\''+splitNumPed[0]+'\' AND DPI.mped_numepedi='+splitNumPed[1]+' AND MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo'
        }
        else
        {
            if(obEXT.value == '')
            {
                var numPed = obSEL.value;
                var splitNumPed = numPed.split('-');
                if(splitNumPed.length==2)
					ModalDialog(ob,'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD,DPI.dped_valounit AS "VALOR UNITARIO" FROM dbxschema.mitems MIT, dbxschema.dpedidoitem DPI, dbxschema.plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.pped_codigo=\''+splitNumPed[0]+'\' AND DPI.mped_numepedi='+splitNumPed[1]+' AND MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase','valToInsert10'));
				else
					alert('No hay pedidos para precepcionar');
                //'SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact as CANTIDAD FROM mitems MIT, dpedidoitem DPI, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.pped_codigo=\''+splitNumPed[0]+'\' AND DPI.mped_numepedi='+splitNumPed[1]+' AND MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo'
            }
            else
                ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE,1 AS CANTIDAD,MSAL.msal_ulticost AS "VALOR" FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN,dbxschema.msaldoitem MSAL WHERE MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo AND MIT.mite_codigo=MSAL.mite_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array('#tbBase','valToInsert10'),1);
                //'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM mitems MIT, plineaitem PLIN WHERE MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo'
        }
    }

    function CargaNITLEG(ob,obSEL,obCmbLin)
    {
        if(document.all["_ctl1:txtNIT"].value.length==0)
        {
            alert("Debe dar un NIT primero!");
            return;
        }
        if(obSEL.length > 0)
        {
            var numPre = obSEL.value;
            var splitNumPre = numPre.split('-');
            ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, DIT.dite_cantidad as "CANTIDAD INGRESADA", DIT.dite_cantidad-DIT.dite_cantdevo as "CANTIDAD A FACTURAR", DIT.dite_valounit as PRECIO, DIT.dite_porcdesc as DESCUENTO, DIT.piva_porciva as IVA FROM mitems MIT, ditems DIT, plineaitem PLIN WHERE MIT.mite_codigo=DIT.mite_codigo AND MIT.plin_codigo=\''+obCmbLin.value+'\' AND DIT.pdoc_codigo=\''+splitNumPre[0]+'\' AND DIT.dite_numedocu='+splitNumPre[1]+' AND (DIT.dite_cantidad-DIT.dite_cantdevo)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,DIT.mite_codigo', new Array('#tbBase','valToInsert10'));
        }
        else
            alert("No se ha seleccionado ninguna prerecepción para legalziar!");
    }

	function CambioPrefijo(obj)
	{
		AMS_Produccion_EntradasProduccion.CargarNumero(obj.value,CargarNumero_CallBack);
	}
	
	function CargarNumero_CallBack(response)
	{
		if (response.error != null)
		{
			alert(response.error);
			return;
		}
		var objNum=document.getElementById("<%=txtNumFacE.ClientID%>");
		objNum.value=response.value;
	}
	Totales();
	</SCRIPT>
</asp:PlaceHolder>
