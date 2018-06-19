<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaDoble.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaDoble" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><h3>Información de la ruta gemela</h3>
			</td>
		</tr>
		<TR>
			<TD><asp:label id="Label11" runat="server"  Font-Bold="True">Ruta Principal:</asp:label></TD>
			<td><asp:textbox id="txtRutaP" onclick="ModalDialog(this,'SELECT MR.MRUT_CODIGO AS CODIGO, MR.MRUT_DESCRIPCION AS DESCRIPCION FROM DBXSCHEMA.MRUTAS MR WHERE MR.MRUT_CLASE=2 ORDER BY MR.MRUT_CODIGO', new Array(),1);TraerActual();"
					ReadOnly="True"  runat="server" Width="80px"></asp:textbox>&nbsp;</TD>
		</TR>
		<TR>
			<TD vAlign="top">
				<asp:label id="Label1" Font-Bold="True"  runat="server">Ruta Primera Planilla :</asp:label></TD>
			<td><asp:textbox id="txtRuta1" onclick="MostrarSubRutas(this)"  runat="server"
					Width="80px" ReadOnly="True"></asp:textbox>&nbsp;</TD>
		</TR>
		<TR>
			<TD vAlign="top">
				<asp:label id="Label14" Font-Bold="True"  runat="server">Ruta Segunda Planilla :</asp:label></TD>
			<td><asp:textbox id="txtRuta2" onclick="MostrarSubRutas(this)"  runat="server"
					Width="80px" ReadOnly="True"></asp:textbox>&nbsp;</TD>
		</TR>
		<TR>
			<TD vAlign="top">
				<asp:label id="Label2" Font-Bold="True"  runat="server">Codigo Interno Ruta Primera Planilla :</asp:label></TD>
			<td><asp:textbox id="txtRutaInt1"  runat="server" Width="80px" MaxLength="10"></asp:textbox>&nbsp;</TD>
		</TR>
		<TR>
			<TD vAlign="top">
				<asp:label id="Label3" Font-Bold="True"  runat="server">Codigo Interno Ruta Segunda Planilla :</asp:label></TD>
			<td><asp:textbox id="txtRutaInt2"  runat="server" Width="80px" MaxLength="10"></asp:textbox>&nbsp;</TD>
		</TR>
		<TR>
			<td></TD>
			<TD align="left">
				<asp:button id="btnGuardar" Font-Bold="True"  Runat="server" Width="87px"
					Text="Actualizar"></asp:button></TD>
		</TR>
		<tr>
			<td colSpan="2">&nbsp;
				<asp:label id="lblError" runat="server"  Font-Bold="True"></asp:label></td>
		</tr>
	</table>
</fieldset>
<script language:javascript>
var txtRutaP=document.getElementById("<%=txtRutaP.ClientID%>");
var txtRuta1=document.getElementById("<%=txtRuta1.ClientID%>");
var txtRuta2=document.getElementById("<%=txtRuta2.ClientID%>");
var txtRutaInt1=document.getElementById("<%=txtRutaInt1.ClientID%>");
var txtRutaInt2=document.getElementById("<%=txtRutaInt2.ClientID%>");
function MostrarSubRutas(Obj)
{
	var rutaP=txtRutaP.value;
	if(rutaP.length==0){
		alert('Debe seleccionar la ruta principal');
		return;}
	ModalDialog(Obj,'SELECT MR.MRUT_CODIGO AS CODIGO, MR.MRUT_DESCRIPCION AS DESCRIPCION FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.MRUTA_INTERMEDIA MRI WHERE MRI.MRUTA_PRINCIPAL=\''+rutaP+'\' AND MRI.MRUTA_SECUNDARIA=MR.MRUT_CODIGO ORDER BY MR.MRUT_CODIGO', new Array(),1);
	return(false);
}
function TraerActual(){
	var rutaP=txtRutaP.value;
	if(rutaP.length>0){
		AMS_Comercial_PlanillaDoble.TraerRuta(rutaP,TraerActual_Callback);
	}
}

function TraerActual_Callback(response)
{
	var respuesta=response.value;
	if(respuesta.Tables[0].Rows.length>0){
		txtRuta1.value=respuesta.Tables[0].Rows[0].MRUT_CODIGO1;
		txtRuta2.value=respuesta.Tables[0].Rows[0].MRUT_CODIGO2;
		txtRutaInt1.value=respuesta.Tables[0].Rows[0].CODIGO_INTERNO1;
		txtRutaInt2.value=respuesta.Tables[0].Rows[0].CODIGO_INTERNO2;
	}
	else{
		txtRuta1.value='';
		txtRuta2.value='';
		txtRutaInt1.value='';
		txtRutaInt2.value='';}
}
</script>
