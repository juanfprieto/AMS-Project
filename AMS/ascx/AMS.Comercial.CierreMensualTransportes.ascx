<%@ Control language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.CierreMensualTransportes.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_CierreMensualTransportes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="fieltersIn">
		<tr>
			<td><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Periodo :</asp:label></td>
			<td><asp:dropdownlist id="ddlperiodo" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD><asp:label id="Label18" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Inicial  :</asp:label></TD>
			<TD><asp:label id="txtFechaI" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Final  :</asp:label></TD>
			<TD><asp:label id="txtFechaF" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
	</table>
	<table id="Table2" class="fieltersIn">
		<TR>
			<td></td>
			<TD><asp:button id="btnAbrirPeriodo" Font-Bold="True" Font-Size="XX-Small" Width="80px" Text="Abrir Periodo"
					Runat="server"></asp:button></TD>
			<TD><asp:button id="btnVerCierre" Font-Bold="True" Font-Size="XX-Small" Width="80px" Text="Ver Periodo"
					Runat="server"></asp:button></TD>
			<TD><asp:button id="btnPrecierrePeriodo" Font-Bold="True" Font-Size="XX-Small" Width="103px" Text="Precierre Periodo"
					Runat="server"></asp:button></TD>
			<TD><asp:button id="btnCerrarPeriodo" Font-Bold="True" Font-Size="XX-Small" Text="Cierre Periodo"
					Runat="server"></asp:button></TD>
		</TR>
	</table>
	<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label><asp:panel id="pnlAbrirPeriodo" Runat="server" Visible="False">
		<TABLE id="Table3" class="fieltersIn">
			<TR>
				<TD>
					<asp:label id="Label21" Font-Bold="True" Font-Size="XX-Small" runat="server">ABRIR PERIODO</asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Periodo :</asp:label></TD>
				<TD>
					<asp:label id="lblPeriodo" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Inicial  :</asp:label></TD>
				<TD>
					<asp:label id="lblFechaInicial" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Final  :</asp:label></TD>
				<TD>
					<asp:label id="lblFechaFinal" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>
				<TD>
					<asp:button id="btnCrear" Font-Bold="True" Font-Size="XX-Small" class="bpequeno" Text="Crear Periodo"
						Runat="server"></asp:button></TD>
			</TR>
		</TABLE>
	</asp:panel><asp:panel id="pnlConsulta" Runat="server" Visible="False">
		<TABLE id="Table4" class="fieltersIn">
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrConceptos" runat="server" cssclass="datagrid" AutoGenerateColumns="True" ShowFooter="True">
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<FooterStyle cssclass="footer"></FooterStyle>
						<Columns>
							
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
	</asp:panel>
	<script language="javascript" type="text/javascript">
	var ddlperiodo=document.getElementById("<%=ddlperiodo.ClientID%>");
	
	</script>
</DIV>
