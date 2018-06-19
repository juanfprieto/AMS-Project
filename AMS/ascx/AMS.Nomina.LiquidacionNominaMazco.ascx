<%@ Control Language="c#" codebehind="AMS.Nomina.LiquidacionNominaMazco.cs" autoeventwireup="false" Inherits="AMS.Nomina.LiquidacionNominaMazco" %>
<script language="JavaScript">
    function Lista() {
        w=window.open('ImpresionGrillaLiquidacionNomina.aspx');
    }
</script>
<h4>PRELIQUIDACION INFORMATIVA
</h4>
<p><asp:label id="titulo" runat="server">Ingrese los datos</asp:label><asp:label id="prueba" runat="server" width="122px" visible="False"></asp:label><asp:label id="lbdocref" runat="server"></asp:label><asp:label id="lbmas1" runat="server" visible="False"></asp:label><asp:label id="prueba2" runat="server"></asp:label></p>
<P>Nomina mazco</P>
<p></p>
<table>
	<tr>
		<td>
			<table style="WIDTH: 324px; HEIGHT: 122px">
				<tr>
					<td height="23"><asp:label id="Label1" runat="server" width="165px" height="21px">Periodo a procesar </asp:label></td>
					<td height="23"><asp:dropdownlist id="DDLQUIN" runat="server" Width="138px"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:label id="Label3" runat="server">Mes</asp:label></td>
					<td><asp:dropdownlist id="DDLMES" runat="server" Width="138px"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td height="16">&nbsp;<asp:label id="Label2" runat="server" width="32px">Año</asp:label>
					</td>
					<td height="15"><asp:dropdownlist id="DDLANO" runat="server" Width="137px"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:label id="Label4" runat="server">Tipo de pago</asp:label></td>
					<td><asp:label id="lbtipopago" runat="server" width="135px"></asp:label></td>
				</tr>
				<tr>
				</tr>
				<tr>
					<td><asp:button id="consulta" onclick="realizar_consulta" runat="server" Text="Enviar"></asp:button></td>
				</tr>
				<tr>
				</tr>
			</table>
		</td>
		<td>
			<p><asp:label id="Label5" runat="server" width="545px" height="59px">El proceso de liquidacion
                    de nómina, realiza los pagos de los empleados correspondientes al PERIODO VIGENTE,
                    antes de realizar este procedimiento debe haber ingresado las novedades que afecten
                    al periodo de pago.</asp:label></p>
		</td>
	</tr>
</table>
<p></p>
<p></p>
<p>
	<table style="WIDTH: 390px; HEIGHT: 41px" height="41" width="390">
		<tbody>
			<tr>
				<td>
					<p><asp:label id="Label8" runat="server" width="133px" DESIGNTIMEDRAGDROP="404">Nombre del Liquidador:</asp:label></p>
				</td>
				<td><asp:label id="lbliquidador" runat="server" width="172px"></asp:label></td>
			</tr>
		</tbody></table>
	<asp:placeholder id="toolsHolder" runat="server">
		<TABLE class="tools" width="780">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" alt="Enviar por email"
						ImageUrl="../img/AMS.Icon.Mail.jpg"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:placeholder>
</p>
<p><asp:placeholder id="phGrilla" runat="server">
		<P>
			<asp:DataGrid id="DataGrid2" runat="server" onEditCommand="editar" onDeleteCommand="eliminar"
				onCancelCommand="cancelar" onUpdateCommand="actualizar" onPageIndexChanged="Grid_Change" AllowPaging="true"
				AutoGenerateColumns="False">
				<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
				<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
				<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
				<Columns>
					<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="CONCEPTO" HeaderText="CONCEPTO" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCION"></asp:BoundColumn>
					<asp:BoundColumn DataField="CANT EVENTOS" HeaderText="CANTIDAD"></asp:BoundColumn>
					<asp:BoundColumn DataField="A PAGAR" HeaderText="A PAGAR" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="A DESCONTAR" HeaderText="A DESCONTAR" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="SALDO" HeaderText="SALDO" DataFormatString="{0:C}"></asp:BoundColumn>
				</Columns>
			</asp:DataGrid></P>
	</asp:placeholder>
<P></P>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<br>
<br>
<asp:label id="lberroresdetectados" runat="server" visible="False" font-size="Larger"></asp:label><br>
<br>
<asp:label id="lbsaldorojo" runat="server" width="286px" visible="False" height="29px" font-size="Larger">Listado
Empleados con Saldo en Rojo</asp:label><br>
<asp:datagrid id="DataGrid3" runat="server" AutoGenerateColumns="False">
	<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
	<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
	<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
	<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
	<Columns>
		<asp:BoundColumn DataField="EMPLEADO" HeaderText="EMPLEADO"></asp:BoundColumn>
		<asp:BoundColumn DataField="DEVENGADOS" HeaderText="DEVENGADOS" DataFormatString="{0:C}"></asp:BoundColumn>
		<asp:BoundColumn DataField="DESCUENTOS" HeaderText="DESCUENTOS" DataFormatString="{0:C}"></asp:BoundColumn>
		<asp:BoundColumn DataField="SALDO" HeaderText="SALDO" DataFormatString="{0:C}"></asp:BoundColumn>
	</Columns>
</asp:datagrid><br>
<asp:label id="lbperipago" runat="server" visible="False" font-size="Larger">Listado
Empleados Diferente Periodo de Pago al Seleccionado</asp:label>
<p></p>
<p></p>
<p></p>
<p><asp:datagrid id="DataGrid1" runat="server" Width="339px" onPageIndexChanged="Grid_Change2" AllowPaging="True"
		AutoGenerateColumns="False" ShowFooter="True" AllowCustomPaging="True" Height="141px">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<Columns>
			<asp:BoundColumn DataField="EMPLEADO" HeaderText="EMPLEADO"></asp:BoundColumn>
			<asp:BoundColumn DataField="PERIODO DE PAGO" HeaderText="PERIODO DE PAGO"></asp:BoundColumn>
		</Columns>
	</asp:datagrid></p>
<p></p>
<P>
	<table style="BORDER-RIGHT: 0px; BORDER-TOP: 0px; BORDER-LEFT: 0px; BORDER-BOTTOM: 0px; BACKGROUND-COLOR: white"
		height="68">
		<tbody>
			<tr>
				<td align="center"><asp:button id="BTNCONFIRMACION" onclick="realizar_liquidacion" runat="server" Text="LIQUIDAR DEFINITIVAMENTE"
						Visible="False"></asp:button><asp:button id="btnGenInforme" onclick="generarInforme" runat="server" visible="False" Width="103px"
						Text="Generar Informe"></asp:button><asp:hyperlink id="hl" runat="server" Visible="False">Bajar Informe</asp:hyperlink></td>
			</tr>
		</tbody></table>
</P>
<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</P>
<P>
	<asp:DataGrid id="dgprueba" runat="server"></asp:DataGrid></P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<p>&nbsp;</p>
<p><asp:label id="lb" runat="server" visible="False"></asp:label><asp:label id="lb2" runat="server" visible="False"></asp:label><asp:label id="lbpag" runat="server" visible="False"></asp:label><asp:label id="lbpag2" runat="server"></asp:label><asp:label id="lb3" runat="server" width="59px" visible="False"></asp:label></p>
