<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RetirarBusAfiliado.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RetirarBusAfiliado" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="2"><b>Información del bus:</b>
			</td>
		</tr>
		<TR>
			<TD>
				<asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa del Vehículo :</asp:label></TD>
			<TD>
				<asp:textbox id="txtPlacaBus" ondblclick="ModalDialog(this,'SELECT mcat_placa AS placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>=0;', new Array(),1);"
					Font-Size="XX-Small" runat="server" Width="80px"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Número del Vehículo :</asp:label></TD>
			<TD>
				<asp:textbox id="txtPlacaBusa" Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="True"
					MaxLength="6"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px">
				<asp:Label id="Label6" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Retiro :</asp:Label></TD>
			<TD style="WIDTH: 374px; HEIGHT: 18px">
				<asp:textbox id="txtFechaRetiro" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server"
					Width="60px"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px">
				<asp:Label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Devolución Fondo Reposición :</asp:Label></TD>
			<TD style="WIDTH: 374px; HEIGHT: 18px">
				<asp:textbox id="txtFechaDevolucion" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server"
					Width="60px"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>
				<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Destino del Vehículo :</asp:label></TD>
			<TD>
				<asp:textbox id="txtDestino" Font-Size="XX-Small" runat="server" Width="200px"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Libro :</asp:label></TD>
			<TD>
				<asp:textbox id="txtLibro" Font-Size="XX-Small" runat="server" Width="100px"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 154px; HEIGHT: 18px">
				<asp:label id="Label23" runat="server" Font-Bold="True" Font-Size="XX-Small">Observaciones </asp:label></TD>
			<TD style="WIDTH: 374px; HEIGHT: 18px">
				<asp:textbox id="txtObservaciones" runat="server" Font-Size="XX-Small" Width="250px" TextMode="MultiLine"
					Height="60px"></asp:textbox></TD>
		</TR>
		<TR>
			<td align="center" colSpan="2" style="WIDTH: 545px"><asp:button id="btnGuardar" Runat="server" Text="Retirar" Font-Size="XX-Small" Font-Bold="True"></asp:button></td>
		</TR>
		<tr>
			<td colSpan="2">&nbsp;
				<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></td>
		</tr>
	</table>
	<br>
</DIV>
</SCRIPT>
