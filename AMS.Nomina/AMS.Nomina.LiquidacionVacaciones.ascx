<%@ Control Language="c#" codebehind="AMS.Nomina.LiquidacionVacaciones.cs" autoeventwireup="false" Inherits="AMS.Nomina.LiquidacionVacaciones" %>
<fieldset>

<table id="Table1" class="filtersIn">
<tr>
<td class="scrollable">

<p>HISTORIAL DE VACACIONES CAUSADAS
</p>
<p><asp:datagrid id="DATAGRIDVACACIONESCAUSADAS" runat="server" cssclass="datagrid" AutoGenerateColumns="False" OnItemCommand="Seleccion_Periodo">
		<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<Columns>
			<asp:BoundColumn DataField="CODIGO EMPLEADO" HeaderText="CODIGO EMPLEADO"></asp:BoundColumn>
			<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE"></asp:BoundColumn>
			<asp:BoundColumn DataField="PERIODO" HeaderText="PERI ODO"></asp:BoundColumn>
			<asp:BoundColumn DataField="FECHA INICIAL" HeaderText="FECHA INICIAL"></asp:BoundColumn>
			<asp:BoundColumn DataField="FECHA FINAL" HeaderText="FECHA FINAL"></asp:BoundColumn>
			<asp:BoundColumn DataField="CAUSADAS" HeaderText="CAUS ADAS"></asp:BoundColumn>
			<asp:BoundColumn DataField="DISFRUTADAS" HeaderText="DISFRU TADAS"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="ELEGIR">
				<ItemTemplate>
					<asp:Button id="Button1" runat="server" Text="PERIODO" enabled="false"></asp:Button>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></p>
<p>Periodo Pendiente Escogido
</p>
<P>
<asp:datagrid id="DATAGRIDPERESCOGIDO" runat="server" cssclass="datagrid" AutoGenerateColumns="False" OnItemCommand="Seleccion_Periodo"
		OnItemDataBound="cargar_ddl">
		<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<Columns>
			<asp:BoundColumn DataField="CODIGO EMPLEADO" HeaderText="CODIGO EMPLEADO"></asp:BoundColumn>
			<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE"></asp:BoundColumn>
			<asp:BoundColumn DataField="PERIODO" HeaderText="PERI ODO"></asp:BoundColumn>
			<asp:BoundColumn DataField="FECHA INICIAL" HeaderText="FECHA INICIAL"></asp:BoundColumn>
			<asp:BoundColumn DataField="FECHA FINAL" HeaderText="FECHA FINAL"></asp:BoundColumn>
			<asp:BoundColumn DataField="CAUSADAS" HeaderText="CAUS ADAS"></asp:BoundColumn>
			<asp:BoundColumn DataField="DISFRUTADAS" HeaderText="DISFRU TADAS"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="DESDE">
				<ItemTemplate>
		
						<TABLE>
							<TR>
								<TD>AÑO:</TD>
							</TR>
							<TR>
								<TD>
									<asp:dropdownlist id="DDLANOINI" class="dmediano" runat="server"></asp:dropdownlist>
                                </TD>
							</TR>
							<TR>
								<TD>MES:</TD>
							</TR>
							<TR>
								<TD>
									<asp:dropdownlist id="DDLMESINI" class="dmediano" runat="server"></asp:dropdownlist>
								</TD>
							</TR>
							<TR>
								<TD>DIA:</TD>
							</TR>
							<TR>
								<TD>
									<asp:dropdownlist id="DDLDIAINI" class="dmediano" runat="server"></asp:dropdownlist>
                                    </TD>
							</TR>
						</TABLE>

				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HASTA">
				<ItemTemplate>
					<TABLE>	
                        <TR>
							<TD>AÑO:</TD>
						</TR>					
						<TR>
							<TD>
								<asp:dropdownlist id="DDLANOFIN" class="dmediano" runat="server"></asp:dropdownlist></TD>
						</TR>
						<TR>
							<TD>MES:</TD>
						</TR>
						<TR>
							<TD>
								<asp:dropdownlist id="DDLMESFIN" class="dmediano" runat="server"></asp:dropdownlist></TD>
						</TR>
						<TR>
							<TD>DIA:</TD>
						</TR>
						<TR>
							<TD>
								<asp:dropdownlist id="DDLDIAFIN" class="dmediano" runat="server"></asp:dropdownlist></TD>
						</TR>
					</TABLE>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="FORMA">
				<ItemTemplate>
					<P>
						<asp:dropdownlist id="DDLFORMA" class="dmediano" runat="server">
							<asp:ListItem Value="EN TIEMPO">EN TIEMPO</asp:ListItem>
							<asp:ListItem Value="EN DINERO">EN DINERO</asp:ListItem>
						</asp:dropdownlist></P>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DIAS EFECTIVOS">
				<ItemTemplate>
					<P>
						<asp:dropdownlist id="DDLDIASEFECTIVOS" runat="server"></asp:dropdownlist></P>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DIAS DE AJUSTE">
				<ItemTemplate>
					<P>
						<asp:textbox id="DiasAjuste" runat="server">0</asp:textbox></P>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
		<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
	</asp:datagrid></P>
<p>&nbsp;<asp:button id="BTNINGRESAR" onclick="ingresar_vacaciones" runat="server" Visible="False" Text="Generar" ></asp:button>
</p>
<p><asp:panel id="Panel1" runat="server" Visible="False">
		<P>
        <br>
        <br>
			<TABLE>
				<TR>
					<TD>LIQ. VACACIONES
					</TD>
					<TD></TD>
				</TR>
				<TR>
					<TD>Código de Empleado:
					</TD>
					<TD>
						<asp:Label id="LBLCEDULA" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Nombre:
					</TD>
					<TD>
						<asp:Label id="LBLEMPLEADO" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Número de días:
					</TD>
					<TD>
						<asp:Label id="LBDTVACACIONES" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Vlr.&nbsp;Vacaciones a Pagar:</TD>
					<TD>
						<asp:Label id="LBVACAAPAGAR" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Fechas:</TD>
					<TD>
						<asp:Label id="LBPERIODO" runat="server" visible="False"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Dias Efectivos</TD>
					<TD>
						<asp:Label id="LBDIASEFECTIVOS" runat="server"></asp:Label></TD>
				</TR>
			</TABLE>
		</P>
		<P>
			<asp:DataGrid id="DataGrid2" AutoGenerateColumns="False" cssclass="datagrid" runat="server" Visible="False" AllowPaging="false">
				<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
				<Columns>
					<asp:BoundColumn DataField="CONCEPTO" HeaderText="CONCEPTO" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCION"></asp:BoundColumn>
					<asp:BoundColumn DataField="CANT EVENTOS" HeaderText="CANT EVENTOS"></asp:BoundColumn>
					<asp:BoundColumn DataField="VALOR EVENTO" HeaderText="VALOR EVENTO" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="A PAGAR" HeaderText="A PAGAR" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="A DESCONTAR" HeaderText="A DESCONTAR" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="TIPO EVENTO" HeaderText="TIPO EVENTO"></asp:BoundColumn>
					<asp:BoundColumn DataField="SALDO" HeaderText="SALDO" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="DOC REFERENCIA" HeaderText="DOC REFERENCIA"></asp:BoundColumn>
				</Columns>
			</asp:DataGrid></P>
		<P align="center">
			<asp:Button id="BTNLIQUIDAR"  onclick="realizar_liquidacion1" runat="server" Text="LIQUIDAR pago en Cheque" ></asp:Button>
            <asp:Button id="BTNLIQUIDAR2" onclick="realizar_liquidacion2" runat="server" Text="LIQUIDAR pago en Banco" ></asp:Button>
			<asp:Button id="Button1" onclick="imprimirGrilla" runat="server" Text="Imprimir"></asp:Button></P>
	</asp:panel>
    </p>

<p><asp:label id="Label1" runat="server" class="lpequeno"></asp:label><asp:label id="lb" runat="server" Visible="False">Label</asp:label><asp:label id="lbmas1" runat="server" Visible="False">Label</asp:label><asp:label id="lb2" runat="server" Visible="False">Label</asp:label></p>
</td></tr></table></fieldset>

