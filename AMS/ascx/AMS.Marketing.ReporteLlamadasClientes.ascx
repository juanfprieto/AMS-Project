<%@ Control Language="c#" codebehind="AMS.Marketing.ReporteLlamadasClientes.ascx.cs" autoeventwireup="True" Inherits="AMS.Marketing.ReporteLlamadasClientes" %>
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
    function toggle_div(id) {
        var diq = document.getElementById(id).style;
        diq.display=(diq.display=="none") ? "" : "none";
     }

</script>
<p>
	<table id="Table1" class="filtersIn">
		<tbody>
			<tr>
				<td>Escoja la fecha inicial
				</td>
				<td></td>
				<td>Escoja la fecha final
				</td>
			</tr>
			<tr>
				<td><asp:calendar BackColor=Beige id="fechaInicial" runat="server" SelectedDate="2005-11-20"></asp:calendar></td>
				<td>&nbsp;&nbsp;&nbsp;
				</td>
				<td><asp:calendar BackColor=Beige id="fechaFinal" runat="server" SelectedDate="2005-11-20"></asp:calendar></td>
			</tr>
			<tr>
				<td>Vendedor
				</td>
				<td>&nbsp;&nbsp;&nbsp;
				</td>
				<td><asp:dropdownlist id="ddlVendedor" runat="server" Visible="True"></asp:dropdownlist></td>
			</tr>
			<asp:PlaceHolder ID="plcClave" runat=server>
			<tr>
				<td>Clave</td>
				<td>&nbsp;&nbsp;&nbsp;
				</td>
				<td><asp:TextBox id="txtClave" Width="100px" runat="server" TextMode="Password"></asp:TextBox></td>
			</tr>
			</asp:PlaceHolder>
		</tbody></table>
</p>
<p><asp:button id="btnGenerar" onclick="btnGenerar_Click" runat="server" Text="Generar Reporte" OnClientClick="espera();"></asp:button></p>
<p><asp:placeholder id="toolsHolder" runat="server">
		<TABLE id="Table2" class="filtersIn">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail.jpg"
						alt="Enviar por email" BorderWidth="0px"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:placeholder></p>
<p><asp:placeholder id="phReporte" runat="server">
		<TABLE id="Table3" class="filtersIn">
			<TR>
				<TD>
					<CENTER>
						<asp:Label id="lbRep" runat="server" text="REPORTE DE SEGUIMIENTO DE ACTIVIDADES Y EVENTOS ESPECIALES DE LOS CLIENTES"
							visible="false"></asp:Label></CENTER>
				</TD>
			</TR>
			<TR>
				<TD></TD>
			</TR>
			<TR>
				<TD>
					<P><a href="javascript:toggle_div('div1');">
						<asp:Label id="lbTitulo" runat="server"></asp:Label></a></P>
				</TD>
			</TR>
			<TR>
				<TD>
    				<div id="div1">
					<asp:DataGrid id="dgReporte" runat="server" cssclass="datagrid" CellPadding="3"
						AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pagerr" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&Activ=PC&tipo=<%=Tipo%>&path=Accion Marketing'>
									    <%# DataBinder.Eval(Container.DataItem, "CC") %>
                                    </a>
                                </ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="PROFESION" HeaderText="Profesión del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Fecha" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD>
					<P>
						<a href="javascript:toggle_div('div2');"><asp:Label id="lbTitulo2" runat="server"></asp:Label></a></P>
				</TD>
			</TR>
			<TR>
				<TD><div id="div2">
					<asp:DataGrid id="dgCumple" runat="server" cssclass="dateagrid" CellPadding="3"
						AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&Activ=CC&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Fecha de Cumpleaños" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD>
					<P>
						<a href="javascript:toggle_div('div3');"><asp:Label id="lbTitulo3" runat="server"></asp:Label></a></P>
				</TD>
			</TR>
			<TR>
				<TD><div id="div3">
					<asp:DataGrid id="dgAniversario" runat="server" cssclass="datagrid" CellPadding="3"
						AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&Activ=AC&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Fecha del Aniversario" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD>
					<a href="javascript:toggle_div('div4');"><asp:Label id="lbTitulo4" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD><div id="div4">
					<asp:DataGrid id="dgAct" runat="server" cssclass="datagrid" CellPadding="3"
						AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="altermate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&Activ=AE&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="ACTIVIDAD" HeaderText="Actividad a Realizar" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Fecha" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div5');">
					<asp:Label id="lbTitulo0" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD></TD>
			</TR>
			<TR>
				<TD>
				    <div id="div5">
					<asp:DataGrid id="dgContacto0" runat="server" cssclass="datagrid" CellPadding="3"
						AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=C2S&CliContact=<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Entrega" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div6');">
					<asp:Label id="lbTitulo5" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD></TD>
			</TR>
			<TR>
				<TD><div id="div6">
					<asp:DataGrid id="dgContacto1" runat="server" CellPadding="3"
						AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=E5&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Entrega" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div7');">
					<asp:Label id="lbTitulo6" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD><div id="div7">
					<asp:DataGrid id="dgContacto2" runat="server" class="datagrid" CellPadding="3" AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=E30&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Entrega" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div8');">
					<asp:Label id="lbTitulo7" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD><div id="div8">
					<asp:DataGrid id="dgContacto3" runat="server" cssclass="datagrid" CellPadding="3" AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=E60&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Entrega" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div9');">
					<asp:Label id="lbTitulo8" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD><div id="div9">
					<asp:DataGrid id="dgContacto4" runat="server" cssclass="datagrid" CellPadding="3" AutoGenerateColumns="False" ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=EM6&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Entrega" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div10');">
					<asp:Label id="lbTitulo9" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD><div id="div10">
					<asp:DataGrid id="dgContacto5" runat="server" cssclass="datagrid" CellPadding="3" AutoGenerateColumns="False"
						ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=EA1&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Entrega" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div11');">
					<asp:Label id="lbTitulo10" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD><div id="div11">
					<asp:DataGrid id="dgMantenimiento" runat="server" cssclass="datagrid" CellPadding="3"  AutoGenerateColumns="False"
						ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
					    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=MP&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Ultimo Mantenimiento" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="kilos_hasta" HeaderText="Kilometraje Estimado" />
							<asp:BoundColumn DataField="KIT" HeaderText="Kit" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div12');">
					<asp:Label id="lblTitulo11" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD><div id="div12">
					<asp:DataGrid id="dgDocsVencen" runat="server" cssclass="datagrid" CellPadding="3" AutoGenerateColumns="False"
						ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=DV&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="DOCUMENTO" HeaderText="Documento" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Fecha Vence" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div13');">
					<asp:Label id="lblTitulo12" runat="server"></asp:Label></a></TD>
			</TR>
			<TR>
				<TD><div id="div13">
					<asp:DataGrid id="dgrCredsVencen" runat="server" cssclass="datagrid" CellPadding="3" AutoGenerateColumns="False"
						ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_NOMBRE") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=CV&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="FECHA" HeaderText="Fecha Vence" DataFormatString="{0:yyyy-MM-dd}" />
							<asp:BoundColumn DataField="MESES" HeaderText="Meses Crédito" />
							<asp:BoundColumn DataField="VALOR" HeaderText="Valor Crédito" />
							<asp:BoundColumn DataField="VEHI" HeaderText="Cat/Vin/Placa" />
							<asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Vendedor" />
							<asp:BoundColumn DataField="TELS" HeaderText="Telefonos" />
							<asp:BoundColumn DataField="EMAIL" HeaderText="email" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>
            <TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD><a href="javascript:toggle_div('div14');">
					<asp:Label id="lblTitulo13" runat="server"></asp:Label></a></TD>
			</TR>
            <TR>
				<TD><div id="div14">
					<asp:DataGrid id="dgrRecomendaciones" runat="server" cssclass="datagrid" CellPadding="3" AutoGenerateColumns="False"
						ShowFooter="False">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Nit o CC del Cliente" HeaderStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<a href='<%=indexPage%>?process=Marketing.AccionMarketing&idCli=<%# DataBinder.Eval(Container.DataItem, "CC") %>&idVen=<%# DataBinder.Eval(Container.DataItem, "PVEN_CODIGO") %>&vin=<%# DataBinder.Eval(Container.DataItem, "VIN") %>&Activ=CV&tipo=<%=Tipo%>&path=Accion Marketing'>
										<%# DataBinder.Eval(Container.DataItem, "CC") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Cliente" />
							<asp:BoundColumn DataField="ORDENTRABAJO" HeaderText="Orden Trabajo" />
                            <asp:BoundColumn DataField="FECHAOT" HeaderText="Fecha OT" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundColumn DataField="OPERACION" HeaderText="Operación" />
							<asp:BoundColumn DataField="OBSERVACIONES" HeaderText="Observaciones" />
						</Columns>
					</asp:DataGrid></div></TD>
			</TR>

			<TR>
				<TD>&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:placeholder></p>
<asp:Panel ID="pnlExcel" Runat="server" Visible="False">
	<TABLE>
		<TR>
			<TD>
				<asp:dropdownlist id="ddlTipoExcel" runat="server" Visible="True">
					<asp:ListItem Value="1">Profesiones</asp:ListItem>
					<asp:ListItem Value="2">Cumpleaños</asp:ListItem>
					<asp:ListItem Value="3">Aniversarios</asp:ListItem>
					<asp:ListItem Value="4">Actividades</asp:ListItem>
					<asp:ListItem Value="11">Contacto 2 dias</asp:ListItem>
					<asp:ListItem Value="5">Contacto 5 dias</asp:ListItem>
					<asp:ListItem Value="6">Contacto 30 dias</asp:ListItem>
					<asp:ListItem Value="7">Contacto 60 dias</asp:ListItem>
					<asp:ListItem Value="8">Contacto 6 meses</asp:ListItem>
					<asp:ListItem Value="9">Contacto 1 año</asp:ListItem>
					<asp:ListItem Value="10">Planes Mantenimiento</asp:ListItem>
					<asp:ListItem Value="12">Documentos a vencer</asp:ListItem>
					<asp:ListItem Value="13">Créditos a vencer</asp:ListItem>
                    <asp:ListItem Value="14">Recomendaciones</asp:ListItem>
				</asp:dropdownlist></TD>
			<TD>
				<asp:button id="btnGenerarExcel" onclick="btnGenerarExcel_Click" runat="server" Text="Descargar Excel"></asp:button></TD>
		</TR>
	</TABLE>
	<script language="JavaScript">
	    document.getElementById('div1').style.display = "none";
	    document.getElementById('div2').style.display = "none";
	    document.getElementById('div3').style.display = "none";
	    document.getElementById('div4').style.display = "none";
	    document.getElementById('div5').style.display = "none";
	    document.getElementById('div6').style.display = "none";
	    document.getElementById('div7').style.display = "none";
	    document.getElementById('div8').style.display = "none";
	    document.getElementById('div9').style.display = "none";
	    document.getElementById('div10').style.display = "none";
	    document.getElementById('div11').style.display = "none";
	    document.getElementById('div12').style.display = "none";
	    document.getElementById('div13').style.display = "none";
	    document.getElementById('div14').style.display = "none";
    </script>
</asp:Panel>
<p><asp:label id="lb" runat="server"></asp:label></p>
