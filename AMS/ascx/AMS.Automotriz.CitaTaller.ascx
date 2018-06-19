<%@ Control Language="c#" codebehind="AMS.Automotriz.CitaTaller.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.CitaTaller" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<fieldset>
<p>
	<table class="filters">
		<tbody>
			<tr>
				<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Consulta.png" border="0">
			</th>
				<td>
                <p>
					<table id="Table" class="filtersIn">
						<tr>
							<td colspan="2">Taller Nº :<asp:dropdownlist id="taller" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Taller"></asp:dropdownlist></td>
							<td>Fecha Preferencia :</td>
							<td><ew:calendarpopup id="calFecha" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage"
									runat="server" Culture="Spanish (Colombia)" width="80">
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
								</ew:calendarpopup></td>
						</tr>
						<tr>
							<%--<td>Hora de Preferencia:</td>--%>
							<td align="right"><asp:dropdownlist id="horaPref" runat="server" Visible = "false" ></asp:dropdownlist></td>
							<td colspan="2" align="right"><asp:button id="consulta" onclick="Consultar_Citas" runat="server" Text="Realizar Consulta"></asp:button></td>
						</tr>
					</table>
                    </p>
				</td>
			</tr>
		</tbody>
    </table>
</p>
<p>
	<table class="filters">
		<tbody>
			<tr>
				<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Nueva.png" border="0">
			</th>
				<td>
                <p>
                <table id="Table" class="filtersIn">
						<tr>
							<td>Placa :</td>
							<td><asp:textbox id="placa" runat="server" class="tpequeno"></asp:textbox></td>
							<td align=right>Clave :</td>
							<td><asp:textbox id="clave" runat="server" class="tpequeno" TextMode="Password"></asp:textbox></td>
                            <td><asp:button id="consultaVehiculo" onclick="Consultar_Vehiculo" runat="server" Text="Consultar"></asp:button></td>
						</tr>
						<tr>
							<td>Vehículo :</td>
							<td colspan="3"><asp:dropdownlist id="vehiculos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Vehiculo"
									class="dmediano"></asp:dropdownlist></td>
                            <td></td>
                        </tr>
                        <tr>
							<asp:PlaceHolder ID="phDatosCliente" runat="server"></asp:PlaceHolder>
						</tr>
						<tr>
							<td>Observaciones :</td>
							<td colspan="3"><asp:textbox id="tbObservaciones" runat="server" class="amediano" TextMode="MultiLine"></asp:textbox></td>
                            <td></td>
						</tr>
						<tr>
							<%--<td>Recepcionista :</td>--%>
							<td colspan="2"><asp:dropdownlist id="recepcion" class="dmediano" Visible="false" runat="server" AutoPostBack="true"></asp:dropdownlist></td>
							<%--<td align=right>Hora Escogida :</td>--%>
							<td><asp:dropdownlist id="horaEsc" width="100" runat="server" Visible="false"></asp:dropdownlist></td>
						</tr>
						<tr>
							<td>Servicio :</td>
							<td colspan="3"><asp:dropdownlist id="servicio" class="dmediano" runat="server"></asp:dropdownlist>
                            <asp:Image id="bntLupa" runat="server"  ImageUrl="../img/infoLibro.jpg" onClick="ModalDialogCont('M','Elementos del KIT',servicio,sqlKit);"></asp:Image>
                            </td>
							<td><asp:button id="guardar" Visible="false" runat="server" Text="Guardar Cita"></asp:button></td>
						</tr>
					</table>
                    </p>
				</td>
			</tr>
		</tbody>
	</table>
    <div id="modalCont" style="visibility:hidden"></div>
</p>

<p><asp:label id="lbErr" runat="server"></asp:label></p>
<asp:datagrid id="dgCitas" runat="server"></asp:datagrid>
</fieldset>
<script type="text/javascript">
    //Registro de IDs
    var servicio = '<%= servicio.ClientID %>';

    //Registro de SQLs
    var sqlKit = 'call dbxschema.consultar_itemskit( (SELECT ppre_codigo FROM pkit WHERE pkit_codigo=\'$\'),\'$\' ) ';
</script>