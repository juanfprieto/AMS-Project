<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AnticiposManuales.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_AnticiposManuales" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Tools.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>


<DIV align="center">
	<table id="Table1" class="filtersIn">
		<tr>
			<td colSpan="2"><b>Adicionar Anticipo Manual:</b></td>
		</tr>
		<tr>
			<td><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia:</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small" class="dmediano" AutoPostBack="True"></asp:dropdownlist></td>
			<td>&nbsp;</TD>
		</tr>
		<asp:panel id="pnlPlanilla" Visible="False" Runat="server">
			<TR>
				<TD>
					<asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia Papeleria:</asp:label></TD>
				<TD>
					<asp:dropdownlist id="ddlAgenciaPapeleria" Font-Size="XX-Small" runat="server" AutoPostBack="True"
						Width="150px"></asp:dropdownlist></TD>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label22" Font-Bold="True" Font-Size="XX-Small" runat="server">Despachador :</asp:label></TD>
				<TD>
					<asp:dropdownlist id="ddlPersonalAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True"
						Width="355px"></asp:dropdownlist></TD>
				<td>&nbsp;</TD>
			</TR>
			<TR>
			<TR>
				<TD>
					<asp:label id="Label26" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo de Anticipo :</asp:label></TD>
				<TD>
					<asp:dropdownlist id="ddlTipoAsociar" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px">
						<asp:ListItem Value="A" Selected="True">Agencias</asp:ListItem>
						<asp:ListItem Value="B">Buses</asp:ListItem>
                        <asp:ListItem Value="C">Administración</asp:ListItem>
					</asp:dropdownlist></TD>
			</TR>
			<asp:panel id="Panelpla" Runat="server" Visible="true">
				<TR>
					<TD>
						<asp:label id="Label12" Font-Bold="True" visible ="true" Font-Size="XX-Small" runat="server">Número de Planilla</asp:label></TD>
					<TD>
						<%--<asp:dropdownlist id="ddlPlanilla" Visible="true" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist>--%>
                        <asp:textbox id="txtPlanilla" runat="server" class="tpequeno" onBlur="cargarPlaca(this)"></asp:textbox>
                    </TD>
					<td>&nbsp;</TD>
				</TR>
				<TR>
					<TD>
						<asp:label id="Label10" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa del Bus :</asp:label></TD>
					<td>
						<asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0;', new Array(),1)"
							Font-Size="XX-Small" runat="server" class="tpequeno" MaxLength="6"></asp:textbox>&nbsp;
					</TD>
				</TR>
			</asp:panel>
			<TR>
				<TD>
					<asp:label id="Label20" Font-Bold="True" Font-Size="XX-Small" runat="server">Numero Anticipo: </asp:label></TD>
				<TD>
					<asp:textbox id=TextNumero Font-Size="XX-Small" runat="server" class="tpequeno" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>" ReadOnly="False">
					</asp:textbox></TD>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Concepto:</asp:label></TD>
				<TD>
					<asp:dropdownlist id="ddlConcepto" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="231px"></asp:dropdownlist></TD>
				<td>&nbsp;</TD>
			</TR>
		</asp:panel>
	</table>
	<br>
	<asp:panel id="pnlDatos" Visible="false" Runat="server">
		<TABLE id="Table2" class="filtersIn">
			<TR>
				<TD colSpan="2"><B>Datos del Receptor:</B></TD>
			</TR>
			<asp:panel id="PanelBus" Runat="server" Visible="true">
				<TR>
					<TD vAlign="top">
						<asp:label id="Label23" Font-Bold="True" Font-Size="XX-Small" runat="server">Identificacion :</asp:label></TD>
					<TD vAlign="top">
						<asp:textbox id="txtNIT" ondblclick="MostrarComprador(this);" Font-Size="XX-Small" runat="server" Width="80px" MaxLength="50" ReadOnly="false"></asp:textbox></TD>
				</TR>
				<TR>
					<TD vAlign="top">
						<asp:label id="Label24" Font-Bold="True" Font-Size="XX-Small" runat="server">Nombre :</asp:label></TD>
					<TD vAlign="top">
						<asp:textbox id="txtNITa" Font-Size="XX-Small" runat="server" MaxLength="60"
							readonly="True"></asp:textbox></TD>
				</TR>
				<TR>
					<TD vAlign="top">
						<asp:label id="Label25" Font-Bold="True" Font-Size="XX-Small" runat="server">Apellido :</asp:label></TD>
					<TD vAlign="top">
						<asp:textbox id="txtNITb" Font-Size="XX-Small" runat="server" MaxLength="60"
							readonly="True"></asp:textbox></TD>
				</TR>
			</asp:panel>
			<asp:panel id="PanelAgencia" Runat="server" Visible="false">
				<TR>
					<TD vAlign="top">
						<asp:label id="Label27" Font-Bold="True" Font-Size="XX-Small" runat="server">Codigo Receptor:</asp:label></TD>
					<TD vAlign="top">
						<asp:textbox id="Textbox1" onclick="ModalDialog(this,'SELECT MNIT_NIT AS CODIGO, MPRO_NOMBCONTACTO AS NOMBRE from DBXSCHEMA.MPROVEEDOR ORDER BY NOMBRE', new Array(),1)"
							Font-Size="XX-Small" runat="server" class="tpequeno" MaxLength="50" ReadOnly="True"></asp:textbox></TD>
				</TR>
				<TR>
					<TD vAlign="top">
						<asp:label id="Label28" Font-Bold="True" Font-Size="XX-Small" runat="server">Nombre :</asp:label></TD>
					<TD vAlign="top">
						<asp:textbox id="Textbox1a" Font-Size="XX-Small" runat="server" Width="300px" MaxLength="60"
							readonly="True"></asp:textbox></TD>
				</TR>
			</asp:panel><!--TR>
				<TD style="WIDTH: 545px" colSpan="2" align="center">&nbsp;</TD>
			</TR--></TABLE>
		<BR>
		<TABLE id="Table3" class="filtersIn">
			<TR>
				<TD colSpan="2"><B>Datos del Anticipo/Servicio:</B></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label18" Font-Bold="True" Font-Size="XX-Small" runat="server" Width="177px">DocumentoReferencia/CLAVE :</asp:label></TD>
				<TD vAlign="top">
					<asp:textbox id="txtNumDocReferencia" Font-Size="XX-Small" runat="server" Width="140px" MaxLength="20"
						ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></TD>
				<TD vAlign="top">
					<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="60px" Runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<TD svAlign="top">
					<asp:label id="Label13" Font-Bold="True" Font-Size="XX-Small" runat="server">Descripcion :</asp:label></TD>
				<TD vAlign="top">
					<asp:textbox id="txtDescripcion" Font-Size="XX-Small" runat="server" Width="458px" ReadOnly="False"
						TextMode="MultiLine" Height="58px"></asp:textbox></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label14" Font-Bold="True" Font-Size="XX-Small" runat="server">Cantidad Consumo :</asp:label></TD>
				<TD vAlign="top">
					<asp:textbox id="txtCantidad" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
						Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label15" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Unidad :</asp:label></TD>
				<TD vAlign="top">
					<asp:textbox id="txtValorUnidad" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
						Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 167px" vAlign="top">
					<asp:label id="Label16" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Total :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtValorTotal" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
						Width="100px" MaxLength="11" ReadOnly="False"></asp:textbox></TD>
			</TR>
			<TR>
				<TD  vAlign="top">
				<TD  vAlign="top">

					<asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small"    Runat="server" Text="Continuar"></asp:button></TD>
			</TR> <!--TR>
				<TD style="WIDTH: 545px" colSpan="2" align="center">&nbsp;</TD>
			</TR--></TABLE>
	</asp:panel><asp:panel id="pnlConfirma" Visible="False" Runat="server">
		<TABLE id="Table4" class="filtersIn">
			<TR>
				<TD colSpan="2"><B>Datos del Anticipo/Servicio:</B></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label21" Font-Bold="True" Font-Size="XX-Small" runat="server">Numero Documento :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblNumDocumento" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label19" Font-Bold="True" Font-Size="XX-Small" runat="server">Documento Referencia :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblNumDocumentoRef" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblPlaca" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Receptor :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblNIT" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label17" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblFecha" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Descripcion :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblDescripcion" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Cantidad Consumo :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblCantidad" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Unidad :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblValorUnidad" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Total :</asp:label></TD>
				<TD vAlign="top">
					<asp:label id="lblValorTotal" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
				<TD vAlign="top">
					<asp:button id="btnRegistrar" Font-Bold="True" Font-Size="XX-Small" Width="80px" Runat="server"
						Text="Registrar"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:button id="btnAtras" Font-Bold="True" Font-Size="XX-Small" Width="80px" Runat="server"
						Text="Atras"></asp:button></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel><br>
</DIV>
<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label>

<script language="javascript">
var txtDescripcion=document.getElementById("<%=txtDescripcion.ClientID%>");
var txtCantidad=document.getElementById("<%=txtCantidad.ClientID%>");
var txtValorUnidad=document.getElementById("<%=txtValorUnidad.ClientID%>");
var txtValorTotal=document.getElementById("<%=txtValorTotal.ClientID%>");
var txtNIT=document.getElementById("<%=txtNIT.ClientID%>");
var txtNITa=document.getElementById("<%=txtNITa.ClientID%>");
var txtNITb=document.getElementById("<%=txtNITb.ClientID%>");

function cargarPlaca(obj){
    AMS_Comercial_AnticiposManuales.Planilla_Changed(obj.value, cargarPlaca_CallBack);
}

function cargarPlaca_CallBack(response) {
    var respuesta = response.value;
    var placa = document.getElementById("<%=txtPlaca.ClientID%>");
    placa.value = respuesta;
}

function MostrarComprador(obj){
	 var sqlDsp='SELECT NIT AS NIT,NOMBRES AS NOMBRE,APELLIDOS AS APELLIDOS,RELACION AS RELACION from DBXSCHEMA.VTRANSPORTE_NITSRELACION ORDER BY NOMBRES';
	 ModalDialog(obj,sqlDsp, new Array(),1)
	}

function TraerNIT(){

	 AMS_Comercial_AnticiposManuales.TraaerNIT(txtNIT.value,TraerNIT_Callback);
	return(false);
}

function TraerNIT_Callback(response){
	var respuesta=response.value;
	var params=respuesta.split('|');
	txtNITa.value=params[0];
	txtNITb.value=params[1];
	if(params[0].length>0);
}

function KeyDownHandlerNIT(){
	if(event.keyCode==13 && txtNIT.value.length>0)TraerNIT();
}




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
		var totV=parseFloat(txtCantidad.value.replace(/\,/g,''))*parseInt(txtValorUnidad.value.replace(/\,/g,''));
		parseValor(Math.round(totV,0),txtValorTotal);
	}
	catch(err){
		txtValorTotal.value='';
	}
}
function parseValor(valor,objTot){
	objTot.value=formatoValor(valor);
}



</script>
