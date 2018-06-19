<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.SeguimientoDiario.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_SeguimientoDiario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link rel="stylesheet" href="../css/tabber.css" TYPE="text/css" MEDIA="screen">
<script type ="text/javascript" src="../js/AMS.Vehiculos.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
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
</script>
<script type="text/javascript" src="../js/tabber.js"></script>

<asp:ValidationSummary ID="ValidationSummary1" runat="server" />
<div class="contenedor">
<div class="tabber" id="mytab1">
    <asp:PlaceHolder id="pAutenticacionUsuario" runat="server">
		<div class="tabbertab" title="Autenticacion">
			<FIELDSET>
				<LEGEND>Autenticación de Usuario</LEGEND>
				<table id="Table2" class="filtersIn">
					<TBODY bgColor="#f2f2f2">
						<TR>
							<TD>
								<asp:label id="lVendedorAutenticacion" runat="server"  >Vendedor:</asp:label>
							</TD>
							<TD>
								<asp:dropdownlist id="ddlVendedorAutenticacion" runat="server"></asp:dropdownlist>
							</TD>
						</TR>
						<TR>
							<td>
								<asp:label id="lContrasena" runat="server"  >Contraseña: </asp:label>
							</TD>
							<TD>
								<asp:textbox id="txtContrasena" runat="server" TextMode="Password"></asp:textbox>
							</TD>
						</TR>
						<TR>
							<TD width="200"></TD>
							<TD>
								<asp:button id="btnIngresar" runat="server" Text="Ingresar" onclick="btnIngresar_Click" >
								</asp:button>
							</TD>
						</TR>
					</TBODY>
				</TABLE>
			</FIELDSET>
		</div>
    </asp:PlaceHolder>
    
    <asp:PlaceHolder id="pContactosDisponibles" runat="server">
	    <div class="tabbertab" title="Contactos">
	        <FIELDSET>
                <LEGEND>Contactos Disponibles</LEGEND>
		        <TABLE class="main" id="tContactosDisponibles" cellSpacing="3" cellPadding="3" width="600"
			        border="0">
			        <TBODY bgColor="#f2f2f2">
				        <TR>
					        <TD>
						        <asp:DropDownList id="ddlContactosDisponibles" runat="server" AutoPostBack="True" onselectedindexchanged="ddlContactosDisponibles_SelectedIndexChanged"></asp:DropDownList>
                            </TD>
                            <TD>
                                <asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image>
                            </TD>
				        </TR>
			        </TBODY>
		        </TABLE>
	        </FIELDSET>
	    </div>
    </asp:PlaceHolder>
    
    <asp:PlaceHolder id="pIngresoDatos" runat="server">
	    <div class="tabbertab" title="Cliente">
	        <FIELDSET>
            <LEGEND>Información del Cliente</LEGEND>
		        <TABLE class="filtersIn" id="tCliente">
			        <TBODY>
			            <TR>
					        <TD>
						        <asp:label id="lPrefijo" runat="server"  >Prefijo:</asp:label>
                            <br />
						        <asp:dropdownlist id="ddlPrefijo" class="dmediano" runat="server" onselectedindexchanged="ddlPrefijo_SelectedIndexChanged"></asp:dropdownlist>
						        <asp:requiredfieldvalidator id="rfvPrefijo" runat="server" ControlToValidate="ddlPrefijo" ErrorMessage="El prefijo es un dato necesario."></asp:requiredfieldvalidator>
                            </TD>
			            </TR>
			            <TR>
					        <TD>
						        <asp:label id="lTipoCliente" runat="server"  >Tipo Cliente:</asp:label>
                             <br />
						        <asp:dropdownlist id="ddlTipoCliente" class="dmediano" runat="server"></asp:dropdownlist>
						        <asp:requiredfieldvalidator id="rfvTipoCliente" runat="server" ControlToValidate="ddlTipoCliente" ErrorMessage="El Tipo de Cliente es un dato necesario."></asp:requiredfieldvalidator>
                            </TD>
				        </TR>
			            <TR>
					        <TD>
						        <asp:label id="lProspecto" runat="server"  >Contacto:</asp:label>
                            <br />
						        <asp:dropdownlist id="ddlProspecto" class="dmediano" runat="server"></asp:dropdownlist>
						        <asp:requiredfieldvalidator id="rfvProspecto" runat="server" ControlToValidate="ddlProspecto" ErrorMessage="El prospecto es un dato necesario."></asp:requiredfieldvalidator>
                            </TD>
				        </TR>
			            <tr>
					        <td>
						        <asp:label id="lMedio" runat="server"  >Medio o Clase de Contacto:</asp:label>
                            <br />
						        <asp:dropdownlist id="ddlMedio" class="dmediano" runat="server"></asp:dropdownlist>
						        <asp:requiredfieldvalidator id="rfvMedio" runat="server" ControlToValidate="ddlMedio" ErrorMessage="El medio es un dato necesario."></asp:requiredfieldvalidator>
                            </TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lNumero" runat="server"  >Numero:</asp:label>
                             <br />
						        <asp:textbox id="txtNumero" class="tmediano" runat="server" Enabled="False" MaxLength="50"></asp:textbox>
						        <asp:requiredfieldvalidator id="rfvNumero" runat="server" ControlToValidate="txtNumero" ErrorMessage="El número es un dato necesario."></asp:requiredfieldvalidator>
                            </TD>
				        </TR>
					    <TR>
                            <td><asp:label id="LabelN" runat="server"  >NIT o C.C.:</asp:label></td>
						    <td><asp:label id="labelRLegal" runat="server" Visible="true" Text="Representante Legal: " style="position:relative; " ></asp:label> </td>
                                
                            <td><asp:label id="labelNitRLegal" runat="server" Visible="true" Text="Nit Representante Legal: " style="position:relative;" ></asp:label> <br /></td>
                        </TR>
                        <TR>
						    <td>
                                <asp:textbox id="TextboxN" runat="server" onblur="VerificarCliente(this);" MaxLength="15" Width="235px"></asp:textbox>
                                <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('TextboxN');"></asp:Image>
                                <asp:requiredfieldvalidator id="RequiredfieldvalidatorNIT" runat="server" ControlToValidate="TextboxN" ErrorMessage="El NIT o C.C. es un dato necesario."></asp:requiredfieldvalidator><br />
                                <asp:Label ID="lbInfoNit" runat="server" style="font-size: large; font-family: unset; color: red;"></asp:Label>
						    </td>
                                                    
                            <td><asp:TextBox id="txtRLegal" runat="server" Visible="true" Width="255px" style="position:relative;" ></asp:TextBox></td>
                            <td><asp:TextBox id="txtNitRLegal" runat="server" Visible="true" Width="255px" style="position:relative;" ></asp:TextBox></td>
                        </TR>
                            <%--<asp:requiredfieldvalidator id="fieldValidatorRLegal" runat="server" ControlToValidate="txtRLegal" ErrorMessage="El Representante Legal es un dato necesario."></asp:requiredfieldvalidator>--%>
                           
				    </TBODY>
                </TABLE>
                        <%--  <tr>
                            <td>
                                CC:
                                <asp:radiobutton id="rbcc" groupname="grupo1" runat="server" OnCheckedChanged="llamar_nuevo" autopostback="true">
                                </asp:radiobutton>
                                <br />
                                NIT:
                                <asp:radiobutton id="rbnit" groupname="grupo1" runat="server" OnCheckedChanged="llamar_usado" autopostback="true">
                                </asp:radiobutton>
                             </td>
                        </tr>--%>
                <table id="infoDatos" runat="server">
                    <tbody>
				        <TR>
					        <TD>
						        <asp:label id="lNombre" runat="server" >Nombre:</asp:label>
                            <br />
						        <asp:textbox id="txtNombre" runat="server" MaxLength="50" class="tgrande" ReadOnly="true"></asp:textbox>
						        <asp:requiredfieldvalidator id="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es un dato necesario."></asp:requiredfieldvalidator>
                            </TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lTelefonoFijo" runat="server"  >Dirección:</asp:label>
                             <br />
						        <asp:textbox id="txtTelefonoFijo" class="tmediano" runat="server" MaxLength="80" ReadOnly="true"></asp:textbox>
						        <asp:requiredfieldvalidator id="rfvTelefonoFijo" runat="server" ControlToValidate="txtTelefonoFijo" ErrorMessage="La Dirección es un dato necesario."></asp:requiredfieldvalidator>
                            </TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lTelefonoMovil" runat="server"  >Teléfono Movil:</asp:label>
                            <br />
						        <asp:textbox id="txtTelefonoMovil" class="tmediano" runat="server" MaxLength="14" ReadOnly="true"></asp:textbox>
                            </TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lTelefonoOficina" runat="server"  >Teléfono Fijo:</asp:label>
                            <br />
						        <asp:textbox id="txtTelefonoOficina" class="tmediano" runat="server" MaxLength="14" ReadOnly="true"></asp:textbox>
                                <asp:requiredfieldvalidator id="rfvTelefonoOficina" runat="server" ControlToValidate="txtTelefonoOficina" ErrorMessage="El telefono es un dato necesario."></asp:requiredfieldvalidator>
                            </TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lEmail" runat="server"  >E-mail Contacto:</asp:label>
                           <br />
						        <asp:textbox id="txtEmail" class="tmediano" runat="server" MaxLength="100" Style="text-transform: lowercase" ReadOnly="true"></asp:textbox>
                            </TD>
				        </TR>
			        </TBODY>
		        </table>
	        </FIELDSET>
	    </div>

	    <div class="tabbertab" title="Vehículo">
	        <FIELDSET >
                <LEGEND>Información del Vehículo</LEGEND>
		        <TABLE class="filtersIn" id="tVehiculo" >
			        <TBODY >
				        <TR>
					        <TD>
						        <asp:label id="lbTipoVehiculo" runat="server">Clase Vehículo:</asp:label></TD>
					        <TD>
						        <asp:dropdownlist id="ddlTipoVehiculo" class="dmediano" runat="server" AutoPostBack="true" onselectedindexchanged="ddlTipoVehiculo_SelectedIndexChanged"></asp:dropdownlist>
						        <asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" ControlToValidate="ddlTipoVehiculo" ErrorMessage="La Clase de Vehículo un dato necesario."></asp:requiredfieldvalidator></TD>
				        </TR>		
				        <TR>
					        <TD>
						        <asp:label id="lCatalogoVehiculo" runat="server" >Catálogo Vehículo:</asp:label></TD>
					        <TD>
						        <asp:dropdownlist id="ddlCatalogoVehiculo" class="dgrande" runat="server" AutoPostBack="true"
							        onselectedindexchanged="ddlCatalogoVehiculo_SelectedIndexChanged"></asp:dropdownlist>
                                <asp:Image id="imgLupaCatalogo" runat="server" ImageUrl="../img/AMS.Search.png" onClick="mostrarDialogo();"></asp:Image>
                                <asp:Label id=disp Runat="server" Text="Disponibilidad" CssClass="PunteroMano"></asp:Label>
						        <asp:requiredfieldvalidator id="rfvCatalogoVehiculo" runat="server" ControlToValidate="ddlCatalogoVehiculo"
							        ErrorMessage="El catálogo del vehículo es necesario."></asp:requiredfieldvalidator></TD>
				        </TR>
                        <TR>
					        <TD>
						        <asp:label id="lModelo" runat="server">Año Modelo:</asp:label></TD>
					        <TD>
						        <asp:dropdownlist id="ddlModelo" class="dmediano" runat="server" Enabled="false" AutoPostBack="true" onselectedindexchanged="cambioAno"></asp:dropdownlist>
                                <asp:requiredfieldvalidator id="requiredYear" runat="server" ControlToValidate="ddlModelo"
                                 ErrorMessage="El año del modelo es requerido!"></asp:requiredfieldvalidator>
                                
                            </TD>
				        </TR>
                        <tr>
                            <td>
                                <asp:Label ID="lblOpcionV" runat="server" Text="Opcion Vehiculo: " Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList id=ddlOpcionVeh runat="server" OnSelectedIndexChanged="Cambio_OpcionVehiculo" AutoPostBack="true" Visible="false" class="dmediano"></asp:DropDownList>
                            </td>
                        </tr>
				        <TR>
					        <TD>
						        <asp:label id="lcolor" runat="server" >Color:</asp:label></TD>
					        <TD>
						        <asp:dropdownlist id="ddlcolor" class="dmediano" runat="server" ></asp:dropdownlist>
						        <asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="ddlCatalogoVehiculo"
							        ErrorMessage="El color del vehículo es necesario."></asp:requiredfieldvalidator></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lEquipamento" runat="server">Equipamento Adicional:</asp:label></TD>
					        <TD>
						        <asp:textbox id="txtEquipamento" runat="server" TextMode="MultiLine" MaxLength="400" class="amediano"></asp:textbox></TD>
				        </TR>
				        
				        <TR>
					        <TD>
						        <asp:label id="lBase" runat="server">Valor Base:</asp:label></TD>
					        <TD>
						        <asp:textbox id="txtBase" onkeyup="NumericMaskE(this,event);" class="tmediano" runat="server" Enabled="false" ></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="Label5" runat="server">Valor Impuestos:</asp:label></TD>
					        <TD>
						        <asp:textbox id="txtImpuestos" onkeyup="NumericMaskE(this,event);" class="tmediano" runat="server" Enabled="false" ></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lVenta" runat="server">Precio de Venta:</asp:label></TD>
					        <TD>
						        <asp:textbox id="txtVenta" onkeyup="NumericMaskE(this,event);" class="tmediano" runat="server" Enabled="false" ></asp:textbox></TD>
				        </TR>
                        <TR>
					        <TD>
						        <asp:label id="Label6" runat="server">Valor Descuento:</asp:label></TD>
					        <TD>
						        <asp:textbox id="txtDescuento" onkeyup="NumericMaskE(this,event);" onblur="ValidarDescuento(this);" class="tmediano" runat="server" ></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="Label7" runat="server">Valor Neto de Venta:</asp:label></TD>
					        <TD>
						        <asp:textbox id="txtNeto" onkeyup="NumericMaskE(this,event);CalcularPrecioTotal();" class="tmediano" runat="server" Enabled="false" ></asp:textbox></TD>
				        </TR>
                        
                                    <asp:label id="Label1" runat="server">Otros Elementos de la Venta:</asp:label>
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
											        <asp:Button CommandName="QuitarObsequios" Text="Borrar" ID="btnDel" runat="server" Width="55" CausesValidation=false />
										        </ItemTemplate>
										        <FooterTemplate>
											        <asp:Button CommandName="AgregarObsequios" Text="Agregar" ID="btnAdd" runat="server" Width="55"  CausesValidation=false/>
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
				        <TR>
                        </TR>
			        </TBODY>
		        </TABLE>
	        </FIELDSET>
	   
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

        <LEGEND>Información de Contacto</LEGEND>
        <fieldset>
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
	        <FIELDSET >
                
		        <TABLE class="filtersIn" id="tContacto">
			        <TBODY >
				        <TR>
					        <TD>
						        <asp:textbox id="txtNumeroContactos" runat="server" class="tmediano" Visible="False" Enabled="False" ></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <P><asp:label id="Label3" runat="server">Contacto Actual:</asp:label>
                                  </P>
					        </TD>
				        </TR>
							        
						      
				        <TR>
					        <TD>
						        <asp:label id="lFechaContacto" runat="server">Fecha Contacto:</asp:label></TD>
					        <TD>
						        <asp:textbox id="txtFechaContacto" class="tmediano" runat="server" Enabled="True"></asp:textbox></TD>
				        </TR>
				         <TR>
					        <TD>
						        <asp:label id="lResultadoContacto" runat="server">Resultado del Contacto-Etapa Proceso Venta:</asp:label></TD>
					        <TD>
						        <asp:dropdownlist id="ddlTipoContacto" class="dmediano" runat="server"></asp:dropdownlist></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lObservacionesContacto" runat="server">Detalle Contacto Actual y Compromisos:</asp:label></TD>
					        <TD>
						        <asp:textbox id="txtObservacionesContacto" runat="server" TextMode="MultiLine" MaxLength="400"
							       class="amediano"></asp:textbox></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lProximoContacto" runat="server">Próximo Contacto:</asp:label></TD>
					        <TD>
						        <asp:dropdownlist id="ddlProximoContacto" class="dmediano" runat="server" AutoPostBack="True" onselectedindexchanged="ddlProximoContacto_SelectedIndexChanged"></asp:dropdownlist>
						        <asp:requiredfieldvalidator id="rfvProximoContacto" runat="server" ControlToValidate="ddlProximoContacto" ErrorMessage="El proximo contacto es necesario."></asp:requiredfieldvalidator></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lFechaProximoContacto" runat="server">Fecha Próximo Contacto:</asp:label></TD>
					        <TD>
						        <asp:calendar BackColor=Beige id="cFechaProximoContacto" runat="server" Enabled="False" class="cmediano">
							        <SelectedDayStyle BackColor="Navy"></SelectedDayStyle>
						        </asp:calendar></TD>
				        </TR>
				        <TR>
					        <TD>
						        <asp:label id="lVendedor" runat="server"  >Vendedor:</asp:label></TD>
					        <TD>
						        <asp:dropdownlist id="ddlVendedor" class="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="validaVendedor"></asp:dropdownlist>
						        <asp:requiredfieldvalidator id="rfvVendedor" runat="server" ControlToValidate="ddlVendedor" ErrorMessage="El vendedor es un campo requerido."></asp:requiredfieldvalidator></TD>
				        </TR>
				        <TR>
					        <TD width="200"></TD>
					        <TD style="WIDTH: 152px">
						        <asp:button id="Grabar" onclick="Grabar_Click" runat="server" Text="Grabar"> </asp:button></TD>
				        </TR>
			        </TBODY>
		        </TABLE>
	        </FIELDSET>
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

    function apagarBotonGrabar() {
        var BotonGbr = document.getElementById("Grabar");
        BotonGbr.disabled = true;
        BotonGbr.setAttribute("JavaScript: doPostBack('Grabar'");
        return false;
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
        var nombre = document.getElementById("<%=txtNombre.ClientID%>");
        var direccion = document.getElementById("<%=txtTelefonoFijo.ClientID%>");
        var celular = document.getElementById("<%=txtTelefonoMovil.ClientID%>");
        var telefono = document.getElementById("<%=txtTelefonoOficina.ClientID%>");
        var email = document.getElementById("<%=txtEmail.ClientID%>");
        var repLegal = document.getElementById("<%=txtRLegal.ClientID%>");
        var nitRepLegal = document.getElementById("<%=txtNitRLegal.ClientID%>");
        var tablaDatos = document.getElementById("<%=infoDatos.ClientID%>");
        var labelError = document.getElementById("<%=lbInfoNit.ClientID%>");
        var btnGuardar = document.getElementById("<%=Grabar.ClientID%>");
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
            $(labelError).html("El N.I.T no existe en el sistema, de click en la lupa, luego click en Insertar... <br /> <span style='size:15px'> Este N.I.T se guarda <b>únicamente</b> en la tabla de Cotizaciones</span>");
            $(tablaDatos).hide();
            $(btnGuardar).prop("disabled", true);
            $(repLegal).hide();
            $(labelRepLegal).hide();
            $(nitRepLegal).hide();
            $(labelNitRLegal).hide();
        }
    }
</script>
