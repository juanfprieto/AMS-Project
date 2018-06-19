<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Produccion.EntradasEnsambles.ascx.cs" Inherits="AMS.Produccion.AMS_Produccion_EntradasEnsambles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
									<p>&nbsp;&nbsp;&nbsp;&nbsp;Almacen:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlAlmacen" runat="server" AutoPostBack="True" onselectedindexchanged="ddlAlmacen_SelectedIndexChanged"></asp:dropdownlist></p>
								</td>
								<td>
									<p>&nbsp;&nbsp;&nbsp;&nbsp;Responsable:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlVendedor" runat="server"></asp:dropdownlist></p>
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
									<P>&nbsp;&nbsp;&nbsp;&nbsp; Prefijo:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:DropDownList id="ddlPrefE" runat="server" onChange="CambioPrefijo(this)" onselectedindexchanged="ddlPrefE_SelectedIndexChanged"></asp:DropDownList></P>
								</TD>
								<TD>
									<P>&nbsp;&nbsp;&nbsp;&nbsp;Numero:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtNumFacE" runat="server" class="tpequeno" ReadOnLy="false"></asp:TextBox>
									</P>
								</TD>
							</TR>
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<table id="Table" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<fieldset><legend>Datos del proceso:</legend>
					<table class="main">
						<tbody>
							<TR>
								<TD colspan="3">
									<P><asp:label id="Label1" forecolor="RoyalBlue" runat="server">ORDEN DE PRODUCCIÓN</asp:label></P>
								</TD>
							</TR>
							<TR>
								<TD>
									<P>&nbsp;&nbsp;&nbsp;&nbsp;Prefijo:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlPrefOrden" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefOrden_SelectedIndexChanged"></asp:dropdownlist></P>
								</TD>
								<TD>
									<P>&nbsp;&nbsp;&nbsp;&nbsp;Numero:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlNumOrden" runat="server"></asp:dropdownlist></P>
								</TD>
								<td>&nbsp;</td>
							</TR>
							<tr>
								<TD>
									<P>&nbsp;&nbsp;&nbsp;&nbsp;Programa de Producción:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlLote" runat="server"></asp:dropdownlist></P>
								</TD>
								<td><P>&nbsp;&nbsp;&nbsp;&nbsp;Año:<BR>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:textbox id="txtAno" Runat="server" class="tpequeno"></asp:textbox></P>
								</td>
								<td>&nbsp;</td>
							</tr>
							<TR>
								<TD>&nbsp;&nbsp;&nbsp;&nbsp; Fecha:<BR>
									&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:TextBox id="tbDate" runat="server" class="tpequeno" onkeyup="DateMask(this)"></asp:TextBox>
								</TD>
								<TD>&nbsp;</TD>
							</TR>
							<tr>
								<td colspan="3">
									<p>Observaciones:
										<asp:textbox id="txtObs" runat="server" class="tgrande" MaxLength="100" Rows="5"></asp:textbox></p>
								</td>
							</tr>
							<TR>
								<TD colSpan="4">
									<asp:datagrid id="dgrColores" runat="server" ShowFooter="True" OnDeleteCommand="DgColoresDelete"
										OnItemCommand="DgColoresAddAndDel" AutoGenerateColumns="False" AlternatingItemStyle-HorizontalAlign="Center"
										ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" OnItemDataBound="DgColoresDataBound">
										<FooterStyle CssClass="footer"></FooterStyle>
						                <HeaderStyle CssClass="header"></HeaderStyle>
						                <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						                <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						                <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						                <ItemStyle CssClass="item"></ItemStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Color">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "PCOL_DESCRIPCION")%>
												</ItemTemplate>
												<FooterTemplate>
													<asp:DropDownList id="ddlColor" runat="server"></asp:DropDownList>
												</FooterTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Cantidad">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "PCOL_CANTIDAD")%>
												</ItemTemplate>
												<FooterTemplate>
													<asp:TextBox id="txtCantidad" runat="server" class="tpequeno" ReadOnLy="false"></asp:TextBox>
												</FooterTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="">
												<ItemTemplate>
													<asp:Button CommandName="Delete" Text="Quitar" ID="btnDelCol" Runat="server" class="bpequeno"/>
												</ItemTemplate>
												<FooterTemplate>
													<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAddCol" Runat="server" class="bpequeno"/>
												</FooterTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:datagrid>
								</TD>
							</TR>
							<tr>
								<td valign="bottom" colspan="4"><asp:button id="btnSeleccionar" onclick="Seleccionar" runat="server" Text="Seleccionar" Enabled="True"></asp:button></td>
							</tr>
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<asp:placeholder id="plcVehiculos" Runat="server" Visible="False">

								<P>
									<ASP:DataGrid id="dgEnsambles" runat="server" OnItemDataBound="DgEnsambleDataBound" AutoGenerateColumns="false"
										ShowFooter="True" CssClass="datagrid" OnEditCommand="DgEnsambleEdit" OnUpdateCommand="DgEnsambleUpdate" GridLines="Vertical" OnCancelCommand="DgEnsambleCancel">
										<FooterStyle CssClass="footer"></FooterStyle>
						                <HeaderStyle CssClass="header"></HeaderStyle>
						                <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						                <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						                <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						                <ItemStyle CssClass="item"></ItemStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Catálogo">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "PCAT_CODIGO") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="VIN">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "MCAT_VIN") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Color">
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "PCOL_DESCRIPCION") %>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:DropDownList id="ddlColorE" runat="server"></asp:DropDownList>
												</EditItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="">
												<ItemTemplate>
													<asp:CheckBox Runat="server" ID="chkUsarE" Checked="True"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Aceptar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
										</Columns>
									</ASP:DataGrid></P>
						
						<TR>
							<TD colSpan="3"><BR>
								<asp:button id="btnEjecutar" onclick="Ejecutar" runat="server" Enabled="True" Text="Ejecutar"></asp:button></TD>
						</TR>
					
</asp:placeholder>
<asp:label id="lbInfo" runat="server" />
<SCRIPT language="javascript">
	
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
		AMS_Produccion_EntradasEnsambles.CargarNumero(obj.value,CargarNumero_CallBack);
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
</SCRIPT>
