<%@ Control language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.CierreDiarioAgencia.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_CierreDiarioAgencia" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="fieltersIn">
		<tr>
			<td><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD><asp:label id="Label18" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Cierre  :</asp:label></TD>
			<TD><asp:TextBox ID="txtFCierre" runat="server" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="62px" MaxLength="10" EnableViewState=true></asp:textbox></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Número Documento :</asp:label></TD>
			<td><asp:textbox id="txtDocumento" Font-Size="XX-Small" runat="server" Width="80px"></asp:textbox></TD>
		</TR>
		<TR>
			<TD vAlign="top"><asp:label id="Label36" Font-Bold="True" Font-Size="XX-Small" runat="server">Valor Consignado :</asp:label></TD>
			<TD vAlign="top"><asp:textbox id="txtValorConsignado" onkeyup="NumericMask(this)" Font-Size="XX-Small" runat="server"
					Width="86px" ReadOnly="False" MaxLength="11"></asp:textbox></TD>
		</TR>
	</table>
	<table id="Table2" class="fieltersIn">
		<TR>
			<td>&nbsp;</TD>
			<TD><asp:button id="btnVerCierre" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Ver Cierre"></asp:button></TD>
			<TD><asp:button id="btnCerrar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Cerrar Dia"></asp:button></TD>
		</TR>
	</table>
	<table id="Table3" class="fieltersIn">
		<TR>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center"><asp:button id="btnGenerar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Generar Reporte"></asp:button>&nbsp;&nbsp;
				<asp:hyperlink id="Ver" runat="server" Target="_blank" Visible="False">De Click Aqui para ver el Reporte</asp:hyperlink></TD>
		</TR>
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server" BackColor="Red"></asp:label></TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlConsulta" Runat="server" Visible="False">
		<TABLE id="Table4" class="fieltersIn">
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrVentas" runat="server" cssclass="datagrid" ShowFooter="False" AutoGenerateColumns="False">
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<Columns>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="AGENCIA" HeaderText="Cdgo"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_AGENCIA" HeaderText="Agencia"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NIT_TAQUILLERO" HeaderText="Nit"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_TAQUILLERO" HeaderText="Nombre"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CUENTA" HeaderText="Cuenta"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_CUENTA" HeaderText="Descripcion"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_DEBITO"
								HeaderText="INGRESOS"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_CREDITO"
								HeaderText="EGRESOS"></asp:BoundColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
		<TABLE id="Table5" class="fieltersIn">
			<TR>
				<TD>
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Ingresos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalIngresos" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Egresos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalEgresos" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Total :</asp:label></TD>
				<TD>
					<asp:label id="lblTotal" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Faltante/Sobrante :</asp:label></TD>
				<TD>
					<asp:label id="lblDiferencia" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></TD>
			</TR>
		</TABLE>
	</asp:panel></DIV>
<script language="javascript" type="text/javascript">
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	
</script>