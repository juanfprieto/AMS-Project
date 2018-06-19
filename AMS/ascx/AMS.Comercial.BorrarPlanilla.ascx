<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.BorrarPlanilla.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_BorrarPlanilla" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table id="Table1" class="fieltersIn">
		<tr>
			<td><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD>
				<asp:Label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Numero Planilla :</asp:Label></TD>
			<TD>
				<asp:textbox id="NroPlanilla" Font-Size="XX-Small" Width="62px" Runat="server" MaxLength="10"></asp:textbox></TD>
		</TR>
	</table>
	<table id="Table2" class="fieltersIn"">
		<TR>
			<TD><asp:button id="btnConsultaPlanilla" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Consultar/Borrar Planilla"></asp:button></TD>
			&nbsp;&nbsp;
			<TD><asp:button id="btnReportePlanilla" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Generar Reporte"
					Width="98px"></asp:button>
			<TD><asp:hyperlink id="Ver" runat="server" Visible="False" Width="240px">De Click Aqui para ver el Reporte</asp:hyperlink>&nbsp;&nbsp;</TD>
		</TR>
	</table>
	<table id="Table3" class="fieltersIn">
		<TR>
			<TD>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server" Width="742px"></asp:label></TD>
		</TR>
	</table>
</DIV>
