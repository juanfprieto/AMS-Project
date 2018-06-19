<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.VentaTiquetesPrepago.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_VentaTiquetesPrepago" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language='javascript' src='../js/AMS.Comercial.GraficaBus.js' type='text/javascript'></script>
<!--
<script language='javascript' type='text/javascript'>
	var prctPrepago=<%=ViewState["PorcentajePrepago"]%>;
</script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información del viaje:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
			<TD>&nbsp;</TD>
		</tr>
		<asp:panel id="pnlPlanilla" Visible="False" Runat="server">
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Número de Planilla :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:dropdownlist id="ddlPlanilla" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
				<TD style="HEIGHT: 18px">&nbsp;</TD>
			</TR>
			<asp:Panel id="pnlRutas" Runat="server" Visible="False">
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
						<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Ruta Principal:</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px">
						<asp:label id="lblRutaPrincipal" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
						<DIV id="divPrincipal"></DIV>
					</TD>
					<TD rowSpan="8">
						<DIV id="divBus"></DIV>
						<DIV id="divProc"></DIV>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 18px">
						<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Ciudad origen :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px">
						<asp:label id="lblOrigen" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
					<TD>&nbsp;</TD>
				</TR>
				<asp:Panel id="pnlBus" Runat="server">
					<TR>
						<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
							<asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Ciudad destino :</asp:label></TD>
						<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
							<asp:dropdownlist id="ddlDestino" Font-Size="XX-Small" runat="server" Width="150px"></asp:dropdownlist>&nbsp;<INPUT id="btnSel" style="FONT-SIZE: xx-small" onclick="CambiaRuta();" type="button" value="seleccionar">
							<DIV id="divDestino"></DIV>
						</TD>
					</TR>
					<TR>
						<TD colSpan="2">&nbsp;</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 154px; HEIGHT: 18px">
							<asp:label id="Label14" Font-Bold="True" Font-Size="XX-Small" runat="server">Numero Tiquete :</asp:label></TD>
						<TD style="WIDTH: 386px; HEIGHT: 18px">
							<asp:textbox id=txtTiquete Font-Size="XX-Small" runat="server" Width="100px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>">
							</asp:textbox></TD>
						<TD>&nbsp;</TD>
					</TR>
					<TR>
						<TD colSpan="2">&nbsp;</TD>
					</TR>
					<TR>
						<TD vAlign="top" colSpan="2">
							<DIV id="divCliente">
								<TABLE>
									<TR>
										<TD colSpan="2"><B>Información del Cliente:</B></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
											<asp:label id="Label13" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo Documento :</asp:label></TD>
										<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
											<asp:dropdownlist id="txtNITc" Font-Size="XX-Small" runat="server" Visible="True"></asp:dropdownlist></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
											<asp:label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Documento :</asp:label></TD>
										<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtNIT" ondblclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES AS NOMBRE, MNIT.MNIT_APELLIDOS AS APELLIDOS, MNIT.TNIT_TIPONIT AS TIPO from DBXSCHEMA.MNIT MNIT', new Array(),1)"
												Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
											<asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Nombre :</asp:label></TD>
										<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtNITa" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 154px" vAlign="top">
											<asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Apellido :</asp:label></TD>
										<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtNITb" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
											<asp:label id="Label10" Font-Bold="True" Font-Size="XX-Small" runat="server">Precio Tiquete : </asp:label></TD>
										<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
											<asp:textbox id="txtPrecio" onkeyup="NumericMask(this);grBus.Totales();" Font-Size="XX-Small"
												runat="server" Width="80px" MaxLength="7" ReadOnly="False"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
											<asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Cantidad : </asp:label></TD>
										<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
											<DIV id="divCantidad"></DIV>
										</TD>
									</TR>
									<TR>
										<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
											<asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Total : </asp:label></TD>
										<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
											<DIV id="divTotal"></DIV>
										</TD>
									</TR>
								</TABLE>
							</DIV>
						</TD>
					</TR>
					<TR>
						<TD align="left">
							<asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Comprar"></asp:button></TD>
					</TR>
					<SCRIPT language="javascript">
var grBus;
var rutaAct;
var rutaStr;
var tiqueteAct="";
var ddlDestino=document.getElementById("<%=ddlDestino.ClientID%>");
var txtNIT=document.getElementById("<%=txtNIT.ClientID%>");
var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
var ddlPlanilla=document.getElementById("<%=ddlPlanilla.ClientID%>");
var txtPrecio=document.getElementById("<%=txtPrecio.ClientID%>");
var txtTiquete=document.getElementById("<%=txtTiquete.ClientID%>");
var txtNITa=document.getElementById("<%=txtNITa.ClientID%>");
var txtNITb=document.getElementById("<%=txtNITb.ClientID%>");
var txtNITc=document.getElementById("<%=txtNITc.ClientID%>");

function CambiaRuta(){
	var planilla=ddlPlanilla.value;
	var tipo="V";
	var agencia=ddlAgencia.value;
	if(ddlDestino.value.length==0){
		divDestino.innerHTML="";
		divBus.innerHTML="";
		divCliente.style.display='none';
		ddlDestino.focus();
		return;
	}
	rutaStr=ddlDestino.options[ddlDestino.selectedIndex].text;
	rutaAct=ddlDestino.value;
	divDestino.innerHTML="Cargando ruta...";
	AMS_Comercial_VentaTiquetesPrepago.CargarPuestos(planilla,rutaAct,tipo,agencia,CambiaRuta_Callback);
	divCliente.style.display = 'block';
	txtTiquete.focus();
}

function CambiaRuta_Callback(response){
	var respuesta=response.value;
	var params=respuesta.split('@');

	grBus=new GraficadorBus(divBus,params[0],params[1],'AMS.Web.index.aspx?process=Comercial.ProcesoTiquete','../img/',0,'',divCantidad,txtPrecio,divTotal,prctPrepago);
	grBus.Actualizar(prctPrepago);
	divDestino.innerHTML=rutaStr;
	divDestino.innerHTML+="<br>["+rutaAct+"]";
	divPrincipal.innerHTML="Fecha Salida:   "+params[2]+"<br>Hora Salida:   "+params[3]+"<br>Conductor:   "+params[4]+"<br>Placa:   "+params[5]+"<br>No. Bus:  "+params[6]+"<br><br>";
	strP=params[7];
	/*if(strP.length>3){
		if(strP.length>6)txtPrecio.value=strP.substring(0,strP.length-6)+","+strP.substring(strP.length-6,strP.length-3)+","+strP.substring(strP.length-3);
		else txtPrecio.value=strP.substring(0,strP.length-3)+","+strP.substring(strP.length-3);
	}
	else txtPrecio.value=strP;*/
	txtTiquete.value=params[8];
	tiqueteAct=params[8];
	grBus.Totales();
	window.scrollTo(0, document.body.scrollHeight);
}

function Comprar(){
	var precioS=txtPrecio.value.replace(/\,/g,'');
	var nit=txtNIT.value;
	var nombre=txtNITa.value;
	var apellido=txtNITb.value;
	var tnit=txtNITc.value;
	var tipo="V";
	var ruta=ddlDestino.value;
	var planilla=ddlPlanilla.value;
	var agencia=ddlAgencia.value;
	var tiquete=txtTiquete.value;
	if(grBus.Cantidad>1){
		alert('Solo puede registrar un tiquete prepago a la vez.');
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
	}
	var precio=parseInt(precioS);
	var param=grBus.Parametros();
	if(param.length==0){
		if(grBus.Lleno())
			alert('No hay puestos libres.');
		else
			alert('No seleccionó ningún puesto.');
		return(false);}
	divProc.innerHTML="<BR>Registrando compra...";
	AMS_Comercial_VentaTiquetesPrepago.Comprar(tiquete,agencia,planilla,tipo,ruta,nit,tnit,nombre,apellido,precio,param,prctPrepago,Comprar_Callback);
	return(false);
}
function Comprar_Callback(response){
	var respuesta=response.value;
	var params=respuesta.split('@');
	CambiaRuta();
	if(params[0]=="0"){
		alert(params[1]);
		divProc.innerHTML="<BR>"+params[1];
		CambiaRuta();}
	else{
	
		//ddlDestino.value="";
		alert(params[1]);
		divBus.innerHTML="";
		divProc.innerHTML="";
		tiqueteAct="";
		ddlDestino.focus();
		/*if(params[3]=="V")
			window.open('AMS.Tiquete.aspx?tq='+params[2], "TIQUETE", "width=600,height=230,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");*/
		CambiaRuta();
	}
}

function TraerNIT(){
	AMS_Comercial_VentaTiquetesPrepago.TraaerNIT(txtNIT.value,TraerNIT_Callback);
	return(false);
}

function TraerNIT_Callback(response){
	var respuesta=response.value;
	var params=respuesta.split('|');
	txtNITa.value=params[0];
	txtNITb.value=params[1];
	if(params[0].length>0)txtPrecio.focus();
}

function KeyDownHandler(){
	if(event.keyCode == 13){
		CambiaRuta();
	}
}
function KeyDownHandlerNIT(){
	if(event.keyCode==13 && txtNIT.value.length>0)TraerNIT();
}
<%=focus%>
					</SCRIPT>
				</asp:Panel>
			</asp:Panel>
			<TR>
				<TD style="WIDTH: 545px" align="left" colSpan="2">
					<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>&nbsp;</TD>
			</TR>
		</asp:panel>
	</table>
</DIV>
-->