<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ReportePersonal.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ReportePersonal" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td><asp:label id="Label1" Font-Bold="True" runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" runat="server"></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD>
				<asp:Label id="Label2" runat="server" Font-Bold="True">Responsable :</asp:Label></TD>
			<TD>
				<asp:textbox id="txtResponsable" onclick="MostrarPersonal(this);" runat="server"
					Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
				<asp:textbox id="txtResponsablea" runat="server" Width="251px" ReadOnly="True"></asp:textbox>
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label18" runat="server" Font-Bold="True">Fecha Planillas :</asp:Label></TD>
			<TD>
				<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Width="62px" Runat="server"
					MaxLength="10"></asp:textbox></TD>
		</TR>
	</table>
	<table class="filtersIn">		
		<TR>
			<TD align="center"><asp:button id="btnGenerar" Font-Bold="True" Runat="server" Text="Generar Reporte"></asp:button>&nbsp;&nbsp;
				<asp:hyperlink id="Ver" runat="server" Visible="False" Target="_blank">De Click Aqui para ver el Reporte</asp:hyperlink></TD>
		</TR>
		<TR>
			<TD>
				<asp:label id="lblError" Font-Bold="True" runat="server"></asp:label>
            </TD>
		</TR>
	</table>
</fieldset> 
<script language="javascript">
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	function MostrarPersonal(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\'D+\';';
		ModalDialog(obj,sqlDsp, new Array(),1);
	}
</script>
