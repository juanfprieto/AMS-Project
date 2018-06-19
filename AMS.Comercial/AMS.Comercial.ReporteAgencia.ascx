<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ReporteAgencia.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ReporteAgencia" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td><asp:label id="Label1" Font-Bold="True" runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" runat="server"></asp:dropdownlist></td>
		</tr>
		<TR>
			<TD>
				<asp:Label id="Label18" runat="server" Font-Bold="True">Fecha Planillas :</asp:Label>
            </TD>
			<TD>
				<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Width="70px" Runat="server" MaxLength="10"></asp:textbox>           
                <asp:button id="btnConsultar" Font-Bold="True" Runat="server" Text="Consultar"></asp:button>
            </TD>
			<TD>
                <asp:button id="btnExportarExcel" Font-Bold="True" Runat="server" Text="Exportar Excel"></asp:button>
            </TD>
		</TR>
	</table>
	<table class="filtersIn">
		<TR>
			<TD align="center" style="HEIGHT: 17px"><asp:button id="btnGenerar" Font-Bold="True" Runat="server" Text="Generar Reporte"></asp:button>
				<asp:hyperlink id="Ver" runat="server" Visible="False" Target="_blank">De Click Aqui para ver el Reporte</asp:hyperlink></TD>			
		</TR>
		<TR>
			<TD>
				<asp:label id="lblError" Font-Bold="True" runat="server"></asp:label>
            </TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlConsulta" Runat="server" Visible="False">
		<TABLE class="filtersIn">
			<TR>
				<TD align="center">
					<asp:Datagrid id="dgrVentas" runat="server" Width="500" ShowFooter="False" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
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
		<TABLE class="filtersIn">
			<TR>
				<TD>
					<asp:label id="Label2" runat="server" Font-Bold="True">Total Tiquetes :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalTiquetes" runat="server" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label8" runat="server" Font-Bold="True">Total Descuentos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalDescuentos" runat="server" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label3" runat="server" Font-Bold="True">Total Encomiendas :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalRemesas" runat="server" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label4" runat="server" Font-Bold="True">Total Ing. Giros :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalGiros" runat="server" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label5" runat="server" Font-Bold="True">Total Otros Ingresos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalIngresos" runat="server" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label10" runat="server" Font-Bold="True">Total Egresos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalEgresos" runat="server" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label7" runat="server" Font-Bold="True">Total a consignar :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalConsignar" runat="server"  Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label6" runat="server" Font-Bold="True">Total Valor Giros :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalValorGiros" runat="server"  Font-Bold="True"></asp:label></TD>
			</TR>
		</TABLE>
	</asp:panel>
</DIV>
</TR></TBODY></TABLE>
<DIV></fieldset>
