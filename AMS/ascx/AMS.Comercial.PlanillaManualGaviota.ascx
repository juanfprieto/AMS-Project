<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaManualGaviota.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaManualGaviota" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language='javascript' src='../js/AMS.Tools.js' type='text/javascript'></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información del viaje:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" Width="150px" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Ruta Principal :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlRutaPrincipal" Font-Size="XX-Small" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<asp:panel id="pnlRuta" runat="server" Visible="false">
			<TR>
				<TD style="WIDTH: 130px; HEIGHT: 18px">
					<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Despachador :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:textbox id="txtNITTiquetero" onclick="MostrarPersonal(this,'D');" runat="server" Font-Size="XX-Small"
						Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:button id="btnSeleccionar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Seleccionar"></asp:button></TD>
			</TR>
			<asp:panel id="pnlViaje" runat="server" Visible="false">
				<TR>
					<TD style="WIDTH: 130px; HEIGHT: 18px">
						<asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Viaje :</asp:label></TD>
					<td>
						<asp:label id="lblNumViaje" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 130px; HEIGHT: 18px">
						<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Planilla :</asp:label></TD>
					<td>
						<asp:textbox id="txtPlanilla" runat="server" Font-Size="XX-Small" Width="80px"></asp:textbox></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa del Bus :</asp:label></TD>
					<td>
						<asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0', new Array(),1);TraerBus(this.value);"
							runat="server" Font-Size="XX-Small" Width="80px" MaxLength="6"></asp:textbox>&nbsp;
					</TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Número del Bus :</asp:label></TD>
					<td>
						<asp:textbox id="txtPlacaa" runat="server" Font-Size="XX-Small" Width="80px" ReadOnly="True"
							MaxLength="6"></asp:textbox>&nbsp;
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px">
						<asp:Label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Despacho :</asp:Label></TD>
					<TD style="WIDTH: 386px">
						<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="62px" Runat="server"
							MaxLength="10"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 139px">
						<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Hora Despacho :</asp:label></TD>
					<TD style="WIDTH: 386px; HEIGHT: 18px">
						<asp:DropDownList id="ddlHora" runat="server" Width="40px" font-Size="XX-Small"></asp:DropDownList>&nbsp;:&nbsp;
						<asp:DropDownList id="ddlMinuto" runat="server" Width="48px" font-Size="XX-Small"></asp:DropDownList></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Conductor Principal :</asp:label></TD>
					<td>
						<asp:textbox id="txtConductor" onclick="MostrarConductor(this);" runat="server" Font-Size="XX-Small"
							Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
						<asp:textbox id="txtConductora" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Relevador :</asp:label></TD>
					<td>
						<asp:textbox id="txtRelevador" onclick="MostrarRelevadores(this);" runat="server" Font-Size="XX-Small"
							Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
						<asp:textbox id="txtRelevadora" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
				</TR>
				<asp:Panel id="pnlSeleccionar" Visible="True" Runat="server">
					<TR>
						<td>&nbsp;</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top" colSpan="2" align="center">
							<asp:button id="btnSeleccionarPlanilla" Font-Size="XX-Small" Font-Bold="True" Runat="server"
								Text="Continuar"></asp:button></TD>
					</TR>
				</asp:Panel>
			</asp:panel>
		</asp:panel>
		<TR>
			<td>&nbsp;</TD>
		</TR>
	</table>
	<BR>
	<asp:panel id="pnlElementos" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Tiquetes:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrTiquetes" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No." ItemStyle-VerticalAlign="Top"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Tiquete" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:TextBox id="txtNumTiqueteEsp" runat="server" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="100px" Font-Size="XX-Small">
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Destino" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:dropdownlist id="ddlDestinoTiquete" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="   " ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left">
								<ItemTemplate>
									<!--table borderColor="transparent" cellSpacing="0" cellPadding="0" background="transparent"
										border="0">
										<tr>
											<td>
												<asp:dropdownlist id="ddlTipoTiqueteEsp" runat="server" Font-Size="XX-Small">
													<asp:ListItem Value="">---seleccione---</asp:ListItem>
													<asp:ListItem Value="PP">Prepago</asp:ListItem>
													<asp:ListItem Value="VO">Venta de Otro</asp:ListItem>
												</asp:dropdownlist></td>
											<td>
												<asp:textbox id="txtNITVentaOtro" onclick="MostrarPersonal(this,'D');" runat="server" Font-Size="XX-Small"
													Width="80px" ReadOnly="True"></asp:textbox></td>
										</tr>
									</table>-->
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cant." ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:textbox id="txtCantidadTiqueteEsp" Font-Size="XX-Small" Runat="server" Width="60px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Pasaje" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:textbox id="txtValorTiqueteEsp" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Total" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:textbox id="txtTotalTiqueteEsp" ReadOnly="True" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalTiqueteEsps" ReadOnly="True" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Encomiendas:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrEncomiendas" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="Num."></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Doc.">
								<ItemTemplate>
									<asp:TextBox id="txtNumEncomienda" runat="server" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="100px" Font-Size="XX-Small">
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Dest.">
								<ItemTemplate>
									<asp:dropdownlist id="ddlDestinoEncomienda" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Encomienda">
								<ItemTemplate>
									<asp:textbox id="txtValorEncomienda" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalEncomiendas" ReadOnly="True" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Descripcion Encomienda">
								<ItemTemplate>
									<asp:textbox id="textDescripcionEncomienda" Font-Size="XX-Small" Runat="server" Width="458px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" id="Table1" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Giros:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrGiros" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No."></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Doc.">
								<ItemTemplate>
									<asp:TextBox id="txtNumGiro" runat="server" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="100px" Font-Size="XX-Small">
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Agen. Dest.">
								<ItemTemplate>
									<asp:dropdownlist id="ddlAgenciaDestinoGiro" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Giro">
								<ItemTemplate>
									<asp:textbox id="txtValorGiro" onkeyup="NumericMask(this)" Font-Size="XX-Small" Runat="server"
										Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalGiros" ReadOnly="True" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Costo Giro">
								<ItemTemplate>
									<asp:textbox id="txtCostoGiro" onkeyup="NumericMask(this)" ReadOnly="False" Font-Size="XX-Small"
										Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalCostoGiros" ReadOnly="True" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" id="tblPago" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Anticipos/Servicios:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrPagos" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No." ItemStyle-VerticalAlign="Top"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Doc." ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:TextBox id="txtNumPago" runat="server" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="100px" Font-Size="XX-Small">
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Concepto" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:dropdownlist id="ddlConceptoPago" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Pago" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:textbox id="txtValorPago" onkeyup="NumericMask(this)" Font-Size="XX-Small" Runat="server"
										Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalPagos" ReadOnly="True" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Descripcion Anticipo-Gasto">
								<ItemTemplate>
									<asp:textbox id="TextDescripcionAnticipo" Font-Size="XX-Small" Runat="server" Width="458px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" id="tblTools" align="center">
			<TR>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:button id="btnPlanillar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Planillar"></asp:button></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel>
	<script language="javascript">
	var prctGiro=<%=ViewState["PorcentajeGiro"]%>;
	
	var tTiquetesEsp='<%=ViewState["strTotalTiquetesEsp"]%>';	
	var tEncomiendas='<%=ViewState["strTotalEncomiendas"]%>';
	var tGiros='<%=ViewState["strTotalGiros"]%>';
	var tCGiros='<%=ViewState["strCostoGiros"]%>';
	var tPagos='<%=ViewState["strTotalPagos"]%>';
	
	
	var txtNITConductor=document.getElementById("<%=txtConductor.ClientID%>");
	var txtNombreConductor=document.getElementById("<%=txtConductora.ClientID%>");
	var txtPlaca=document.getElementById("<%=txtPlaca.ClientID%>");
	var txtNumeroBus=document.getElementById("<%=txtPlacaa.ClientID%>");
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
		
	function totalesPrts(strPrts){
		arTiqs=strPrts.split(',');
		totalT=0;
		if(arTiqs.length<=1)return;
		nTq=0;
		tqA=0;
		for(nTq=0;nTq<arTiqs.length-1;nTq++){
			tqAs=document.getElementById(arTiqs[nTq]).value.replace(/\,/g,'');
			tqA=parseFloat(tqAs);
			if(tqA>0)totalT+=tqA;
		}
		if(totalT>0)totalT=(Math.round(totalT*100))/100;
		objTot=document.getElementById(arTiqs[nTq]);
		parseValor(totalT,objTot);
	}
	function totalTicketes(txtNum,txtVal,txtTot,n){
		var objNum=document.getElementById(txtNum);
		var objVal=document.getElementById(txtVal);
		var objTot=document.getElementById(txtTot);
		tTiq = (parseFloat(objNum.value.replace(/\,/g,'')) * parseFloat(objVal.value.replace(/\,/g,'')));
		parseValor(tTiq,objTot);
		if(n==1)
			totalesPrts(tTiquetes);
		else
			totalesPrts(tTiquetesEsp);
	}
	function totalTicketesEsp(txtNum,txtVal,txtTot,n){
		var objNum=document.getElementById(txtNum);
		var objVal=document.getElementById(txtVal);
		var objTot=document.getElementById(txtTot);
		tTiq = (parseFloat(objNum.value.replace(/\,/g,'')) * parseFloat(objVal.value.replace(/\,/g,'')));
		parseValor(tTiq,objTot);
		if(n==1)
			totalesPrts(tTiquetes);
		else
			totalesPrts(tTiquetesEsp);
	}
	function verNIT(val,txtN){
		var txtNIT=document.getElementById(txtN);
		if(val=="VO")txtNIT.style.display = "block";
		else txtNIT.style.display = "none";
	}
	function verConcepto(chkAnulado,txtC){
		var ddlConcepto=document.getElementById(txtC);
		if(chkAnulado.checked)ddlConcepto.style.display = "block";
		else ddlConcepto.style.display = "none";
	}
	function costoGiro(txtValor, strCosto){
		objTot=document.getElementById(strCosto);
		valor=(parseFloat(txtValor.value.replace(/\,/g,''))*prctGiro)/100;
		if(isNaN(valor))objTot.value="0";
		else {
			valor=Math.round(valor*100)/100;
			objTot.value=valor;
			parseValor(valor,objTot);}
	}
	function parseValor(valor,objTot){
		objTot.value=formatoValor(valor);
	}
	function KeyDownHandler(){
		if(event.keyCode == 13){
			TraerBus(txtPlaca.value);
			return(false);
		}
	}
	function TraerBus(placa){
		AMS_Comercial_PlanillaManual.TraerBus(placa,CambiaBus_Callback);
	}
	function CambiaBus_Callback(response){
		var respuesta=response.value;
		txtNITConductor.value="";
		txtNombreConductor.value="";
		txtNumeroBus.value="";
		if(respuesta.Tables[0].Rows.length>0){
			txtNITConductor.value=respuesta.Tables[0].Rows[0].NIT;
			txtNombreConductor.value=respuesta.Tables[0].Rows[0].NOMBRE;
			txtNumeroBus.value=respuesta.Tables[0].Rows[0].NUMERO;
		}
	}
	function MostrarPersonal(obj,flt){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	function MostrarConductor(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODICARGO=\'CO\';';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	function MostrarRelevadores(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MRELEVADORES_TRANSPORTES MP  WHERE MP.MNIT_NIT=MNIT.MNIT_NIT AND MP.FECHA_DESDE<=\'<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>\' AND MP.FECHA_HASTA>=\'<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>\';';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	<%=strActScript%>
	</script>
	<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label>
</DIV>
