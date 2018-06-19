<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaVirtual.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaVirtual" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información del viaje:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Ruta Principal :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlRutaPrincipal" runat="server" Font-Size="XX-Small" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<asp:panel id="pnlRuta" runat="server" Visible="false">
			<TR>
				<TD>
					<asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Placa del Bus :</asp:label></TD>
				<TD>
					<asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0', new Array(),1);TraerBus(this.value);"
						Font-Size="XX-Small" runat="server" Width="80px" MaxLength="6"></asp:textbox>&nbsp;
				</TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label10" Font-Bold="True" Font-Size="XX-Small" runat="server">Número del Bus :</asp:label></TD>
				<TD>
					<asp:textbox id="txtPlacaa" Font-Size="XX-Small" runat="server" Width="80px" MaxLength="6" ReadOnly="True"></asp:textbox>&nbsp;
				</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:Label id="Label18" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha Despacho :</asp:Label></TD>
				<TD style="WIDTH: 386px">
					<asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="62px" MaxLength="10"
						Runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 139px">
					<asp:label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">Hora Despacho :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px">
					<asp:DropDownList id="ddlHora" runat="server" Width="40px" font-Size="XX-Small"></asp:DropDownList>&nbsp;:&nbsp;
					<asp:DropDownList id="ddlMinuto" runat="server" Width="48px" font-Size="XX-Small"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Conductor Principal :</asp:label></TD>
				<TD>
					<asp:textbox id="txtConductor" onclick="MostrarConductor(this);" Font-Size="XX-Small" runat="server"
						Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:textbox id="txtConductora" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Relevador :</asp:label></TD>
				<TD>
					<asp:textbox id="txtRelevador" onclick="MostrarRelevadores(this);" Font-Size="XX-Small" runat="server"
						Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:textbox id="txtRelevadora" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<asp:Panel id="pnlSeleccionar" Visible="True" Runat="server">
				<TR>
					<TD>&nbsp;</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top" align="center" colSpan="2">
						<asp:button id="btnSeleccionarPlanilla" Font-Bold="True" Font-Size="XX-Small" Runat="server"
							Text="Continuar"></asp:button></TD>
				</TR>
			</asp:Panel>
		</asp:panel>
		<TR>
			<TD>&nbsp;</TD>
		</TR>
	</table>
	<BR>
	<asp:panel id="pnlElementos" Visible="False" Runat="server">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Tiquetes:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrTiquetes" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Dest.">
								<ItemTemplate>
									<asp:dropdownlist id="ddlDestinoTiquete" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cant.">
								<ItemTemplate>
									<asp:textbox id="txtCantidadTiquete" Font-Size="XX-Small" Runat="server" Width="60px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Pasaje">
								<ItemTemplate>
									<asp:textbox id="txtValorTiquete" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Total">
								<ItemTemplate>
									<asp:textbox id="txtTotalTiquete" ReadOnly="True" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalTiquetes" ReadOnly="True" Font-Size="XX-Small" Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Anulado" ItemStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<table borderColor="transparent" cellSpacing="0" cellPadding="0" background="transparent"
										border="0">
										<tr>
											<td>
												<asp:CheckBox id="chkAnlulado" Font-Size="XX-Small" Runat="server"></asp:CheckBox></td>
											<td>
												<asp:dropdownlist id="ddlConceptoAnulacion" Font-Size="XX-Small" runat="server"></asp:dropdownlist>
											</td>
										</tr>
									</table>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE id="tblTools" style="WIDTH: 773px" align="center">
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:button id="btnPlanillar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Planillar"></asp:button></TD>
			</TR>
			<TR>
				<TD>&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel>
	<script language="javascript">
	var tTiquetes='<%=ViewState["strTotalTiquetes"]%>';
	var tTiquetesEsp='<%=ViewState["strTotalTiquetesEsp"]%>';	
	var tEncomiendas='<%=ViewState["strTotalEncomiendas"]%>';
	var tGiros='<%=ViewState["strTotalGiros"]%>';
	var tCGiros='<%=ViewState["strCostoGiros"]%>';
	var tPagos='<%=ViewState["strTotalPagos"]%>';
	var tTiposEsp='<%=ViewState["strTiposTiquetesEsp"]%>';
	var tTiquetesAnul='<%=ViewState["strAnulacionTiquetes"]%>';
	var txtNITConductor=document.getElementById("<%=txtConductor.ClientID%>");
	var txtNombreConductor=document.getElementById("<%=txtConductora.ClientID%>");
	var txtPlaca=document.getElementById("<%=txtPlaca.ClientID%>");
	var txtNumeroBus=document.getElementById("<%=txtPlacaa.ClientID%>");
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	verTiposEsp();
	verConceptosAnulacion();
	function verConceptosAnulacion(){
		arTiqs=tTiquetesAnul.split(',');
		for(nTq=0;nTq<arTiqs.length;nTq++){
			if(arTiqs[nTq].length>0){
				arElm=arTiqs[nTq].split('@');
				if(arElm.length==2){
					chkAnulado=document.getElementById(arElm[0]);
					dlConcepto=document.getElementById(arElm[1]);
					if(chkAnulado.checked)
						dlConcepto.style.display = "block";
					else
						dlConcepto.style.display = "none";
				}
			}
		}
	}
	function verTiposEsp(){
		arTiqs=tTiposEsp.split(',');
		for(nTq=0;nTq<arTiqs.length;nTq++){
			if(arTiqs[nTq].length>0){
				arElm=arTiqs[nTq].split('@');
				if(arElm.length==2){
					ddlTipo=document.getElementById(arElm[0]);
					txtNIT=document.getElementById(arElm[1]);
					if(ddlTipo.value=="VO")
						txtNIT.style.display = "block";
					else
						txtNIT.style.display = "none";
				}
			}
		}
	}
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
	<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></DIV>
