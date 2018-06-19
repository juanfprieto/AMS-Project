<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.DevolucionesOT.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_DevolucionesOT" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
Devoluciones de Ordenes de Trabajo Facturadas
    
	<table id="Table" class="filtersIn">
		<tr>
            <td>Prefijo Orden de Trabajo :
				<asp:dropdownlist id="ddPrefOt" OnSelectedIndexChanged="CambioTipoPref" AutoPostBack="True" class="dmediano" runat="server"></asp:dropdownlist>
            </td>
			<td>Orden de Trabajo :
				<asp:dropdownlist id="ddlOTS" OnSelectedIndexChanged="CambioOTLiqu" AutoPostBack="True" class="dmediano" runat="server"></asp:dropdownlist>
            </td>
        <tr>
			<td>
				<p>Factura Relacionada :
					<asp:dropdownlist id="ddlFactRel" OnSelectedIndexChanged="CambioFacLiqu" class="dgrande" AutoPostBack="True" runat="server"></asp:dropdownlist>
                </p>
                 
				<p>Cargo Relacionado :
					<asp:label id="lbCargo" runat="server" forecolor="RoyalBlue"></asp:label>
                </p>
			</td>
        </tr>
		</tr>
		<tr>
			<td>Prefijo Nota Devolución :
				<asp:dropdownlist id="ddlPrefNDC" class="dmediano" runat="server"></asp:dropdownlist></td>
            <td>
                Obervación devolución: <br />
                <asp:TextBox id="txtObserva" runat="server" TextMode="MultiLine" Rows="5" Width="55%"></asp:TextBox>
            </td>
		</tr>
		<tr>
			<td><asp:button id="btnDevolucion" runat="server" Text="Realizar Devolución" onclick="btnDevolucion_Click" ></asp:button></td>
		</tr>
	</table>
</fieldset>

<P><asp:label id="lb" runat="server"></asp:label></P>
