<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RecepcionGiros.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RecepcionGiros" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Información de la ruta:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Origen :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgenciaO" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
			<TD>&nbsp;</TD>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label15" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Destino :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgenciaD" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
			<TD>&nbsp;</TD>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo de Giro :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlTipo" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px">
					<asp:ListItem Value="V">Virtual</asp:ListItem>
					<asp:ListItem Value="M">Real</asp:ListItem>
				</asp:dropdownlist></td>
			</TD>
			<TD>&nbsp;</TD>
		</tr>
		<asp:panel id="pnlPlanilla" Runat="server" Visible="False">
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Planilla :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:dropdownlist id="ddlPlanilla" runat="server" Font-Size="XX-Small" Width="150px"></asp:dropdownlist></TD>
				<TD>&nbsp;</TD>
			</TR>
		</asp:panel>
		<TR>
			<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
		</TR>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Datos del Emisor:</b></td>
		</tr>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top"><asp:label id="Label16" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo Documento :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:dropdownlist id="txtNITEmisorc" Font-Size="XX-Small" runat="server" Visible="True"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top"><asp:label id="Label17" Font-Bold="True" Font-Size="XX-Small" runat="server">Documento :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITEmisor" ondblclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES AS NOMBRE, MNIT.MNIT_APELLIDOS AS APELLIDOS, MNIT.TNIT_TIPONIT AS TIPO, MNIT.MNIT_TELEFONO AS TELEFONO, MNIT.MNIT_DIRECCION AS DIRECCION from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					Font-Size="XX-Small" runat="server" Width="80px" MaxLength="15" ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top"><asp:label id="Label18" Font-Bold="True" Font-Size="XX-Small" runat="server">Nombre :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITEmisora" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label19" Font-Bold="True" Font-Size="XX-Small" runat="server">Apellido :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITEmisorb" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label20" Font-Bold="True" Font-Size="XX-Small" runat="server">Telefono :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITEmisord" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="40"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label21" Font-Bold="True" Font-Size="XX-Small" runat="server">Direccion :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITEmisore" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="100"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
		</TR>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Datos del Receptor:</b></td>
		</tr>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top"><asp:label id="Label22" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo Documento :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:dropdownlist id="txtNITReceptorc" Font-Size="XX-Small" runat="server" Visible="True"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top"><asp:label id="Label23" Font-Bold="True" Font-Size="XX-Small" runat="server">Documento :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITReceptor" ondblclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES AS NOMBRE, MNIT.MNIT_APELLIDOS AS APELLIDOS, MNIT.TNIT_TIPONIT AS TIPO, MNIT.MNIT_TELEFONO AS TELEFONO, MNIT.MNIT_DIRECCION AS DIRECCION from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					Font-Size="XX-Small" runat="server" Width="80px" MaxLength="15" ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top"><asp:label id="Label24" Font-Bold="True" Font-Size="XX-Small" runat="server">Nombre :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITReceptora" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label25" Font-Bold="True" Font-Size="XX-Small" runat="server">Apellido :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITReceptorb" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label26" Font-Bold="True" Font-Size="XX-Small" runat="server">Telefono :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITReceptord" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="40"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label27" Font-Bold="True" Font-Size="XX-Small" runat="server">Direccion :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITReceptore" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="100"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlInicial" Runat="server" Visible="True">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Giro:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Giro :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtValor" onkeyup="NumericMask(this);Totales();" runat="server" Font-Size="XX-Small"
						Width="100px" ReadOnly="False" MaxLength="11"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Costo Giro :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtCosto" onkeyup="NumericMask(this);" runat="server" Font-Size="XX-Small" Width="100px"
						ReadOnly="False" MaxLength="11"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:button id="btnGuardar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Continuar"></asp:button></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel><asp:panel id="pnlConfirmar" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Giro:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblNumDocumento" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblFecha" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Giro :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblValorGiro" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Porcentaje Costo Giro :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblPorcentajeGiro" runat="server" Font-Size="XX-Small"></asp:Label>&nbsp;%</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Costo Giro :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblCostoGiro" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Porcentaje IVA :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblPorcentajeIVA" runat="server" Font-Size="XX-Small"></asp:Label>&nbsp;%</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">IVA :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblValorIVA" runat="server" Font-Size="XX-Small"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Total :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:Label id="lblTotal" runat="server" Font-Size="Small"></asp:Label></TD>
			</TR>
			<asp:panel id="pnlCrear" Visible="True" Runat="server">
				<TR>
					<TD style="WIDTH: 154px" vAlign="top" align="right"></TD>
					<TD style="WIDTH: 154px" vAlign="top" align="left"><BR>
						<asp:button id="btnRegistrar" Font-Size="XX-Small" Font-Bold="True" Width="100px" Runat="server"
							Text="Recibir Giro"></asp:button><BR>
						<BR>
						<asp:button id="btnAtras" Font-Size="XX-Small" Font-Bold="True" Width="100px" Runat="server"
							Text="Atras"></asp:button></TD>
				</TR>
			</asp:panel>
			<asp:panel id="pnlImprimir" Visible="False" Runat="server">
				<TR>
					<TD>
						<P align="center"><BR>
							GIRO REGISTRADO</P>
					</TD>
				</TR>
				<TR>
					<TD>
						<P align="center"><INPUT onclick="verGiro();" type="button" value="Imprimir" class="noEspera"></P>
					</TD>
				</TR>
			</asp:panel>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel></DIV>
<br>
<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
<script language="javascript">
var txtNITEmisor=document.getElementById("<%=txtNITEmisor.ClientID%>");
var txtNITEmisora=document.getElementById("<%=txtNITEmisora.ClientID%>");
var txtNITEmisorb=document.getElementById("<%=txtNITEmisorb.ClientID%>");
var txtNITEmisorc=document.getElementById("<%=txtNITEmisorc.ClientID%>");
var txtNITEmisord=document.getElementById("<%=txtNITEmisord.ClientID%>");
var txtNITReceptor=document.getElementById("<%=txtNITReceptor.ClientID%>");
var txtNITReceptora=document.getElementById("<%=txtNITReceptora.ClientID%>");
var txtNITReceptorb=document.getElementById("<%=txtNITReceptorb.ClientID%>");
var txtNITReceptorc=document.getElementById("<%=txtNITReceptorc.ClientID%>");
var txtNITReceptord=document.getElementById("<%=txtNITReceptord.ClientID%>");
var ddlAgenciaO=document.getElementById("<%=ddlAgenciaO.ClientID%>");
var ddlAgenciaD=document.getElementById("<%=ddlAgenciaD.ClientID%>");
var txtValor=document.getElementById("<%=txtValor.ClientID%>");
var txtCosto=document.getElementById("<%=txtCosto.ClientID%>");
var txtNIT;
var porcentajeGiro=<%=strPorcentajeGiro%>;
function TraerNIT(obj){
	txtNIT=obj;
	AMS_Comercial_RecepcionGiros.TraaerNIT(txtNIT.value,TraerNIT_Callback);
	return(false);
}

function TraerNIT_Callback(response){
	var respuesta=response.value;
	var params=respuesta.split('|');
	document.getElementById(txtNIT.id+'a').value=params[0];
	document.getElementById(txtNIT.id+'b').value=params[1];
	if(params[2].length>0)document.getElementById(txtNIT.id+'c').value=params[2];
	document.getElementById(txtNIT.id+'d').value=params[3];
	document.getElementById(txtNIT.id+'e').value=params[4];
}

function KeyDownHandlerNIT(obj){
	if(event.keyCode==13)
		if(obj.value.length>0)
			return(TraerNIT(obj));
}
function validarGiro(){
	var valor=txtValor.value.replace(/\,/g,'');
	if(txtNITEmisor.value.length==0 || txtNITEmisora.value.length==0 || txtNITEmisorb.value.length==0 || txtNITEmisorc.value.length==0 || txtNITEmisord.value.length==0 || txtNITReceptor.value.length==0 || txtNITReceptora.value.length==0 || txtNITReceptorb.value.length==0 || txtNITReceptorc.value.length==0 || txtNITReceptord.value.length==0){
		alert('Debe ingresar todos los datos del emisor y el receptor para crear el giro.');
		return(false);
	}
	if(ddlAgenciaO.value==ddlAgenciaD.value){
		alert('La agencia de origen es igual a la de destino.');
		return(false);
	}
	if(valor.length==0){
		alert('Valor de giro no válido.');
		return(false);
	}
	return(true);
}
function Totales(){
	try{
		var totV=parseInt(txtValor.value.replace(/\,/g,''));
		totV=Math.round((totV*porcentajeGiro)/100);
		parseValor(totV,txtCosto);
	}
	catch(err){
		txtCosto.value='';
	}
}
function parseValor(valor,objTot){
	objTot.value=formatoValor(valor);
}
function verGiro(){
	window.open('../aspx/AMS.Comercial.Giro.aspx?gir='+<%=ViewState["Giro"]%>, 'GIRO'+<%=ViewState["Giro"]%>, "width=340,height=340,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");
}
</script>
