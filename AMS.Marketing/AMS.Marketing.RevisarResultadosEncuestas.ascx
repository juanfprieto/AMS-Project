<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Marketing.RevisarResultadosEncuestas.ascx.cs" Inherits="AMS.Marketing.AMS_Marketing_RevisarResultadosEncuestas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table>
	<tr>
		<td>
			Escoja la encuesta a revisar
		</td>
		<td>
			<asp:DropDownList ID="ddlencuesta" Runat="server"></asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td>
		</td>
	</tr>
	<tr>
		<td>
			Revisar Resultados :
		</td>
		<td>
			<asp:RadioButtonList ID="rblenc" Runat="server" RepeatDirection="Horizontal">
				<asp:ListItem Value="F">Por Formulario</asp:ListItem>
				<asp:ListItem Value="R">Resultados Totales</asp:ListItem>
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td>
			<asp:Button ID="btnAceptar" Runat="server" Text="Aceptar" onclick="btnAceptar_Click"></asp:Button>
		</td>
	</tr>
</table>
<P>&nbsp;</P>
