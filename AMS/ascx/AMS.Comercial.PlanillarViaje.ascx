<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillarViaje.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillarViaje" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Información del viaje:</b>
			</td>
		</tr>
		<TR>
			<TD style="WIDTH: 139px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia:</asp:label></TD>
			<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
		</TR>
		<asp:panel id="pnlAgencia" Runat="server" Visible="False">
			<TR>
				<TD style="WIDTH: 139px" vAlign="top">
					<asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">No. Viaje - Cod.Ruta :</asp:label></TD>
				<td>
					<asp:dropdownlist id="ddlRuta" Font-Size="XX-Small" runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist></TD>
			</TR>
			<asp:panel id="pnlRuta" Runat="server" Visible="False">
				<TR>
					<TD style="WIDTH: 107px">
						<asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Número Planilla :</asp:label></TD>
					<td>
						<asp:textbox id="txtCodigo" Font-Size="XX-Small" runat="server" Width="50px" MaxLength="6"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px">
						<asp:Label id="Label13" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:Label></TD>
					<TD style="WIDTH: 374px">
						<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server" Width="60px"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<td></TD>
					<TD align="left">
						<asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Width="89px"
							Text="Planillar"></asp:button></TD>
				</TR>
			</asp:panel>
		</asp:panel>
		<TR>
			<TD style="WIDTH: 545px" align="left" colSpan="2"><asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>&nbsp;</TD>
		</TR>
	</table>
	<br>
</DIV>
<SCRIPT language:javascript>
function cargarPlanillaDB(Obj){
	AMS_Comercial_PlanillarViaje.CargarPlanilla(Obj.value,CargarPlanilla_Callback);
}

function CargarPlanilla_Callback(response){
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	var txtFecha=document.getElementById("<%=txtFecha.ClientID%>");
	var info=true;
	respuesta=response.value;
	if(respuesta.Tables[0]==null)info=false;
	if(info && respuesta.Tables[0].Rows.length>0){
		txtFecha.value=convertirFecha(respuesta.Tables[0].Rows[0].FECHA);
	}
	else{
		txtFecha.value="";
		return;
	}
}
function convertirFecha(fechaIni){
	var someD = new Date();
	someD = fechaIni;
	var un = someD.getMonth()+1;
	if (un<9){
		un= '0'+un;
	}
	un = un.toString();
	var dosF = someD.getDate();
	if (dosF<9){
		dosF= '0'+dosF;
	}
	dosF = dosF.toString();
	var tresF = someD.getFullYear();
	tresF=tresF.toString();
	return(tresF+'-'+un+'-'+dosF);
}
</SCRIPT>
