<%@ Control Language="c#" codebehind="AMS.Contabilidad.CompGrid.ascx.cs" autoeventwireup="True" ClassName="Comprobante" Inherits="AMS.Contabilidad.CompGrid" %>
<script type ="text/javascript" src="../js/AMS.Contabilidad.Comprobante.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
 <script type="text/javascript" src="../js/jquery-ui.js"></script>
<script type="text/javascript">
    $(document).ready(function ()
    {
        if ($('#' + '<%= chkDocumentos.ClientID %>').prop('checked'))
        {
            $('#' + '<%= divDocumentos.ClientID %>').show("fast");
        } else
            $('#' + '<%= divDocumentos.ClientID %>').hide();

        $('#' + '<%= chkDocumentos.ClientID %>').click(function ()
        {
            if ($('#' + '<%= chkDocumentos.ClientID %>').prop('checked'))
            {
                $('#' + '<%= divDocumentos.ClientID %>').show("fast");
            }
            else
                $('#' + '<%= divDocumentos.ClientID %>').toggle("fast");
        });
    });
    
    /*function mostrarDocu()
    {
        var checking = document.getElementById('');
        var divsito = document.getElementById('');
        if (checking.checked) {
            divsito.style.visibility = 'visible';
        } else
            divsito.style.visibility = 'hidden';
    }*/
</script>
<p>
	<table style="vertical-align: bottom">
		<tbody>
			<tr>
				<td>
					<fieldset>
						<legend>Comprobante</legend>
						<p>&nbsp;<asp:label id="Label1" runat="server">Tipo :</asp:label><asp:label id="lbRef" runat="server">lbRef</asp:label>
						</p>
						<p>Número:
							<asp:textbox id="idComp" runat="server" class="tpequeno"></asp:textbox></p>
					</fieldset>
				</td>
				<td>
					<fieldset>
						<legend>Razón</legend>
						<p><asp:textbox id="tbDetail" runat="server"></asp:textbox></p>
					</fieldset>
				</td>
				<td>
					<fieldset>
						<legend>Fecha</legend>
						<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:label id="lbDate" runat="server"></asp:label></p>
					</fieldset>
				</td>
			</tr>
		</tbody>
    </table>
</p>

<P>
<p></p>
<fieldset>
    <span style="CURSOR: pointer; COLOR: royalblue; TEXT-DECORATION: none" onclick="document.getElementById('divContainerExcelOption').style.display='block';">Mostrar Herramienta Carga Excel</span>
    <div id="divContainerExcelOption" style="DISPLAY: none">
        <legend>Opción de Carga desde Archivo Excel</legend>
		<table class="tablewhite" cellSpacing="1" cellPadding="1" border="0">
			<tr>
			<tr>
				<td align="right" colSpan="2"><span style="CURSOR: pointer; COLOR: royalblue; TEXT-DECORATION: none" onclick="document.getElementById('divContainerExcelOption').style.display='none';">Ocultar</span></td>
			<tr>
				<td colSpan="2">Por favor genere un archivo de excel con las siguientes columnas<br>
					<li>
					    CUENTA : Código de la cuenta del PUC.
					<LI>
					    NIT : Nit de tercero relacionado.
					<LI>
					    PREF : Prefijo documento relacionado. (Hasta 6 caracteres)
					<LI>
					    COMPROB : Número de documento relacionado.
					<LI>
					    DETALLE : Detalle del documento. (no digite comas)
					<LI>
					    SEDE : Código de la sede o almacén. (debe existir en el parámetro de sedes)
					<LI>
					    CCOSTOS : Código del centro de costos. (debe existir en el parámetro de centros de costo)
					<LI>
					    DEBITO : valor débito de renglón (valor numérico obligatorio sin separadores de 
					    mil , ni signos pesos) .
					<LI>
					    CREDITO : valor crédito de renglón (valor numérico obligatorio sin 
					    separadores de mil , ni signos pesos).
					<LI>
						BASE : valor base de renglón (valor numérico obligatorio sin separadores de mil, ni signos pesos).
					<LI>
					    DEBITONIIF : valor débito NIIF de renglón (valor numérico obligatorio sin separadores de mil , ni signos pesos).
					<LI>
					    CREDITONIIF : valor crédito NIIF de renglón (valor numérico obligatorio sin separadores de mil , ni signos pesos).<br>
					    Notas :
					<li>
					    El primer renglón del archivo debe llevar los titulos de las columnas como se encuentran listados anteriormente.
					<LI>
					    Los campos numéricos no deben llevar separadores de miles ni signo de pesos Y Ningún campo puede ser vacio.
					<LI>
					    Debe seleccionar todo el espacio de comprobante y asignarle&nbsp;el nombre COMPROBANTE.
					<LI>
					    Solo utilizar Excel 2010 en adelante(formato XLSX)
					<LI>
					    Los valores de cuenta, nit, sede y centro costos deben estar registrados en la base de datos Tal Cual.
					<LI>
						Revisar los valores que se cargan de forma detenida, para verificar la veracidad de la información.</LI>
                </td>
			</tr>
			<tr>
				<td width="697">
					<input id="flArchivoExcel" runat="server" type="file"/></td>
				<td align="right">
					<asp:Button id="btnCargar" runat="server" Width="327px" Text="Cargar" onclick="btnCargar_Click"></asp:Button></td>
			</tr>
		</table>
    </div>
</fieldset>
<p><ASP:DATAGRID id="dgInserts" runat="server" cssclass="datagrid" OnCancelCommand="DgInserts_Cancel" OnUpdateCommand="DgInserts_Update"
		OnEditCommand="DgInserts_Edit" OnItemCommand="DgInserts_AddAndDel" OnItemDataBound="DgInserts_ItemDataBound"
		CellPadding="3" ShowFooter="True" AutoGenerateColumns="false">
		<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
		<FooterStyle cssclass="footer"></FooterStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Cuenta">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "cuenta", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox style="name:val1" id="valToInsert1" ReadOnLy="false" runat="server" Width="100px"
						ondblclick="ModalDialog(this, 'SELECT mcue_codipuc as CODIGO, mcue_nombre as NOMBRE FROM mcuenta where timp_codigo IN (\'A\',\'P\') ORDER BY mcue_codipuc', new Array())"></asp:TextBox>
					<asp:TextBox id="valToInsert1a" ReadOnLy="false" runat="server" Width="100px"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_1" onclick="ModalDialog(this, 'SELECT mcue_codipuc as CODIGO, mcue_nombre as NOMBRE FROM mcuenta where timp_codigo IN (\'A\',\'P\') ORDER BY mcue_codipuc', new Array(),null,null,1)" ReadOnLy="true" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "cuenta") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="NIT">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "nit", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert2" ReadOnLy="false" runat="server" Width="100px" ondblclick="ModalDialog(this, 'SELECT mnit_nit as NIT, mnit_nombres concat \' \' concat mnit_apellidos as Nombre FROM mnit', new Array(),1)"></asp:TextBox>
					<asp:TextBox id="valToInsert2a" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_2" onclick="ModalDialog(this, 'SELECT mnit_nit as NIT, mnit_nombres concat \' \' concat mnit_apellidos as Nombre FROM mnit', new Array())" ReadOnLy="true" width="100" Text='<%# DataBinder.Eval(Container.DataItem, "nit") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="Pref.">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "pref", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert3" onclick="PutDatas(this, 'docType')" runat="server" Width="50px"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_3" onclick="PutDatas(this, 'docType')" width="50px" Text='<%# DataBinder.Eval(Container.DataItem, "pref") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="#Doc. Ref.">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "docref", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert4" onclick="PutDatas(this, 'numRef')" runat="server" Width="50px"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_4" onclick="PutDatas(this, 'numRef')" width="50px" Text='<%# DataBinder.Eval(Container.DataItem, "docref") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="Detalle">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "detalle", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert5" onclick="PutDatas(this, 'detail')" runat="server" Width="100px"></asp:TextBox>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_5" onclick="PutDatas(this, 'detail')" width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "detalle") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="Sede">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "sede", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="ddlSede" class="dpequeno" runat="server"></asp:DropDownList>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:DropDownList id="ddlSedeEdicion" runat="server"></asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="Centro Costo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ccosto", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="ddlCC" class="dpequeno" runat="server"></asp:DropDownList>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:DropDownList id="ddlCCEdicion" runat="server"></asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="D&#233;bito">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "debito", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert8" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" runat="server"
						Width="80px"></asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator2" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert8"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_8" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" width="80px" Text='<%# DataBinder.Eval(Container.DataItem, "debito", "{0:N}") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator1" ASPClass="RegularExpressionValidator" ControlToValidate="edit_8"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="Cr&#233;dito">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "credito", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert9" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" runat="server"
						Width="80px"></asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator4" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert9"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_9" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" width="80px" Text='<%# DataBinder.Eval(Container.DataItem, "credito", "{0:N}") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator3" ASPClass="RegularExpressionValidator" ControlToValidate="edit_9"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="Base">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "base", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert10" runat="server" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha"
						Width="80px"></asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator6" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert10"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_10" width="80px" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" Text='<%# DataBinder.Eval(Container.DataItem, "base", "{0:N}") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator5" ASPClass="RegularExpressionValidator" ControlToValidate="edit_10"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>
            			
			<asp:TemplateColumn HeaderText="DébitoNiif">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "debitoNiif", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert11" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" runat="server"
						Width="80px"></asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator8" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert11"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_11" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" width="80px" Text='<%# DataBinder.Eval(Container.DataItem, "debitoNiif", "{0:N}") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator7" ASPClass="RegularExpressionValidator" ControlToValidate="edit_11"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="CréditoNiif">
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "creditoNiif", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert12" onkeyup="NumericMaskE(this,event)"  runat="server" CssClass="AlineacionDerecha"
						Width="80px"></asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator10" ASPClass="RegularExpressionValidator" ControlToValidate="valToInsert12"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="edit_12" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" width="80px" Text='<%# DataBinder.Eval(Container.DataItem, "creditoNiif", "{0:N}") %>' />
					<asp:RegularExpressionValidator id="RegularExpressionValidator9" ASPClass="RegularExpressionValidator" ControlToValidate="edit_12"
						ValidationExpression="[0-9\-\.\,]+" Display="Dynamic" Font-Name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
				</EditItemTemplate>
			</asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Operaciones">
				<ItemTemplate>
					<asp:Button CommandName="DelDatasRow" Text="Borrar" ID="btnDel" Runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar" onmouseover="focus(); checkBase()" ID="btnAdd"
						Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>

			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
		</Columns>
		<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
	</ASP:DATAGRID></p>
<p><asp:label id="lbInfo" runat="server"></asp:label></p>
<p><input id="tbBase" type="hidden"/>
</p>
<p></p>

<fieldset>
	<p></p>
	<legend>Totales</legend>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>Débito:
				</td>
				<td><asp:textbox id="tbDebito" runat="server" Width="77px" readonly="true">0</asp:textbox></td>
                <td><asp:textbox id="tbDebitoNiif" runat="server" Width="77px" readonly="true">0</asp:textbox></td>
			</tr>
			<tr>
				<td>Crédito:
				</td>
				<td vAlign="middle"><asp:textbox id="tbCredito" runat="server" Width="77px" readonly="true">0</asp:textbox></td>
                <td vAlign="middle"><asp:textbox id="tbCreditoNiif" runat="server" Width="77px" readonly="true">0</asp:textbox></td>
			</tr>
			<tr>
				<td>Diferencia:&nbsp;&nbsp;
				</td>
				<td><asp:textbox id="tbDiferencia" runat="server" Width="77px" readonly="true">0</asp:textbox></td>
                <td><asp:textbox id="tbDiferenciaNiif" runat="server" Width="77px" readonly="true">0</asp:textbox></td>
			</tr>
			<tr>
				<td><asp:button id="Button1" onclick="RecordComp" runat="server" Text="Grabar" Enabled="False" OnClientClick="espera();" ></asp:button>&nbsp;&nbsp;
					<asp:button id="Button2" onclick="CancelComp" runat="server" Text="Cancelar"></asp:button>&nbsp;&nbsp;
					<asp:Button id="Button3" runat="server" Text="Validar" Enabled="False" onclick="Button3_Click"></asp:Button></td>
                <td></td>
			</tr>
            <tr>
                <td width="697"><br /><br />
                    <asp:CheckBox id="chkDocumentos" runat="server" AutoPostBack="false" Text="Deseo agregar archivos de soporte!"/>
                    <div id="divDocumentos" runat="server">
                        <asp:Label runat="server" Text="A continuación, podrá adjuntar documentos de Soporte" Font-Size="16px"></asp:Label><br />
					    <input id="uploadDocCont" runat="server" type="file" multiple=""/> 
					    <asp:Button id="btnDocContables" runat="server" onclick="cargarDocsContables" Text="Cargar Documentos de soporte" OnClientClick="noEspera();"></asp:Button>
                        <br />
                        Usted ha agregado lo siguientes archivos:
	                    <asp:Label id="lbArchivos" runat="server" forecolor="MidnightBlue"></asp:Label>
                    </div>
                    
				</td>
				<%--<td>
                    <asp:button id="btnDocContables" runat="server" Text="Adjuntar documentos contables" OnClientClick="noEspera();" ></asp:button>
				</td>--%>
			</tr>
		</tbody>
    </table>
</fieldset>
<p></p>


