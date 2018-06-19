<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Nomina.InterfaseContableWeb.ascx.cs" Inherits="AMS.Nomina.AMS_Nomina_InterfaseContableWeb" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
Interfase Contable Nomina
<fieldset>
<TABLE id="Table" class="filtersIn">
	<TR>
		<TD>Prefijo Comprobante de Diario Nómina:</TD>
		<TD><asp:dropdownlist id="ddlPrefijo" class="dpequeno" runat="server"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD>Número Documento Nómina:</TD>
		<TD><asp:textbox id="txtNumeroDocumento" class="tpequeno" runat="server"></asp:textbox></TD>
	</TR>
	<TR>
		<TD>Prefijo Comprobante de Diario Provisiones:</TD>
		<TD><asp:dropdownlist id="ddlprefProvisiones" class="dpequeno" runat="server"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD>Número Documento Provisiones:</TD>
		<TD><asp:textbox id="txtNumProvisiones" class="tpequeno" runat="server"></asp:textbox></TD>
	</TR>
	<TR>
		<TD>Prefijo Comprobante de Diario ARL:</TD>
		<TD><asp:dropdownlist id="ddlprefArp" class="dpequeno" runat="server"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD>Número Documento ARL:</TD>
		<TD><asp:textbox id="txtNumArp" class="tpequeno" runat="server"></asp:textbox></TD>
	</TR>
	<TR>
		<TD>Prefijo Comprobante de Diario Parafiscales:</TD>
		<TD><asp:dropdownlist id="ddlprefPara" class="dpequeno" runat="server"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD>Número Documento Parafiscales:</TD>
		<TD><asp:textbox id="txtNumPara" class="tpequeno" runat="server"></asp:textbox></TD>
	</TR>
	<TR>
		<TD>Fecha del Comprobante:</TD>
		<TD><asp:textbox id="txtFechaComprobante" onkeyup="DateMask(this)" class="tpequeno" runat="server"></asp:textbox></TD>
	</TR>
	<TR>
		<TD>Razón</TD>
		<TD><asp:textbox id="txtRazon" class="tpequeno" runat="server"></asp:textbox></TD>
	</TR>
</TABLE>
<P>

	<TABLE id="Table1" class="filtersIn">
		<TR>
			<TD><asp:label id="Label1" runat="server" class="lmediano">Período a procesar </asp:label></TD>
			<TD><asp:dropdownlist id="DDLQUIN" runat="server" class="dpequeno"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label3" runat="server">Mes</asp:label></TD>
			<TD><asp:dropdownlist id="DDLMES" runat="server" class="dpequeno"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label2" runat="server" class="lpequeno">Año</asp:label></TD>
			<TD><asp:dropdownlist id="DDLANO" runat="server" class="dpequeno"></asp:dropdownlist></TD>
		</TR>
	</TABLE>
</P>
<P><asp:button id="btnGenerar" onclick="Generar" runat="server" Text="Generar" 
 OnClientClick="espera();"></asp:button><asp:datagrid id="dgMovs" runat="server"></asp:datagrid></P>
<P><asp:button id="btnGuardar" onclick="GuardarIntefase" runat="server" Text="Contabilizar" Visible="False"  
 OnClientClick="espera();"></asp:button></P>
 </fieldset>
<P><asp:label id="lb" runat="server"></asp:label></P>
