<%@ Control Language="c#" codebehind="AMS.Inventarios.CrearPedido.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.CrearPedido" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link rel="stylesheet" href="../css/bootstrap.min.css" />

<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript">
	var produc=<%=Convert.ToBoolean(ViewState["PRODUCCION"])?"true":"false"%>;  

	function ConsultasClienteNit(objSender, idObjDesc, idObjNom, idHdDescCli, tipoCliente, idObjCupo, idObjSaldo, idObjSaldoM)
	{
		document.getElementById(idObjNom).value = ConsultarNombre(objSender.value);
        
		if(objSender.value != '' && tipoCliente != 'P')
		{
			ConsultarEstado(objSender.value, idObjCupo,idObjSaldo,idObjSaldoM);
			var descuento;
			if(tipoCliente == 'M')
				descuento = ConsultarDescuentoM(objSender.value);   
			else
				descuento = ConsultarDescuento(objSender.value);
			if(descuento == '')
			{
            //      este mensaje es innecesario
//				if(tipoCliente == 'C')    
//					alert("Este cliente no tiene configurado un descuento, debe agregar manualmente item a item el descuento deseado");
//				else
//					alert("Cliente no registrado");
				document.getElementById(idObjDesc).value = '0';
			}
			else
			{
				document.getElementById(idObjDesc).value = descuento;
				document.getElementById(idHdDescCli).value = descuento;
			}
		}		
	}

    function ConsultarNombre(nitCliente)
	{
		return CrearPedido.ConsultarNombreCliente(nitCliente).value;
	}

    function ConsultarEstado(nitCliente, idObjCupo,idObjSaldo,idObjSaldoM)
    {
		var saldo=CrearPedido.ConsultarSaldoCliente(nitCliente).value;
		var saldoM=CrearPedido.ConsultarSaldoMoraCliente(nitCliente).value;
		var cupo=CrearPedido.ConsultarCupoCliente(nitCliente).value;
		if(saldo)
			document.getElementById(idObjSaldo).value = saldo;
		else
			document.getElementById(idObjSaldo).value = '0';
		if(saldoM)
			document.getElementById(idObjSaldoM).value = saldoM;
		else
			document.getElementById(idObjSaldoM).value = '0';
		if(cupo)
			document.getElementById(idObjCupo).value = cupo;
		else
		    document.getElementById(idObjCupo).value = '0';
	}

    function ConsultarDescuento(nitCliente)
	{
        var nitOrdenTrabajo = "";  
//      en Ordenes de Trabajo (nit taller) debe buscar el % de descuento de la Aseguradora si es seguros, caso contrario el nit del propietario de la orden.
		return CrearPedido.ConsultarDescuentoCliente(nitCliente).value;
	}

	function ConsultarDescuentoM(nitCliente)
	{
		return CrearPedido.ConsultarDescuentoClienteMayor(nitCliente).value;
	}

    function CargaNITP(ob,obCmbLin,ano) //(textbox,dropdownlist,string,string)
    {
        if(document.all["_ctl1:txtNIT"].value.length==0)
        {
            alert("Debe dar un NIT de Proveedor primero!");
            return;
        }
        
        ModalDialog(document.getElementById(ob),'**INVENTARIOS_ITEMSALDO_'+ano+'_'+(document.getElementById(obCmbLin).value.split('-'))[1]+' ', new Array(),1);
    }

    function mostrar()
    {
        if(document.getElementById('divContainerExcelOption').style.display == 'block')
            document.getElementById('divContainerExcelOption').style.display = 'none';
        else 
            document.getElementById('divContainerExcelOption').style.display = 'block';
    }
    
    function CargaNIT(/*textbox*/ob,/*dropdownlist*/obCmbLin,/*string*/ano,/*dropdownlist*/objLstPr)
    {
        if(document.all["<%=txtNIT.ClientID%>"].value.length==0)
        {
            alert("Debe dar un NIT primero..!");
            return;
        }
        if(document.all["<%=ddlPrecios.ClientID%>"].value=="Seleccione..")
        {
            alert("Debe seleccionar una LISTA DE PRECIOS .. primero!");
            return;
        }
        if(document.all["<%=ddlCodigo.ClientID%>"].value=="Seleccione..")
        {
            alert("Debe seleccionar un Tipo de PEDIDO .. primero!");
            return;
        }
        if(document.all["<%=ddlAlmacen.ClientID%>"].value=="Seleccione..")
        {
            alert("Debe seleccionar un ALMACEN .. primero!");
            return;
        }
         
        ModalDialogInventarios(document.getElementById(ob),'**INVENTARIOS_ITEMSCLIENTE_'+ano+'_'+(document.getElementById(obCmbLin).value.split('-'))[1]+'_'+document.getElementById('<%=ddlAlmacen.ClientID%>').value+'_'+document.getElementById(objLstPr).value+' ',new Array(),1);
    }

    //objSender = id del objeto que hace el llamado a la función
	//idDDLLinea = id del objeto que contiene la linea de bodega
	//idObjNombre = id objeto que tiene el nombre
	//tipoCliente = tipo de cliente (C) Cliente (P) Proveedor
	//idDDLTipoPedido = id objeto que trae el tipo de pedido
	//idObjCantidad = id objeto que tiene la cantidad pedida
	//idDDLOTPref = id del objeto que tiene el prefijo de la orden de trabajo
	//idDDLOTNum = id del objeto que tiene el número de la orden de trabajo
	//idhdTipPed = id del hidden que tiene el tipo de pedido
	//idDDLLista = id del objeto que tiene la lista de precios
	//idDDLalmacen = id del objeto que tiene el almacen
	//idObjValor = id del objeto que tiene el valor unitario
	//idlbPre   = id del label que tiene el precio
	//idPrefPed = id del objeto que tiene el prefijo del pedido

    function ConsultarInfoReferencia(objSender, idDDLLinea, idObjNombre, tipoCliente, idDDLTipoPedido, idObjCantidad, idDDLOTPref, idDDLOTNum, idhdTipPed, idDDLLista, idDDLalmacen, idObjValor, idlbPre, idPrefPed, idTipoCargo, idBtnAgregar, autoClick, idlbCantActual)
	{
        //Ajustar codigo
		var codigoRef = objSender.value;
		if(codigoRef!="")
		{
			var lineaRef = (document.getElementById(idDDLLinea).value.split('-'))[1];
		    var oItm=objSender.value;
			objSender.value=CrearPedido.ConsultarSustitucion(objSender.value,lineaRef).value
			if(objSender.value!=oItm)
			{
				alert('El item: '+oItm+' ha sido cambiado por su sustitución: '+objSender.value);
			    oItm=objSender.value;
			    codigoRef = objSender.value;
			}
			var nombreRef = ConsultarNombreRef(codigoRef,lineaRef);
			var almacen=document.getElementById(idDDLalmacen).value;
			var tipoPedido = document.getElementById(idhdTipPed).value;
			
			if(idTipoCargo.valueOf(null))
			{var tipoCargo = 0;
			}
			else{var tipoCargo = document.getElementById(idTipoCargo).value;}
			
			var prefijoPedido=document.getElementById(idPrefPed).value;

			if(nombreRef != "")
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
					document.getElementById(idObjValor).value = ConsultarPrecioReferencia(codigoRef, lineaRef, tipoCliente, tipoPedido, listaPrecio, almacen, prefijoPedido, tipoCargo);
					
                    document.getElementById(idlbPre).innerHTML=AsignarPrecioMinimoReferencia(codigoRef,lineaRef,tipoCliente,tipoPedido,listaPrecio,almacen,"PRECIO_REF");
					document.getElementById(idlbCantActual).innerHTML = AsignarPrecioMinimoReferencia(codigoRef,lineaRef,tipoCliente,tipoPedido,listaPrecio,almacen, "CANT_ACTUAL");

                    var hdnvalor=document.getElementById("<%=hdValor.ClientID%>");
					if(document.getElementById(idObjValor).value == "" || document.getElementById(idObjValor).value == "null")
						document.getElementById(idObjValor).value = "0";
					hdnvalor.value=document.getElementById(idObjValor).value;
					if(document.getElementById(idObjValor).value=="0")
						alert('Se ha detectado el valor de 0 en el precio, esto se puede deber a las siguientes causas: \n' 
							+'1. El item '+objSender.value+' no tiene un precio registrado en la lista de precios seleccionada (Clientes)\n'
							+'2. El item aún no tiene movimiento (Clientes-Proveedores)\n'
							+ 'Le recomendamos revisar su configuración, por favor digite un valor distinto de cero');
		            if (document.getElementById(idObjValor).value == "-1") 
                    {
		                alert('Esta vendiendo por debajo del costo + el factor de utilidad. Digite usted el precio bajo su responsabilidad.');
                    }
                    autoClick = autoClick.toLowerCase();
                    if(autoClick == 'true'){
                        document.getElementById(idBtnAgregar).click();
                        espera();
                    }
				}
				else if(tipoCliente=="P")
				{
				    document.getElementById(idObjValor).value = ConsultarPrecioReferencia(codigoRef, lineaRef, tipoCliente, tipoPedido, '', almacen,prefijoPedido, tipoCargo);
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
				document.getElementById(idObjValor).value='';
			}
            
		}
	}

    function ConsultarNombreRef(codRef, codLinea)
	{
		return CrearPedido.ConsultarNombreItem(codRef,codLinea).value;
	}
    
    function ConsultarPrecioReferencia(codRef,codLin,tipCli,tipPed,lista,almacen,pedido,tipoCargo)
	{
        return CrearPedido.ConsultarPrecioItem(codRef,codLin,tipCli,tipPed,lista,almacen,pedido,produc,tipoCargo).value;
	}

    function AsignarPrecioMinimoReferencia(codRef,codLin,tipCli,tipPed,lista,almacen, accion)
	{
        return CrearPedido.AsignarPrecioMinimoReferencia(codRef,codLin,tipCli,tipPed,lista,almacen,accion).value;
	}

    
    function CargaNIT2(ob,obCmbLin)
    {
        if(document.all["_ctl1:txtNIT"].value.length==0)
        {
            document.getElementById(ob).value = "";
            alert("Debe dar un NIT primero!");
            return;
        }
        
        //ItemMask(ob,obCmbLin);
    }

    function validarCitas() 
    {
//        var strCitas = <%=ViewState["citasConKitsProgramados"]%>;

//        if(strCitas.length > 0)
//            return mostrarConfirmacion(mensaje);

        return true;
    }

    function abrirEmergente()
    {
        var tipoPedido = document.getElementById('<%=lblTipoPedido.ClientID%>');
        var nit = document.getElementById('<%=txtNIT.ClientID%>');
        if(tipoPedido.textContent == "Transferencia")
        {
            ModalDialog(nit, '**NITS_TALLER', new Array(), 1);
        }
        else if(tipoPedido.textContent == "Interno")
        {
            ModalDialog(nit, '**NITS_INTERNO', new Array(), 1);
        }
        else 
        {
            ModalDialog(nit, '**NITS_CLIENTE', new Array(), 1);
        }
    }
</script>


<FIELDSET>
    <legend class=Legends>Datos del Pedido</legend>
     <table id="Table1" class="filtersIn">
		     
			    <td>
				    Pedido :<br>
					    <asp:dropdownlist id="ddlCodigo" AutoPostBack="True" runat="server" onselectedindexchanged="ddlCodigo_SelectedIndexChanged"></asp:dropdownlist>
			    </td>
			    <td>
				    Tipo Pedido :<br>
                    <b><asp:label id="lblTipoPedido" runat="server"></asp:label></b>
			    </td>
			    <td>
				    Numero Pedido :<br>
					<asp:textbox id="txtNumPed" runat="server" class="tpequeno"></asp:textbox>
			    </td>
                <td>
                    <asp:Label id="lbCotiPendt" runat="server" Visible="false" >
                    <strong><i>Cotizaciones Pendientes</i></strong></asp:Label> <br />
                    <asp:DropDownList ID="ddlCotiPendt" AutoPostBack="true" runat="server" Visible="false" OnSelectedIndexChanged="cargarItemsCotizacion"></asp:DropDownList>
                </td>
		      
		    <tr>
			    <td>
				    Almacén :<br>
					<asp:dropdownlist id="ddlAlmacen" AutoPostBack="True" runat="server" OnSelectedIndexChanged="CambioAlmacen"></asp:dropdownlist>
			    </td>
 			    <td>
				    Nit : <br>
					<asp:textbox id="txtNIT" ondblclick="abrirEmergente();" runat="server" class="tpequeno" Enabled="False" ReadOnLy="false" ontextchanged="cargarListasPrecios" autopostback="true"></asp:textbox>
                    <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente();"></asp:Image>
                     <br>
    			</td>
                <td>
                    <asp:Label id="Labelkits" Visible="False" runat="server" >
                    <strong><i>Kits o Combos</i></strong></asp:Label> <br />
                    <asp:DropDownList id="ddlKits" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="cargarKits" ></asp:DropDownList>
                </td>
                <td>
                    <asp:Label id="LbPediPendt" Visible="false" runat="server" >
                    <strong><i>Pedidos Pendientes</i></strong></asp:Label> <br />
                    <asp:DropDownList id="ddlPediPendt" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="cargarItemsPedido" ></asp:DropDownList>
                </td>
            </tr>
            <asp:panel id="bloque" runat="server" visible="false">
            <tr>
			    <td>
				    <asp:label id="lblTipoOrden" runat="server">Tipo de Orden :</asp:label>
                    <br><asp:dropdownlist id="ddlTipoOrden" AutoPostBack="True" runat="server" onselectedindexchanged="ddlTipoOrden_SelectedIndexChanged"></asp:dropdownlist>
			    </td>
			    <td >
				    <asp:label id="lblNumOrden" runat="server">Numero de Orden :</asp:label>
                    <br><asp:dropdownlist id="ddlNumOrden" runat="server" OnSelectedIndexChanged="cambioOT" AutoPostBack="true" ></asp:dropdownlist>
			    </td>
			    <td >
                    <asp:placeholder id="plCargoPlaca" runat="server">
					    <div <%=Convert.ToBoolean(ViewState["PRODUCCION"])?"style='VISIBILITY: hidden;'":""%>>
						    <asp:label id="lblCargo" runat="server">Cargo :</asp:label><br>
						    <asp:dropdownlist id="ddlCargo" runat="server"></asp:dropdownlist><br>
						    <asp:label id="lblPlaca" runat="server">Placa y Nombre</asp:label><br>
                        </div>
				    </asp:placeholder>
                </td>
		    </tr>
            </asp:panel> 
		    <tr>
			<td>
               <asp:placeholder id="plListaPrecios" runat="server">
		    	    <div <%=Convert.ToBoolean(ViewState["PRODUCCION"])?"style='VISIBILITY: visible;'":""%>>
					        Lista de Precios :<br>
                            <asp:DropDownList id="ddlPrecios" runat="server"></asp:DropDownList>
			        </div>
               </asp:placeholder>
		    </td>
                <td>
                <asp:Button id="btnConsultaCoti" Visible="false" OnClick="consultarCotizaciones" runat="server" Text="Cargar pedidos/Cotizaciones/Kits" />
                <asp:textbox id="txtNITa"  runat="server" class="tmediano" ReadOnLy="true"></asp:textbox>
                </td>
                <td>
                    Fecha :
				    <br><asp:textbox id="tbDate" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox><IMG onmouseover="calendar.style.visibility='visible'" src="../img/AMS.Icon.Calendar.gif"
					    border="0">
                </td>      
			    <td>
                    Observaciones :<br>
                    <asp:textbox id="txtObs" height="40px" MaxLength="100" TextMode="multiLine" runat="server"></asp:textbox>
			    </td>
		    </tr>
                <td></td>
                <td>
				    <table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
					    onmouseout="calendar.style.visibility='hidden'">
					    <tbody>
						    <tr>
							    <td><asp:calendar BackColor="Beige" id="calDate" runat="server" OnSelectionChanged="ChangeDate" enableViewState="true"></asp:calendar></td>
						    </tr>
					    </tbody>
				    </table>
			    </td>
                <td></td>
		    </tr>
			<tr>
                <asp:placeholder id="plcEstadoCliente" runat="server">
				    <td>
                        Cupo:<br>
					    <asp:textbox id="txtCupo" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox>&nbsp;&nbsp; 
					</td>
                    <td>
                        Saldo Cartera:<br>
					    <asp:textbox id="txtSaldoCartera" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox>&nbsp;&nbsp; 
					</td>
                    <td>
                        Saldo Cartera Mora:<br>
					    <asp:textbox id="txtSaldoMoraCartera" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox>
                    </td>
                    <td>
                       <%-- % Descuento Orbita:<br>
					    <asp:textbox id="TxtPorcDesc" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox>--%>
                    </td>
                </asp:placeholder>
			</tr>
    </table>
</FIELDSET>

<p></p>
    <div hidden>
            <asp:radiobuttonlist id="rdbCalSinIVA" CellSpacing="6" RepeatDirection="Horizontal" BorderStyle="None" runat="server">
			    <asp:ListItem Value="A" Selected="False">Calcular Valor Sin IVA</asp:ListItem>
		    </asp:radiobuttonlist>
    </div>
			<ASP:DATAGRID id="dgItems" runat="server" enableViewState="true" OnItemDataBound="DgInsertsDataBound"
						OnDeleteCommand="DgInserts_Delete" OnCancelCommand="DgInserts_Cancel" HeaderStyle-BackColor="#ccccdd"
						Font-Size="8pt" Font-Name="Verdana" CellPadding="3" ShowFooter="True" BorderColor="#999999"
						BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana"
						AutoGenerateColumns="false" OnItemCommand="DgInserts_AddAndDel" OnUpdateCommand="DgInserts_Update"
						OnEditCommand="DgInserts_Edit" style="table-layout=auto;">
						<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Código:">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "mite_codigo", "{0:N}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="valToInsert1" class="tpequeno" runat="server"></asp:TextBox>
                                    <asp:Image id="imglupa2" runat="server" ImageUrl="../img/AMS.Search.png" ></asp:Image>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Nombre:" >
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="valToInsert1a" Width= "270px" ReadOnLy="true" TextMode="MultiLine" runat="server"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Línea/Bodega:">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "plin_codigo") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList OnSelectedIndexChanged="ddlListas_SelectedIndexChanged" id="ddlListas" class="dmediano" runat="server"></asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cantidad:">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "mite_cantidad", "{0:N}") %>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox runat="server" id="edit_1"  CssClass="AlineacionDerecha" Text='<%# DataBinder.Eval(Container.DataItem, "mite_cantidad") %>' />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="valToInsertCant" CssClass="AlineacionDerecha"  Width="60px" runat="server"  text="1"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cant Asig:">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "mite_cantasig", "{0:N}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Precio Inicial :">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "mite_precioinicial", "{0:C2}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Precio Promedio :">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "mite_preciopromedio", "{0:C2}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Precio Final:">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "mite_precio", "{0:C2}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="edit_precioc" Runat="server" Text="0" onKeyUp="NumericMaskE(this,event)" CssClass="AlineacionDerecha"
										Width="90"></asp:TextBox>
									<div id="dvPrc" style="display: none">
										<asp:Label ID="lbPrecMin" Runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
									</div>
                                    <div id="dvCantActual" >
										<asp:Label ID="lbCantActual" Runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
									</div>
								</FooterTemplate>
								<EditItemTemplate>
									<asp:TextBox ReadOnly="False" runat="server" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)" id="edit_precio" Text='<%# DataBinder.Eval(Container.DataItem, "mite_precio","{0:N}") %>' />
								</EditItemTemplate>
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
									<asp:TextBox runat="server" id="edit_2"  CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "mite_desc") %>' />
									
								</EditItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Total:">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "mite_tot", "{0:C}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Acciones:">
								<ItemTemplate>
									<asp:Button CommandName="Delete" Text="Quitar" ID="btnDel" Runat="server"  />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" width="70px" />
									<asp:Button CommandName="ClearRows" Text="Reiniciar" ID="btnClear" Runat="server" width="70px" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
						</Columns>
				</ASP:DATAGRID>
                <br />
        <fieldset id="fldExcel" runat="server" visible="true" style="padding: 10px;">
        <span style="CURSOR: pointer; COLOR: darkblue; TEXT-DECORATION: underline; font-size:large;font-style:italic;font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;" onclick="mostrar();"><b>Mostrar Herramienta Carga Excel</b></span>
        <div id="divContainerExcelOption" style="DISPLAY: none">
        <legend>Opción de Carga desde Archivo Excel</legend>
		    <table class="tablewhite" cellSpacing="1" cellPadding="1" border="0">
			    <tr>
			    <tr>
				    <td align="right" colSpan="2"><span style="CURSOR: pointer; COLOR: royalblue; TEXT-DECORATION: none" onclick="document.getElementById('divContainerExcelOption').style.display='none';">Ocultar</span></td>
			    <tr>
				    <td colSpan="2">Por favor genere un archivo de excel con las siguientes columnas<br>
					    <li>
					        CODIGO
                        <LI>
                            LINEA
                        <lI>
                            CANTIDAD
                        <lI>
                            PRECIO_INICIAL
                        <lI>
                            DESCUENTO
						    <br>
					    Notas :
					    <li>
					        El primer renglón del archivo debe llevar los titulos de las columnas como se 
					        encuentran listados anteriormente.
					    <LI>
					        Ningún campo puede estar vacio y los campos numéricos no deben llevar separadores de miles ni signo de peso. (sólo número ejemplo: 300)
                        <LI>
                            Use punto(.) para valores decimales(sólo válido para el campo de Precio)
					    <LI>
					        Debe seleccionar todo el espacio de la tabla  y asignarle el nombre 
					        ITEM.
                        <LI>
                            Seleccione toda la tabla y cambie el formato a solo texto.
					    <LI>
					        Solo utilizar Excel formato xlsx (.xlsx 2010 o superior)
					    <LI style="font-size:15px; color:firebrick; text-decoration-color:orangered">
					        EL CODIGO del item debe existir en la base de datos.
					    </LI>
                    </td>
			    </tr>
			    <tr>
				    <td width="697">
					    <input id="flArchivoExcel" runat="server" type="file"/></td>
				    <td align="right">
					    <asp:Button id="btnCargar" runat="server" Width="327px" Text="Cargar" onclick="btnCargar_Click1"></asp:Button></td>
			    </tr>
		    </table>
        </div>
    <asp:Label id="lbError" runat="server" ></asp:Label>
    </fieldset>
        <br />
		<asp:placeholder id="plSugerido" runat="server" Visible="false">
		<table class="filtersIn">
            <TR>
				<TD>Tipo de Sugerido :&nbsp;
					<asp:DropDownList id="ddlTipoSugerido" runat="server"></asp:DropDownList></TD>
				<TD align="right">
					<asp:Button id="btnCargSug" onclick="AgregSug" runat="server" Text="Cargar Sugeridos"></asp:Button></TD>
			</TR>
        </table>
		</asp:placeholder>

		<asp:label id="lbInfoLeg1" runat="server" visible="False">Porcentaje de Descuento
                por Promoción :</asp:label>&nbsp;
        <asp:label id="lbInfoPct" runat="server" visible="False"></asp:label>

<br>
<FIELDSET>
    <LEGEND class=Legends></LEGEND>
    <p>
    <asp:PlaceHolder ID="plcTotales" runat="server">
        <TABLE class=filtersIn >
		    <tbody>
			    <tr>
				    <td>
					    <asp:label id="Label1" runat="server">Valor Pedido:</asp:label>
				    </td>
				    <td><asp:textbox id="txtTotal" runat="server" ReadOnly="True" CssClass="AlineacionDerecha" Class="tmediano"></asp:textbox></td>
		                
				    <td>
					    <asp:label id="Label2" runat="server">Número de Items:</asp:label>
				    </td>
				    <td><asp:textbox id="txtNumItem" runat="server" ReadOnly="True" CssClass="AlineacionDerecha" Class="tpequeno"></asp:textbox></td>
			     
				    <td>
					    <asp:label id="Label3" runat="server">Total Asignado:</asp:label>
				    </td>
				    <td><asp:textbox id="txtTotAsig" runat="server" ReadOnly="True" CssClass="AlineacionDerecha" Class="tmediano"></asp:textbox></td>
			    </tr>
		    </tbody>
	    </table>
    </asp:PlaceHolder>
</p>
<p>

	<TABLE class=filtersIn >
		<tbody>
			<tr>
				<td>Vendedor :
					<asp:dropdownlist id="ddlVendedor" class="dmediano" runat="server"></asp:dropdownlist></td>
                <asp:PlaceHolder ID="plcVendedor" runat="server">
				<td>Clave Vendedor :&nbsp;
					<asp:textbox id="tbClaveVend" runat="server" class="tpequeno" TextMode="Password"></asp:textbox></td>
                </asp:PlaceHolder>
			</tr>
		</tbody>
	</table>
</p>
</FIELDSET>
<p>
<FIELDSET>
&nbsp;<asp:button id="btnActualizar" runat="server" onclick="ActualizarPedido" Text="Actualizar Pedido" Visible="false"></asp:button>
&nbsp;<asp:button id="btnAjus" OnClientClick="validarCitas();" onclick="NewAjust" runat="server" Text="Realizar Pedido" Visible="true"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
<asp:button id="btnAjusFac" OnClientClick="validarCitas();" onclick="NewAjustFac" runat="server" Text="Realizar Pedido y Facturar"></asp:button>
</FIELDSET>
</p>
<div id="autorizar" runat="server"  visible="false" class="divHabeas">
    <asp:PlaceHolder id="plcAutorizar" runat="server" Visible="true"></asp:PlaceHolder>
</div>
<p><asp:label id="lbInfo" runat="server"></asp:label></p>
<input id="hdDescCli" type="hidden" runat="server" />
<input id="hdTipoPed" type="hidden" runat="server" />
<input id="hdValor" type="hidden" runat="server" />

<script language = "javascript" type="text/javascript">
    $(function () {
        var divAutorizar = "<%=autorizar.ClientID%>";
        $("#" + divAutorizar).draggable();
    });
    
    
</script>