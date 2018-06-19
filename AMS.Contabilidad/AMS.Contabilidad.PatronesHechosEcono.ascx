<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Contabilidad.PatronesHechosEcono.ascx.cs" Inherits="AMS.Contabilidad.AMS_Contabilidad_PatronesHechosEcono" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<meta content="False" name="vs_snapToGrid">
<p align="center"><asp:label id="ParamLabel" Font-Bold="True" runat="server" Width="248px">Parametrización Hechos Económicos</asp:label></p>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
<p align="center"><asp:label id="DetallesLabel" Font-Bold="True" runat="server">Detalles</asp:label></p>
<TABLE id="Detalles" style="WIDTH: 972px; HEIGHT: 587px" height="587" width="972" border="0"
	runat="server">
	<TR>
		<td>
			<p><asp:label id="DocumentoLabel" runat="server">Documento</asp:label></p>
			<asp:dropdownlist id="Documento" runat="server" Width="220px"></asp:dropdownlist></td>
		<td style="HEIGHT: 42px">
			<p><asp:label id="ConsecutivoLabel" runat="server">Consecutivo:</asp:label></p>
			<asp:textbox id="Consecutivo" runat="server" Enabled="False" ReadOnly="True"></asp:textbox></td>
		<td>
			<p><asp:label id="CuentaLabel" runat="server">Cuenta</asp:label></p>
			<asp:dropdownlist id="Cuenta" runat="server" Width="180px"></asp:dropdownlist></td>
		<td style="HEIGHT: 42px">
			<p><asp:label id="AplicacionLabel" runat="server">Aplicación</asp:label></p>
			<asp:textbox id="Aplicación" runat="server" Width="64px"></asp:textbox>%</td>
	</TR>
	<tr>
		<td>
			<p><asp:label id="TablaDetalleLabel" runat="server">Tabla Detalle</asp:label></p>
			<asp:dropdownlist id="TablaDetalle" runat="server" Width="220px" AutoPostBack="true"></asp:dropdownlist></td>
	</tr>
	<TR>
		<td style="HEIGHT: 42px">
			<p><asp:label id="CodigoDocLabel" runat="server">Código Documento</asp:label></p>
			<asp:dropdownlist id="CodigoDocumento" runat="server" Width="180px" AutoPostBack="true"></asp:dropdownlist></td>
		<td style="HEIGHT: 42px">
			<p><asp:label id="NumeroDocLabel" runat="server">Número Documento</asp:label></p>
			<asp:dropdownlist id="NumeroDocumento" runat="server" Width="300px"></asp:dropdownlist></td>
		<td style="HEIGHT: 42px">
			<p><asp:label id="CodigoDocRLabel" runat="server">Código Documento Referencia</asp:label></p>
			<asp:dropdownlist id="CodigoDocRefe" runat="server" Width="180px"></asp:dropdownlist></td>
		<td><asp:label id="NumeroDRefLabel" runat="server">Número Documento Referencia</asp:label><asp:dropdownlist id="NumeroDocRef" runat="server" Width="180px"></asp:dropdownlist></td>
	</TR>
	<tr>
		<td>
			<p><asp:label id="TablaNitLabel" runat="server">Tabla Nit</asp:label></p>
			<asp:dropdownlist id="TablaNit" runat="server" Width="180px"></asp:dropdownlist></td>
		<td>
			<p><asp:label id="NitLabel" runat="server">Nit</asp:label></p>
			<asp:dropdownlist id="Nit" runat="server" Width="300px"></asp:dropdownlist>
		<td>
			<p><asp:label id="CampoLLaveNitLabel" runat="server">Campo Llave Tabla Referencia Nit:</asp:label></p>
			<asp:dropdownlist id="CampoLLaveNit" runat="server" Width="180px"></asp:dropdownlist></td>
		</td>
		<td><asp:label id="CampoSolTabRefNitLabel" runat="server">Campo Solicitado Tabla Referencia Nit:</asp:label><asp:dropdownlist id="CampoSolTabRefNit" runat="server" Width="180px"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td style="HEIGHT: 62px"><asp:label id="TablaRefSedeLabel" runat="server">Tabla Referencia Sede:</asp:label><asp:dropdownlist id="TablaRefSede" runat="server" Width="180px" AutoPostBack="true"></asp:dropdownlist></td>
		<td style="HEIGHT: 62px">
			<p><asp:label id="CampoLLaveTabRefSedeLabel" runat="server">Campo Llave Tabla Referencia Sede:</asp:label></p>
			<asp:dropdownlist id="CampoLLaveTabRefSede" runat="server" Width="180px"></asp:dropdownlist></td>
		<TD style="HEIGHT: 62px"><asp:label id="CampoSolTabRefSedeLabel" runat="server">Campo Solicitado Tabla Referencia Sede:</asp:label><asp:dropdownlist id="CampoSolTabRefSede" runat="server" Width="180px"></asp:dropdownlist></TD>
		<TD style="HEIGHT: 62px"></TD>
	</tr>
	<TR>
		<TD><asp:label id="TablaRefCentroCostoLabel" runat="server">Tabla Referencia Centro Costo:</asp:label><asp:dropdownlist id="TablaRefCentroCosto" runat="server" Width="180px" AutoPostBack="true"></asp:dropdownlist></TD>
		<TD><asp:label id="CampoLLaveTabRefCentroCostolabel" runat="server">Campo Llave Tabla Referencia Centro Costo:</asp:label><asp:dropdownlist id="CampoLLaveTabRefCentroCosto" runat="server" Width="180px"></asp:dropdownlist></TD>
		<TD><asp:label id="CampoSolTabRefCentroCostoLabel" runat="server">Campo Solicitado Tabla Referencia Centro Costo:</asp:label><asp:dropdownlist id="CampoSolTabRefCentroCosto" runat="server" Width="180px"></asp:dropdownlist></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD><asp:label id="TablaRefRazonLabel" runat="server">Tabla Referencia Razon:</asp:label><asp:dropdownlist id="TablaRefRazon" runat="server" Width="180px" AutoPostBack="true"></asp:dropdownlist></TD>
		<TD>
			<p><asp:label id="CampoLLaveTabRefRazonLabel" runat="server">Campo Llave Tabla Referencia Razon:</asp:label></p>
			<asp:dropdownlist id="CampoLLaveTabRefRazon" runat="server" Width="180px"></asp:dropdownlist></TD>
		<TD><asp:label id="CampoSolTbaRefRazonLabel" runat="server">Campo Solicitado Tabla Referencia Razon:</asp:label><asp:dropdownlist id="CampoSolTbaRefRazon" runat="server" Width="180px"></asp:dropdownlist></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD style="HEIGHT: 4px">
			<p><asp:label id="naturalezaLabel" runat="server"> Naturaleza</asp:label></p>
			<asp:dropdownlist id="Naturaleza" runat="server" Width="180px"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD><asp:label id="TablaRefValorImputacionLabel" runat="server"> Tabla Referencia Valor Imputacion:</asp:label><asp:dropdownlist id="TablaRefValorImputacion" runat="server" Width="180px" AutoPostBack="true"></asp:dropdownlist></TD>
		<TD><asp:label id="CampoLLaveTbaRefValorImputacionLabel" runat="server"> Campo Llave Tabla Referencia Valor Imputacion:</asp:label><asp:dropdownlist id="CampoLLaveTbaRefValorImputacion" runat="server" Width="180px"></asp:dropdownlist></TD>
		<TD><asp:label id="CampoSolTabRefValorImputacionLabel" runat="server"> Campo Solicitado Tabla Referencia Valor Imputacion:</asp:label><asp:dropdownlist id="CampoSolTabRefValorImputacion" runat="server" Width="180px"></asp:dropdownlist></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD>
			<p><asp:label id="SumatoriaLabel" runat="server"> Es Sumatoria ?:</asp:label></p>
			<asp:dropdownlist id="Sumatoria" runat="server" Width="120px">
				<asp:ListItem Value="S" Selected="True">SI</asp:ListItem>
				<asp:ListItem Value="N">NO</asp:ListItem>
			</asp:dropdownlist></TD>
		<TD>
			<p><asp:label id="FormulaLabel" runat="server">Formula:</asp:label></p>
			<asp:textbox id="Formula" runat="server" Width="300px" Height="42px"></asp:textbox></TD>
	</TR>
</TABLE>
<HR style="WIDTH: 148.91%; HEIGHT: 8px" width="148.91%" color="#000099" SIZE="8">
<P><asp:button id="GRABAR" onclick="Grabar_Click" runat="server" Text="GRABAR"></asp:button>
	<asp:Label id="Label1" runat="server">Label</asp:Label></P>
