<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.Repocision.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_Repocision" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta name="vs_showGrid" content="True">
<HR width="100%" color="#3300ff" SIZE="2">
<asp:label id="Label1" runat="server" Font-Bold="True">Fondo de Repocision Vehicular</asp:label>
<HR width="100%" color="#3300ff" SIZE="2">
<FIELDSET style="WIDTH: 576px; HEIGHT: 100%"><LEGEND>Reposicion Vehicular</LEGEND>
	<TABLE id="Table1" style="WIDTH: 552px; HEIGHT: 76px" cellSpacing="0" cellPadding="0" width="552"
		border="0">
		<TR>
			<TD style="WIDTH: 145px"><asp:label id="Label2" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small">Placa:</asp:label></TD>
			<TD><asp:dropdownlist id="placa" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			<TD></TD>
			<TD></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 145px"><asp:label id="Label3" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small">Fecha:</asp:label></TD>
			<TD><asp:label id="fecha" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small">AAAA-MM-DD</asp:label></TD>
			<td></td>
			<td></td>
		</TR>
		<tr>
			<td style="WIDTH: 145px"><asp:label id="Label19" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small">Año Vigente:</asp:label><asp:label id="añov" runat="server" Font-Bold="True" ForeColor="Red" Font-Size="XX-Small">Año</asp:label></td>
			<td><asp:label id="Label20" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small">Mes Vigente:</asp:label><asp:label id="mesv" runat="server" Font-Bold="True" ForeColor="Red" Font-Size="XX-Small">Mes</asp:label></td>
			<td></td>
			<td></td>
		</tr>
		<TR>
			<TD style="WIDTH: 145px"><asp:button id="Generar" onclick="generar_OnClick" runat="server" Font-Bold="True" Font-Size="XX-Small"
					Text="Generar"></asp:button></TD>
			<TD><asp:button id="liquitotal" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small"
					Text="Liquidacion Total Parque Automotor" OnClick="LiquidarTotal_OnClick"></asp:button></TD>
			<TD></TD>
			<TD></TD>
		</TR>
	</TABLE>
</FIELDSET>
<HR width="100%" color="#3300ff" SIZE="2">
<p><asp:panel id="Panel3" runat="server" Visible="False">
		<TABLE id="Table4" style="WIDTH: 650px; HEIGHT: 57px" cellSpacing="0" cellPadding="0" width="650"
			border="0">
			<TR>
				<TD>
					<asp:label id="Label21" Font-Bold="True" runat="server" Font-Size="XX-Small">El proceso de Liquidacion Total Genera automaticamente el Valor de la Cuota de Reposicion de todos los Vehiculos.</asp:label></TD>
			</TR>
			<TR>
				<TD>
					
						<DIV style="OVERFLOW: auto; WIDTH: 851px; HEIGHT: 146px">
						<asp:datagrid id="Grid" runat="server" Width="780px" AutoGenerateColumns="False" HorizontalAlign="Center">
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<EditItemStyle Font-Size="XX-Small"></EditItemStyle>
							<AlternatingItemStyle Font-Size="XX-Small" BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle Font-Size="XX-Small" HorizontalAlign="left" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="PLACA">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="TOTAL VENTAS">
									<ItemStyle HorizontalAlign="left"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "TOTALVENTAS") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="VALOR CUOTA">
									<ItemStyle HorizontalAlign="left"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "VALORCUOTA") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="FECHA LIQUIDACION">
									<ItemStyle HorizontalAlign="left"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></div>
						<asp:Label id="Label22" Font-Bold="True" runat="server" Font-Size="XX-Small">Numero de Vehiculos Liquidados:</asp:Label>
				
						<asp:Label id="numero" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Red">#</asp:Label>
				</TD>
				
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label23" Font-Bold="True" runat="server" Font-Size="XX-Small">Total Recaudo:</asp:Label>
					<asp:Label id="recaudo" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Red">$</asp:Label></TD>
			</TR>
		</TABLE>
		<asp:Button id="Liquidar2" onclick="LiquidarFinal_OnClick" Font-Bold="True" runat="server" Font-Size="XX-Small"
			Text="Liquidar"></asp:Button>
	</asp:panel></p>
<asp:panel id="Panel1" runat="server" Visible="False" Height="280px">
	<FIELDSET style="WIDTH: 576px; HEIGHT: 264px"><LEGEND>Datos Base Liquidacion
		</LEGEND>
		<TABLE id="Table2" style="WIDTH: 568px; HEIGHT: 57px" cellSpacing="0" cellPadding="0" width="568"
			border="0">
			<TR>
				<TD style="WIDTH: 327px">
					<asp:Label id="Label4" Font-Bold="True" runat="server" Font-Size="XX-Small">Total Ventas Tiquetes</asp:Label></TD>
				<TD>
					<asp:Label id="Label5" Font-Bold="True" runat="server" Font-Size="XX-Small">Total Ventas Remesas</asp:Label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 327px">
					<TABLE style="WIDTH: 352px; HEIGHT: 19px" cellSpacing="0" cellPadding="0" border="0">
						<TR>
							<TD>
								<asp:Label id="Label7" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">Tiquetes Automaticos:</asp:Label></TD>
							<TD>
								<asp:TextBox id="tiquetesauto" runat="server" Font-Size="XX-Small" BorderColor="Black" BackColor="#FF8000"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label6" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">Tiquetes Manuales:</asp:Label></TD>
							<TD>
								<asp:TextBox id="tiquetesmanu" runat="server" Font-Size="XX-Small" BorderColor="Black" BackColor="#FF8000"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label8" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">Tiquetes Pre-Pago:</asp:Label></TD>
							<TD>
								<asp:TextBox id="tiquetespre" runat="server" Font-Size="XX-Small" BorderColor="Black" BackColor="#FF8000"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label9" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">TOTAL Ventas Tiquetes:</asp:Label></TD>
							<TD>
								<asp:TextBox id="totaltiquetes" runat="server" Font-Size="XX-Small" BorderColor="Black" BackColor="Lime"
									BorderWidth="2px" BorderStyle="Outset"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD></TD>
							<TD></TD>
						</TR>
					</TABLE>
					<asp:Label id="Label12" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">TOTAL Ventas:</asp:Label>
					<asp:TextBox id="totalventas" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black"
						Width="120px" BorderColor="Black" BackColor="Lime" BorderStyle="Ridge"></asp:TextBox></TD>
				<TD>
					<TABLE style="WIDTH: 352px; HEIGHT: 19px" cellSpacing="0" cellPadding="0" border="0">
						<TR>
							<TD>
								<asp:Label id="Label10" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">Remesas Automaticas:</asp:Label></TD>
							<TD>
								<asp:TextBox id="remesasauto" runat="server" Font-Size="XX-Small" BorderColor="Black" BackColor="#FF8000"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label11" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">Remesas Manuales:</asp:Label></TD>
							<TD>
								<asp:TextBox id="remesasmanu" runat="server" Font-Size="XX-Small" BorderColor="Black" BackColor="#FF8000"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD></TD>
							<TD></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="Label13" Font-Bold="True" runat="server" Font-Size="XX-Small" ForeColor="Black">TOTAL Ventas Remesas:</asp:Label></TD>
							<TD>
								<asp:TextBox id="totalremesas" runat="server" Font-Size="XX-Small" BorderColor="Black" BackColor="Lime"
									BorderWidth="2px" BorderStyle="Outset"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD></TD>
							<TD></TD>
						</TR>
					</TABLE>
					<asp:Label id="Label14" Font-Bold="True" runat="server" Font-Size="XX-Small">Valor Cuota Reposicion:</asp:Label>
					<asp:TextBox id="valorcuota" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="120px"
						BorderColor="Black" BackColor="Lime" BorderStyle="Outset"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD>
					<asp:Button id="detalles" onclick="Detalles_OnClick" Font-Bold="True" runat="server" Font-Size="XX-Small"
						ForeColor="Black" Text="Detalles"></asp:Button></TD>
				<TD></TD>
			</TR>
		</TABLE>
		<asp:Panel id="Panel2" runat="server" Width="708px" Height="82px">
			<FIELDSET style="WIDTH: 494px; HEIGHT: 154px"><LEGEND>Detalles</LEGEND>
				<TABLE id="Table3" style="WIDTH: 472px; HEIGHT: 57px" cellSpacing="0" cellPadding="0" width="472"
					border="0">
					<TR>
						<TD style="WIDTH: 139px">
							<asp:Label id="Label15" Font-Bold="True" runat="server" Font-Size="XX-Small">Ultima Liquidacion:</asp:Label></TD>
						<TD>
							<asp:Label id="fechaultim" Font-Bold="True" runat="server" Font-Size="XX-Small">AAAA-MM-DD</asp:Label></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 139px">
							<asp:Label id="Label16" Font-Bold="True" runat="server" Font-Size="XX-Small">Valor Ultima Liquidacion:</asp:Label></TD>
						<TD>
							<asp:TextBox id="ultimaliq" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="120px"
								BorderColor="Black" BackColor="Lime" BorderStyle="Outset" ReadOnly="True"></asp:TextBox></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 139px">
							<asp:Label id="Label18" Font-Bold="True" runat="server" Font-Size="XX-Small">Valor Acumulado A la Fecha:</asp:Label></TD>
						<TD>
							<asp:TextBox id="acumliq" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="120px"
								BorderColor="Black" BackColor="Lime" BorderStyle="Outset" ReadOnly="True"></asp:TextBox></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 139px"></TD>
						<TD></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 139px"></TD>
						<TD></TD>
						<TD></TD>
					</TR>
				</TABLE>
			</FIELDSET>
		</asp:Panel>
		<asp:Button id="LIQUIDAR" onclick="Liquidar_OnClick" Font-Bold="True" runat="server" Font-Size="XX-Small"
			ForeColor="Black" Text="LIQUIDAR"></asp:Button>
		<asp:Label id="Label17" runat="server">Label</asp:Label></FIELDSET>
</asp:panel>
