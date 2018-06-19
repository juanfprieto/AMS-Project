<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarRutasRecorrido.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarRutasRecorrido" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td colspan="2">
                <h3>Información de la ruta</h3>
			</td>
		</tr>
		<tr>
			<td >
				<asp:Label runat="server" Font-Bold="True" id="Label1">Ruta Principal:</asp:Label>
			</td>
			<td>
				<asp:dropdownlist id="ddlRuta" runat="server" Width="150px" AutoPostBack="True"></asp:dropdownlist>
            </td>
		</tr>
		<asp:Panel Runat="server" Visible="False" ID="pnlRuta">
			
			<TR>
				<TD>
					<asp:Label id="Label2" Font-Bold="True" runat="server">Descripción:</asp:Label>
                </TD>
				<TD>
					<asp:Label id="lblDesc" runat="server" Width="500px"></asp:Label>
                </TD>
			</TR>
			<TR>
				<TD>&nbsp;&nbsp;&nbsp;
					<asp:Label id="Label21" Font-Bold="True" runat="server">Origen:</asp:Label></TD>
				<TD>
					<asp:Label id="lblOrigen" runat="server" Width="500px"></asp:Label></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 14px">&nbsp;&nbsp;&nbsp;
					<asp:Label id="Label3" Font-Bold="True" runat="server">Destino:</asp:Label></TD>
				<TD style="HEIGHT: 14px">
					<asp:Label id="lblDestino" Font-Bold="False" runat="server" Width="500px"></asp:Label></TD>
			</TR>
		</asp:Panel>		
	</table>
	<br>
	<asp:Panel Runat="server" Visible="False" ID="pnlSubrutas">
		<TABLE class="filtersIn">
			<TR>
				<TD style="WIDTH: 247px">
                    <h3>Rutas intermedias:</h3>
				</TD>
			</TR>
			<TR>
				<td>
					<asp:textbox id="txtRuta" ondblclick="ModalDialog(this,'SELECT MR.MRUT_CODIGO AS CODIGO, MR.MRUT_DESCRIPCION AS DESCRIPCION FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PCO, DBXSCHEMA.PCIUDAD PCD WHERE MR.PCIU_COD=PCO.PCIU_CODIGO AND MR.PCIU_CODDES=PCD.PCIU_CODIGO ORDER BY MR.MRUT_CODIGO', new Array(),1)"
						runat="server" Width="80px"></asp:textbox>&nbsp;
					<asp:button id="btnAgregar" Font-Bold="True" Width="81px" Runat="server" Text="Agregar"></asp:button>
                </TD>				
			</TR>			
			<TR>
				<TD align="center" colspan="3">
					<asp:ListBox id="lstSubRutas" Width="647px" Runat="server" DataValueField="COD"
						DataTextField="NOM" Height="268px"></asp:ListBox>
                </TD>				
			</TR>
			<TR>
                <TD vAlign="middle" colspan="2">
					<asp:button id="btnSubir" Font-Bold="True" Width="81px" Runat="server" Text="Subir"></asp:button>
					
					<asp:button id="btnBajar" Font-Bold="True" Width="81px" Runat="server" Text="Bajar"></asp:button>
					
					<asp:button id="btnQuitar" Font-Bold="True" Width="81px" Runat="server" Text="Borrar"></asp:button>
                </TD>
            </tr>
            <tr>
				<TD  colSpan="3">
					<asp:button id="btnGuardar" Font-Bold="True" Runat="server" Text="Guardar Recorrido"></asp:button>
                </TD>			
			</TR>
		</TABLE>
	</asp:Panel>
</fieldset>
