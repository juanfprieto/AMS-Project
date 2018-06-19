<%@ Control Language="c#" codebehind="AMS.Automotriz.HojaVida.ConsultarHoja.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.ConsultarHoja" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<link href="../style/AMS.css" type="text/css" rel="stylesheet">
		<script type ="text/javascript">
        function Lista() {
            w=window.open('AMS.DBManager.Reporte.aspx');
        }
		</script>
		<asp:placeholder id="toolsHolder" runat="server">
			<table class="filterHead" >
				<tr>
					<th class="filterHead">
				        <img height="30" src="../img/AMS.Flyers.Tools.png" border="0" />
			        </th>
					<td>Imprimir <a href="javascript: Lista()"><img height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0" />
						</a>
					</td>
					<td>&nbsp; &nbsp;Enviar por correo
						<asp:TextBox id="tbEmail" runat="server"></asp:TextBox>

					</td>
					<td>
                            <%--<asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel" OnClick="ImprimirExcelGrid" runat="server"
                                alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" cssClass="Excelimg" style="width: 50%; margin-bottom: -6px;">
                            </asp:ImageButton>--%>
						<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
							ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
						<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"
							BorderWidth="0px"></asp:ImageButton>

					</td>
				</tr>
			</table>
		</asp:placeholder>
		<asp:placeholder id="controlesConsulta" runat="server">
            <fieldset>
				<CENTER>Hoja de Vida de Automovil
				</CENTER>
				<CENTER>
					<fieldset>
						<LEGEND>Datos del Propietario</LEGEND>
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
					</fieldset>
					<fieldset>
						<LEGEND>Datos del Vehículo</LEGEND>
						<P align="left">Catálogo : &nbsp;&nbsp;
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
					</fieldset>
                    </fieldset>
					
                     <!--
					<fieldset style="WIDTH: 640px">
						<LEGEND>Repuestos</LEGEND>
						<asp:datagrid id="Grid" runat="server" Width="640px" AutoGenerateColumns="False" HorizontalAlign="center">
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
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
								<asp:TemplateColumn HeaderText="VALOR UNITARIO">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "VALORU") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
						<P></P></CENTER>
			</fieldset>
            -->
            <fieldset>
            <center><b> Hoja de Vida</b></center>
             </fieldset>
						<asp:DataGrid id="operacionesRealizadasGrilla" runat="server" AutoGenerateColumns="false" GridLines="Vertical" cssclass="datagrid">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Orden de Trabajo">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "PREFNUMORD") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Descripción">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "OPERACION") %>
									</ItemTemplate>
								</asp:TemplateColumn>
                                					
                                <asp:TemplateColumn HeaderText="Cantidad">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
									</ItemTemplate>
								</asp:TemplateColumn>
                                			
                                <asp:TemplateColumn HeaderText="TIEMPO">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "TIEMPO") %>
									</ItemTemplate>
								</asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Precio">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "PRECIO") %>
									</ItemTemplate>
								</asp:TemplateColumn>

								<asp:TemplateColumn HeaderText="IVA">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "IVA") %>
									</ItemTemplate>
								</asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Kilometraje">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "KILOMETRAJE") %>
									</ItemTemplate>
								</asp:TemplateColumn>

								<asp:TemplateColumn HeaderText="Fecha">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Fecha Terminación">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "FECHATERM") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Recepcionista / Técnico">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "MECANICO") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Estado / Cargo">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Obser Recepción">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "OBSERECE")%>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Obser Cliente">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "OBSECLIE")%>
									</ItemTemplate>
									</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
            </asp:placeholder>
           
		<p><asp:button id="volver" onclick="Volver" runat="server" Text="Volver"></asp:button></p>
		<P><asp:label id="lb" runat="server"></asp:label></P>
        <asp:Label ID="lblResult" class="bg-success" runat="server"></asp:Label></p>