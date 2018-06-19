<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.CausacionFacturasAdministrativas.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.CausacionFacturaAdministrativa" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language='javascript' src='../js/AMS.Tools.js' type='text/javascript'></script>

<fieldset >
    <legend>
        Escoja el tipo de Factura que desea realizar
	</legend>
    <table id="Table1" class="filtersIn">
        <tbody>
            <tr>
                <td>
                    <asp:radiobuttonlist id="tipoFactura" Width="313px" onSelectedIndexChanged="Escoger_Factura" AutoPostBack="True" RepeatDirection="Horizontal" runat="server">
			            <asp:ListItem Value="FC">Factura de Cliente</asp:ListItem>
			            <asp:ListItem Value="FP">Factura de Proveedor</asp:ListItem>
		            </asp:radiobuttonlist>
                </td>
            </tr>
        </tbody>
    </table>
</fieldset>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>

<fieldset id="fldGenerales" runat="server" visible="false">
    <table>
        <tbody>
            <tr>
                <asp:panel id="pnlDatos" runat="server">
	                <P>Datos Generales de la Factura</P>
	                <P>
		                <asp:PlaceHolder id="encabezadoCliente" runat="server"></asp:PlaceHolder>
	                </P>
	                <P>
		                <asp:PlaceHolder id="encabezadoProveedor" runat="server"></asp:PlaceHolder>
	                </P>
                </asp:panel>
            </tr>
        </tbody>
    </table>
</fieldset>
<p></p>
<p>
    <asp:panel id="pnlDet" runat="server" Visible="False">
		<P>Detalle de la Factura</P>
        <table id="Table3" class="filtersIn">
            <tr>
                <td class="scrollable">
                    <fieldset id="fldActivosFijos" runat="server">
		                <asp:DataGrid id="gridActivosFijos" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False" CellPadding="3" ShowFooter="True">
			                <FooterStyle cssclass="footer"></FooterStyle>
			                <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			                <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
			                <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			                <ItemStyle cssclass="item"></ItemStyle>
			                <Columns>
				                <asp:TemplateColumn HeaderText="C&#243;digo Gasto">
					                <ItemTemplate>
						                <%# DataBinder.Eval(Container.DataItem, "CODIGOGASTO") %>
					                </ItemTemplate>
					                <FooterTemplate>
						                <center>
							                <asp:TextBox id="gastoText" runat="server" ReadOnly="true" onClick="ModalDialog(this,'SELECT P.ptip_codigo AS Codigo,P.ptip_descripcion AS Descripcion FROM ptipogasto P,ttipogasto T WHERE P.ttip_codigo=T.ttip_codigo AND T.ttip_codigo=1',new Array(),1)" ToolTip="Haga Click" Width="50" />
						                </center>
					                </FooterTemplate>
				                </asp:TemplateColumn>
				                <asp:TemplateColumn HeaderText="C&#243;digo Activo">
					                <ItemTemplate>
						                <%# DataBinder.Eval(Container.DataItem, "CODIGOACTIVO") %>
					                </ItemTemplate>
					                <FooterTemplate>
						                <center>
							                <asp:TextBox id="activoText" runat="server" ReadOnly="true" ToolTip="Haga Click" Width="110px" />
						                </center>
					                </FooterTemplate>
				                </asp:TemplateColumn>
				                <asp:TemplateColumn HeaderText="Nombre">
					                <ItemTemplate>
						                <%# DataBinder.Eval(Container.DataItem, "NOMBREACTIVO") %>
					                </ItemTemplate>
					                <FooterTemplate>
						                <asp:TextBox id="activoText1" runat="server" ReadOnly="true" />
					                </FooterTemplate>
				                </asp:TemplateColumn>
				                <asp:TemplateColumn HeaderText="Valor">
					                <ItemTemplate>
						                <%# DataBinder.Eval(Container.DataItem, "VALORACTIVO") %>
					                </ItemTemplate>
					                <FooterTemplate>
						                <asp:TextBox id="activoText2" runat="server" Text="0"  vCssClass="AlineacionDerecha" />
					                </FooterTemplate>
				                </asp:TemplateColumn>
				                <asp:TemplateColumn HeaderText="Agregar/Eliminar">
					                <ItemTemplate>
						                <center>
							                <asp:Button id="remover" runat="server" CommandName="Remover_Gasto" Text="Remover" CausesValidation="false" />
						                </center>
					                </ItemTemplate>
					                <FooterTemplate>
						                <center>
							                <asp:Button id="agregar" runat="server" CommandName="Agregar_Gasto" Text="Agregar" CausesValidation="false" />
						                </center>
					                </FooterTemplate>
				                </asp:TemplateColumn>
			                </Columns>
		                </asp:DataGrid>
                </fieldset>
            </td>
        </tr>
    </table>
		<p></p>
    <table id="Table4" class="filtersIn">
          <tr>
           <td class="scrollable">
            <fieldset id="fldDiferidos" runat="server" visible="false">
		<asp:DataGrid id="gridDiferidos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3" ShowFooter="True">
			<FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="C&#243;digo Gasto">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CODIGOGASTO") %>
					</ItemTemplate>
					<FooterTemplate>
						<center>
							<asp:TextBox id="gastoText" runat="server" ReadOnly="true" onClick="ModalDialog(this,'SELECT P.ptip_codigo AS Codigo,P.ptip_descripcion AS Descripcion FROM ptipogasto P,ttipogasto T WHERE P.ttip_codigo=T.ttip_codigo',new Array(),1)" ToolTip="Haga Click" Width="50" />
						</center>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Código Diferido">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CODIGODIF") %>
					</ItemTemplate>
					<FooterTemplate>
						<center>
							<asp:TextBox id="difText" runat="server" ReadOnly="true" onClick="ModalDialog(this,'SELECT MDIF_CODIDIFE,MDIF_NOMBDIFE,MDIF_VALOHIST FROM MDIFERIDO',new Array(),1)" ToolTip="Haga Click" Width="50" />
						</center>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Nombre">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "NOMBREDIF") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="difText1" runat="server" ReadOnly="true" />
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "VALORDIF") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="difText2" runat="server" Text="0" ReadOnly="true" CssClass="AlineacionDerecha" />
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Agregar/Eliminar">
					<ItemTemplate>
						<center>
							<asp:Button id="remover" runat="server" CommandName="Remover_Gasto" Text="Remover" CausesValidation="false" />
						</center>
					</ItemTemplate>
					<FooterTemplate>
						<center>
							<asp:Button id="agregar" runat="server" CommandName="Agregar_Gasto" Text="Agregar" CausesValidation="false" />
						</center>
					</FooterTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
        </fieldset>
    </td>
    </tr>
</table>
		<P></P>
    <table id="Table5" class="filtersIn">
    <tr>
     <td class="scrollable">
 <fieldset id="fldDetalles" runat="server" visible="false">
		<asp:DataGrid id="gridDetalles" runat="server" AutoGenerateColumns="False" CellPadding="3" ShowFooter="True">
			<FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="C&#243;digo Gasto">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CODIGOGASTO") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="gastoText" runat="server" ReadOnly="true" onClick="ModalDialog(this,'SELECT P.ptip_codigo AS Codigo,P.ptip_descripcion AS Descripcion FROM ptipogasto P,ttipogasto T WHERE P.ttip_codigo=T.ttip_codigo AND T.ttip_codigo=3',new Array(),1)" ToolTip="Haga Click" Width="50" />
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Detalle">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "DETALLE") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="detalleText" runat="server" ReadOnly="false" TextMode="Multiline" />
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Documento Referencia">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "DOCUREFE") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="docuText" runat="server" ReadOnly="false" MaxLength="10" />
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "VALORDETALLE") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="opTextb" runat="server" Text="0" ReadOnly="false" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" />
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Agregar/Eliminar">
					<ItemTemplate>
						<center>
							<asp:Button id="remover" runat="server" CommandName="Remover_Gasto" Text="Remover" CausesValidation="false" />
						</center>
					</ItemTemplate>
					<FooterTemplate>
						<center>
							<asp:Button id="agregar" runat="server" CommandName="Agregar_Gasto" Text="Agregar" CausesValidation="false" />
						</center>
					</FooterTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
                      </fieldset>
    </td>
    </tr>
</table>
		<asp:Label id="lbInfoImp" Runat="server"></asp:Label>
        <br />
        <fieldset>
            <table>
                <tr>
                    <td style="width:60%">
                        <asp:PlaceHolder ID="plcExcelImporter" runat="server"></asp:PlaceHolder>
                        <asp:Button ID="btnVincularExcel" runat="server" OnClick="VincularExcel" Text="Vincular Datos en Tabla" Visible = "false"/>
                    </td>
                    <td>
                        <asp:Button ID="btnCargaGastos" runat="server" OnClick="cargarValesCMenor" Text="Cargar Vales Caja Menor" Visible = "false" style="position:relative; margin-left: auto; margin-bottom: auto; vertical-align:top"/>
                        <asp:Label ID="lbAlmacen" runat="server" style="font-size: large;"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
		<p>
         <table id="Table2" class="filtersIn">
              <tr>
                <td class="scrollable">
                    <fieldset id="fldNC" runat="server" visible="false">
			            <asp:DataGrid id="gridNC" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3" ShowFooter="True"
                        OnEditCommand="gridNC_Edit" OnCancelCommand="gridNC_Cancel" OnUpdateCommand="gridNC_Update" >
				<FooterStyle cssclass="footer"></FooterStyle>
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
							<asp:TextBox id="descripcion_edit" runat="server" Width="200" maxlength="160" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>' />
						</EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox ID="descripciontxt" Runat="server" Width="200" maxlength="160" TextMode="MultiLine"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Cuenta">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
						</ItemTemplate>
                        <EditItemTemplate>
							<asp:TextBox id="cuentatxt_edit" runat="server" Width="80" ReadOnly="false" Text='<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>' onDblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())" ToolTip="Haga Doble Click para iniciar la busqueda"/>
						</EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox ID="cuentatxt" Runat="server" Width="80" ReadOnly="false" onDblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())" ToolTip="Haga Doble Click para iniciar la busqueda"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Sede">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "SEDE") %>
						</ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlalmacen_edit" Runat="server" class="dmediano"></asp:DropDownList>
                        </EditItemTemplate>
						<FooterTemplate>
							<asp:DropDownList ID="ddlalmacen" Runat="server" class="dmediano"></asp:DropDownList>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Centro de Costo">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "CENTROCOSTO") %>
						</ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlcentrocosto_edit" Runat="server" class="dpequeno"></asp:DropDownList>
                        </EditItemTemplate>
						<FooterTemplate>
							<asp:DropDownList ID="ddlcentrocosto" Runat="server" class="dpequeno"></asp:DropDownList>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Prefijo Documento">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>
						</ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="prefijotxt_edit" Runat="server" Width="80" ReadOnly="false" MaxLength="6" Text='<%# DataBinder.Eval(Container.DataItem, "PREFIJO") %>'></asp:TextBox>
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
                            <asp:TextBox ID="numdocutxt_edit" Runat="server" Width="80" ReadOnly="false" maxlength="8" Text='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>'></asp:TextBox>
                        </EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox ID="numdocutxt" Runat="server" Width="80" ReadOnly="false" maxlength="8" CssClass="AlineacionDerecha"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="NIT Beneficiario">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "NIT") %>
						</ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="numnittxt_edit" Runat="server" Width="80" ReadOnly="false" Text='<%# DataBinder.Eval(Container.DataItem, "NIT") %>' onDblCLick="ModalDialog(this,'SELECT mnit_nit AS Nit,mnit_apellidos CONCAT \' \' CONCAT COALESCE(mnit_apellido2,\'\') CONCAT \' \' CONCAT mnit_nombres CONCAT \' \' CONCAT COALESCE(mnit_nombre2,\'\') AS Nombre FROM dbxschema.mnit order by mnit_nit',new Array())" ToolTip="Haga Doble Click para iniciar la busqueda"></asp:TextBox>
                        </EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox ID="numnittxt" Runat="server" Width="80" ReadOnly="false" onDblCLick="ModalDialog(this,'SELECT mnit_nit AS Nit,mnit_apellidos CONCAT \' \' CONCAT COALESCE(mnit_apellido2,\'\') CONCAT \' \' CONCAT mnit_nombres CONCAT \' \' CONCAT COALESCE(mnit_nombre2,\'\') AS Nombre FROM dbxschema.mnit order by mnit_nit',new Array())" ToolTip="Haga Doble Click para iniciar la busqueda"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Valor Debito" ItemStyle-HorizontalAlign="Right">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALORDEBITO","{0:C}") %>
						</ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="valordebitotxt_edit" Runat="server" Width="80" ReadOnly="false" Text='<%# DataBinder.Eval(Container.DataItem, "VALORDEBITO","{0:C}") %>' onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha"></asp:TextBox>
                        </EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox ID="valordebitotxt" Runat="server" Width="80" ReadOnly="false" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Valor Credito" ItemStyle-HorizontalAlign="Right">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALORCREDITO","{0:C}") %>
						</ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="valorcreditotxt_edit" Runat="server" Width="80" ReadOnly="false" Text='<%# DataBinder.Eval(Container.DataItem, "VALORCREDITO","{0:C}") %>' onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha"></asp:TextBox>
                        </EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox ID="valorcreditotxt" Runat="server" Width="80" ReadOnly="false" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Valor Base" ItemStyle-HorizontalAlign="Right">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALORBASE","{0:C}") %>
						</ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="valorbasetxt_edit" Runat="server" class="tpequeno" ReadOnly="false" Text='<%# DataBinder.Eval(Container.DataItem, "VALORBASE","{0:C}") %>' onkeyup="NumericMaskE(this,event)"  CssClass="AlineacionDerecha"></asp:TextBox>
                        </EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox ID="valorbasetxt" Runat="server" class="tpequeno" ReadOnly="false" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha"></asp:TextBox>
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Agregar" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<asp:Button CommandName="RemoverFilas" Text="Remover" ID="btnRem" runat="server" class="bpequeno" CausesValidation="False" />
                            <asp:Button CommandName="CopiarFilas" Text="Copiar" ID="btnCop" runat="server" class="bpequeno" CausesValidation="False" />
						</ItemTemplate>
						<FooterTemplate>
							<asp:Button CommandName="AgregarFilas" Text="Agregar" ID="btnAdd" runat="server" cssclass="bpequeno" CausesValidation="False" />
						</FooterTemplate>
					</asp:TemplateColumn>
                    <asp:EditCommandColumn ButtonType="PushButton" EditText="Editar" CancelText="Cancelar" UpdateText="Actualizar"></asp:EditCommandColumn>
				</Columns>
			</asp:DataGrid>
            </fieldset>
           </td>
          </tr>
        </table>
            </p>
		<P></P>
		<TABLE style="BACKGROUND-COLOR: white">
			<TR>
				<TD vAlign="middle">
					<asp:Panel id="pnlValores" runat="server" Visible="False">
						<P>
							<TABLE>
								<TR>
									<TD>
										<asp:Label id="lbcuenta" Runat="server"></asp:Label></TD>
									<TD align="right">
										<asp:TextBox id="tbcuenta" ondblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM DBXSCHEMA.mcuenta WHERE timp_codigo IN(\'A\',\'P\') AND mcue_codipuc LIKE \'2%\'; AND ',new Array())" Width="104px" Runat="server" ToolTip="Digite la cuenta o de doble click para iniciar la busqueda"></asp:TextBox>
										<asp:RequiredFieldValidator id="rfv1" runat="server" ErrorMessage="*" ControlToValidate="tbcuenta" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirLupa();"></asp:Image>
									</TD>
								</TR>
								<TR>
									<TD>Valor Factura :
									</TD>
									<TD>
										<asp:TextBox id="valorTotal" runat="server" class="tmediano" Text="$0.00" ReadOnly="true" CssClass="AlineacionDerecha">$0.00</asp:TextBox></TD>
								</TR>
								<TR>
									<TD>Valor IVA :
									</TD>
									<TD>
										<asp:TextBox id="valorIva" runat="server" class="tmediano" Text="$0.00" ReadOnly="True" CssClass="AlineacionDerecha"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD>Valor Fletes :
									</TD>
									<TD>
										<asp:TextBox id="valorFletes" onkeyup="CalcularTotalesFletes(this)" runat="server" class="tmediano" Text="0" CssClass="AlineacionDerecha"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD>Valor IVA Fletes :
									</TD>
									<TD>
										<asp:TextBox id="valorIvaFletes" runat="server" class="tmediano" Text="0" ReadOnly="True" CssClass="AlineacionDerecha">$0.00</asp:TextBox></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lbCosto" runat="server">Costo de la Factura : </asp:Label></TD>
									<TD>
										<asp:TextBox id="costoFac" onkeyup="NumericMaskE(this,event)" runat="server" class="tmediano" Text="0" CssClass="AlineacionDerecha"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD>Valor Retenciones :
									</TD>
									<TD>
										<asp:TextBox id="tbretenciones" class="tmediano" Runat="server" Text="$0.00" ReadOnly="True" CssClass="AlineacionDerecha"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD align="left">Total Factura&nbsp;:
									</TD>
									<TD>
										<asp:TextBox id="tbTotalFac" class="tmediano" Runat="server" ReadOnly="True" CssClass="AlineacionDerecha">0</asp:TextBox></TD>
								</TR>
							</TABLE>
						</P>
					</asp:Panel></TD>
				<TD align="center">
					<asp:Label id="lbRet" Visible="False" Runat="server" Text="Retenciones"></asp:Label>
                    <table id="Table6" class="filtersIn">
    <tr>
     <td class="scrollable">
                <fieldset id="fldRtns" runat="server" visible="false">
					<asp:DataGrid id="gridRtns" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3" onItemCommand="gridRtns_Item" OnItemDataBound="gridRtns_ItemDataBound" showfooter="True">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
                            <asp:TemplateColumn HeaderText="Tipo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
						        FooterStyle-HorizontalAlign="Center">
						        <ItemTemplate>
							        <%# DataBinder.Eval(Container.DataItem, "TIPORETE") %>
						        </ItemTemplate>
						        <FooterTemplate>
							        <center>
								        <asp:DropDownList id="ddlTiporet"  class="dmediano" runat="server"></asp:DropDownList>
							        </center>
						        </FooterTemplate>
					        </asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="C&#243;digo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
								</ItemTemplate>
								<FooterTemplate>
									<center>
										<asp:TextBox id="codret" runat="server" ReadOnly="true" Width="70" ToolTip="Haga Click"></asp:TextBox>
									</center>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Porcentaje de Retenci&#243;n (%)" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="codretb" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Base">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="base" runat="server" Enabled="true" Text="0" CssClass="AlineacionDerecha" Width="100"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="valor" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha" Width="100" ReadOnly="True"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Agregar" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<asp:Button id="remRet" runat="server" CommandName="RemoverRetencion" Text="Remover" CausesValidation="false" />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button id="agRet" runat="server" CommandName="AgregarRetencion" Text="Agregar" Enabled="true" CausesValidation="false" />
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
                     </fieldset>
                   </td>
                </tr>
            </table>
					<asp:Label id="lbIva" Visible="False" Runat="server" Text="IVA"></asp:Label>
                    <table id="Table7" class="filtersIn">
                        <tr>
                        <td class="scrollable">
                    <fieldset id="fldIva" runat="server" visible="false">
					    <asp:DataGrid id="dgIva" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3" showfooter="True">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Porcentaje de IVA (%)" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE","{0:N}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ddlPorcIva" Runat="server" class="dmediano"></asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Base">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="tbValIvaBase" runat="server" Enabled="true" Text="0" CssClass="AlineacionDerecha" Width="100"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="tbValIva" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha" Width="100" ReadOnly="True"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cuenta PUC">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="tbCuenta" runat="server" Enabled="true" Width="100" onDblclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\') AND mcue_codipuc LIKE \'2408%\' ',new Array())"></asp:TextBox>
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
									<asp:Button id="btnAddIva" runat="server" CommandName="AgregarIva" Text="Agregar" Enabled="true" CausesValidation="false" />
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
                     </fieldset>
                   </td>
                </tr>
            </table>
            </TD>
		  </TR>
		</TABLE>
		<P>
			<asp:Button id="guardar" onclick="Guardar_Factura" runat="server" Text="Guardar" Enabled="False" UseSubmitBehavior="false" 
 OnClientClick="clickOnce(this, 'Cargando...')">
			</asp:Button>
			<asp:Button id="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False" onclick="btnCancelar_Click">
			</asp:Button></P>
	</asp:panel>
<p><asp:label id="lbInfo" runat="server"></asp:label><input id="hdniva" type="hidden" runat="server"></p>
<script type ='text/javascript'>

    function Cambio_Retencion(objDdlRete, objTxtcodret, tipoSociedad, porcentaje, base, valor) {

    $("#" + objTxtcodret.id).click(function () {
        if (tipoSociedad == "N"|| tipoSociedad == "U"){
            ModalDialog(objTxtcodret, 'SELECT PR.PRET_CODIGO CODIGO,PR.PRET_NOMBRE NOMBRE,PR.PRET_PORCENNODECL PORCENTAJE,PR.TTIP_PROCESO PROCESO, ' +
                                  'PR.TRET_CODIGO TIPO, PR.MCUE_CODIPUCPROV CUENTA FROM PRETENCION PR WHERE PR.TRET_CODIGO=\'' + objDdlRete.value + '\'  ' +
                                  ' ORDER BY TIPO;', new Array());
        }
        else 
            ModalDialog(objTxtcodret, 'SELECT PR.PRET_CODIGO CODIGO,PR.PRET_NOMBRE NOMBRE,pr.pret_porcendecl PORCENTAJE,PR.TTIP_PROCESO PROCESO, ' +
                                  'PR.TRET_CODIGO TIPO, PR.MCUE_CODIPUCPROV CUENTA FROM PRETENCION PR WHERE PR.TRET_CODIGO=\'' + objDdlRete.value + '\'  ' +
                                  ' ORDER BY TIPO;', new Array());
       
        });


        $("#" + objTxtcodret.id).change(function () {

            PorcentajeVal(porcentaje, base, valor);
        });
    }

function clickOnce(btn, msg)
        {
            // Comprobamos si se está haciendo una validación
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Procesando..'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
        
	function RemoverComas(obj)
	{
		if(obj.value.indexOf('.')!=-1)
			obj.value=obj.value.substring(0,obj.value.indexOf('.'));
		obj.value=EliminarComas(obj.value);
	}

	function CalcularTotalesFletes(obj)
	{
		var objValFac=document.getElementById("<%=valorTotal.ClientID%>");
		var objValIva=document.getElementById("<%=valorIva.ClientID%>");
		var objValFle=obj;
		var objValIvaFle=document.getElementById("<%=valorIvaFletes.ClientID%>");
		var objValRet=document.getElementById("<%=tbretenciones.ClientID%>");
		var totFac=document.getElementById("<%=tbTotalFac.ClientID%>");
		if(objValFle.value=='')
			return;
		NumericMask(objValFle);
		var valRealFac=parseFloat(EliminarComas(objValFac.value.substring(1)));
		var valRealIva=parseFloat(EliminarComas(objValIva.value.substring(1)));
		var valRealFle=parseFloat(EliminarComas(objValFle.value));
		var valRealIvaFle=parseFloat(EliminarComas(objValIvaFle.value.substring(1)));
		var valRealRet=parseFloat(EliminarComas(objValRet.value.substring(1)));
		totFac.value=String(valRealFac+valRealIva+valRealFle+valRealIvaFle-valRealRet);
		ApplyNumericMask(totFac);
		totFac.value="$"+totFac.value;
	}
	
	function CalcularTotalesIvaFletes(obj)
	{
		var objValFac=document.getElementById("<%=valorTotal.ClientID%>");
		var objValIva=document.getElementById("<%=valorIva.ClientID%>");
		var objValFle=document.getElementById("<%=valorFletes.ClientID%>");
		var objValIvaFle=obj;
		var objValRet=document.getElementById("<%=tbretenciones.ClientID%>");
		var totFac=document.getElementById("<%=tbTotalFac.ClientID%>");
		if(objValIvaFle.value=='')
			return;
		NumericMask(objValIvaFle);
		var valRealFac=parseFloat(EliminarComas(objValFac.value.substr(1)));
		var valRealIva=parseFloat(EliminarComas(objValIva.value));
		var valRealFle=parseFloat(EliminarComas(objValFle.value));
		var valRealIvaFle=parseFloat(EliminarComas(objValIvaFle.value));
		var valRealRet=parseFloat(EliminarComas(objValRet.value.substr(1)));
		if(valRealFle<valRealIvaFle)
		{
			alert('El valor del IVA de los fletes es mayor al valor de los fletes');
			objValIvaFle.value="0";
			totFac.value=String(valRealFac+valRealIva+valRealFle+0-valRealRet);
			ApplyNumericMask(totFac);
			totFac.value="$"+totFac.value;
			return;
		}
		totFac.value=String(valRealFac+valRealIva+valRealFle+valRealIvaFle-valRealRet);
		ApplyNumericMask(totFac);
		totFac.value="$"+totFac.value;
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
	function abrirLupa()
	{
	    var campo = document.getElementById('<%=tbcuenta.ClientID%>');
	    ModalDialog(campo, 'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')', new Array())

	}
</script>
