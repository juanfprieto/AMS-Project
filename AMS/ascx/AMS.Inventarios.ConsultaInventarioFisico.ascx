<%@ Register TagPrefix="web" Namespace="WebChart" Assembly="WebChart" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.ConsultaInventarioFisico.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_ConsultaInventarioFisico" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<LINK href="../css/tab.webfx.css" type="text/css" rel="StyleSheet">
<script language="javascript" src="../js/tabpane.js" type="text/javascript"></script>
<script language="javascript">
	function ValSelInvFis()
	{
		if(document.getElementById('<%=ddlInventarios.ClientID%>').value == '')
		{
			alert('Seleccione un inventario físico para mirar los detalles!');
			return false;
		}
	}
	
	function ConvertirCantidad(cantidad)
	{
		var cantidadCadena = "";

		if (cantidad != "-1")
			cantidadCadena = cantidad;

		return cantidadCadena;
	}

	function ConsultarInformacionTarjeta()
	{
		var arrInventarioFisico = document.getElementById('<%=ddlInventarios.ClientID%>').value.split('-');
		var regexEntero = /^(?:\+|-)?\d+$/;
		
		if(!regexEntero.test(document.getElementById('<%=tbNumeroTarjeta.ClientID%>').value))
		{
			alert('Número de tarjeta invalido.');
			document.getElementById('<%=tbNumeroTarjeta.ClientID%>').value = '';
			return;
		}
		
		var numeroTarjeta = parseInt(document.getElementById('<%=tbNumeroTarjeta.ClientID%>').value);
		var arrayValoresTarjeta = AMS_Inventarios_ConsultaInventarioFisico.TraerInformacionTarjeta(arrInventarioFisico[0],parseInt(arrInventarioFisico[1]),numeroTarjeta).value.split('&');
		
		if(arrayValoresTarjeta <= 1)
		{
			alert('Número de tarjeta invalido');
			
			document.getElementById('<%=tbNumeroTarjeta.ClientID%>').value = '';
			document.getElementById('<%=hdNumeroTarjeta.ClientID%>').value = '';
			document.getElementById('<%=lbCodigoReferencia.ClientID%>').innerText = '';
			document.getElementById('<%=lbNombreReferencia.ClientID%>').innerText = '';
			document.getElementById('<%=lbAlmacen.ClientID%>').innerText = '';
			document.getElementById('<%=lbUbicacion.ClientID%>').innerText = '';
			document.getElementById('<%=lbConteoActual.ClientID%>').innerText = '';
			document.getElementById('<%=lbCantidadConteo1.ClientID%>').innerText = '';
			document.getElementById('<%=lbCantidadConteo2.ClientID%>').innerText = '';
			document.getElementById('<%=lbCantidadConteo3.ClientID%>').innerText = '';
		}
		else
		{
			document.getElementById('<%=hdNumeroTarjeta.ClientID%>').value = arrayValoresTarjeta[0];
			document.getElementById('<%=lbCodigoReferencia.ClientID%>').innerText = arrayValoresTarjeta[1];
			document.getElementById('<%=lbNombreReferencia.ClientID%>').innerText = arrayValoresTarjeta[2];
			document.getElementById('<%=lbAlmacen.ClientID%>').innerText = innerText[3];
			document.getElementById('<%=lbUbicacion.ClientID%>').innerText = arrayValoresTarjeta[4];
			document.getElementById('<%=lbConteoActual.ClientID%>').innerText = String(parseInt(arrayValoresTarjeta[5]) + 1);
			document.getElementById('<%=lbCantidadConteo1.ClientID%>').innerText = ConvertirCantidad(arrayValoresTarjeta[6]);
			document.getElementById('<%=lbCantidadConteo2.ClientID%>').innerText = ConvertirCantidad(arrayValoresTarjeta[7]);
			document.getElementById('<%=lbCantidadConteo3.ClientID%>').innerText = ConvertirCantidad(arrayValoresTarjeta[8]);
		}
	}
</script>

<FIELDSET>
<LEGEND>Inventario Físico</LEGEND>
	<table id="Table1" class="filtersIn">
    <tbody>
		<TR>
			<TD>Seleccione el Inventario:<br />
		<asp:dropdownlist id="ddlInventarios" class="dmediano" runat="server"></asp:dropdownlist>
        </TD>
		</TR>
		<TR>
			<TD colSpan="2"><asp:button id="btnAceptar" runat="server" CausesValidation="False" Text="Cargar Información" onclick="btnAceptar_Click">
			</asp:button>&nbsp;
				<asp:button id="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" onclick="btnCancelar_Click"></asp:button></TD>
		</TR>
	
<table>
<asp:panel id="pnlInfoProceso" Runat="server" Visible="False">
	<DIV class="tab-pane" id="tab-pane-1">
		<DIV class="tab-page" align="center">
			<H2 class="tab">Información General Inventario Físico</H2>
			<TABLE class="tablewhite" cellSpacing="3" cellPadding="3" width="100%" border="0">
				<TR>
					<TD width="45%">Prefijo de Inventario Físico : </TD>
					<TD align="right"><asp:Label id="lbPrefijoInventario" runat="server"></asp:Label></TD></TR>
				<TR>
					<TD width="45%">Número de Inventario Físico :</TD>
					<TD align="right"><asp:Label id="lbNumeroInventario" runat="server"></asp:Label></TD></TR>
				<TR>
					<TD width="45%">Fecha Inicio Inventario Físico :</TD>
					<TD align="right"><asp:Label id="lbFechaInicioInventario" runat="server"></asp:Label></TD></TR>
				<TR>
					<TD width="45%">Fecha Cierre Inventario Físico :</TD>
					<TD align="right"><asp:Label id="lbFechaFinInventario" runat="server"></asp:Label></TD></TR>
				<TR>
					<TD width="45%">Tipo de Inventario Físico :</TD>
					<TD align="right"><asp:Label id="lbTipoInventario" runat="server"></asp:Label></TD></TR>
				<TR>
					<TD width="45%">Almacén Relacionados a Inventario Físico :</TD>
					<TD align="right"><asp:Label id="lbAlmacenRelacionado" runat="server"></asp:Label></TD></TR></TABLE></DIV>
		<DIV class="tab-page" align="center">
			<H2 class="tab">Graficos Estadisticos</H2>
			<TABLE class="tablewhite" cellSpacing="3" cellPadding="3" width="100%" border="0">
				<TR>
					<TD width="90%"><Web:ChartControl id="chtEstadisticasInv" runat="server" BorderWidth="5px" BorderStyle="Outset" Width="721px" Height="348px">
							<XTitle StringFormat="Center,Near,Character,LineLimit"></XTitle>
							<YAxisFont StringFormat="Far,Near,Character,LineLimit"></YAxisFont>
							<ChartTitle StringFormat="Center,Near,Character,LineLimit"></ChartTitle>
							<XAxisFont StringFormat="Center,Near,Character,LineLimit"></XAxisFont>
							<Background Color="LightSteelBlue"></Background>
							<YTitle StringFormat="Center,Near,Character,LineLimit"></YTitle>
						</Web:ChartControl></TD>
					<TD align="right"></TD></TR></TABLE></DIV>
		<DIV class="tab-page" align="center">
			<H2 class="tab">Consulta por Tarjeta</H2>
			<FIELDSET style="WIDTH: 100%"><LEGEND>Información Tajeta de Conteo</LEGEND>
				<TABLE class="tablewhite" cellSpacing="3" cellPadding="3" width="100%" border="0">
					<TR>
						<TD><asp:Label id="lbTxNumTj" runat="server">Número de Tarjeta :</asp:Label></TD>
						<TD align="right"><asp:TextBox id="tbNumeroTarjeta" runat="server" CssClass="AlineacionDerecha"></asp:TextBox><INPUT id="hdNumeroTarjeta" type="hidden" name="hdNumeroTarjeta" runat="server"> <INPUT id="btnCargarInfo" onclick="ConsultarInformacionTarjeta();" type="button" value="Mostrar Información Tarjeta" name="btnCargarInfo" class="noEspera">
						</TD></TR>
					<TR>
						<TD><asp:Label id="lbTxCodRef" runat="server">Código de Referencia :</asp:Label></TD>
						<TD align="right"><asp:Label id="lbCodigoReferencia" runat="server"></asp:Label></TD></TR>
					<TR>
						<TD><asp:Label id="lbTxNomRef" runat="server">Nombre de Referencia :</asp:Label></TD>
						<TD align="right"><asp:Label id="lbNombreReferencia" runat="server"></asp:Label></TD></TR>
					<TR>
						<TD><asp:Label id="lbTxAlm" runat="server">Almacén de la Ubicación :</asp:Label></TD>
						<TD align="right"><asp:Label id="lbAlmacen" runat="server"></asp:Label></TD></TR>
					<TR>
						<TD><asp:Label id="lbTxtUbi" runat="server">Ubicación :</asp:Label></TD>
						<TD align="right"><asp:Label id="lbUbicacion" runat="server"></asp:Label></TD></TR>
					<TR>
						<TD><asp:Label id="lbTxtContAct" runat="server">Conteo Actual</asp:Label></TD>
						<TD align="right"><asp:Label id="lbConteoActual" runat="server"></asp:Label></TD></TR>
					<TR>
						<TD><asp:Label id="lbTxtCont1" runat="server">Cantidad Ingresada en Conteo 1 :</asp:Label></TD>
						<TD align="right"><asp:Label id="lbCantidadConteo1" runat="server"></asp:Label></TD></TR>
					<TR>
						<TD><asp:Label id="lbTxtCont2" runat="server">Cantidad Ingresada en Conteo 2 :</asp:Label></TD>
						<TD align="right"><asp:Label id="lbCantidadConteo2" runat="server"></asp:Label></TD></TR>
					<TR>
						<TD><asp:Label id="lbTxtCont3" runat="server">Cantidad Ingresada en Conteo 3 :</asp:Label></TD>
						<TD align="right"><asp:Label id="lbCantidadConteo3" runat="server"></asp:Label></TD></TR></TABLE></FIELDSET>
		</DIV>
		<DIV class="tab-page" align="center">
			<H2 class="tab">Consulta por Conteo</H2>

			<table id="Table2" class="filtersIn">
				<TR>
					<TD width="70%">Cargar Información del Conteo : </TD>
					<TD align="right"><asp:DropDownList id="ddlConteosConsultas" runat="server">
							<asp:ListItem Value="0" Selected="True">Conteo 1</asp:ListItem>
							<asp:ListItem Value="1">Conteo 2</asp:ListItem>
							<asp:ListItem Value="2">Conteo 3</asp:ListItem>
							<asp:ListItem Value="3">Conteo Definitivo</asp:ListItem>
						</asp:DropDownList></TD></TR>
				<TR>
					<TD width="70%">Cantidad de Tarjetas sin registrar este conteo : <asp:Label id="lbCantidadTarjetas" runat="server" Font-Bold="True"></asp:Label></TD>
					<TD align="right"><asp:Button id="btnCargar" runat="server" Text="Cargar Información" onclick="btnCargar_Click"></asp:Button></TD></TR>
				<TR>
					<TD colSpan="2"><asp:DataGrid id="dgConteoInformacion" runat="server" cssclass="datagrid" Visible="False" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False">
							<FooterStyle cssclass="footer">
							</FooterStyle>

							<SelectedItemStyle Font-Bold="True" cssclass="selected">
							</SelectedItemStyle>

							<AlternatingItemStyle cssclass="alternate">
							</AlternatingItemStyle>

							<ItemStyle cssclass="item">
							</ItemStyle>

							<HeaderStyle Font-Bold="True" cssclass="header">
							</HeaderStyle>

							<Columns>
								<asp:BoundColumn DataField="dinv_tarjeta" ReadOnly="True" HeaderText="N&#250;mero de Tarjeta de Conteo"></asp:BoundColumn>
								<asp:BoundColumn DataField="referencia_editada" ReadOnly="True" HeaderText="C&#243;digo de &#205;tem"></asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_mite_nombre" ReadOnly="True" HeaderText="Nombre de &#205;tem"></asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_ubicacion" ReadOnly="True" HeaderText="Nombre Ubicaci&#243;n"></asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_msal_cantactual" HeaderText="Cantidad Registrada">
									<ItemStyle HorizontalAlign="Right">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_conteo1" ReadOnly="True" HeaderText="Cantidad Conteo 1">
									<ItemStyle HorizontalAlign="Right">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_conteo2" ReadOnly="True" HeaderText="Cantidad Conteo 2">
									<ItemStyle HorizontalAlign="Right">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_conteo3" ReadOnly="True" HeaderText="Cantidad Conteo 3">
									<ItemStyle HorizontalAlign="Right">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_diferencia" ReadOnly="True" HeaderText="Cantidad de Diferencia">
									<ItemStyle HorizontalAlign="Right">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_costdiferencia" ReadOnly="True" HeaderText="Costo de Diferencia">
									<ItemStyle HorizontalAlign="Right">
									</ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="dinv_contdefinitivo" ReadOnly="True" HeaderText="Conteo Definitivo">
									<ItemStyle HorizontalAlign="Right">
									</ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:DataGrid></TD></TR></TABLE></DIV></DIV>
</asp:panel>
</table>
</tbody>
</TABLE>
</fieldset>
<script type="text/javascript">
setupAllTabs();
</script>

