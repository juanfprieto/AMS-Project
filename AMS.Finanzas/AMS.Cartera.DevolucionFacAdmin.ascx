<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Cartera.DevolucionFacAdmin.ascx.cs" Inherits="AMS.Finanzas.AMS_Cartera_DevolucionFacAdmin" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language='javascript' src='../js/AMS.Tools.js' type='text/javascript'></script>
<style runat="server">
    #_ctl1_rbFC
    {
        width:3%;
    }
</style>

<fieldset>
    <table>
        <tr>
            <td>
                <asp:radiobutton id="rbFC" onclick="CambiarTipo(this)" Text="Factura Cliente" Runat="server" GroupName="facturas"
		        Checked="True"></asp:radiobutton>
            </td>
            <td>
                <asp:radiobutton id="rbFP" onclick="CambiarTipo(this)" Text="Factura Proveedor" Runat="server" GroupName="facturas"
		        Checked="False"></asp:radiobutton>
            </td>
        </tr>
    </table>
   <%-- <P>
        <asp:radiobutton id="rbFC" onclick="CambiarTipo(this)" Text="Factura Cliente" Runat="server" GroupName="facturas"
		        Checked="True"></asp:radiobutton>
	</P>
    <p>
        <asp:radiobutton id="rbFP" onclick="CambiarTipo(this)" Text="Factura Proveedor" Runat="server" GroupName="facturas"
		        Checked="False"></asp:radiobutton>
    </p>--%>
<table id="Table1" class="filtersIn">
<tbody>
<tr>
<td>

<table>
	<tr>
   <%-- <p>
        Prefijo Factura Administrativa :--%>
		<%--<asp:dropdownlist id="ddlPrefijo" class="dmediano" Runat="server" onChange="CambiarFactura(this)"></asp:dropdownlist>--%>
       <%-- <asp:dropdownlist id="ddlPrefijo" class="dmediano" Runat="server"></asp:dropdownlist>
    </p>--%>

<%--	<p>
        Número Factura Administrativa :
        <asp:TextBox id="txtNumero" class="tpequeno" Runat="server"></asp:TextBox>
    </p>--%>
        <td>
            Prefijo Factura Administrativa :
            <asp:dropdownlist id="ddlPrefijo" class="dmediano" Runat="server"></asp:dropdownlist>
        </td>
        <td>
            Número Factura Administrativa :
            <asp:TextBox id="txtNumero" class="tpequeno" Runat="server" onblur="cargarObs()"></asp:TextBox>
        </td>
	</tr>
    <tr>
        <td>
            Observaciones: <br />
            <asp:TextBox id="txtObservacion" TextMode="multiline"  Rows="3" runat="server" cssClass="amediano"/>
        </td>
    </tr>
</table> 

<P><asp:button id="btnCargar" Text="Cargar Información" runat="server" CausesValidation="False" onclick="btnCargar_Click"></asp:button></P>
<P>
	<table id="tbNotas" runat="server">
		<tr>
			<td>
                Prefijo Nota Devolución:
			</td>
			<td>
                <asp:dropdownlist id="ddlPrefNota" Runat="server"></asp:dropdownlist>
            </td>
		</tr>
		<tr>
			<td>
                <asp:label id="lbCuenta" Runat="server" Visible="False"></asp:label>
            </td>
			<td>
                <asp:textbox id="tbCuenta" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\') order by mcue_codipuc',new Array())"
					Runat="server" Visible="False" Width="128px"></asp:textbox><asp:requiredfieldvalidator id="RFV1" Text="*" Runat="server" ControlToValidate="tbCuenta" Display="Dynamic"></asp:requiredfieldvalidator>
            </td>
		</tr>
        <tr>
        <td>
        <table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 209px; left:550px; top:337px; POSITION: absolute";
            onmouseout="calendar.style.visibility='hidden'">
			<tbody>
				<tr>
					<td><asp:calendar BackColor=Beige id="calDate" runat="server" OnSelectionChanged="ChangeDate" enableViewState="true"></asp:calendar></td>
				</tr>
			</tbody>
		</table>
         Fecha:
        </td>
            <td>
                 <br><asp:textbox id="tbDate" runat="server" class="tpequeno" ReadOnly="True"></asp:textbox><IMG onmouseover="calendar.style.visibility='visible'" src="../img/AMS.Icon.Calendar.gif" border="0">
        <%--    <input type="text" id="fechaNota" class="tpequeno fechaNota" Runat="server" required="true"/>--%>
            </td>
        </tr>
	</table>
</P>
<asp:datagrid id="dgActivos" runat="server" Visible="False" AutoGenerateColumns="False">
	<Columns>
		<asp:BoundColumn DataField="CODIGO ACTIVO FIJO" ReadOnly="True" HeaderText="ACTIVO FIJO"></asp:BoundColumn>
		<asp:BoundColumn DataField="NOMBRE" ReadOnly="True" HeaderText="NOMBRE"></asp:BoundColumn>
		<asp:BoundColumn DataField="VALOR" ReadOnly="True" DataFormatString="{0:C}" HeaderText="VALOR"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Devolver Si/No">
			<ItemStyle HorizontalAlign="Center"></ItemStyle>
			<ItemTemplate>
				<asp:CheckBox id="cbDev" Checked="True" runat="server"></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<P>
          <asp:datagrid id="dgDiferidos" runat="server" Visible="False" AutoGenerateColumns="False">
		<Columns>
			<asp:BoundColumn DataField="CODIGO DIFERIDO" ReadOnly="True" HeaderText="CODIGO DIFERIDO"></asp:BoundColumn>
			<asp:BoundColumn DataField="NOMBRE" ReadOnly="True" HeaderText="NOMBRE"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" ReadOnly="True" DataFormatString="{0:C}" HeaderText="VALOR"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Devolver Si/No">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
				<ItemTemplate>
					<asp:CheckBox id="cbDev" Checked="True" runat="server"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid>
</P>
<P>
            <asp:datagrid id="dgOperativos" runat="server" Visible="False" AutoGenerateColumns="False">
		<Columns>
			<asp:BoundColumn DataField="CODIGO GASTO" ReadOnly="True" HeaderText="CODIGO GASTO"></asp:BoundColumn>
			<asp:BoundColumn DataField="DETALLE" ReadOnly="True" HeaderText="DETALLE"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" ReadOnly="True" DataFormatString="{0:c}" HeaderText="VALOR"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Devolver Si/No">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
				<ItemTemplate>
					<asp:CheckBox id="cbDev" Checked="True" runat="server"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid>
</P>
<P>
     <asp:datagrid id="dgVarios" runat="server" Visible="False" AutoGenerateColumns="False">
		<Columns>
			<asp:BoundColumn DataField="DESCRIPCION" ReadOnly="True" HeaderText="DESCRIPCION"></asp:BoundColumn>
			<asp:BoundColumn DataField="CUENTA" ReadOnly="True" HeaderText="CUENTA"></asp:BoundColumn>
			<asp:BoundColumn DataField="SEDE" ReadOnly="True" HeaderText="SEDE"></asp:BoundColumn>
			<asp:BoundColumn DataField="CENTRO DE COSTO" ReadOnly="True" HeaderText="CENTRO DE COSTO"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO REFERENCIA" ReadOnly="True" HeaderText="PREFIJO REFERENCIA"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERO REFERENCIA" ReadOnly="True" HeaderText="NUMERO REFERENCIA"></asp:BoundColumn>
			<asp:BoundColumn DataField="NIT" ReadOnly="True" HeaderText="NIT"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR DEBITO" ReadOnly="True" DataFormatString="{0:c}" HeaderText="VALOR DEBITO"
				HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR CREDITO" ReadOnly="True" DataFormatString="{0:c}" HeaderText="VALOR CREDITO"
				HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
		</Columns>
	 </asp:datagrid>

</P>

<table id="Table6" class="filtersIn">
    <tr>
     <td class="scrollable">
           <asp:datagrid id="gridNC" runat="server" Visible="False" AutoGenerateColumns="False" HeaderStyle-BackColor="#ccccdd" 
		Font-Size="8pt" Font-Name="Verdana" CellPadding="3" Font-Names="Verdana" ShowFooter="True">
		<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"> </SelectedItemStyle>
		<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
		<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
		<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Descripci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="descripciontxt" Runat="server" Width="200" TextMode="MultiLine"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox ID=tbDesc Runat=server Width=200 TextMode=MultiLine Text='<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cuenta">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="cuentatxt" Runat="server" Width="80" ReadOnly="false" onDblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\') order by mcue_codipuc',new Array())"
						ToolTip="Haga Doble Click para iniciar la busqueda"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox ID=tbCue Runat=server Width=80 Text='<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>' onDblClick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\') order by mcue_codipuc',new Array())" ToolTip="Haga Doble Click para iniciar la busqueda">
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Sede">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "SEDE") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlAlmacen" Runat="server"></asp:DropDownList>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:DropDownList id="ddlAlm" Runat="server"></asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Centro de Costo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CENTROCOSTO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlCentroCosto" Runat="server"></asp:DropDownList>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlCenCos" Runat="server"></asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Prefijo Documento">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="prefijotxt" Runat="server" Width="80" ReadOnly="false" MaxLength="6"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox ID=tbPref Runat=server Width=80 Text='<%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Numero Documento">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="numdocutxt" Runat="server" Width="80" ReadOnly="false" CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox ID=tbNum Runat=server Width=80 Text='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>'>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NIT Beneficiario">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NIT") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="numnittxt" Runat="server" Width="80" ReadOnly="false" onDblCLick="ModalDialog(this,'SELECT mnit_nit AS Nit,mnit_apellidos CONCAT \' \' CONCAT COALESCE(mnit_apellido2,\'\') CONCAT \' \' CONCAT mnit_nombres CONCAT \' \' CONCAT COALESCE(mnit_nombre2,\'\') AS Nombre FROM dbxschema.mnit order by mnit_nit',new Array())"
						ToolTip="Haga Doble Click para iniciar la busqueda"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox ID=tbNit Runat=server Width=80 Text='<%# DataBinder.Eval(Container.DataItem, "NIT") %>' onDblClick="ModalDialog(this,'SELECT mnit_nit AS Nit,mnit_apellidos CONCAT \' \' CONCAT COALESCE(mnit_apellido2,\'\') CONCAT \' \' CONCAT mnit_nombres CONCAT \' \' CONCAT COALESCE(mnit_nombre2,\'\') AS Nombre FROM dbxschema.mnit order by mnit_nit',new Array())">
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Debito">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALORDEBITO","{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="valordebitotxt" Runat="server" Width="80" ReadOnly="false" onkeyup="NumericMaskE(this,event)"
						Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox ID=tbDeb Runat=server Width=80 Text='<%# DataBinder.Eval(Container.DataItem, "VALORDEBITO","{0:N}") %>' onKeyUp=NumericMaskE(this,event) CssClass=AlineacionDerecha>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Credito">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALORCREDITO","{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="valorcreditotxt" Runat="server" Width="80" ReadOnly="false" onkeyup="NumericMaskE(this,event)"
						Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="tbCred" Runat=server Width=80 Text='<%# DataBinder.Eval(Container.DataItem, "VALORCREDITO","{0:N}") %>' onKeyUp=NumericMaskE(this,event) CssClass=AlineacionDerecha>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Base">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALORBASE","{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="valorbasetxt" Runat="server" Width="80" ReadOnly="false" onkeyup="NumericMaskE(this,event)"
						Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox id=tbBas Runat=server Width=80 Text='<%# DataBinder.Eval(Container.DataItem, "VALORBASE","{0:N}") %>' onKeyUp=NumericMaskE(this,event) CssClass=AlineacionDerecha>
					</asp:TextBox>
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Agregar">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
				<ItemTemplate>
					<asp:Button CommandName="RemoverFilas" Text="Remover" ID="btnRem" runat="server" Width="65" />
				</ItemTemplate>
				<FooterStyle HorizontalAlign="Center"></FooterStyle>
				<FooterTemplate>
					<asp:Button CommandName="AgregarFilas" Text="Agregar" ID="btnAdd" runat="server" Width="60" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" HeaderText="Editar/Actualizar" CancelText="Cancelar"
				EditText="Editar"></asp:EditCommandColumn>
            </Columns>
		<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </td>   
  </tr>
</table>


<p style="FONT-WEIGHT: bold; COLOR: red">Si ha realizado cambios en las casillas de 
	chequeo de devolución de las tablas, por favor haga click en el botón 
	Actualizar Información antes de guardar la devolución, esto evitará problemas 
	de devoluciones mal realizadas. Si el botón se encuentra deshabilitado, omita 
	esta información.</p>
<P><asp:button id="btnActualizar" Text="Actualizar Información" runat="server" CausesValidation="False"
		Enabled="False" onclick="btnActualizar_Click"></asp:button></P>

<table style="width:400px">
	<tr>
		<td>Valor de la Factura :
		</td>
		<td align="right"><asp:label id="lbValFac" Text="$0.00" Runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>Valor Fletes :</td>
		<td align="right"><asp:label id="lbValFletes" Text="$0.00" Runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>Valor IVA Fletes :</td>
		<td align="right"><asp:label id="lbIvaFletes" Text="$0.00" Runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>Valor IVA :</td>
		<td align="right"><asp:label id="lbIva" Text="$0.00" Runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>Valor Retenciones :</td>
		<td align="right"><asp:label id="lbRet" Text="$0.00" Runat="server"></asp:label></td>
	</tr>
	<tr>
		<td style="HEIGHT: 26px">Total de la Factura :
		</td>
		<td style="HEIGHT: 26px" align="right"><asp:label id="lbTotFac" Text="$0.00" Runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>Valor Abonos :</td>
		<td align="right"><asp:label id="lbAbon" Text="$0.00" Runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>Saldo :</td>
		<td align="right"><asp:label id="lbSaldo" Text="$0.00" Runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td>Valor a Devolver :</td>
		<td align="right"><asp:textbox id="tbValor" Text="0" Runat="server" CssClass="AlineacionDerecha" ReadOnly="True">0</asp:textbox></td>
	</tr>
	<tr>
		<td>Valor Fletes :</td>
		<td align="right"><asp:textbox id="tbFletes" Text="0" Runat="server" onkeyup="CalcularTotales();NumericMaskE(this,event)"
				CssClass="AlineacionDerecha">0</asp:textbox></td>
	</tr>
	<tr>
		<td>Valor IVA Fletes :</td>
		<td align="right"><asp:textbox id="tbIvaFletes" Text="0" Runat="server" onkeyup="CalcularTotales();NumericMaskE(this,event)" 
            CssClass="AlineacionDerecha" ReadOnly="false">0</asp:textbox></td>
	</tr>
	<tr>
		<td>Valor IVA Devolver:</td>
		<td><asp:textbox id="tbvalIva" runat="server" CssClass="AlineacionDerecha" ReadOnly="True" Placeholder="Agregue el IVA">0</asp:textbox></td>
	</tr>
	<TR>
		<td>Valor Retenciones a Devolver :</td>
		<td><asp:textbox id="tbValRet" Text="0" Runat="server" CssClass="AlineacionDerecha" ReadOnly="True" Placeholder="Agregue las Retenciones">0</asp:textbox></td>
	</TR>
	<tr>
		<td style="HEIGHT: 26px">Total a Devolver :</td>
		<td style="HEIGHT: 26px"><asp:textbox id="tbTotal" Text="0" Runat="server" CssClass="AlineacionDerecha" ReadOnly="True"></asp:textbox></td>
	</tr>
</table>
		
<table>
	<tr>
		<TD align="center">
			<asp:Label id="Label2" Visible="True" Runat="server" Text="Retenciones"></asp:Label>
            <table id="Table7" class="filtersIn">
                <tr>
                    <td class="scrollable">
                        <fieldset>
			<asp:DataGrid id="gridRtns" runat="server"  Visible="True" AutoGenerateColumns="False"
				HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3" Font-Names="Verdana"
				onItemCommand="gridRtns_Item" OnItemDataBound="gridRtns_ItemDataBound" showfooter="True">
				<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
				<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
				<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
				<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
				<Columns>
                    <asp:TemplateColumn HeaderText="Tipo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
						FooterStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "TIPORETE") %>
						</ItemTemplate>
						<FooterTemplate>
							<center>
								<asp:DropDownList id="ddlTiporet"  class="dpequeno" runat="server"></asp:DropDownList>
							</center>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="C&#243;digo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
						FooterStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
						</ItemTemplate>
						<FooterTemplate>
							<center>
								<asp:TextBox id="codret" runat="server" style="width: 50px;" ReadOnly="true" ToolTip="Haga Click"></asp:TextBox>
							</center>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Porcentaje de Retenci&#243;n (%)" HeaderStyle-HorizontalAlign="Center"
						FooterStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:TextBox id="codretb" runat="server" style="width: 70px;" ReadOnly="true" ></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Base">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:TextBox id="base" runat="server" Enabled="true" Text="0" CssClass="AlineacionDerecha"  class="tpequeno"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Valor">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:TextBox id="valor" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha"
								class="tpequeno" ReadOnly="True"></asp:TextBox>
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
				</Columns>
			</asp:DataGrid>
            </fieldset>
         </td>
    </tr>
</table>
			<asp:Label id="Label3" Visible="True" Runat="server" Text="IVA"></asp:Label>
           <table id="Table8" class="filtersIn">
             <tr>
               <td class="scrollable">
                  <fieldset>
			<asp:DataGrid id="dgIva" runat="server"  Visible="True" AutoGenerateColumns="False"
				HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3" Font-Names="Verdana"
				showfooter="True">
				<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
				<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
				<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
				<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Porcentaje de IVA (%)" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE","{0:N}") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:DropDownList ID="ddlPorcIva" Runat="server"></asp:DropDownList>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Base">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:TextBox id="tbValIvaBase" runat="server" Enabled="true" Text="0" CssClass="AlineacionDerecha"
								Width="100"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Valor">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:TextBox id="tbValIvaI" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0"
								CssClass="AlineacionDerecha" Width="100" ReadOnly="True"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Cuenta PUC">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:TextBox id="tbCuentaI" runat="server" Enabled="true" Width="100" onDblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Nit">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "NIT") %>
						</ItemTemplate>
						<FooterTemplate>
							<asp:DropDownList ID="ddlnit" Runat="server"></asp:DropDownList>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tipo de IVA">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "TIPO")%>
						</ItemTemplate>
						<FooterTemplate>
							<asp:RadioButton ID="rbiva1" Runat="server" Text="IVA" GroupName="ivas" Checked="True"></asp:RadioButton>
							<asp:RadioButton ID="rbiva2" Runat="server" Text="IVA Fletes" GroupName="ivas"></asp:RadioButton>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Agregar" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<asp:Button id="btnRemIva" runat="server" CommandName="RemoverIva" Text="Remover" CausesValidation="false" />
						</ItemTemplate>
						<FooterTemplate>
							<asp:Button id="btnAddIva" runat="server" CommandName="AgregarIva" Text="Agregar" Enabled="true"
								CausesValidation="false" />
						</FooterTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
              </fieldset>
         </td>
    </tr>
</table>
</TD>
	</tr>
</table>
<P><div align=center><asp:button id="btnAceptar" Text="Realizar Proceso" runat="server" Enabled="False" onclick="btnAceptar_Click"></asp:button>&nbsp;<asp:button id="btnCancelar" Text="Cancelar" runat="server" CausesValidation="False" onclick="btnCancelar_Click"></asp:button></div></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
</td>
</tr>
</tbody>
</table>
</fieldset>
<script type ='text/javascript'>


    function cargarObs() {
        var prefijo = document.getElementById("<%=ddlPrefijo.ClientID%>").value;
        var numero = document.getElementById("<%=txtNumero.ClientID%>").value;
        AMS_Cartera_DevolucionFacAdmin.cargar_Obs(prefijo, numero, cargar_Obs_CallBack);
    }
    function cargar_Obs_CallBack(response) {
        respuesta = response.value;
        document.getElementById("<%=txtObservacion.ClientID%>").value = respuesta;
    }
    function Cambio_Retencion(objDdlRete, objTxtcodret, tipoSociedad, porcentaje, base, valor, tipo) {

        if (tipo == 'P')
        { 
            $("#" + objTxtcodret.id).click(function () {
                    ModalDialog(objTxtcodret, 'SELECT PR.PRET_CODIGO CODIGO,PR.PRET_NOMBRE NOMBRE,PR.PRET_PORCENNODECL PORCENTAJE,PR.TTIP_PROCESO PROCESO, ' +
                                      'PR.TRET_CODIGO TIPO, PR.MCUE_CODIPUCPROV CUENTA FROM PRETENCION PR WHERE PR.TRET_CODIGO=\'' + objDdlRete.value + '\'  ' +
                                      ' ORDER BY TIPO;', new Array());
            });
        }
        else
        {
            $("#" + objTxtcodret.id).click(function () {
                ModalDialog(objTxtcodret, 'SELECT PR.PRET_CODIGO CODIGO,PR.PRET_NOMBRE NOMBRE,PR.PRET_PORCENNODECL PORCENTAJE,PR.TTIP_PROCESO PROCESO, ' +
                                  'PR.TRET_CODIGO TIPO, PR.MCUE_CODIPUCCLIE CUENTA FROM PRETENCION PR WHERE PR.TRET_CODIGO=\'' + objDdlRete.value + '\'  ' +
                                  ' ORDER BY TIPO;', new Array());
            });
        }

        $("#" + objTxtcodret.id).change(function () {

            PorcentajeVal(porcentaje, base, valor);
        });
    }

	function CambiarTipo(obj)
	{
		AMS_Cartera_DevolucionFacAdmin.CambiarTipoFactura(obj.value,CambiarTipoFactura_Callback);
	}
	
	function CambiarTipoFactura_Callback(response)
	{
		if(response.error!=null)
		{
			alert(response.error);
			return;
		}
		var respuesta=response.value;
		var ddlPref=document.getElementById("<%=ddlPrefijo.ClientID%>");
		ddlPref.options.length=0;
		if(respuesta.Tables.length!=0)
		{
			if(respuesta.Tables[0].Rows.length!=0)
			{
				for(var i=0;i<respuesta.Tables[0].Rows.length;i++)
				{
					ddlPref.options[ddlPref.options.length]=new Option(respuesta.Tables[0].Rows[i].DESCRIPCION,respuesta.Tables[0].Rows[i].PREFIJO);
				}
				CambiarFactura(ddlPref);
			}
			else
				alert('No hay documentos para este prefijo');
		}
	}
	
	function CalcularTotalesIva(obj)
	{
		var valValor=document.getElementById("<%=tbValor.ClientID%>").value;
		var valIva=obj.value;
		var valRet=document.getElementById("<%=tbValRet.ClientID%>").value;
		var objTotal=document.getElementById("<%=tbTotal.ClientID%>");
		if(obj.value=='')
			return;
		NumericMask(obj)
		var valor=parseFloat(EliminarComas(valValor));
		var iva=parseFloat(EliminarComas(valIva));
		var retencion=parseFloat(EliminarComas(valRet));
		objTotal.value=String(valor+iva-retencion);
		ApplyNumericMask(objTotal);
		objTotal.value="$"+objTotal.value
	}
	
	function CalcularTotalesRetenciones(obj)
	{
		var valValor=document.getElementById("<%=tbValor.ClientID%>").value;
		var valIva=document.getElementById("<%=tbvalIva.ClientID%>").value;
		var valRet=obj.value; 
		var objTotal=document.getElementById("<%=tbTotal.ClientID%>");
		if(obj.value=='')
			return;
		NumericMask(obj)
		var valor=parseFloat(EliminarComas(valValor));
		var iva=parseFloat(EliminarComas(valIva));
		var retencion=parseFloat(EliminarComas(valRet));
		objTotal.value=String(valor+iva-retencion);
		ApplyNumericMask(objTotal);
		objTotal.value="$"+objTotal.value
	}
	
	function CalcularTotales()
	{
		var valValor=document.getElementById("<%=tbValor.ClientID%>").value;
		var valFlete=document.getElementById("<%=tbFletes.ClientID%>").value;
		var valIvaFlete=document.getElementById("<%=tbIvaFletes.ClientID%>").value;
		var valIva  =document.getElementById("<%=tbvalIva.ClientID%>").value;
		var valRet  =document.getElementById("<%=tbValRet.ClientID%>").value;
		var objTotal=document.getElementById("<%=tbTotal.ClientID%>");

		var valor   =parseFloat(EliminarComas(valFlete));
		var flete   =parseFloat(EliminarComas(valValor));
		var ivaFlete=parseFloat(EliminarComas(valIvaFlete));
		var iva     =parseFloat(EliminarComas(valIva));
		var retencion=parseFloat(EliminarComas(valRet));

		objTotal.value=String(valor+iva+ivaFlete+flete-retencion);
		ApplyNumericMask(objTotal);
		objTotal.value="$"+objTotal.value
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
