<%@ Control Language="c#" codebehind="AMS.Vehiculos.EntregaVehiculosFormulario.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.EntregaVehiculosFormulario" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<link href="../style/AMS.css" type="text/css" rel="stylesheet">
		<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
		</script>
		<p>
			<asp:PlaceHolder id="toolsHolder" runat="server">
				<TABLE class="tools">
					<TR>
						<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
						<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</TD>
						<TD>&nbsp; &nbsp;Enviar por correo
							<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
						<TD>
							<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
								ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
							<asp:ImageButton id="ibMail" runat="server" BorderWidth="0px" ImageUrl="../img/AMS.Icon.Mail.jpg"
								alt="Enviar por email"></asp:ImageButton></TD>
						<TD width="380"></TD>
					</TR>
				</TABLE>
			</asp:PlaceHolder>
		</p>
		<asp:PlaceHolder id="controlsFormulario" runat="server">
			<TABLE class="filters" borderColor="#cdcdcd" width="608">
				<TR>
					<TD bgColor="#f2f2f2">
						<P align="center">Entrega Nº&nbsp;
							<asp:Label id="numEntrega" runat="server"></asp:Label>
						</P>
						<P>Responsable de Entrega :
							<asp:Label id="responsable" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							Fecha y Hora de&nbsp;Creación :
							<asp:Label id="fechaHoraentrega" runat="server"></asp:Label></P>
						<P>Observaciones :
							<asp:Label id="observaciones" runat="server"></asp:Label></P>
						<P>Fecha y Hora de Entrega :
						</P>
					</TD>
				</TR>
				<P></P>
				<TR>
					<TD bgColor="#f2f2f2">
						<FIELDSET  align="center">
							<LEGEND>Datos 
      Sobre el Cliente</LEGEND>
							<TABLE>
								<TR>
									<TD>Nombre&nbsp;:
										<asp:Label id="nombreCliente" runat="server"></asp:Label></TD>
									<TD>Identificación :
										<asp:Label id="idCliente" runat="server"></asp:Label></TD>
								</TR>
								<TR>
									<TD>Dirección :
										<asp:Label id="direccionCliente" runat="server"></asp:Label></TD>
									<TD>Ciudad :
										<asp:Label id="ciudadCliente" runat="server"></asp:Label></TD>
								</TR>
								<TR>
									<TD>Teléfono&nbsp;:
										<asp:Label id="telefonoCliente" runat="server"></asp:Label></TD>
									<TD>Movil :
										<asp:Label id="movilCliente" runat="server"></asp:Label></TD>
								</TR>
								<TR>
									<TD>E-Mail :
										<asp:Label id="emailCliente" runat="server"></asp:Label></TD>
									<TD>WebSite :
										<asp:Label id="websiteCliente" runat="server"></asp:Label></TD>
								</TR>
							</TABLE>
						</FIELDSET>
					</TD>
				</TR>
				<TR>
					<TD bgColor="#f2f2f2">
						<FIELDSET align="center">
							<LEGEND>Datos 
      Sobre el Vehículo</LEGEND>
							<TABLE>
								<TR>
									<TD>Número de Inventario :
										<asp:Label id="numInvent" runat="server"></asp:Label>&nbsp;</TD>
									<TD>Tipo de Vehículo :&nbsp;&nbsp;
										<asp:Label id="catalogoVeh" runat="server"></asp:Label></TD>
									<TD>VIN:&nbsp;
										<asp:Label id="vinVeh" runat="server"></asp:Label></TD>
								</TR>
								<TR>
									<TD>Número Motor :
										<asp:Label id="numMotor" runat="server"></asp:Label></TD>
									<TD>Número Serie :
										<asp:Label id="numSerie" runat="server"></asp:Label></TD>
									<TD>Año de Modelo :&nbsp;&nbsp;
										<asp:Label id="anoModelo" runat="server"></asp:Label></TD>
								</TR>
								<TR>
									<TD>Tipo Servicio :
										<asp:Label id="tipoServicio" runat="server"></asp:Label></TD>
									<TD>Color :
										<asp:Label id="colorVeh" runat="server"></asp:Label></TD>
									<TD>Kilometraje :
										<asp:Label id="kilomVeh" runat="server"></asp:Label></TD>
								</TR>
							</TABLE>
						</FIELDSET>
					</TD>
				</TR>
				<TR>
				</TR>
				<TR>
					<TD bgColor="#f2f2f2">
						<FIELDSET align="center">
							<LEGEND>Accesorios</LEGEND>
							<asp:DataGrid id="grillaAccesorios" runat="server" cssclass="datagrid" GridLines="Vertical" AutoGenerateColumns="true">
								<HeaderStyle CssClass="header"></HeaderStyle>
						        <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						        <ItemStyle CssClass="item"></ItemStyle>
							</asp:DataGrid>
						</FIELDSET>
					</TD>
					<TD></TD>
				</TR>
				<TR>
					<TD bgColor="#f2f2f2">
						<FIELDSET  align="center"><LEGEND>Abonos 
      Realizados al Pedido</LEGEND>Facturas Relacionadas&nbsp;Cliente: &nbsp; 
<asp:DataGrid id="grillaFacturas" runat="server" cssclass="datagrid" GridLines="Vertical" AutoGenerateColumns="false">
	<HeaderStyle CssClass="header"></HeaderStyle>
	<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
	<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
	<ItemStyle CssClass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="PREFIJO FACTURA">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "PREFIJOFACTURA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="NÚMERO FACTURA">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMEROFACTURA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="VALOR FACTURA">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALORTOTAL","{0:C}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="VALOR ABONADO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALORABONADO","{0:C}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="VALOR SALDO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALORSALDO","{0:C}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="NIT CLIENTE">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NITCLIENTE") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="NOMBRE CLIENTE">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NOMBRECLIENTE") %>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
      <P align="right">&nbsp;&nbsp;&nbsp; Total Saldos :
								<asp:Label id="saldos" runat="server"></asp:Label></P>
      <P>Vehículos Pedientes de Retoma :
		<asp:DataGrid id="grillaRetoma" runat="server" cssclass="datagrid" GridLines="Vertical" AutoGenerateColumns="false">
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="N&#218;MERO CONTRATO RETOMA">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "NUMEROCONTRATORETOMA") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="TIPO VEHICULO">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "TIPOVEHICULO") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="PLACA VEHICULO">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "PLACAVEHICULO") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="A&#209;O MODELO">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "ANOMODELO") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="CUENTA DE IMPUESTOS">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CUENTAIMPUESTOS") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="VALOR RETOMA">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "VALORRETOMA","{0:C}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="ESTADO RETOMA">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "ESTADORETOMA") %>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid></P>
      <P align="right">Total Retomas :
								<asp:Label id="retomas" runat="server"></asp:Label></P>
      <P align="right">Saldo Restante :
								<asp:Label id="saldoFinal" runat="server"></asp:Label></P></FIELDSET>
					</TD>
				</TR>
			</TABLE>
		</asp:PlaceHolder>
		<p>
			<asp:Button id="btnGuardar" runat="server" Text="Guardar"></asp:Button>
		</p>
		<p>
			<asp:Label id="lb" runat="server"></asp:Label>
		</p>
