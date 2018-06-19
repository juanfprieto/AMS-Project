<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.Manuales.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_Manuales" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<asp:label id="RemeasLabel" Font-Size="Medium" Font-Bold="True" runat="server">Manuales TMS</asp:label>
<P></P>
<HR style="WIDTH: 148.66%" width="148.66%" color="#000099" SIZE="4">
<FIELDSET style="WIDTH: 552px; HEIGHT: 80px"><LEGEND>Consulta De Manuales</LEGEND>
	<TABLE id="Table1" style="WIDTH: 520px; HEIGHT: 136px" cellSpacing="0" cellPadding="0" width="520"
		border="0">
		<TR>
			<td><asp:label id="Label1" Font-Size="XX-Small" Font-Bold="True" runat="server">Procedimiento a Consultar:</asp:label><asp:dropdownlist id="Procedimientos" Font-Size="XX-Small" runat="server" Width="352px" AutoPostBack="True"></asp:dropdownlist></TD>
			<td></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 71px">
<asp:Label id=Label2 runat="server" Font-Bold="True" Font-Size="XX-Small">Descripcion Proceso</asp:Label>
<asp:TextBox id=descripcion runat="server" Font-Bold="True" Font-Size="XX-Small" Width="384px" ReadOnly="True" TextMode="MultiLine" Height="64px"></asp:TextBox></TD>
			<TD style="HEIGHT: 71px"></TD>
		</TR>
		<tr>
			<td>
				<asp:Button id="Consultar" Font-Size="XX-Small" Font-Bold="True" runat="server" Text="Consultar"
					OnClick="Consultar_Click"></asp:Button></td>
			<td></td>
		</tr>
	</TABLE>
</FIELDSET> 
