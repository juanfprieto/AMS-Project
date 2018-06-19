<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.VentaTiquetes.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_VentaTiquetes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Comercial.GraficaBusB.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
	var prctPrepago=<%=ViewState["PorcentajePrepago"]%>;
</script>
<OBJECT id="prtTiquete" height="1" width="1" classid="CLSID:7AF0226B-7A49-4253-BA78-326BA0FA4497"
	VIEWASTEXT>
</OBJECT>
<table>
	<tr>
		<td>
			<DIV align="left">
				<table align="left">
					<tr>
						<td>
							<table align="left">
								<tr>
									<td><b>Viaje:</b></td>
									<td><asp:button id="btnPlanillar" Text="Nuevo Viaje" Runat="server" Font-Size="XX-Small" Font-Bold="True"
											onclick="btnPlanillar_Click"></asp:button></td>
								</tr>
								<TR>
									<td><b>Agencia:</b></td>
									<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True" onselectedindexchanged="ddlAgencia_SelectedIndexChanged"></asp:dropdownlist></td>
								</TR>
								<tr>
									<TD colspan="2"><asp:label id="lblTipoAgencia" Font-Size="Small" Font-Bold="True" runat="server"></asp:label></TD>
								</tr>
								<TR>
									<td colspan="1"><b>Fecha:</b></td>
									<td><asp:dropdownlist id="ddlFechaPlanilla" Font-Size="XX-Small" runat="server" AutoPostBack="True" onselectedindexchanged="ddlFechaPlanilla_SelectedIndexChanged"></asp:dropdownlist></td>
								</TR>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<table align="left">
								<asp:PlaceHolder id="pnlPlanilla" Runat="server" Visible="False">
									<TR>
										<TD><b>Planilla:</b></TD>
										<td><asp:dropdownlist id="ddlPlanilla" Font-Size="XX-Small" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPlanilla_SelectedIndexChanged"></asp:dropdownlist></td>
									</TR>
									<asp:PlaceHolder id="pnlRutas" Runat="server" Visible="False">
										<TR>
											<TD vAlign="top">
												<b>Ruta:</b></TD>
											<td><asp:label id="lblRutaPrincipal" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
												<BR>
												<DIV id="divPrincipal"></DIV>
											</td>
										</TR>
										<asp:PlaceHolder id="pnlCiudadOrigen" Runat="server">
											<TR>
												<TD><B>Origen:</B></TD>
												<TD>
													<asp:label id="lblOrigen" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
											</TR>
										</asp:PlaceHolder>
										<asp:PlaceHolder id="PnlConduce" Runat="server" Visible="False">
											<TR>
												<TD><B>Conduce:</B></TD>
												<TD>
													<asp:button id="btnConduce" onclick="btnConduce_Click" Font-Bold="True" Font-Size="XX-Small"
														Runat="server" Text="Boleta Conduce" Width="99px"></asp:button></TD>
											</TR>
										</asp:PlaceHolder>
										<asp:PlaceHolder id="pnlBus" Runat="server">
											<TR>
												<TD vAlign="top"><b>Destino:</b></TD>
												<td><asp:dropdownlist id="ddlDestino" Font-Size="XX-Small" runat="server"></asp:dropdownlist>&nbsp;<INPUT id="btnSel" style="FONT-SIZE: xx-small" onclick="CambiaRuta();" type="button" value="actualizar">
													<DIV id="divDestino"></DIV>
												</td>
											</TR>
											<TR>
												<TD vAlign="top"><b>Tiquete:</b></TD>
												<td><DIV id="divTiquete"><asp:textbox id=txtTiquete Font-Size="XX-Small" runat="server" Width="100px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>"></asp:textbox></DIV>
													<DIV id="divTiqueteA" style="FONT-SIZE: medium"></DIV><br>
													<table align="left" onclick="VerRecorrido();">
														<tr>
															<td>
																<DIV id="divBus" align=left style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; BORDER-BOTTOM: black thin solid"></DIV>
																<DIV id="divProc"></DIV>
																<DIV id="divSelVarios" style="DISPLAY: none"><INPUT id="txtPuestos" style="WIDTH: 30px" type="text" name="txtPuestos">&nbsp;&nbsp;<INPUT id="btnSelv" style="FONT-SIZE: xx-small" onclick="TomarPuestos();" type="button" value="tomar"></DIV>
															</td>
															<TD vAlign="bottom" align="center" rowSpan="11" width="200px">
																<DIV id="divInfoRuta" align="left"></DIV>
																<h2 align=left><DIV id="divNumPuesto">&nbsp;</DIV></h2>
															</TD>
														</tr>
													</table>
												</td>
											</TR>
											<TR>
												<TD>
													<b>Prepago:</b></TD>
												<td><asp:CheckBox id="chkPrepago" Runat="server"></asp:CheckBox></td>
											</TR>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<table align="left">
								<TR>
									<TD vAlign="top" colspan="2">
										<DIV id="divCliente">
											<TABLE>
												<TR>
													<TD colSpan="3"><B>Información del Cliente:</B></TD>
												</TR>
												<TR>
													<TD vAlign="top" align="left">
														<b>Documento:</b></TD>
													<TD vAlign="top" colSpan="2" align="left">
														<asp:dropdownlist id="txtNITc" Font-Size="XX-Small" runat="server" Visible="True"></asp:dropdownlist></TD>
												</TR>
												<TR>
													<TD vAlign="top" align="left"><b>Numero:</b></TD>
													<TD vAlign="top" colSpan="2" align="left">
														<asp:textbox id="txtNIT" Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="False"></asp:textbox><INPUT id="btnTN" style="FONT-SIZE: xx-small" onclick="TraerNIT();" type="button" value=">"></TD>
												</TR>
												<TR>
													<TD vAlign="top" align="left"><b>Nombre:</b></TD>
													<TD vAlign="top" colSpan="2">
														<asp:textbox id="txtNITa" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60" ReadOnly="False"></asp:textbox></TD>
												</TR>
												<TR>
													<TD vAlign="top" align="left"><b>Apellido:</b></TD>
													<TD vAlign="top" colSpan="2" align="left"><asp:textbox id="txtNITb" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60" ReadOnly="False"></asp:textbox></TD>
												</TR>
												<TR>
													<TD vAlign="top" align="left"><b>Telefono :</b></TD>
													<TD vAlign="top" colSpan="2" align="left"><asp:textbox id="txtNITd" Font-Size="XX-Small" runat="server" Width="112px" MaxLength="15" ReadOnly="False"></asp:textbox></TD>
												</TR>
												<TR>
													<TD vAlign="top" align="left"><b>Precio : </b>
													</TD>
													<TD vAlign="top" align="left" colspan="2">
														<asp:textbox id="txtPrecio" onkeyup="NumericMaskE(this,event);grBus.Totales();" Font-Size="XX-Small"
															runat="server" Width="80px" MaxLength="7" ReadOnly="False"></asp:textbox></TD>
												</TR>
												<TR>
													<TD vAlign="top" align="left"><b>Cantidad:</b></TD>
													<TD vAlign="top" align="left">
														<DIV id="divCantidad"></DIV>
													</TD>
												</TR>
												<TR>
													<TD vAlign="top"><b>Total:</b></TD>
													<TD vAlign="top">
														<DIV id="divTotal"></DIV>
													</TD>
												</TR>
												<TR>
													<TD vAlign="top"><b>Puestos:</b></TD>
													<TD vAlign="top">
														<DIV id="divTotalPuestos"></DIV>
													</TD>
												</TR>
												<TR>
													<TD vAlign="top">
														<b>Libres:</b></TD>
													<TD vAlign="top">
														<DIV id="divPuestosLibres"></DIV>
													</TD>
												</TR>
												<TR>
													<TD vAlign="top">
														<b>Vendidos:</b></TD>
													<TD vAlign="top">
														<DIV id="divPuestosVendidos"></DIV>
													</TD>
												</TR>
												<TR>
													<TD vAlign="top">
														<b>Ventas:</b></TD>
													<TD vAlign="top">
														<DIV id="divTotalVendido"></DIV>
													</TD>
												</TR>
											</TABLE>
										</DIV>
									</TD>
								</TR>
								<TR>
									<TD align="center">
										<asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Comprar"
											Width="126px" Height="30px"></asp:button></TD>
								</TR>
								<TR>
									<TD align="center">
										<asp:button id="btnPreDespacho" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Pre-Despachar"
											Width="126px" Height="30px" onclick="btnPreDespacho_Click"></asp:button></TD>
								</TR>
								<SCRIPT language="javascript">
	var grBus;
	var rutaAct;
	var rutaStr;
	var tiqueteActM="";
	var tiqueteActV="";
	var ddlDestino=document.getElementById("<%=ddlDestino.ClientID%>");
	var txtNIT=document.getElementById("<%=txtNIT.ClientID%>");
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	var ddlPlanilla=document.getElementById("<%=ddlPlanilla.ClientID%>");
	var txtPrecio=document.getElementById("<%=txtPrecio.ClientID%>");
	var txtTiqueteM=document.getElementById("<%=txtTiquete.ClientID%>");
	var txtNITa=document.getElementById("<%=txtNITa.ClientID%>");
	var txtNITb=document.getElementById("<%=txtNITb.ClientID%>");
	var txtNITd=document.getElementById("<%=txtNITd.ClientID%>");
	var txtNITc=document.getElementById("<%=txtNITc.ClientID%>");
	var txtPuestos=document.getElementById("txtPuestos");
	var chkPrepago=document.getElementById("<%=chkPrepago.ClientID%>");
	var strRecorrido="";

	function Prepago(chkV){
		if(grBus!=null && document.getElementById('divTiquete')!=null){
			if(chkPrepago.checked){
				grBus.descuento=prctPrepago;
				txtTiqueteM.value="";
				divTiquete.style.display = 'block';
				divTiqueteA.innerHTML="";}
			else{
				grBus.descuento=0;
				txtTiqueteM.value=tiqueteActM;
				divTiqueteA.innerHTML=tiqueteActV;
				divTiquete.style.display = 'none';
				cambiaTipo("V");
			}
			grBus.Totales();
		}
		else{
			alert('Seleccione el destino.');
			chkPrepago.checked=false;
		}
	}

	function CambiaRuta(){
		var planilla=ddlPlanilla.value;
		var tipo="V";
		var agencia=ddlAgencia.value;
		//chkPrepago.checked=false;
		if(ddlDestino.value.length==0){
			divDestino.innerHTML="";
			divBus.innerHTML="";
			divCliente.style.display='none';
			divSelVarios.style.display='none';
			ddlDestino.focus();
			return;
		}
		rutaStr=ddlDestino.options[ddlDestino.selectedIndex].text;
		rutaAct=ddlDestino.value;
		divDestino.innerHTML="Cargando ruta...";
		if(grBus!=null)parmsBus=grBus.Parametros();
		else parmsBus="";
		divBus.innerHTML="....cargando...."
		tiqueteActM=tiqueteActV='';
		divTiqueteA.innerHTML="";
		AMS_Comercial_VentaTiquetes.CargarPuestos(planilla,rutaAct,tipo,agencia,parmsBus,CambiaRuta_Callback);
		divCliente.style.display = 'block';
		divBus.focus();
	}

	function CambiaRuta_Callback(response){
		var respuesta=response.value;
		var params=respuesta.split('@');
		if(params.length==1){
			alert(params[0]);
			return;
		}
		strRecorrido=params[16];
		grBus=new GraficadorBus(divBus,params[0],params[1],'AMS.Web.index.aspx?process=Comercial.ProcesoTiquete','../img/','',divCantidad,txtPrecio,divTotal,0,params[15],divNumPuesto,30,params[16],divInfoRuta);
		grBus.Actualizar();
		divDestino.innerHTML=rutaStr;
		divDestino.innerHTML+="<br>["+rutaAct+"]";
		divPrincipal.innerHTML="Fecha Salida:   "+params[2]+"<br>Hora Salida:   "+params[3]+"<br>Conductor:   "+params[4]+"<br>Relevador:   "+params[5]+"<br>Placa:   "+params[6]+"<br>No. Bus:  "+params[7]+"<br><br>";
		strP=params[8];
		//txtTiqueteM.value=params[9];
		txtPrecio.value=params[8];
		tiqueteActM=params[9];
		tiqueteActV=params[10];
		divPuestosVendidos.innerHTML=params[11];
		divTotalVendido.innerHTML="$"+params[12];
		divPuestosLibres.innerHTML=params[13];
		divTotalPuestos.innerHTML=params[14];
		divInfoRuta.innerHTML=params[16];
		if(chkPrepago.checked)cambiaTipo("M");
		else cambiaTipo("V");
		grBus.Totales();
		divSelVarios.style.display='block';
		divBus.focus();
	}

	function Comprar(){
		var precioS=txtPrecio.value.replace(/\,/g,'');
		var nit=txtNIT.value;
		var nombre=txtNITa.value;
		var apellido=txtNITb.value;
		var telefono=txtNITd.value;
		var tnit=txtNITc.value;
		var tipo="V";
		var ruta=ddlDestino.value;
		var planilla=ddlPlanilla.value;
		var agencia=ddlAgencia.value;
		var tiquete;
		var prctP=0;
		tiquete=tiqueteActV;
		//Prepago
		if(chkPrepago.checked){
			prctP=prctPrepago;
			tiquete=txtTiqueteM.value;
			if(grBus.Cantidad>1){
				alert('Solo puede registrar un tiquete prepago a la vez.');
				return(false);
			}
		}
		if(ruta.length==0){
			alert('Seleccione el destino.');
			return(false);
		}
		if(tiquete.length==0 || isNaN(tiquete)){
			alert('Número de tiquete no válido.');
			return(false);
		}
		if(precioS.length==0 || isNaN(precioS)){
			alert('Precio no válido.');
			return(false);
		}
		if(nit.length>0){
			if(txtNITc.value=="N"){
				if(nombre.length==0){
					alert('Debe dar el nombre del comprador si ingresa su nit.');
					return(false);}}
			else{
				if(nombre.length==0||apellido.length==0){
					alert('Debe dar el nombre y el apellido del comprador si ingresa su documento.');
					return(false);}}
	   		if(telefono.length==0) telefono="ND";
		}
		var precio=parseInt(precioS);
		var param=grBus.Parametros();
		if(param.length==0){
			if(grBus.Lleno()){
				alert('No hay puestos libres.');
			}
			else{
				alert('No seleccionó ningún puesto.');
			}
			return(false);}
		divProc.innerHTML="<BR>Registrando compra...";
		AMS_Comercial_VentaTiquetes.Comprar(tiquete,agencia,planilla,tipo,ruta,nit,tnit,nombre,apellido,telefono,precio,param,chkPrepago.checked,prctP,Comprar_Callback);
		return(false);
	}
	function Comprar_Callback(response){
		var respuesta=response.value;
		var params=respuesta.split('@');
		txtPuestos.value='';
		if(params[0]=="0"){
			alert(params[1]);
			divProc.innerHTML="<BR>"+params[1];
			CambiaRuta();}
		else{
			alert(params[1]);
			divBus.innerHTML="";
			divProc.innerHTML="";
			tiqueteActM="";
			tiqueteActV="";
			txtPrecio.value="";
			txtNIT.value="";
			txtNITa.value="";
			txtNITb.value="";
			ddlDestino.focus();
			chkPrepago.checked=false;
			cambiaTipo("V");
			CambiaRuta();
			if(params[3]=="V" && params[4]=="False"){
				AMS_Comercial_VentaTiquetes.GenerarTiquete(params[2],GenerarTiquete_CallBack);
			}
		}
	}
	function GenerarTiquete_CallBack(response){
		var prtTiquete=document.getElementById("prtTiquete");
		prtTiquete.Imprimir(response.value.replace(/\\n/g, '\n'));
	}

	function MostrarComprador(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES AS NOMBRE, MNIT.MNIT_APELLIDOS AS APELLIDOS, MNIT.TNIT_TIPONIT AS TIPO_DOC,mnit.MNIT_TELEFONO as telefono from DBXSCHEMA.MNIT MNIT where MNIT.TNIT_TIPONIT = \''+txtNITc.value.replace('|','')+'\' AND TSOC_SOCIEDAD in (\'U\',\'P\',\'N\') ';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}

	function TraerNIT(){
		AMS_Comercial_VentaTiquetes.TraaerNIT(txtNIT.value,TraerNIT_Callback);
		return(false);
	}

	function TraerNIT_Callback(response){
		var respuesta=response.value;
		var params=respuesta.split('|');
		txtNITa.value=params[0];
		txtNITb.value=params[1];
		txtNITd.value=params[2];
		//if(params[0].length>0)txtPrecio.focus();
	}

	function KeyDownHandler(){
		if(event.keyCode == 13){
			CambiaRuta();
		}
	}

	function KeyDownHandlerNIT(){
		if(event.keyCode==13 && txtNIT.value.length>0)TraerNIT();
	}
	//Cambia manual o virtual
	function cambiaTipo(tipo){
		if(chkPrepago.checked)return;
		if(document.getElementById('divTiquete')!=null)
			if(tipo!="V"){
				divTiquete.style.display = 'block';
				divTiqueteA.innerHTML="";}
			else{
				divTiquete.style.display = 'none';
				divTiqueteA.innerHTML=tiqueteActV;}
	}
	function SelPrecio(){
		txtPrecio.focus();
	}
	function VerRecorrido(){
		divInfoRuta.innerHTML=strRecorrido;
	}
	//Seleccionar varios puestos
	function TomarPuestos(){
		var puestos=parseInt(txtPuestos.value);
		if(isNaN(puestos)){
			alert('Numero de puestos no valido.');
			return;
		}
		grBus.Tomar(puestos);
		
	}
	function InfoVenta(f,c){
		var planilla=ddlPlanilla.value;
		divInfoRuta.innerHTML="Cargando datos...";
		AMS_Comercial_VentaTiquetes.DatosVenta(planilla,f,c,InfoVenta_Callback)
	}
	function InfoVenta_Callback(response){
		divInfoRuta.innerHTML=response.value;
	}
	<%=focus%>
								</SCRIPT>
								</asp:PlaceHolder></asp:PlaceHolder></asp:PlaceHolder></table>
						</td>
					</tr>
				</table>
			</DIV>
		</td>
	<tr>
	<tr>
		<td>
			<DIV align="center">
				<asp:PlaceHolder id="pnlDespacho" Runat="server" Visible="false">
					<TABLE>
						<TR>
							<TD>
								<TABLE align="center">
									<TR>
										<TD>
											<asp:label id="Label23" Font-Bold="True" Font-Size="XX-Small" runat="server">No. Viaje :</asp:label>&nbsp;
											<asp:label id="lblNroViaje" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
									</TR>
									<TR>
										<TD>
											<asp:label id="Label22" Font-Bold="True" Font-Size="XX-Small" runat="server">Planilla :</asp:label>&nbsp;
											<asp:label id="lblPlanila" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
									</TR>
									<TR>
										<TD>
											<asp:label id="Label16" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa :</asp:label>&nbsp;
											<asp:label id="lblPlacaOriginal" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
									</TR>
									<TR>
										<TD>
											<asp:label id="Label24" Font-Bold="True" Font-Size="XX-Small" runat="server">Nro bus :</asp:label>&nbsp;
											<asp:label id="lblNroBus" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
						<TR>
							<TD>
								<asp:datagrid id="dgrAsignacion" runat="server" AutoGenerateColumns="False" ShowFooter="False"
									AllowPaging="True" PageSize="20">
									<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
									<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
									<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
									<Columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:CheckBox id="chkAsignacion" Font-Size="XX-Small" Runat=server Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ASIGNACION")) %>'>
												</asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="TIPO" HeaderText="TIPO"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUMERO" HeaderText="DCMNTO"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="PLACA_DOCUMENTO" HeaderText="PlacaDoc"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="RUTA_DOCUMENTO" HeaderText="RutaDoc"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_DOCUMENTO" DataFormatString="{0:yyyy-MM-dd}"
											HeaderText="FechaDoc"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NIT_EMISOR" HeaderText="NIT ENVIA"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_EMISOR" HeaderText="NOMBRE ENVIA"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NIT_DESTINATARIO" HeaderText="NIT RECEPTOR"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DESCRIPCION_CONTENIDO" HeaderText="DESCRIPCION"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="COSTO_DOCUMENTO"
											HeaderText="VALOR COSTO"></asp:BoundColumn>
										<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_DOCUMENTO"
											HeaderText="VALOR BASE"></asp:BoundColumn>
									</Columns>
									<PagerStyle Mode="NumericPages"></PagerStyle>
								</asp:datagrid></TD>
						</TR>
						<TR>
							<TD>
								<TABLE align="center">
									<TR>
										<TD vAlign="top">
											<asp:label id="Label25" Font-Bold="True" Font-Size="XX-Small" runat="server">Observacion:</asp:label></TD>
										<TD vAlign="top">
											<asp:textbox id="txtObservacion" Font-Size="XX-Small" runat="server" Width="520px" ReadOnly="False"
												Height="48px" TextMode="MultiLine"></asp:textbox></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
						<TR>
							<TD>
								<TABLE align="center">
									<TR>
										<TD align="center">
											<asp:button id="btnAsociar" onclick="btnAsociar_Click" Font-Bold="True" Font-Size="XX-Small"
												Runat="server" Text="Asignar/Desasignar a Planilla" Width="200px"></asp:button><BR>
											<asp:button id="btnVerDespacho" onclick="btnVerDespacho_Click" Font-Bold="True" Font-Size="XX-Small"
												Runat="server" Text="VerDespacho" Width="80px" CommandName="VerDespacho"></asp:button><BR>
											<asp:button id="btnDespachar" onclick="btnDespachar_Click" Font-Bold="True" Font-Size="XX-Small"
												Runat="server" Text="Despachar" CommandName="Despachar"></asp:button></TD>
									</TR>
									<TR>
										<TD>
											<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</asp:PlaceHolder>
			</DIV>
		</td>
	<tr>
	</tr>
</table>
