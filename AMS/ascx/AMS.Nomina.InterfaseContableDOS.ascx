<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Nomina.InterfaseContableDOS.ascx.cs" Inherits="AMS.Nomina.AMS_Nomina_InterfaseContableDOS" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P>Incorporación de Comprobantes archivo plano</P>
<P>
	<TABLE id="Table1" cellSpacing="1" cellPadding="1" class="filtersIn" border="1">
		<TR>
			<TD>Vigencia(año/mes):</TD>
			<TD><asp:textbox id="txtVigencia" runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Comprobante de Diario:</TD>
			<TD><asp:textbox id="txtCompDiario" runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Número Documento:</TD>
			<TD><asp:textbox id="txtNumeroDocumento" runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Fecha del Comprobante:</TD>
			<TD><asp:textbox id="txtFechaComprobante" runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Codigo del Usuario:</TD>
			<TD><asp:textbox id="txtCodUsuario" runat="server"></asp:textbox></TD>
		</TR>
	</TABLE>
</P>
<P>Se generaran todas las interfases contables (ARPs,EPS..)Por Defecto esta 
	escogida la Ultima Quincena Liquidada por el Sistema.</P>
<P>
	<TABLE id="Table2" cellSpacing="1" cellPadding="1" class="filtersIn" border="1">
		<TR>
			<TD><asp:label id="Label1" runat="server" class="lpequeno" height="21px">Periodo a procesar </asp:label></TD>
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
<P><asp:button id="Generar" runat="server" Text="Generar"></asp:button></P>
<P><asp:hyperlink id="hl1" runat="server" Target="_blank" Visible="False"></asp:hyperlink></P>
<P><asp:hyperlink id="hl2" runat="server" Target="_blank"></asp:hyperlink></P>
<P><asp:hyperlink id="hl3" runat="server" Target="_blank"></asp:hyperlink></P>
<P><asp:hyperlink id="hl4" runat="server" Target="_blank"></asp:hyperlink></P>
<P><asp:label id="lb" runat="server" Visible="False">Label</asp:label></P>
