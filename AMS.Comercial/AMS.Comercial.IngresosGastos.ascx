<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.IngresosGastos.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_IngresosGastos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Información del Ingreso/Gasto/Servicio:</b></td>
		</tr>
		<TR>
			<TD style="WIDTH: 214px; HEIGHT: 24px"><asp:label id="Label20" runat="server" Font-Size="XX-Small" Font-Bold="True">Número Documento</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="TextDocumento" runat="server" Font-Size="XX-Small" Width="106px" AutoPostBack="True"></asp:textbox></TD>
			<TD style="HEIGHT: 24px">&nbsp;</TD>
		</TR>
		<tr>
			<td style="WIDTH: 214px; HEIGHT: 21px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 21px"><asp:label id="lblAgencia" runat="server" Font-Size="XX-Small" Font-Bold="True" Width="20px"></asp:label>&nbsp;&nbsp;</td>
			<asp:label id="lblNombreAgencia" runat="server" Font-Size="XX-Small" Font-Bold="True" Width="150px"></asp:label></TD>
			<TD style="HEIGHT: 21px">&nbsp;</TD>
		</tr>
		<TR>
			<TD style="WIDTH: 214px; HEIGHT: 24px"><asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Planilla</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="TextPlanilla" runat="server" Font-Size="XX-Small" Width="106px" AutoPostBack="True"></asp:textbox></TD>
			<TD style="HEIGHT: 24px">&nbsp;</TD>
		</TR>
		<TR>
			<td><asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa del Bus :</asp:label></TD>
			<td><asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0;', new Array(),1)"
					runat="server" Font-Size="XX-Small" Width="80px" MaxLength="6"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 214px; HEIGHT: 18px"><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Concepto:</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlConcepto" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			<td>&nbsp;</TD>
		</TR>
	</table>
	<br>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Receptor:</B></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top"><asp:label id="Label23" runat="server" Font-Size="XX-Small" Font-Bold="True">Documento :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNITReceptor" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES AS NOMBRE, MNIT.MNIT_APELLIDOS AS APELLIDOS from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" Font-Size="XX-Small" Width="80px" MaxLength="15" ReadOnly="True"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top"><asp:label id="Label24" runat="server" Font-Size="XX-Small" Font-Bold="True">Nombre :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNombreReceptor" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="60"
					readonly="True"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label25" runat="server" Font-Size="XX-Small" Font-Bold="True">Apellido :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtApellidoReceptor" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="60"
					readonly="True"></asp:textbox></TD>
		</TR>
	</TABLE>
	<BR>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Anticipo/Servicio:</B></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Documento Referencia :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtNumDocReferencia" runat="server" Font-Size="XX-Small" Width="200px" MaxLength="20"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha :</asp:label></TD>
			<TD style="WIDTH: 386px" vAlign="top"><asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="60px" Runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 67px" vAlign="top"><asp:label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Descripcion :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 67px" vAlign="top"><asp:textbox id="txtDescripcion" runat="server" Font-Size="XX-Small" Width="458px" ReadOnly="False"
					Height="96px" TextMode="MultiLine"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label14" runat="server" Font-Size="XX-Small" Font-Bold="True">Cantidad Consumo :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtCantidad" onkeyup="NumericMask(this)" runat="server" Font-Size="XX-Small"
					Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label15" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Unidad :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtValorUnidad" onkeyup="NumericMask(this)" runat="server" Font-Size="XX-Small"
					Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px" vAlign="top"><asp:label id="Label16" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Total :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:textbox id="txtValorTotal" onkeyup="NumericMask(this)" runat="server" Font-Size="XX-Small"
					Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
		</TR>
	</TABLE>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<TD style="WIDTH: 14px" vAlign="top">
			<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top"><asp:button id="BtnModificar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Modificar"></asp:button>&nbsp;&nbsp;&nbsp;</TD>
			<TD style="WIDTH: 386px; HEIGHT: 20px" vAlign="top"><asp:button id="BtnBorrar" Font-Size="XX-Small" Font-Bold="True" Width="65px" Runat="server"
					Text="Borrar"></asp:button></TD>
			<TD style="WIDTH: 386px; HEIGHT: 20px" vAlign="top"><asp:button id="BtnRegresar" Font-Size="XX-Small" Font-Bold="True" Width="65px" Runat="server"
					Text="Regresar"></asp:button></TD>
			<TD style="WIDTH: 386px; HEIGHT: 20px" vAlign="top"><asp:button id="btnImprimir" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Imprimir"
					Width="65px"></asp:button>
			<TD style="WIDTH: 300px"><asp:hyperlink id="Ver" runat="server" Visible="False" Width="300px">De Click Aqui para ver el Reporte</asp:hyperlink>&nbsp;&nbsp;</TD>
		</TR>
	</TABLE>
</DIV>
<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label>
<script language="javascript">
var txtDescripcion=document.getElementById("<%=txtDescripcion.ClientID%>");
var txtCantidad=document.getElementById("<%=txtCantidad.ClientID%>");
var txtValorUnidad=document.getElementById("<%=txtValorUnidad.ClientID%>");
var txtValorTotal=document.getElementById("<%=txtValorTotal.ClientID%>");
var txtNITReceptor=document.getElementById("<%=txtNITReceptor.ClientID%>");

function validarGasto(){
	if(txtNITReceptor.value.length==0){
		alert('Debe ingresar el receptor.');
		return(false);
	}
	if(txtDescripcion.value.length==0 || txtCantidad.value.length==0 || txtValorUnidad.value.length==0 || txtValorTotal.value.length==0){
		alert('Debe ingresar todos los datos del anticipo o servicio.');
		return(false);
	}
	return(true);
}
function Totales(){
	try{
		var totV=parseInt(txtCantidad.value.replace(/\,/g,''))*parseInt(txtValorUnidad.value.replace(/\,/g,''));
		parseValor(totV,txtValorTotal);
	}
	catch(err){
		txtValorTotal.value='';
	}
}
function parseValor(valor,objTot){
	objTot.value=formatoValor(valor);
}
</script>
