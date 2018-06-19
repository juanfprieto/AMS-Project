<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarViaje.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarViaje" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td colSpan="2"><b>Información del viaje:</b>
			</td>
		</tr>
		<TR>
			<TD><asp:label id="Label11" Font-Bold="True"  runat="server">Agencia:</asp:label></TD>
			<TD><asp:dropdownlist id="ddlAgencia"  runat="server" AutoPostBack="True" Width="127px"></asp:dropdownlist></TD>
		</TR>
		<asp:panel id="pnlAgencia" Visible="False" Runat="server">
          <TR>
            <TD vAlign=top>
        <asp:label id=Label1 runat="server"  Font-Bold="True">Código Ruta :</asp:label></TD>
            <td>
        <asp:dropdownlist id=ddlRuta runat="server"  AutoPostBack="True" Width="330px"></asp:dropdownlist></TD></TR>
        <asp:panel id=pnlRuta Runat="server" Visible="False">
          <TR>
            <TD>
        <asp:label id=Label4 runat="server"  Font-Bold="True">Número Viaje :</asp:label></TD>
            <td>
        <asp:dropdownlist id=ddlNumero runat="server"  AutoPostBack="True" Width="430px"></asp:dropdownlist></TD></TR>
        <asp:panel id=pnlNumero Runat="server" Visible="False">
          <TR>
            <TD>
        <asp:label id=Label5 runat="server"  Font-Bold="True">Tipo de programación:</asp:label></TD>
            <td>
        <asp:dropdownlist id=ddlProgramacion runat="server"  AutoPostBack="True"></asp:dropdownlist></TD></TR>
          <TR>
            <TD style=" HEIGHT: 99px" vAlign=top>
        <asp:label id=Label12 runat="server"  Font-Bold="True">Horario : </asp:label></TD>
            <TD style="WIDTH: 386px; HEIGHT: 99px">
        <asp:ListBox id=lstFecha runat="server" Width="298px" Height="96px" ></asp:ListBox></TD></TR>
          <TR>
            <TD>
        <asp:label id=Label3 runat="server"  Font-Bold="True">Hora :</asp:label></TD>
            <TD style=" HEIGHT: 18px">
        <asp:DropDownList id=ddlHora runat="server" Width="40px"  OnChange="cargarCuadro()"></asp:DropDownList>&nbsp;:&nbsp; 
        <asp:DropDownList id=ddlMinuto runat="server" Width="48px"  OnChange="cargarCuadro()"></asp:DropDownList></TD></TR>
          <TR>
            <TD >
        <asp:Label id=Label6 runat="server"  Font-Bold="True">Fecha Salida :</asp:Label></TD>
            <TD >
        <asp:textbox id=txtFecha onkeyup=DateMask(this)  Runat="server" Width="85px"></asp:textbox></TD></TR>
          <TR>
            <TD style=" HEIGHT: 18px">
        <asp:Label id=Label7 runat="server"  Font-Bold="True">Configuración asociada :</asp:Label></TD>
            <TD >
        <asp:dropdownlist id=ddlConfiguracion  Runat="server"></asp:dropdownlist></TD></TR>
          <TR>
            <TD vAlign=top>
        <asp:label id=Label2 runat="server"  Font-Bold="True">Placa :</asp:label></TD>
            <td>
        <asp:dropdownlist id=ddlPlaca runat="server"  Width="150px" OnChange="cargarPlacaDB(this)"></asp:dropdownlist></TD></TR>
          <TR>
            <TD>
        <asp:label id=Label8 runat="server"  Font-Bold="True">Conductor Principal :</asp:label></TD>
            <td>
        <asp:textbox id=txtConductor onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODICARGO=\'CO\';', new Array(),1)" runat="server"  Width="80px" ReadOnly="True"></asp:textbox>&nbsp; 
        <asp:textbox id=txtConductora runat="server"  Width="300px" ReadOnly="True"></asp:textbox></TD></TR>
          <TR>
            <TD>
        <asp:label id=Label9 runat="server"  Font-Bold="True">Relevador 1 :</asp:label></TD>
            <td>
        <asp:textbox id=txtRelevador1 onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MRELEVADORES_TRANSPORTES MR  WHERE MR.MNIT_NIT=MNIT.MNIT_NIT AND MR.FECHA_DESDE <= CURRENT DATE AND MR.FECHA_HASTA >= CURRENT DATE;', new Array(),1)" runat="server"  Width="80px" ReadOnly="True"></asp:textbox>&nbsp; 
        <asp:textbox id=txtRelevador1a runat="server"  Width="300px" ReadOnly="True"></asp:textbox></TD></TR>
          <TR>
            <TD>
        <asp:label id=Label10 runat="server"  Font-Bold="True">Relevador 2 :</asp:label></TD>
            <td>
        <asp:textbox id=txtRelevador2 onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MRELEVADORES_TRANSPORTES MR  WHERE MR.MNIT_NIT=MNIT.MNIT_NIT AND MR.FECHA_DESDE <= CURRENT DATE AND MR.FECHA_HASTA >= CURRENT DATE;', new Array(),1)" runat="server"  Width="80px" ReadOnly="True"></asp:textbox>&nbsp; 
        <asp:textbox id=txtRelevador2a runat="server"  Width="300px" ReadOnly="True"></asp:textbox></TD></TR>
          <TR>
            <TD align=center colSpan=2>&nbsp;</TD></TR>
          <TR>
            <TD align=left>
        <asp:button id=btnGuardar  Font-Bold="True" Runat="server" Width="89px" Text="Actualizar"></asp:button></TD></TR>
          <TR>
            <TD align=center  colSpan=2>&nbsp;</TD></TR></asp:panel></asp:panel>
		        </asp:panel>	
	</table>
</fieldset>
<script language:javascript>
function cargarPlacaDB(Obj)
{
	AMS_Comercial_ModificarViaje.CargarPlaca(Obj.value,CargarPlaca_Callback);
}
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
function CargarPlaca_Callback(response)
{
	var txtConductor=document.getElementById("<%=txtConductor.ClientID%>");
	var txtRelevador1=document.getElementById("<%=txtRelevador1.ClientID%>");
	var txtConductora=document.getElementById("<%=txtConductora.ClientID%>");
	var txtRelevador1a=document.getElementById("<%=txtRelevador1a.ClientID%>");
	respuesta=response.value;
	if(respuesta.Tables[0].Rows.length>0){
		if(respuesta.Tables[0].Rows[0].CHOFER!=null)txtConductor.value=respuesta.Tables[0].Rows[0].CHOFER;
		else txtConductor.value="";
		if(respuesta.Tables[0].Rows[0].CHOFER2!=null) txtRelevador1.value=respuesta.Tables[0].Rows[0].CHOFER2;
		else txtRelevador1.value="";
		if(respuesta.Tables[0].Rows[0].NOMCONDUCTOR1!=null)txtConductora.value=respuesta.Tables[0].Rows[0].NOMCONDUCTOR1;
		else txtConductora.value="";
		if(respuesta.Tables[0].Rows[0].NOMCONDUCTOR2!=null) txtRelevador1a.value=respuesta.Tables[0].Rows[0].NOMCONDUCTOR2;
		else txtRelevador1a.value="";
	}
	else{
		txtConductor.value="";
		txtRelevador1.value="";
		txtConductora.value="";
		txtRelevador1a.value="";
	}
}
</script>

