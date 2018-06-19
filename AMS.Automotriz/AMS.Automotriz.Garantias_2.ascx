<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.Garantias_2.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_Garantias_2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
<asp:panel id="Panel1" HorizontalAlign="Center" runat="server" Width="696px" Height="96px">&nbsp;Fecha:&nbsp; 
<asp:Label id="FechaActual" runat="server" class="lpequeno">AAAA-MM-DD</asp:Label>
<TABLE id="Table1" class="filtersIn">
		<TR>
			<TD>Operaciones:</TD>
			<TD>
				<asp:DropDownList id="ddlOperaciones" runat="server" class="dgrande" OnChange="CargarVin(this);"></asp:DropDownList></TD>
			<TD>
				<P>
					<asp:Button id="Agregar_O" runat="server" class="bpequeno" Text="Agregar" onclick="Agregar_O_Click"></asp:Button></P>
			</TD>
		</TR>
	</TABLE></asp:panel>
</fieldset>