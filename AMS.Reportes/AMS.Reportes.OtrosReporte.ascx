<%@ Control Language="c#" codebehind="AMS.Reportes.OtrosReporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Otros" %>
<script language="javascript" src="../js/AMS.Web.ConfiguracionOperacion.js" type="text/javascript"></script>
<p>
	Filas Especiales : Estas son filas que muestran informacion especial y 
	especifica, que puede estar relacionada con alguna columna del cuerpo del 
	informe o simplemente con alguna informacion. Pueden ser de dos tipos 
	:&nbsp;Filas&nbsp;Externas o Filas Relacionadas
</p>
<p>
</p>
<fieldset style="WIDTH: 598px; HEIGHT: 60px">
	<legend>Filas Estaticas</legend>
	<table class="main">
		<tbody>
			<tr>
				<td>
					Posición con Respecto al Cuerpo del Reporte :
					<asp:DropDownList id="posRep1" runat="server">
						<asp:ListItem Value="Ar" Selected="True">Arriba</asp:ListItem>
						<asp:ListItem Value="Ab">Abajo</asp:ListItem>
					</asp:DropDownList>
				</td>
				<td>
					Orden de Presentación :&nbsp;
					<asp:TextBox id="ordPrs1" runat="server" Width="59px"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Texto o Etiqueta :
					<asp:TextBox id="text1" runat="server" Width="208px"></asp:TextBox>
				</td>
				<td>
					Alineación :
					<asp:DropDownList id="alin1" runat="server">
						<asp:ListItem Value="Left" Selected="True">Izquierda</asp:ListItem>
						<asp:ListItem Value="Right">Derecha</asp:ListItem>
						<asp:ListItem Value="Middle">Centrado</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					Opciones :
					<asp:RadioButtonList id="rdOpc" runat="server" RepeatDirection="Horizontal">
						<asp:ListItem Value="N" Selected="True">Ninguna</asp:ListItem>
						<asp:ListItem Value="FH">Fecha y Hora</asp:ListItem>
						<asp:ListItem Value="U">Usuario</asp:ListItem>
					</asp:RadioButtonList>
				</td>
			</tr>
		</tbody>
	</table>
	<asp:Button id="btnAgr1" onclick="Agregar_Estatica" runat="server" CausesValidation="False"
		Text="Agregar"></asp:Button>
</fieldset>
<p>
</p>
<p>
</p>
<fieldset style="WIDTH: 598px; HEIGHT: 60px">
    <legend>Filas Dinamicas</legend>
    <table class="main">
		<tbody>
			<tr>
				<td>
					Posición con Respecto al Cuerpo del Reporte :
					<asp:DropDownList id="posRep2" runat="server">
						<asp:ListItem Value="Ar" Selected="True">Arriba</asp:ListItem>
						<asp:ListItem Value="Ab">Abajo</asp:ListItem>
					</asp:DropDownList>
				</td>
				<td>
					Orden de Presentación :&nbsp;
					<asp:TextBox id="ordPrs2" runat="server" Width="59px"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Texto o Etiqueta :
					<asp:TextBox id="text2" runat="server" Width="208px"></asp:TextBox>
				</td>
				<td>
					Alineación :
					<asp:DropDownList id="alin2" runat="server">
						<asp:ListItem Value="Left" Selected="True">Izquierda</asp:ListItem>
						<asp:ListItem Value="Right">Derecha</asp:ListItem>
						<asp:ListItem Value="Middle">Centrado</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<p>
						Tabla :
						<asp:DropDownList id="ddlTablas" runat="server" OnSelectedIndexChanged="Cambio_Tabla" AutoPostBack="True"></asp:DropDownList>
						&nbsp;
					</p>
					<p>
						Campos :
						<asp:DropDownList id="ddlCampos" runat="server" OnSelectedIndexChanged="Cambio_Campo" AutoPostBack="True"></asp:DropDownList>
						&nbsp;<asp:Button id="btnConfi" runat="server" CausesValidation="False" Text="Configurar"></asp:Button>
					</p>
				</td>
			</tr>
		</tbody>
	</table>
    Resultado configuración :
    <asp:TextBox id="tbResultado" runat="server" Width="209px" ReadOnly="True"></asp:TextBox>
    &nbsp;&nbsp;
    <asp:Button id="btnAcpConf" onclick="Aceptar_Dinamica" runat="server" Text="Aceptar Configuración"></asp:Button>
</fieldset>
<p>
</p>
<p>
	<asp:DataGrid id="dgFilas" runat="server" Width="551px" BackColor="White" AutoGenerateColumns="False"
		Font-Names="Verdana" BorderWidth="1px" GridLines="Vertical" BorderColor="#999999" CellPadding="3"
		Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" BorderStyle="None">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="POSICI&#211;N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "POSICION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ORDEN PRESENTACI&#211;N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ORDEN") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ALINEACI&#211;N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ALINEACION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="VALOR">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="OPCI&#211;N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OPCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="TABLAS ASOCIADAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TABLAS") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Button id="btnAceptar" onclick="Aceptar_Filas" runat="server" Text="Aceptar Filas"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
