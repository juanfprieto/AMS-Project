<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Finanzas.AnulacionTesoreria.ascx.cs" Inherits="AMS.Finanzas.AMS_Finanzas_AnulacionTesoreria" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<table id="Table" class="filtersIn">
	<TR>
		<TD>Prefijo del Documento :
		<br /><asp:dropdownlist id="prefijoDocumento" AutoPostBack="True" class="dmediano" runat="server" onselectedindexchanged="prefijoDocumento_SelectedIndexChanged"></asp:dropdownlist></TD>
		</tr>
        <tr>
        <TD>Número :
		<br /><asp:label id="numeroTesoreria" class="lpequeno" runat="server"></asp:label></TD>
	</TR>
	<TR>
		<TD>Almacén :
		<br /><asp:dropdownlist id="almacen" class="dmediano" runat="server"></asp:dropdownlist></TD>
	</TR>
	<TR>

		<TD><asp:button id="aceptar" runat="server" Enabled="False" Text="Aceptar" CausesValidation="False" onclick="aceptar_Click"></asp:button></TD>
	</TR>
</TABLE>
<P><asp:placeholder id="holderAnulaciones" runat="server" Visible="False"></asp:placeholder></P>
<P><asp:panel id="panelValores" runat="server" Visible="False" Width="583px" style="margin-left: 8%;">
		<TABLE id="Table2">
        <tbody>
			<TR>
				<TD>
					<asp:Label id="lbDetalle" runat="server"></asp:Label></TD>
				<TD>
					<asp:TextBox id="detalleTransaccion" runat="server" TextMode="MultiLine"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbValor" runat="server"></asp:Label></TD>
				<TD>
					<asp:TextBox id="valorConsignado" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTotalEf" runat="server"></asp:Label></TD>
				<TD>
					<asp:TextBox id="totalEfectivo" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox></TD>
			</TR>
            </tbody>
		</TABLE>
	</asp:panel></P>
<P><asp:button id="guardar" runat="server" Enabled="False" Text="Guardar" onclick="guardar_Click" onClientClick="espera();"></asp:button>&nbsp;
	<asp:button id="cancelar" runat="server" Text="Cancelar" CausesValidation="False" onclick="cancelar_Click"></asp:button></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
</fieldset>
