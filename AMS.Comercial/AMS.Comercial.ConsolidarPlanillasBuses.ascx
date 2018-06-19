<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsolidarPlanillasBuses.ascx.cs" Inherits="AMS.Comercial.AMS_ComercialConsolidarPlanillasBuses" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P><asp:label id="Label1" runat="server" Font-Bold="True">Consolidar Planillas</asp:label>
	<HR width="100%" SIZE="2">
	<FIELDSET style="WIDTH: 494px; HEIGHT: 220px"><LEGEND>Filtros</LEGEND>
		<TABLE id="Table1" style="WIDTH: 494px; HEIGHT: 175px" cellSpacing="0" cellPadding="0"
			width="494" border="0">
			<TR>
				<TD style="WIDTH: 241px">
					<TABLE id="Table2" style="WIDTH: 240px; HEIGHT: 156px" cellSpacing="0" cellPadding="0"
						width="240" align="left" border="0">
						<TR>
							<TD style="WIDTH: 123px"><asp:label id="Label2" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Blue">Fecha:</asp:label>
								<asp:Label id="FechaLabel" Font-Bold="True" runat="server" Font-Size="XX-Small">Label</asp:Label></TD>
						</TR>
						<tr>
							<td style="WIDTH: 123px"><asp:label id="label" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Blue">Fecha Inicial:</asp:label></td>
							<td><asp:label id="Label6" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Blue">Fecha Final:</asp:label></td>
						</tr>
						<TR>
							<TD style="WIDTH: 123px"><asp:label id="Label3" runat="server" Font-Bold="True" Font-Size="XX-Small">AÑO</asp:label><asp:dropdownlist id="añoi" runat="server" Font-Bold="True" Font-Size="XX-Small" Width="90px"></asp:dropdownlist></TD>
							<td><asp:label id="label20" runat="server" Font-Bold="True" Font-Size="XX-Small">AÑO</asp:label><asp:dropdownlist id="añof" runat="server" Font-Bold="True" Font-Size="XX-Small" Width="90px"></asp:dropdownlist></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 123px; HEIGHT: 17px"><asp:label id="Label4" runat="server" Font-Bold="True" Font-Size="XX-Small">MES</asp:label><asp:dropdownlist id="mesi" runat="server" Font-Bold="True" Font-Size="XX-Small" Width="90px"></asp:dropdownlist></TD>
							<TD style="HEIGHT: 17px"><asp:label id="Label7" runat="server" Font-Bold="True" Font-Size="XX-Small">MES</asp:label><asp:dropdownlist id="mesf" runat="server" Font-Bold="True" Font-Size="XX-Small" Width="90px"></asp:dropdownlist></TD>
						</TR>
						<tr>
							<td style="WIDTH: 123px"><asp:label id="Label5" runat="server" Font-Bold="True" Font-Size="XX-Small">DIA</asp:label><asp:textbox id="diai" runat="server" Font-Bold="True" Font-Size="XX-Small" Width="40px"></asp:textbox></td>
							<td><asp:label id="Label9" runat="server" Font-Bold="True" Font-Size="XX-Small">DIA</asp:label><asp:textbox id="diaf" runat="server" Font-Bold="True" Font-Size="XX-Small" Width="40px"></asp:textbox></td>
						</tr>
					</TABLE>
				<TD style="WIDTH: 413px">
					<TABLE id="Table3" style="WIDTH: 238px; HEIGHT: 128px" cellSpacing="0" cellPadding="0"
						width="238" align="left" border="0">
						<TR>
							<TD style="WIDTH: 147px"><asp:label id="Label8" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Blue">Vehiculo:</asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 147px; HEIGHT: 3px"><asp:label id="Label10" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Blue">Bus:</asp:label><asp:dropdownlist id="bus" runat="server" Font-Bold="True" Font-Size="XX-Small" AutoPostBack="True"></asp:dropdownlist></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 147px">
								<P><asp:label id="Label12" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Blue">Conductor:</asp:label></P>
								<P><asp:label id="conductor" runat="server" Font-Bold="True" Font-Size="XX-Small">Nombre</asp:label></P>
								<P><asp:label id="Label13" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Blue">Bus #</asp:label><asp:label id="numbus" runat="server" Font-Bold="True" Font-Size="XX-Small">Numero</asp:label></P>
							</TD>
						</TR>
					</TABLE>
				</TD>
			</TR>
		</TABLE>
		<asp:button id="generar" onclick="generar_OnClick" runat="server" Text="Generar"></asp:button></FIELDSET>
<P></P>
<P>
	<HR width="100%" SIZE="2">
<P></P>
<P><asp:panel id="Panel1" runat="server" Height="282px" Visible="False">
		<TABLE id="Table4" style="WIDTH: 840px; HEIGHT: 344px" cellSpacing="0" cellPadding="0"
			width="840" border="0">
			<TR>
				<TD style="WIDTH: 518px">
					<FIELDSET style="WIDTH: 549px; HEIGHT: 316px"><LEGEND>Ventas</LEGEND>
						<TABLE id="Table5" style="WIDTH: 544px; HEIGHT: 288px" cellSpacing="0" cellPadding="0"
							width="544" border="0">
							<TR>
								<TD style="WIDTH: 245px">
									<asp:Label id="Label14" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small">Tiquetes automaticos</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								</TD>
								<td>
									<asp:Label id="Label16" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small">Remesas Automaticas:</asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px; HEIGHT: 1px">
									<asp:Label id="Label18" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"># Tiquetes:</asp:Label>
									<asp:Label id="numta" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								</TD>
								<TD style="HEIGHT: 1px">
									<asp:Label id="Label23" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"># Remesas:</asp:Label>
									<asp:Label id="numrema" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px">
									<asp:Label id="Label19" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Total Tiquetes:</asp:Label>
									<asp:Label id="ttiqa" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								</TD>
								<td>
									<asp:Label id="Label24" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Total Remesas:</asp:Label>
									<asp:Label id="totrema" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px; HEIGHT: 9px">
									<asp:Label id="Label15" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small">Tiquetes Manuales:</asp:Label></TD>
								<TD style="HEIGHT: 9px">
									<asp:Label id="Label17" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small">Remesas Manuales:</asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px; HEIGHT: 7px">
									<asp:Label id="Label21" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"># Tiquetes:</asp:Label>
									<asp:Label id="numtiqm" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
								<TD style="HEIGHT: 7px">
									<asp:Label id="Label25" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"># Remesas:</asp:Label>
									<asp:Label id="numremm" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px">
									<asp:Label id="Label22" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Total Tiquetes:</asp:Label>
									<asp:Label id="ttiqm" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
								<td>
									<asp:Label id="Label26" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Total Remesas:</asp:Label>
									<asp:Label id="totremm" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px">
									<asp:Label id="Label39" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small">Tiquetes Prepago:</asp:Label></TD>
								<td>
									<asp:Label id="Label29" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">TOTAL REMESAS:</asp:Label>
									<asp:TextBox id="totalremesas" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"
										ReadOnly="True" BorderWidth="1px" BorderStyle="Dotted" BorderColor="Black" BackColor="Lime"></asp:TextBox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px">
									<asp:Label id="Label40" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small"># Tiquetes:</asp:Label>
									<asp:Label id="numtipre" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
								<td></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px">
									<asp:Label id="Label41" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small">Total Tiquetes:</asp:Label>
									<asp:Label id="tottiqpre" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px; HEIGHT: 10px">
									<asp:Label id="Label27" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">TOTAL TIQUETES:</asp:Label>
									<asp:TextBox id="totaltiquetes" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"
										ReadOnly="True" BorderWidth="1px" BorderStyle="Dotted" BorderColor="Black" BackColor="Lime"></asp:TextBox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px">
									<asp:Label id="Label31" Font-Bold="True" runat="server" Font-Size="XX-Small">TOTAL VENTAS:</asp:Label>&nbsp;&nbsp;&nbsp;
									<asp:TextBox id="totalventas" Font-Bold="True" runat="server" Font-Size="XX-Small" ReadOnly="True"
										BorderStyle="Outset" BorderColor="Black" BackColor="Lime"></asp:TextBox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px; HEIGHT: 1px"></TD>
								<TD style="HEIGHT: 1px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px; HEIGHT: 2px">
									<asp:Label id="Label33" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">TOTAL NETO</asp:Label>
									<asp:TextBox id="TOTALNETO" Font-Bold="True" runat="server" Font-Size="XX-Small" ReadOnly="True"
										BorderStyle="Dotted" BorderColor="Black" BackColor="Yellow"></asp:TextBox></TD>
								<TD style="HEIGHT: 2px">&nbsp;
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px; HEIGHT: 8px">&nbsp;&nbsp;
									<P>
										<asp:Label id="Label34" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">(Ventas - Gastos)</asp:Label></P>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 245px">
									<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									</P>
									<P>&nbsp;</P>
								</TD>
								<td></TD>
							</TR>
						</TABLE>
					</FIELDSET>
				</TD>
				<TD style="WIDTH: 381px">
					<FIELDSET style="WIDTH: 296px; HEIGHT: 356px"><LEGEND>Gastos</LEGEND>
						<TABLE id="Table6" style="WIDTH: 288px; HEIGHT: 126px" cellSpacing="0" cellPadding="0"
							width="288" border="0">
							<TR>
								<TD style="HEIGHT: 18px">
									<asp:Label id="Label11" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small">Anticipos:</asp:Label></TD>
							</TR>
							<TR>
								<td>
									<asp:Label id="Label30" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"># de Anticipos:</asp:Label>
									<asp:Label id="numanti" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
								<td></TD>
							</TR>
							<TR>
								<td>
									<asp:Label id="Label32" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Total Anticipos:</asp:Label>
									<asp:Label id="totanti" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
							</TR>
							<TR>
								<td>
									<asp:Label id="Label35" Font-Bold="True" runat="server" ForeColor="Blue" Font-Size="XX-Small">Anticipos Extras</asp:Label></TD>
								<td></TD>
							</TR>
							<TR>
								<td>
									<asp:Label id="Label36" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"># de Anticipos</asp:Label>
									<asp:Label id="numae" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
								<td></TD>
							</TR>
							<TR>
								<td>
									<asp:Label id="Label37" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Total Anticipos</asp:Label>
									<asp:Label id="totane" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">--</asp:Label></TD>
								<td></TD>
							</TR>
							<TR>
								<td>
									<asp:Label id="Label28" Font-Bold="True" runat="server" Font-Size="XX-Small">TOTAL ANTICIPOS</asp:Label>
									<asp:TextBox id="totalanticipos" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small"
										ReadOnly="True" BorderStyle="Outset" BorderColor="Black" BackColor="Red"></asp:TextBox></TD>
								<td></TD>
							</TR>
						</TABLE>
					</FIELDSET>
				</TD>
			</TR>
		</TABLE>
		<asp:Button id="guardar" onclick="guardar_OnClick" runat="server" Text="Guardar"></asp:Button>
		<asp:Label id="Label38" runat="server">Label</asp:Label>
	</asp:panel></P>
