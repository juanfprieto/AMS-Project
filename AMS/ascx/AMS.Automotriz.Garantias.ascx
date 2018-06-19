<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.Garantias.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_Garantias" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:panel id="Panel1" HorizontalAlign="Center" runat="server" Width="696px" Height="126px">
	<P><STRONG><EM>Consulta de Factibilidad de Garantía</EM></STRONG></P>
	<P>Fecha:&nbsp;
		<asp:Label id="FechaActual" class="lpequeno" runat="server">AAAA-MM-DD</asp:Label></P>
	<TABLE id="Table1" class="filtersIn" cellSpacing="1" cellPadding="1" 
		border="1">
		<TR>
			<TD>Catálogo:</TD>
			<TD>
				<asp:DropDownList id="ddlCatalogo" class="dmediano" runat="server" OnChange="CargarVin(this);"></asp:DropDownList></TD>
			<TD>
				<P>&nbsp;</P>
			</TD>
		</TR>
		<TR>
			<TD>VIN:</TD>
			<TD>
				<asp:DropDownList id="ddlVin" class="dgrande" runat="server"></asp:DropDownList></TD>
			<TD>
				<P>
					<asp:Button id="Consultar_G" class="bpequeno" runat="server" Text="Consultar" DESIGNTIMEDRAGDROP="761" onclick="Consultar_G_Click"></asp:Button></P>
			</TD>
		</TR>
	</TABLE>
</asp:panel>
<asp:Panel id="Panel4" HorizontalAlign="Center" runat="server" Visible="False">
	<P><STRONG><FONT color="#000000">Datos del Vehículo:</FONT></STRONG>
		<TABLE id="Table2" style="WIDTH: 680px; HEIGHT: 98px" cellSpacing="1" cellPadding="1" width="680"
			bgColor="lavender" border="1">
			<TR>
				<TD>Cliente:</TD>
				<TD>
					<asp:Label id="TxtCliente" Width="144px" runat="server" DESIGNTIMEDRAGDROP="195">NomCliente</asp:Label></TD>
				<TD>Documento:</TD>
				<TD>
					<asp:Label id="TxtDoc" Width="144px" runat="server">DocCliente</asp:Label></TD>
			</TR>
			<TR>
				<TD>Catálogo:</TD>
				<TD>
					<asp:Label id="Catalogo" class="lpequeno" runat="server" DESIGNTIMEDRAGDROP="195">Catalogo</asp:Label></TD>
				<TD>Marca:</TD>
				<TD>
					<asp:Label id="Marca" class="lpequeno" runat="server">Marca</asp:Label></TD>
			</TR>
			<TR>
				<TD>Año Modelo:</TD>
				<TD>
					<asp:Label id="AModeloV" class="lpequeno" runat="server" DESIGNTIMEDRAGDROP="195">AModeloV</asp:Label></TD>
				<TD>Color:</TD>
				<TD>
					<asp:Label id="ColorV" class="lpequeno" runat="server">ColorVeh</asp:Label></TD>
			</TR>
			<TR>
				<TD>Placa:</TD>
				<TD>
					<asp:Label id="PlacaV" class="lpequeno" runat="server" DESIGNTIMEDRAGDROP="955">PlacaV</asp:Label></TD>
				<TD>Motor:</TD>
				<TD>
					<asp:Label id="MotorV" class="lpequeno" runat="server" DESIGNTIMEDRAGDROP="1252">MotorV</asp:Label></TD>
			</TR>
			<TR>
				<TD>Concesioanrio:</TD>
				<TD>
					<asp:Label id="Concesionario" class="lpequeno" runat="server" DESIGNTIMEDRAGDROP="1048">Concesionario</asp:Label></TD>
				<TD>VIN:</TD>
				<TD>
					<asp:Label id="TxtVIN" class="lpequeno" runat="server">VIN</asp:Label></TD>
			</TR>
			<TR>
				<TD>Fecha de Compra:</TD>
				<TD>
					<asp:Label id="FechaCompra" class="lpequeno" runat="server" DESIGNTIMEDRAGDROP="1049" Font-Bold="True"
						Font-Size="Medium">FechaCompra</asp:Label></TD>
				<TD>Km en Medidor:</TD>
				<TD>
					<asp:DropDownList id="KilomSelect" Width="120px" runat="server" AutoPostBack="True" onselectedindexchanged="KilomSelect_SelectedIndexChanged">
						<asp:ListItem Value="1000">1000</asp:ListItem>
						<asp:ListItem Value="1000">3000</asp:ListItem>
						<asp:ListItem Value="1000">5000</asp:ListItem>
						<asp:ListItem Value="7500">7500</asp:ListItem>
						<asp:ListItem Value="10000">10000</asp:ListItem>
						<asp:ListItem Value="15000">15000</asp:ListItem>
						<asp:ListItem Value="20000">20000</asp:ListItem>
						<asp:ListItem Value="25000">25000</asp:ListItem>
						<asp:ListItem Value="30000">30000</asp:ListItem>
						<asp:ListItem Value="35000">35000</asp:ListItem>
						<asp:ListItem Value="40000">40000</asp:ListItem>
						<asp:ListItem Value="45000">45000</asp:ListItem>
						<asp:ListItem Value="50000">50000</asp:ListItem>
					</asp:DropDownList>Km</TD>
			</TR>
		</TABLE>
	</P>
</asp:Panel>
<asp:panel id="Panel3" HorizontalAlign="Center" runat="server" 
	Visible="False">
	<P><STRONG><FONT color="#000000">Garantía:</FONT></STRONG>
		<TABLE id="Table3" class="filtersIn" cellSpacing="1" cellPadding="1" 
			bgColor="lavender" border="1">
			<TR>
				<TD>Tiempo Transcurrdio desde Compra:</TD>
				<TD>
					<asp:Label id="Meses" class="lpequeno" runat="server" Font-Bold="True" Font-Names="Times New Roman">MM</asp:Label>Meses 
					&nbsp; -&nbsp;&nbsp;
					<asp:Label id="Dias" class="lpequeno" runat="server" Font-Bold="True" Font-Names="Times New Roman">DD</asp:Label>Días</TD>
				<TD>Garantia Kilometraje:</TD>
				<TD>Hasta los&nbsp;&nbsp;&nbsp;
					<asp:Label id="GarKm" class="lpequeno" runat="server" Font-Bold="True" Font-Size="Medium">GarKm</asp:Label>Kilometros</TD>
			</TR>
			<TR>
				<TD>Garantia por Tiempo:</TD>
				<TD>
					<asp:Label id="GarTiempo" class="lpequeno" runat="server" Font-Bold="True" Font-Size="Medium">GarTiempo</asp:Label></TD>
				<TD>Garantia&nbsp;por&nbsp;Kilometraje:</TD>
				<TD>
					<asp:Label id="GarantiaKm" class="lpequeno" runat="server" Font-Bold="True" Font-Size="Medium">GarantiaKm</asp:Label></TD>
			</TR>
		</TABLE>
	</P>
	<P>
		<asp:Button id="ValidarGar" class="bpequeno" runat="server" Text="Aplicar" onclick="ValidarGar_Click"></asp:Button>
</asp:panel></P>
<script language:javascript>

function CargarVin(Obj)
{
	AMS_Automotriz_Garantias.CargarVin(Obj.value,CargarVin_Callback);
}

function CargarVin_Callback(response)
{
	var ddlVin=document.getElementById("<%=ddlVin.ClientID%>");
		var respuesta=response.value;
		if(respuesta.Tables[0].Rows.length>0)
		{
			ddlVin.options.length=0;
			for(var i=0;i<respuesta.Tables[0].Rows.length;i++)
			{
				ddlVin.options[ddlVin.options.length] = new Option(respuesta.Tables[0].Rows[i].VIN,respuesta.Tables[0].Rows[i].VIN);
			}
		}
		else
		{
			alert('NO HAY VINS DISPONIBLES');
			return;
		}	
		
}

</script>
