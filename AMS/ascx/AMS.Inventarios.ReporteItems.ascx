<%@ Control Language="c#" codebehind="AMS.Inventarios.ReporteItems.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ReporteItems" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script src="../js/AMS.modernizr" type="text/javascript"></script>
<script src="../js/AMS.modernizr-custom" type="text/javascript"></script>
<script type="text/javascript">
    function Lista()
    {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<script type="text/javascript">  

    
	function CargaITEM(/*textbox*/ob,/*dropdownlist*/obCmbLin,/*string*/ano)
    {

	    ModalDialogInventarios(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE,DBXSCHEMA.CANTACTL(MIT.mite_codigo,' + ano + ') AS SALDO FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN, dbxschema.plineaitem PLIN2 WHERE PLIN.plin_codigo=\'' + (obCmbLin.value.split('-'))[0] + '\' AND PLIN.plin_TIPO=PLIN2.plin_TIPO AND MIT.plin_codigo=PLIN2.plin_codigo ORDER By Nombre', new Array(), 1, obCmbLin.value);
       
	}

	function MostrarRefs(obTex,obCmbLin)
	{
	    ModalDialogInventarios(obTex, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE,DBXSCHEMA.CANTACTL(MIT.mite_codigo,' + ano + ') AS SALDO, MPR.mpre_precio AS PRECIO FROM dbxschema.mitems MIT LEFT JOIN dbxschema.mprecioitem MPR ON MPR.mite_codigo=MIT.mite_codigo, dbxschema.plineaitem PLIN, dbxschema.plineaitem PLIN2 WHERE PLIN.plin_codigo=\'' + (obCmbLin.value.split('-'))[0] + '\'  AND PLIN.PLIN_TIPO = PLIN2.PLIN_TIPO AND MIT.plin_codigo=PLIN2.plin_codigo ORDER By MIT.mite_codigo', new Array(), 1, obCmbLin.value);
	    ModalDialog(obTex, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN, dbxschema.plineaitem PLIN2 WHERE PLIN.plin_codigo=\'' + (obCmbLin.value.split('-'))[0] + '\' AND PLIN.PLIN_TIPO = PLIN2.PLIN_TIPO AND MIT.plin_codigo=PLIN2.plin_codigo ORDER By mite_codigo', new Array());
	}

	function LlenarInformacionBasica(obj)
	{
		var objLinea=document.getElementById("<%=ddlLineas.ClientID%>");
		var linea=objLinea.value.split('-');
		var oItm=obj.value;
			obj.value=ReporteItems.ConsultarSustitucion(obj.value,linea[1]).value
		if(obj.value!=oItm)
			alert('El item: '+oItm+' ha sido cambiado por su sustitución: '+obj.value);
	
		var nombre=document.getElementById("<%=txtRefa.ClientID%>");
		var objCosto=document.getElementById("<%=lbCosto.ClientID%>");
	    var objValor = document.getElementById("<%=lbValor.ClientID%>");
        var objValor=document.getElementById("<%=lbValorIva.ClientID%>");
		var objCantidad=document.getElementById("<%=lbCantidad.ClientID%>");
		var objSustituciones=document.getElementById("<%=lbSustituciones.ClientID%>");
		var objUbicacion=document.getElementById("<%=lbUbicacion.ClientID%>");
	    var objSerial = document.getElementById("<%=lbSerial.ClientID%>");
	    var url = document.location.href;
	    var getString = url.split('?')[1];
	    var GET = getString.split('&');
	    var get = {};//this object will be filled with the key-value pairs and returned.
	    for (var i = 0, l = GET.length; i < l; i++) {
	        var tmp = GET[i].split('=');
	        get[tmp[0]] = unescape(decodeURI(tmp[1]));
	        var permiso = get.Permiso;
	    }
              
        
		if(obj.value!='')
		{
			var existe=ExisteReferencia(obj.value,linea[1]);
			if(existe!='')
			{
			    nombre.value = existe;
			    if (permiso == undefined)
			    { permiso = 'C' }
				ReporteItems.TraerInformacionCosto(obj.value, linea[1],permiso, TraerInformacionCosto_CallBack);
				ReporteItems.TraerInformacionValor(obj.value, linea[1], TraerInformacionValor_CallBack);
				ReporteItems.TraerInformacionValorIva(obj.value, linea[1], TraerInformacionValorIva_CallBack);
				ReporteItems.TraerInformacionCantidad(obj.value,linea[1],TraerInformacionCantidad_CallBack);
				ReporteItems.TraerInformacionSustituciones(obj.value,linea[1],TraerInformacionSustituciones_CallBack);
				ReporteItems.TraerInformacionUbicacion(obj.value,linea[1],TraerInformacionUbicacion_CallBack);
				ReporteItems.TraerInformacionSerial(obj.value, linea[1], TraerInformacionSerial_CallBack);
				ReporteItems.TraerInformacionAplicacion(obj.value, TraerInformacionAplicacion_CallBack);	
			}
			else
			{
				alert('La referencia '+obj.value+' no existe');
				obj.value='';
				nombre.value='';
				objCosto.innerHTML='';
				objValor.innerHTML='';
				objCantidad.innerHTML='';
				objSustituciones.innerHTML='';
				objUbicacion.innerHTML='';
				objSerial.innerHTML='';
			}
		}
		else
		{
			nombre.value='';
			objCosto.innerHTML='';
			objValor.innerHTML='';
			objCantidad.innerHTML='';
			objSustituciones.innerHTML='';
			objUbicacion.innerHTML='';
			objSerial.innerHTML='';
		}
	}

	function ExisteReferencia(item,linea)
	{
		return ReporteItems.TraerNombreReferencia(item,linea).value;
	}

	function TraerInformacionAplicacion_CallBack(response) 
    {
        var objTextoAplicacion = document.getElementById("<%=lbAplicacion.ClientID%>");
        var hdAplic = document.getElementById("<%=hdAplic.ClientID%>");
        objTextoAplicacion.innerHTML = response.value;
        hdAplic.value = response.value;
	}

	function TraerInformacionCosto_CallBack(response)
	{
		if(response.error != null)
		{
			alert('TraerInformacionCosto: '+response.error);
			return;
		}
		
		var objCosto=document.getElementById("<%=lbCosto.ClientID%>");
		
		var respuesta=response.value;
		
		objCosto.innerHTML=respuesta;
		hidCos.value=respuesta;
	}

	function TraerInformacionValor_CallBack(response)
	{
		if(response.error != null)
		{
			alert('TraerInformacionValor: '+response.error);
			return;
		}
		
		var objValor=document.getElementById("<%=lbValor.ClientID%>");
		var hidVal=document.getElementById("<%=hdnvalor.ClientID%>");
		var respuesta=response.value;
		
		objValor.innerHTML=respuesta;
		hidVal.value=respuesta;
	}

    	function TraerInformacionValorIva_CallBack(response)
	{
		if(response.error != null)
		{
			alert('TraerInformacionValorIva: '+response.error);
			return;
		}
		
		var objValor=document.getElementById("<%=lbValorIva.ClientID%>");
		var hidVal=document.getElementById("<%=hdnvalorIva.ClientID%>");
		var respuesta=response.value;
		
		objValor.innerHTML=respuesta;
		hidVal.value=respuesta;
	}

	function TraerInformacionCantidad_CallBack(response)
	{
		if(response.error != null)
		{
			alert('TraerInformacionCantidad: '+response.error);
			return;
		}
		
		var objCantidad=document.getElementById("<%=lbCantidad.ClientID%>");
		var hidCant=document.getElementById("<%=hdncant.ClientID%>");
		var respuesta=response.value;
		
		objCantidad.innerHTML=respuesta;
		hidCant.value=respuesta;
	}

	function TraerInformacionSustituciones_CallBack(response)
	{
		if(response.error != null)
		{
			alert('TraerInformacionSustituciones: '+response.error);
			return;
		}
		
		var objSustituciones=document.getElementById("<%=lbSustituciones.ClientID%>");
		var hidSust=document.getElementById("<%=hdnsust.ClientID%>");
		var respuesta=response.value;
		
		objSustituciones.innerHTML=respuesta;
		hidSust.value=respuesta;
	}

	function TraerInformacionUbicacion_CallBack(response)
	{
		if(response.error != null)
		{
			alert('TraerInformacionUbicacion: '+response.error);
			return;
		}
		
		var objUbicacion=document.getElementById("<%=lbUbicacion.ClientID%>");
		var hidUbi=document.getElementById("<%=hdnubi.ClientID%>");
		var respuesta=response.value;
		
		objUbicacion.innerHTML=respuesta;
		hidUbi.value=respuesta;
	}

	function TraerInformacionSerial_CallBack(response)
	{
		if(response.error != null)
		{
			alert('TraerInformacionSerial: '+response.error);
			return;
		}
		
		var objSerial=document.getElementById("<%=lbSerial.ClientID%>");
		var hidSer=document.getElementById("<%=hdnser.ClientID%>");
		var respuesta=response.value;
		
		objSerial.innerHTML=respuesta;
		hidSer.value=respuesta;
	}
    

</script>
<p>
    
	<table class="filters">
		<tbody>
			<tr>
				<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
				<td>
                <fieldset>
                <legend>REFERENCIA</legend>
					<table class="main" cellspacing="5">
						<tbody>							
							<tr>
								<td>
									Código:<br>
								
									<asp:TextBox id="txtRef" class="tpequeno" runat="server" onBlur="LlenarInformacionBasica(this)"></asp:TextBox>
								</td>
								<td rowspan="2">
									<TABLE style="BACKGROUND-COLOR: white" width="100%">
										<TR>
											<TD style="COLOR: navy">Costo Promedio:
											</TD>
											<TD align="left">
												<asp:Label ID="lbCosto" class="lpequeno" Runat="server"></asp:Label>
											</TD>
										</TR>
										<TR>
											<TD style="COLOR: navy">Valor Público sin IVA:
											</TD>
											<TD align="left"><asp:Label ID="lbValor" class="lpequeno" Runat="server"></asp:Label></TD>
										</TR>
                                        <TR>
											<TD style="COLOR: navy">Valor Público con IVA:
											</TD>
											<TD align="left"><asp:Label ID="lbValorIva" class="lpequeno" Runat="server"></asp:Label></TD>
										</TR>
										<TR>
											<TD style="COLOR: navy">Cantidad Actual:
											</TD>
											<TD align="left"><asp:Label ID="lbCantidad" class="lpequeno" Runat="server"></asp:Label></TD>
										</TR>
										<TR>
											<TD style="COLOR: navy">Sustituciones:
											</TD>
											<TD>
												<asp:Label ID="lbSustituciones" class="lpequeno" Runat="server"></asp:Label>
											</TD>
										</TR>
										<TR>
											<TD style="COLOR: navy">Ubicación:
											</TD>
											<TD>
												<asp:Label ID="lbUbicacion" class="lpequeno" Runat="server"></asp:Label>
											</TD>
										</TR>
										<TR>
											<TD style="COLOR: navy">Serial:
											</TD>
											<TD>
												<asp:Label ID="lbSerial" class="lpequeno" Runat="server"></asp:Label>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<tr>
								<td>
									Descripción:<br>								
									<asp:TextBox id="txtRefa" class="tpequeno" runat="server" ReadOnLy="True"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>
									Linea:<br>								
									<asp:DropDownList id="ddlLineas" class="dpequeno" runat="server"></asp:DropDownList>
								</td>
							</tr>
                            <tr>
                                <td colspan="4">
                                    <b>Aplicación:</b> <asp:Label ID="lbAplicacion" class="lgrande" Runat="server"></asp:Label>
                                </td>
                            </tr>
						</tbody>
					</table>
                    </fieldset>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>

	<asp:PlaceHolder id="toolsHolder" runat="server" visible="false">
		<TABLE class="tools" width="780">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"
						BorderWidth="0px"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:PlaceHolder>
</p>
<table bordercolor="white">
	<tbody>
		<tr>
			<td>
				<p>
					<asp:LinkButton id="btnDatBas" onclick="GenDatBas" Width="90px" runat="server" BorderStyle="None"
						Font-Underline="True" Text="Datos Básicos">Datos Básicos</asp:LinkButton>
					&nbsp;<asp:LinkButton id="btnCants" onclick="GenCants" Width="100px" runat="server" BorderStyle="None"
						Font-Underline="True" Text="Costos y Saldos"></asp:LinkButton>
					&nbsp;
					<asp:LinkButton id="btnResMes" onclick="GenResMes" Width="94px" runat="server" BorderStyle="None"
						Font-Underline="True" Text="Resumen Mes"></asp:LinkButton>
					&nbsp;<asp:LinkButton id="btnResAno" onclick="GenResAno" Width="90px" runat="server" BorderStyle="None"
						Font-Underline="True" Text="Resumen Año ">Resumen Año</asp:LinkButton>
					&nbsp;
					<asp:LinkButton id="btnKar" onclick="BindGridView" Width="47px" runat="server" BorderStyle="None" Font-Underline="True"
						Text="Kardex"></asp:LinkButton>
					&nbsp;
					<asp:LinkButton id="btnEst" onclick="GenEst" Width="75px" runat="server" BorderStyle="None" Font-Underline="True"
						Text="Estadisticas">Estadisticas Demanda</asp:LinkButton>
					&nbsp;<asp:LinkButton id="btnPed" onclick="GenPed" Width="56px" runat="server" BorderStyle="None" Font-Underline="True"
						Text="Pedidos"></asp:LinkButton>
                    &nbsp;<asp:LinkButton id="btnDistrib" onclick="GenDist" Width="56px" runat="server" BorderStyle="None"
						Font-Underline="True" Text="Distribución"></asp:LinkButton>
					&nbsp;<asp:LinkButton id="btnAplic" onclick="GenApli" Width="56px" runat="server" BorderStyle="None"
						Font-Underline="True" Text="Aplicación"></asp:LinkButton>

				</p>
			</td>
		</tr>
		<tr>
			<td>
				<table width="100%">
					<tbody>
						<asp:PlaceHolder id="plhDatosBasicos" runat="server">
							<TR>
								<TD>
									<P align="right">
										<ASP:DataGrid id="grMITEM" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
	                                        CellPadding="3" CellSpacing="1">
	                                        <FooterStyle cssclass="footer"></FooterStyle>
	                                        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
	                                        <PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
	                                        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
	                                        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	                                        <ItemStyle cssclass="item"></ItemStyle>
                                        </ASP:DataGrid></P>
								</TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plhCantidades" runat="server">
							<TR>
								<TD align="right">Año:
									<asp:DropDownList id="ddlCantAno" class="dpequeno" runat="server"></asp:DropDownList>&nbsp;&nbsp; 
									Almacen:
									<asp:DropDownList id="ddlCantAlm" class="dpequeno" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:Button id="btnCantAct" onclick="GenCants" runat="server" Text="Actualizar"></asp:Button>
									<P align="right">
										<ASP:DataGrid id="grCants" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
											CellPadding="3" CellSpacing="1">
											<FooterStyle cssclass="footer"></FooterStyle>
											<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
											<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
											<SelectedItemStyle font-bold="True" cssclass="selectde"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
										</ASP:DataGrid></P>
								</TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plhResumenMes" runat="server">
							<TR>
								<TD align="right">Año:
									<asp:DropDownList id="ddlResMesAno" runat="server"></asp:DropDownList>&nbsp;&nbsp; 
									Mes:
									<asp:DropDownList id="ddlResMesMes" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
									<asp:Button id="btnResMesAct" onclick="GenResMes" runat="server" Text="Actualizar"></asp:Button>
									<P align="right">
										<ASP:DataGrid id="grResMes" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
											CellPadding="3" CellSpacing="1">
											<FooterStyle cssclass="footer"></FooterStyle>
											<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
											<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
											<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
										</ASP:DataGrid></P>
								</TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plhResumenAno" runat="server">
							<TR>
								<TD align="right">
									<TABLE class="main" width="100%">
										<TR>
											<TD>Formato Muestra Información:
												<BR>
												Cantidad
												<BR>
												Costo
												<BR>
												Precio
											</TD>
											<TD align="right">
                                            Año:
												<asp:DropDownList id="ddlResAnoAno" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
												<asp:Button id="btnResAnoAct" onclick="GenResAno" runat="server" Text="Actualizar"></asp:Button></TD>
										</TR>
									</TABLE>
									<P align="right">
										<ASP:DataGrid id="grResAno" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
											CellPadding="3" CellSpacing="1">
											<FooterStyle cssclass="footer"></FooterStyle>
											<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
											<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
											<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
										</ASP:DataGrid></P>
								</TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plhKar" runat="server">
							<TR>
								<TD align="left">
                                <fieldset>
									<TABLE>
										<TR>
											<TD vAlign="top">Tipo Movimiento:
											</TD>
											<TD vAlign="top">
												<asp:RadioButtonList id="rblKarTMov" runat="server" ForeColor="Gray" RepeatDirection="Horizontal">
													<asp:ListItem Value="true" Selected="True">Todos</asp:ListItem>
													<asp:ListItem Value="false">Un tipo</asp:ListItem>
												</asp:RadioButtonList></TD>
											<TD vAlign="top" align="right">
												<asp:DropDownList id="ddlKarTMov" class="dmediano" runat="server"></asp:DropDownList></TD>
										</TR>
										<TR>
											<TD vAlign="top">Fecha:
											</TD>
											<TD vAlign="top" align="right">
												<asp:RadioButtonList id="rblKarFec" runat="server" ForeColor="Gray" RepeatDirection="Horizontal">
													<asp:ListItem Value="true" Selected="True">Un Año</asp:ListItem>
													<asp:ListItem Value="false">Un Año y un Mes</asp:ListItem>
													<asp:ListItem Value="2">2 Fechas</asp:ListItem>
												</asp:RadioButtonList></TD>
											<TD vAlign="top" align="right">Año:
												<asp:DropDownList id="ddlKarAno" class="dpequeno" runat="server"></asp:DropDownList>&nbsp;&nbsp; 
												Mes:
												<asp:DropDownList id="ddlKarMes" class="dpequeno" runat="server"></asp:DropDownList><BR>
												Fecha1:&nbsp;&nbsp;
												<asp:TextBox id="tbDate1" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox><IMG onmouseover="calendar1.style.visibility='visible'" onmouseout="calendar1.style.visibility='hidden'"
													src="../img/AMS.Icon.Calendar.gif" border="0">
												<TABLE id="calendar1" onmouseover="calendar1.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
													onmouseout="calendar1.style.visibility='hidden'">
													<TR>
														<TD>
															<asp:calendar BackColor="Beige" id="calDate1" runat="server" OnSelectionChanged="ChangeDate1" enableViewState="true"></asp:Calendar>
                                                        </TD>
													</TR>
												</TABLE>
												Fecha2:&nbsp;&nbsp;
												<asp:TextBox id="tbDate2" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox><img onmouseover="calendar2.style.visibility='visible'" onmouseout="calendar2.style.visibility='hidden'"
													src="../img/AMS.Icon.Calendar.gif" border="0">
												<TABLE id="calendar2" onmouseover="calendar2.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
													onmouseout="calendar2.style.visibility='hidden'">
													<TR>
														<TD>
															<asp:calendar BackColor="Beige" id="calDate2" runat="server" OnSelectionChanged="ChangeDate2" enableViewState="true"></asp:Calendar>
                                                        </TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
                                        <TR>
											<TD vAlign="top">Sede:
											</TD>
											<TD vAlign="top">
												<asp:RadioButtonList id="rbCedes" runat="server" ForeColor="Gray" RepeatDirection="Horizontal">
													<asp:ListItem Value="true" Selected="True">Todas las sedes</asp:ListItem>
													<asp:ListItem Value="false">Una sede</asp:ListItem>
												</asp:RadioButtonList>
                                            </TD>
											<TD vAlign="top" align="right">
												<asp:DropDownList id="ddlCedes" class="dmediano" runat="server"></asp:DropDownList>
                                            </TD>
										</TR>
									</TABLE>
                                    </fieldset>
									<asp:Button id="btnKarAct" onclick="BindGridView" runat="server" Text="Actualizar"></asp:Button>
									<P align="right">
										<ASP:DataGrid id="grKar"  Visible="false" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
											CellPadding="3" CellSpacing="1" >
											<FooterStyle cssclass="footer"></FooterStyle>
											<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
											<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
											<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternare"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
									    </ASP:DataGrid></P>  

                                       <P align="left">

                                        <asp:GridView id="GenKarPru" runat="server" AllowColumnReorder="True" OnPageIndexChanging="pruebaMethod" onsorting="GenKarPru_Sorting" BorderColor="lightgray" 
                                                      AutoGenerateColumns="True" CellPadding="4" AlternatingRowStyle-BackColor="LightCyan">                                                    
                                        </asp:GridView> </P>        
								</TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plhPed" runat="server">
							<TR>
								<TD align="right">
									<TABLE>
										<TR>
											<TD vAlign="top">Fecha:</TD>
											<TD vAlign="top">
												<asp:RadioButtonList id="rblPedFec" runat="server" ForeColor="Gray" RepeatDirection="Horizontal">
													<asp:ListItem Selected="True">Todas</asp:ListItem>
													<asp:ListItem>Un Año</asp:ListItem>
													<asp:ListItem>Un Año y un Mes</asp:ListItem>
												</asp:RadioButtonList></TD>
											<TD align="right">&nbsp;&nbsp; Año:
												<asp:DropDownList id="ddlPedAno" runat="server"></asp:DropDownList>&nbsp;&nbsp; 
												Mes:
												<asp:DropDownList id="ddlPedMes" runat="server"></asp:DropDownList></TD>
										</TR>
										<TR>
											<TD vAlign="top">Sede:</TD>
											<TD vAlign="top">
												<asp:RadioButtonList id="rblPedAlm" runat="server" ForeColor="Gray" RepeatDirection="Horizontal">
													<asp:ListItem Value="true" Selected="True">Todas</asp:ListItem>
													<asp:ListItem Value="false">Una</asp:ListItem>
												</asp:RadioButtonList></TD>
											<TD align="right">
												<asp:DropDownList id="ddlPedAlm"  runat="server"></asp:DropDownList></TD>
										</TR>
									</TABLE>
									<asp:Button id="Button1" onclick="GenPed" runat="server" Text="Actualizar"></asp:Button>
									<P align="left">
										<ASP:DataGrid id="grPed" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
											CellPadding="3" CellSpacing="1">
											<FooterStyle cssclass="footer"></FooterStyle>
											<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
											<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
											<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
										</ASP:DataGrid></P>
									<P align="left"></P>

                                 
								</TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plhEst" runat="server">
							<TR>
								<TD align="right">Año:
									<asp:DropDownList id="ddlEstAno" class="dpequeno" runat="server"></asp:DropDownList>&nbsp;&nbsp;
									<asp:Button id="btnEstAct" onclick="GenEst" runat="server" Text="Actualizar"></asp:Button>
									<P align="right">
										<ASP:DataGrid id="grEst" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
											CellPadding="3" CellSpacing="1">
											<FooterStyle cssclass="footer"></FooterStyle>
											<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
											<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
											<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
										</ASP:DataGrid></P>
								</TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plhDistrib" runat="server">
							<TR>
								<TD align="right">
									<P align="right">Año:
										<asp:DropDownList id="ddlAnoDistrib" class="dpequeno" runat="server"></asp:DropDownList>&nbsp;&nbsp;
										<asp:Button id="btnGenDist" onclick="GenDisAno" runat="server" Text="Actualizar"></asp:Button><BR>
										<ASP:DataGrid id="grDistrib" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="true"
											CellPadding="3" CellSpacing="1">
											<FooterStyle cssclass="footer"></FooterStyle>
											<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
											<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
											<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
										</ASP:DataGrid></P>
								</TD>
							</TR>
						</asp:PlaceHolder>
                        <asp:PlaceHolder id="plhApli" runat="server">
							<TR>
								<TD>
									<P align="left">
										<asp:DataGrid id="grAplic" runat="server" cssclass="datagrid" BorderWidth="2px" BorderStyle="Ridge" EnableViewState="False"
											CellPadding="3" CellSpacing="1">
											<FooterStyle cssclass="footer"></FooterStyle>
											<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
											<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
											<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
											<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
											<ItemStyle cssclass="item"></ItemStyle>
										</ASP:DataGrid></P>                                   
								</TD>
							</TR>
						</asp:PlaceHolder>
					</tbody>
				</table>
			</td>
		</tr>
	</tbody>
</table>
<p>

	<asp:Label id="lb" runat="server"></asp:Label>
</p>
<input type="hidden" runat="server" id="hdncosto"> <input type="hidden" runat="server" id="hdnvalor"> <input type="hidden" runat="server" id="hdnvalorIva">
<input type="hidden" runat="server" id="hdncant"> <input type="hidden" runat="server" id="hdnsust">
<input type="hidden" runat="server" id="hdnubi"> <input type="hidden" runat="server" id="hdnser">
<input type="hidden" runat="server" id="hdAplic"> 