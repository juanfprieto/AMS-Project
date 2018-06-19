<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Inventarios.ModificarPedido.ascx.cs" Inherits="AMS.Inventarios.ModificarPedido" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
function ConsultarInfoReferencia(objSender, idDDLLinea, idObjNombre, tipoCliente, idDDLTipoPedido, idObjCantidad, idDDLOTPref, idDDLOTNum, idhdTipPed, idDDLLista, idDDLalmacen, idObjValor, idlbPre, idPrefPed)
	{
		var codigoRef = objSender.value;
		if(codigoRef!="")
		{
			var oItm=objSender.value;
			objSender.value=CrearPedido.ConsultarSustitucion(objSender.value).value
			if(objSender.value!=oItm)
				alert('El item: '+oItm+' ha sido cambiado por su sustitución: '+objSender.value);
			var lineaRef = (document.getElementById(idDDLLinea).value.split('-'))[1];
			var nombreRef = ConsultarNombreRef(codigoRef,lineaRef);
			var almacen=document.getElementById(idDDLalmacen).value;
			var tipoPedido=document.getElementById(idhdTipPed).value;
			var prefijoPedido=document.getElementById(idPrefPed).value;
			if(nombreRef != '')
			{
				document.getElementById(idObjNombre).value = nombreRef;
				if(tipoCliente != "P")
				{
					var listaPrecio=document.getElementById(idDDLLista).value;
					//Se debe revisar el tipo de pedido, si es transferencia se trae la cantidad configurada
					if(tipoPedido=="T")
					{
						var prefOT = document.getElementById(idDDLOTPref).value;
						var numOT = document.getElementById(idDDLOTNum).value;
						document.getElementById(idObjCantidad).value = CrearPedido.ConsultarUsoXVehiculo(prefOT, numOT, codigoRef, lineaRef).value;
					}
					document.getElementById(idObjValor).value=ConsultarPrecioReferencia(codigoRef,lineaRef,tipoCliente,tipoPedido,listaPrecio,almacen,prefijoPedido);
					document.getElementById(idlbPre).innerHTML=AsignarPrecioMinimoReferencia(codigoRef,lineaRef,tipoCliente,tipoPedido,listaPrecio,almacen);
					var hdnvalor=document.getElementById("<%=hdValor.ClientID%>");
					if(document.getElementById(idObjValor).value == "" || document.getElementById(idObjValor).value == "null")
						document.getElementById(idObjValor).value = "0";
					hdnvalor.value=document.getElementById(idObjValor).value;
					if(document.getElementById(idObjValor).value=="0")
						alert('Se ha detectado el valor de 0 en el precio, esto se puede deber a las siguientes causas: \n' 
							+'1. El item '+objSender.value+' no tiene un precio registrado en la lista de precios seleccionada (Clientes)\n'
							+'2. El item aún no tiene movimiento (Clientes-Proveedores)\n'
							+'Le recomendamos revisar su configuración, por favor digite un valor distinto de cero');
				}
				else if(tipoCliente=="P")
				{
					document.getElementById(idObjValor).value=ConsultarPrecioReferencia(codigoRef,lineaRef,tipoCliente,tipoPedido,'',almacen);
					if(document.getElementById(idObjValor).value=="0")
						alert('Se ha detectado el valor de 0 en el precio, esto se puede deber a las siguientes causas: \n' 
							+'1. El item '+objSender.value+' no tiene un precio registrado en la lista de precios seleccionada (Clientes)\n'
							+'2. El item aún no tiene movimiento (Clientes-Proveedores)\n'
							+'Le recomendamos revisar su configuración, por favor digite un valor distinto de cero');
				}
			}
			else
			{
				alert('La referencia '+objSender.value+' no esta registrada');
				document.getElementById(idObjNombre).value = '';
				document.getElementById(idIbjValor).value='';
			}
		}
	}
</script>
<table>
	<tbody>
		<tr>
			<td>
				<p>Cod. Pedido :
					<asp:dropdownlist id="ddlCodigo" AutoPostBack="True" runat="server" onselectedindexchanged="ddlCodigo_SelectedIndexChanged"></asp:dropdownlist></p>
			</td>
			<td>
				<p>Num. Pedido :
					<asp:dropdownlist id="ddlNumero" AutoPostBack="False" runat="server"></asp:dropdownlist></p>
			</td>
			<td align=right>
    			<asp:Button id="btnSeleccionar" runat="server" Text="Seleccionar" OnClick="btnSeleccionar_Click"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
<asp:PlaceHolder ID="plcPedido" runat=server Visible=false>
    <table>
        <tbody>
		    <tr>
		        <td>
				    <p>Tipo Pedido :&nbsp;<asp:label id="lblTipoPedido" runat="server"></asp:label></p>
			    </td>
			    <td colspan="2">
				    <p>NIT :&nbsp;<asp:label id="lblNIT" runat="server"></asp:label>-<asp:label id="lblNITa" runat="server"></asp:label></p>
			    </td>
		    </tr>
		    <tr>
		        <td>
				    <p>Almacen :&nbsp;<asp:label id="lblAlmacen" runat="server"></asp:label>-<asp:label id="lblAlmacena" runat="server"></asp:label></p>
			    </td>
			    <td colspan="2">
				    <p>Fecha :&nbsp;<asp:label id="lblFecha" runat="server"></asp:label></p>
			    </td>
		    </tr>
		    <tr>
			    <td colspan="3">
				    <p>Observaciones :&nbsp;<asp:label id="lblObservacion" runat="server"></asp:label></p>
			    </td>
		    </tr>
	    </tbody>
    </table>
    <p></p>
    <table>
	    <tbody>
		    <tr>
			    <td colSpan="2">
				    <P><ASP:DATAGRID id="dgItems" runat="server" cssclass="datagrid" enableViewState="true" OnItemDataBound="DgInsertsDataBound"
						    OnDeleteCommand="DgInserts_Delete" OnCancelCommand="DgInserts_Cancel" CellPadding="3" ShowFooter="True"
						    GridLines="Vertical" AutoGenerateColumns="false" OnItemCommand="DgInserts_AddAndDel" OnUpdateCommand="DgInserts_Update"
						    OnEditCommand="DgInserts_Edit">
						    <FooterStyle cssclass="footer"></FooterStyle>
						    <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						    <PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
						    <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						    <ItemStyle cssclass="item"></ItemStyle>
						    <Columns>
							    <asp:TemplateColumn HeaderText="Codigo:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_codigo", "{0:N}") %>
								    </ItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox id="valToInsert1" runat="server" Width="180px" ondblclick="ModalDialog(this, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo, PLIN.plin_tipo), MIT.mite_nombre FROM mitems MIT, dbxschema.plineaitem PLIN WHERE MIT.plin_codigo=PLIN.plin_codigo ORDER BY mite_codigo', new Array());"></asp:TextBox>
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Nombre:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
								    </ItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox id="valToInsert1a" ReadOnLy="true" runat="server" Width="80px"></asp:TextBox>
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Linea de Bodega:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "plin_codigo") %>
								    </ItemTemplate>
								    <FooterTemplate>
									    <asp:DropDownList id="ddlListas" runat="server"></asp:DropDownList>
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Cantidad:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_cantidad", "{0:N}") %>
								    </ItemTemplate>
								    <EditItemTemplate>
									    <asp:TextBox runat="server" id="edit_1" Width="60px" CssClass="AlineacionDerecha" Text='<%# DataBinder.Eval(Container.DataItem, "mite_cantidad") %>' />
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox id="valToInsertCant" CssClass="AlineacionDerecha" runat="server" Width="60px" text="1"></asp:TextBox>
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Cant Asig:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_cantasig", "{0:N}") %>
								    </ItemTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Precio Inicial :">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_precioinicial", "{0:C}") %>
								    </ItemTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Precio Final:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_precio", "{0:C}") %>
								    </ItemTemplate>
								    <EditItemTemplate>
									    <asp:TextBox ReadOnly="False" runat="server" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)" id="edit_precio" Width="90px" Text='<%# DataBinder.Eval(Container.DataItem, "mite_precio","{0:N}") %>' />
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="edit_precioc" Runat="server" Text="0" onKeyUp="NumericMaskE(this,event)" CssClass="AlineacionDerecha"
										   class="tepequeno"></asp:TextBox>
									    <DIV id="dvPrc" style="VISIBILITY: hidden">
										    <asp:Label ID="lbPrecMin" Runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
									    </DIV>
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="IVA:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_iva", "{0:N}%") %>
								    </ItemTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Descuento:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_desc", "{0:N}%") %>
								    </ItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="tbfdesc" Runat="server" CssClass="AlineacionDerecha" onKeyUp="NumericMaskE(this,event)"
										    Width="60"></asp:TextBox>
								    </FooterTemplate>
								    <EditItemTemplate>
									    <asp:TextBox runat="server" id="edit_2" Width="60px" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "mite_desc") %>' />
									    %
								    </EditItemTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Total:">
								    <ItemTemplate>
									    <%# DataBinder.Eval(Container.DataItem, "mite_tot", "{0:C}") %>
								    </ItemTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn HeaderText="Operaciones:">
								    <ItemTemplate>
									    <asp:Button CommandName="Delete" Text="Quitar" ID="btnDel" Runat="server" width="80px" />
								    </ItemTemplate>
								    <FooterTemplate>
									    <asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" width="80px" />
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
						    </Columns>
					    </ASP:DATAGRID></P>
			    </td>
		    </tr>
	    </tbody>
    </table>
    <p></p>
    <br>
    <p>
	    <table class="main">
		    <tbody>
			    <tr>
				    <td>
					    <p><asp:label id="Label1" runat="server">Total Inventario:</asp:label></p>
				    </td>
				    <td><asp:textbox id="txtTotal" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
			    </tr>
			    <tr>
				    <td>
					    <p><asp:label id="Label2" runat="server">Numero Items:</asp:label></p>
				    </td>
				    <td><asp:textbox id="txtNumItem" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
			    </tr>
			    <tr>
				    <td>
					    <p><asp:label id="Label3" runat="server">Total Asignado:</asp:label></p>
				    </td>
				    <td><asp:textbox id="txtTotAsig" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
			    </tr>
		    </tbody>
	    </table>
    </p>
    <p><asp:button id="btnAjus" onclick="NewAjust" runat="server" Text="Actualizar Pedido"></asp:button></p>
</asp:PlaceHolder>	    
<p><asp:label id="lbInfo" runat="server"></asp:label></p>
<input id="hdDescCli" type="hidden" runat="server"> <input id="hdTipoPed" type="hidden" runat="server">
<input id="hdValor" type="hidden" runat="server">
