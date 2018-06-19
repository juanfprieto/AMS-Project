<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Nomina.SubirPrimaNomina.ascx.cs" Inherits="AMS.Nomina.SubirPrimaNomina" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
<P>Seleccione el Periodo de primas y el periodo de pago correspondiente que sera 
	integrado.</P>

<table id="Table" class="filtersIn">
	<tbody>
		<tr>
			<td>
				PERIODO DE PAGO
			</td>
			<td>
				PERIODO DE PRIMA
			</td>
		</tr>
		<tr>
			<td>
				<asp:DropDownList id="ddlPeriodoPago" class="dpequeno" runat="server"></asp:DropDownList>
			</td>
			<td>
				<asp:DropDownList id="ddlPeriodoPrima" class="dmediano" runat="server"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:Button ID="btn_subir" runat="server" Text="Subir" OnClick="subirPrimas"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>
</fieldset>
