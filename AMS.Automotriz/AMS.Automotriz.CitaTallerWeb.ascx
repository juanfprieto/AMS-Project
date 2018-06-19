<%@ Control Language="c#" CodeBehind="AMS.Automotriz.CitaTallerWeb.ascx.cs" AutoEventWireup="True" Inherits="AMS.Automotriz.CitaTallerWeb" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<p>
    <table class="filterHead">
        <tbody>
            <tr>
                <th class="filterHead">
			   <IMG height="60" src="../img/AMS.Flyers.Nueva.png" border="0">
			</th>
                <td bgcolor="#f2f2f2">
                    <table class="tablewhite2" cellspacing="2" cellpadding="2" width="100%" border="0">
                        <tr>
                            <td colspan="3">
                                Ingrese la placa de su vehículo y presione Consultar
                            </td>
                        </tr>
                        <tr>
                            <td height="16">
                                <asp:Label ID="lblPlaca" runat="server"> Placa :</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="placa" runat="server" class="tpequeno"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="consultaVehiculo" OnClick="Consultar_Vehiculo" runat="server" Text="Consultar">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblVehiculo" runat="server">Vehiculo :</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehiculo" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblNombre" runat="server" Visible="false">Nombre :</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="nombre" runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTelefono" runat="server" Visible="false">Telefono :</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="telefono" runat="server" class="tpequeno" Visible="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblCelular" runat="server" Visible="false">Movil :</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="celular" runat="server" Visible="false" class="tmediano"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCorreo" runat="server" Visible="false">Correo Electrónico :</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="correo" Visible="false" runat="server"></asp:TextBox>
                            </td>
                        </tr>
						<tr>
							<td valign="top">
                                <asp:Label ID="lblObservaciones" runat="server">Observaciones :</asp:Label>
							</td>
							<td colspan="4">
                                <asp:textbox id="tbObservaciones" runat="server" class="tpequeno" TextMode="MultiLine"></asp:textbox>
                            </td>
						</tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblServicio" runat="server">Servicio :</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="servicio" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>
    <table class="consult" bordercolor="#cdcdcd">
        <tbody>
            <tr>
                <th class="filterHead">
			   <IMG height="60" src="../img/AMS.Flyers.Consulta.png" border="0">
			</th>
                <td bgcolor="#f2f2f2" valign="middle">
                    <table class="tablewhite" cellspacing="2" cellpadding="0" border="0" width="100%">
                        <tr>
                            <td colspan="4">
                                Seleccione el taller y la fecha de su preferencia y haga click en Actualizar para
                                ver la disponibilidad de los técnicos
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Taller :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTaller" runat="server" OnSelectedIndexChanged="Cambio_Taller" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>
                                Fecha Preferencia :
                            </td>
                            <td>
                                <ew:calendarpopup id="calFecha" imageurl="../img/AMS.Icon.Calendar.gif" controldisplay="TextBoxImage"
                                    runat="server" culture="Spanish (Colombia)">
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
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="consulta" OnClick="Consultar_Citas" runat="server" Text="Actualizar">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>
    <asp:Label ID="lbErr" runat="server"></asp:Label></p>
<asp:DataGrid ID="dgCitas" runat="server">
</asp:DataGrid>