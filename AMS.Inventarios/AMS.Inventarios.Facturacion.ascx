<%@ Control Language="c#" codebehind="AMS.Inventarios.Facturacion.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.Facturacion" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript">
    
        $(function () {
        //mirar documentacion JQueryUI sobre datepicker
                var fechaVal = $("#<%=tbDate.ClientID%>").val();
        $("#<%=tbDate.ClientID%>").datepicker();
        $("#<%=tbDate.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=tbDate.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=tbDate.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=tbDate.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=tbDate.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=tbDate.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=tbDate.ClientID%>").val(fechaVal);
    });
    function CalculoIva(obValFlt, obCmbIVA, obValIVAFlt ,obTotFl, obTotRep, obTotal)
    {
        if(obValFlt.value.length > 0)
        {
            //NumericMask(obValFlt);
            var stringValorFletes = EliminarComas(obValFlt.value);
            var stringValorRepuestos = EliminarComas(document.getElementById(obTotRep).value.substring(1, document.getElementById(obTotRep).value.length));
            var valorFletes = parseFloat(stringValorFletes);
            var valorIva = parseFloat(obCmbIVA.value);
            var valorRepuestos = parseFloat(stringValorRepuestos);
            obTotFl.value = String(valorFletes);
            obValIVAFlt.value = String(valorFletes*(valorIva/100));
            ApplyNumericMask(obTotFl);
            ApplyNumericMask(obValIVAFlt);
            obTotFl.value = '$'+obTotFl.value;
            obValIVAFlt.value = '$'+obValIVAFlt.value;
            document.all[obTotal].value = String(valorRepuestos + (valorFletes + (valorFletes*(valorIva/100))));
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = '$'+document.all[obTotal].value;
        }
        else
        {
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorRepuestos = parseFloat(stringValorRepuestos);
            document.all[obTotal].value = String(valorRepuestos);
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = '$'+document.all[obTotal].value;
            obTotFl.value = '$0';
            obValIVAFlt.value = '$0';
        }
    }

</script>
<script language="javascript">
    function CambioIva(obValFlt, obCmbIVA, obValIVAFlt, obTotFl, obTotRep, obTotal)
    {
        if(obValFlt.value.length > 0)
        {
            var stringValorFletes = EliminarComas(obValFlt.value);
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorFletes = parseFloat(stringValorFletes);
            var valorIva = parseFloat(obCmbIVA.value);
            var valorRepuestos = parseFloat(stringValorRepuestos);
            obTotFl.value = String(valorFletes);
            obValIVAFlt.value = String(valorFletes*(valorIva/100));
            ApplyNumericMask(obTotFl);
            ApplyNumericMask(obValIVAFlt);
            obTotFl.value = '$'+obTotFl.value;
            obValIVAFlt.value = '$'+obValIVAFlt.value;
            document.all[obTotal].value = valorRepuestos + (valorFletes + (valorFletes*(valorIva/100)));
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = '$'+document.all[obTotal].value;
        }
        else
        {
            var stringValorRepuestos = EliminarComas(document.all[obTotRep].value.substring(1,document.all[obTotRep].value.length));
            var valorRepuestos = parseFloat(stringValorRepuestos);
            document.all[obTotal].value = String(valorRepuestos);
            ApplyNumericMask(document.all[obTotal]);
            document.all[obTotal].value = '$'+document.all[obTotal].value;
            obTotFl.value = '$0';
            obValIVAFlt.value = '$0';
        }
    }

</script>
<fieldset>    
  <table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<p>NIT:<asp:textbox id="txtNIT" runat="server" ReadOnLy="true"></asp:textbox>
					<asp:textbox id="txtNITa" runat="server"  ReadOnLy="true"></asp:textbox>
					<asp:textbox id="txtAlm" runat="server" ReadOnLy="true" Visible="false"></asp:textbox></p>
			</td>
			<td >Fecha:
				<asp:textbox id="tbDate" runat="server" Width="78px" ReadOnly="True"></asp:textbox>              
			</td>
		</tr>
		<tr>
			<td>Almacén :
				<asp:dropdownlist id="ddlAlmacen" runat="server" Enabled="False" OnSelectedIndexChanged="ddlAlmacen_OnSelectedIndexChanged"></asp:dropdownlist></td>
			<td>Vendedor : &nbsp;<asp:dropdownlist id="ddlVendedor" runat="server" AutoPostBack="True"></asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td>
				<p align="left">Cod. Documento:&nbsp;<asp:dropdownlist id="ddlCodDoc" 
                        runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlCodDoc_SelectedIndexChanged"></asp:dropdownlist>
					<br>
					Num. Documento:&nbsp;&nbsp;<asp:textbox id="txtNumDoc" runat="server" ReadOnly="True"  Width="100px"></asp:textbox>
				</p>
			</td>
			<td>
				<p>Días de plazo:&nbsp;
					<asp:textbox id="txtDiasP" runat="server" CssClass="AlineacionDerecha">0</asp:textbox><br>
					&nbsp;
				</p>
			</td>
		</tr>
		<tr>
			<td colSpan="2">Observaciones:&nbsp;<asp:textbox id="txtObs" runat="server" Width="584px" MaxLength="100" Rows="5"></asp:textbox>
			</td>
		</tr>
	</tbody>
 </table>

<br>
<table style="BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; BACKGROUND-COLOR: transparent; BORDER-BOTTOM-STYLE: none">
	<tbody>
		<tr>
			<td><ASP:DATAGRID id="dgItems" runat="server" cssclass="datagrid" CellPadding="3" ShowFooter="False" 
					GridLines="Vertical" BorderWidth="1px" AutoGenerateColumns="False" OnItemCommand="dgItems_ItemCommand">
					<FooterStyle cssclass="footer"></FooterStyle>
					<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
					<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
					<ItemStyle cssclass="item"></ItemStyle>
					<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="mite_codigo" ReadOnly="True" HeaderText="REFERENCIA"></asp:BoundColumn>
						<asp:BoundColumn DataField="mite_nombre" ReadOnly="True" HeaderText="NOMBRE"></asp:BoundColumn>
						<asp:BoundColumn DataField="plin_codigo" ReadOnly="True" HeaderText="LINEA"></asp:BoundColumn>
						<ASP:TemplateColumn HeaderText="CANTIDAD FACTURADA" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:TextBox ID=tbcantfac Runat=server Text='<%# DataBinder.Eval(Container.DataItem,"mite_cantfac","{0:N}")%>' Width="80px" onkeyup="NumericMaskE(this,event)" CssClass=AlineacionDerecha>
								</asp:TextBox>
							</ItemTemplate>
						</ASP:TemplateColumn>
						<asp:BoundColumn DataField="mite_precio" ReadOnly="True" HeaderText="PRECIO" DataFormatString="{0:C4}"></asp:BoundColumn>
						<asp:BoundColumn DataField="mite_iva" ReadOnly="True" HeaderText="IVA" DataFormatString="{0:N}%"></asp:BoundColumn>
						<ASP:TemplateColumn HeaderText="DESCUENTO" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:TextBox ID=tbdesc Runat=server Text='<%# DataBinder.Eval(Container.DataItem,"mite_desc","{0:N}")%>' Width="60px" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha">
								</asp:TextBox>
							</ItemTemplate>
						</ASP:TemplateColumn>
						<asp:BoundColumn DataField="mite_tot" ReadOnly="True" HeaderText="TOTAL" DataFormatString="{0:C}"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PPED_CODIGO" ReadOnly="True" HeaderText="PREFIJO PEDI" DataFormatString="{0:C}"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MPED_NUMEPEDI" ReadOnly="True" HeaderText="NUM PEDI" DataFormatString="{0:C}"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="ACCIONES" FooterStyle-HorizontalAlign="Center" Visible="False">
							<FooterTemplate>
								<asp:Button CommandName="Actualizar" Text="Actualizar" Runat="server" Width="80px" ID="Button1"></asp:Button>
								<asp:Button CommandName="ResetRows" Text="Recargar" ID="btnClear" Runat="server" width="80px" />
							</FooterTemplate>
						</asp:TemplateColumn>
						<ASP:EditCommandColumn></ASP:EditCommandColumn>
					</Columns>
				</ASP:DATAGRID></td>
		</tr>
		<tr>
			<td><asp:button id="btnAct" runat="server" Text="Actualizar" ToolTip="Se actualizaran los valores modificados en la grilla" onclick="btnAct_Click"></asp:button>&nbsp;
				<asp:button id="btnRec" runat="server" Text="Recargar" ToolTip="Se dejara la grilla en su estado original" onclick="btnRec_Click"></asp:button></td>
		</tr>
	</tbody>
</table>
<P style="COLOR: red">Atención : Los cambios realizados en los campos cantidad 
	facturada y descuento no tomaran efecto hasta que de click en el boton 
	Actualizar</P>
<table style="WIDTH: 384px; HEIGHT: 56px">
	<tbody>
		<td>
			<td>
				<asp:Label>Valor Fletes:</asp:Label>
			</td>
			<td>
				<p align="right"><asp:textbox id="txtFlet" runat="server" Width="100px" CssClass="AlineacionDerecha">0</asp:textbox></p>
			</td>
		<td>
		</td>
			<td>
				&nbsp;&nbsp;<asp:Label>%IVA de &nbsp;&nbsp;Fletes:</asp:Label>
			</td>
			<td>
				<p align="right"><asp:dropdownlist id="ddlPIVA" runat="server"></asp:dropdownlist></p>
			</td>
		</tr>
	</tbody>
</table>
<p>
	<table style="WIDTH: 578px; HEIGHT: 114px">
		<tbody>
			<tr>
				<td>
					<p>Subtotal :
					</p>
				</td>
				<td align="right"><asp:textbox id="txtSubTotal" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
				<td>
					<p>&nbsp;&nbsp;Numero Items:
					</p>
				</td>
				<td align="right"><asp:textbox id="txtNumItem" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
				<td>
					<p>Descuento :
					</p>
				</td>
				<td align="right"><asp:textbox id="txtDesc" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
			</tr>
			<tr>
			    <td>
					<p>&nbsp;&nbsp;Numero Unidades:
					</p>
				</td>
				<td align="right"><asp:textbox id="txtNumUnid" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
				<td>
					<p>IVA Items :
					</p>
				</td>
				<td align="right"><asp:textbox id="txtIVA" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
				<td>
					<p>&nbsp;&nbsp;Valor Fletes :
					</p>
				</td>
				<td align="right"><asp:textbox id="txtTotIF" runat="server" ReadOnly="True" CssClass="AlineacionDerecha">$0</asp:textbox></td>
			</tr>
			<tr>
				<td>
					<p>Total :
					</p>
				</td>
				<td align="right"><asp:textbox id="txtTotal" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:textbox></td>
				<td>&nbsp;&nbsp;Valor Iva Fletes :
				</td>
				<td align="right"><asp:textbox id="tbIvaFlts" runat="server" ReadOnly="True" CssClass="AlineacionDerecha">$0</asp:textbox></td>
				<td>
					<p>Gran Total :
					</p>
				</td>
				<td align="right"><asp:textbox id="txtGTot" runat="server" ReadOnly="True" AutoPostBack="True" CssClass="AlineacionDerecha"></asp:textbox></td>
			</tr>
		</tbody>
	</table>
</fieldset> 
</p>
<p><asp:button id="btnAjus" onclick="NewAjust" runat="server" AutoPostBack="True" Text="Facturar" >
</asp:button><INPUT id="hdCargoTrans" type="hidden" runat="server">
	&nbsp;&nbsp;
</p>
<p>
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p>


