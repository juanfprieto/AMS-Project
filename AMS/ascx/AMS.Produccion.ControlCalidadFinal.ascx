<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Produccion.ControlCalidadFinal.ascx.cs" Inherits="AMS.Produccion.AMS_Produccion_ControlCalidadFinal" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table style="WIDTH: 600px">
	<tbody>
		<tr>
			<td>
				<fieldset><legend>Datos del vehículo:</legend>
					<table class="main">
						<tbody>
							<tr>
								<td colSpan="4">
									<p><asp:label id="Label10" runat="server" forecolor="RoyalBlue">INFORMACIÓN GENERAL</asp:label></p>
								</td>
							</tr>
							<tr>
								<td>
									<p>&nbsp;&nbsp;&nbsp;&nbsp;VIN:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtVIN" ondblclick="ModalDialog(this,'SELECT MC.MCAT_VIN, MC.PCAT_CODIGO, PC.PCOL_DESCRIPCION FROM MVEHICULO MV, MCATALOGOVEHICULO MC, PCOLOR PC WHERE MV.MCAT_VIN=MC.MCAT_VIN AND MC.PCOL_CODIGO=PC.PCOL_CODIGO AND MV.TEST_TIPOESTA=1;',new Array(),1)"
											runat="server"></asp:TextBox></p>
								</td>
								<td colSpan="2">
									<p>&nbsp;&nbsp;&nbsp;&nbsp;Catálogo:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtVINa" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox></p>
								</td>
								<td>
									<p>&nbsp;&nbsp;&nbsp;&nbsp;Color:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtVINb" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox></p>
								</td>
							</tr>
							<tr>
								<td colspan="3">
									<p>&nbsp;&nbsp;&nbsp;&nbsp;Motor:<br>
										&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtMotor" runat="server"></asp:TextBox></p>
								</td>
							</tr>
							<tr>
								<td colspan="3"><br>
									<asp:button id="btnSeleccionar" runat="server" Enabled="True" Text="Actualizar" onclick="btnSeleccionar_Click"></asp:button>
								</td>
							</tr>
						</tbody>
					</table>
				</fieldset>
			</td>
		</tr>
	</tbody>
</table>
<asp:label id="lblInfo" runat="server"></asp:label>
<script language="javascript" type="text/javascript">
	document.getElementById("<%=txtVIN.ClientID%>").focus();
</script>
