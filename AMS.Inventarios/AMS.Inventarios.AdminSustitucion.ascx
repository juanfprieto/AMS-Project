<%@ Control Language="c#" codebehind="AMS.Inventarios.AdminSustitucion.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.AdminSustitucion" %>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
				<img height="57" src="../img/AMS.Flyers.Nueva.png" border="0">
			</tH>
			<td> 
				<table class="filtersIn">
					<tbody>
						<tr>
							<td>
								Prefijo Documento Sustitucion :<br />
								<asp:DropDownList id="ddlPrefDocu" class="dmediano" runat="server" OnSelectedIndexChanged="CambioDocumento" AutoPostBack="True"></asp:DropDownList>
							</td>
                            <td>
								Número Documento Sustitución :<br />
								<asp:TextBox id="tbNumDocu" runat="server" class="tpequeno"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Button id="btnAceptar" onclick="CrearSustitucion" runat="server" Text="Aceptar"></asp:Button>
							</td>
						</tr>
					</tbody>
				</table>
			</td>
		</tr>
	</tbody>
</table>

<asp:Label id="lb" runat="server"></asp:Label>
