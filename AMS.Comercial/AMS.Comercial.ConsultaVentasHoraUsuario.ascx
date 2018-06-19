<%@ Control language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultaVentasHoraUsuario.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultaVentasHoraUsuario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td><asp:label id="Label1" Font-Bold="True"  runat="server">Agencia :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia"  runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
		</tr>
		<tr>
			<td><asp:label id="Label99" Font-Bold="True"  runat="server">Despachador :</asp:label></td>
			<td><asp:dropdownlist id="ddlDespachador"  runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
		</tr>
	</table>
	<table class="filtersIn">
		<TR>
			<TD><asp:label id="Label18" Font-Bold="True"  runat="server">Fecha Inicial  :</asp:label></TD>
			<TD><asp:textbox id="txtFechaI" onkeyup="DateMask(this)"  CssClass="tpequeno" Runat="server"
					MaxLength="10"></asp:textbox></TD>
			<TD>
				<asp:label id="Label11" runat="server"  Font-Bold="True">Hora :</asp:label></TD>
			<TD>
				<asp:DropDownList id="ddlHoraI" runat="server" Width="40px" ></asp:DropDownList>&nbsp;:&nbsp;
				<asp:DropDownList id="ddlMinutoI" runat="server" Width="43px" ></asp:DropDownList></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label8" Font-Bold="True"  runat="server">Fecha Final  :</asp:label></TD>
			<TD><asp:textbox id="txtFechaF" onkeyup="DateMask(this)" CssClass="tpequeno" Runat="server"
					MaxLength="10"></asp:textbox></TD>
			<TD>
				<asp:label id="Label9" runat="server"  Font-Bold="True">Hora :</asp:label></TD>
			<TD>
				<asp:DropDownList id="ddlHoraF" runat="server" Width="40px" ></asp:DropDownList>&nbsp;:&nbsp;
				<asp:DropDownList id="ddlMinutoF" runat="server" Width="44px" ></asp:DropDownList></TD>
		</TR>
		<TR>
			<td></td>
			<TD><asp:button id="btnConsultar" Font-Bold="True"  Runat="server" Text="Consultar"></asp:button></TD>
		</TR>
	</table>
	<table class="filtersIn">
		<TR>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center">
                <asp:button id="btnGenerar" Font-Bold="True"  Runat="server" Text="Generar Reporte"></asp:button>
				<asp:hyperlink id="Ver" runat="server" Target="_blank" Visible="False">De Click Aqui para ver el Reporte</asp:hyperlink>
            </TD>
		</TR>
		<TR>
			<td>
				<asp:label id="lblError" Font-Bold="True"  runat="server"></asp:label>
            </TD>
		</TR>
	</table>
	<br>
	<asp:panel id="pnlConsulta" Runat="server" Visible="False">
		<TABLE class="filtersIn">
			<TR>
				<TD>
					<asp:label id="Label12" runat="server"  Font-Bold="True">Agencia:</asp:label></TD>
				<TD>
					<asp:label id="lblCodigoAgencia" runat="server"  Font-Bold="True" Width="6px"></asp:label></TD>
				<TD>
					<asp:label id="lblNombreAgencia" runat="server"  Font-Bold="True"></asp:label></TD>
			</TR>
		</TABLE>
		<TABLE class="filtersIn">
			<TR>
				<TD align="center">
                    <div style="overflow:scroll;">
					<asp:datagrid id="dgrVentas" runat="server" Width="500" ShowFooter="False" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle  HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle  Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
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
					</asp:datagrid>
                    </div>
                </TD>
			</TR>
		</TABLE>
		<TABLE class="filtersIn">
			<TR>
				<TD>
					<asp:label id="Label2" runat="server"  Font-Bold="True">Total Tiquetes :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalTiquetes" runat="server"  Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label3" runat="server"  Font-Bold="True">Total Remesas :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalRemesas" runat="server"  Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label4" runat="server"  Font-Bold="True">Total Giros :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalGiros" runat="server"  Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label5" runat="server"  Font-Bold="True">Total Ingresos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalIngresos" runat="server"  Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label10" runat="server"  Font-Bold="True">Total Egresos :</asp:label></TD>
				<TD>
					<asp:label id="lblTotalEgresos" runat="server"  Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label7" runat="server"  Font-Bold="True">Total :</asp:label></TD>
				<TD>
					<asp:label id="lblTotal" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label13" runat="server"  Font-Bold="True">Total valor Giros :</asp:label></TD>
				<TD>
					<asp:label id="lblValorGiros" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></TD>
			</TR>
		</TABLE>
	</asp:panel>
</fieldset>
<!--script language="javascript" type="text/javascript">
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	function MostrarPersonal(obj,flt){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
</script>-->
