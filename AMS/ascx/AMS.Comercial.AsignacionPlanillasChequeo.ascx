<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AsignacionPlanillasChequeo.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_AsignacionPlanillasChequeo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="fieltersIn">
		<tr>
			<td colSpan="3"><b>Información de la asignación:</b></td>
		</tr>
		<tr>
			<td><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Asignación :</asp:label></td>
			<td><asp:label id="lblNumero" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></td>
		</tr>
		<tr>
			<td><asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha :</asp:label></td>
			<td><asp:label id="lblFecha" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></td>
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
			<TD align="center">
            <asp:datagrid id="dgrDocumentos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" ShowFooter="True">
				<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
				<ItemStyle cssclass="item"></ItemStyle>
				<HeaderStyle cssclass="header"></HeaderStyle>
				<FooterStyle cssclass="footer"></FooterStyle>
				<Columns>
						<asp:BoundColumn DataField="NUMERO" HeaderText="Linea"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Inspector">
							<ItemTemplate>
								<asp:textbox id="txtInspector" onclick="MostrarInspector(this);" runat="server" Font-Size="XX-Small"
									Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
								<asp:textbox id="txtInspectora" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox>
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
			<TD align="center"><asp:button id="btnRecibir" Font-Size="XX-Small" Font-Bold="True" Text="Despachar" Runat="server"></asp:button></TD>
		</TR>
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
	</TABLE>
</DIV>
<script language="javascript">
<%=strActScript%>
	var cantTalonarios=<%=ViewState["CANTIDAD_TALONARIOS"]%>;
	function Total(tTalonarios,tInicio,tFin){
		var txtTalonarios=document.getElementById(tTalonarios);
		var txtInicio=document.getElementById(tInicio);
		var txtFin=document.getElementById(tFin);
		if(txtTalonarios.value.length>0 && txtInicio.value.length>0){
			try{
				txtFin.value=((parseInt(txtTalonarios.value)*cantTalonarios)+parseInt(txtInicio.value)-1);
			}
			catch(err){
				txtFin.value="";
			}
		}
	}
	function MostrarInspector(obj){
	//	var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODICARGO=\'IR\';';
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODIGO=2 and mag_codigo=9303;';
	ModalDialog(obj,sqlDsp, new Array(),1)
	}
</script>
