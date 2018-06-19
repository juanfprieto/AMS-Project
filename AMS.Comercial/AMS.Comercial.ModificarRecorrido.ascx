<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarRecorrido.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarRecorrido" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td colspan="2"><b>Información de la ruta:</b>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label runat="server"  Font-Bold="True" id="Label1">Ruta Principal:</asp:Label>
			</td>
			<td>
				<asp:dropdownlist id="ddlRuta"  runat="server" Width="150px" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<asp:Panel Runat="server" Visible="False" ID="pnlRuta">
			<TR>
				<td>&nbsp;</TD>
				
			</TR>
			<TR>
				<TD>&nbsp;&nbsp;&nbsp;
					<asp:Label id="Label2" Font-Bold="True"  runat="server">Descripción:</asp:Label></TD>
				<TD>
					<asp:Label id="lblDesc"  runat="server" Width="500px"></asp:Label></TD>
			</TR>
			<TR>
				<TD>&nbsp;&nbsp;&nbsp;
					<asp:Label id="Label21" Font-Bold="True"  runat="server">Origen:</asp:Label></TD>
				<TD>
					<asp:Label id="lblOrigen"  runat="server" Width="500px"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 14px">&nbsp;&nbsp;&nbsp;
					<asp:Label id="Label3" Font-Bold="True"  runat="server">Destino:</asp:Label></TD>
				<TD style="HEIGHT: 14px">
					<asp:Label id="lblDestino" Font-Bold="False"  runat="server" Width="500px"></asp:Label></TD>
			</TR>
		</asp:Panel>
		<tr>
			<td style="WIDTH: 84px">&nbsp;</td>
			
		</tr>
	</table>
	<br>
	<asp:Panel Runat="server" Visible="False" ID="pnlSubrutas">
		<TABLE class="filtersIn">
			<TR>
				<TD>
                    <h3>Ciudades intermedias:</h3>
				</TD>				
			</TR>
			<TR>
				<td>
					<asp:dropdownlist id="ddlCiudad"  runat="server"></asp:dropdownlist>&nbsp;
					<asp:button id="btnAgregar" Font-Bold="True"  Width="81px" Runat="server"
						Text="Agregar"></asp:button></TD>
			</TR>
			<TR>
				<td>Origen:&nbsp;
					<asp:Label id="lblOrigenR"  runat="server"></asp:Label>
                </TD>				
			</TR>
			<TR>
				<TD align="center" colspan="3">
					<asp:ListBox id="lstSubRutas"  Width="647px" Runat="server" DataValueField="COD"
						DataTextField="NOM" Height="268px"></asp:ListBox></TD>

            </tr>
            <tr>
				<TD vAlign="middle">
					<asp:button id="btnSubir" Font-Bold="True"  Width="81px" Runat="server" Text="Subir"></asp:button>
					<asp:button id="btnBajar" Font-Bold="True"  Width="81px" Runat="server" Text="Bajar"></asp:button>
					<asp:button id="btnQuitar" Font-Bold="True"  Width="81px" Runat="server" Text="Borrar"></asp:button>
                </TD>
			</TR>
			<TR>
				<td>Destino:&nbsp;
					<asp:Label id="lblDestinoR"  runat="server"></asp:Label></TD>
				
			</TR>
			<TR>
				<td>
					<asp:CheckBox id="chkInversa" Runat="server" Text="&amp;nbsp;Crear/modificar ruta inversa."></asp:CheckBox></TD>
				
			</TR>
			<TR>
				<td>
					<asp:CheckBox Checked=False id="chkRutas" Runat="server" Text="&amp;nbsp;Asociar rutas al recorrido (elimina actuales)."></asp:CheckBox></TD>
				
			</TR>
			<TR>
				<TD colSpan="2"><BR>
					<asp:button id="btnGuardar" Font-Bold="True" Runat="server" Text="Guardar Recorrido"></asp:button></TD>
				
			<TR>
				<TD >&nbsp;
					<asp:Label id="lblError"  runat="server"></asp:Label>
                </TD>
			</TR>
		</TABLE>
	</asp:Panel>
</fieldset>
