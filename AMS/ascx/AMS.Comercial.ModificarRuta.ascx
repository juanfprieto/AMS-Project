<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarRuta.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarRuta" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language='javascript' src='../js/AMS.Tools.js' type='text/javascript'></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colspan="2"><b>Información de la ruta:</b>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Código :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlRuta" Font-Size="XX-Small" runat="server" Width="266px" OnChange="cargarRutaDB(this)"></asp:dropdownlist>&nbsp;
				<asp:Label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">crear:</asp:Label>
				<asp:textbox id="txtCodigo" Font-Size="XX-Small" runat="server" Width="70px" MaxLength="7"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Descripción :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:textbox id="txtDescripcion" Font-Size="XX-Small" Width="300px" Runat="server"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 20px"><asp:label id="Label13" Font-Bold="True" Font-Size="XX-Small" runat="server">Clase de ruta:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 20px"><asp:dropdownlist id="ddlClase" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 20px"><asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Ciudad de origen:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 20px"><asp:dropdownlist id="ddlCiudadOrigen" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 20px"><asp:label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Ciudad de destino:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 20px"><asp:dropdownlist id="ddlCiudadDestino" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label14" Font-Bold="True" Font-Size="XX-Small" runat="server">Distancia (kms): </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="txtDistancia" Font-Size="XX-Small" runat="server"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Tiempo aprox. del recorrido: </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="txtHrsRecorrido" Font-Size="XX-Small" runat="server" Width="30px" MaxLength="4"></asp:textbox>hrs.&nbsp;&nbsp;<asp:textbox id="txtMnsRecorrido" Font-Size="XX-Small" runat="server" Width="30px" MaxLength="2"></asp:textbox>min.</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label24" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor minimo: </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="txtValorMin" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
					Width="100px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor maximo: </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="txtValorMax" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
					Width="100px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor sugerido: </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="txtValorSugerido" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
					Width="100px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor peso encomiendas: </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="txtValPeso" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
					Width="100px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor volumen encomiendas: </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="txtValVol" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
					Width="100px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label10" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Conduce: </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="TxtValorConduce" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
					Width="100px"></asp:textbox></td>
		</tr>
		
		<tr>
		<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label15" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor sancion por incumplimiento de seguridad social</asp:label></td>
		<td style="WIDTH: 386px; HEIGHT: 24px"><asp:textbox id="Txtsinsegurida" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
					Width="100px"></asp:textbox></td>
		</tr>
		
		<%--<TR>
			<td style="WIDTH: 130px; HEIGHT: 24px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Paradero(no vende): </asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 24px">
				<asp:CheckBox id="chkParadero" runat="server" Text=" "></asp:CheckBox></td>
		</tr>--%>
		<TR>
			<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
		</TR>
		<TR>
			<td style="WIDTH: 130px"></td>
			<td align="left"><asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Actualizar"></asp:button></td>
		</TR>
		<TR>
			<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
		</TR>
	</table>
</DIV>
<script language:javascript>
function cargarRutaDB(Obj){
	AMS_Comercial_ModificarRuta.CargarRuta(Obj.value,CargarPlaca_Callback);
}


function CargarPlaca_Callback(response)
{
	var txtDesc=document.getElementById("<%=txtDescripcion.ClientID%>");
	var ddlClase=document.getElementById("<%=ddlClase.ClientID%>");
	var ddlCOrig=document.getElementById("<%=ddlCiudadOrigen.ClientID%>");
	var ddlCDest=document.getElementById("<%=ddlCiudadDestino.ClientID%>");
	var txtDist=document.getElementById("<%=txtDistancia.ClientID%>");
	var txtHrsR=document.getElementById("<%=txtHrsRecorrido.ClientID%>");
	var txtMnsR=document.getElementById("<%=txtMnsRecorrido.ClientID%>");
	var txtValMax=document.getElementById("<%=txtValorMax.ClientID%>");
	var txtValMin=document.getElementById("<%=txtValorMin.ClientID%>");
	var txtValSug=document.getElementById("<%=txtValorSugerido.ClientID%>");
	var txtValPeso=document.getElementById("<%=txtValPeso.ClientID%>");
	var txtValVol=document.getElementById("<%=txtValVol.ClientID%>");
	var TxtValorConduce = document.getElementById("<%=TxtValorConduce.ClientID%>");
	var Txtsinseg = document.getElementById("<%=Txtsinsegurida.ClientID%>");
	respuesta=response.value;
		if(respuesta.Tables[0].Rows.length>0)
		{
			txtDesc.value=respuesta.Tables[0].Rows[0].DESC;
			ddlClase.value=respuesta.Tables[0].Rows[0].CLASE;
			ddlCOrig.value=respuesta.Tables[0].Rows[0].ORIG;
			ddlCDest.value=respuesta.Tables[0].Rows[0].DEST;
			txtDist.value=respuesta.Tables[0].Rows[0].DIST;
			temp=respuesta.Tables[0].Rows[0].TIEMPO;
			txtHrsR.value=Math.floor(temp);
			txtMnsR.value=Math.round((temp-Math.floor(temp))*60);
			txtValMax.value=formatoValor(respuesta.Tables[0].Rows[0].VMAX);
			txtValMin.value=formatoValor(respuesta.Tables[0].Rows[0].VMIN);
			txtValSug.value=formatoValor(respuesta.Tables[0].Rows[0].VSUG);
			txtValPeso.value=formatoValor(respuesta.Tables[0].Rows[0].VPESO);
			txtValVol.value=formatoValor(respuesta.Tables[0].Rows[0].VVOL);
			TxtValorConduce.value = formatoValor(respuesta.Tables[0].Rows[0].VCONDUCE);
			Txtsinseg.value = formatoValor(respuesta.Tables[0].Rows[0].SANCION);
			if (Txtsinseg.value == "") {
			    Txtsinseg.value = "0";
            }
		} 
		else{
			txtDesc.value="";
			txtDist.value="";
			txtHrsR.value="";
			txtMnsR.value="";
			txtValMax.value="";
			txtValMin.value="";
			txtValSug.value="";
			txtValPeso.value="";
			txtValVol.value="";
			TxtValorConduce.value="";
			return;
		}
}
</script>
