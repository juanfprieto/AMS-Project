<%@ Control Language="c#" codebehind="AMS.Inventarios.RemisionAlmacenes.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.RemisionAlmacenes" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript">
	function MostrarRefs(obTex,obCmbLin,obCmbAlm,ano)
	{
	//	ModalDialog(obTex,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, CAST(MSIA.msal_cantactual - MSIA.msal_cantasig AS integer) AS "CANTIDAD DISPONIBLE" FROM mitems MIT, msaldoitemalmacen MSIA, plineaitem PLIN WHERE MIT.mite_codigo = MSIA.mite_codigo AND MSIA.palm_almacen = \''+obCmbAlm.value+'\' AND MSIA.pano_ano = '+ano+' AND MIT.plin_codigo = \''+(obCmbLin.value.split('-'))[0]+'\' AND (MSIA.msal_cantactual -MSIA.msal_cantasig)>0 AND MIT.plin_codigo = PLIN.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo', new Array());
		
        var sql = 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, CAST(MSIA.msal_cantactual - MSIA.msal_cantasig AS integer) AS CANTIDAD_DISPONIBLE FROM mitems MIT, msaldoitemalmacen MSIA, plineaitem PLIN WHERE MIT.mite_codigo = MSIA.mite_codigo AND MSIA.palm_almacen = \''+obCmbAlm.value+'\' AND MSIA.pano_ano = '+ano+' AND PLIN.plin_tipo=\''+(obCmbLin.value.split('-'))[1]+'\' AND (MSIA.msal_cantactual -MSIA.msal_cantasig)>0 AND MIT.plin_codigo = PLIN.plin_codigo ORDER BY MIT.plin_codigo,MIT.mite_codigo';
        ModalDialog(obTex, sql, new Array());
    }
    function MostrarRefsBlur(obTex, obCmbLin, obCmbAlm, ano, txtNombre, txtCantidad) {
        //RemisionAlmacenes.MostrarRefsOnBlur(txtCodigo.value, obCmbAlm.value, ano, (obCmbLin.value.split('-'))[1], MostrarRefsBlur_CallBack);

        if (obTex.value != "") {
            var response = RemisionAlmacenes.MostrarRefsOnBlur(obTex.value, obCmbAlm.value, ano, (obCmbLin.value.split('-'))[1]);
            var respuesta = response.value;
            if (respuesta.Tables[0].Rows.length != 0) {
                txtNombre.value = respuesta.Tables[0].Rows[0].NOMBRE;
                txtCantidad.value = respuesta.Tables[0].Rows[0].CANTIDAD_DISPONIBLE;
            }
            else {
                alert("El código ingresado no está definido!");
                txtNombre.value = "";
                txtCantidad.value = "";
            }
        }
        
    }
    
</script>

	<fieldset>
		<p></p>
		<legend>Información Traslado</legend>
		<table id="Table2" class="filtersIn">
			<tbody>
            <tr>
				<tr>
					<td>Prefijo Traslado : &nbsp;&nbsp;<br />
					<asp:dropdownlist id="ddlPrefRemi" OnSelectedIndexChanged="CambioDocumento" AutoPostBack="True" class="dmediano" runat="server"></asp:dropdownlist></td>
				
					<td>Número Traslado :<br />
					<asp:textbox id="tbNumRemi" class="tpequeno" runat="server"></asp:textbox></td>
				</tr>
                <tr>
           		    <td>Vendedor :<br />
					<asp:dropdownlist id="ddlVendedor" class="dmediano" runat="server"></asp:dropdownlist></td>
                    <td>Clave Vendedor :<br />
					<asp:TextBox id="pswVendedor" class="tpequeno" runat="server" TextMode="Password"></asp:TextBox></td>
          	    </tr>
				<tr>
                <td>Observaciones :<br />
                    <asp:TextBox id="txObservaciones" class="amediano" runat="server"  TextMode="MultiLine"></asp:TextBox>
					</td>
					
				</tr>
                    </tr>        
			</tbody></table>
	</fieldset>
			
          
	<fieldset>
		<p></p>
		<legend>Almacén Origen</legend>
		<table id="Table2" class="filtersIn">
			<tbody>
            <tr>
            <td>
				<tr>
					<td>Almacén Origen :
					<asp:dropdownlist id="ddlAlmaOrigen" class="dmediano" runat="server"></asp:dropdownlist></td>
				
					<td>Almacén Destino :
					<asp:dropdownlist id="ddlAlmaDestino" class="dmediano" runat="server"></asp:dropdownlist></td>
				</tr>
                </td>
                </tr>
			</tbody>
            </table>
</fieldset>
		
<p>
<fieldset>

<ASP:DATAGRID id="dgItems" runat="server" OnItemDataBound="DgItemsDataBound" OnDeleteCommand="DgItemsDelete"
		OnCancelCommand="DgItemsCancel" CellPadding="3" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="false" OnItemCommand="DgItemsAddAndDel"
		OnUpdateCommand="DgItemsUpdate" OnEditCommand="DgItemsEdit" onselectedindexchanged="dgItems_SelectedIndexChanged" cssclass="datagrid">
		    <FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
            <PagerStyle mode="NumericPages" cssclass="pager"></PagerStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Codigo:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
				</ItemTemplate> 
				<FooterTemplate>
					<asp:TextBox id="valToInsert1" class="tmediano" runat="server"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1a" ReadOnLy="true" class="tmediano" runat="server"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Linea de Bodega:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "LINEA") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="ddlLinea" class="dmediano" runat="server"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad Disponible:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CANTIDADDISPONIBLE") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1b" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad Traslado:">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CANTIDADTRANSLADO") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" class="tpequeno" id="edit_1" Text='<%# DataBinder.Eval(Container.DataItem, "CANTIDADTRANSLADO") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert2" class="tpequeno" runat="server" text="1"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones">
				<ItemTemplate>
					<asp:Button CommandName="Delete" Text="Quitar" ID="btnDel" class="bpequeno" Runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" class="bpequeno" Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
		</Columns>
	</ASP:DATAGRID></p>


            <table>
            <td align="center">
            <asp:button id="btnAceptar" onclick="RealizarRemision" class="bpequeno" runat="server" Text="Aceptar"></asp:button>&nbsp;
            <asp:button id="btnCancelar" onclick="CancelarRemision" class="bpequeno" runat="server" Text="Cancelar"></asp:button>
            </td>
            </table>
            <p>
            <asp:label id="lb" runat="server"></asp:label>
            </p>
</fieldset>