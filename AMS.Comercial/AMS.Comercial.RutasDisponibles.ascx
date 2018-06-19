<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RutasDisponibles.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RutasDisponibles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P><asp:label id="RemeasLabel" runat="server" Font-Bold="True" Font-Size="Small">Rutas disponibles</asp:label></P>
<P>
	<HR style="WIDTH: 148.66%" width="148.66%" color="#000099" SIZE="4">
<P></P>
<FIELDSET style="WIDTH: 552px; HEIGHT: 80px"><LEGEND>Datos Rutas</LEGEND>
	<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
		<TR>
			<TD><asp:label id="Label1" runat="server">Fecha:</asp:label></TD>
			<TD><asp:textbox id="fecha" runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD><asp:button id="Buscar" onclick="generar" runat="server" Text="Buscar"></asp:button></TD>
			<TD></TD>
		</TR>
	</TABLE>
</FIELDSET>
<P>
	<HR style="WIDTH: 148.66%" width="148.66%" color="#000099" SIZE="4">
	<asp:panel id="Panel1" runat="server">
		<FIELDSET style="WIDTH: 786px; HEIGHT: 318px">
			<P><LEGEND>Vehiculos</LEGEND></P>
			<P>
				<TABLE id="Table2" style="WIDTH: 780px; HEIGHT: 57px" cellSpacing="0" cellPadding="0" width="780"
					border="0">
					<TR>
						<TD style="HEIGHT: 29px">
							<asp:Image id="Image1" runat="server" Height="40px" Width="140px" ImageUrl="../img/busEstado3.jpg"
								ImageAlign="Top"></asp:Image></TD>
						<TD style="HEIGHT: 29px">
							<asp:Image id="Image2" runat="server" Height="40px" Width="140px" ImageUrl="../img/busEstado2.jpg"
								ImageAlign="Top"></asp:Image></TD>
						<TD style="HEIGHT: 29px">
							<asp:Image id="Image3" runat="server" Height="40px" Width="140px" ImageUrl="../img/busEstado1.jpg"
								ImageAlign="Top"></asp:Image></TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="Label2" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Vehiculo a Salir en 1 hora o Mas</asp:Label></TD>
						<TD>
							<asp:Label id="Label3" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Vehiculo a salir en 30 Minutos o menos</asp:Label></TD>
						<TD>
							<asp:Label id="Label4" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Black">Vehiculo en ruta</asp:Label></TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="Label8" Font-Size="XX-Small" Font-Bold="True" runat="server">//</asp:Label>
							<asp:Label id="hora1" Font-Size="XX-Small" Font-Bold="True" runat="server" ForeColor="Red">Hora</asp:Label></TD>
						<TD></TD>
						<TD></TD>
					</TR>
				</TABLE>
			</P>
			<asp:datagrid id="Grid" runat="server" Height="144px" Width="780px" AutoGenerateColumns="False"
				HorizontalAlign="Center">
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
				<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
				<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
				<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="FECHA">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="PLACA">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="NUMERO">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="ESTADO">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Image id="IMAGEN" runat="server"></asp:Image>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="HORA SALIDA">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "HORASAL") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="RUTA">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "RUTA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="PUESTOS DISPONIBLES">
						<ItemStyle HorizontalAlign="center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "PUESTOS") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="TIPO DE RUTA">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Image id="TIPO" runat="server"></asp:Image>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
			</asp:datagrid></FIELDSET>
	</asp:panel><asp:panel id="Panel2" runat="server" Width="782px" Height="46px" Visible="False">
		<P>
			<asp:Label id="Label5" Font-Size="Medium" Font-Bold="True" runat="server" ForeColor="Black">No Hay Vehiculos Programados Para HOY</asp:Label></P>
		<P>
			<asp:Label id="FechaProceso" Font-Size="Medium" Font-Bold="True" runat="server" ForeColor="Red">Fecha</asp:Label>&nbsp;
			<asp:Label id="Label6" Font-Size="Medium" Font-Bold="True" runat="server" ForeColor="Black">/</asp:Label>
			<asp:Label id="horalabel" Font-Size="Medium" Font-Bold="True" runat="server" ForeColor="Red">Hora</asp:Label></P>
	</asp:panel>
