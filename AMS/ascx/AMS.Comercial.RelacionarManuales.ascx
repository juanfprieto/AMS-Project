<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RelacionarManuales.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RelacionarManuales" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:label id="RemeasLabel" runat="server" Font-Bold="True" Font-Size="Small">Relacionar Manuales</asp:label>
<HR style="WIDTH: 148.66%" width="148.66%" color="#000099" SIZE="4">
<FIELDSET style="WIDTH: 620px; HEIGHT: 103px"><LEGEND>Relacion&nbsp;De Manuales</LEGEND>
	<TABLE id="Table1" style="WIDTH: 548px; HEIGHT: 63px" cellSpacing="0" cellPadding="0" width="548"
		border="0">
		<TR>
			<TD><asp:label id="Label2" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Black">Nombre Procedimiento</asp:label><asp:textbox id="procedimiento" runat="server" Font-Size="XX-Small" Width="300px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" Font-Bold="True" Font-Size="XX-Small"
					ErrorMessage="Campo Requerido" ControlToValidate="procedimiento"></asp:requiredfieldvalidator></TD>
			<TD></TD>
		</TR>
		<tr>
			<td vAlign="top"><asp:label id="Label1" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Black">Descripcion del Proceso</asp:label><asp:textbox id="descripcion" runat="server" Width="380px" TextMode="MultiLine" Height="70px"></asp:textbox></td>
			<td></td>
		</tr>
		<TR>
			<TD><asp:label id="Label3" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Black">Archivo</asp:label>&nbsp;&nbsp;<INPUT id="filUpl" style="WIDTH: 360px; HEIGHT: 22px" type="file" size="40" name="filUpl"
					runat="server"></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD><asp:button id="btnSubir" onclick="Relacionar_Click" runat="server" Font-Size="XX-Small" Text="Relacionar Archivo"></asp:button></TD>
			<TD></TD>
		</TR>
	</TABLE>
</FIELDSET>
<P>
	<HR style="WIDTH: 148.66%" width="148.66%" color="#000099" SIZE="4">
<P></P>
<P>
</P>
