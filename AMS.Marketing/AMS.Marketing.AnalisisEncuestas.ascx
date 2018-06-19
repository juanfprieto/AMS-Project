<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Marketing.AnalisisEncuestas.ascx.cs" Inherits="AMS.Marketing.AnalisisEncuestas" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<%@ Register TagPrefix="web" Namespace="WebChart" Assembly="WebChart" %>

<asp:PlaceHolder id="plcSeleccion" runat="server" Visible="True">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET>
                <LEGEND class="Legends">Analisis de Encuestas</LEGEND>
					<TABLE id="Table1" class="filtersIn">
						<TR>
							<TD>
								<TABLE class="main" cellSpacing="10">
									<TR>
										<TD>
											<asp:Label id="Label1" runat="server" Font-Bold="True" CssClass="Legends">Seleccione una Encuesta : </asp:Label></TD>
										<TD>
											<asp:dropdownlist id="ddlEncuesta" runat="server"></asp:dropdownlist></TD>
									</TR>
								</TABLE>
							</TD>
							<TD>
								<TABLE class="main" cellSpacing="10">
									<TR>
										<TD>
											<asp:Label id="Label2" runat="server" Font-Bold="True" CssClass="Legends">Fecha de Inicio : </asp:Label></TD>
										<TD>
                                            <asp:TextBox id="txtFechaInicio" onkeyup="DateMask(this)" runat="server" Width="90px"></asp:TextBox>
                                            <asp:RegularExpressionValidator id="validatorFecha1" runat="server" ErrorMessage="RegularExpressionValidator" ControlToValidate="txtFechaInicio" Text="*" ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:RegularExpressionValidator>
											</TD>
									</TR>
									<TR>
										<TD>
											<asp:Label id="Label23" runat="server" Font-Bold="True" CssClass="Legends">Fecha de Finalizacion : </asp:Label></TD>
										<TD>
                                            <asp:TextBox id="txtFechaFin" onkeyup="DateMask(this)" runat="server" Width="90px"></asp:TextBox>
											</TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
						<TR>
							<TD align="right" colSpan="2">
								<asp:Button id="btnCargar" runat="server" Text="Cargar Resultados" Width="200px" onclick="btnCargar_Click"></asp:Button></TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
</asp:PlaceHolder>

<asp:PlaceHolder id="plcResultadosEnc" runat="server" Visible="False">
	<TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Información de la Encuesta</LEGEND>
					<TABLE id="Table2" class="filtersIn">
						<TR>
							<TD>
								<TABLE class="main" cellSpacing="10">
									<TR>
										<TD>
											<asp:Label id="Label3" runat="server" Font-Bold="True" CssClass="Legends">Encuesta: </asp:Label></TD>
										<TD>
											<asp:Label id="lblEncuesta" runat="server" Font-Bold="True"></asp:Label></TD>
										<TD>
											<asp:Label id="Label16" runat="server" Font-Bold="True" CssClass="Legends">Fecha de Creación: </asp:Label></TD>
										<TD>
											<asp:Label id="lblFechCreacion" runat="server" Font-Bold="True"></asp:Label></TD>
                                    </TR>
                                    <TR>
                                        <TD>
											<asp:Label id="Label4" runat="server" Font-Bold="True" CssClass="Legends">Objetivo de la Encuesta: </asp:Label></TD>
										<TD>
											<asp:Label id="lblObjetivoEnc" runat="server" Font-Bold="True"></asp:Label></TD>
                                    </TR>
                                    <TR>
                                        <TD>
											<asp:Label id="Label5" runat="server" Font-Bold="True" CssClass="Legends">Responsable: </asp:Label></TD>
										<TD>
											<asp:Label id="lblResponsable" runat="server" Font-Bold="True"></asp:Label></TD>
                                    </TR>
                                    <TR>
                                        <TD>
											<asp:Label id="Label6" runat="server" Font-Bold="True" CssClass="Legends">Cantidad de Encuestas Evaluadas: </asp:Label></TD>
										<TD>
											<asp:Label id="lblCantEncuesta" runat="server" Font-Bold="True"></asp:Label></TD>
                                    </TR>
                                    <TR>
                                         <TD>
											<asp:Label id="Label7" runat="server" Font-Bold="True" CssClass="Legends">Rango fecha de inicio: </asp:Label></TD>
										<TD>
											<asp:Label id="lblFechaIni" runat="server" Font-Bold="True"></asp:Label></TD>
                                        <TD>
											<asp:Label id="Label13" runat="server" Font-Bold="True" CssClass="Legends">Rango fecha de fin: </asp:Label></TD>
										<TD>
											<asp:Label id="lblFechafin" runat="server" Font-Bold="True"></asp:Label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>

    <TABLE>
		<TR>
			<TD>
				<FIELDSET><LEGEND class="Legends">Preguntas de la Encuesta</LEGEND>
					<asp:Repeater id="rptPreguntas" runat="server" OnItemDataBound="rptPreguntas_Bound" OnItemCommand="Graph">
						<ItemTemplate>
							<BR><BR>
                            <table cellspacing="0" cellpadding="3" bordercolor="#999999" border="1" style="background-color:White;border-color:#999999;border-width:1px;border-style:None;font-family:Verdana;font-size:8pt;width:700px;border-collapse:collapse;">
								<tr style="color:White;background-color:#000084;font-weight:bold;">
									<td width="2%"><%#DataBinder.Eval(Container.DataItem,"numero") %></td>
                                    <td width="4%"><asp:Label id="lblPreg" runat="server" Font-Bold="True" CssClass="Legends" Text=<%#DataBinder.Eval(Container.DataItem,"codigo") %>></asp:Label></td>
									<td width="75%"><%#DataBinder.Eval(Container.DataItem,"pregunta")%></td>
                                    <TD width="5%">Graficar </td>
                                    <td width="10%"><asp:DropDownList ID="ddlTipoGrafica" runat="server">
					                    <asp:ListItem Selected="True" Value="B">Barra</asp:ListItem>
					                    <asp:ListItem Value="L">Linea</asp:ListItem>
                                        <asp:ListItem Value="P">Pastel</asp:ListItem>
				                        </asp:DropDownList>
				                    </td>
                                    <td width="4%"><asp:ImageButton id="ibGrafica" CommandName="btnGrafica" runat="server" alt="Graficar" ImageUrl="../img/AMS.Icon.Graph.png"
						                    BorderWidth="0px"></asp:ImageButton></TD>
								</tr>
								<tr>
									<td colspan="7">
										<asp:DataGrid id="dgrRespuestas" runat="server" Width="730px" AutoGenerateColumns="false" Font-Names="Verdana"
											BorderWidth="1px" GridLines="Vertical" BorderStyle="None" BackColor="White" BorderColor="#999999"
											CellPadding="3" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd">
											<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
											<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
											<HeaderStyle font-bold="True" forecolor="White" backcolor="#6060E4"></HeaderStyle>
											<Columns>
											    <asp:TemplateColumn HeaderText="Descripcion">
													<HeaderStyle Width="80%"></HeaderStyle>
													<ItemTemplate> 
														<%# DataBinder.Eval(Container.DataItem, "PRES_DESCRESP") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Porcentaje">
													<HeaderStyle Width="10%"></HeaderStyle>
													<ItemTemplate> 
														<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE") %>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Votos">
													<HeaderStyle Width="10%"></HeaderStyle>
													<ItemTemplate>
														<%# DataBinder.Eval(Container.DataItem, "RESPUESTAS") %>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:DataGrid>
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:Repeater>
				</FIELDSET>
			</TD>
		</TR>
	</TABLE>
</asp:PlaceHolder>

<asp:placeholder id="plcGrafica" runat="server" visible="false">
	<P align=center>
		<asp:TextBox id="txtAncho" runat="server" Text="800" Columns=3 MaxLength=4></asp:TextBox>X<asp:TextBox id="txtAlto" runat="server" Text="600" Columns=3 MaxLength=4></asp:TextBox><br><br>
		<Web:ChartControl id="chtGrafica" runat="server" BorderWidth="5px" BorderStyle="Outset" Width="8000px" Height="400px" Visible=false>
			<XTitle StringFormat="Center,Near,Character,LineLimit"></XTitle>
			<YAxisFont StringFormat="Far,Near,Character,LineLimit"></YAxisFont>
			<ChartTitle StringFormat="Center,Near,Character,LineLimit"></ChartTitle>
			<XAxisFont StringFormat="Center,Near,Character,LineLimit"></XAxisFont>
			<Background Color="LightSteelBlue"></Background>
			<YTitle StringFormat="Center,Near,Character,LineLimit"></YTitle>
		</Web:ChartControl>
		<br>
		<asp:Button ID="btnVolver" runat="server" Text="Volver" onclick="btnVolver_Click"/>
	</P>
</asp:placeholder>








