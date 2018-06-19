<%@ Control Language="c#" codebehind="AMS.Tiquetes.ReservacionTiquetes.ascx.cs" autoeventwireup="false" Inherits="AMS.Comercial.AMS_Tiquetes_ReservacionTiquetes" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<table>
	<tbody>
		<tr>
			<td>
				<table>
					<tbody>
						<tr>
							<td>
								<p>
									Nombres y Apellidos:<br>
									<asp:TextBox id="txtNombres" style="name: txtNIT" Width="253px" runat="server"></asp:TextBox>
									<asp:TextBox id="txtAlm" style="name: txtNIT" ReadOnLy="true" Width="0px" runat="server" Height="0px"></asp:TextBox>
								</p>
							</td>
							<td>
								&nbsp; &nbsp;Ruta:<br>
								&nbsp; &nbsp;<asp:DropDownList id="ddlRuta" runat="server"></asp:DropDownList>
							</td>
							<td>
								&nbsp;&nbsp; Fecha:<br>
								&nbsp; &nbsp;<asp:TextBox id="tbDate" Width="78px" runat="server" ReadOnly="True"></asp:TextBox>
								<img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'"
									src="../img/AMS.Icon.Calendar.gif" border="0">
								<table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
									onmouseout="calendar.style.visibility='hidden'">
									<tbody>
										<tr>
											<td>
												<asp:Calendar id="calDate" runat="server" enableViewState="true" OnSelectionChanged="ChangeDate"></asp:Calendar>
											</td>
										</tr>
									</tbody>
								</table>
							</td>
						</tr>
					</tbody>
				</table>
			</td>
		</tr>
	</tbody>
</table>
<form name="frmReserva">
	<p>
	</p>
	<p>
		<asp:Button id="btnReserva" onclick="NewAjust" runat="server" Enabled="False" Text="Reservar"></asp:Button>
		&nbsp;&nbsp;
	</p>
	<p>
		<asp:Label id="lbInfo" runat="server"></asp:Label>
	</p>
</form>
