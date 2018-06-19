<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<%@ Control Language="c#" codebehind="AMS.Automotriz.LiquidaManoObra.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.LiquidaManoObra" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<link href="../style/AMS.css" type="text/css" rel="stylesheet">
		<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
		<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
		</script>
        <fieldset>
		<table id="Table" class="filtersIn">
			<tbody>
				<tr>
					<td>
						
							<legend>Tipo de Proceso</legend>
							<asp:RadioButtonList id="tipoProceso" BorderStyle="None" runat="server">
								<asp:ListItem Value="P" Selected="True">Pre-Liquidaci&#243;n</asp:ListItem>
								<asp:ListItem Value="L">Liquidaci&#243;n</asp:ListItem>
								<asp:ListItem Value="R">Re-Liquidaci&#243;n</asp:ListItem>
							</asp:RadioButtonList>
						
					</td>
					<td>
						
							<legend>Tipo de Operaciones a Liquidar</legend>
							<asp:RadioButtonList id="tipoOperacion" BorderStyle="None" runat="server" Width="199px">
								<asp:ListItem Value="B" Selected="True">Operaciones Por Bono</asp:ListItem>
								<asp:ListItem Value="T">Operaciones por Tiempo</asp:ListItem>
                                <asp:ListItem Value="F">Operaciones por Valor Fijo</asp:ListItem>
								<asp:ListItem Value="BT">Todas las Operaciones</asp:ListItem>
							</asp:RadioButtonList>
						
					</td>
				</tr>
			</tbody>
		</table>
		<table id="Table" class="filtersIn">
			<tbody>
				<tr>
					<td>
						
                    <p>
							</p>
                    <legend>Talleres</legend>Seleccione si va liquidar TODOS los talleres o UNO específico&nbsp;: 
                    <p>
								<asp:RadioButtonList id="tipoTaller" BorderStyle="None" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Tipo_Taller"
									RepeatDirection="Horizontal">
									<asp:ListItem Value="T" Selected="True">Todos los Talleres</asp:ListItem>
									<asp:ListItem Value="E">Un Taller Específico</asp:ListItem>
								</asp:RadioButtonList>
							</p>
                    <p>
								<asp:Label id="labelTaller" runat="server" enabled="False">Taller Específico : </asp:Label>&nbsp;&nbsp;
								<asp:DropDownList id="taller" runat="server" class="dmediano" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Taller"
									Enabled="False"></asp:DropDownList>
							</p>
              
					</td>
				</tr>
			</tbody>
		</table>

		<table id="Table1" class="filtersIn">
			<tbody>
				<tr>
					<td>
						
							<legend>Tipo de Empleado a Liquidar</legend>
							<asp:RadioButtonList id="tipoEmpleado" BorderStyle="None" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Tipo_Empleado">
								<asp:ListItem Value="RT" Selected="True">Recepcionista</asp:ListItem>
								<asp:ListItem Value="MG">Técnico</asp:ListItem>
							</asp:RadioButtonList>
						
					</td>
					<td>
					
                    <p>
							</p>
                    <legend>Empleados a Liquidar</legend>Seleccione si va a liquidar todos los talleres
                    o uno específico : 
                    <p>
								<asp:RadioButtonList id="grupoEmpleado" BorderStyle="None" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Tipo_Conjunto_Empleado"
									RepeatDirection="Horizontal">
									<asp:ListItem Value="T" Selected="True">Todos los Empleados</asp:ListItem>
									<asp:ListItem Value="E">Empleado Específico</asp:ListItem>
								</asp:RadioButtonList>
							</p>
                    <p>
								<asp:Label id="labelEmpleado" runat="server" enabled="False">Empleado Específico : </asp:Label>&nbsp;&nbsp;
								<asp:DropDownList id="empleado" runat="server" class="dmediano" Enabled="False"></asp:DropDownList>
							</p>
                
					</td>
				</tr>
			</tbody>
		</table>

		<table id="Table2" class="filtersIn">
			<tbody>
				<tr>
					<td>
						
                    <legend>Parámetros</legend>Seleccione si va a liquidar mediante los Parámetros del
                    Sistema o Tabla Diferencial 
                    <p>
								<asp:RadioButtonList id="tipoParametro" BorderStyle="None" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Tipo_Parametro"
									RepeatDirection="Horizontal">
									<asp:ListItem Value="P" Selected="True">Parámetros del Sistema</asp:ListItem>
									<asp:ListItem Value="D">Tabla Diferencial</asp:ListItem>
								</asp:RadioButtonList>
							</p>
                    <p>
								<asp:DataGrid id="grillaTablaDiferencial" BorderStyle="None" runat="server" cssclass="datagrid" Enabled="False"
									GridLines="Vertical" AutoGenerateColumns="false" OnItemCommand="dgSeleccion_TablaDiferencial"
									ShowFooter="True">
									<FooterStyle CssClass="footer"></FooterStyle>
						            <HeaderStyle CssClass="header"></HeaderStyle>
						            <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						            <ItemStyle CssClass="item"></ItemStyle>
									<Columns>
										<asp:TemplateColumn HeaderText="HASTA">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "HASTA", "{0:N}") %>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox id="aPartir" runat="server" class="tpequeno"></asp:TextBox>
											</FooterTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="PAGAR">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "PAGAR", "{0:C}") %>
											</ItemTemplate>
											<FooterTemplate>
												<asp:TextBox id="pago" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:TextBox>
											</FooterTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="SELECCIONADO">
											<ItemTemplate>
												<asp:Button CommandName="Remover" Text="Remover" ID="btnRmv2" runat="server" />
											</ItemTemplate>
											<FooterTemplate>
												<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd2" runat="server" />
											</FooterTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:DataGrid>
							</p>
                
					</td>
				</tr>
				<tr>
					<td>
					
                    <p>
							</p>
                    <legend>Parámetros de Fecha</legend>Seleccione si va Liquidar de Forma General o entre
                    un Intervalo de tiempo específico 
                    <p>
								<asp:RadioButtonList id="tipoFecha" BorderStyle="None" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Tipo_Fecha"
									RepeatDirection="Horizontal">
									<asp:ListItem Value="G" Selected="True">General</asp:ListItem>
									<asp:ListItem Value="I">Intervalo de Tiempo</asp:ListItem>
								</asp:RadioButtonList>
							</p>
                    <p>
								<asp:Label id="labelFecha1" runat="server" enabled="False">Fecha Inicial :</asp:Label>&nbsp;&nbsp;
								<ew:calendarpopup id="calFechaInicial" ImageUrl="../img/AMS.Icon.Calendar.gif" width= "100" ControlDisplay="TextBoxImage"
									runat="server" Culture="Spanish (Colombia)" Enabled="False">
									<WeekdayStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="White"></WeekdayStyle>
									<MonthHeaderStyle Font-Size="X-Small" Font-Names="Arial" ForeColor="Black" BackColor="Silver"></MonthHeaderStyle>
									<OffMonthStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="#FF8080"></OffMonthStyle>
									<GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="White"></GoToTodayStyle>
									<TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="LightGoldenrodYellow"></TodayDayStyle>
									<DayHeaderStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightBlue"></DayHeaderStyle>
									<WeekendStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightGray"></WeekendStyle>
									<SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="Khaki"></SelectedDateStyle>
									<ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="White"></ClearDateStyle>
									<HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="White"></HolidayStyle>
								</ew:calendarpopup>
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label id="labelFecha2" runat="server" enabled="False">Fecha Final :</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
								<ew:calendarpopup id="calFechaFinal" ImageUrl="../img/AMS.Icon.Calendar.gif"  width= "100" ControlDisplay="TextBoxImage"
									runat="server" Culture="Spanish (Colombia)" Enabled="False">
									<WeekdayStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="White"></WeekdayStyle>
									<MonthHeaderStyle Font-Size="X-Small" Font-Names="Arial" ForeColor="Black" BackColor="Silver"></MonthHeaderStyle>
									<OffMonthStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="#FF8080"></OffMonthStyle>
									<GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="White"></GoToTodayStyle>
									<TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="LightGoldenrodYellow"></TodayDayStyle>
									<DayHeaderStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightBlue"></DayHeaderStyle>
									<WeekendStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightGray"></WeekendStyle>
									<SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="Khaki"></SelectedDateStyle>
									<ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="White"></ClearDateStyle>
									<HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
										BackColor="White"></HolidayStyle>
								</ew:calendarpopup>
							</p>
              
					</td>
				</tr>
			</tbody>
		</table>
		<p>
		</p>
		<p>
			<asp:Button id="ejecutar" onclick="Ejecutar_Accion" runat="server" Width="95px" Text="Ejecutar"></asp:Button>
		</p>
		<p>
			<asp:PlaceHolder id="toolsHolder" runat="server">
				<table class="filtersIn">
					<TR>
						<th class="filterHead">
			   <IMG height="16" src="../img/AMS.Flyers.Tools.png" border="0">
			</th>
            <td>
            <p>
            <table id="Table" class="filtersIn">
            <tr>
            <td>
            <TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</TD>
						<TD>&nbsp; &nbsp;Enviar por correo
							<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
						<TD>
							<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
								ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
							<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" ImageUrl="../img/AMS.Icon.Mail.jpg"
								alt="Enviar por email"></asp:ImageButton></TD>
						<TD></TD>
                        </td>
            </tr>
                        </table>
                        </p>
                        </td>
					</TR>
				</TABLE>
			</asp:PlaceHolder>
		</p>
		<p>
			<asp:PlaceHolder id="controlesResultado" runat="server"></asp:PlaceHolder>
		</p>
		<p>
		</p>
		<p>
			<asp:Label id="lb" runat="server"></asp:Label>
		</p>
        </fieldset>
