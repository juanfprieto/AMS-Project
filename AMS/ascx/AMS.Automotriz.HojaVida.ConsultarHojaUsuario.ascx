<%@ Control Language="c#" codebehind="AMS.Automotriz.HojaVida.ConsultarHojaUsuario.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.ConsultarHojaUsuario" %>
<LINK href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<LINK href="../style/AMS.css" type="text/css" rel="stylesheet">
		<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
		</script>
		<p><asp:placeholder id="toolsHolder" runat="server">
				<TABLE class="tools">
					<TR>
						<th class="filterHead">
				<img height="30" src="../img/AMS.Flyers.Tools.png" border="0">
			</tH>
						<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</TD>
						<TD>&nbsp; &nbsp;Enviar por correo
							<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
						<TD>
							<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
								ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
							<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"
								BorderWidth="0px"></asp:ImageButton></TD>
						<td></td>
					</TR>
				</TABLE>
			</asp:placeholder></p>
		<asp:placeholder id="controlesConsulta" runat="server">

				<CENTER>Hoja de Vida de Automovil
				</CENTER>
				<CENTER>
					<FIELDSET>
						<LEGEND>Datos del 
Propietario</LEGEND>
						<P align="left">Nombres :&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Label id="nombres" runat="server" text="nombres"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;Apellidos 
							:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Label id="apellidos" runat="server" text="apellidos"></asp:Label></P>
						<P align="left">Documento de Identidad : &nbsp;&nbsp;
							<asp:Label id="nit" runat="server" text="documento"></asp:Label>&nbsp;&nbsp; 
							Expedido En : &nbsp;&nbsp;
							<asp:Label id="ciudadExpedicion" runat="server" text="ciudad"></asp:Label>&nbsp;&nbsp; 
							Tipo de Nacionalidad : &nbsp;&nbsp;
							<asp:Label id="tipoNacionalidad" runat="server" text="nacionalidad"></asp:Label></P>
						<P align="left">Dirección : &nbsp;&nbsp;
							<asp:Label id="direccion" runat="server" text="direccion"></asp:Label>&nbsp;&nbsp; 
							Ciudad : &nbsp;&nbsp;
							<asp:Label id="ciudad" runat="server" text="ciudad"></asp:Label>&nbsp;&nbsp; 
							Telefono : &nbsp;&nbsp;
							<asp:Label id="telefono" runat="server" text="telefono"></asp:Label></P>
						<P align="left">Movil : &nbsp;&nbsp;
							<asp:Label id="celular" runat="server" text="celular"></asp:Label>&nbsp;&nbsp; 
							E-Mail : &nbsp;&nbsp;
							<asp:Label id="email" runat="server" text="email"></asp:Label>&nbsp;&nbsp; 
							WebSite : &nbsp;&nbsp;
							<asp:Label id="website" runat="server" text="website"></asp:Label></P>
					</FIELDSET>
					<FIELDSET >
						<LEGEND>Datos del 
Vehiculo</LEGEND>
						<P align="left">Catalogo : &nbsp;&nbsp;
							<asp:Label id="catalogo" runat="server" text="catalogo"></asp:Label>&nbsp;&nbsp; 
							VIN : &nbsp;&nbsp;
							<asp:Label id="vin" runat="server" text="vin"></asp:Label>&nbsp;&nbsp; Placa : 
							&nbsp;&nbsp;
							<asp:Label id="placa" runat="server" text="placa"></asp:Label></P>
						<P align="left">Marca : &nbsp;&nbsp;
							<asp:Label id="marca" runat="server" text="marca"></asp:Label>&nbsp;&nbsp; 
							Detalle : &nbsp;&nbsp;
							<asp:Label id="detalle" runat="server" text="detalle"></asp:Label>
							</P>
						<P align="left">Número Motor : &nbsp;&nbsp;
							<asp:Label id="numeroMotor" runat="server" text="motor"></asp:Label>&nbsp;&nbsp; 
							Serie : &nbsp;&nbsp;
							<asp:Label id="serie" runat="server" text="serie"></asp:Label>&nbsp;&nbsp; 
							Chasis : &nbsp;&nbsp;
							<asp:Label id="chasis" runat="server" text="chasis"></asp:Label></P>
						<P align="left">Color : &nbsp;&nbsp;
							<asp:Label id="color" runat="server" text="color"></asp:Label>&nbsp;&nbsp; Año 
							de Modelo : &nbsp;&nbsp;
							<asp:Label id="anoModelo" runat="server" text="Año Modelo"></asp:Label>&nbsp;&nbsp; 
							Tipo de Servicio : &nbsp;&nbsp;
							<asp:Label id="tipoServicio" runat="server" text="tipo servicio"></asp:Label></P>
						<P align="left">Concesionario Vendedor : &nbsp;&nbsp;
							<asp:Label id="conscVendedor" runat="server" text="Concesionario Vendedor"></asp:Label>&nbsp;&nbsp; 
							Fecha de Venta : &nbsp;&nbsp;
							<asp:Label id="fechaVenta" runat="server" text="Fecha de Venta"></asp:Label></P>
						<P align="left">Ultimo Kilometraje : &nbsp;&nbsp;
							<asp:Label id="ultimoKilometraje" runat="server" text="Ultimo Kilometraje"></asp:Label></P>
						<P align="left">Kilometraje Promedio Mes: &nbsp;&nbsp;
							<asp:Label id="kilometrajePromedio" runat="server" text="Kilometraje Promedio Mes"></asp:Label></P>
					</FIELDSET>
					
						<LEGEND>Operaciones</LEGEND>
						<asp:DataGrid id="operacionesRealizadasGrilla" runat="server" cssclass="datagrid"
							AutoGenerateColumns="false" GridLines="Vertical">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="PREFIJO-NUMERO ORDEN">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "PREFNUMORD") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="OPERACION">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "OPERACION") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="KILOMETRAJE">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "KILOMETRAJE") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="FECHA">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="FECHA DE TERMINACION">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "FECHATERM") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="RECEPCIONISTA">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "MECANICO") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="ESTADO">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="OBS RECEPCION">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "OBSERECE")%>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="OBS CLIENTE">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "OBSECLIE")%>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
						<P></P>
					
						<LEGEND>Repuestos</LEGEND>
						<asp:datagrid id="Grid" runat="server" AutoGenerateColumns="False" HorizontalAlign="center">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="PREFIJO-NUMERO ORDEN">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "OTRELAC") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="DOCUMENTO DE TRASNFERENCIA">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "ORDENOP") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="REPUESTO">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "REPUESTO") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="CODIGO REPUESTO">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "CODREPUESTO") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="CANTIDAD">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
						<P></P></CENTER>
			 </asp:placeholder>
		<p><asp:button id="volver" onclick="Volver" runat="server" Text="Volver"></asp:button></p>
		<P><asp:label id="lb" runat="server"></asp:label></P>
