<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.CuadroRutas.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_CuadroRutas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colspan="2"><b>Informaci�n de la ruta:</b>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 139px" valign="top"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">C�digo :</asp:label></td>
			<td><asp:dropdownlist id="ddlRuta" Font-Size="XX-Small" runat="server" Width="150px" OnChange="cargarCuadro()"></asp:dropdownlist><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server"><SPAN id="spnAjax" style="font-size:XX-Small;font-weight:bold;"></SPAN></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 139px"><asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo de programaci�n:</asp:label></td>
			<td><asp:dropdownlist id="ddlProgramacion" Font-Size="XX-Small" runat="server" OnChange="cargarCuadro()"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 139px"><asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Hora :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">
				<asp:DropDownList font-Size="XX-Small" id="ddlHora" runat="server" Width="40px" OnChange="cargarCuadro()"></asp:DropDownList>&nbsp;:&nbsp;
				<asp:DropDownList font-Size="XX-Small" id="ddlMinuto" runat="server" Width="48px" OnChange="cargarCuadro()"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td colspan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colspan="2"><b>Dias:</b>
			</td>
		</tr>
		<TR>
			<td style="WIDTH: 11px"></td>
			<td align="left">
				<asp:CheckBox id="chkLunes" runat="server" Font-Size="XX-Small" Text="&nbsp;Lunes"></asp:CheckBox></td>
		</TR>
		<TR>
			<td style="WIDTH: 11px"></td>
			<td align="left">
				<asp:CheckBox id="chkMartes" runat="server" Font-Size="XX-Small" Text="&nbsp;Martes"></asp:CheckBox></td>
		</TR>
		<TR>
			<td style="WIDTH: 11px"></td>
			<td align="left">
				<asp:CheckBox id="chkMiercoles" runat="server" Font-Size="XX-Small" Text="&nbsp;Miercoles"></asp:CheckBox></td>
		</TR>
		<TR>
			<td style="WIDTH: 11px"></td>
			<td align="left">
				<asp:CheckBox id="chkJueves" runat="server" Font-Size="XX-Small" Text="&nbsp;Jueves"></asp:CheckBox></td>
		</TR>
		<TR>
			<td style="WIDTH: 11px"></td>
			<td align="left">
				<asp:CheckBox id="chkViernes" runat="server" Font-Size="XX-Small" Text="&nbsp;Viernes"></asp:CheckBox></td>
		</TR>
		<TR>
			<td style="WIDTH: 11px"></td>
			<td align="left">
				<asp:CheckBox id="chkSabado" runat="server" Font-Size="XX-Small" Text="&nbsp;Sabado"></asp:CheckBox></td>
		</TR>
		<TR>
			<td style="WIDTH: 11px"></td>
			<td align="left">
				<asp:CheckBox id="chkDomingo" runat="server" Font-Size="XX-Small" Text="&nbsp;Domingo"></asp:CheckBox></td>
		</TR>
		<TR>
			<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
		</TR>
		<TR>
			<td style="WIDTH: 11px"></td>
			<td align="left"><asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Actualizar"></asp:button></td>
		</TR>
		<TR>
			<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
		</TR>
	</table>
</DIV>
<script language:javascript>
var chkLunes=document.getElementById("<%=chkLunes.ClientID%>");
var chkMartes=document.getElementById("<%=chkMartes.ClientID%>");
var chkMiercoles=document.getElementById("<%=chkMiercoles.ClientID%>");
var chkJueves=document.getElementById("<%=chkJueves.ClientID%>");
var chkViernes=document.getElementById("<%=chkViernes.ClientID%>");
var chkSabado=document.getElementById("<%=chkSabado.ClientID%>");
var chkDomingo=document.getElementById("<%=chkDomingo.ClientID%>");
function cargarCuadro(){
	var mruta=document.getElementById("<%=ddlRuta.ClientID%>").value;
	var hora=document.getElementById("<%=ddlHora.ClientID%>").value;
	var mins=document.getElementById("<%=ddlMinuto.ClientID%>").value;
	var prog=document.getElementById("<%=ddlProgramacion.ClientID%>").value;
	if(mruta.length==0){
		rstChecks();
		document.getElementById('spnAjax').innerHTML="";
		return;}
	AMS_Comercial_CuadroRutas.CargarCuadro(mruta,hora,mins,prog,CargarPlaca_Callback);
}
function rstChecks(){
	chkLunes.checked=false;
	chkMartes.checked=false;
	chkMiercoles.checked=false;
	chkJueves.checked=false;
	chkViernes.checked=false;
	chkSabado.checked=false;
	chkDomingo.checked=false;
}
function CargarPlaca_Callback(response){
	var info=true;
	respuesta=response.value;
	if(respuesta.Tables[0]==null)info=false;
	if(info && respuesta.Tables[0].Rows.length>0)
		document.getElementById('spnAjax').innerHTML="<table style='font-size:XX-Small;font-weight:bold;'><tr><td>Descripci�n:</td><td>"+respuesta.Tables[0].Rows[0].DESC+"</td></tr><tr><td>Origen:</td><td>"+respuesta.Tables[0].Rows[0].ORIG+"</td></tr><tr><td>Destino:</td><td>"+respuesta.Tables[0].Rows[0].DEST+"</td></tr></table>";	
	else{
		document.getElementById('spnAjax').innerHTML="";
		info=false;}
	info=true;
	if(respuesta.Tables[1]==null)info=false;
	if(info && respuesta.Tables[1].Rows.length>0){
		chkLunes.checked=(respuesta.Tables[1].Rows[0].LUN=="S")?true:false;
		chkMartes.checked=(respuesta.Tables[1].Rows[0].MAR=="S")?true:false;
		chkMiercoles.checked=(respuesta.Tables[1].Rows[0].MIE=="S")?true:false;
		chkJueves.checked=(respuesta.Tables[1].Rows[0].JUE=="S")?true:false;
		chkViernes.checked=(respuesta.Tables[1].Rows[0].VIE=="S")?true:false;
		chkSabado.checked=(respuesta.Tables[1].Rows[0].SAB=="S")?true:false;
		chkDomingo.checked=(respuesta.Tables[1].Rows[0].DOM=="S")?true:false;
	}
	else info=false;
	if(!info)
		rstChecks();
}
</script>
