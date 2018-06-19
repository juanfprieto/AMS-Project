<%@ Control Language="c#" codebehind="AMS.Marketing.ReporteSeguimientoOrdenes.ascx.cs" autoeventwireup="True" Inherits="AMS.Marketing.ReporteSeguimientoOrdenes" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<link href="../style/AMS.css" type="text/css" rel="stylesheet">
		<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
		</script>
        <fieldset>
		<p>
			En este proceso vamos a generar un reporte que nos permita mirar las acciones 
			de marketing ejecutadas para un grupo de ordenes de trabajo comprendidas dentro 
			de un rango de fechas. Recuerde que la Fecha Inicial debe ser estrictamente 
			menor que la fecha Final
		</p>
		<p>
			Tipo de Acciones de Marketing :
			<asp:DropDownList id="tipoAcciones" class="dmediano" runat="server"></asp:DropDownList>
		</p>
		<p>
			Ordenadas Por :
			<asp:DropDownList id="tipoOrden" class="dmediano" runat="server">
				<asp:ListItem Value="orden">Orden de Trabajo</asp:ListItem>
				<asp:ListItem Value="actividad" Selected="True">Actividad de Marketing</asp:ListItem>
			</asp:DropDownList>
		</p>
		<p>
			<table id="Table" class="FiltersIn">
				<tbody>
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
				</tbody>
			</table>
		</p>
		<p>
			<asp:PlaceHolder id="toolsHolder" runat="server" visible="false">
				<TABLE class="tools" width="780">
					<TR>
						<th class="filterHead">
				<img height="30" src="../img/AMS.Flyers.Tools.png" border="0">
			</th></TD>
						<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</TD>
						<TD>&nbsp; &nbsp;Enviar por correo
							<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
						<TD>
							<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"
								BorderWidth="0px"></asp:ImageButton>
							<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
								ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></TD>
						<TD width="380"></TD>
					</TR>
				</TABLE>
			</asp:PlaceHolder>
		</p>
		<p>
			<asp:Button id="generar" onclick="Generar_Reporte" Width="178px" runat="server" Text="Generar Reporte"></asp:Button>
		</p>
		<p>
			<asp:PlaceHolder id="resultadoReporte" runat="server" EnableViewState="False"></asp:PlaceHolder>
		</p>
		<p>
			<asp:Label id="lb" runat="server"></asp:Label>
		</p>
        </fieldset>
<!-- Insert content here -->
