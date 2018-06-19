<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaManual.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaManual" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Tools.js"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td><b>Información del viaje:</b></td>
		</tr>
		<tr>
			<td><asp:label id="Label4" Font-Bold="True"  runat="server">Agencia:</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia"  runat="server" Width="150px" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td><asp:label id="Label11" Font-Bold="True"  runat="server">Ruta Principal:</asp:label></td>
			<td><asp:dropdownlist id="ddlRutaPrincipal"  runat="server" AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<asp:panel id="pnlRuta" runat="server" Visible="false">
			<TR>
				<td>
					<asp:label id="Label6" runat="server"  Font-Bold="True">Despachador:</asp:label>
                </TD>
				<TD>
					<asp:textbox id="txtNITTiquetero" onclick="MostrarPersonal(this,'D');" runat="server" Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:button id="btnSeleccionar"  Font-Bold="True" Runat="server" Text="Seleccionar"></asp:button>
                </TD>
			</TR>
			<asp:panel id="pnlViaje" runat="server" Visible="false">
				<TR>
					<td>
						<asp:label id="Label12" runat="server"  Font-Bold="True">Número de Viaje:</asp:label>
                    </TD>
					<td>
						<asp:label id="lblNumViaje" runat="server"  Font-Bold="True"></asp:label>
                    </TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label5" runat="server"  Font-Bold="True">Número de Planilla:</asp:label>
                    </TD>
					<td>
						<asp:textbox id="txtPlanilla" runat="server"  Width="80px"></asp:textbox>
                    </TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label1" runat="server"  Font-Bold="True">Placa del Bus:</asp:label>
                    </TD>
					<td>
						<asp:textbox id="txtPlaca" ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0', new Array(),1);TraerBus(this.value);"
							runat="server"  Width="80px" MaxLength="6"></asp:textbox>
					</TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label10" runat="server"  Font-Bold="True">Número del Bus:</asp:label></TD>
					<td>
						<asp:textbox id="txtPlacaa" runat="server"  Width="80px" ReadOnly="True" MaxLength="6"></asp:textbox>
					</TD>
				</TR>
				<TR>
					<TD>
						<asp:Label id="Label18" runat="server"  Font-Bold="True">Fecha Despacho:</asp:Label></TD>
					<TD>
						<asp:textbox id="txtFecha" onkeyup="DateMask(this)"  Width="62px" Runat="server"
							MaxLength="10"></asp:textbox></TD>
				</TR>
				<TR>
					<TD>
						<asp:label id="Label3" runat="server"  Font-Bold="True">Hora Despacho:</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlHora" runat="server" Width="40px" ></asp:DropDownList>&nbsp;:&nbsp;
						<asp:DropDownList id="ddlMinuto" runat="server" Width="48px" ></asp:DropDownList></TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label8" runat="server"  Font-Bold="True">Conductor Principal:</asp:label>
                    </TD>
					<td colspan="2">
						<asp:textbox id="txtConductor" onclick="MostrarConductor(this);" runat="server" Width="80px" ReadOnly="True"></asp:textbox>
						<asp:textbox id="txtConductora" runat="server"  Width="200px" ReadOnly="True"></asp:textbox>
                    </TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label9" runat="server"  Font-Bold="True">Relevador:</asp:label>
                    </TD>
					<td colspan="2">
						<asp:textbox id="txtRelevador" onclick="MostrarRelevadores(this);" runat="server"  Width="80px" ReadOnly="True"></asp:textbox>
						<asp:textbox id="txtRelevadora" runat="server"  Width="200px" ReadOnly="True"></asp:textbox>
                    </TD>
				</TR>
				<TR>
					<td>
						<asp:label id="Label7" runat="server"  Font-Bold="True">Numero primer tiquete:</asp:label>
                    </TD>
					<td>
						<asp:textbox id=txtPrimerTiquete runat="server"  Width="80px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>">
						</asp:textbox>
                    </TD>
				</TR>
				<asp:Panel id="pnlSeleccionar" Visible="True" Runat="server">					
					<TR>
						<TD vAlign="top" colSpan="2" align="center">
							<asp:button id="btnSeleccionarPlanilla"  Font-Bold="True" Runat="server" Text="Continuar"></asp:button>
                        </TD>
					</TR>
				</asp:Panel>
			</asp:panel>
		</asp:panel>
    </table>


	<BR>
	<asp:panel id="pnlElementos" Visible="False" Runat="server">
		<TABLE class="filtersIn">
			<TR>
				<TD><h3>Tiquetes:</h3></TD>
			</TR>
			<TR>
				<TD>
                <div  style="height:350px; overflow:scroll;">
					<asp:datagrid id="dgrTiquetes" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle  HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle  Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No."></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Tiquete">
								<ItemTemplate>
									<asp:TextBox id="txtNumTiquete" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TIQUETE") %>' MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="80px" >
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Asignado ">
								<ItemTemplate>
									<asp:textbox id="txtResponsable"  Runat="server" Width="200px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Dest.">
								<ItemTemplate>
									<asp:dropdownlist id="ddlDestinoTiquete" CssClass="dpequeno"  runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cant.">
								<ItemTemplate>
									<asp:textbox id="txtCantidadTiquete"  Runat="server" Width="60px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Pasaje">
								<ItemTemplate>
									<asp:textbox id="txtValorTiquete"  Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Total">
								<ItemTemplate>
									<asp:textbox id="txtTotalTiquete" ReadOnly="True"  Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalTiquetes" ReadOnly="True"  Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Anulado" ItemStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<table borderColor="transparent" cellSpacing="0" cellPadding="0" background="transparent" border="0">
										<tr>
											<td>
												<asp:CheckBox id="chkAnlulado"  Runat="server"></asp:CheckBox></td>
											<td>
												<asp:dropdownlist id="ddlConceptoAnulacion"  runat="server"></asp:dropdownlist>
											</td>
										</tr>
									</table>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Pend." ItemStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<table borderColor="transparent" cellSpacing="0" cellPadding="0" background="transparent"
										border="0">
										<tr>
											<td>
												<asp:CheckBox id="chkPendiente"  Runat="server"></asp:CheckBox></td>
										</tr>
									</table>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid>
                    </div>
                </TD>
			</TR>
			<TR>
				<td>Tiquetes Prepago</TD>
			</TR>
			<TR>
				<TD align="center" class="scrollable">
					<asp:datagrid id="dgrTiqueteEsps" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle  HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle  Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No." ItemStyle-VerticalAlign="Top"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Tiquete" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:TextBox id="txtNumTiqueteEsp" runat="server" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="80px" >
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Dest." ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:dropdownlist id="ddlDestinoTiqueteEsp"  runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Pasaje" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:textbox id="txtValorTiqueteEsp"  Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Total" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:textbox id="txtTotalTiqueteEsp" ReadOnly="True"  Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalTiqueteEsps" ReadOnly="True"  Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE class="filtersIn">
			<TR>
				<TD><h3>Encomiendas/Giros:</h3></TD>
			</TR>
			<TR>
				<TD align="center" class="scrollable">
					<asp:datagrid id="dgrEncomiendas" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle  HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle  Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="Consctvo."></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Tipo.">
								<ItemTemplate>
									<asp:dropdownlist id="ddlTipo" runat="server" CssClass="dpequeno">
										<asp:ListItem Value="E">Encomienda</asp:ListItem>
										<asp:ListItem Value="G">Giro</asp:ListItem>
									</asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Doc.">
								<ItemTemplate>
									<asp:TextBox id="txtNumEncomienda" runat="server" MaxLength="7" Width="80px" ></asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Asig.">
								<ItemTemplate>
									<asp:CheckBox id="chkAsignacion"  Runat=server Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "RELACIONAR")) %>'>
									</asp:CheckBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Dest.">
								<ItemTemplate>
									<asp:dropdownlist id="ddlDestinoEncomienda"  runat="server" CssClass="dpequeno"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr.Remesa/Giro">
								<ItemTemplate>
									<asp:textbox id="txtValorEncomienda"  Runat="server" Width="90px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalEncomiendas" ReadOnly="True"  Runat="server" Width="90px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Diligencio">
								<ItemTemplate>
									<asp:dropdownlist id="ddlNitResponsable" CssClass="dmediano" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Descripcion ">
								<ItemTemplate>
									<asp:textbox id="txtDescripcionEncomienda"  Runat="server" Width="300px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
		<BR> <!--TABLE id="Table1" class="filtersIn">
			<TR>
				<TD  colSpan="3"><B>Giros:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrGiros" runat="server" AutoGenerateColumns="False" ShowFooter="True">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle  HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle  Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No."></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Doc.">
								<ItemTemplate>
									<asp:TextBox id="txtNumGiro" runat="server" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="100px" >
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Agen. Dest.">
								<ItemTemplate>
									<asp:dropdownlist id="ddlAgenciaDestinoGiro"  runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Giro">
								<ItemTemplate>
									<asp:textbox id="txtValorGiro" onkeyup="NumericMask(this)"  Runat="server"
										Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalGiros" ReadOnly="True"  Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Costo Giro">
								<ItemTemplate>
									<asp:textbox id="txtCostoGiro" onkeyup="NumericMask(this)" ReadOnly="False" 
										Runat="server" Width="100px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalCostoGiros" ReadOnly="True"  Runat="server" Width="100px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
		</TABLE>
		<BR-->
		<TABLE class="filtersIn">
			<TR>
				<TD><h3>Anticipos/Servicios:</h3></TD>
			</TR>
			<TR>
				<TD align="center" class="scrollable">
					<asp:datagrid id="dgrPagos" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle  HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle  Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No." ItemStyle-VerticalAlign="Top"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Doc." ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:TextBox id="txtNumPago" runat="server" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="80px" >
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Concepto" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:dropdownlist id="ddlConceptoPago" CssClass="dmediano" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Vr. Pago" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:textbox id="txtValorPago" onkeyup="NumericMask(this)"  Runat="server"
										Width="90px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalPagos" ReadOnly="True" CssClass="dmediano" Runat="server" Width="90px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Diligencio">
								<ItemTemplate>
									<asp:dropdownlist id="ddlResponsableAnticipo" CssClass="dmediano" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Descripcion ">
								<ItemTemplate>
									<asp:textbox id="TxtDescripcionAnticipo"  Runat="server" Width="300px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE class="filtersIn">
			<TR>
				<TD><h3>Descuentos Planilla:</h3></TD>
			</TR>
			<TR>
				<TD align="center" class="scrollable">
					<asp:datagrid id="dgrDescuentos" runat="server" ShowFooter="True" AutoGenerateColumns="False">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle  HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle  Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="No." ItemStyle-VerticalAlign="Top"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Doc." ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:TextBox id="txtNumDescuento" runat="server" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>' Width="80px" >
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Descuento" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:dropdownlist id="ddlConceptoDescuento" CssClass="dmediano" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="ValorDescuento" ItemStyle-VerticalAlign="Top">
								<ItemTemplate>
									<asp:textbox id="txtValorDescuento" onkeyup="NumericMask(this)"  Runat="server"
										Width="90px"></asp:textbox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:textbox id="txtTotalDescuentos" ReadOnly="True"  Runat="server" Width="90px"></asp:textbox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Diligencio">
								<ItemTemplate>
									<asp:dropdownlist id="ddlResponsableDescuento" CssClass="dmediano" runat="server"></asp:dropdownlist>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Descripcion ">
								<ItemTemplate>
									<asp:textbox id="txtDescripciondescuento"  Runat="server" Width="300px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
		<TABLE class="filtersIn">
			<TR>
				<TD>
					<asp:label id="Label25" runat="server"  Font-Bold="True">Observacion :</asp:label>                
					<asp:textbox id="txtObservacion" runat="server"  Width="520px" ReadOnly="False" TextMode="MultiLine" Height="48px"></asp:textbox>
                </TD>
			</TR>
		</TABLE>
		<TABLE class="filtersIn" id="tblTotal" >
			<TR>
				<TD>
					<asp:label id="Labe13" runat="server"  Font-Bold="True">Total Planilla :</asp:label></TD>
				<td>
					<asp:label id="lblTotalPlanilla" runat="server" Font-Size="Small" Font-Bold="True" Width="90px"></asp:label></TD>
			</TR>
			<TR>
				<TD vAlign="top">
					<asp:button id="btnPlanillar"  Font-Bold="True" Runat="server" Text="Planillar"></asp:button>
                </TD>
				<TD vAlign="top">
					<asp:button id="btnGrabar"  Font-Bold="True" Width="65px" Runat="server" Text="Registrar"></asp:button>
                </TD>
			</TR>
		</TABLE>
	</asp:panel>
	<script language="javascript">
	var prctGiro=<%=ViewState["PorcentajeGiro"]%>;
	var prctPrepago=<%=ViewState["PorcentajePrepago"]%>;
	var tTiquetes='<%=ViewState["strTotalTiquetes"]%>';
	var tTiquetesEsp='<%=ViewState["strTotalTiquetesEsp"]%>';	
	var tEncomiendas='<%=ViewState["strTotalEncomiendas"]%>';
	var tGiros='<%=ViewState["strTotalGiros"]%>';
	var tCGiros='<%=ViewState["strCostoGiros"]%>';
	var tPagos='<%=ViewState["strTotalPagos"]%>';
	var tDescuentos='<%=ViewState["strTotalDescuentos"]%>';
	var tTiposEsp='<%=ViewState["strTiposTiquetesEsp"]%>';
	var tTiquetesAnul='<%=ViewState["strAnulacionTiquetes"]%>';
	var txtNITConductor=document.getElementById("<%=txtConductor.ClientID%>");
	var txtNombreConductor=document.getElementById("<%=txtConductora.ClientID%>");
	var txtPlaca=document.getElementById("<%=txtPlaca.ClientID%>");
	var txtNumeroBus=document.getElementById("<%=txtPlacaa.ClientID%>");
	var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
	//verTiposEsp();
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
	function totalTicketes(txtNum,txtVal,txtTot){
		var objNum=document.getElementById(txtNum);
		var objVal=document.getElementById(txtVal);
		var objTot=document.getElementById(txtTot);
		tTiq = (parseFloat(objNum.value.replace(/\,/g,'')) * parseFloat(objVal.value.replace(/\,/g,'')));
		parseValor(tTiq,objTot);
		
		totalesPrts(tTiquetes);
		
	}
    function totalTicketesEsp(txtNum,txtVal,txtTot){
		var objNum=document.getElementById(txtNum);
		var objVal=document.getElementById(txtVal);
		var objTot=document.getElementById(txtTot);
		tTiq = parseFloat(objVal.value.replace(/\,/g,''));
		
		tTiq = (tTiq*(100-prctPrepago))/100;
			
		parseValor(tTiq,objTot);
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
	function MostrarPersonalAgencia(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE,PCAR_DESCRIPCION AS CARGO from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO;';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	function MostrarConductor(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODICARGO=\'CO\' order by  MNIT.MNIT_NIT;';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	function MostrarRelevadores(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MRELEVADORES_TRANSPORTES MP  WHERE MP.MNIT_NIT=MNIT.MNIT_NIT AND MP.FECHA_DESDE<=\'<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>\' AND MP.FECHA_HASTA>=\'<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>\' order by  MNIT.MNIT_NIT;';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	<%=strActScript%>
	</script>
	<asp:label id="lblError" Font-Bold="True"  runat="server"></asp:label>

</fieldset>
