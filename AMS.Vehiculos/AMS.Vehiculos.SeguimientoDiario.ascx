<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.SeguimientoDiario.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_SeguimientoDiario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link rel="stylesheet" href="../css/tabber.css" type="text/css" media="screen">
<script type ="text/javascript" src="../js/AMS.Vehiculos.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>

<asp:ValidationSummary ID="ValidationSummary1" runat="server" />
<div class="contenedor">
<div class="tabber" id="mytab1">
    <asp:PlaceHolder id="pAutenticacionUsuario" runat="server">
		<div class="tabbertab" title="Autenticacion">
			<fieldset>
				<legend>Autenticación de Usuario</legend>
				<table id="Table2" class="filtersIn">
					<tbody style="background-color:#f2f2f2">
						<tr>
							<td>
								<asp:label id="lVendedorAutenticacion" runat="server"  >Vendedor:</asp:label>
							</td>
							<td>
								<asp:dropdownlist id="ddlVendedorAutenticacion" runat="server"></asp:dropdownlist>
							</td>
						</tr>
						<tr>
							<td>
								<asp:label id="lContrasena" runat="server"  >Contraseña: </asp:label>
							</td>
							<td>
								<asp:textbox id="txtContrasena" runat="server" TextMode="Password"></asp:textbox>
							</td>
						</tr>
						<tr>
							<td width="200"></td>
							<td>
								<asp:button id="btnIngresar" runat="server" Text="Ingresar" onclick="btnIngresar_Click" >
								</asp:button>
							</td>
						</tr>
					</tbody>
				</table>
			</fieldset>
		</div>
    </asp:PlaceHolder>
    
    <asp:PlaceHolder id="pContactosDisponibles" runat="server">
	    <div class="tabbertab" title="Contactos">
	        <fieldset>
                <legend>Contactos Disponibles</legend>
		        <table class="main" id="tContactosDisponibles" cellspacing="3" cellpadding="3" width="600"
			        border="0">
			        <tbody style="background-color:#f2f2f2">
				        <tr>
					        <td>
						        <asp:DropDownList id="ddlContactosDisponibles" runat="server" AutoPostBack="True" onselectedindexchanged="ddlContactosDisponibles_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image>
                            </td>
				        </tr>
			        </tbody>
		        </table>
	        </fieldset>
	    </div>
    </asp:PlaceHolder>
    
    <asp:PlaceHolder id="pIngresoDatos" runat="server">
	    <div class="tabbertab" title="Cliente">
	        <fieldset>
            <legend>Información del Cliente</legend>
		        <table class="filtersIn" id="tCliente">
			        <tbody>
			            <tr>
					        <td>
						        <asp:label id="lPrefijo" runat="server"  >Prefijo:</asp:label>
                            <br />
						        <asp:dropdownlist id="ddlPrefijo" class="dmediano" runat="server" onselectedindexchanged="ddlPrefijo_SelectedIndexChanged"></asp:dropdownlist>
						        <%--<asp:requiredfieldvalidator id="rfvPrefijo" runat="server" ControlToValidate="ddlPrefijo" ErrorMessage="El prefijo es un dato necesario." ></asp:requiredfieldvalidator>--%>
                            </td>
			            </tr>
			            <tr>
					        <td>
						        <asp:label id="lTipoCliente" runat="server"  >Tipo Cliente:</asp:label>
                             <br />
						        <asp:dropdownlist id="ddlTipoCliente" class="validador" runat="server" Width="250px"></asp:dropdownlist><asp:Label ID="lbCiente" runat="server" class="label"></asp:Label>
                            </td>
				        </tr>
			            <tr>
					        <td>
						        <asp:label id="lProspecto" runat="server"  >Contacto:</asp:label>
                            <br />
						        <asp:dropdownlist id="ddlProspecto" class="validador" runat="server" Width="250px"></asp:dropdownlist><asp:Label ID="lbProspecto" runat="server" class="label"></asp:Label>
                            </td>
				        </tr>
			            <tr>
					        <td>
						        <asp:label id="lMedio" runat="server"  >Medio o Clase de Contacto:</asp:label>
                            <br />
						        <asp:dropdownlist id="ddlMedio" class="validador" runat="server" Width="250px"></asp:dropdownlist><asp:Label ID="lbClase" runat="server" class="label"></asp:Label>
                            </td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lNumero" runat="server"  >Cotización Número:</asp:label>
                             <br />
						        <asp:textbox id="txtNumero" class="validador" runat="server" Enabled="False" MaxLength="50" Width="250px"></asp:textbox><asp:Label ID="lbCoti" runat="server" class="label"></asp:Label>
                            </td>
				        </tr>
					    <tr>
                            <td><asp:label id="LabelN" runat="server"  >NIT o C.C.:</asp:label></td>
						    <td><asp:label id="labelRLegal" runat="server" Visible="true" Text="Representante Legal: " style="position:relative; " ></asp:label> </td>
                                
                            <td><asp:label id="labelNitRLegal" runat="server" Visible="true" Text="Nit Representante Legal: " style="position:relative;" ></asp:label> <br /></td>
                        </tr>
                        <tr>
						    <td>
                                <asp:textbox id="TextboxN" runat="server" onblur="VerificarCliente(this);" MaxLength="15" Width="235px" class="validador" ></asp:textbox>
                                <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('TextboxN');"></asp:Image><br />
                                <asp:Label ID="lbInfoNit" runat="server" style="font-size: large; font-family: unset; color: red;"></asp:Label>
						    </td>
                                                    
                            <td><asp:TextBox id="txtRLegal" runat="server" Visible="true" Width="255px" style="position:relative;" ></asp:TextBox></td>
                            <td><asp:TextBox id="txtNitRLegal" runat="server" Visible="true" Width="255px" style="position:relative;" ></asp:TextBox></td>
                        </tr>
                            <%--<asp:requiredfieldvalidator id="fieldValidatorRLegal" runat="server" ControlToValidate="txtRLegal" ErrorMessage="El Representante Legal es un dato necesario."></asp:requiredfieldvalidator>--%>
                           
				    </tbody>
                </table>
                <table id="infoDatos" runat="server">
                    <tbody>
				        <tr>
					        <td>
						        <asp:label id="lNombre" runat="server" >Nombre:</asp:label>
                            <br />
						        <asp:textbox id="txtNombre" runat="server" MaxLength="50" class="tgrande" ReadOnly="true"></asp:textbox><asp:Label ID="lbNomb" runat="server" ></asp:Label>
                                <%--<asp:textbox id="Textbox1" runat="server" MaxLength="50" class="tgrande" ValidateRequestMode="Enabled"></asp:textbox><asp:RequiredFieldValidator ControlToValidate="Textbox1" ErrorMessage="seera lok" runat="server"></asp:RequiredFieldValidator>--%>
                            </td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lTelefonoFijo" runat="server"  >Dirección:</asp:label>
                             <br />
						        <asp:textbox id="txtTelefonoFijo" class="tmediano" runat="server" MaxLength="80" ReadOnly="true"></asp:textbox><asp:Label ID="lbDir" runat="server" ></asp:Label>
                            </td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lTelefonoMovil" runat="server"  >Teléfono Movil:</asp:label>
                            <br />
						        <asp:textbox id="txtTelefonoMovil" class="tmediano" runat="server" MaxLength="14" ReadOnly="true"></asp:textbox>
                            </td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lTelefonoOficina" runat="server"  >Teléfono Fijo:</asp:label>
                            <br />
						        <asp:textbox id="txtTelefonoOficina" class="tmediano" runat="server" MaxLength="14" ReadOnly="true"></asp:textbox><asp:Label ID="lbTelFijo" runat="server" ></asp:Label>
                            </td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lEmail" runat="server"  >E-mail Contacto:</asp:label>
                           <br />
						        <asp:textbox id="txtEmail" class="tmediano" runat="server" MaxLength="100" Style="text-transform: lowercase" ReadOnly="true"></asp:textbox>
                            </td>
				        </tr>
			        </tbody>
		        </table>
	        </fieldset>
	    </div>

	    <div class="tabbertab" title="Vehículo">
        
	        <fieldset >
                <legend>Información del Vehículo</legend>
                <div style="height:20%">
                    <asp:PlaceHolder runat="server">
                        <asp:DataGrid id="dgServicios" runat="server" cssclass="datagrid" AutoGenerateColumns="False" OnEditCommand="dgServicios_Edicion"
                            OnItemCommand="dgServicios_Item" OnUpdateCommand="dgServicios_Actualizar" OnItemDataBound="dgServicios_Databound" >
                            <FooterStyle CssClass="footer"></FooterStyle>
						    <HeaderStyle CssClass="header"></HeaderStyle>
						    <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						    <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						    <ItemStyle CssClass="item"></ItemStyle>
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="Clase Vehículo">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "CLASE_VEHICULO")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlClaseVehi" runat="server" CssClass="dmediano" AutoPostBack="true" OnSelectedIndexChanged="cambiaClase"></asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Catálogo vehículo">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "CATALOGO_VEHICULO")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlCatalogoVehi" runat="server" AutoPostBack="false"></asp:DropDownList>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlCatalogoVehi" runat="server" Enabled="false"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Año Modelo">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "ANO_MODELO")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlModeloVehi" class="dmediano" runat="server"></asp:DropDownList>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlModeloVehi" runat="server" Enabled="false"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Color">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "COLOR")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlColorVehi" runat="server"></asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Equipamiento Adicional">
                                            <ItemTemplate>
                                                <%--<%# DataBinder.Eval(Container.DataItem, "COLOR")%>--%>
                                                <%# DataBinder.Eval(Container.DataItem, "EQUIP_ADICIONAL")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtEquipAdicional" runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <%--<asp:TemplateColumn HeaderText="Valor Base">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "VALOR_BASE")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Valor Impuesto">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "VALOR_IMPUESTO")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>--%>
                                        <asp:TemplateColumn HeaderText="Precio Venta">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "PRECIO_VENTA", "{0:N}")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%--<asp:Label ID="lbPrecioVenta" runat="server"></asp:Label>--%>
                                                <asp:TextBox ID="txtPrecioVenta" Class="tmediano" runat="server" ></asp:TextBox>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPrecioVenta" runat="server" Enabled="false"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Valor Descuento">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "VALOR_DESCUENTO", "{0:N}")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox id="txtValorDesc" runat="server" ></asp:TextBox>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox id="txtValorDesc" runat="server" ></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Valor Neto">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "VALOR_NETO", "{0:N}")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%--<asp:Label ID="lbValorNeto" runat="server" ></asp:Label>--%>
                                                <asp:TextBox id="txtValorNeto" Class="tmediano" runat="server" ></asp:TextBox>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox id="txtValorNeto" runat="server" Enabled="false"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Acciones">
								            <ItemTemplate>
									                <asp:Button CommandName="Delete" Text="Quitar" ID="btnDel" Runat="server"  />
								            </ItemTemplate>
								            <FooterTemplate>
									            <asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" width="70px" />
									            <%--<asp:Button CommandName="ClearRows" Text="Reiniciar" ID="btnClear" Runat="server" width="70px" />--%>
								            </FooterTemplate>
							            </asp:TemplateColumn>
							        <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
                                </Columns>
                        </asp:DataGrid>
                    </asp:PlaceHolder>
                </div>
                <br />
                    <asp:label id="Label1" runat="server"><b>Otros Elementos de la Venta:</b></asp:label>
						    <asp:DataGrid id="grillaElementos" runat="server" cssclass="datagrid" OnItemCommand="dgEvento_Grilla" 
								AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" OnItemDataBound="dgAccesorioBound">
								<FooterStyle CssClass="footer"></FooterStyle>
						        <HeaderStyle CssClass="header"></HeaderStyle>
						        <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						        <ItemStyle CssClass="item"></ItemStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="CODIGO">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:textbox id="obesequioTextBox" class="tmediano" onclick="ModalDialog(this,'SELECT pite_codigo,pite_nombre,pite_costo FROM DBXSCHEMA.pitemventavehiculo',new Array())" runat="server" ReadOnly="true"></asp:textbox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="DESCRIPCION">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:textbox id="obesequioTextBoxa" class="tmediano" onclick="ModalDialog(this,'SELECT pite_codigo,pite_nombre,pite_costo FROM DBXSCHEMA.pitemventavehiculo',new Array())" runat="server" ReadOnly="true"></asp:textbox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="COSTO">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "COSTO", "{0:C}") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:textbox id="obesequioTextBoxb" runat="server" class="tmediano" onkeyup="NumericMaskE(this,event)" ReadOnly="false"></asp:textbox>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="PORCENTAJE IVA">
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "IVA", "{0:N}%") %>
										</ItemTemplate>
										<FooterTemplate>
											<asp:DropDownList id="ddlIVA" class="dpequeno" runat="server"></asp:DropDownList>
										</FooterTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="AGREGAR">
										<ItemTemplate>
											<asp:Button CommandName="QuitarObsequios" Text="Borrar" ID="btnDel" runat="server" Width="100" CausesValidation=false />
										</ItemTemplate>
										<FooterTemplate>
											<asp:Button CommandName="AgregarObsequios" Text="Agregar" ID="btnAdd" runat="server" Width="75"  CausesValidation=false/>
										</FooterTemplate>
									</asp:TemplateColumn>
								</Columns>
								        
							</asp:DataGrid>
						     
                        <fieldset>
                        <table id="Table3" class="filtersIn">
				        <TR>
					        <TD>
						        <asp:label id="lblFinanciera" runat="server" >Finaciera:</asp:label><br />
						        <asp:dropdownlist id="ddlFinanciera" class="dmediano" runat="server"></asp:dropdownlist>
					        </TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lImpuestos" runat="server">Valor Total Accesorios:</asp:label><br />
						        <asp:textbox id="txtElementos" class="tmediano" runat="server" ReadOnly=true></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lPrecioTotal" runat="server">Precio Total:</asp:label><br />
						        <asp:textbox id="txtPrecioTotal" class="tmediano" runat="server" Enabled="False"></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lValorFinanciacion" runat="server"> Valor Financiación:</asp:label><br />
						        <asp:textbox id="txtValorFinanciacion" class="tmediano" runat="server" onKeyup="NumericMaskE(this,event);"></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="Label4" runat="server"> Número Meses Plazo:</asp:label><br />
						        <asp:textbox id="txtNumCuotas" class="tmediano" runat="server"></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="Label2" runat="server">% Tasa:</asp:label><br />
						        <asp:textbox id="txtTasaFinanciacion" class="tmediano" runat="server" onKeyup="NumericMaskE(this,event);"></asp:textbox>&nbsp;&nbsp;<asp:button id="btnPagos" runat="server" Text="Calcular Pagos" onclick="btnPagos_Click" CausesValidation=false></asp:button>
                           </TD>
				        </TR>
                        </table>
                        </fieldset>
			        </tbody>
		        </table>
	        </fieldset>
	   
            <td colspan="2" align="center">
						<asp:DataGrid id="dgrPagos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" ShowFooter="False" >
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
							    <Columns>
								    <asp:TemplateColumn HeaderText="&nbsp;&nbsp;">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "MCRED_NUMEPAGO")%>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Cuota" ItemStyle-HorizontalAlign="Right">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "MCRED_CUOTA", "{0:C}")%>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Capital" ItemStyle-HorizontalAlign="Right">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "MCRED_CAPITAL", "{0:C}")%>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Interes" ItemStyle-HorizontalAlign="Right">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "MCRED_INTERES", "{0:C}") %>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Saldo" ItemStyle-HorizontalAlign="Right">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "MCRED_SALDO", "{0:C}")%>
									    </ItemTemplate>
								    </asp:TemplateColumn>
							    </Columns>
						        </asp:DataGrid>
					        </td>
				 </div>
                <div id="divServicio" runat="server" class="tabbertab" title="Servicios" visible="false">
	                <fieldset >
                        <legend>Información de Servicios</legend>
                        <table>
                             <tr>
					        <td>
						        <asp:label id="lbTipoVehiculo" runat="server">Clase Vehículo:</asp:label></td>
					        <td>
						        <asp:dropdownlist id="ddlTipoVehiculo" class="dmediano validador" runat="server" AutoPostBack="true" onselectedindexchanged="ddlTipoVehiculo_SelectedIndexChanged" ></asp:dropdownlist><asp:Label ID="lbClaseVehiculo" runat="server" class="label"></asp:Label>
						        <asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" ControlToValidate="ddlTipoVehiculo" ErrorMessage="La Clase de Vehículo un dato necesario."></asp:requiredfieldvalidator>

					        </td>
				        </tr>		
				        <tr>
					        <td>
						        <asp:label id="lCatalogoVehiculo" runat="server" >Catálogo Vehículo:</asp:label></td>
					        <td>
						        <asp:dropdownlist id="ddlCatalogoVehiculo" class="dgrande validador" runat="server" AutoPostBack="true"
							        onselectedindexchanged="ddlCatalogoVehiculo_SelectedIndexChanged"></asp:dropdownlist>
                                <asp:Image id="imgLupaCatalogo" runat="server" ImageUrl="../img/AMS.Search.png" onClick="mostrarDialogo();"></asp:Image>
                                <asp:Label id=disp Runat="server" Text="Disponibilidad" CssClass="PunteroMano"></asp:Label><asp:Label ID="lbCatalogoVehiculo" runat="server" class="label"></asp:Label>
						        <%--<asp:requiredfieldvalidator id="rfvCatalogoVehiculo" runat="server" ControlToValidate="ddlCatalogoVehiculo"
							        ErrorMessage="El catálogo del vehículo es necesario."></asp:requiredfieldvalidator> --%>

					        </td>
				        </tr>
                        <tr>
					        <td>
						        <asp:label id="lModelo" runat="server">Año Modelo:</asp:label></td>
					        <td>
						        <asp:dropdownlist id="ddlModelo" class="dmediano validador" runat="server" Enabled="false" AutoPostBack="true" onselectedindexchanged="cambioAno"></asp:dropdownlist><asp:Label ID="lbModelo" runat="server" class="label"></asp:Label>
                                <asp:requiredfieldvalidator id="requiredYear" runat="server" ControlToValidate="ddlModelo"
                                 ErrorMessage="El año del modelo es requerido!"></asp:requiredfieldvalidator>
                                
                            </td>
				        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOpcionV" runat="server" Text="Opcion Vehiculo: " Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList id=ddlOpcionVeh runat="server" OnSelectedIndexChanged="Cambio_OpcionVehiculo" AutoPostBack="true" Visible="false" class="dmediano"></asp:DropDownList>
                            </td>
                        </tr>
				        <tr>
					        <td>
						        <asp:label id="lcolor" runat="server" >Color:</asp:label></td>
					        <td>
						        <asp:dropdownlist id="ddlcolor" class="dmediano validador" runat="server" ></asp:dropdownlist><asp:Label ID="lbColor" runat="server" class="label"></asp:Label>
						        <asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="ddlCatalogoVehiculo"
							        ErrorMessage="El color del vehículo es necesario."></asp:requiredfieldvalidator>

					        </td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lEquipamento" runat="server">Equipamento Adicional:</asp:label></td>
					        <td>
						        <asp:textbox id="txtEquipamento" runat="server" TextMode="MultiLine" MaxLength="400" class="amediano"></asp:textbox></td>
				        </tr>
				        
				        <tr>
					        <td>
						        <asp:label id="lBase" runat="server">Valor Base:</asp:label></td>
					        <td>
						        <asp:textbox id="txtBase" onkeyup="NumericMaskE(this,event);" class="tmediano" runat="server" Enabled="false" ></asp:textbox></td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="Label5" runat="server">Valor Impuestos:</asp:label></td>
					        <td>
						        <asp:textbox id="txtImpuestos" onkeyup="NumericMaskE(this,event);" class="tmediano" runat="server" Enabled="false" ></asp:textbox></td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lVenta" runat="server">Precio de Venta:</asp:label></td>
					        <td>
						        <asp:textbox id="txtVenta" onkeyup="NumericMaskE(this,event);" class="tmediano" runat="server" Enabled="false" ></asp:textbox></td>
				        </tr>
                        <tr>
					        <td>
						        <asp:label id="Label6" runat="server">Valor Descuento:</asp:label></td>
					        <td>
						        <asp:textbox id="txtDescuento" onkeyup="NumericMaskE(this,event);" onblur="ValidarDescuento(this);" class="tmediano" runat="server" ></asp:textbox></td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="Label7" runat="server">Valor Neto de Venta:</asp:label></td>
					        <td>
						        <asp:textbox id="txtNeto" onkeyup="NumericMaskE(this,event);CalcularPrecioTotal();" class="tmediano" runat="server" Enabled="false" ></asp:textbox></td>
				        </tr>
                        </table>
                    </fieldset>
                </div>
            <%--<div id="divSoftware" runat="server" class="tabbertab" title="Software">
	            <FIELDSET >
                    <LEGEND>Información de Software</LEGEND>
                </FIELDSET>
            </div>--%>
        
        <fieldset id="fldContacto" runat="server" visible="false">
        <legend>Información de Contacto</legend>
         <table id="Table1" class="filtersIn">
            <asp:DataGrid id="dgContactos" runat="server" AutoGenerateColumns="false" cssclass="datagrid" GridLines="Vertical" ShowFooter="False" OnItemDataBound="dgContactosBound">
					<FooterStyle CssClass="footer"></FooterStyle>
					<HeaderStyle CssClass="header"></HeaderStyle>
					<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
					<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
					<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					<ItemStyle CssClass="item"></ItemStyle>
								        <Columns>
									        <asp:TemplateColumn HeaderText="Nro Contacto y Etapa">
										        <ItemTemplate>
											        <%# DataBinder.Eval(Container.DataItem, "NUMERO")%>
										        </ItemTemplate>
									        </asp:TemplateColumn>
									        <asp:TemplateColumn HeaderText="Fecha">
										        <ItemTemplate>
											        <%# DataBinder.Eval(Container.DataItem, "FECHA","{0:yyyy-MM-dd}")%>
										        </ItemTemplate>
									        </asp:TemplateColumn>
									        <asp:TemplateColumn HeaderText="Detalle del Contacto y Compromiso">
										        <ItemTemplate>
											        <%# DataBinder.Eval(Container.DataItem, "OBSERVACION")%>
										        </ItemTemplate>
									        </asp:TemplateColumn>
								        </Columns>
							        </asp:DataGrid>
          </table>
       </fieldset>
       
	    <div class="tabbertab" title="Contacto">
	        <fieldset >
		        <table class="filtersIn" id="tContacto">
			        <tbody >
				        <tr>
					        <td>
						        <asp:textbox id="txtNumeroContactos" runat="server" class="tmediano" Visible="False" Enabled="False" ></asp:textbox></td>
				        </tr>
				        <tr>
					        <td>
						        <P><asp:label id="Label3" runat="server">Contacto Actual:</asp:label>
                                  </P>
					        </td>
				        </tr> 
				        <tr>
					        <td>
						        <asp:label id="lFechaContacto" runat="server">Fecha Contacto:</asp:label></td>
					        <td>
						        <asp:textbox id="txtFechaContacto" class="tmediano" runat="server" Enabled="True"></asp:textbox></td>
				        </tr>
				         <tr>
					        <td>
						        <asp:label id="lResultadoContacto" runat="server">Resultado del Contacto-Etapa Proceso Venta:</asp:label></td>
					        <td>
						        <asp:dropdownlist id="ddlTipoContacto" class="dmediano validador" runat="server"></asp:dropdownlist>

					        </td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lObservacionesContacto" runat="server">Detalle Contacto Actual y Compromisos:</asp:label></td>
					        <td>
						        <asp:textbox id="txtObservacionesContacto" runat="server" TextMode="MultiLine" MaxLength="400"
							       class="amediano"></asp:textbox></td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lProximoContacto" runat="server">Próximo Contacto:</asp:label></td>
					        <td>
						        <asp:dropdownlist id="ddlProximoContacto" class="dmediano validador" runat="server" AutoPostBack="True" onselectedindexchanged="ddlProximoContacto_SelectedIndexChanged"></asp:dropdownlist><asp:Label id="lbContacto" runat="server" class="label"></asp:Label>
						        <%--<asp:requiredfieldvalidator id="rfvProximoContacto" runat="server" ControlToValidate="ddlProximoContacto" ErrorMessage="El proximo contacto es necesario."></asp:requiredfieldvalidator>--%>

					        </td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lFechaProximoContacto" runat="server">Fecha Próximo Contacto:</asp:label></td>
					        <td>
						        <asp:calendar BackColor=Beige id="cFechaProximoContacto" runat="server" Enabled="False" class="cmediano">
							        <SelectedDayStyle BackColor="Navy"></SelectedDayStyle>
						        </asp:calendar></td>
				        </tr>
				        <tr>
					        <td>
						        <asp:label id="lVendedor" runat="server"  >Vendedor:</asp:label></td>
					        <td>
						        <asp:dropdownlist id="ddlVendedor" class="dmediano validador" runat="server" AutoPostBack="true" OnSelectedIndexChanged="validaVendedor"></asp:dropdownlist><asp:Label ID="lbVendedor" runat="server" class="label"></asp:Label>
						        <%--<asp:requiredfieldvalidator id="rfvVendedor" runat="server" ControlToValidate="ddlVendedor" ErrorMessage="El vendedor es un campo requerido."></asp:requiredfieldvalidator>--%>

					        </td>
				        </tr>
				        <tr>
					        <td width="200"></td>
					        <td style="WIDTH: 152px">
						        <%--<asp:button id="Grabar" onclick="Grabar_Click" runat="server" Text="Grabar" enabled="false" Visible ="false"> </asp:button><br />--%>
                                <asp:Button id="grabarCoti" OnClick ="guardarCotizacion" runat="server" Text="Grabar" />
					        </td>
                                
				        </tr>
			        </tbody>
		        </table>
	        </fieldset>
	    </div>
    </asp:PlaceHolder>
</div>
</div>

<P><asp:textbox id="txtModo" runat="server" Visible="False"></asp:textbox></P>
<P><asp:label id="lError" runat="server"></asp:label></P>

<script type ="text/javascript">
    var labelRepLegal = document.getElementById("<%=labelRLegal.ClientID%>");
    var repLegal = document.getElementById("<%=txtRLegal.ClientID%>");
    var labelNitRLegal = document.getElementById("<%=labelNitRLegal.ClientID%>");
    var nitRepLegal = document.getElementById("<%=txtNitRLegal.ClientID%>");
    var nit = document.getElementById("<%=TextboxN.ClientID%>");
    $(repLegal).hide();
    $(labelRepLegal).hide();
    $(labelNitRLegal).hide();
    $(nitRepLegal).hide();
    if (repLegal.value != '' || nitRepLegal.value != '') {
        $(repLegal).show();
        $(labelRepLegal).show();
        $(labelNitRLegal).show();
        $(nitRepLegal).show();
    }

    function InterpretarCadenaVacia(valor) {
        if (valor == "") return 0;
        else return parseFloat(valor);
    }

        function mostrarDialogo() {
        var catalogo = document.getElementById("<%=ddlCatalogoVehiculo.ClientID%>");
        ModalDialog(catalogo, 'SELECT pc.Pcat_codigo, pmar_nombre concat \'  -  \' concat pcat_descripcion concat \'  -  \' concat pc.pcat_codigo AS descripcion  ' +
                 'FROM dbxschema.pcatalogovehiculo pc, dbxschema.pmarca pm, dbxschema.PPRECIOVEHICULO pp  ' +
                 'WHERE pc.pcat_codigo = pp.pcat_CODIGO AND PC.PMAR_CODIGO = PM.PMAR_CODIGO  ' +
                 'ORDER BY descripcion;', new Array());
    }

    function ValidarDescuento(txtDescuento) {
        var venta = InterpretarCadenaVacia(EliminarComas(document.getElementById("<%=txtVenta.ClientID%>").value));
        var ddlCatalogoDesc = document.getElementById("<%=ddlCatalogoVehiculo.ClientID%>");
        var catalogo = ddlCatalogoDesc.options[ddlCatalogoDesc.selectedIndex].value;

        AMS_Vehiculos_SeguimientoDiario.ValidarDescuento(txtDescuento.value, venta, catalogo, ValidarDescuento_CallBack);
    }
    function ValidarDescuento_CallBack(response) {
        if (response.value == false) {
            var txtValDescu = document.getElementById("<%=txtDescuento.ClientID%>");
            txtValDescu.value = "0";
            alert("El valor de descuento no corresponde al valor máximo permitido!");
        }

        CalcularPrecioVenta();
    }

    function CalcularPrecioVenta() {
        var venta = InterpretarCadenaVacia(EliminarComas(document.getElementById("<%=txtVenta.ClientID%>").value));
        var descuento = InterpretarCadenaVacia(EliminarComas(document.getElementById("<%=txtDescuento.ClientID%>").value));
        var precioTotal = venta - descuento;
        var txtNeto = document.getElementById("<%=txtNeto.ClientID%>");
        txtNeto.value = precioTotal;
        ApplyNumericMask(txtNeto);
    }
    CalcularPrecioVenta();

    function CalcularPrecioTotal() {
        var venta = InterpretarCadenaVacia(EliminarComas(document.getElementById("<%=txtNeto.ClientID%>").value));
        var elementos = InterpretarCadenaVacia(EliminarComas(document.getElementById("<%=txtElementos.ClientID%>").value));
        var precioTotal = venta + elementos;
        var txtT = document.getElementById("<%=txtPrecioTotal.ClientID%>");
        txtT.value = precioTotal;
        ApplyNumericMask(txtT);
    }
    CalcularPrecioTotal();

    function VerificarCliente(obj) {
        AMS_Vehiculos_SeguimientoDiario.Verificar_Cliente(obj.value, Verificar_Cliente_CallBack);
    }

    function Verificar_Cliente_CallBack(response) {

        var respuesta = response.value;
        var contactos = "";
        var nit = document.getElementById("<%=TextboxN.ClientID%>");
        var nombre = document.getElementById("<%=txtNombre.ClientID%>");
        var direccion = document.getElementById("<%=txtTelefonoFijo.ClientID%>");
        var celular = document.getElementById("<%=txtTelefonoMovil.ClientID%>");
        var telefono = document.getElementById("<%=txtTelefonoOficina.ClientID%>");
        var email = document.getElementById("<%=txtEmail.ClientID%>");
        var repLegal = document.getElementById("<%=txtRLegal.ClientID%>");
        var nitRepLegal = document.getElementById("<%=txtNitRLegal.ClientID%>");
        var tablaDatos = document.getElementById("<%=infoDatos.ClientID%>");
        var labelError = document.getElementById("<%=lbInfoNit.ClientID%>");
        var btnGuardar = document.getElementById("<%=grabarCoti.ClientID%>");
        try {
            if (respuesta.Tables[1].Rows.length > 0) {
                //if()
                contactos = "Se han encontrado contactos registrados por este cliente: \n\n";

                for (var i = 0; i < respuesta.Tables[1].Rows.length; i++) {
                    contactos = contactos + respuesta.Tables[1].Rows[i].CONTACTOS + '\n\n'
                }
                alert(contactos);
            }

        } catch (e) { }
        
        nombre.readOnly = true;
        direccion.readOnly = true;
        telefono.readOnly = true;
        celular.readOnly = true;

        if (respuesta.Tables[0].Rows.length > 0)
        {
            $(labelError).text("");
            $(tablaDatos).show();
            $(btnGuardar).prop("disabled", false);
            /*
                $(nombre).show();
                $(direccion).show();
                $(telefono).show();
                $(celular).show();
                $(email).show();
            */
            nombre.value = respuesta.Tables[0].Rows[0].NOMBRE;
            direccion.value = respuesta.Tables[0].Rows[0].DIRECCION;
            telefono.value = respuesta.Tables[0].Rows[0].TELEFONO;
            celular.value = respuesta.Tables[0].Rows[0].CELULAR;
            email.value = respuesta.Tables[0].Rows[0].EMAIL;
            repLegal.value = respuesta.Tables[0].Rows[0].REPRESENTANTE_LEGAL;
            nitRepLegal.value = respuesta.Tables[0].Rows[0].NIT_REPRESENTANTE;
            if (nombre.value.trim() == "" || nombre.value == " ")
            {
                nombre.readOnly = false;
                //$(nombre).hide();
            }
            if (direccion.value.trim() == "" || direccion.value == " ")
            {
                direccion.readOnly = false;
                //$(direccion).hide();
            }
            if (telefono.value.trim() == "" || telefono.value == " ")
            {
                telefono.readOnly = false;
                //$(telefono).hide();
            }
            if (celular.value.trim() == "" || celular.value == " ")
            {
                celular.readOnly = false;
                //$(celular).hide();
            } 
            if (email.value.trim() == "" || email.value == " ")
            {
                email.readOnly = false;
                //$(email).hide();
            }
            
            if (respuesta.Tables[0].Rows[0].TIPONIT == "N") {
                $(repLegal).show();
                $(labelRepLegal).show();
                $(nitRepLegal).show();
                $(labelNitRLegal).show();

            }
            else {
                $(repLegal).hide();
                $(labelRepLegal).hide();
                $(nitRepLegal).hide();
                $(labelNitRLegal).hide();
                repLegal.value = '';
            }
        }
        else {
            nombre.value = "";
            direccion.value = "";
            telefono.value = "";
            celular.value = "";
            email.value = "";
            repLegal.value = '';
            /*nombre.readOnly     = false;
            direccion.readOnly  = false;
            telefono.readOnly   = false;
            celular.readOnly    = false;
            email.readOnly      = false;
            $(nombre).hide();
            $(direccion).hide();
            $(telefono).hide();
            $(celular).hide();
            $(email).hide();*/
            if (nit.value != '' && nit.value != undefined){
                ModalDialog(nit, '*MNITCOTIZACION' + '*' + nit.value, new Array());
            }
            $(labelError).html("El N.I.T no existe en el sistema, de click en la lupa, luego click en Insertar... <br /> <span style='size:15px'> Este N.I.T se guarda <b>únicamente</b> en la tabla de Cotizaciones</span>");
            $(tablaDatos).hide();
            $(btnGuardar).prop("disabled", true);
            $(repLegal).hide();
            $(labelRepLegal).hide();
            $(nitRepLegal).hide();
            $(labelNitRLegal).hide();
        }
    }
    function cambiarPrecio(obj, idTxtPrecio, idTxtValorNeto, idAno, idTxtValorDesc,idClaseVehiculo, sender)
    {
        var campoTexto = document.getElementById(idTxtPrecio);
        var campoValorNeto = document.getElementById(idTxtValorNeto);
        var ano = document.getElementById(idAno);
        var valorDesc = document.getElementById(idTxtValorDesc);
        var claseVehiculo = document.getElementById(idClaseVehiculo);

        if (claseVehiculo.value == "U")
        {
            campoTexto.value = AMS_Vehiculos_SeguimientoDiario.cargarPrecioVehiculoUsado(obj.value).value;
        }
        else {
            //llenarAnosCatalogo(ano, obj.value);
            if (sender.id == obj.id)
            {
                ano.options.length = 0;
                var datosAnos = AMS_Vehiculos_SeguimientoDiario.consultarAnos(obj.value.split('~')[0], obj.value.split('~')[1]).value
                if (datosAnos.Tables[0].Rows.length > 0){
                    for (var i = 0; i < datosAnos.Tables[0].Rows.length; ++i)
                        ano.options[ano.options.length] = new Option(datosAnos.Tables[0].Rows[i].DESCRIPCION, datosAnos.Tables[0].Rows[i].CODIGO);
                }
            }
            
            $(campoTexto).prop('readonly', true);
            $(campoValorNeto).prop('readonly', true);

            campoTexto.value = AMS_Vehiculos_SeguimientoDiario.cargarPrecioVehiculo(obj.value.split('~')[0], ano.value, obj.value.split('~')[1]).value;
        }
        if (campoTexto.value == '') {
            campoTexto.value = '0';
        }
        campoValorNeto.value = campoTexto.value;
        valorDesc.value = '';
        
    }

    function calcularNeto(obj, idTxtValorDesc,idCatalogo, idAno)
    {
        var valorDesc = document.getElementById(idTxtValorDesc);
        //calcular valor neto con el desceunto, falta validar que no sea mayor a lo permitido
    }
    function validarDescuento(obj,idTxtPrecioVenta,idCatalogo,idAno, idTxtValorNeto)
    {
        var valorN = obj.value;
        if (valorN == "")
        {
            document.getElementById(idTxtValorNeto).value = document.getElementById(idTxtPrecioVenta).value;
            return;
        }
        var esLetra = isNaN(valorN);
        if (esLetra || valorN == " ")
        {
            obj.value = obj.value.substring(0, obj.value.length - 1);
            return;
        }

        var valor = document.getElementById(idTxtPrecioVenta);
        var valorNeto = document.getElementById(idTxtValorNeto);
        var catalogo = document.getElementById(idCatalogo);
        var ano = document.getElementById(idAno);
        //var claseVehiculo = document.getElementById(idClaseVehiculo)
        //declaración nuestros elementos

       descuentoPermitido = AMS_Vehiculos_SeguimientoDiario.calcularDescuento(obj.value, valor.value, catalogo.value.split('~')[0], ano.value);
        if(descuentoPermitido.value == "-1")
        {
            alert('El valor del descuento es mayor al permitido o no se encontró ningún valor para aplicar');
            obj.value = obj.value.substring(0, obj.value.length - 1);
            validarDescuento(obj, idTxtPrecioVenta, idCatalogo, idAno, idTxtValorNeto);
        } else
        {
            //valor.value = descuentoPermitido.value;
            valorNeto.value = descuentoPermitido.value;
        }

    }
</script>

<script type ="text/javascript">
    $(document).ready(function () {
        $('select.validador').next().text('*').css({ "color": "red", "font-size": "larger" });
    });
</script>
<script type="text/javascript">
	<%=strCookie%>
	document.write('<style type="text/css">.tabber{display:none;}<\/style>');
	var tabberOptions = {
	  'cookie':"tabber",'onLoad': function(argsObj){
		var t = argsObj.tabber;
		var i;
		if (t.id) {t.cookie = t.id + t.cookie;}
		i = parseInt(getCookie(t.cookie));
		if (isNaN(i)) { return; }
		t.tabShow(i);},
	  'onClick':function(argsObj){
		var c = argsObj.tabber.cookie;
		var i = argsObj.index;
		setCookie(c, i);}
	};
	function setCookie(name, value, expires, path, domain, secure) {
		document.cookie= name + "=" + escape(value) +
			((expires) ? "; expires=" + expires.toGMTString() : "") +
			((path) ? "; path=" + path : "") +
			((domain) ? "; domain=" + domain : "") +
			((secure) ? "; secure" : "");
	}
	function getCookie(name) {
		var dc = document.cookie;
		var prefix = name + "=";
		var begin = dc.indexOf("; " + prefix);
		if (begin == -1) {
			begin = dc.indexOf(prefix);
			if (begin != 0) return null;
		} else {
			begin += 2;
		}
		var end = document.cookie.indexOf(";", begin);
		if (end == -1) {
			end = dc.length;
		}
		return unescape(dc.substring(begin + prefix.length, end));
	}
	function deleteCookie(name, path, domain) {
		if (getCookie(name)) {
			document.cookie = name + "=" +
				((path) ? "; path=" + path : "") +
				((domain) ? "; domain=" + domain : "") +
				"; expires=Thu, 01-Jan-70 00:00:01 GMT";
		}
	}

    function mostrarDialogo() {
        var catalogo = document.getElementById("<%=ddlCatalogoVehiculo.ClientID%>");
        ModalDialog(catalogo, 'SELECT pc.Pcat_codigo, pmar_nombre concat \'  -  \' concat pcat_descripcion concat \'  -  \' concat pc.pcat_codigo AS descripcion  ' +
                 'FROM dbxschema.pcatalogovehiculo pc, dbxschema.pmarca pm, dbxschema.PPRECIOVEHICULO pp  ' +
                 'WHERE pc.pcat_codigo = pp.pcat_CODIGO AND PC.PMAR_CODIGO = PM.PMAR_CODIGO  ' +
                 'ORDER BY descripcion;', new Array());
    }
    function abrirEmergente(obj) {
    var nit = document.getElementById('_ctl1_' + obj);
    ModalDialog(nit, 'SELECT mnit_nit, MNIT_NOMBRES CONCAT \' \' CONCAT coalesce(MNIT_NOMBRE2, \'\') CONCAT \' \' CONCAT MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT_APELLIDO2, \'\') AS NOMBRE FROM MNITCOTIZACION UNION SELECT mnit_nit, MNIT_NOMBRES CONCAT \' \' CONCAT coalesce(MNIT_NOMBRE2, \'\') CONCAT \' \' CONCAT MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT_APELLIDO2, \'\') AS NOMBRE FROM  MNIT;', new Array(), 1);
    }
    function numeros(evt)
    {
        var valor = evt.key;
        var esLetra = isNaN(valor);
        if (esLetra || valor == " ") {
            return false;
        } else
            return true;
    }
    function llenarNeto(obj,txtNeto)
    {
        document.getElementById(txtNeto).value = obj.value; 
    }
</script>
<script type="text/javascript" src="../js/tabber.js"></script>
