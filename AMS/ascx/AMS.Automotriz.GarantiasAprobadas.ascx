<%@ Control Language="c#" codebehind="AMS.Automotriz.GarantiasAprobadas.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.GarantiasAprobadas" %>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.News.png" border="0">
			</th>
			<td>
				<p>
                <table id="Table" class="filtersIn">
                <tr>
                <td>
                
					Prefijo de Factura Generador :
					<asp:DropDownList id="prefFact" class="dmediano" runat="server"></asp:DropDownList>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
					Nit del CSA:
					<asp:TextBox id="nitCSA" onclick="ModalDialog(this, 'SELECT mn.mnit_nit AS NIT, mn.mnit_apellidos ||\' \'|| COALESCE(mn.mnit_apellido2,\'\') ||\' \'|| mn.mnit_nombres ||\' \'|| COALESCE(mn.mnit_nombre2,\'\') AS NOMBRES FROM mnit mn, pcasamatriz pc where mn.mnit_nit=pc.mnit_nit ORDER BY mn.mnit_nit ASC', new Array())"
						runat="server" ReadOnly="True" class="tpequeno"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorNitCSA" runat="server" ControlToValidate="nitCSA" Display="Dynamic"
						Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
				<br />
					&nbsp;<asp:CheckBox id="chkGenNum" Checked="True" Text="Desea Generar el Número Automatico ?" TextAlign="Left"
						runat="server"></asp:CheckBox>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
					Fecha Factura :
					<asp:TextBox id="fechFactura" runat="server" ReadOnly="True" class="tpequeno"></asp:TextBox>
					<img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'"
						src="../img/AMS.Icon.Calendar.gif" border="0">
                        </td>
                </tr>
                        </table> 
                        </p>
                        <p>
					<table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
						onmouseout="calendar.style.visibility='hidden'">
						<tbody>
							<tr>
								<td>
									<asp:calendar BackColor="Beige" id="fecha" runat="server" OnSelectionChanged="ChangeDate"></asp:Calendar>
								</td>
							</tr>
						</tbody>
					</table>
				</p>
				<p>
					<asp:Button id="btnNuevo" onclick="Nueva_Factura" Text="Nuevo" runat="server"></asp:Button>
                    
				</p>
			</td>
		</tr>
	</tbody>
</table>
</fieldset>

<asp:Label id="lb" runat="server"></asp:Label></P>