<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultaViajes.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultaViajes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td><h3>Información de los viajes:</h3></td>
		</tr>
		<TR>
			<td><asp:label id="Label4" Font-Bold="True"  runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia"  runat="server" AutoPostBack="True"></asp:dropdownlist></td>
		</TR>
		<asp:panel id="pnlRuta" Runat="server" Visible="False">
			<TR>
				<TD>
					<asp:label id="Label11" runat="server"  Font-Bold="True">Ruta Principal :</asp:label></TD>
				<TD>
					<asp:dropdownlist id="ddlRuta" runat="server" ></asp:dropdownlist></TD>
				</TD></TR>
			<TR>
				<TD>
					<asp:label id="Label1" runat="server"  Font-Bold="True">Agencia Salida :</asp:label></TD>
				<TD>
					<asp:dropdownlist id="ddlAgenciaS" runat="server" ></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label6" runat="server"  Font-Bold="True">Fecha :</asp:label></TD>
				<TD>
					<asp:TextBox id="txtFecha" onkeyup="DateMask(this)" runat="server" Width="92px"></asp:TextBox></TD>
			</TR>
		</asp:panel>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
		<TR>
			<TD>
			<TD align="left" colSpan="1"><asp:button id="btnConsultar" Font-Bold="True"  Runat="server" Text="Consultar"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlPapeleria" Runat="server" Visible="False">
		<TABLE class="filtersIn">
			<TR>
				<TD colSpan="3"><B>Viajes:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrPapeleria" runat="server" Width="500px" PageSize="30" AllowPaging="True"
						AutoGenerateColumns="False">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle  HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle  Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
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
	<TABLE class="filtersIn">
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True"  runat="server"></asp:label></TD>
		</TR>
	</TABLE>
</fieldset>
