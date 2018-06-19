<%@ Control Language="c#" codebehind="AMS.Automotriz.ProcesosLiquidacion.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.ProcesosLiquidacion" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script src="../js/AMS.Web.ModalDialog.js" type="text/javascript" ></script>
<script src="../js/AMS.Web.Masks.js" type="text/javascript" ></script>
<script src="../js/AMS.Tools.js" type="text/javascript" ></script>
<script type="text/javascript" >
function ReCalcularTotalIVAOperaciones(/*textbox*/objInner,/*string*/nac)
{
	if(objInner.value == '')
		return;
	NumericMask(objInner);
	var valorInicialOperaciones = parseFloat(document.getElementById("<%=hdValNetOper.ClientID%>").value);
	var valorInicialIvaOperaciones = parseFloat(document.getElementById("<%=hdValIVAOper.ClientID%>").value);
	var valorDescuento = parseFloat(EliminarComas(objInner.value));
	if((valorInicialOperaciones) >= valorDescuento)
	{
		if(nac=='E')
		{
			document.getElementById("<%=tbValNetOper.ClientID%>").value = valorInicialOperaciones - valorDescuento;
			ApplyNumericMask(document.getElementById("<%=tbValNetOper.ClientID%>"));
			document.getElementById("<%=tbValNetOper.ClientID%>").value = "$"+document.getElementById("<%=tbValNetOper.ClientID%>").value;
		}
		else
		{
			var porcentajeDescuento = ((valorDescuento*100)/(valorInicialOperaciones));
			var varValorDescuento = ((valorInicialOperaciones)*porcentajeDescuento)/100;
			var varValorDescuentoIVA = ((valorInicialIvaOperaciones)*porcentajeDescuento)/100;
			document.getElementById("<%=tbValNetOper.ClientID%>").value = valorInicialOperaciones - varValorDescuento;
			ApplyNumericMask(document.getElementById("<%=tbValNetOper.ClientID%>"));
			document.getElementById("<%=tbValNetOper.ClientID%>").value = "$"+document.getElementById("<%=tbValNetOper.ClientID%>").value;
			document.getElementById("<%=tbValIVAOper.ClientID%>").value = valorInicialIvaOperaciones - varValorDescuentoIVA;
			ApplyNumericMask(document.getElementById("<%=tbValIVAOper.ClientID%>"));
			document.getElementById("<%=tbValIVAOper.ClientID%>").value = "$"+document.getElementById("<%=tbValIVAOper.ClientID%>").value;
		}
	}
	else
	{
		objInner.value = "0";
		document.getElementById("<%=tbValNetOper.ClientID%>").value = valorInicialOperaciones;
		ApplyNumericMask(document.getElementById("<%=tbValNetOper.ClientID%>"));
		document.getElementById("<%=tbValNetOper.ClientID%>").value = "$"+document.getElementById("<%=tbValNetOper.ClientID%>").value;
		document.getElementById("<%=tbValIVAOper.ClientID%>").value = valorInicialIvaOperaciones;
		ApplyNumericMask(document.getElementById("<%=tbValIVAOper.ClientID%>"));
		document.getElementById("<%=tbValIVAOper.ClientID%>").value = "$"+document.getElementById("<%=tbValIVAOper.ClientID%>").value;
		alert('Se ha superado el valor total de operaciones y no se puede realizar el descuento');
	}
	ReCalcularTotal();
}

function ReCalcularTotalIVARepuestos(/*textbox*/objInner,/*string*/nac)
{
	if(objInner.value == '')
		return;
	NumericMask(objInner);
	var valorInicialRepuestos = parseFloat(document.getElementById("<%=hdValNetItem.ClientID%>").value);
	var valorInicialIvaRepuestos = parseFloat(document.getElementById("<%=hdValIVAItem.ClientID%>").value);
	var valorDescuento = parseFloat(EliminarComas(objInner.value));
	var valorBaseIva = parseFloat(document.getElementById("<%=hdValBase.ClientID%>").value);
	if((valorInicialRepuestos) >= valorDescuento)
	{
		if(nac=='E')
		{
			document.getElementById("<%=tbValNetItem.ClientID%>").value = valorInicialRepuestos - valorDescuento;
			ApplyNumericMask(document.getElementById("<%=tbValNetItem.ClientID%>"));
			document.getElementById("<%=tbValNetItem.ClientID%>").value = "$"+document.getElementById("<%=tbValNetItem.ClientID%>").value;
		}
		else
		{
			<% bool varLiquidacionProcesosDOS = false;
                if (!varLiquidacionProcesosDOS){ %>
				//Logica correcta
				var porcentajeDescuento = ((valorDescuento*100)/(valorInicialRepuestos));
				var varValorDescuento = ((valorInicialRepuestos)*porcentajeDescuento)/100;
				var varValorDescuentoIVA = ((valorInicialIvaRepuestos)*porcentajeDescuento)/100;
				document.getElementById("<%=tbValNetItem.ClientID%>").value = valorInicialRepuestos - varValorDescuento;
				ApplyNumericMask(document.getElementById("<%=tbValNetItem.ClientID%>"));
				document.getElementById("<%=tbValNetItem.ClientID%>").value = "$"+document.getElementById("<%=tbValNetItem.ClientID%>").value;
				document.getElementById("<%=tbValIVAItem.ClientID%>").value = valorInicialIvaRepuestos - varValorDescuentoIVA;
				ApplyNumericMask(document.getElementById("<%=tbValIVAItem.ClientID%>"));
				document.getElementById("<%=tbValIVAItem.ClientID%>").value = "$"+document.getElementById("<%=tbValIVAItem.ClientID%>").value;				
			<%}
			else{%>
				//Logica sistemas DOS
				var porcentajeDescuento;
				var varValorDescuento;
				var varValorDescuentoIVA;
				if(valorBaseIva>0){
					porcentajeDescuento = ((valorDescuento*100)/(valorBaseIva));
					varValorDescuento = ((valorBaseIva)*porcentajeDescuento)/100;
					varValorDescuentoIVA = ((valorInicialIvaRepuestos)*porcentajeDescuento)/100;}
				else{
					porcentajeDescuento = ((valorDescuento*100)/(valorInicialRepuestos));
					varValorDescuento = ((valorInicialRepuestos)*porcentajeDescuento)/100;
					varValorDescuentoIVA = ((valorInicialIvaRepuestos)*porcentajeDescuento)/100;
				}
				document.getElementById("<%=tbValNetItem.ClientID%>").value = valorInicialRepuestos - varValorDescuento;
				ApplyNumericMask(document.getElementById("<%=tbValNetItem.ClientID%>"));
				document.getElementById("<%=tbValNetItem.ClientID%>").value = "$"+document.getElementById("<%=tbValNetItem.ClientID%>").value;
				document.getElementById("<%=tbValIVAItem.ClientID%>").value = valorInicialIvaRepuestos - varValorDescuentoIVA;
				ApplyNumericMask(document.getElementById("<%=tbValIVAItem.ClientID%>"));
				document.getElementById("<%=tbValIVAItem.ClientID%>").value = "$"+document.getElementById("<%=tbValIVAItem.ClientID%>").value;				
			<%}%>
		}
	}
	else
	{
		objInner.value = "0";
		document.getElementById("<%=tbValNetItem.ClientID%>").value = valorInicialRepuestos;
		ApplyNumericMask(document.getElementById("<%=tbValNetItem.ClientID%>"));
		document.getElementById("<%=tbValNetItem.ClientID%>").value = "$"+document.getElementById("<%=tbValNetItem.ClientID%>").value;
		document.getElementById("<%=tbValIVAItem.ClientID%>").value = valorInicialIvaRepuestos;
		ApplyNumericMask(document.getElementById("<%=tbValIVAItem.ClientID%>"));
		document.getElementById("<%=tbValIVAItem.ClientID%>").value = "$"+document.getElementById("<%=tbValIVAItem.ClientID%>").value;
		alert('Se ha superado el valor total de operaciones y no se puede realizar el descuento');
	}
	ReCalcularTotal();
}

function ReCalcularTotal(/*string*/nac)
{
	var valorRep,valorRepIva,valorOper,valorOperIva;
	if(nac=='E')
	{
		valorRep = parseFloat(EliminarComas(document.getElementById("<%=tbValNetItem.ClientID%>").value.substring(1,document.getElementById("<%=tbValNetItem.ClientID%>").value.length)));
		valorOper = parseFloat(EliminarComas(document.getElementById("<%=tbValNetOper.ClientID%>").value.substring(1,document.getElementById("<%=tbValNetOper.ClientID%>").value.length)));
		document.getElementById("<%=tbTotal.ClientID%>").value = String(valorRep+valorOper);
		ApplyNumericMask(document.getElementById("<%=tbTotal.ClientID%>"));
		document.getElementById("<%=tbTotal.ClientID%>").value = "$"+document.getElementById("<%=tbTotal.ClientID%>").value;
	}
	else
	{
		valorRep = parseFloat(EliminarComas(document.getElementById("<%=tbValNetItem.ClientID%>").value.substring(1,document.getElementById("<%=tbValNetItem.ClientID%>").value.length)));
		valorRepIva = parseFloat(EliminarComas(document.getElementById("<%=tbValIVAItem.ClientID%>").value.substring(1,document.getElementById("<%=tbValIVAItem.ClientID%>").value.length)));
		valorOper = parseFloat(EliminarComas(document.getElementById("<%=tbValNetOper.ClientID%>").value.substring(1,document.getElementById("<%=tbValNetOper.ClientID%>").value.length)));
		valorOperIva = parseFloat(EliminarComas(document.getElementById("<%=tbValIVAOper.ClientID%>").value.substring(1,document.getElementById("<%=tbValIVAOper.ClientID%>").value.length)));
		document.getElementById("<%=tbTotal.ClientID%>").value = String(valorRep+valorRepIva+valorOper+valorOperIva);
		ApplyNumericMask(document.getElementById("<%=tbTotal.ClientID%>"));
		document.getElementById("<%=tbTotal.ClientID%>").value = "$"+document.getElementById("<%=tbTotal.ClientID%>").value;
	}
}

function CambioPrefFC()
{
	var prefFact=document.getElementById("<%=ddlPrefFact.ClientID%>").value
	var valor = ProcesosLiquidacion.CambioPrefFC(prefFact).value;
	document.getElementById('<%=tbNumFact.ClientID%>').value = valor;
}

function PorcentajeVal(tPorcentaje,tBase,tTotal){
		var txtT=document.getElementById(tTotal);
		try{
			var prct=parseFloat(document.getElementById(tPorcentaje).value.replace(/\,/g,''));
			var bse=parseFloat(document.getElementById(tBase).value.replace(/\,/g,''));
			var pt=Math.round((prct*bse)/100);
			txtT.value=formatoValor(pt);
		}
		catch(err){
			txtT.value="";
		}
}
</script>
<asp:placeholder id="plInfoOT" runat="server">
	<FIELDSET>
        <LEGEND class="Legends">Orden de Trabajo</LEGEND>
		<TABLE class="filtersIn" cellspacing="5">
			<TR>
				<TD>Prefijo Orden de Trabajo :&nbsp;
					<asp:Label id="lbPrefOT" runat="server"></asp:Label>&nbsp;</TD>
				<TD colspan="2">Número de Orden de Trabajo :&nbsp;
					<asp:Label id="lbNumOT" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Nit Cliente&nbsp;:
					<asp:Label id="lbNitCliente" runat="server"></asp:Label></TD>
				<TD colspan="2">Nombre Cliente :
					<asp:Label id="lbNomCliente" runat="server"></asp:Label></TD>
			</TR>
			<tr>
			    <td></td>
			    <td colspan="2">
			        Nombre Contacto :
					<asp:Label id="lbNomContacto" runat="server"></asp:Label>
			    </td>
			</tr>
			<TR>
				<TD colspan="2">Cargo Principal Orden de Trabajo :
					<asp:Label id="lbCrgPrnOT" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD colspan="2">Otros Cargos Relacionados Orden de Trabajo :
					<asp:DropDownList id="ddlCrgsRels" runat="server"></asp:DropDownList></TD>
				<TD align="right">
					<asp:Button id="btnLiqCrg" onclick="LiquidarCargo" runat="server" Text="Facturar Cargo" CausesValidation="False" UseSubmitBehavior="false" ></asp:Button></TD>
			</TR>
		</TABLE>
	</FIELDSET>
</asp:placeholder><asp:placeholder id="plInfoFC" runat="server">
	<FIELDSET>
        <LEGEND class="Legends">Información Factura</LEGEND>
		<TABLE class="filtersIn">
			<TR>
				<TD width="50%">Cargo a Facturar :
					<asp:Label id="lbCrgLiqu" runat="server"></asp:Label></TD>
				<TD width="50%">Fecha Factura : <IMG onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'" 
						src="../img/AMS.Icon.Calendar.gif" border="0">
					<TABLE id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
						onmouseout="calendar.style.visibility='hidden'  ">
						<TR>
							<TD>
								<asp:calendar BackColor="Beige" id="fechaFactura" runat="server" Enabled="false" ></asp:Calendar></TD>
						</TR>
					</TABLE>
				</TD>
			</TR>
			<TR>
				<TD width="50%">Prefijo Factura :
					<asp:DropDownList id="ddlPrefFact" runat="server" Width="300px" OnChange="CambioPrefFC();"></asp:DropDownList></TD>
				<TD width="50%">Número Factura :
					<asp:TextBox id="tbNumFact" runat="server" Width="74px" CssClass="AlineacionDerecha"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorTbNumFact" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="tbNumFact">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorTbNumFact2" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="tbNumFact" ValidationExpression="[0-9]+" ASPClass="RegularExpressionValidator">*</asp:RegularExpressionValidator></TD>
			</TR>
			<TR>
				<TD width="50%">Nit Factura :
					<asp:TextBox id="tbNitCli" runat="server" Width="131px" ReadOnly="true"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorTbNitCli" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="tbNitCli">*</asp:RequiredFieldValidator></TD>
				<TD width="50%">Nombre Cliente :
					<asp:TextBox id="tbNitClia" runat="server" Width="186px" ReadOnly="true"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD width="50%">Recepcionista :
					<asp:DropDownList id="ddlRecep" runat="server" Enabled="false"></asp:DropDownList></TD>
				<TD width="50%">Mercaderista :
				    <asp:DropDownList id="ddlVendedor" runat="server" Enabled="true"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD colSpan="2">Días Plazo :
					<asp:TextBox id="tbDiasVig" runat="server" Width="77px" CssClass="AlineacionDerecha"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorTbDiasVig" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="tbDiasVig">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorTbDiasVig2" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
						ControlToValidate="tbDiasVig" ValidationExpression="[0-9]+" ASPClass="RegularExpressionValidator">*</asp:RegularExpressionValidator></TD>
			</TR>
			<TR>
				<TD colSpan="2">Observaciones :
					<asp:TextBox id="tbObsr" runat="server" Width="509px" TextMode="MultiLine"></asp:TextBox></TD>
			</TR>
		</TABLE>
	</FIELDSET>
	<asp:PlaceHolder id="plInfoFCAseg" runat="server">
		<FIELDSET>
            <LEGEND class="Legends">Información Específica para Aseguradora
			</LEGEND>
			<TABLE class="filtersIn">
				<TR>
					<TD>Nit Aseguradora :
					</TD>
					<TD align="right">
						<asp:TextBox id="tbNitAseg" onclick="ModalDialog(this, 'SELECT mnit_nit AS NIT, mnit_nombres CONCAT \' \' CONCAT mnit_apellidos AS NOMBRE FROM mnit ORDER BY mnit_nit', new Array())"
							runat="server" CssClass="AlineacionDerecha" ReadOnly="true"></asp:TextBox>
						<asp:RequiredFieldValidator id="validatorTbNitAseg" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
							ControlToValidate="tbNitAseg">*</asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD>Porcentaje Mínimo Deducible Cliente :
					</TD>
					<TD align="right">
						<asp:TextBox id="tbPorDedCli" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
						<asp:RequiredFieldValidator id="validatorTbPorDedCli" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
							ControlToValidate="tbPorDedCli">*</asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="validatorTbPorDedCli2" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
							ControlToValidate="tbPorDedCli" ValidationExpression="[0-9\,\.]+" ASPClass="RegularExpressionValidator">*</asp:RegularExpressionValidator>
					</TD>
					<td rowspan="2">
					    <asp:Label id="lbDeduccion" runat="server" forecolor="RoyalBlue"></asp:Label>
					</td>
				</TR>
				<TR>

					<TD>Valor Mínimo Deducible Cliente:</TD>
					<TD align="right">
						<asp:TextBox id="tbDedOT" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
						<asp:RequiredFieldValidator id="validatorTbDedOT" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
							ControlToValidate="tbDedOT">*</asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="validatorTbDedOT2" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
							ControlToValidate="tbDedOT" ValidationExpression="[0-9\,\.]+" ASPClass="RegularExpressionValidator">*</asp:RegularExpressionValidator></TD>
				</TR>
				<TR>
					<TD>Valor Mínimo Deducible Suministros :
					</TD>
					<TD align="right">
						<asp:TextBox id="tbDedSum" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
						<asp:RequiredFieldValidator id="validatorTbDedSum" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
							ControlToValidate="tbDedSum">*</asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="validatorTbDedSum2" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
							ControlToValidate="tbDedSum" ValidationExpression="[0-9\,\.]+" ASPClass="RegularExpressionValidator">*</asp:RegularExpressionValidator></TD>
				    <td>
				        <asp:Button id="btnActDed" onclick="calculoValorADeducirLbl" runat="server" Text="Calcular"></asp:Button>
				    </td>
				</TR>
			</TABLE>
		</FIELDSET>
	</asp:PlaceHolder>
    <asp:PlaceHolder ID="plhRetenciones" runat="server">
        <fieldset>
            <legend class="Legends">Retenciones</legend>
            <asp:DataGrid id="gridRtns" runat="server" showfooter="True" OnItemDataBound="gridRtns_ItemDataBound"
						onItemCommand="gridRtns_Item" Font-Names="Verdana" CellPadding="3" Font-Name="Verdana" Font-Size="8pt"
						HeaderStyle-BackColor="#ccccdd" AutoGenerateColumns="False" OnCancelCommand="dgrItems_Cancel"
						OnUpdateCommand="dgrItems_Update" OnEditCommand="dgrItems_Edit">
						<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="C&#243;digo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
								</EditItemTemplate>
								<FooterTemplate>
									<center>
										<asp:TextBox id="codret" runat="server" ReadOnly="true" Width="70" ToolTip="Haga Click"></asp:TextBox>
									</center>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Nombre Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</EditItemTemplate>
								<FooterTemplate>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Tipo Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TRET_NOMBRE") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TRET_NOMBRE") %>
								</EditItemTemplate>
								<FooterTemplate>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Tipo Proceso" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TTIP_PROCESO") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TTIP_PROCESO") %>
								</EditItemTemplate>
								<FooterTemplate>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Tipo Persona" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TTIP_NOMBRE") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TTIP_NOMBRE") %>
								</EditItemTemplate>
								<FooterTemplate>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Porcentaje de Retenci&#243;n (%)" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="codretb" runat="server" ReadOnly="true" Width="60"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Base">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
								</ItemTemplate>
								<EditItemTemplate>
								<asp:TextBox id="txtEdV" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:#,###.##}") %>' CssClass="AlineacionDerecha"
										Width="100"></asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="base" runat="server" Enabled="true" Text="0" CssClass="AlineacionDerecha" Width="100"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
								</ItemTemplate>
								<EditItemTemplate>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="valor" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha"
										Width="100" ReadOnly="True"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Agregar" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<asp:Button id="remRet" runat="server" CommandName="RemoverRetencion" Text="Remover" CausesValidation="false" />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button id="agRet" runat="server" CommandName="AgregarRetencion" Text="Agregar" Enabled="true"
										CausesValidation="false" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
						</Columns>
					</asp:DataGrid>
        </fieldset>
    </asp:placeholder>
	<FIELDSET>
        <LEGEND class="Legends">Información Elementos OT</LEGEND>
		<TABLE class="filtersIn">
			<TR>
				<TD >
					<asp:Label id="lbInfoOper" runat="server" forecolor="RoyalBlue">Información Operaciones</asp:Label>
					<asp:DataGrid id="dgInfoOper" runat="server" CssClass="main" Font-Name="Verdana"
						Font-Size="8pt" AutoGenerateColumns="False" Font-Names="Verdana" GridLines="Vertical" BorderStyle="None"
						BackColor="White" BorderColor="#999999" CellPadding="3" HeaderStyle-BackColor="#ccccdd" BorderWidth="1px">
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO OPERACI&#211;N"></asp:BoundColumn>
							<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCI&#211;N OPERACI&#211;N"></asp:BoundColumn>
							<asp:BoundColumn DataField="MECANICO" HeaderText="MECANICO"></asp:BoundColumn>
							<asp:BoundColumn DataField="EXCENCION" HeaderText="EXC. IVA" DataFormatString="{0:c}"></asp:BoundColumn>
							<asp:BoundColumn DataField="TIEMPO" HeaderText="TIEMPO" DataFormatString="{0:N}"></asp:BoundColumn>
							<asp:BoundColumn DataField="VALOROPERACION" HeaderText="VALOR OPERACI&#211;N" DataFormatString="{0:c}"></asp:BoundColumn>
						</Columns>
					</asp:DataGrid></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbInfoItems" runat="server" forecolor="RoyalBlue">Información Items</asp:Label>
					<asp:DataGrid id="dgInfoItems" runat="server" CssClass="main" Font-Name="Verdana"
						Font-Size="8pt" AutoGenerateColumns="False" Font-Names="Verdana" GridLines="Vertical" BorderStyle="None"
						BackColor="White" BorderColor="#999999" CellPadding="3" HeaderStyle-BackColor="#ccccdd" BorderWidth="1px">
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO ITEM"></asp:BoundColumn>
							<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCI&#211;N ITEM"></asp:BoundColumn>
							<asp:BoundColumn DataField="VALOR" HeaderText="VALOR ITEM" DataFormatString="{0:c}"></asp:BoundColumn>
							<asp:BoundColumn DataField="IVA" HeaderText="IVA" DataFormatString="{0:n}%"></asp:BoundColumn>
							<asp:BoundColumn DataField="CANTIDAD" HeaderText="CANTIDAD" DataFormatString="{0:n}"></asp:BoundColumn>
							<asp:BoundColumn DataField="TOTAL" HeaderText="TOTAL" DataFormatString="{0:c}"></asp:BoundColumn>
							<asp:BoundColumn DataField="TRANSFERENCIA" HeaderText="TRANSFERENCIA RELACIONADA"></asp:BoundColumn>
						</Columns>
					</asp:DataGrid></TD>
			</TR>
		</TABLE>
	</FIELDSET>
	<DIV>
		<FIELDSET>
            <LEGEND class="Legends">Información Costo Factura</LEGEND>
			<TABLE class="filtersIn">
				<tr>
				    <td>
				    </td>
				    <td align="center">Sin Descuento
				    </td>
				    <td align="center">Con Descuento
				    </td>
				</tr>
				<TR>
					<TD>Valor Neto Operaciones :
					</TD>
					<TD align="right">
						<asp:TextBox id="tbValNetOper" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox><INPUT id="hdValNetOper" type="hidden" name="hdValNetOper" runat="server">
					</TD>
					<TD align="right">
						<asp:TextBox id="tbValNetOperDescuento" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox>
					</TD>
				</TR>
				<TR>
					<TD>Valor Iva Operaciones :</TD>
					<TD align="right">
						<asp:TextBox id="tbValIVAOper" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox><INPUT id="hdValIVAOper" type="hidden" name="hdValIVAOper" runat="server">
					</TD>
					<TD align="right">
						<asp:TextBox id="tbValIVAOperDescuento" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox>
					</TD>
				</TR>
				<TR>
					<TD>Valor Neto Repuestos :</TD>
					<TD align="right">
						<asp:TextBox id="tbValNetItem" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox><INPUT id="hdValNetItem" type="hidden" name="hdValNetItem" runat="server">
					</TD>
					<TD align="right">
						<asp:TextBox id="tbValNetItemDescuento" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox>
					</TD>
				</TR>
				<TR>
					<TD>Valor Iva Repuestos :
					</TD>
					<TD align="right">
						<asp:TextBox id="tbValIVAItem" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox><INPUT id="hdValIVAItem" type="hidden" name="hdValIVAItem" runat="server">
					</TD>
					<TD align="right">
						<asp:TextBox id="tbValIVAItemDescuento" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox>
					</TD>
				</TR>
				<TR>
					<TD>Total :
					</TD>
					<TD align="right">
						<asp:TextBox id="tbTotal" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox><INPUT id="hdValBase" type="hidden" name="hdValBase" runat="server">
					</TD>
					<TD align="right">
						<asp:TextBox id="tbTotalDescuento" runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:TextBox>
					</TD>
				</TR>
			</TABLE>
		</FIELDSET>
		<FIELDSET>
            <LEGEND class="Legends">Discriminación valor de operaciones</LEGEND>
			<TABLE class="filtersIn">
				<TR>
					<TD>Valor Operaciones de Electricidad :
					</TD>
					<TD align="right">
						<asp:Label id="lbElectricidad" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Valor Operaciones de Electronica :</TD>
					<TD align="right">
						<asp:Label id="lbElectronica" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Valor Operaciones de Latoneria :</TD>
					<TD align="right">
						<asp:Label id="lbLatoneria" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Valor Operaciones de Mano de obra mecanica :
					</TD>
					<TD align="right">
						<asp:Label id="lbManodeObra" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Valor Operaciones de Pintura :
					</TD>
					<TD align="right">
						<asp:Label id="lbPintura" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Valor Operaciones de Terceros :
					</TD>
					<TD align="right">
						<asp:Label id="lbTerceros" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Valor Operaciones de Tapiceria y Vidrios :
					</TD>
					<TD align="right">
						<asp:Label id="lbTapiceria" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Valor Operaciones de Trabajos Varios :
					</TD>
					<TD align="right">
						<asp:Label id="lbTrabajosVarios" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD>Descuento en Operaciones (antes de IVA):
					</TD>
					<TD align="right">
						<asp:TextBox id="tbDescuentoOperaciones" runat="server" CssClass="AlineacionDerecha"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD>Descuento en Respuestos (antes de IVA):
					</TD>
					<TD align="right">
						<asp:TextBox id="tbDescuentoRespuestos" runat="server" CssClass="AlineacionDerecha"></asp:TextBox></TD>
				</TR>
				<tr><td></td>
				    <td><asp:Button id="btnActDesc" onclick="CambioDescuento" runat="server" Text="Actualizar Descuentos"></asp:Button>
				    </td>
				</tr>
				<TR>
					<TD>Autoriza Descuento :
					</TD>
					<TD align="right">
						<asp:TextBox id="tbDescuentoAutoriza" runat="server" CssClass="AlineacionDerecha" TextMode="MultiLine"></asp:TextBox></TD>
				</TR>
				<TR>
				    <TD>Le explicaron la cuenta al cliente?</TD>
				    <TD><asp:RadioButtonList ID="rdbExplica" runat="server">
				            <asp:ListItem Text="Si" Value="S" Selected="True"></asp:ListItem>
				            <asp:ListItem Text="No" Value="N"></asp:ListItem>
				        </asp:RadioButtonList></TD>
				</TR>
			</TABLE>
		</FIELDSET>
	</DIV>
	<P>
		<asp:Button id="btnFctCrg" onclick="FacturarCargo" runat="server" Text="Facturar Cargo"></asp:Button></P>
</asp:placeholder><asp:placeholder id="plhInfoFactAsegu" runat="server">
	<FIELDSET>
        <LEGEND class="Legends">Información facturas generadas por cargo Aseguradora</LEGEND>
		<TABLE class="filtersIn">
			<TR>
				<TD>Orden de trabajo :
				</TD>
				<TD align="right">
					<asp:Label id="lbOrdenTrabajo" Runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Facturas Generadas :
				</TD>
				<TD align="right">
					<asp:DropDownList id="ddlFacturasGeneradas" Runat="server" OnSelectedIndexChanged="CambioFacturaAseguradora"
						AutoPostBack="true"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>Información Factura :
				</TD>
				<TD>
					<asp:Label id="lbInfoFactura" Runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2">
					<asp:Button id="btnIrVistaImpresion" onclick="IrVistaImpresion" Text="Ir a Vista Impresión"
						Runat="server"></asp:Button></TD>
			</TR>
		</TABLE>
	</FIELDSET>
</asp:placeholder>
<p><asp:label id="lb" runat="server"></asp:label></p>
<p><asp:linkbutton id="lnkVolver" onclick="Volver" runat="server" CausesValidation="False">Volver</asp:linkbutton></p>



