<%@ Control Language="c#" codebehind="AMS.Comercial.ProcesoReservas.ascx.cs" autoeventwireup="false" Inherits="AMS.Comercial.ProcesoReservas" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset style="WIDTH: 560px">
	<legend>Datos Sobre la Ruta</legend>
	<p>
		<table class="main" bordercolor="#ffffff" width="550" height="136">
			<tbody>
				<tr>
					<td>
						Identificador de la Ruta :
						<asp:Label id="idRuta" runat="server" width="51px"></asp:Label></td>
					<td>
						Descripción de la Ruta :
						<asp:Label id="dscRuta" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td>
						Ciudad Origen :
						<asp:Label id="ciuOrigen" runat="server"></asp:Label></td>
					<td>
						Ciudad Destino :
						<asp:Label id="ciuDestino" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td>
						Fecha Salida :
						<asp:Label id="fchSalida" runat="server"></asp:Label></td>
					<td>
						Hora Salida :
						<asp:Label id="hrSalida" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td>
						Tipo Vehiculo :
						<asp:Label id="tipVehiculo" runat="server"></asp:Label></td>
					<td>
						Placa del Vehiculo :
						<asp:Label id="plcVehiculo" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td>
						Capacidad :
						<asp:Label id="capacidad" runat="server"></asp:Label></td>
					<td>
						Disponibilidad :
						<asp:Label id="disponibilidad" runat="server"></asp:Label></td>
				</tr>
			</tbody>
		</table>
	</p>
</fieldset>
<p>
</p>
<p>
	<asp:DataGrid id="grillaInformativa" runat="server" Width="595px" HeaderStyle-BackColor="#ccccdd"
		Font-Size="8pt" Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White"
		BorderStyle="None" GridLines="Vertical" Font-Names="Verdana" AutoGenerateColumns="False" BorderWidth="1px">
		<Columns>
			<asp:TemplateColumn HeaderText="N&#218;MERO SILLA (FILA-COLUMNA)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NUMSILLA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DOCUMENTO IDENTIFICACI&#211;N">
				<ItemTemplate>
					<asp:TextBox id="numeDocu" runat="server" ReadOnly="true" onclick="ModalDialog(this,'SELECT * FROM mpasajerofrecuente', new Array(),1)"></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NOMBRE CLIENTE">
				<ItemTemplate>
					<asp:TextBox id="numeDocua" runat="server" ReadOnly="true"></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="CONFIRMAR">
				<ItemTemplate>
					<asp:CheckBox id="confirmar" runat="server" Checked="true" Text="Reservar Tiquete"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<SelectedItemStyle forecolor="White" font-bold="True" backcolor="#008A8C"></SelectedItemStyle>
		<HeaderStyle forecolor="White" font-bold="True" backcolor="#000084"></HeaderStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
	</asp:DataGrid>
</p>
<P>&nbsp;</P>
<P style="WIDTH: 595px" align="right">
	<asp:Button id="recalculo" onclick="Recalcula_Total" runat="server" Text="Recalcular Total"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;Total 
	de la Venta :
	<asp:Label id="total" runat="server"></asp:Label></P>
<P>&nbsp;</P>
<fieldset style="WIDTH: 595px">
    <legend>Información Oficina de Venta</legend>Oficina de Venta : 
    <asp:DropDownList id="oficinas" runat="server" Width="192px"></asp:DropDownList>
</fieldset>
<p>
	&nbsp;<asp:Button id="reservar" onclick="Reservar_Tiquetes" runat="server" Width="156px" Text="Reservar Tiquetes"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
