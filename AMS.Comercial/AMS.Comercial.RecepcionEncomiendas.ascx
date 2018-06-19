<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RecepcionEncomiendas.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RecepcionEncomiendas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 439px" colSpan="2"><b>Información de la ruta:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 5px"><asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 281px; HEIGHT: 5px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 154px; HEIGHT: 5px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Ruta :</asp:label></td>
			<td style="WIDTH: 281px; HEIGHT: 5px"><asp:dropdownlist id="ddlRuta" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="280px"></asp:dropdownlist></td>
			<TD style="HEIGHT: 5px" rowSpan="4"><asp:label id="lblRuta" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</tr>
		<asp:panel id="pnlPlanilla" Runat="server" Visible="False">
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Planilla :</asp:label></TD>
				<TD style="WIDTH: 281px; HEIGHT: 18px">
					<asp:dropdownlist id="ddlPlanilla" runat="server" Font-Size="XX-Small" Width="150px"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label35" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa del Bus :</asp:label></TD>
				<TD style="WIDTH: 281px">
					<asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado  where testa_codigo>0;', new Array(),1)"
						runat="server" Font-Size="XX-Small" Width="80px" MaxLength="6"></asp:textbox>&nbsp;
				</TD>
			</TR>
		</asp:panel>
		<TR>
			<TD style="WIDTH: 439px" align="center" colSpan="2">&nbsp;</TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlEncomienda" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Emisor:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label16" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:dropdownlist id="txtNITEmisorc" runat="server" Font-Size="XX-Small" Visible="True"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label17" runat="server" Font-Size="XX-Small" Font-Bold="True">Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITEmisor" ondblclick="ModalDialog(this,'SELECT MPAS_NIT AS NIT,MPAS_NOMBRES AS NOMBRE, MPAS_APELLIDOS AS APELLIDOS, \'C\', MPAS_TELEFONO as telefono, MPAS_DIRECCION as DIRECCION FROM DBXSCHEMA.MPASAJERO;', new Array(),1)"
						runat="server" Font-Size="XX-Small" Width="80px" MaxLength="15" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Nombre :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITEmisora" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="60"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label19" runat="server" Font-Size="XX-Small" Font-Bold="True">Apellido :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITEmisorb" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="60"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label20" runat="server" Font-Size="XX-Small" Font-Bold="True">Telefono :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITEmisord" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="40"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label21" runat="server" Font-Size="XX-Small" Font-Bold="True">Direccion :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITEmisore" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="100"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="2"><B>Datos del Receptor:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label22" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:dropdownlist id="txtNITReceptorc" runat="server" Font-Size="XX-Small" Visible="True"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label23" runat="server" Font-Size="XX-Small" Font-Bold="True">Documento :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITReceptor" ondblclick="ModalDialog(this,'SELECT MPAS_NIT AS NIT,MPAS_NOMBRES AS NOMBRE, MPAS_APELLIDOS AS APELLIDOS, \'C\', MPAS_TELEFONO as telefono, MPAS_DIRECCION as DIRECCION FROM DBXSCHEMA.MPASAJERO;', new Array(),1)"
						runat="server" Font-Size="XX-Small" Width="80px" MaxLength="15" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px" vAlign="top">
					<asp:label id="Label24" runat="server" Font-Size="XX-Small" Font-Bold="True">Nombre :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITReceptora" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="60"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label25" runat="server" Font-Size="XX-Small" Font-Bold="True">Apellido :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITReceptorb" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="60"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label26" runat="server" Font-Size="XX-Small" Font-Bold="True">Telefono :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITReceptord" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="40"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label27" runat="server" Font-Size="XX-Small" Font-Bold="True">Direccion :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtNITReceptore" runat="server" Font-Size="XX-Small" Width="300px" MaxLength="100"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<asp:panel id="pnlInicial" Visible="True" Runat="server">
			<TABLE style="WIDTH: 773px" align="center">
				<TR>
					<TD style="WIDTH: 545px" colSpan="2"><B>Datos de la Encomienda:</B></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label32" runat="server" Font-Size="XX-Small" Font-Bold="True">Documento Referencia :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:textbox id="txtNumDocReferencia" runat="server" Font-Size="XX-Small" Width="200px" MaxLength="20"
							ReadOnly="False"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 22px" vAlign="top">
						<asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 22px" vAlign="top">
						<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="136px" Runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 67px" vAlign="top">
						<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Descripcion :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 67px" vAlign="top">
						<asp:textbox id="txtDescripcion" runat="server" Font-Size="XX-Small" Width="458px" ReadOnly="False"
							TextMode="MultiLine" Height="62px"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Unidades :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:textbox id="txtUnidades" onkeyup="NumericMask(this)" runat="server" Font-Size="XX-Small"
							Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Peso total (lbs) :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:textbox id="txtPeso" onkeyup="Totales();NumericMask(this);" runat="server" Font-Size="XX-Small"
							Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Volumen total (mts. cubicos) :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:textbox id="txtVolumen" onkeyup="Totales();NumericMask(this);" runat="server" Font-Size="XX-Small"
							Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label14" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Declarado :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:textbox id="txtValorAvaluo" onkeyup="NumericMask(this)" runat="server" Font-Size="XX-Small"
							Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label36" runat="server" Font-Size="XX-Small" Font-Bold="True">Costo Encomienda :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:textbox id="txtCosto" onkeyup="NumericMask(this)" runat="server" Font-Size="XX-Small" Width="100px"
							MaxLength="11" ReadOnly="False"></asp:textbox></TD>
				</TR>
				<TR>
					<TD></TD>
					<TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="btnGuardar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Continuar"></asp:button></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
				</TR>
			</TABLE>
		</asp:panel>
		<asp:panel id="pnlConfirmar" Visible="False" Runat="server">
			<TABLE style="WIDTH: 773px" align="center">
				<TR>
					<TD style="WIDTH: 545px" colSpan="2"><B><B>Datos de la Encomienda</B>:</B></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label30" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero Documento :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:Label id="lblNumDocumento" runat="server" Font-Size="XX-Small"></asp:Label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label34" runat="server" Font-Size="XX-Small" Font-Bold="True">Documento Referencia :</asp:label></TD>
					<TD style="WIDTH: 386px" vAlign="top">
						<asp:label id="lblNumDocumentoRef" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label28" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:Label id="lblFecha" runat="server" Font-Size="XX-Small"></asp:Label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Descripcion :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:Label id="lblDescripcion" runat="server" Font-Size="XX-Small"></asp:Label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label15" runat="server" Font-Size="XX-Small" Font-Bold="True">Unidades :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:Label id="lblUnidades" runat="server" Font-Size="XX-Small"></asp:Label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label29" runat="server" Font-Size="XX-Small" Font-Bold="True">Peso Total (lbs) :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:Label id="lblPeso" runat="server" Font-Size="XX-Small"></asp:Label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label31" runat="server" Font-Size="XX-Small" Font-Bold="True">Volumen Total (mts. cubicos) :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:Label id="lblVolumen" runat="server" Font-Size="XX-Small"></asp:Label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label33" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor Declarado :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:Label id="lblAvaluo" runat="server" Font-Size="XX-Small"></asp:Label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px" vAlign="top">
						<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Costo Encomienda :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
						<asp:Label id="lblCostoEncomienda" runat="server" Font-Size="XX-Small"></asp:Label></TD>
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
						<asp:Label id="lblTotal" runat="server" Font-Size="XX-Small"></asp:Label></TD>
				</TR>
				<asp:panel id="pnlCrear" Visible="True" Runat="server">
					<TR>
						<TD style="WIDTH: 154px" vAlign="top" align="right"></TD>
						<TD style="WIDTH: 154px" vAlign="top" align="left"><BR>
							<asp:button id="btnRegistrar" Font-Size="XX-Small" Font-Bold="True" Width="125px" Runat="server"
								Text="Recibir Encomienda"></asp:button><BR>
							<BR>
							<asp:button id="btnAtras" Font-Size="XX-Small" Font-Bold="True" Width="125px" Runat="server"
								Text="Atras"></asp:button></TD>
					</TR>
				</asp:panel>
				<asp:panel id="pnlImprimir" Visible="False" Runat="server">
					<TR>
						<TD>
							<P align="center"><BR>
								ENCOMIENDA REGISTRADA</P>
						</TD>
					</TR>
					<TR>
						<TD>
							<P align="center"><INPUT onclick="verEncomienda();" type="button" value="Imprimir" class="noEspera"></P>
						</TD>
					</TR>
				</asp:panel>
				<TR>
					<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
				</TR>
			</TABLE>
		</asp:panel>
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
var ddlRuta=document.getElementById("<%=ddlRuta.ClientID%>");
var ddlPlanilla=document.getElementById("<%=ddlPlanilla.ClientID%>");
var txtDescripcion=document.getElementById("<%=txtDescripcion.ClientID%>");
var txtUnidades=document.getElementById("<%=txtUnidades.ClientID%>");
var txtPeso=document.getElementById("<%=txtPeso.ClientID%>");
var txtVolumen=document.getElementById("<%=txtVolumen.ClientID%>");
var txtValorAvaluo=document.getElementById("<%=txtValorAvaluo.ClientID%>");
var txtCosto=document.getElementById("<%=txtCosto.ClientID%>");
var txtNIT;
var facPeso=<%=strFactorPeso%>;
var facVolumen=<%=strFactorVolumen%>;

function TraerNIT(obj){
	txtNIT=obj;
	AMS_Comercial_RecepcionEncomiendas.TraaerNIT(txtNIT.value,TraerNIT_Callback);
	return(false);
}

function TraerNIT_Callback(response){
	var respuesta=response.value;
	var params=respuesta.split('|');
	document.getElementById(txtNIT.id+'a').value=params[0];
	document.getElementById(txtNIT.id+'b').value=params[1];
	//if(params[2].length>0)document.getElementById(txtNIT.id+'c').value=params[2];
	document.getElementById(txtNIT.id+'d').value=params[3];
	document.getElementById(txtNIT.id+'e').value=params[4];
}

function KeyDownHandlerNIT(obj){
	if(event.keyCode==13)
		if(obj.value.length>0)
			return(TraerNIT(obj));
}

function validarEncomienda(){
	if(txtNITEmisor.value.length==0 || txtNITEmisora.value.length==0 || txtNITEmisorb.value.length==0 || txtNITEmisorc.value.length==0 || txtNITEmisord.value.length==0 || txtNITReceptor.value.length==0 || txtNITReceptora.value.length==0 || txtNITReceptorb.value.length==0 || txtNITReceptorc.value.length==0 || txtNITReceptord.value.length==0){
		alert('Debe ingresar todos los datos del emisor y el receptor para crear la encomienda.');
		return(false);
	}
	if(txtDescripcion.value.length==0||txtUnidades.value.length==0||txtPeso.value.length==0||txtVolumen.value.length==0||txtValorAvaluo.value.length==0){
		alert('Debe ingresar todos los datos de la encomienda.');
		return(false);
	}
	if(ddlRuta.value.length==0){
		alert('Debe seleccionar la ruta.');
		return(false);
	}
	return(true);
}
function Totales(){
	try{
		var totC=(parseInt(txtPeso.value.replace(/\,/g,''))*facPeso)+(parseInt(txtVolumen.value.replace(/\,/g,''))*facVolumen);
		parseValor(totC,txtCosto);
	}
	catch(err){
		txtCosto.value='';
	}
}
function parseValor(valor,objTot){
	objTot.value=formatoValor(valor);
}
function verEncomienda(){
	window.open('../aspx/AMS.Comercial.Encomiendas.aspx?enc='+<%=ViewState["Encomienda"]%>, 'ENCOMIENDA'+<%=ViewState["Encomienda"]%>, "width=340,height=340,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");
}
</script>
