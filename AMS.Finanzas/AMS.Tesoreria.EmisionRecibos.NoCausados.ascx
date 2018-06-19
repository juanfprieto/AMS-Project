<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EmisionRecibos.NoCausados.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.NoCausados" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<p>
	<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
	<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
	<table id="Table1" class="filtersIn">
    <tr>

    <td class="scrollable">
    <fieldset>
        <table>
            <tr>
                <td>
                    <asp:PlaceHolder ID="plcExcelImporter" runat="server"></asp:PlaceHolder>
                </td>
                <td style="vertical-align: initial;">
                    <asp:Button ID="btnVincularExcel" runat="server" OnClick="VincularExcel" Text="Vincular Datos en Tabla" Visible = "false"/>        
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblInforme" runat="server" Text=""></asp:Label>    
                </td>
            </tr>
        </table>
    </fieldset>

    <fieldset>
    <asp:DataGrid id="gridNC" runat="server" cssclass="datagrid" onItemCommand="gridNC_Item" OnCancelCommand="gridNC_Cancel" CellPadding="3"
		AutoGenerateColumns="False"	ShowFooter="True" OnItemDataBound="gridNC_ItemDataBound" OnEditCommand="gridNC_Edit" OnUpdateCommand="gridNC_Update">
		<FooterStyle cssclass="datagrid"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Descripci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_descripcion"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>' />
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="descripciontxt" Runat="server" Width="200" TextMode="MultiLine"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cuenta">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_cuenta"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>' onDblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"/>
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="cuentatxt" Runat="server" Width="80" ReadOnly="false" onDblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
						ToolTip="Haga Doble Click para iniciar la busqueda"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Sede">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "SEDE") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_sede"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "SEDE") %>' onDblclick="ModalDialog(this,'SELECT palm_almacen AS Almacen,palm_descripcion AS Descripcion FROM palmacen where pcen_centcart is not null or pcen_centteso is not null order by palm_descripcion;',new Array())"/>
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="almacentxt" Runat="server" Width="80" ReadOnly="false" onDblclick="ModalDialog(this,'SELECT palm_almacen AS Almacen,palm_descripcion AS Descripcion FROM palmacen where pcen_centcart is not null or pcen_centteso is not null order by palm_descripcion;',new Array())"
						ToolTip="Haga Click"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Centro de Costo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CENTROCOSTO") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_centrocosto"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "CENTROCOSTO") %>' onDblclick="ModalDialog(this,'SELECT pcen_codigo AS Codigo,pcen_nombre AS Nombre FROM pcentrocosto where timp_codigo <> \'N\' order by 1',new Array())"/>
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="centrocostotxt" Runat="server" Width="80" ReadOnly="false" onDblclick="ModalDialog(this,'SELECT pcen_codigo AS Codigo,pcen_nombre AS Nombre FROM pcentrocosto where timp_codigo <> \'N\' order by 1',new Array())"
						ToolTip="Haga Click"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Prefijo Documento">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_prefijo"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>' />
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="prefijotxt" Runat="server" Width="80" ReadOnly="false" MaxLength="6"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Numero Documento" ItemStyle-HorizontalAlign="Right">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_numero"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>' />
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="numdocutxt" runat="server" Width="80" MaxLength="8" ReadOnly="false"
						CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NIT Beneficiario">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NIT") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_nit"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "NIT") %>' onDblCLick="ModalDialog(this,'SELECT mnit_nit AS Nit,mnit_apellidos CONCAT \' \' CONCAT COALESCE(mnit_apellido2,\'\') CONCAT \' \' CONCAT mnit_nombres CONCAT \' \' CONCAT COALESCE(mnit_nombre2,\'\') AS Nombre FROM dbxschema.mnit order by mnit_nit',new Array())"/>
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="numnittxt" Runat="server" Width="115px" style="height:30px;" ReadOnly="false" onDblCLick="ModalDialog(this,'SELECT mnit_nit AS Nit,mnit_apellidos CONCAT \' \' CONCAT COALESCE(mnit_apellido2,\'\') CONCAT \' \' CONCAT mnit_nombres CONCAT \' \' CONCAT COALESCE(mnit_nombre2,\'\') AS Nombre FROM dbxschema.mnit order by mnit_nit',new Array())"
						ToolTip="Haga Doble Click para iniciar la busqueda"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Debito" ItemStyle-HorizontalAlign="Right">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALORDEBITO","{0:C}") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_valordebito"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "VALORDEBITO","{0:C}") %>' />
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="valordebitotxt" Runat="server" Width="80" ReadOnly="false" onkeyup="NumericMaskE(this,event)"
						Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Credito" ItemStyle-HorizontalAlign="Right">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALORCREDITO","{0:C}") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_valorcredito"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "VALORCREDITO","{0:C}") %>' />
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="valorcreditotxt" Runat="server" Width="80" ReadOnly="false" onkeyup="NumericMaskE(this,event)"
						Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Base" ItemStyle-HorizontalAlign="Right">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALORBASE","{0:C}") %>
				</ItemTemplate>
                <EditItemTemplate>
				    <asp:TextBox runat="server" id="edit_valorbase"  CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "VALORBASE","{0:C}") %>' />
			    </EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="valorbasetxt" Runat="server" Width="80" ReadOnly="false" onkeyup="NumericMaskE(this,event)"
						Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Agregar" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Button CommandName="RemoverFilas" Text="Remover" ID="btnRem" runat="server" Width="65" />
                    <asp:Button CommandName="CopiarFilas" Text="Copiar" ID="btnCop" runat="server"  Width="65"  />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AgregarFilas" Text="Agregar" ID="btnAdd" runat="server" Width="75px" style="height:30px;"/>
				</FooterTemplate>
			</asp:TemplateColumn>
            <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
		</Columns>
	</asp:DataGrid>
    </fieldset>
    </td>
    </tr>
</table>
</p>
<p>
	<table style="COLOR: navy; BACKGROUND-COLOR: transparent">
		<tbody>
			<tr>
				<td>
					Total Débitos :
				</td>
				<td>
					<asp:TextBox id="totalDNC" runat="server" ReadOnly="true" CssClass="AlineacionDerecha"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Total Créditos :
				</td>
				<td>
					<asp:TextBox id="totalCNC" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td align="center">
					<asp:Button id="aceptar" onclick="aceptar_Click" runat="server" Text="Aceptar"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
</p>
