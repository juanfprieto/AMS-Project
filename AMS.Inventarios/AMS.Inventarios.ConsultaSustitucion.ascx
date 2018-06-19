<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.ConsultaSustitucion.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ConsultaSustitucion" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="GirosLabel" Font-Size="Medium" Font-Bold="True" runat="server">Consulta de Sustituciones</asp:label>

<fieldset>
    <TABLE id="Table1" class="filtersIn">
	    <TR>
		    <TD>
			    <asp:Label id="Label1" runat="server" Font-Bold="True" ForeColor="Black">Repuesto:</asp:Label>
                
			    <asp:TextBox id="Repuesto" class="tpequeno" runat="server"></asp:TextBox>
            </TD>
		    
	    </TR>
	    <TR>
		    <TD>
			    <asp:Button id="Buscar" runat="server" Text="Buscar" OnClick="Buscar_Click"></asp:Button></TD>
		    <TD></TD>
		    <TD></TD>
	    </TR>
    </TABLE>
</fieldset>
<asp:Panel id="Panel1" runat="server" Height="304px" Width="654px" Visible="False">
	<FIELDSET>
    <LEGEND>Sustituciones</LEGEND>
		<TABLE id="Table2" class="filtersIn">
			<TR>
				<TD>
					<asp:Label id="Label2" Font-Bold="True" runat="server" ForeColor="Black">Nombre Repuesto:</asp:Label></TD>
				<TD>
					<asp:Label id="NomLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label4" Font-Bold="True" runat="server" ForeColor="Black">Codigo Actual</asp:Label></TD>
				<TD>
					<asp:Label id="CodActual" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label6" Font-Bold="True" runat="server" ForeColor="Black">Codigo Anterior</asp:Label></TD>
				<TD>
					<asp:Label id="CodAnt" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label3" Font-Bold="True" runat="server" ForeColor="Black">Codigo Sustituido</asp:Label></TD>
				<TD>
					<asp:Label id="CodSus" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label8" Font-Bold="True" runat="server" ForeColor="Black">Numero de Sustituciones:</asp:Label></TD>
				<TD>
					<asp:Label id="NumSus" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label9" Font-Bold="True" runat="server" ForeColor="Black">Ultima Sustitucion:</asp:Label></TD>
				<TD>
					<asp:Label id="UltiSus" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label10" Font-Bold="True" runat="server" ForeColor="Black">Fecha Ultima Sustitucion:</asp:Label></TD>
				<TD>
					<asp:Label id="FeUlSus" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
				<TD></TD>
			</TR>
		</TABLE>
	</FIELDSET>
</asp:Panel>
