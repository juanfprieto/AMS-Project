<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.EditarTarjetaConteo.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_EditarTarjetaConteo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type = "text/javascript" >
function ValSelInvFis(){if(document.getElementById('<%=ddlInventarios.ClientID%>').value == ''){alert('Seleccione un inventario físico para mirar la observación!');return false;}}
function CargarUbicacionesPorAlmacen(codigoAlmacen){var ddlUbicaciones = document.getElementById('<%=ddlUbicacion.ClientID%>');var opciones = AMS_Inventarios_EditarTarjetaConteo.ConsultaUbicacionesPorAlmacen(codigoAlmacen).value;ddlUbicaciones.options.length = 0;if(opciones.Rows.length > 0){for (var i = 0; i < opciones.Rows.length; ++i)ddlUbicaciones.options[ddlUbicaciones.options.length] = new Option(opciones.Rows[i].NOMBRE_UBICACION,opciones.Rows[i].CODIGO_UBICACION);}}
function IndexOf(objDdl,objValue){var index = -1;for(var i=0;i<objDdl.options.length;i++){if(objDdl.options[i].value == objValue){index = i;break;}}return index;}
function CargarInfoTarjetaEvento(){var arrInventarioFisico = document.getElementById('<%=ddlInventarios.ClientID%>').value.split('-');var regexEntero = new RegExp("[0-9]+");if(regexEntero.test(document.getElementById('<%=tbNumeroTarjeta.ClientID%>').value))	CargarInfoTarjeta(arrInventarioFisico[0],parseInt(arrInventarioFisico[1]),parseInt(document.getElementById('<%=tbNumeroTarjeta.ClientID%>').value));else alert('Ingrese un número de tarjeta valido');}
function ValidarInformacionEdicion(){var regexEntero = /^(?:\+|-)?\d+$/;if(document.getElementById('<%=hdNumeroTarjeta.ClientID%>').value == ''){alert('Por favor ingrese una tarjeta registrada en este inventario físico');return false;}var conteoRelacionado = parseInt(document.getElementById('<%=hdConteoActual.ClientID%>').value);var txtSalida = '';var salidaBooleano = true;if(conteoRelacionado == 2){if(!regexEntero.test(document.getElementById('<%=tbEdicionConteo1.ClientID%>').value)){txtSalida += '- Ingrese un valor valido para el primer conteo\n';salidaBooleano = false;}}else if(conteoRelacionado == 3){if(!regexEntero.test(document.getElementById('<%=tbEdicionConteo1.ClientID%>').value)){txtSalida += '- Ingrese un valor valido para el primer conteo\n';salidaBooleano = false;}if(!regexEntero.test(document.getElementById('<%=tbEdicionConteo2.ClientID%>').value)){txtSalida += '- Ingrese un valor valido para el segundo conteo\n';salidaBooleano = false;}}else if(conteoRelacionado == 4){if(!regexEntero.test(document.getElementById('<%=tbEdicionConteo1.ClientID%>').value)){txtSalida += '- Ingrese un valor valido para el primer conteo\n';salidaBooleano = false;}if(!regexEntero.test(document.getElementById('<%=tbEdicionConteo2.ClientID%>').value)){txtSalida += '- Ingrese un valor valido para el segundo conteo\n';salidaBooleano = false;}if(!regexEntero.test(document.getElementById('<%=tbEdicionConteo3.ClientID%>').value)){txtSalida += '- Ingrese un valor valido para el tercer conteo\n';salidaBooleano = false;}}if(!salidaBooleano){alert(txtSalida);return false;}return confirm('Esta seguro de realizar este proceso de edición?');}
function CargarInfoTarjeta(prefijoInventario,numeroInventario,numeroTarjeta)
{
	var arrayValoresTarjeta = AMS_Inventarios_EditarTarjetaConteo.TraerInformacionTarjetaEdicion(prefijoInventario,numeroInventario,numeroTarjeta).value.split('&');
	if(arrayValoresTarjeta.length > 1)
	{
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
		if((parseInt(arrayValoresTarjeta[6])+1) < 4)
			document.getElementById('<%=lbConteoActual.ClientID%>').innerText = String(parseInt(arrayValoresTarjeta[6])+1);
		else if((parseInt(arrayValoresTarjeta[6])+1) == 4)
			document.getElementById('<%=lbConteoActual.ClientID%>').innerText = 'Finalizado';
		document.getElementById('<%=hdConteoActual.ClientID%>').value = String(parseInt(arrayValoresTarjeta[6])+1);
		document.getElementById('<%=tbEdicionConteo1.ClientID%>').value = arrayValoresTarjeta[7];
		document.getElementById('<%=tbEdicionConteo2.ClientID%>').value = arrayValoresTarjeta[8];
		document.getElementById('<%=tbEdicionConteo3.ClientID%>').value = arrayValoresTarjeta[9];
		document.getElementById('<%=lbTxCodRef.ClientID%>').disabled =  document.getElementById('<%=lbTxNomRef.ClientID%>').disabled =  document.getElementById('<%=lbTxAlm.ClientID%>').disabled = document.getElementById('<%=lbTxtUbi.ClientID%>').disabled =  document.getElementById('<%=lbTxtContAct.ClientID%>').disabled =  document.getElementById('<%=lbTxtCont1.ClientID%>').disabled = document.getElementById('<%=lbTxtCont2.ClientID%>').disabled =  document.getElementById('<%=lbTxtCont3.ClientID%>').disabled = document.getElementById('<%=lbCodigoReferencia.ClientID%>').disabled =  document.getElementById('<%=lbNombreReferencia.ClientID%>').disabled =  document.getElementById('<%=ddlAlmacen.ClientID%>').disabled =  document.getElementById('<%=ddlUbicacion.ClientID%>').disabled =  document.getElementById('<%=lbConteoActual.ClientID%>').disabled = document.getElementById('<%=btnGuardarEdicion.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo1.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo2.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo3.ClientID%>').disabled = false;
		if((parseInt(arrayValoresTarjeta[6])+1) == 1)
			document.getElementById('<%=btnGuardarEdicion.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo1.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo2.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo3.ClientID%>').disabled = true;	
		else if((parseInt(arrayValoresTarjeta[6])+1) == 2)
		{
			document.getElementById('<%=btnGuardarEdicion.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo1.ClientID%>').disabled = false;
			document.getElementById('<%=tbEdicionConteo2.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo3.ClientID%>').disabled = true;
		}
		else if((parseInt(arrayValoresTarjeta[6])+1) == 3)
		{
			document.getElementById('<%=btnGuardarEdicion.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo1.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo2.ClientID%>').disabled = false;
			document.getElementById('<%=tbEdicionConteo3.ClientID%>').disabled = true;
		}
		else if((parseInt(arrayValoresTarjeta[6])+1) == 4)
			document.getElementById('<%=btnGuardarEdicion.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo1.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo2.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo3.ClientID%>').disabled = false;
	}
	else
	{
		document.getElementById('<%=lbTxCodRef.ClientID%>').disabled =  document.getElementById('<%=lbTxNomRef.ClientID%>').disabled =  document.getElementById('<%=lbTxAlm.ClientID%>').disabled = document.getElementById('<%=lbTxtUbi.ClientID%>').disabled =  document.getElementById('<%=lbTxtContAct.ClientID%>').disabled =  document.getElementById('<%=lbTxtCont1.ClientID%>').disabled = document.getElementById('<%=lbTxtCont2.ClientID%>').disabled =  document.getElementById('<%=lbTxtCont3.ClientID%>').disabled = document.getElementById('<%=lbCodigoReferencia.ClientID%>').disabled =  document.getElementById('<%=lbNombreReferencia.ClientID%>').disabled =  document.getElementById('<%=ddlAlmacen.ClientID%>').disabled =  document.getElementById('<%=ddlUbicacion.ClientID%>').disabled =  document.getElementById('<%=lbConteoActual.ClientID%>').disabled = document.getElementById('<%=btnGuardarEdicion.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo1.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo2.ClientID%>').disabled = document.getElementById('<%=tbEdicionConteo3.ClientID%>').disabled = true;
		alert('No se encuentra registrado el numero de Tarjeta')
	}
}
</script>

<fieldset><legend>Inventario Físico</legend>
	<table id="Table1" class="filtersIn">
		<tr>
			<td>Seleccione el Inventario:<br />
			<asp:dropdownlist id="ddlInventarios" class="dmediano" runat="server"></asp:dropdownlist>
            </td>
		</tr>
		<tr>
			<td colSpan="2"><asp:button id="btnAceptar" runat="server" CausesValidation="False" Text="Cargar Información" onclick="btnAceptar_Click"></asp:button>&nbsp;
				<asp:button id="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" onclick="btnCancelar_Click"></asp:button></td>
		</tr>
	</table>
</fieldset>

<asp:panel id="pnlInfoProceso" Visible="False" Runat="server">
	<FIELDSET><LEGEND>Información Tajeta de Conteo</LEGEND>
		<table id="Table2" class="filtersIn">
			<TR>
				<TD>
					<asp:Label id="lbTxNumTj" runat="server">Número de Tarjeta :</asp:Label>
					<asp:TextBox id="tbNumeroTarjeta" class="tpequeno" runat="server"></asp:TextBox>
                    </td>
                    <td>
                    <INPUT id="hdNumeroTarjeta" type="hidden" name="hdNumeroTarjeta" runat="server">
				<INPUT id="btnCargarInfo" onclick="CargarInfoTarjetaEvento();" type="button" value="Cargar Información Tarjeta" name="btnCargarInfo" class= "noEspera">
                </TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxCodRef" runat="server">Código de Referencia :</asp:Label>
					<asp:Label id="lbCodigoReferencia" class="lpequeno" runat="server"></asp:Label><INPUT id="hdCodRef" type="hidden" name="hdCodRef" runat="server"></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxNomRef" runat="server">Nombre de Referencia :</asp:Label>
					<asp:Label id="lbNombreReferencia" class="lpequeno" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxAlm" runat="server">Almacen de la Ubicación :</asp:Label>
					<asp:DropDownList id="ddlAlmacen" runat="server" class="dpequeno" OnChange="CambioAlmacen(this);"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxtUbi" runat="server">Ubicación :</asp:Label>
					<asp:DropDownList id="ddlUbicacion" class="dmediano" runat="server"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxtContAct" runat="server">Conteo Actual</asp:Label>
					<asp:Label id="lbConteoActual" class="lpequeno" runat="server"></asp:Label><input id="hdConteoActual" type="hidden" name="hdConteoActual" runat="server"/></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxtCont1" runat="server">Cantidad Editar en Conteo 1 :</asp:Label>
					<asp:TextBox id="tbEdicionConteo1" runat="server" class="tpequeno">0</asp:TextBox></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxtCont2" runat="server">Cantidad Editar en Conteo 2 :</asp:Label>
					<asp:TextBox id="tbEdicionConteo2" runat="server" class="tpequeno">0</asp:TextBox></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxtCont3" runat="server">Cantidad Editar en Conteo 3 :</asp:Label>
					<asp:TextBox id="tbEdicionConteo3" runat="server" class="tpequeno">0</asp:TextBox></TD>
			</TR>
			<TR>
				<TD>
					<asp:Button id="btnGuardarEdicion" runat="server" Text="Guardar Edición" onclick="btnGuardarEdicion_Click"></asp:Button></TD>
			</TR>
		</TABLE>
	</FIELDSET>

</asp:panel>
