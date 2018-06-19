<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaBus.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaBus" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<asp:panel id="pnlImpMovil" Runat="server" Visible="False">
	<OBJECT id="prtPlanilla" height="1" width="1" classid="CLSID:7AF0226B-7A49-4253-BA78-326BA0FA4497"
		VIEWASTEXT>
	</OBJECT>
</asp:panel>
<script language="javascript" type="text/javascript">
	function verPlanillaM(){
		var strPlanilla='<%=strPlanilla%>';
		var prtPlanilla=document.getElementById("prtPlanilla");
		prtPlanilla.Imprimir(strPlanilla.replace(/\\n/g, '\n'));
		alert('La planilla '+<%=ViewState["Planilla"]%>+' se ha impreso.');
	}
	
</script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td><b>&nbsp;</b></td>
		</tr>
		<TR>
			<TD style="WIDTH: 100px"><asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">No. Planilla :</asp:label></TD>
			<TD colSpan="3"><asp:label id="lblPlanilla" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 100px"><asp:label id="Label31" runat="server" Font-Size="XX-Small" Font-Bold="True">Origen :</asp:label></TD>
			<td><asp:label id="lblOrigen" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			<TD style="WIDTH: 100px"><asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Destino :</asp:label></TD>
			<td><asp:label id="lblDestino" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
		<TR>
			<td><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia :</asp:label></TD>
			<TD colSpan="3"><asp:label id="lblAgencia" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 16px"><asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Salida :</asp:label></TD>
			<TD style="HEIGHT: 16px"><asp:label id="lblFecha" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			<TD style="HEIGHT: 16px"><asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Hora Salida :</asp:label></TD>
			<TD style="HEIGHT: 16px"><asp:label id="lblHora" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
		<TR>
			<td><asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Número Vehículo :</asp:label></TD>
			<td><asp:label id="lblNumeroBus" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			<td><asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa Vehículo :</asp:label></TD>
			<td><asp:label id="lblPlaca" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
		<TR>
			<td><asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Conductor :</asp:label></TD>
			<TD colSpan="3"><asp:label id="lblConductor" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
		<TR>
			<td><asp:label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Relevador :</asp:label></TD>
			<td><asp:label id="lblRelevador" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Tiquetes</b></td>
		</tr>
		<TR>
			<TD align="center"><asp:datagrid id="dgrTiquetes" runat="server" Width="500" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="No." DataFormatString="{0:000000}"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DESTINO" HeaderText="Dest."></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="NUMERO_PUESTOS" HeaderText="Cant."></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_PASAJE"
							HeaderText="Vr. Pasaje"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_TOTAL"
							HeaderText="Vr. Total"></asp:BoundColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Tiquetes Prepago</b></td>
		</tr>
		<TR>
			<TD align="center"><asp:datagrid id="dgrTiquetesPre" runat="server" Width="500" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="No." DataFormatString="{0:000000}"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DESTINO" HeaderText="Dest."></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_PASAJE" DataFormatString="{0:#,#}"
							HeaderText="Vr. Pasaje"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_TOTAL" DataFormatString="{0:#,#}"
							HeaderText="Vr. Total"></asp:BoundColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Encomiendas</b></td>
		</tr>
		<TR>
			<TD align="center"><asp:datagrid id="dgrRemesas" runat="server" Width="500" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="No." DataFormatString="{0:000000}"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DESTINO" HeaderText="Dest."></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="COSTO_ENCOMIENDA" DataFormatString="{0:#,#}"
							HeaderText="Costo"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_TOTAL" DataFormatString="{0:#,#}"
							HeaderText="Vr CostoTotal."></asp:BoundColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Giros</b></td>
		</tr>
		<TR>
			<TD align="center"><asp:datagrid id="dgrGiros" runat="server" Width="500" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="No." DataFormatString="{0:000000}"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="DESTINO" HeaderText="Dest."></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="COSTO_GIRO" DataFormatString="{0:#,###.##}"
							HeaderText="Costo Giro"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_GIRO" DataFormatString="{0:#,#}"
							HeaderText="Valor giro"></asp:BoundColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Anticipos/Servicios/Gastos/OtrosIngresos</b></td>
		</tr>
		<TR>
			<TD align="center"><asp:datagrid id="dgrAnticipos" runat="server" Width="500" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="No." DataFormatString="{0:000000}"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CONCEPTO" HeaderText="Concepto"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_TOTAL_INGRESO" DataFormatString="{0:#,#}"
							HeaderText="Valor Ingreso"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_TOTAL_EGRESO" DataFormatString="{0:#,#}"
							HeaderText="Valor Egreso"></asp:BoundColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
	</table>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Descuentos Planilla</b></td>
		</tr>
		<TR>
			<TD align="center"><asp:datagrid id="dgrDescuentos" runat="server" Width="500" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="No." DataFormatString="{0:000000}"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CONCEPTO" HeaderText="Concepto"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataField="VALOR_DESCUENTO" DataFormatString="{0:#,#}"
							HeaderText="Valor Descuento"></asp:BoundColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
	</table>
	<br>
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 110px"></td>
		</tr>
		<tr>
			<td style="WIDTH: 110px"><asp:label id="Label14" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Ingresos :</asp:label></td>
			<td><asp:label id="lblIngresos" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 110px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Egresos :</asp:label></td>
			<td><asp:label id="lblEgresos" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 110px"><asp:label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Descuentos :</asp:label></td>
			<td><asp:label id="lblDescuentos" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 110px"><asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Pasajeros :</asp:label></td>
			<td><asp:label id="lblPasajeros" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 110px"><asp:label id="Labe13" runat="server" Font-Size="XX-Small" Font-Bold="True">Total Valor Giros :</asp:label></td>
			<td><asp:label id="lblValorGiros" runat="server" Font-Size="Small" Font-Bold="True"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 110px"><b>&nbsp;</b></td>
		</tr>
		<TR>
			<TD style="WIDTH: 110px; HEIGHT: 59px" vAlign="top"><asp:label id="Label25" runat="server" Font-Size="XX-Small" Font-Bold="True">Observacion :</asp:label></TD>
			<TD style="WIDTH: 265px; HEIGHT: 59px" vAlign="top"><asp:label id="txtObservacion" runat="server" Font-Size="XX-Small" Width="520px" TextMode="MultiLine"
					Height="48px"></asp:label></TD>
		</TR>
	</table>
	<br>
	<asp:panel id="ImprimirPlanilla" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<td>
					<P align="center"><INPUT onclick="verPlanilla();" type="button" value="Imprimir" class="noEspera"></P>
				</TD>
			</TR>
		</TABLE>
	</asp:panel>
	<asp:panel id="ImprimirPlanillaMovil" Runat="server" Visible="False">
		<OBJECT id="prtPlanilla" height="1" width="1" classid="CLSID:7AF0226B-7A49-4253-BA78-326BA0FA4497"
			VIEWASTEXT>
		</OBJECT>
		<SCRIPT language="javascript" type="text/javascript">
			verPlanillaM();
		</SCRIPT>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<td>
					<P align="center"><INPUT onclick="verPlanillaM();" type="button" value="Imprimir" class="noEspera"></P>
				</TD>
			</TR>
		</TABLE>
	</asp:panel>
	</asp:panel><asp:panel id="pnlGemelas" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<td>
					<P align="center"><INPUT onclick="verPlanillaG(1);" type="button" value="Imprimir Gemela 1" class="noEspera"></P>
				</TD>
				<td>
					<P align="center"><INPUT onclick="verPlanillaG(2);" type="button" value="Imprimir Gemela 2" class="noEspera"></P>
				</TD>
			</TR>
		</TABLE>
	</asp:panel><asp:panel id="PnlBorrarPlanilla" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="HEIGHT: 1px" align="center">
					<asp:button id="btnBorrarPlanilla" Font-Bold="True" Font-Size="XX-Small" Width="126px" Height="20px"
						Runat="server" Text="Borrar Planilla"></asp:button></TD>
			</TR>
		</TABLE>
	</asp:panel><br>
	<asp:panel id="Regresar" Runat="server" Visible="True">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="HEIGHT: 1px" align="center">
					<asp:button id="btnRegresar" Font-Bold="True" Font-Size="XX-Small" Width="126px" Height="20px"
						Runat="server" Text="Regresar"></asp:button></TD>
			</TR>
		</TABLE>
	</asp:panel><br>
</DIV>
<script language="javascript" type="text/javascript">
	function verPlanilla(){
		window.open('../aspx/AMS.Comercial.Planilla.aspx?pln='+<%=ViewState["Planilla"]%>, 'PLANILLA'+<%=ViewState["Planilla"]%>, "width=340,height=600,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");
	}
	function verPlanillaG(tp){
		window.open('../aspx/AMS.Comercial.PlanillaGemela.aspx?tp='+tp+'&pln=<%=ViewState["Planilla"]%>', 'PLANILLAG'+tp+<%=ViewState["Planilla"]%>, "width=340,height=600,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");
	}
</script>
<asp:label id="lblP" runat="server"></asp:label>
