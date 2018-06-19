<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ConfiguracionOperacion.aspx.cs" Inherits="AMS.Web.ConfigurarOperacion" %>
<HTML>
	<HEAD>
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="mainApp">
		<form runat="server">
			<p>
				Nombre de la tabla :
				<asp:Label id="nomTabla" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
				Nombre del Campo :
				<asp:Label id="nomCampo" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp; Tipo de 
				Dato :
				<asp:Label id="tipDato" runat="server"></asp:Label>
			</p>
			<p>
			</p>
			<fieldset style="WIDTH: 598px; HEIGHT: 60px">
            <legend>Configuración de la operación sobre el mismo campo</legend>Tipo de Operación
            : 
            <asp:DropDownList id="ddlOper1" runat="server"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp; 
            <asp:Button id="btnAgre1" onclick="Agregar_Operacion" runat="server" Text="Agregar Operación"></asp:Button>
        </fieldset>
			<p>
			</p>
			<fieldset style="WIDTH: 598px; HEIGHT: 60px">
				<p>
				</p>
				<legend>Configuración de relación con constantes u otros campos</legend>
				<table class="main">
					<tbody>
						<tr>
							<td>
								Operación de relación :
								<asp:DropDownList id="ddlOper2" runat="server"></asp:DropDownList>
							</td>
							<td>
								Objeto a relacionar :
							</td>
							<td>
								<asp:RadioButtonList id="opcionesRel" runat="server" RepeatDirection="Horizontal" BackColor="White">
									<asp:ListItem Value="C" Selected="True">Valor Constante</asp:ListItem>
									<asp:ListItem Value="V">Valor de Otro Campo</asp:ListItem>
								</asp:RadioButtonList>
							</td>
						</tr>
					</tbody>
				</table>
				<p>
					<asp:Button id="btnConf" onclick="Configurar_Opcion" runat="server" Text="Configurar Opción"></asp:Button>
				</p>
				<p>
					<asp:PlaceHolder id="confConst" runat="server" Visible="False">
						<P>Valor Constante :
							<asp:TextBox id="vlrConst" runat="server" Width="86px"></asp:TextBox>&nbsp;
							<asp:Button id="btnAcpt1" onclick="Aceptar_Conf1" runat="server" Text="Aceptar"></asp:Button>&nbsp;
							<asp:Button id="btnCncl1" onclick="Cancelar_Conf1" runat="server" Text="Cancelar"></asp:Button>&nbsp;
							<asp:CheckBox id="chkSpace" runat="server" Text="Agregar Como Un Espacio En Blanco" Visible="False"
								TextAlign="Left"></asp:CheckBox></P>
					</asp:PlaceHolder>
				<P></P>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
				<p>
					<asp:PlaceHolder id="confVar" runat="server" Visible="False">
						<P>Tablas :
							<asp:DropDownList id="ddlTablas" runat="server" OnSelectedIndexChanged="Cambio_Tabla" AutoPostBack="True"></asp:DropDownList></P>
						<P>Campos :
							<asp:DropDownList id="ddlCampos" runat="server"></asp:DropDownList></P>
						<P>Operaciones Sobre Campo :
							<asp:DropDownList id="ddlOper3" runat="server"></asp:DropDownList></P>
						<P>
							<asp:Button id="btnAcpt2" onclick="Aceptar_Conf2" runat="server" Text="Aceptar"></asp:Button>&nbsp;
							<asp:Button id="btnCncl2" onclick="Cancelar_Conf2" runat="server" Text="Cancelar"></asp:Button></P>
					</asp:PlaceHolder>
				<P></P>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
				<p>
				</p>
			</fieldset>
			<p>
				<asp:DataGrid id="dgOpers" runat="server" Width="508px" BackColor="White" AutoGenerateColumns="False"
					Font-Names="Verdana" BorderWidth="1px" GridLines="Vertical" BorderColor="#999999" CellPadding="3"
					Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd" BorderStyle="None">
					<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
					<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
					<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
					<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="VALOR">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="OPERACI&#211;N">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "OPERACION") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="OBJETO OPERADO">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "OBJETO") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="TABLA">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "TABLA") %>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid>
			</p>
			<p>
				<asp:Button id="btnConstruir" onclick="Construir_Valor" runat="server" Text="Construir Valor"></asp:Button>
				&nbsp;
				<asp:Button id="btnAcptF" runat="server" Text="Aceptar" Visible="False"></asp:Button>
				&nbsp;
				<asp:Button id="btnCnclF" runat="server" Text="Cancelar"></asp:Button>
			</p>
			<p>
				<asp:Label id="lb" runat="server"></asp:Label>&nbsp;
			</p>
		</form>
	</body>
</HTML>
