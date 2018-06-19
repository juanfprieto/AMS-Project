<%@ Control Language="c#" codebehind="AMS.Automotriz.ProgramacionCitas.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.ProgramacionCitas" %>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
			   <IMG height="80" src="../img/AMS.Flyers.Consultar.png" border="0">
			</th>
			<td>
				<p>
                <table id="Table" class="filtersIn">
                <tr>
                <td>
                Taller Nº :
					<asp:DropDownList id="taller" class="dmediano" runat="server"></asp:DropDownList>
					<br /> Ordenar Por :&nbsp;
					<asp:DropDownList id="tipoOrden" class="dmediano" runat="server"></asp:DropDownList>
				<br />
					Ordenar de Forma :
					<asp:DropDownList id="formaOrden" class="dpequeno" runat="server">
						<asp:ListItem Value="ASC" Selected="True">
                        Ascendente</asp:ListItem>
						<asp:ListItem Value="DESC">
                        Descendente</asp:ListItem>
					</asp:DropDownList>
				<br />
					
							<tr>
								<td>
									<center>Fecha Inicial
									</center>
								</td>
								<td>
									<center>Fecha Final
									</center>
								</td>
							</tr>
							<tr>
								<td>
									<asp:calendar BackColor=Beige id="fechaInicial" runat="server"></asp:Calendar>
								</td>
								<td>
									<asp:calendar BackColor=Beige id="fechaFinal" runat="server"></asp:Calendar>
								</td>
							</tr>
                            </td>
                </tr>
					</table>
				</p>
				<p>
					<asp:Button id="consultar" onclick="Realizar_Consulta" Width="126px" runat="server" Text="Consultar"></asp:Button>
				</p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
			   <IMG height="80" src="../img/AMS.Flyers.ConsultaAvanzada.png" border="0">
			</th>
			<td>
				<p>
					<asp:PlaceHolder id="controlesConsulta" runat="server"></asp:PlaceHolder>
				</p>
				<p>
					<asp:Button id="consultaAvanzada" onclick="Realizar_Consulta_Avanzada" Width="163px" runat="server"
						Text="Consulta Avanzada"></asp:Button>
				</p>
			</td>
		</tr>
	</tbody>
</table>
</fieldset>
<p>
</p>
<p>
	<asp:PlaceHolder id="toolsHolder" runat="server" visible="false">
		<TABLE class="tools">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:ImageButton id="ibMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"
						BorderWidth="0px"></asp:ImageButton>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:PlaceHolder>
</p>
<p>
	<asp:DataGrid id="resultadoConsulta" runat="server" BorderWidth="1px" AutoGenerateColumns="false"
		Font-Names="Verdana" GridLines="Vertical" BorderStyle="None" BackColor="White" BorderColor="#999999"
		CellPadding="3" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="FECHA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HORA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="RECEPCIONISTA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "RECEPCIONISTA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="CATALOGO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CATALOGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="PLACA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NOMBRE CLIENTE">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMBCLIENTE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="TELEFONO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TELEFONO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="MOVIL">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "MOVIL") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="E-MAIL">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CORREO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="SERVICIO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "SERVICIO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ESTADO CITA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ESTCITA") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
