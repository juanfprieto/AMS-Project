<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.DeshacerRecepcionPapeleria.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_DeshacerRecepcionPapeleria" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información de la eliminación:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Número de Eliminación :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:label id="lblNumero" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:label id="lblFecha" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<TD style="WIDTH: 545px" colSpan="3"><B>Documentos:</B></TD>
		</TR>
		<TR>
			<TD align="center"><asp:datagrid id="dgrDocumentos" runat="server" AutoGenerateColumns="False">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn DataField="NUMERO" HeaderText="Linea" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Doc." ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
							<ItemTemplate>
								<asp:dropdownlist id="ddlTipoDocumento" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
								<asp:dropdownlist id="ddlPrefijo" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Talonarios" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
							<ItemTemplate>
								<asp:textbox id="txtTalonarios" Font-Size="XX-Small" Runat="server" Width="50px" MaxLength="4"></asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Inicio" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
							<ItemTemplate>
								<asp:textbox id="txtInicioDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'>
								</asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Fin" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
							<ItemTemplate>
								<asp:textbox id="txtFinDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'>
								</asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
		<TR>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center"><asp:button id="btnRecibir" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Eliminar"></asp:button></TD>
		</TR>
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
	</TABLE>
</DIV>
<script language="javascript">
<%=ViewState["CAPACIDADTALONARIOS"]%>
function Total(dTipo,tTalonarios,tInicio,tFin){
	var ddlTipo=document.getElementById(dTipo);
	var txtTalonarios=document.getElementById(tTalonarios);
	var txtInicio=document.getElementById(tInicio);
	var txtFin=document.getElementById(tFin);
	if(ddlTipo.value.length>0 && txtTalonarios.value.length>0 && txtInicio.value.length>0){
		try{
			numTalonarios=TraerCantidadTalonarios(ddlTipo.value.replace("|",""));
			txtFin.value=((parseInt(txtTalonarios.value)*numTalonarios)+parseInt(txtInicio.value)-1);
		}
		catch(err){
			txtFin.value="";
		}
	}
}
function TraerCantidadTalonarios(tdoc){
	for(n=0;n<arrTdocs.length;n++)
		if(arrTdocs[n]==tdoc)
			return(parseInt(arrTalonarios[n]));
	return(0);
}
function VerPrefijo(dlTipo,dlPref){
	ddlTipo=document.getElementById(dlTipo);
	ddlPref=document.getElementById(dlPref);
	strTipo=ddlTipo.value;
	if(strTipo.charAt(strTipo.length-1)=='|')
		ddlPref.style.display = "block";
	else
		ddlPref.style.display = "none";
}
<%=strActScript%>
//Prefijos
<%=ViewState["PREFIJOS"]%>
</script>
