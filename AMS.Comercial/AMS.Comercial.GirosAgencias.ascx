<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.GirosAgencias.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_GirosAgencias" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<asp:label id="GirosLabel" Font-Size="Medium" Font-Bold="True" runat="server">Giros</asp:label>
<P></P>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
<TABLE id="Table1" style="WIDTH: 584px; HEIGHT: 259px" cellSpacing="1" cellPadding="1"
	width="584" border="0">
	<TR>
		<TD style="WIDTH: 222px; HEIGHT: 21px"><asp:label id="Label1" runat="server">Codigo Giro</asp:label></TD>
		<TD style="HEIGHT: 21px"><asp:label id="CodigoLabel" runat="server">Label</asp:label></TD>
	</TR>
	<TR>
		<TD style="WIDTH: 222px"><asp:label id="Label2" runat="server">Fecha Giro</asp:label></TD>
		<td><asp:label id="FechaLabel" runat="server">Label</asp:label></TD>
	</TR>
	<TR>
		<TD style="WIDTH: 222px; HEIGHT: 26px"><asp:label id="Label3" runat="server">Agencia Origen</asp:label></TD>
		<TD style="HEIGHT: 26px"><asp:dropdownlist id="Origen" runat="server" Width="250px"></asp:dropdownlist></TD>
	</TR>
	<tr>
		<td style="WIDTH: 222px"><asp:label id="Label4" runat="server">Agencia Destino</asp:label></td>
		<td><asp:dropdownlist id="Destino" runat="server" Width="250px"></asp:dropdownlist></td>
	</tr>
	</TR>
	<tr>
		<td style="WIDTH: 222px"><asp:label id="Label5" runat="server">Nombre Del Emisor</asp:label></td>
		<td><asp:textbox id="NombreEmisor" ondblclick="ModalDialog(this,'Select distinct NOMBRE_USUARIO from DBXSCHEMA.TUSUARIOREMESA', new Array(),1)"
				runat="server"></asp:textbox><FONT color="#ff0000">*</FONT></td>
	</tr>
	</TR>
	<tr>
		<td style="WIDTH: 222px; HEIGHT: 25px"><asp:label id="Label7" runat="server">Cedula Emisor</asp:label></td>
		<td style="HEIGHT: 25px"><asp:textbox id="CedulaEmisor" runat="server"></asp:textbox><FONT color="#ff0000">*
				<asp:RegularExpressionValidator id="RegularExpressionValidator5" runat="server" ErrorMessage="Cedula Invalida" ValidationExpression="\d+"
					ControlToValidate="CedulaEmisor">Cedula Invalida</asp:RegularExpressionValidator></FONT></td>
	</tr>
	</TR>
	<tr>
		<td style="WIDTH: 222px"><asp:label id="Label6" runat="server">Telefono Emisor</asp:label></td>
		<td><asp:textbox id="TelEmisor" runat="server"></asp:textbox><FONT color="#ff0000">*
				<asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" ErrorMessage="Numero Telefonico Invalido"
					ValidationExpression="\d+" ControlToValidate="TelEmisor">Numero Telefonico Invalido</asp:RegularExpressionValidator></FONT></td>
	</tr>
	</TR>
	<tr>
		<td style="WIDTH: 222px"><asp:label id="Label8" runat="server">Nombre Del Destinatario</asp:label></td>
		<td><asp:textbox id="NombreDestinatario" ondblclick="ModalDialog(this,'Select distinct NOMBRE_USUARIO from DBXSCHEMA.TUSUARIOREMESA', new Array(),1)"
				runat="server"></asp:textbox><FONT color="#ff0000">*</FONT></td>
	</tr>
	</TR>
	<tr>
		<td style="WIDTH: 222px"><asp:label id="Label9" runat="server">Cedula Destinatario</asp:label></td>
		<td><asp:textbox id="CedulaDestinatario" runat="server"></asp:textbox><FONT color="#ff0000">*
				<asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" ErrorMessage="Cedula Invalida" ValidationExpression="\d+"
					ControlToValidate="CedulaDestinatario">Cedula Invalida</asp:RegularExpressionValidator></FONT></td>
	</tr>
	<tr>
		<td><asp:label id="Label10" runat="server">Telefono Destinatario</asp:label></td>
		<td><asp:textbox id="TelDestinatario" runat="server"></asp:textbox><FONT color="#ff0000">*
				<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ErrorMessage="Numero Telefonico Invalido"
					ValidationExpression="\d+" ControlToValidate="TelDestinatario">Numero Telefonico Invalido</asp:RegularExpressionValidator></FONT></td>
	</tr>
	<tr>
		<td><asp:label id="Label11" runat="server">Monto Giro</asp:label></td>
		<td><asp:textbox id="ValorGiro" runat="server"></asp:textbox><FONT color="#ff0000">*
				<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Monto Invalido" ValidationExpression="\d+"
					ControlToValidate="ValorGiro">Monto Invalido</asp:RegularExpressionValidator></FONT></td>
	</tr>
	<tr>
		<td></td>
		<td></td>
	</tr>
	<tr>
		<td><asp:button id="Calcular" onclick="Calcular_Click" runat="server" Width="70px" Text="Calcular"></asp:button><asp:label id="Label17" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Red">Presione Calcular,Para Obtener Valores</asp:label></td>
		<td></td>
	</tr>
	<TR>
		<td>
			<P><asp:label id="Label13" Font-Size="X-Small" Font-Bold="True" runat="server" ForeColor="Red">* Campos Obligatorios</asp:label></P>
			<P><asp:button id="Guardar" onclick="Guardar_click" runat="server" Width="70px" Text="Guardar"></asp:button></P>
		</TD>
		<td>
			<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="300" border="0">
				<TR>
					<td><asp:label id="Label12" runat="server">Costo del Giro</asp:label></TD>
					<td><asp:textbox id="CostoGiro" Font-Bold="True" runat="server" ReadOnly="True" BorderColor="Black"
							BackColor="Lime">Costo Giro</asp:textbox><asp:label id="Label15" Font-Size="X-Small" Font-Bold="True" runat="server">Pesos</asp:label></TD>
				</TR>
				<TR>
					<td><asp:label id="Label14" Font-Size="X-Small" Font-Bold="True" runat="server" Font-Italic="True">VALOR TOTAL </asp:label></TD>
					<td><asp:textbox id="ValorTotal" Font-Bold="True" runat="server" ReadOnly="True" BorderColor="Black"
							BackColor="Lime">Valor Total+ Costo</asp:textbox><asp:label id="Label16" Font-Size="X-Small" Font-Bold="True" runat="server">Pesos</asp:label></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
</TABLE>
