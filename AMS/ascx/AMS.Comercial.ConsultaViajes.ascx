<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultaViajes.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultaViajes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Informaci�n de los viajes:</b></td>
		</tr>
		<TR>
			<td style="WIDTH: 123px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 386px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
		</TR>
		<asp:panel id="pnlRuta" Runat="server" Visible="False">
			<TR>
				<TD style="WIDTH: 123px; HEIGHT: 15px">
					<asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Ruta Principal :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 15px">
					<asp:dropdownlist id="ddlRuta" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
				</TD></TR>
			<TR>
				<TD style="WIDTH: 123px">
					<asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia Salida :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlAgenciaS" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 123px; HEIGHT: 18px">
					<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:TextBox id="txtFecha" onkeyup="DateMask(this)" runat="server" Width="92px"></asp:TextBox></TD>
			</TR>
		</asp:panel>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
		<TR>
			<TD style="WIDTH: 123px; HEIGHT: 18px">
			<TD align="left" colSpan="1"><asp:button id="btnConsultar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Consultar"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlPapeleria" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 183px; HEIGHT: 11px" colSpan="3"><B>Viajes:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrPapeleria" runat="server" Width="500px" PageSize="30" AllowPaging="True"
						AutoGenerateColumns="False">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="CODIGO" HeaderText="Cdgo"></asp:BoundColumn>
							<asp:BoundColumn DataField="AGENCIA" HeaderText="Agencia"></asp:BoundColumn>
							<asp:BoundColumn DataField="PLANILLA" HeaderText="Planilla." DataFormatString="{0:000000}" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
							<asp:BoundColumn DataField="VIAJE" HeaderText="NoViaje" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
							<asp:BoundColumn DataField="BUS" HeaderText="NoBus" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
							<asp:BoundColumn DataField="PLACA" HeaderText="Placa"></asp:BoundColumn>
							<asp:BoundColumn DataField="RUTA_PRINCIPAL" HeaderText="Ruta Viaje"></asp:BoundColumn>
							<asp:BoundColumn DataField="DESPACHADO" HeaderText="Dspcho"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_SALIDA" DataFormatString="{0:yyyy-MM-dd}"
								HeaderText="--FechaSalida--"></asp:BoundColumn>
							<asp:BoundColumn DataField="HORA_PROGRAMADA" HeaderText="Hora Prog."></asp:BoundColumn>
							<asp:BoundColumn DataField="HORA_DESPACHO" HeaderText="Hora Dspcho."></asp:BoundColumn>
						</Columns>
						<PagerStyle Mode="NumericPages"></PagerStyle>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
	</TABLE>
</DIV>
