<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.CrearViaje.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_CrearViaje" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table align="LEFT">
		<tr>
			<td colSpan="2"><b>Información del viaje:</b>
			</td>
		</tr>
		<TR>
			<td><asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia:</asp:label></TD>
			<td><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small" AutoPostBack="True"></asp:dropdownlist></TD>
		</TR>
		<asp:PlaceHolder id="pnlAgencia" Runat="server" Visible="False">
			<TR>
				<TD vAlign="top">
					<asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Ruta Principal :</asp:label></TD>
				<td>
					<asp:dropdownlist id="ddlRuta" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
			</TR>
			<asp:PlaceHolder id="pnlRuta" Visible="False" Runat="server">
				<TR>
					<td>
						<asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Número Viaje :</asp:label></TD>
					<td>
						<asp:label id="lblNumViaje" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label15" Font-Bold="True" Font-Size="XX-Small" runat="server">Número Viaje Padre:</asp:label></TD>
					<td>
						<asp:dropdownlist id="ddlNumViajePadre" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo de programación:</asp:label></TD>
					<td>
						<asp:dropdownlist id="ddlProgramacion" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD HEIGHT: 75px" vAlign="top">
						<asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Horario : </asp:label></TD>
					<TD style="HEIGHT: 75px">
						<asp:ListBox id="lstFecha" runat="server" Height="78px" font-Size="XX-Small" Width="328px"></asp:ListBox></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label10" Font-Bold="True" Font-Size="XX-Small" runat="server">Hora :</asp:label></TD>
					<TD style="HEIGHT: 18px">
						<asp:DropDownList id="ddlHora" runat="server" font-Size="XX-Small" Width="42px" OnChange="cargarCuadro()"></asp:DropDownList>&nbsp;:&nbsp;
						<asp:DropDownList id="ddlMinuto" runat="server" font-Size="XX-Small" Width="42px" OnChange="cargarCuadro()"></asp:DropDownList></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px">
						<asp:Label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Salida :</asp:Label></TD>
					<TD style="HEIGHT: 18px">
						<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server" Width="75px"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px">
						<asp:Label id="Label14" Font-Bold="True" Font-Size="XX-Small" runat="server">Configuración asociada :</asp:Label></TD>
					<TD style="HEIGHT: 18px">
						<asp:dropdownlist id="ddlConfiguracion" Font-Size="XX-Small" Runat="server"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Número del Bus :</asp:label></TD>
					<td>
						<asp:textbox id="txtNumeroBus" ondblclick="ModalDialog(this,'SELECT rtrim(char(mbus_numero)) as numero,mcat_placa   concat \'  [\' concat coalesce(rtrim(char(mcon_cod)),\'\') concat \']\'  AS placa from DBXSCHEMA.mbusafiliado where testa_codigo>0;', new Array(),1);TraerBus(this);"
							Font-Size="XX-Small" runat="server" Width="59px"></asp:textbox>&nbsp;<INPUT id=btnSelv style="FONT-WEIGHT: bold; FONT-SIZE: xx-small" onclick=TraerBusA(); type=button value=Traer class="noEspera">
					</TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa del Bus :</asp:label></TD>
					<td>
						<asp:textbox id="txtNumeroBusa" Font-Size="XX-Small" runat="server" Width="60px" ReadOnly="True"
							MaxLength="6"></asp:textbox>&nbsp;
					</TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Conductor Principal :</asp:label></TD>
					<td>
						<asp:textbox id="txtConductor" onclick="MostrarConductor(this);" Font-Size="XX-Small" runat="server"
							Width="80px" ReadOnly="False"></asp:textbox>&nbsp;
						<asp:textbox id="txtConductora" Font-Size="XX-Small" runat="server" Width="250px" ReadOnly="True"></asp:textbox></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Relevador 1 :</asp:label></TD>
					<td>
						<asp:textbox id="txtRelevador1" onclick="MostrarRelevadores(this);" Font-Size="XX-Small" runat="server"
							Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
						<asp:textbox id="txtRelevador1a" Font-Size="XX-Small" runat="server" Width="250px" ReadOnly="True"></asp:textbox></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Número Planilla :</asp:label></TD>
					<td>
						<asp:label id="lblPlanilla" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
				</TR>
				<asp:PlaceHolder id="pnlPuestos" Visible="False" Runat="server">
					<TR>
						<td><asp:label id="Label13" Font-Bold="True" Font-Size="XX-Small" runat="server">Número Puestos Libres :</asp:label></TD>
						<td><asp:textbox id="txtPuestos" Font-Size="XX-Small" runat="server" Width="55px" MaxLength="3"></asp:textbox></TD>
					</TR>
				</asp:PlaceHolder>
				<TR>
					<TD align="center" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<td></TD>
					<TD align="left">
						<asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Width="87px"
							Text="Crear"></asp:button></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">&nbsp;</TD>
				</TR>
			</asp:PlaceHolder>
		</asp:PlaceHolder>
		<tr>
			<td colSpan="2">&nbsp;
				<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></td>
		</tr>
	</table>
	<br>
</DIV>
<script language:javascript>
var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
function TraerBusA(){
	TraerBus(document.getElementById("<%=txtNumeroBus.ClientID%>"));
}
function TraerBus(Obj)
{
	if(Obj.value.length==0)return;
	AMS_Comercial_CrearViaje.CargarPlaca(Obj.value,CargarPlaca_Callback);
	return(false);
}
function CargarPlaca_Callback(response)
{
	var txtConductor=document.getElementById("<%=txtConductor.ClientID%>");
	var txtRelevador1=document.getElementById("<%=txtRelevador1.ClientID%>");
	var txtConductora=document.getElementById("<%=txtConductora.ClientID%>");
	var txtRelevador1a=document.getElementById("<%=txtRelevador1a.ClientID%>");
	var txtPlaca=document.getElementById("<%=txtNumeroBusa.ClientID%>");
	var ddlConfiguracion=document.getElementById("<%=ddlConfiguracion.ClientID%>");
	respuesta=response.value;
	if(respuesta.Tables[0].Rows.length>0){
		if(respuesta.Tables[0].Rows[0].PLACA!=null)txtPlaca.value=respuesta.Tables[0].Rows[0].PLACA;
		else txtPlaca.value="";
		if(respuesta.Tables[0].Rows[0].CHOFER!=null)txtConductor.value=respuesta.Tables[0].Rows[0].CHOFER;
		else txtConductor.value="";
		/*if(respuesta.Tables[0].Rows[0].CHOFER2!=null) txtRelevador1.value=respuesta.Tables[0].Rows[0].CHOFER2;
		else txtRelevador1.value="";*/
		if(respuesta.Tables[0].Rows[0].NOMCONDUCTOR1!=null)txtConductora.value=respuesta.Tables[0].Rows[0].NOMCONDUCTOR1;
		else txtConductora.value="";
		/*if(respuesta.Tables[0].Rows[0].NOMCONDUCTOR2!=null) txtRelevador1a.value=respuesta.Tables[0].Rows[0].NOMCONDUCTOR2;
		else txtRelevador1a.value="";*/
		if(respuesta.Tables[0].Rows[0].CONFIG!=null)ddlConfiguracion.value=respuesta.Tables[0].Rows[0].CONFIG;
	}
	else{
		txtPlaca.value="";
		txtConductor.value="";
		txtRelevador1.value="";
		txtConductora.value="";
		txtRelevador1a.value="";
		ddlConfiguracion.value="";
	}
}
function KeyDownHandlerBus(obj){
	if(event.keyCode==13)
		if(obj.value.length>0)
			return(TraerBus(obj));
	return(true);
}
function MostrarPersonal(obj,flt){
	var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
	ModalDialog(obj,sqlDsp, new Array(),1)
}
function MostrarRelevadores(obj){
	var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MRELEVADORES_TRANSPORTES MP  WHERE MP.MNIT_NIT=MNIT.MNIT_NIT AND MP.FECHA_DESDE<=\'<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>\' AND MP.FECHA_HASTA>=\'<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>\' order by  MNIT.MNIT_NIT;';
	ModalDialog(obj,sqlDsp, new Array(),1)
}
function MostrarConductor(obj){
	var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODICARGO=\'CO\' order by  MNIT.MNIT_NIT;';
	ModalDialog(obj,sqlDsp, new Array(),1)
}
</script>
