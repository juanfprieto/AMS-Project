<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.TiqueteraPrepago.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_TiqueteraPrepago" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:label id="Label1" Font-Bold="True" runat="server">Tiquetera pre-pago</asp:label>
<HR width="100%" color="#3300ff" SIZE="2">
<FIELDSET style="WIDTH: 494px; HEIGHT: 112px"><LEGEND>Datos Tiquetera</LEGEND>
	<TABLE id="Table1" style="WIDTH: 424px; HEIGHT: 81px" cellSpacing="0" cellPadding="0" width="424"
		border="0">
		<TR>
			<TD>
				<asp:Label id="Label2" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Black">#Tiquetera</asp:Label></TD>
			<TD>
				<asp:TextBox id="numero" runat="server" Width="104px"></asp:TextBox></TD>
			<TD>
				<asp:Label id="Label4" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Black">Fecha</asp:Label></TD>
			<TD>
				<asp:TextBox id="fecha" runat="server" Width="80px"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label6" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">Inicio Seuencia</asp:Label></TD>
			<TD>
				<asp:TextBox id="inicio" runat="server" Width="50px"></asp:TextBox></TD>
			<TD>
				<asp:Label id="Label3" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">Fin Secuencia</asp:Label></TD>
			<TD>
				<asp:TextBox id="fin" runat="server" Width="50px"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD></TD>
			<TD></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<tr>
			<td>
				<asp:Button id="Crear" runat="server" Text="Crear" OnClick="Crear_Click"></asp:Button></td>
			<td>
				<P>
					<asp:RegularExpressionValidator id="RegularExpressionValidator1" Font-Bold="True" runat="server" Font-Size="XX-Small"
						ErrorMessage="Valor Inicio Incorrecto" ControlToValidate="inicio" ValidationExpression="\d+"></asp:RegularExpressionValidator>
					<asp:RegularExpressionValidator id="RegularExpressionValidator2" Font-Bold="True" runat="server" Font-Size="XX-Small"
						ErrorMessage="Valor Fin Incorrecto" ControlToValidate="fin" ValidationExpression="\d+"></asp:RegularExpressionValidator></P>
			</td>
		</tr>
	</TABLE>
	<asp:Label id="Label5" runat="server">Label</asp:Label>
</FIELDSET>
<HR width="100%" color="#3300ff" SIZE="2">
