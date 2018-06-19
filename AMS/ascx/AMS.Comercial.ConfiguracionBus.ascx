<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConfiguracionBus.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConfiguracionBus" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/prototype.js" type="text/javascript"></script>
<script language="javascript" src="../js/rico.js" type="text/javascript"></script>
<asp:panel id="Panel1" runat="server">
	<DIV align="center">&nbsp;</DIV>
	<DIV align="center">&nbsp;</DIV>
	<DIV align="center">
		<TABLE id="Table1" class="fieltersIn">
			<TR>
				<TD colSpan="2">
					<P><B>Por Favor Seleccione la configuración o cree una nueva:</B></P>
				</TD>
			</TR>
			<TR>
				<TD>Configuración&nbsp;:
				</TD>
				<td>
					<asp:DropDownList id="ddlConfiguracion" runat="server" Width="220px" onChange="CargarTabla(this);"
						Font-Size="XX-Small"></asp:DropDownList></TD>
			</TR>
			</TR>
			<TR>
				<TD>Numero de Filas :</TD>
				<td>
					<asp:TextBox id="txtFilas" runat="server" Width="40px" Font-Size="XX-Small"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD>Numero de Columnas :</TD>
				<td>
					<asp:TextBox id="txtColumnas" runat="server" Width="40px" Font-Size="XX-Small"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD>Vista Previa :</TD>
				<TD align="center"></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2"><SPAN id="spnAjax"></SPAN></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2">&nbsp;</TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2">
					<asp:Button id="btnReconfigurar" runat="server" Width="90px" Text="Reconfigurar"></asp:Button>&nbsp;&nbsp;
					<asp:Button id="btnVolver" runat="server" Width="90px" Text="Volver" Visible="False"></asp:Button>&nbsp;&nbsp;
					<asp:Button id="btnModificar" runat="server" Width="90px" Text="Editar"></asp:Button><BR>
				</TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
	</DIV>
</asp:panel>
<p align="center">
	<asp:Image id="Image2" runat="server" Visible="False" ImageUrl="../img/AMS.BotonExpandir.png"></asp:Image>
</p>
<asp:panel id="Panel2" runat="server" Height="67px" Width="912px" BorderColor="White" Visible="False"
	HorizontalAlign="Center" BackColor="White">
	<TABLE id="Table2" class="fieltersIn">
		<TR>
			<TD align="center" colSpan="3">&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center" width="200"><BR>
				<asp:PlaceHolder id="PlaceHolder2" runat="server"></asp:PlaceHolder><BR>
			</TD>
			<TD align="center" bgColor="#ffffff"><BR>
				<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder><BR>
			</TD>
			<TD align="center" width="200">
				<asp:Image id="Image1" runat="server" ImageUrl="../img/BUS2.gif"></asp:Image></TD>
		</TR>
		<TR>
			<TD align="center" colSpan="3"><BR>
				<asp:Button id="Button2" runat="server" Text="Guardar Modelo" align="center"></asp:Button><BR>
				<BR>
			</TD>
		</TR>
	</TABLE>
</asp:panel>
<P><asp:literal id="Literal1" runat="server"></asp:literal>
	<asp:placeholder id="PlaceHolder3" runat="server"></asp:placeholder></P>
<SCRIPT language:javascript>
function CargarTabla(Obj){
	if(Obj.value.length<1){
		document.getElementById('spnAjax').innerHTML="Seleccione una configuracion";
		return;}
	document.getElementById('spnAjax').innerHTML = AMS_Comercial_ConfiguracionBus.ConstruirPrevioDinamico(Obj.value).value;
	AMS_Comercial_ConfiguracionBus.CargarConfig(Obj.value,CargarConfig_Callback);
}

function CargarConfig_Callback(response){
	var txtFilas=document.getElementById("<%=txtFilas.ClientID%>");
	var txtColumnas=document.getElementById("<%=txtColumnas.ClientID%>");
	var respuesta=response.value;
	if(respuesta.Tables[0].Rows.length>0){
		txtFilas.value=respuesta.Tables[0].Rows[0].FILAS;
		txtColumnas.value=respuesta.Tables[0].Rows[0].COLUMNAS;
	}
	else{
		alert('No hay Informacion disponible');
		return;
	}	
}
</SCRIPT>
<asp:Label id="lblError" runat="server"></asp:Label>
