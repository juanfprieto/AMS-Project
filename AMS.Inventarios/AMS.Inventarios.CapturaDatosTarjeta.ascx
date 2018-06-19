<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.CapturaDatosTarjeta.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_CapturaDatosTarjeta" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script src="../js/tabpane.js" type="text/javascript"></script>
<script src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script type="text/javascript">setupAllTabs();</script>
<script type="text/javascript" >

	function ValSelInvFis()
	{
		// Muestra un mensaje de error si el inventario no ha sido seleccionado.
		if(document.getElementById('<%=ddlInventarios.ClientID%>').value == '')
		{
			alert('Seleccione un inventario físico para mirar la observación!');
			return false;
		}
	}

    function cambiarTarjeta(obj) 
    {
	    // Obtiene el código del inventario seleccionado.
	    var arrInventarioFisico = document.getElementById('<%=ddlInventarios.ClientID%>').value.split('-');

	    // Carga la información de la tarjeta.
	   // CargarInfoTarjeta(parseInt(objSender.value), arrInventarioFisico[0] +"-"+ arrInventarioFisico[1], parseInt(arrInventarioFisico[2]));
	    if (arrInventarioFisico.length == 3) {
	        CargarInfoTarjeta(parseInt(obj.value), arrInventarioFisico[0] + "-" + arrInventarioFisico[1], parseInt(arrInventarioFisico[2]));
	    } else {

	        CargarInfoTarjeta(parseInt(obj.value), arrInventarioFisico[0], parseInt(arrInventarioFisico[1]));
	    }

    }

	function IndexOf(objDdl,objValue)
	{
		var index = -1;
		
		for(var i=0;i<objDdl.options.length;i++)
		{
			if(objDdl.options[i].value == objValue)
			{
				index = i;
				break;
			}
		}
		
		return index;
	}
	
	function CargarUbicacionesPorAlmacen(codigoAlmacen)
	{
		var ddlUbicaciones = document.getElementById('<%=ddlUbicacion.ClientID%>');
		var opciones = AMS_Inventarios_CapturaDatosTarjeta.ConsultaUbicacionesPorAlmacen(codigoAlmacen).value;
		
		ddlUbicaciones.options.length = 0;
		
		if(opciones.Rows.length > 0)
		{
			for (var i = 0; i < opciones.Rows.length; ++i)
				ddlUbicaciones.options[ddlUbicaciones.options.length] = new Option(opciones.Rows[i].NOMBRE_UBICACION,opciones.Rows[i].CODIGO_UBICACION);
		}
	}
	
	function CambioAlmacen(objSender)
	{
		CargarUbicacionesPorAlmacen(objSender.value);
	}
	
	function CargarTarjetaSecuencia() 
	{

	    alert("entra funcion cargar tarjeta");
		var arrInventarioFisico = document.getElementById('<%=ddlInventarios.ClientID%>').value.split('-');

		//var numeroTarjeta = AMS_Inventarios_CapturaDatosTarjeta.ConsultarNumeroTarjetaSecuencia(arrInventarioFisico[0],parseInt(arrInventarioFisico[1]),parseInt(document.getElementById('<%=ddlConteosRelacionados.ClientID%>').value)).value;
		if (arrInventarioFisico.length == 3) {
		    alert("entra funcion cargar tarjeta 1er if");
            var numeroTarjeta = AMS_Inventarios_CapturaDatosTarjeta.ConsultarNumeroTarjetaSecuencia(arrInventarioFisico[1] + "-" + arrInventarioFisico[2], parseInt(arrInventarioFisico[2]), parseInt(document.getElementById('<%=ddlConteosRelacionados.ClientID%>').value)).value;
            if (numeroTarjeta != -1) {
                alert("entra funcion cargar tarjeta 2do if");
                // CargarInfoTarjeta(numeroTarjeta,arrInventarioFisico[0],parseInt(arrInventarioFisico[1]));
                CargarInfoTarjeta(numeroTarjeta, arrInventarioFisico[0] + "-" + arrInventarioFisico[1], parseInt(arrInventarioFisico[2]));
            } else
                document.getElementById('<%=pnlTarjetaPendiente.ClientID%>').style.display = 'none';
        } else {

		    var numeroTarjeta = AMS_Inventarios_CapturaDatosTarjeta.ConsultarNumeroTarjetaSecuencia(arrInventarioFisico[0], parseInt(arrInventarioFisico[1]), parseInt(document.getElementById('<%=ddlConteosRelacionados.ClientID%>').value)).value;
		    
            if (numeroTarjeta != -1)
		       CargarInfoTarjeta(numeroTarjeta,arrInventarioFisico[0],parseInt(arrInventarioFisico[1])); 
		    else
		        document.getElementById('<%=pnlTarjetaPendiente.ClientID%>').style.display = 'none';
        }
		
	}
	
	function ValidarValorConteo()
	{
		var regexEntero = /^(?:\+|-)?\d+$/;
		
		if(document.getElementById('<%=hdCodRef.ClientID%>').value == '')
		{
			alert('Se debe ingresar una tarjeta para realizar el conteo.');
			return false;
		}
		
		if(!regexEntero.test(document.getElementById('<%=tbCantidadConteo.ClientID%>').value))
		{
			alert('Por favor ingrese un valor valido para el valor del conteo.');
			return false;
		}
		
		return true;
	}
	
	function MostrarRefs(obTex,obCmbLin)
	{
		var codigoLinea = (obCmbLin.value.split('-'))[0];
		
		ModalDialogInventarios(obTex,
			'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as Codigo,MIT.mite_nombre as Nombre FROM dbxschema.mitems MIT inner join dbxschema.plineaitem PLIN ON MIT.plin_codigo = PLIN.plin_codigo WHERE MIT.plin_codigo=\''+codigoLinea+'\' ORDER By MIT.mite_codigo',
			new Array(),
			1,
			obCmbLin.value);
	}
	
	function CambioManualTarjetaAlta(objSender)
	{
		var arrInventarioFisico = document.getElementById('<%=ddlInventarios.ClientID%>').value.split('-');
		var regexEntero = /^(?:\+|-)?\d+$/;
		
		if(!regexEntero.test(objSender.value))
		{
			alert('Número ingresado invalido!');
			CargarTarjetaAltaSecuencia();
			return;
		}
		
		/* Código candidato a eliminación
		if(!AMS_Inventarios_CapturaDatosTarjeta.VerificarNumeroTarjetaAlta(arrInventarioFisico[0],parseInt(arrInventarioFisico[1]),parseInt(objSender.value)).value)
		{
			alert('El número ingresado no corresponde a una tarjeta de alta valida!');
			CargarTarjetaAltaSecuencia();
			return;
		}
		*/
	}
	
	function CargarTarjetaAltaSecuencia()
	{
		var arrInventarioFisico = document.getElementById('<%=ddlInventarios.ClientID%>').value.split('-');
		//	var numeroTarjeta = AMS_Inventarios_CapturaDatosTarjeta.ConsultarNumeroTarjetaSecuenciaAlta(arrInventarioFisico[0],parseInt(arrInventarioFisico[1])).value;

		if (arrInventarioFisico.length == 3) {
		    var numeroTarjeta = AMS_Inventarios_CapturaDatosTarjeta.ConsultarNumeroTarjetaSecuenciaAlta(arrInventarioFisico[0] + "-" + arrInventarioFisico[1], parseInt(arrInventarioFisico[2])).value;
		} else {
		    var numeroTarjeta = AMS_Inventarios_CapturaDatosTarjeta.ConsultarNumeroTarjetaSecuenciaAlta(arrInventarioFisico[0], parseInt(arrInventarioFisico[1])).value;
		}
		if(numeroTarjeta == -1)
		{
			document.getElementById('<%=tbNumTarjAlta.ClientID%>').readOnly = true;
			document.getElementById('<%=tbNumTarjAlta.ClientID%>').value = 'default';
			document.getElementById('<%=hdNumTarjAlta.ClientID%>').value = '-1';
		}
		else
		{
			document.getElementById('<%=tbNumTarjAlta.ClientID%>').readOnly = false;
			document.getElementById('<%=tbNumTarjAlta.ClientID%>').value = 
			document.getElementById('<%=hdNumTarjAlta.ClientID%>').value = 
			String(numeroTarjeta);
		}
	}
	
	// Verifica la información de la tarjeta de alta.
	function ValidarInformacionTarjetaAlta()
	{
		var errores = '';
		var salida = true;
		var regexEntero = /^(?:\+|-)?\d+$/;
		var arrInventarioFisico = document.getElementById('<%=ddlInventarios.ClientID%>').value.split('-');
		
		// Verifica que el valor de la tarjeta de conteo haya sido ingresada.		
		if(document.getElementById('<%=tbNumTarjAlta.ClientID%>').value == '')
		{
			errores += '- Ingrese un valor para la tarjeta de conteo.\n';
			salida = false;
		}
		
		// Verifica que el código ingresado sea válido.
		if(!AMS_Inventarios_CapturaDatosTarjeta.VerificarCodigoReferencia(document.getElementById('<%=ddlLineaTarjAlta.ClientID%>').value.split('-')[1],document.getElementById('<%=tbCodRefAlta.ClientID%>').value).value)
		{
			errores += '- El código de item ingresado es invalido.\n';
			salida = false;
		}
		
		// Verifica que el almacen haya sido seleccionado.
		if(document.getElementById('<%=ddlAlmacenAlta.ClientID%>').value == '')
		{
			errores += '- Seleccione un almacen.\n';
			salida = false;
		}
		
		// Verifica que una ubicación haya sido seleccionada.
		if(document.getElementById('<%=ddlUbicacionAlta.ClientID%>').value == '')
		{
			errores += '- Seleccione una ubicación.\n';
			salida = false;
		}
		
		// Verifica el formato de la cantidad.		
		if(!regexEntero.test(document.getElementById('<%=tbConteoAlta.ClientID%>').value))
		{
			errores += '- Ingrese un valor valido para la cantidad.\n';
			salida = false;
		}
		
		// Muestra el listado de errores procesados.
		if(!salida)
			alert(errores);
			
		return salida;
	}
	
	// Carga la información de la tarjeta seleccionada en los controles.
	function CargarInfoTarjeta(numeroTarjeta,prefijoInventario,numeroInventario)
	{
		// Carga la información de la tarjeta seleccionada.
		var arrayValoresTarjeta = AMS_Inventarios_CapturaDatosTarjeta.TraerInformacionTarjeta(prefijoInventario,numeroInventario,numeroTarjeta).value.split('&');
		
		if(arrayValoresTarjeta.length > 1)
		{
			// Cargar la información de la tarjeta seleccionada en los controles.
			document.getElementById('<%=lbTxtIngr2.ClientID%>').innerText = String(parseInt(arrayValoresTarjeta[6])+1);
			document.getElementById('<%=tbNumeroTarjeta.ClientID%>').value = arrayValoresTarjeta[0];
			document.getElementById('<%=hdNumeroTarjeta.ClientID%>').value = arrayValoresTarjeta[0];
			document.getElementById('<%=lbCodigoReferencia.ClientID%>').innerText = arrayValoresTarjeta[1];
			document.getElementById('<%=hdCodRef.ClientID%>').value = arrayValoresTarjeta[2];
			document.getElementById('<%=lbNombreReferencia.ClientID%>').innerText = arrayValoresTarjeta[3];
			
			if(document.getElementById('<%=ddlAlmacen.ClientID%>').value != arrayValoresTarjeta[4])
			{
				document.getElementById('<%=ddlAlmacen.ClientID%>').selectedIndex = IndexOf(document.getElementById('<%=ddlAlmacen.ClientID%>'),arrayValoresTarjeta[4]);
				CargarUbicacionesPorAlmacen(arrayValoresTarjeta[4]);
			}
			
			document.getElementById('<%=ddlUbicacion.ClientID%>').selectedIndex = IndexOf(document.getElementById('<%=ddlUbicacion.ClientID%>'),arrayValoresTarjeta[5]);
			document.getElementById('<%=lbConteoActual.ClientID%>').innerText = String(parseInt(arrayValoresTarjeta[6])+1);
			document.getElementById('<%=hdConteoActual.ClientID%>').value = String(parseInt(arrayValoresTarjeta[6])+1);
			document.getElementById('<%=lbCantidadConteo1.ClientID%>').innerText = arrayValoresTarjeta[7];
			document.getElementById('<%=lbCantidadConteo2.ClientID%>').innerText = arrayValoresTarjeta[8];
			document.getElementById('<%=lbCantidadConteo3.ClientID%>').innerText = arrayValoresTarjeta[9];
			document.getElementById('<%=tbCantidadConteo.ClientID%>').value = '';
			
			document.getElementById('<%=lbTxCodRef.ClientID%>').disabled =  
			document.getElementById('<%=lbTxNomRef.ClientID%>').disabled =  
			document.getElementById('<%=lbTxAlm.ClientID%>').disabled = 
			document.getElementById('<%=lbTxtUbi.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtContAct.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtCont1.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtCont2.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtCont3.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtIngr1.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtIngr2.ClientID%>').disabled = 
			document.getElementById('<%=lbCodigoReferencia.ClientID%>').disabled =  
			document.getElementById('<%=lbNombreReferencia.ClientID%>').disabled =  
			document.getElementById('<%=ddlAlmacen.ClientID%>').disabled =  
			document.getElementById('<%=ddlUbicacion.ClientID%>').disabled =  
			document.getElementById('<%=lbConteoActual.ClientID%>').disabled =  
			document.getElementById('<%=lbCantidadConteo1.ClientID%>').disabled =  
			document.getElementById('<%=lbCantidadConteo2.ClientID%>').disabled =  
			document.getElementById('<%=lbCantidadConteo3.ClientID%>').disabled =  
			document.getElementById('<%=tbCantidadConteo.ClientID%>').disabled =  
			document.getElementById('<%=btnGuardarConteo.ClientID%>').disabled = false;
		}
		else
		{
			// Desactiva los controles.
			document.getElementById('<%=lbTxCodRef.ClientID%>').disabled =  
			document.getElementById('<%=lbTxNomRef.ClientID%>').disabled =  
			document.getElementById('<%=lbTxAlm.ClientID%>').disabled = 
			document.getElementById('<%=lbTxtUbi.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtContAct.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtCont1.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtCont2.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtCont3.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtIngr1.ClientID%>').disabled =  
			document.getElementById('<%=lbTxtIngr2.ClientID%>').disabled = 
			document.getElementById('<%=lbCodigoReferencia.ClientID%>').disabled =  
			document.getElementById('<%=lbNombreReferencia.ClientID%>').disabled =  
			document.getElementById('<%=ddlAlmacen.ClientID%>').disabled =  
			document.getElementById('<%=ddlUbicacion.ClientID%>').disabled =  
			document.getElementById('<%=lbConteoActual.ClientID%>').disabled =  
			document.getElementById('<%=lbCantidadConteo1.ClientID%>').disabled =  
			document.getElementById('<%=lbCantidadConteo2.ClientID%>').disabled =  
			document.getElementById('<%=lbCantidadConteo3.ClientID%>').disabled =  
			document.getElementById('<%=tbCantidadConteo.ClientID%>').disabled =  
			document.getElementById('<%=btnGuardarConteo.ClientID%>').disabled = true;
			
			alert('No se encuentra registrado el número de tarjeta')
		}
	}
</script>

<fieldset><legend>Inventario Físico</legend>
	<table id="Table1" class="filtersIn">
		<tr>
			<td>Seleccione el Inventario:<br />
			<asp:dropdownlist id="ddlInventarios" class="dmediano" runat="server">
            </asp:dropdownlist>
            </td>
		</tr>
		<tr>
			<td colspan="2"><asp:button id="btnAceptar" runat="server" Text="Cargar Información" CausesValidation="False" onclick="btnAceptar_Click"></asp:button>&nbsp;
				<asp:button id="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False" onclick="btnCancelar_Click"></asp:button></td>
		</tr>
	</table>
</fieldset>
<fieldset>
    <asp:Panel id="pnlInfoProceso" Runat="server" Visible="False">
	    <div class="tab-pane" id="tab-pane-1">
    	    <div class="tab-page" id="pnlTarjetaPendiente" align="center" runat="server">
			    <h2 class="tab">Conteos Relacionados</h2>
			    <table id="Table3" class="filtersIn">
				    <tr>
					    <td>Conteos Relacionados :<br />
						    <asp:dropdownlist id="ddlConteosRelacionados" class="dpequeno" runat="server" onchange="CargarTarjetaSecuencia();"></asp:dropdownlist></td>
				
					    <td>
						    <p><input id="btnCargarSecuencia" onclick="CargarTarjetaSecuencia();" type="button" value="Cargar Tarjeta en Secuencia" class="noEspera" /></p>
					    </td>
				    </tr>
			    </table>

			    <fieldset>
                    <legend>Información Tajeta de Conteo</legend>
				    <table id="Table2" class="filtersIn">
					    <tr>
						    <td>
							    <p>
								    <asp:Label id="lbTxNumTj" class="lpequeno" runat="server">Número de Tarjeta :</asp:Label></p>
							    <p>(Ingrese el número de la tarjeta y teclee TAB.)</p>
						    </td>
						    <td>
							    <asp:TextBox id="tbNumeroTarjeta" runat="server" class="tpequeno" onBlur="cambiarTarjeta(this);"></asp:TextBox>
                                <input id="hdNumeroTarjeta" type="hidden" runat="server" />
						    </td>
					    </tr>
					    <tr>
						    <td>
							    <asp:Label id="lbTxCodRef" runat="server">Código de Referencia :</asp:Label></td>
						    <td>
							    <asp:Label id="lbCodigoReferencia" class="lpequeno" runat="server"></asp:Label><input id="hdCodRef" type="hidden" runat="server" /></td>
					    </tr>
					    <tr>
						    <td>
							    <asp:Label id="lbTxNomRef" runat="server">Nombre de Referencia :</asp:Label></td>
						    <td>
							    <asp:Label id="lbNombreReferencia" runat="server"></asp:Label></td>
					    </tr>
					    <tr>
						    <td>
							    <asp:Label id="lbTxAlm" runat="server">Almacén de la Ubicación :</asp:Label></td>
						    <td>
							    <asp:DropDownList id="ddlAlmacen" runat="server" class="dpequeno" OnChange="CambioAlmacen(this);"></asp:DropDownList></td>
					    </tr>
					    <tr>
						    <td>
							    <asp:Label id="lbTxtUbi" runat="server">Ubicación :</asp:Label></td>
						    <td>
							    <asp:DropDownList id="ddlUbicacion" class="dmediano" runat="server"></asp:DropDownList></td>
					    </tr>
					    <tr>
						    <td>
							    <asp:Label id="lbTxtContAct" runat="server">Conteo Actual</asp:Label></td>
						    <td>
							    <asp:Label id="lbConteoActual" class="lpequeno" runat="server"></asp:Label><input id="hdConteoActual" type="hidden" runat="server" /></td>
					    </tr>
					    <tr style="DISPLAY: none">
						    <td>
							    <asp:Label id="lbTxtCont1" runat="server">Conteo 1 :</asp:Label></td>
						    <td align="right">
							    <asp:Label id="lbCantidadConteo1" runat="server"></asp:Label></td>
					    </tr>
					    <tr style="DISPLAY: none">
						    <td>
							    <asp:Label id="lbTxtCont2" runat="server">Conteo 2 :</asp:Label></td>
						    <td align="right">
							    <asp:Label id="lbCantidadConteo2" runat="server"></asp:Label></td>
					    </tr>
					    <tr style="DISPLAY: none">
						    <td>
							    <asp:Label id="lbTxtCont3" runat="server">Conteo 3 :</asp:Label></td>
						    <td align="right">
							    <asp:Label id="lbCantidadConteo3" runat="server"></asp:Label></td>
					    </tr>
					    <tr>
						    <td>
							    <asp:Label id="lbTxtIngr1" runat="server">Ingresar Información para conteo </asp:Label>
							    <asp:Label id="lbTxtIngr2" runat="server"></asp:Label></td>
						    <td>
							    <asp:TextBox id="tbCantidadConteo" runat="server" class="tpequeno"></asp:TextBox></td>
					    </tr>
					    <tr>
						    <td align="right" colspan="2">
							    <P>
								    <asp:Button id="btnGuardarConteo" runat="server" Text="Guardar Conteo" onclick="btnGuardarConteo_Click"></asp:Button></P>
						    </td>
					    </tr>
				    </table>
			</fieldset>
		</div>
		<div class="tab-page" id="pnlTarjetaAltaPendiente" align="center" runat="server">
            <fieldset>
			    <h2 class="tab">Tarjetas de Alta</h2>
			    <table id="Table4" class="filtersIn">
				    <tr>
					    <td>Número de Tarjeta : (conteo único y definitivo) </td>
					    <td>
						    <asp:TextBox id="tbNumTarjAlta" onblur="CambioManualTarjetaAlta(this);" runat="server" class="tpequeno"></asp:TextBox><INPUT id="hdNumTarjAlta" type="hidden" runat="server">
					    </td>
					    <td>
						    <p><input id="btnCargarSecuenciaAlta" onclick="CargarTarjetaAltaSecuencia();" type="button"
								    value="Cargar Tarjeta en Secuencia" /></p>
					    </td>
				    </tr>
				    <tr>
					    <td>Línea de Bodega :</td>
					    <td>
						    <asp:DropDownList id="ddlLineaTarjAlta" class="dmediano" runat="server"></asp:DropDownList></td>
				    </tr>
				    <tr>
					    <td>
						    <p>Referencia Ítem :</p>
						    <p>(Haga doble clic para consultar los ítems registrados.)</p>
					    </td>
					    <td>
						    <asp:TextBox id="tbCodRefAlta" runat="server" class="tpequeno"></asp:TextBox></td>
				    </tr>
				    <tr>
					    <td>Almacén :</td>
					    <td>
						    <asp:DropDownList id="ddlAlmacenAlta" class="dpequeno" runat="server"></asp:DropDownList></td>
				    </tr>
				    <tr>
					    <td>Ubicación :</td>
					    <td>
						    <asp:DropDownList id="ddlUbicacionAlta" class="dmediano" runat="server"></asp:DropDownList></td>
				    </tr>
				    <tr>
					    <td>Cantidad&nbsp;Primer Conteo :</td>
					    <td>
						    <asp:TextBox id="tbConteoAlta" runat="server" class="tpequeno"></asp:TextBox></td>
				    </tr>
				    <tr>
					    <td align="right" colspan="2">
						    <P>
							    <asp:Button id="btnGrabarTarjAlta" runat="server" Text="Guardar Conteo" onclick="btnGrabarTarjAlta_Click"></asp:Button></P>
					    </td>
				    </tr>
			    </table>
            </fieldset>
		</div>
	</div>
</asp:Panel>
</fieldset>

