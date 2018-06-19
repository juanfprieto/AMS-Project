<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.ActDatosOrdenTemp.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ActDatosOrdenTemp" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<P>
	<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
	<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>

	<legend> Prefijo y Número de la Orden de Trabajo a Modificar&nbsp;</legend></P>
<table id="Table" class="filtersIn">  
	<tr>
		<td>Prefijo de la Orden :
		</td>
		<td><asp:dropdownlist id="ddlPrefijo" Runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefijo_SelectedIndexChanged"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Número de la Orden :
		</td>
		<td><asp:dropdownlist id="ddlNumero" class="dpequeno" Runat="server" AutoPostBack="true" onselectedindexchanged="ddlNumero_SelectedIndexChanged"></asp:dropdownlist></td>
	</tr>
</table>
<P></P>
<table id="Table1" class="filtersIn">
	<tr>
		<td>Cargo Actual de la Orden :</td>  
	    <td><asp:Label id="lbCargo" runat="server"></asp:Label></td>
	</tr>
</table>
<P></P>
<table id="Table3" class="filtersIn">
	<tr>
		<td>Nuevo Cargo de la Orden : </td>
		<td><asp:dropdownlist id="ddlCargo" class="tpequeno" Runat="server" AutoPostBack="True" onselectedindexchanged="ddlCargo_SelectedIndexChanged"></asp:dropdownlist></td>
	</tr>
</table>
<P></P>
<asp:Panel ID="pnlSeguros" Runat="server" Visible="False" Width="280px">
Información de la Aseguradora 
<TABLE id="Table4" class="filtersIn">
		<TR>
			<TD>Nit :</TD>
			<TD align="right">
				<asp:textbox id="nitAseguradora" ondblclick="ModalDialog(this, 'SELECT MN.mnit_nit AS NIT ,  MN.mnit_apellidos AS Aseguradora FROM mnit MN, paseguradora pa where mn.mnit_nit = pa.mnit_nit ORDER BY MN.mnit_nit', new Array());"
					Enabled="True" class="tmediano" runat="server" ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Siniestro :</TD>
			<TD align="right">
				<asp:textbox id="siniestro" Enabled="True" class="tmediano" runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Porcentaje Deducible:</TD>
			<TD align="right">
				<asp:textbox id="porcentajeDeducible" onkeyup="NumericMaskE(this,event)" Enabled="True" class="tpequeno"
					runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Valor Mínimo Deducible :</TD>
			<TD align="right">
				<asp:textbox id="valorMinDeducible" onkeyup="NumericMaskE(this,event)" Enabled="True" class="tpequeno" runat="server">
                </asp:textbox></TD>
		</TR>
		<TR>
			<TD>Número de Autorización:</TD>
			<TD align="right">
				<asp:textbox id="numeroAutorizacionAsegura" Enabled="True" class="tpequeno" runat="server"></asp:textbox></TD>
		</TR>
	</TABLE>
</asp:Panel>
<P></P>
<asp:Panel ID="pnlGarantia" Runat="server" Visible="False" Width="288px">
	Datos de la Casa Matríz de Garantía 
<TABLE id="Table5" class="filtersIn>
		<TR>
			<TD>Nit :</TD>
			<TD align="right">
				<asp:TextBox id="nitCompania" onclick="ModalDialog(this, 'SELECT MN.mnit_nit AS NIT , MN.mnit_apellidos AS casa_matriz FROM mnit MN, pCASAMATRIZ CM WHERE MN.MNIT_NIT = CM.MNIT_NIT ORDER BY MN.mnit_nit', new Array())"
					Enabled="True" runat="server" ReadOnly="false"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Número de Autorización:</TD>
			<TD align="right">
				<asp:TextBox id="numeroAutorizacionGarant" Enabled="True" class="tpequeno" runat="server"></asp:TextBox></TD>
		</TR>
	</TABLE>
</asp:Panel>
<P></P>
<P>
	<asp:button id="btnGuardar" Runat="server" Text="Guardar Cambios" Enabled="False" onclick="btnGuardar_Click"></asp:button><asp:button id="btnCancelar" Runat="server" Text="Cancelar" onclick="btnCancelar_Click"></asp:button></P>
<P>
	<asp:Label id="lb" runat="server"></asp:Label></P>
</fieldset>