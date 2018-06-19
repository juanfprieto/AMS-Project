<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ReporteAgencia.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ReporteAgencia" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD style="WIDTH: 154px">
				<asp:Label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Planillas :</asp:Label></TD>
			<TD style="WIDTH: 386px">
				<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="62px" Runat="server"
					MaxLength="10"></asp:textbox></TD>
		</TR>
		<TR>
			<td></td>
			<TD style="WIDTH: 154px"><asp:button id="btnConsultar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Consultar"></asp:button></TD>
			<TD style="WIDTH: 400px"><asp:button id="btnExportarExcel" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Exportar Excel"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
		</TR>
	</table>
	<table style="WIDTH: 773px" align="center">
		<TR>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center" style="HEIGHT: 17px"><asp:button id="btnGenerar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Generar Reporte"
					Width="102px"></asp:button>&nbsp;&nbsp;
				<asp:hyperlink id="Ver" runat="server" Visible="False" Target="_blank">De Click Aqui para ver el Reporte</asp:hyperlink></TD>
			<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
		</TR>
		<TR>
			<TD>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlConsulta" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD align="center">
					<asp:Datagrid id="dgrVentas" runat="server" Width="500" ShowFooter="False" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_AGENCIA" HeaderText="Cdgo"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_AGENCIA" HeaderText="Agencia"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="PLANILLA" HeaderText="Planilla"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="RUTA" HeaderText="Ruta"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="VIAJE" HeaderText="Viaje"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUMERO_BUS" HeaderText="Bus"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="PLACA" HeaderText="Placa"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DESPACHADOR" HeaderText="NitDespchdor"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DOCUMENTO" HeaderText="Dcmnto"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_LIQUIDACION" HeaderText="FechaDespacho"
								DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="HORA_DESPACHO" HeaderText="HraDesp"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_CONCEPTO" HeaderText="Concepto"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="TIQUETES" HeaderText="Tiqs"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_TIQUETES"
								HeaderText="Tiquetes"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="DESCUENTOS"
								HeaderText="Descuentos"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="ENCOMIENDAS"
								HeaderText="Encomiendas"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="COSTO_GIROS"
								HeaderText="Giros"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_GIROS"
								HeaderText="Valor Giros"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_INGRESOS"
								HeaderText="INGRESOS"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_EGRESOS"
								HeaderText="EGRESOS"></asp:BoundColumn>
						</Columns>
					</asp:Datagrid></TD>
			</TR>
		</TABLE>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Tiquetes :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalTiquetes" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Descuentos :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalDescuentos" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Encomiendas :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalRemesas" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Ing. Giros :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalGiros" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Otros Ingresos :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalIngresos" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Egresos :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalEgresos" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Total a consignar :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalConsignar" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Valor Giros :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalValorGiros" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></TD>
			</TR>
		</TABLE>
	</asp:panel>
</DIV>
</TR></TBODY></TABLE>
<DIV></DIV>
