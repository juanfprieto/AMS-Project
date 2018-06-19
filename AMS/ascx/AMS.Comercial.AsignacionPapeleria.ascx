<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AsignacionPapeleria.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_AsignacionPapeleria" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="fieltersIn">
		<tr>
			<td colSpan="3"><b>Información de la asignación:</b></td>
		</tr>
		<tr>
			<td><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Número de Asignación :</asp:label></td>
			<td><asp:label id="lblNumero" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></td>
			<td><asp:label id="lblFecha" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td><asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Responsable :</asp:label></td>
			<td><asp:textbox id="txtResponsable" onclick="MostrarPersonal(this,'D');" Font-Size="XX-Small" runat="server"
					Width="80px" ReadOnly="True"></asp:textbox></td>
		</tr>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<TABLE id="Table2" class="fieltersIn">
		<TR>
			<TD colSpan="3"><B>Documentos:</B></TD>
		</TR>
		<TR>
			<TD align="center"><asp:datagrid id="dgrDocumentos" runat="server" cssclass="datagrid" ShowFooter="True" AutoGenerateColumns="False">
					<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
					<ItemStyle cssclass="item"></ItemStyle>
					<HeaderStyle cssclass="header"></HeaderStyle>
					<FooterStyle cssclass="footer"></FooterStyle>
					<Columns>
						<asp:BoundColumn DataField="NUMERO" HeaderText="Linea"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Doc.">
							<ItemTemplate>
								<asp:dropdownlist id="ddlTipoDocumento" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Talonarios">
							<ItemTemplate>
								<asp:textbox id="txtTalonarios" Font-Size="XX-Small" Runat="server" Width="50px" MaxLength="4"></asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Inicio">
							<ItemTemplate>
								<asp:textbox id="txtInicioDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'>
								</asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Fin">
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
			<TD align="center"><asp:button id="btnRecibir" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Asignar"></asp:button></TD>
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
				txtFin.value=((parseInt(txtTalonarios.value)*numTalonarios)+parseInt(txtInicio.value)-1)
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
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	function MostrarPersonal(obj,flt){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	<%=strActScript%>
</script>
