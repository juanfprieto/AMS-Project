<%@ Control Language="c#" codebehind="AMS.Automotriz.VistaImpresion.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.VistaImpresion" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
		<title>Tree View Example</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie8" name="vs_targetSchema">

        <link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet"/>
        <link href="../style/AMS.css" type="text/css" rel="stylesheet"/>
        <link rel="stylesheet" href="../css/tabber.css" type="text/css" media="screen" />
        <link href="../css/AMS.Menu.css" type="text/css" rel="stylesheet" />

        <script type="text/javascript" src="../js/tabber.js"></script>
        <script type ="text/javascript" src="../js/AMS.Web.PrintBox.js"></script>
        <script type ="text/javascript">
            function Lista() {
                w = window.open('AMS.DBManager.Reporte.aspx');
            }
        </script>
</head>

<p></p>

    <body ms_positioning="GridLayout" style="padding: 0px; margin: 0px">
       <form id="frmTVExample" method="post">
            <table width="100%">
				<tr>
					<td valign="top">
                        <div class="tabber" id="mytab1">
                            <div class="tabbertab" title="Formato Preliquidación">
                            <asp:placeholder id="toolsHolder" runat="server">
                                <fieldset>
                                <table id="Table1" class="filtersIn">
		                            <tr>
			                            <th class="filterHead">
                                            <img height="40" src="../img/AMS.Flyers.Tools.png" width="40" border="0" />
                                        </th>
			                            <td>
                                            Imprimir en alta calidad 
                                            <a href="javascript: Lista()">
                                                <img height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" border="0" />
			                                </a>
			                            </td>
			                            <td>&nbsp; &nbsp;Enviar por correo
				                            <asp:TextBox id="tbEmail" runat="server"></asp:TextBox></td>
			                            <td>
				                            <asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
					                            ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
				                            <asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" ImageUrl="../img/AMS.Icon.Mail.jpg" alt="Enviar por email"></asp:ImageButton>
                                        </td>
		                            </tr>
	                            </table>
                                </fieldset>
                            </asp:placeholder>
                                <p></p>
                        <p>
	                        <table class="reports" id="Table1" bgcolor="white">
			                        <tr>
				                        <td>
					                        <p><asp:table id="tabHeaderEmpresa" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
							                        Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"></asp:table></p>
					                        <p><asp:table id="tabHeaderPropietario" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
							                        Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"></asp:table></p>
					                        <p><asp:table id="tabHeaderVehiculo" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
							                        Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"></asp:table></p>
				                        </td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <p><asp:table id="tabFooterDiscriminacionOperaciones" BorderWidth="0px" HorizontalAlign="Center"
							                        Font-Name="Verdana" Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White"
							                        CellPadding="1" CellSpacing="0"></asp:table></p>
					                        <p><asp:table id="tabFooterTotal" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
							                        Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"></asp:table></p>
				                        </td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <center><asp:table id="tituloOperaciones" runat="server"></asp:table></center>
				                        </td>
			                        </tr>
			                        <tr>
				                        <td><ASP:DATAGRID id="operaciones" runat="server" BorderWidth="2px" GridLines="None" BackColor="White"
						                        CellPadding="3" CellSpacing="1" BorderStyle="Ridge" BorderColor="White" Font-Size="8pt">
						                        <FooterStyle forecolor="Black" backcolor="#C6C3C6"></FooterStyle>
						                        <HeaderStyle font-bold="True"  forecolor="#E7E7FF" backcolor="#4A3C8C"></HeaderStyle>
						                        <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#C6C3C6"></PagerStyle>
						                        <AlternatingItemStyle backcolor="#F0F0F0"></AlternatingItemStyle>
						                        <ItemStyle forecolor="Black" backcolor="#DEDFDE"></ItemStyle>
					                        </ASP:DATAGRID></td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <center><asp:table id="tituloRepuestos" runat="server"></asp:table></center>
				                        </td>
			                        </tr>
			                        <tr>
				                        <td><ASP:DATAGRID id="repuestos" runat="server" BorderWidth="2px" GridLines="None" BackColor="White"
						                        CellPadding="3" CellSpacing="1" BorderStyle="Ridge" BorderColor="White" Font-Size="8pt">
						                        <FooterStyle forecolor="Black" backcolor="#C6C3C6"></FooterStyle>
						                        <HeaderStyle font-bold="True" forecolor="#E7E7FF" backcolor="#4A3C8C"></HeaderStyle>
						                        <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#C6C3C6"></PagerStyle>
						                        <AlternatingItemStyle backcolor="#F0F0F0"></AlternatingItemStyle>
						                        <ItemStyle forecolor="Black" backcolor="#DEDFDE"></ItemStyle>
					                        </ASP:DATAGRID></td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <center><asp:table id="tituloPeritaje" runat="server" Visible="false"></asp:table></center>
				                        </td>
			                        </tr>
			                        <tr>
				                        <td><asp:placeholder id="peritaje" runat="server" Visible="False"></asp:placeholder></td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <p><asp:table id="tabFooterTerminos" BorderWidth="0px" HorizontalAlign="Center" Font-Name="Verdana"
							                        Font-Size="8pt" Runat="server" GridLines="Both" BackColor="White" CellPadding="1" CellSpacing="0"></asp:table></p>
				                        </td>
			                        </tr>
	                        </table>
                        </p>
                            <center>
                                <asp:button id="cerrar" onclick="CerrarVentana" runat="server" Text="Cerrar" OnClientClick = "espera();"></asp:button>
                            </center>
                        <p>
                                <asp:label id="lb" runat="server"></asp:label>&nbsp;
                        </p>
                    </div>

                    <div class="tabbertab"  title="Evaluación">
                        <asp:placeholder id="tab2" runat="server">
                           
                                <table class="filtersIn">
                                    <tr>
                                        <td>
					                        <asp:Label id="opPeritaje" runat="server">
                                                <b>Peritaje</b>
                                            </asp:Label>
                                        </td>
				                        <%--<td>
					                        <asp:ImageButton id="opPeritaje" onclick="Cargar_Operaciones_Peritaje" runat="server" CausesValidation="False">
                                            </asp:ImageButton>
				                        </td>--%>
                                    </tr>
                                    <tr>
				                        <td colspan="2">
					                        <asp:PlaceHolder id="operacionesPeritaje" runat="server" Visible="False"></asp:PlaceHolder>
				                        </td>
			                        </tr>
                                </table>
                          
                        </asp:placeholder>
                    </div>

                <div class="tabbertab"  title="Registro Cita">
                    <asp:placeholder id="tab3" runat="server">
                            <table class="filtersIn">
		                        <tr>
			                        <td>Número</td>
			                        <td><asp:label id="lblNumero" Runat="server"></asp:label></td>
		                        </tr>
		                        <tr>
			                        <td>Cliente</td>
			                        <td><asp:label id="lblCliente" Runat="server"></asp:label></td>
		                        </tr>
		                        <tr>
			                        <td>Actividad</td>
			                        <td><asp:dropdownlist id="ddlActividad" runat="server"></asp:dropdownlist>
                                    </td>
		                        </tr>
		                        <tr>
			                        <td>Detalle</td>
			                        <td><asp:textbox id="txtDetalle" runat="server" Height="80px" Width="200px" MaxLength="400" TextMode="MultiLine"></asp:textbox></TD>
		                        </tr>
		                        <tr>
			                        <td>Fecha</td>
			                        <td><asp:label id="lblFecha" Runat="server"></asp:label></td>
		                        </tr>
		                        <tr>
			                        <td>Vendedor</td>
			                        <td><asp:label id="lblVendedor" Runat="server"></asp:label></td>
		                        </tr>
		                        <tr>
			                        <td>Resultado</td>
			                        <td><asp:dropdownlist id="ddlResultado" runat="server"></asp:dropdownlist></td>
		                        </tr>
		                        <tr>
			                        <td>Fecha Próxima Contacto </td>
			                        <td>
                                    <ew:calendarpopup id="txtFechaProx" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage"
				                    runat="server" Culture="Spanish (Colombia)" Nullable="True">
				                    <WeekdayStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="White"></WeekdayStyle>
				                    <MonthHeaderStyle Font-Size="X-Small" Font-Names="Arial" ForeColor="Black" BackColor="Silver"></MonthHeaderStyle>
				                    <OffMonthStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="#FF8080"></OffMonthStyle>
				                    <GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                    BackColor="White"></GoToTodayStyle>
				                    <TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                    BackColor="LightGoldenrodYellow"></TodayDayStyle>
				                    <DayHeaderStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightBlue"></DayHeaderStyle>
				                    <WeekendStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightGray"></WeekendStyle>
				                    <SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                    BackColor="Khaki"></SelectedDateStyle>
				                    <ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                    BackColor="White"></ClearDateStyle>
				                    <HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                    BackColor="White"></HolidayStyle>
				                    </ew:calendarpopup>
                                    </td>

		                        </tr>
		                        <tr>
			                        <td>Mercaderista</td>
			                        <td><asp:dropdownlist id="ddlMercadeista" runat="server"></asp:dropdownlist></td>
		                        </tr>
		                        <%--<tr>
			                        <td>Duración Mins.</td>
			                        <td><asp:textbox id="txtMinutos" onkeyup="NumericMaskE(this,event)" runat="server" Width="80px"></asp:textbox></td>
                                </tr>--%>
	                        </table>
                 
                        </P>
                            <P>
                                <asp:button id="btnGrabar" runat="server" Text="Grabar" onclick="btnGuardarRegistroC_Click"></asp:button>
                            </P>
                        </asp:placeholder>
                             </div>
                        </div>
                    </td>
				</tr>
			</table>
			<input id="Sistema" style="Z-INDEX: 1; LEFT: 280px; POSITION: absolute; TOP: 16px" type="hidden" runat="server" />
        </form>
    </body>
</html>


