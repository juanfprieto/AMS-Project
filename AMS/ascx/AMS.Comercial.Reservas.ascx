<%@ Control Language="c#" codebehind="AMS.Comercial.Reservas.ascx.cs" autoeventwireup="false" Inherits="AMS.Comercial.AMS_Comercial_Reservas" %>
<script language="JavaScript">
    function Prueba(objS)
    {
        document.frames['frameDibujo'].location = "../aspx/AMS.Comercial.DibujoR.aspx?val="+objS.value+"&fte=1";
    }
</script>
<table style="WIDTH: 700px; HEIGHT: 700px" bordercolor="#ffffff" width="597">
	<tbody>
		<tr>
			<td align="center">
				<p>
				</p>
				<p>
					<asp:Label id="resulBusq" runat="server"></asp:Label>
				</p>
				<fieldset style="WIDTH: 300px">
					<legend>Rutas Disponibles&nbsp; Para Reservas</legend>
					<asp:ListBox id="rutasDisponibles" onclick="Prueba(this)" runat="server" Width="252px" Visible="False"></asp:ListBox>
				</fieldset>
				<p>
				</p>
				<p>
					Busqueda
				</p>
				<fieldset style="WIDTH: 300px">
					<legend>Parámetro de fecha</legend>
					<asp:CheckBox id="parFecha" runat="server" Text="Utilizar este Parametro"></asp:CheckBox>
					<p align="center">
						<asp:Calendar id="fechaRuta" runat="server"></asp:Calendar>
					</p>
					<p align="center">
						<asp:RadioButtonList id="especificar" runat="server" RepeatDirection="Horizontal">
							<asp:ListItem Value="&lt;=" Selected="True">Antes de</asp:ListItem>
							<asp:ListItem Value="=">Fecha Exacta</asp:ListItem>
							<asp:ListItem Value="&gt;=">Despues de</asp:ListItem>
						</asp:RadioButtonList>
					</p>
				</fieldset>
				<fieldset style="WIDTH: 300px">
                    <legend>Parámetros de Origen - Destino</legend>Ciudad Origen : 
                    <asp:DropDownList id="ciudadOrigen" runat="server" Width="97px"></asp:DropDownList>
                    &nbsp;<asp:CheckBox id="parOri" runat="server" Text="Utilizar"></asp:CheckBox>
                    <p>
						Ciudad Destino :
						<asp:DropDownList id="ciudadDestino" runat="server" Width="97px"></asp:DropDownList>
						&nbsp;<asp:CheckBox id="parDes" runat="server" Text="Utilizar"></asp:CheckBox>
					</p>
                </fieldset>&nbsp;
				<p>
					<asp:Button id="buscar" onclick="Realizar_Busqueda" runat="server" Text="Buscar"></asp:Button>
				</p>
			</td>
			<td>
				<p>
				</p>
				<iframe id="frameDibujo" src="../aspx/AMS.Comercial.DibujoR.aspx?val=inicio" width="360"
					height="600"></iframe>
			</td>
		</tr>
	</tbody>
</table>
<asp:Label id="lb" runat="server"></asp:Label>
