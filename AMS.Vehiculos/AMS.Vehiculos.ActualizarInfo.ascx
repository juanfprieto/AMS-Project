<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.ActualizarInfo.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_ActualizarInfo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<table>
	<tbody>
		<tr>
			<td>Seleccione el Vehículo:
			</td>
			<td><asp:dropdownlist id="ddlVehiculo" runat="server"></asp:dropdownlist><asp:image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:image></td>
		</tr>
		<tr align="center">
			<td colSpan="2"><asp:button id="Button1" onclick="cargaDatos" runat="server" Text="Cargar"></asp:button></td>
		</tr>
	</tbody>
</table>
<asp:panel id="pan1" runat="server" Height="96px" Visible="False">Información a Actualizar 
<TABLE>
		<TR>
			<TD>Fecha Manifiesto
			</TD>
			<TD>
				<asp:TextBox id="txtFechaMan" onkeyup="DateMask(this)" Visible="False" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Número Manifiesto
			</TD>
			<TD>
				<asp:TextBox id="txtNumMan" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Número DO
			</TD>
			<TD>
				<asp:TextBox id="txtNumDO" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Aduana</TD>
			<TD>
				<asp:TextBox id="txtAduana" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Levante</TD>
			<TD>
				<asp:TextBox id="txtLevante" Runat="server"></asp:TextBox></TD>
		</TR>
	</TABLE>
<asp:Button id="Enviar" onclick="ActualizarInfo" Text="Actualizar" Runat="server"></asp:Button></asp:panel>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
