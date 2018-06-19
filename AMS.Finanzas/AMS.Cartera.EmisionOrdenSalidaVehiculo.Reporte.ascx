<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.EmisionOrdenSalidaVehiculo.Reporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.ReporteOrdenSalida" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ='text/javascript'>
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<p>
<asp:placeholder id="toolsHolder" runat="server">
		<table id="Table1" class="filtersIn">
			<TR>
				<th class="filterHead">
                <IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0">
                </th>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail1.png"
						alt="Enviar por email" BorderWidth="0px"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:placeholder></p>
<p><asp:placeholder id="phInfo" runat="server">
		<TABLE style="WIDTH: 650px; BACKGROUND-COLOR: white">
			<TR>
				<TD align="center"><asp:Image id="ImgLogo" runat="server"/> <!--<IMG src="../img/AMS.LogoOrdenSalida.jpg"></TD>-->
				<TD align="center">
					<P style="FONT-SIZE: 15pt">ORDEN DE SALIDA DE VEHICULO</P>
				</TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2">Información del Propietario y el Vehículo
				</TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2"><asp:datagrid id="dgOrdSal" runat="server" cssclass="datagrid" ShowFooter="False" AutoGenerateColumns="False" 
						CellPadding="3">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="CC" HeaderText="Nit o CC del Propietario" />
							<asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre del Propietario" />
							<asp:BoundColumn DataField="CATALOGO" HeaderText="Catalogo del Vehículo" />
							<asp:BoundColumn DataField="VIN"   HeaderText="VIN del Vehículo" />
							<asp:BoundColumn DataField="PLACA" HeaderText="Placa del Vehículo" />
							<asp:BoundColumn DataField="MOTOR" HeaderText="Número de Motor del Vehículo" />
							<asp:BoundColumn DataField="COLOR" HeaderText="Color del Vehículo" />
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2"><asp:label id="lbInfo" runat="server" Font-Size="15"></asp:label></TD>
			</TR>
			<tr>
				<td align="center" colspan="2">
					<asp:Label ID="lbCond" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
				</td>
			</tr>
			<TR>
				<TD align="center" colSpan="2">Facturas Pendientes de Pago
				</TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2"><asp:datagrid id="dgFacturas" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="PREFIJO" HeaderText="Prefijo de la Factura"></asp:BoundColumn>
							<asp:BoundColumn DataField="NUMERO" HeaderText="N&#250;mero de la Factura"></asp:BoundColumn>
							<asp:BoundColumn DataField="VALOR" HeaderText="Valor Faltante de la Factura" DataFormatString="{0:C}"></asp:BoundColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<TD vAlign="bottom">
					<P>&nbsp;</P>
					<P>&nbsp;</P>
					<P>VoBo___________________
					</P>
				</TD>
				<TD vAlign="bottom"><asp:label id="lbProc" Runat="server"></asp:label></TD>
			</TR>
		</TABLE>
	</asp:placeholder>
<p><asp:label id="lb" runat="server"></asp:label></p>
