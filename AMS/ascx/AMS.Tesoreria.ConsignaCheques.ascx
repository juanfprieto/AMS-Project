<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ConsignacionCheques" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<fieldset>
Tipo de Proceso
<p></p>
<table id="table" class="filtersIn">
	<tbody>
		<tr>
			<td>Almacen :
			</td>
			<td><asp:dropdownlist id="almacen" class="dpequeno" runat="server"></asp:dropdownlist></td>
		</tr>
	    <tr>
			<td>Escoja la acción a seguir :
			</td>
			<td><asp:dropdownlist id="tipoConsignacion" AutoPostBack="true" runat="server" onSelectedIndexChanged="Cambiar_Accion"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>Prefijo del Documento :
			</td>
			<td><asp:dropdownlist id="prefijoDocumento" AutoPostBack="True" class="dpequeno" runat="server" onSelectedIndexChanged="Cambiar_Prefijo"></asp:dropdownlist></td>
            </tr>
            <tr>
			<td>Número :
			</td>
			<td><asp:label id="numeroTesoreria" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td><asp:button id="aceptar" onclick="Aceptar_Valores" runat="server" Enabled="False" Text="Aceptar"
					CausesValidation="False"></asp:button></td>
		</tr>
	</tbody>

    
   </table>
<p></p>
<p>Datos de la Operación
</p>
<p><asp:placeholder id="holderConsignacionCC" runat="server"></asp:placeholder></p>
<p></p>
<asp:placeholder id="holderDevolucionCC" runat="server"></asp:placeholder>
<p></p>
<asp:placeholder id="holderRemisionFinanciera" runat="server"></asp:placeholder>
<p></p>
<asp:placeholder id="holderDevolucionFinanciera" runat="server"></asp:placeholder>
<p></p>
<asp:placeholder id="holderTrasladoCCCarta" runat="server"></asp:placeholder>
<p></p>
<asp:placeholder id="holderTrasladoCCCheque" runat="server"></asp:placeholder>
<p></p>
<asp:placeholder id="holderNotasBancarias" runat="server"></asp:placeholder>
<p></p>
<asp:placeholder id="holderAnulaciones" runat="server"></asp:placeholder>
<p></p>
<asp:placeholder id="holderChequesProveedores" runat="server"></asp:placeholder>
<p></p>
Detalle y Valores
<p></p>
<asp:panel id="panelValores" runat="server" Visible="False">
	<TABLE>
		<TR>
			<TD>
				<asp:Label id="lbDetalle" runat="server"></asp:Label></TD>
			<TD>
				<asp:TextBox id="detalleTransaccion" runat="server" TextMode="MultiLine"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="lbValor" runat="server"></asp:Label>
			</TD>
			<TD>
				<asp:TextBox id="valorConsignado" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="lbTotalEf" runat="server"></asp:Label></TD>
			<TD>
				<asp:TextBox id="totalEfectivo" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox></TD>
		</TR>
	</TABLE>
</asp:panel>
<p></p>
<asp:button id="guardar" onclick="Guardar_Accion" runat="server" Enabled="False" Text="Guardar"></asp:button>&nbsp;&nbsp;
<asp:button id="cancelar" onclick="Cancelar_Accion" runat="server" Text="Cancelar" CausesValidation="False"></asp:button>
<p><asp:label id="lb" runat="server"></asp:label></p>
</fieldset>


