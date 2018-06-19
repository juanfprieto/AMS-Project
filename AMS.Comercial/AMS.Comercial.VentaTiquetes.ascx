<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="c#" codebehind="AMS.Comercial.VentaTiquetes.ascx.cs" autoeventwireup="false" Inherits="AMS.Comercial.AMS_Comercial_VentaTiquetes" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%--<% if(Request.Browser.Browser != "IE"){ %><link href="../css/reset.css" type="text/css" rel="stylesheet" > <% } %>--%>

<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Comercial.GraficaBus.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
	var prctPrepago=<%=ViewState["PorcentajePrepago"]%>;
</script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td><b>Información del viaje:</b></td>
			<td><asp:button id="btnPlanillar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Nuevo Viaje"></asp:button></td>
			<td></td>
		</tr>
		<TR>
			<td><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAgencia_SelectedIndexChanged"></asp:dropdownlist></td>
			<TD></TD>
		</TR>
		<TR>
			<td><asp:label id="Label20" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Planilla :</asp:label></td>
			<td><asp:dropdownlist id="ddlFechaPlanilla" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
			<TD style="HEIGHT: 18px">&nbsp;</TD>
		</TR>
		<asp:placeholder id="pnlPlanilla" Runat="server" Visible="False">
			<TR>
				<TD style="HEIGHT: 18px">
					<asp:label id="Label12" Font-Size="XX-Small" Font-Bold="True" runat="server">Número de Planilla :</asp:label></TD>
				<TD style="HEIGHT: 18px">
					<asp:dropdownlist id="ddlPlanilla" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
				<TD style="HEIGHT: 18px">&nbsp;</TD>
			</TR>
			<asp:PlaceHolder id="pnlRutas" Runat="server" Visible="False">
  <TR>
					<TD align="center" colSpan="3">
						<asp:label id="lblTipoAgencia" Font-Size="Small" Font-Bold="True" runat="server"></asp:label></TD>
				</TR>
  <TR>
					<TD style="HEIGHT: 18px" vAlign="top">
						<asp:label id="Label3" Font-Size="XX-Small" Font-Bold="True" runat="server">Ruta Principal:</asp:label></TD>
					<TD style="HEIGHT: 18px">
						<asp:label id="lblRutaPrincipal" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label><!--<DIV id="divNumPuesto" style="BORDER-RIGHT: 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: 1px solid; DISPLAY: none; PADDING-LEFT: 2px; PADDING-BOTTOM: 2px; BORDER-LEFT: 1px solid; PADDING-TOP: 2px; BORDER-BOTTOM: 1px solid; POSITION: absolute; BACKGROUND-COLOR: white"></DIV>--><BR>
						<DIV id="divPrincipal"></DIV>
					</TD>
					<TD vAlign="bottom" align="left" rowSpan="11">
						<TABLE onclick="VerRecorrido();" align="left">
							<TR>
								<TD>
									<DIV id="divBus" style="width:150px;     box-shadow: -1px 10px 18px; BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; BORDER-BOTTOM: black thin solid"
										align="left"></DIV>
									<DIV id="divProc"></DIV>
									<DIV id="divSelVarios" style="DISPLAY: none"><INPUT id="txtPuestos" style="WIDTH: 30px" type="text" name="txtPuestos">&nbsp;&nbsp;<INPUT id="btnSelv" style="FONT-SIZE: xx-small" onclick="TomarPuestos();" type="button"
											value="tomar"></DIV>
								</TD>
								<TD vAlign="bottom" align="center" width="200" rowSpan="11">
									<DIV id="divInfoRuta" align="left"></DIV>
									<H2 align="left">
										<DIV id="divNumPuesto">&nbsp;</DIV>
									</H2>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR></TR>
<asp:PlaceHolder id="pnlCiudadOrigen" Runat="server">
					<TR>
						<TD style="HEIGHT: 18px">
							<asp:label id="Label2" Font-Size="XX-Small" Font-Bold="True" runat="server">Ciudad origen :</asp:label></TD>
						<TD style="HEIGHT: 18px">
							<asp:label id="lblOrigen" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label></TD>
					</TR>
				</asp:PlaceHolder>
<asp:PlaceHolder id="PnlConduce" Runat="server">
					<TR>
						<TD style="HEIGHT: 18px" colSpan="2">
							<asp:button id="btnBoleta" Text="Imprimir Boleta" Runat="server" Font-Size="XX-Small"
								Font-Bold="True" Width="125px"></asp:button></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 18px" colSpan="2">
							<asp:button id="btnConduce" Text="Imprimir Anticipo Conduce" Runat="server" Font-Size="XX-Small"
								Font-Bold="True" Width="125px"></asp:button></TD>
						<TD></TD>
					</TR>
				</asp:PlaceHolder>
<asp:PlaceHolder id="pnlBus" Runat="server">
					<TR>
						<TD style="HEIGHT: 18px" vAlign="top">
							<asp:label id="Label5" Font-Size="XX-Small" Font-Bold="True" runat="server">Ciudad destino :</asp:label></TD>
						<TD style="HEIGHT: 18px" vAlign="top">
							<asp:dropdownlist id="ddlDestino" Font-Size="XX-Small" runat="server"></asp:dropdownlist>&nbsp;<INPUT id="btnSel" style="FONT-SIZE: xx-small" onclick="CambiaRuta();" type="button" value="actualizar" class="noEspera">
							<DIV id="divDestino"></DIV>
						</TD>
					</TR>
					<TR>
						<TD>&nbsp;</TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 18px" vAlign="top">
							<asp:label id="Label14" Font-Size="XX-Small" Font-Bold="True" runat="server">Numero Tiquete :</asp:label></TD>
						<TD style="HEIGHT: 18px" vAlign="top">
							<DIV id="divTiquete">
								<asp:textbox id=txtTiquete Font-Size="XX-Small" runat="server" Width="100px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>">
								</asp:textbox></DIV>
							<DIV id="divTiqueteA" style="FONT-SIZE: medium"></DIV>
						</TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 18px">
							<asp:label id="Label18" Font-Size="XX-Small" Font-Bold="True" runat="server">Prepago :</asp:label></TD>
						<TD style="HEIGHT: 18px">
							<asp:CheckBox id="chkPrepago" Runat="server"></asp:CheckBox></TD>
					</TR>
					<TR>
						<TD>&nbsp;</TD>
					</TR>
					<TR>
						<TD vAlign="top" colSpan="2">
							<DIV id="divCliente">
								<TABLE>
									<TR>
										<TD colSpan="2"><B>Información del Cliente:</B></TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label13" Font-Size="XX-Small" Font-Bold="True" runat="server">Tipo Documento :</asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:dropdownlist id="txtNITc" Font-Size="XX-Small" runat="server" Visible="True"></asp:dropdownlist></TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label6" Font-Size="XX-Small" Font-Bold="True" runat="server">Documento :</asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtNIT" ondblclick="MostrarComprador(this);" Font-Size="XX-Small" runat="server"
												Width="80px" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label7" Font-Size="XX-Small" Font-Bold="True" runat="server">Nombres :</asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtNITa" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD vAlign="top">
											<asp:label id="Label8" Font-Size="XX-Small" Font-Bold="True" runat="server">Apellidos :</asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtNITb" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label26" Font-Size="XX-Small" Font-Bold="True" runat="server">Telefono :</asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtNITd" Font-Size="XX-Small" runat="server" Width="112px" MaxLength="15" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label10" Font-Size="XX-Small" Font-Bold="True" runat="server">Precio : </asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtPrecio" onkeyup="NumericMask(this);grBus.Totales();" Font-Size="XX-Small"
												runat="server" Width="80px" MaxLength="7" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label9" Font-Size="XX-Small" Font-Bold="True" runat="server">Cantidad : </asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<DIV id="divCantidad"></DIV>
										</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label11" Font-Size="XX-Small" Font-Bold="True" runat="server">Total : </asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<DIV id="divTotal"></DIV>
										</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top" colSpan="2"><BR>
											<BR>
										</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label19" Font-Size="XX-Small" Font-Bold="True" runat="server">Total Puestos : </asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<DIV id="divTotalPuestos"></DIV>
										</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label17" Font-Size="XX-Small" Font-Bold="True" runat="server">Puestos Libres : </asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<DIV id="divPuestosLibres"></DIV>
										</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label1" Font-Size="XX-Small" Font-Bold="True" runat="server">Puestos Vendidos : </asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<DIV id="divPuestosVendidos"></DIV>
										</TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 18px" vAlign="top">
											<asp:label id="Label15" Font-Size="XX-Small" Font-Bold="True" runat="server">Total Vendido : </asp:label></TD>
										<TD style="HEIGHT: 18px" vAlign="top">
											<DIV id="divTotalVendido"></DIV>
										</TD>
									</TR>
								</TABLE>
							</DIV>
						</TD>
					</TR>
					<TR>
						<TD align="center">
							<asp:button id="btnGuardar" Text="Comprar" Runat="server" Font-Size="XX-Small" Font-Bold="True"
								Width="126px" Height="30px" class="noEspera"></asp:button></TD>
					</TR>
					<TR>
						<TD>&nbsp;</TD>
					</TR>
					<TR>
						<TD align="center">
							<asp:button id="btnPreDespacho" Text="Pre-Despachar" Runat="server" Font-Size="XX-Small" Font-Bold="True"
								Width="126px" Height="30px"></asp:button></TD>
						<TD>&nbsp;</TD>
					</TR>
					<TR>
						<TD>&nbsp;</TD>
					</TR>
					<SCRIPT language="javascript" type="text/javascript" >
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
		
        if(ddlDestino.value.length==0)
		{
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
		//txtPrecio.focus();
	}

	function CambiaRuta_Callback(response){
		var respuesta=response.value;
		var params=respuesta.split('@');
		if(params.length==1){
			alert(params[0]);
			return;
		}
		strRecorrido=params[16];
		grBus=new GraficadorBus(divBus,params[0],params[1],'AMS.Web.index.aspx?process=Comercial.ProcesoTiquete','../img/','',divCantidad,txtPrecio,divTotal,0,params[15],divNumPuesto,40,params[16],divInfoRuta);
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
		window.scrollTo(0, document.body.scrollHeight);
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
			txtNITd.value="";
			ddlDestino.focus();
			chkPrepago.checked=false;
			cambiaTipo("V");
			CambiaRuta();
			if(params[3]=="V" && params[4]=="False"){
				window.open('../aspx/AMS.Comercial.Tiquete.aspx?tq='+params[2], 'TIQUETE'+params[2], "width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");
				/*var wtq=window.open('../papeleria/TIQ_'+params[2]+'.txt', 'TIQUETE'+params[2], "width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");
				wtq.print();
				wtq.close();*/}
		}
	}
	function MostrarComprador(obj){
		var sqlDsp='SELECT MPAS_NIT AS NIT,MPAS_NOMBRES AS NOMBRE, MPAS_APELLIDOS AS APELLIDOS,\'C\' as C,MPAS_TELEFONO as telefono FROM DBXSCHEMA.MPASAJERO';
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
				</asp:PlaceHolder></asp:PlaceHolder>
		</asp:placeholder></table>
</DIV>
<DIV align="center"><asp:placeholder id="pnlDespacho" Runat="server" Visible="false">
<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 71px; HEIGHT: 18px">
					<asp:label id="Label23" Font-Size="XX-Small" Font-Bold="True" runat="server">Nro Viaje :</asp:label></TD>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="lblNroViaje" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 71px; HEIGHT: 18px">
					<asp:label id="Label22" Font-Size="XX-Small" Font-Bold="True" runat="server">Planilla :</asp:label></TD>
				<TD style="WIDTH: 390px; HEIGHT: 18px">
					<asp:label id="lblPlanila" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 71px; HEIGHT: 18px">
					<asp:label id="Label16" Font-Size="XX-Small" Font-Bold="True" runat="server">Placa :</asp:label></TD>
				<TD style="WIDTH: 390px; HEIGHT: 18px">
					<asp:label id="lblPlacaOriginal" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 71px; HEIGHT: 18px">
					<asp:label id="Label24" Font-Size="XX-Small" Font-Bold="True" runat="server">Nro bus :</asp:label></TD>
				<TD style="WIDTH: 390px; HEIGHT: 18px">
					<asp:label id="lblNroBus" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
		</TABLE>
<asp:datagrid id="dgrAsignacion" runat="server" Width="773px" PageSize="20" AllowPaging="True"
			ShowFooter="False" AutoGenerateColumns="False">
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
		</asp:datagrid></TD></TR></TR></TR>
<TABLE style="WIDTH: 773px" align="center" >
			<TR>
				<TD style="WIDTH: 129px; HEIGHT: 19px" vAlign="top">
					<asp:label id="lblAnticipoViaje" Font-Size="XX-Small" Font-Bold="True" runat="server">Anticipo gastos de Viaje :</asp:label></TD>
				<TD style="WIDTH: 265px; HEIGHT: 19px" vAlign="top">
					<asp:textbox id="txtAnticipoViaje" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
						Width="80px" MaxLength="8" ReadOnly="False" Height="18px"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 129px; HEIGHT: 59px" vAlign="top">
					<asp:label id="Label25" Font-Size="XX-Small" Font-Bold="True" runat="server">Observacion :</asp:label></TD>
				<TD style="WIDTH: 265px; HEIGHT: 59px" vAlign="top">
					<asp:textbox id="txtObservacion" Font-Size="XX-Small" runat="server" Width="520px" ReadOnly="False"
						Height="48px" TextMode="MultiLine"></asp:textbox></TD>
			</TR>
		</TABLE>
<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 129px; HEIGHT: 21px" align="left">
					<asp:button id="btnAsociar" Text="Asignar/Desasignar a Planilla" Runat="server" Font-Size="XX-Small"
						Font-Bold="True" Width="154px"></asp:button></TD>
				<TD style="WIDTH: 105px; HEIGHT: 15px" align="center">
					<asp:button id="btnVerDespacho" Text="VerDespacho" Runat="server" Font-Size="XX-Small" Font-Bold="True"
						Width="80px" CommandName="VerDespacho"></asp:button></TD>
				<TD style="WIDTH: 100px; HEIGHT: 20px" align="left">
					<asp:button id="btnDespachar" Text="Despachar" Runat="server" Font-Size="XX-Small" Font-Bold="True"
						CommandName="Despachar"></asp:button></TD>
			</TR>
			<asp:label id="lblError" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label>&nbsp;</TD></TR></TABLE></asp:placeholder></DIV>