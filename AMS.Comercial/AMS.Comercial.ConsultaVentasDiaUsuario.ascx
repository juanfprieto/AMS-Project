<%@ Control language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultaVentasDiaUsuario.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultaVentasDiaUsuario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset> 
	<table class="filtersIn">
		<tr>
			<td><asp:label id="Label1" runat="server" Font-Bold="True">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" runat="server" ></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD><asp:label id="Label6" runat="server" Font-Bold="True">Despachador :</asp:label></TD>
			<TD><asp:textbox id="txtNITTiquetero" onclick="MostrarPersonal(this,'D');" runat="server" 
					ReadOnly="True" Width="80px"></asp:textbox>&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px"><asp:label id="Label18" runat="server" Font-Bold="True">Fecha Inicial  :</asp:label></TD>
			<TD style="WIDTH: 386px"><asp:textbox id="txtFechaI" onkeyup="DateMask(this)" Width="62px" MaxLength="10"
					Runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px"><asp:label id="Label8" runat="server" Font-Bold="True">Fecha Final  :</asp:label></TD>
			<TD style="WIDTH: 386px"><asp:textbox id="txtFechaF" onkeyup="DateMask(this)" Width="62px" MaxLength="10"
					Runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<td></td>
			<TD style="WIDTH: 154px"><asp:button id="btnConsultar" Font-Bold="True" Runat="server" Text="Consultar"></asp:button></TD>
		</TR>
	</table>
	<table class="filtersIn">
		<TR>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center"><asp:button id="btnGenerar" Font-Bold="True" Runat="server" Text="Generar Reporte"></asp:button>&nbsp;&nbsp;
				<asp:hyperlink id="Ver" runat="server" Target="_blank" Visible="False">De Click Aqui para ver el Reporte</asp:hyperlink>&nbsp;</TD>
		</TR>
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True" runat="server"></asp:label></TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlConsulta" Runat="server" Visible="False">
		<TABLE class="filtersIn">
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrVentas" runat="server" Width="500" AutoGenerateColumns="true" ShowFooter="False"></asp:datagrid>
				</TD>	
			</TR>
		</TABLE>
		<TABLE class="filtersIn">
			<TR>
				<TD>
					<asp:label id="Label2" Font-Bold="True" runat="server">Total Tiquetes :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalTiquetes" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label3" Font-Bold="True" runat="server">Total Remesas :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalRemesas" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label4" Font-Bold="True" runat="server">Total Giros :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalGiros" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label5" Font-Bold="True" runat="server">Total Ingresos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalIngresos" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label10" Font-Bold="True" runat="server">Total Egresos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalEgresos" Font-Bold="True" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label7" Font-Bold="True" runat="server">Total :</asp:label></TD>
				<TD>
					<asp:label id="lblTotal" Font-Bold="True" Font-Size="Small" runat="server"></asp:label></TD>
			</TR>
		</TABLE>
	</asp:panel>
</fieldset>
<script language="javascript" type="text/javascript">
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	function MostrarPersonal(obj,flt){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
</script>
