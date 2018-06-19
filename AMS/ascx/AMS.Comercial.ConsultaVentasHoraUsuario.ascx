<%@ Control language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultaVentasHoraUsuario.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultaVentasHoraUsuario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH:82px; HEIGHT:25px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
		</tr>
		<tr>
			<td style="WIDTH:82px; HEIGHT:25px"><asp:label id="Label99" Font-Bold="True" Font-Size="XX-Small" runat="server">Despachador :</asp:label></td>
			<td><asp:dropdownlist id="ddlDespachador" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
		</tr>
	</table>
	<table style="WIDTH: 773px" align="center">
		<TR>
			<TD style="WIDTH: 66px"><asp:label id="Label18" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Inicial  :</asp:label></TD>
			<TD style="WIDTH: 57px"><asp:textbox id="txtFechaI" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="65px" Runat="server"
					MaxLength="10"></asp:textbox></TD>
			<TD style="WIDTH: 8px">
				<asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Hora :</asp:label></TD>
			<TD style="WIDTH: 450px; HEIGHT: 18px">
				<asp:DropDownList id="ddlHoraI" runat="server" Width="40px" font-Size="XX-Small"></asp:DropDownList>&nbsp;:&nbsp;
				<asp:DropDownList id="ddlMinutoI" runat="server" Width="43px" font-Size="XX-Small"></asp:DropDownList></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 66px"><asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Final  :</asp:label></TD>
			<TD style="WIDTH: 57px"><asp:textbox id="txtFechaF" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="67px" Runat="server"
					MaxLength="10"></asp:textbox></TD>
			<TD style="WIDTH: 8px">
				<asp:label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Hora :</asp:label></TD>
			<TD style="HEIGHT: 15px">
				<asp:DropDownList id="ddlHoraF" runat="server" Width="40px" font-Size="XX-Small"></asp:DropDownList>&nbsp;:&nbsp;
				<asp:DropDownList id="ddlMinutoF" runat="server" Width="44px" font-Size="XX-Small"></asp:DropDownList></TD>
		</TR>
		<TR>
			<td style="WIDTH: 66px"></td>
			<TD style="WIDTH: 57px"><asp:button id="btnConsultar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Consultar"></asp:button></TD>
		</TR>
	</table>
	<table style="WIDTH: 773px" align="center">
		<TR>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center"><asp:button id="btnGenerar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Generar Reporte"></asp:button>&nbsp;&nbsp;
				<asp:hyperlink id="Ver" runat="server" Target="_blank" Visible="False">De Click Aqui para ver el Reporte</asp:hyperlink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
			<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
		</TR>
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlConsulta" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 26px; HEIGHT: 18px">
					<asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia :</asp:label></TD>
				<TD style="WIDTH: 49px; HEIGHT: 5px">
					<asp:label id="lblCodigoAgencia" runat="server" Font-Size="XX-Small" Font-Bold="True" Width="6px"></asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblNombreAgencia" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
		</TABLE>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrVentas" runat="server" Width="500" ShowFooter="False" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DESPACHADOR" HeaderText="Nit"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_DESPACHADOR" HeaderText="Nombre"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="PLANILLA" HeaderText="Plnlla"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="RUTA" HeaderText="RtaViaje"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="VIAJE" HeaderText="Viaje"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="HORA_DESPACHO" HeaderText="HraDesp"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="PLACA" HeaderText="Placa"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUMERO_BUS" HeaderText="Bus"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DOCUMENTO" HeaderText="Dcmnto"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_DOCUMENTO" HeaderText="FechaDcmnto"
								DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="RUTA_DOCUMENTO" HeaderText="RtaDoc"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_CONCEPTO" HeaderText="Concepto"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="TIQUETES"
								HeaderText="Tiqs"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_TIQUETES"
								HeaderText="TIQUETES"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="ENCOMIENDAS"
								HeaderText="REMESAS"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="COSTO_GIROS"
								HeaderText="GIROS"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_INGRESOS"
								HeaderText="INGRESOS"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_EGRESOS"
								HeaderText="EGRESOS"></asp:BoundColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Tiquetes :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalTiquetes" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Remesas :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalRemesas" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Giros :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalGiros" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Ingresos :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalIngresos" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Egresos :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotalEgresos" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Total :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblTotal" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Total valor Giros :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:label id="lblValorGiros" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></TD>
			</TR>
		</TABLE>
	</asp:panel></DIV>
<!--script language="javascript" type="text/javascript">
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	function MostrarPersonal(obj,flt){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
</script>-->
