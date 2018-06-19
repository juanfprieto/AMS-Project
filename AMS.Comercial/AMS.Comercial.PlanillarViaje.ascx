<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillarViaje.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillarViaje" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>

<fieldset>
	<table class="filtersIn">
		<tr>
			<td colSpan="2">
                <b>Información del viaje:</b>
			</td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label11" Font-Bold="True"  runat="server">Agencia:</asp:label>
            </td>
			<td>
                <asp:dropdownlist id="ddlAgencia"  runat="server" AutoPostBack="True"></asp:dropdownlist>
            </td>
		</tr>
		<asp:panel id="pnlAgencia" Runat="server" Visible="False">
			<tr>
				<td style="vAlign="top">
					<asp:label id="Label1" Font-Bold="True"  runat="server">No. Viaje - Cod.Ruta :</asp:label>
                </td>
				<td>
					<asp:dropdownlist id="ddlRuta"  runat="server" AutoPostBack="True" Width="150px"></asp:dropdownlist>
                </td>
			</tr>
			<asp:panel id="pnlRuta" Runat="server" Visible="False">
				<tr>
					<td>
						<asp:label id="Label4" Font-Bold="True"  runat="server">Número Planilla :</asp:label>
                    </td>
					<td>
						<asp:textbox id="txtCodigo"  runat="server" Width="50px" MaxLength="6"></asp:textbox>
                    </td>
				</tr>
				<tr>
					<td>
						<asp:Label id="Label13" Font-Bold="True"  runat="server">Fecha :</asp:Label>
                    </td>
					<td>
						<asp:textbox id="txtFecha" onkeyup="DateMask(this)"  Runat="server" Width="60px"></asp:textbox>
                    </td>
				</tr>
				<tr>
					<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
				</tr>
				<tr>
					<td></td>
					<td align="left">
						<asp:button id="btnGuardar" Font-Bold="True"  Runat="server" Width="89px" Text="Planillar"></asp:button>
                    </td>
				</tr>
			</asp:panel>
		</asp:panel>
		<tr>
			<td style="align="left" colSpan="2">
                <asp:label id="lblError" Font-Bold="True"  runat="server"></asp:label>&nbsp;
            </td>
		</tr>
	</table>
</fieldset>
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
	var dosF = someD.getdate();
	if (dosF<9){
		dosF= '0'+dosF;
	}
	dosF = dosF.toString();
	var tresF = someD.getFullYear();
	tresF=tresF.toString();
	return(tresF+'-'+un+'-'+dosF);
}
</SCRIPT>
