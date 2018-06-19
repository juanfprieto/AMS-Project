<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Garantias.Factibilidad.ascx.cs" Inherits="AMS.Garantias.AMS_Garantias_Factibilidad" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<asp:panel id="Panel1" Height="136px" Width="648px" runat="server" HorizontalAlign="Left" BorderStyle="None">
	<P><STRONG><EM style="FONT-STYLE: normal; TEXT-ALIGN: left">Consulta de Factibilidad de 
				Garantía&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</EM></STRONG>Fecha:&nbsp;
		<asp:Label id="FechaActual" runat="server" Width="112px">AAAA-MM-DD</asp:Label></P>
	<P>&nbsp;Prefijo y Numero de la solicitud de Garantia:
		<asp:DropDownList id="ddlPrefijo" runat="server" OnChange="consecutivo(this)"></asp:DropDownList>
		<asp:TextBox id="tbPdoc_codigo" runat="server" class="tpequeno"></asp:TextBox></P>
	<TABLE id="Table1" class="filtersIn">
		<TR>
			<TD>Catálogo:</TD>  
			<TD>
				<asp:DropDownList id="ddlCatalogo" runat="server" OnChange="CargarVin(this);"></asp:DropDownList></TD>
		</TR>
		<TR>
			<TD align="left" colSpan="1" rowSpan="1">
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="9px" ControlToValidate="tbVin"
					ErrorMessage="*"></asp:RequiredFieldValidator></TD>
			<TD>
				<asp:TextBox id="tbVin" runat="server" class="tmediano"></asp:TextBox>
				<asp:DropDownList id="ddlOpcion" runat="server" class="dmediano">
					<asp:ListItem Value="MCAT_VIN">Vin</asp:ListItem>
					<asp:ListItem Value="MCAT_MOTOR">Motor</asp:ListItem>
					<asp:ListItem Value="MCAT_VIN">Chasis</asp:ListItem>
				</asp:DropDownList></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label1" runat="server" class="lmediano">C.C o Nit</asp:Label></TD>
			<TD>
				<asp:TextBox id="tbId" runat="server" class="dmediano"></asp:TextBox>
				<asp:Button id="Consultar_G" runat="server" class="bpequeno" Text="Consultar" onclick="Consultar_G_Click"></asp:Button></TD>
		</TR>
	</TABLE>
</asp:panel>
<P><asp:label id="Label3" runat="server" BackColor="WhiteSmoke" ForeColor="MidnightBlue">*Datos del vehiculo</asp:label></P>
<asp:panel id="Panel4" Height="290px" Width="536px" runat="server" HorizontalAlign="Left" Visible="False">
	<P>Nombre del cliente:
		<asp:Label id="tbCliente" runat="server">NomCliente</asp:Label></P>
	<P>C.C o Nit:&nbsp;
		<asp:Label id="tbDoc" runat="server" class="lgrande">DocCliente</asp:Label></P>
	<P>
		<TABLE id="Table2" class="fieltersIn">
			<TR>
				<TD bgColor="whitesmoke" colSpan="1" rowSpan="1">Catálogo 
					:</TD>
				<TD bgColor="whitesmoke" colSpan="1" rowSpan="1">
					<asp:Label id="lbPcatcodigo" runat="server" Width="136px">codigocat</asp:Label></TD>
				<TD bgColor="whitesmoke" colSpan="4">
					<asp:Label id="tbCatalogo" runat="server" class="lgrande">Catalogo</asp:Label></TD>
			</TR>
			<TR>
				<TD bgColor="#ffffff">Año modelo:</TD>
				<TD bgColor="#ffffff">
					<asp:Label id="lbAModeloV" runat="server" class="lpequeno">AModeloV</asp:Label></TD>
				<TD bgColor="#ffffff">Marca:</TD>
				<TD bgColor="#ffffff">
					<asp:Label id="lbMarca" runat="server" class="lmediano">Marca</asp:Label></TD>
			</TR>
			<TR>
				<TD bgColor="#f5f5f5">Placa:</TD>
				<TD bgColor="#f5f5f5">
					<asp:Label id="lbPlacaV" runat="server" class="lpequeno">PlacaV</asp:Label></TD>
				<TD bgColor="#f5f5f5">Color:</TD>
				<TD bgColor="whitesmoke">
					<asp:Label id="lbColorV" runat="server" class="lmediano">ColorVeh</asp:Label></TD>
			</TR>
			<TR>
				<TD bgColor="#ffffff">Concesionario</TD>
				<TD bgColor="#ffffff">
					<asp:Label id="lbConcesionario" runat="server" class="lpequeno">Concesionario</asp:Label></TD>
				<TD bgColor="#ffffff">Motor:</TD>
				<TD bgColor="#ffffff">
					<asp:Label id="lbMotorV" runat="server" class="lmediano" DESIGNTIMEDRAGDROP="1252">MotorV</asp:Label></TD>
			</TR>
			<TR>
				<TD bgColor="#f5f5f5">Fecha de Entrega:</TD>
				<TD bgColor="#f5f5f5">
					<asp:Label id="lbFechaCompra" runat="server">FechaCompra</asp:Label></TD>
				<TD bgColor="#f5f5f5">VIN:</TD>
				<TD bgColor="#f5f5f5">
					<asp:Label id="lb2VIN" runat="server" Width="215px" Height="12px">VIN</asp:Label></TD>
			</TR>
			<TR>
				<TD bgColor="#ffffff">Ultimo kilometraje</TD>
				<TD bgColor="#ffffff">
					<asp:Label id="lbUltiKilo" runat="server" class="lpequeno">ultikilo</asp:Label></TD>
				<TD bgColor="#ffffff">Kilometraje :</TD>
				<TD bgColor="#ffffff" colSpan="1" rowSpan="1">&nbsp;
					<asp:TextBox id="tbKilometros" onkeyup="NumericMaskE(this,event);" runat="server" class="tpequeno"></asp:TextBox>&nbsp;Kmts
					<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="1px" ControlToValidate="tbKilometros"
						ErrorMessage="*"></asp:RequiredFieldValidator>
					<asp:Button id="btAplicar" runat="server" class="bpequeno" Text="Aplicar" onclick="btaplicar_Click"></asp:Button></TD>
			</TR>
		</TABLE>
	</P>
</asp:panel><asp:panel id="Panel3" Height="510px" Width="584px" runat="server" HorizontalAlign="Center"
	BorderStyle="None" Visible="False" BorderColor="White" BorderWidth="2px">
	<P align="left">
		<asp:Label id="Label2" runat="server" class="lmediano" ForeColor="Red" BackColor="Transparent"
			Font-Bold="True">SOLICITUD NO APROBADA</asp:Label></P>
	<P align="left">
		<asp:Label id="lbMensaje" runat="server" class="lgrande" ForeColor="Red">Mensaje</asp:Label>
		<asp:Label id="lbFaltaKit" runat="server" ForeColor="Red"></asp:Label></P>
	<P align="left">Tiempo de garantia del Vehiculo: &nbsp;
		<asp:Label id="lbGarTiempo" runat="server" class="lpequeno" ForeColor="RoyalBlue" Font-Bold="True"
			Font-Size="Small">GaTie</asp:Label>Meses</P>
	<P align="left">Garantia por Kilometraje :
		<asp:Label id="lbgarantiaKm" runat="server" class="lpequeno" ForeColor="RoyalBlue" Font-Bold="True"
			Font-Size="Small">Garantiakm</asp:Label>&nbsp;Kmts</P>
	<P align="left">Tiempo trancurrido a partir de la fecha de entega :&nbsp;&nbsp;
		<asp:Label id="lbMeses" runat="server" class="lpequeno" ForeColor="RoyalBlue" Font-Bold="True" Font-Size="Small"
			Font-Names="Times New Roman">M</asp:Label>&nbsp;Meses y
		<asp:Label id="lbDias" runat="server" class="lpequeno" ForeColor="RoyalBlue" Font-Bold="True" Font-Size="Small"
			Font-Names="Times New Roman">D</asp:Label>&nbsp;Dias</P>
	<P align="left">
		<asp:Label id="Label4" runat="server"  ForeColor="MidnightBlue" BackColor="WhiteSmoke">*Ultimos Kits Aplicados</asp:Label></P>
	<P align="left">
		<asp:DataGrid id="dgKits" runat="server" Visible="False" AutoGenerateColumns="False">
			<Columns>
				<asp:BoundColumn DataField="PREFIJO_ORDEN" HeaderText="PREFIJO ORDEN"></asp:BoundColumn>
				<asp:BoundColumn DataField="NUM_ORDEN" HeaderText="N&#186; DE ORDEN "></asp:BoundColumn>
				<asp:BoundColumn DataField="FECHA_ENTRADA" HeaderText="FECHA DE ENTRADA" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
				<asp:BoundColumn DataField="MORD_KILO" HeaderText="KILOMETRAJE DEL VEHICULO"></asp:BoundColumn>
				<asp:BoundColumn DataField="CODIGO_KIT" HeaderText="CODIGO DEL KIT"></asp:BoundColumn>
				<asp:BoundColumn DataField="NOMBRE_KIT" HeaderText="NOMBRE DEL KIT"></asp:BoundColumn>
				<asp:BoundColumn DataField="KILOMETRAJE" HeaderText="KILOMETRAJE A APLICAR"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="VALIDO">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
						<asp:Image id="imVale" runat="server"></asp:Image>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid></P>
	<P align="left">
		<asp:Button id="ValidarGar" runat="server" class="bpequeno" Text="Enviar solicitud" onclick="ValidarGar_Click"></asp:Button></P>
	<P align="left">
</asp:panel></P>
<script language="javascript">
function consecutivo (obj)
{
AMS_Garantias_Factibilidad.cargar_consecutivo(obj, consecutivo_retorno)
}
function consecutivo_retorno(response)
{
	var respuesta=response.value;
	var codigo=document.getElementById("<%=tbPdoc_codigo.ClientID%>");
	if(respuesta!="")
	{
		codigo.value=respuesta;
	}
}
</script>
