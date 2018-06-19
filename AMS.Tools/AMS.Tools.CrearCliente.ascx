<%@ Control Language="c#" autoeventwireup="false" Inherits="AMS.Tools.CrearCliente" %>
<head>
    <link href="../css/AMS.Menu.css" type="text/css" rel="stylesheet" media="screen" /> 
</head>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script type="text/javascript">
    function abrirEmergente1() {
        var nNit = document.getElementById("<%=ddlnit.ClientID%>");
        ModalDialog(nNit, 'SELECT mnit_nit, CASE WHEN TNIT_TIPONIT = \'N\' THEN mnit_nit CONCAT \' - \' CONCAT MNIT_DIGITO CONCAT \' \' CONCAT MNIT_APELLIDOS ELSE  mnit_nit CONCAT \'  \' CONCAT MNIT_APELLIDOS CONCAT \' \' CONCAT COALESCE(MNIT_APELLIDO2,\'\') CONCAT \' \' CONCAT MNIT_NOMBRES CONCAT \' \' CONCAT COALESCE(MNIT_NOMBRE2,\'\') END FROM mnit order by 2', new Array(), 1);
    }
</script>


<fieldset>
    <asp:panel id="pnlInfo" runat="server" Width="568px">
	    <TABLE id="Table" class="filtersIn">
 		    <TR>
			    <TD>Información Básica del Cliente
			    </TD>
		    </TR>
		    <TR>
			    <TD></TD>
		    </TR>
		    <TR>
			    <TD>Nit del Cliente :
			    </TD>
			    <TD>
				    <asp:DropDownList id="ddlnit" runat="server" style="width: 237px"></asp:DropDownList><asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente1();"></asp:Image>
                    
			    </TD>
		    </TR>
		    <TR>
			    <TD>Tarifa Cobrada para Visitas No Incluidas en el contrato :
			    </TD>
			    <TD>
				    <asp:DropDownList id="ddltarifa" runat="server"></asp:DropDownList></TD>
		    </TR>
		    <TR>
			    <TD>Marcas que maneja el cliente :
			    </TD>
			    <TD>
				    <asp:TextBox id="tbmarcas" class="tmediano" runat="server" TextMode="MultiLine"></asp:TextBox>
				    <asp:RequiredFieldValidator id="rfv1" runat="server" ControlToValidate="tbmarcas">*</asp:RequiredFieldValidator></TD>
		    </TR>
		    <TR>
			    <TD>Nombre del Gerente :
			    </TD>
			    <TD>
				    <asp:TextBox id="tbnomger" class="tmediano" runat="server"></asp:TextBox>
				    <asp:RequiredFieldValidator id="rfv2" runat="server" ControlToValidate="tbnomger">*</asp:RequiredFieldValidator></TD>
		    </TR>
		    <TR>
			    <TD>Nombre del Contador :
			    </TD>
			    <TD>
				    <asp:TextBox id="tbnomcon" class="tmediano" runat="server"></asp:TextBox>
				    <asp:RequiredFieldValidator id="rfv3" runat="server" ControlToValidate="tbnomcon">*</asp:RequiredFieldValidator></TD>
		    </TR>
		    <TR>
			    <TD>Nombre del Revisor Fiscal :
			    </TD>
			    <TD>
				    <asp:TextBox id="tbnomrev" class="tmediano" runat="server"></asp:TextBox></TD>
		    </TR>
		    <TR>
			    <TD>Vigencia del Cliente :
			    </TD>
			    <TD>
				    <asp:DropDownList id="ddlvig" runat="server"></asp:DropDownList></TD>
		    </TR>
	    </TABLE>
    </asp:panel>
    <p></p>
    <p><asp:panel id="pnlContrato" runat="server" Width="400px" Visible="False">
		    <TABLE>
			    <TR>
				    <TD>Información del Contrato
				    </TD>
			    </TR>
			    <TR>
				    <TD></TD>
			    </TR>
			    <TR>
				    <TD>Fecha de Iniciación del Contrato
				    </TD>
				    <TD>
					    <asp:TextBox id="tbfecini" onkeyup="DateMask(this)" runat="server"></asp:TextBox>
					    <asp:RequiredFieldValidator id="rfv5" runat="server" ControlToValidate="tbfecini">*</asp:RequiredFieldValidator></TD>
			    </TR>
			    <TR>
				    <TD>Fecha de Finalización del Contrato
				    </TD>
				    <TD>
					    <asp:TextBox id="tbfecfin" onkeyup="DateMask(this)" runat="server"></asp:TextBox>
					    <asp:RequiredFieldValidator id="rfv6" runat="server" ControlToValidate="tbfecfin">*</asp:RequiredFieldValidator></TD>
			    </TR>
			    <TR>
				    <TD>Valor Mensual del Contrato
				    </TD>
				    <TD>
					    <asp:TextBox id="tbvalcon" onkeyup="NumericMask(this)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
					    <asp:RequiredFieldValidator id="rfv7" runat="server" ControlToValidate="tbvalcon">*</asp:RequiredFieldValidator></TD>
			    </TR>
			    <TR>
				    <TD>Número de Visitas Mensuales
				    </TD>
				    <TD>
					    <asp:TextBox id="tbnumvis" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
					    <asp:RequiredFieldValidator id="rfv8" runat="server" ControlToValidate="tbnumvis">*</asp:RequiredFieldValidator>
					    <asp:RegularExpressionValidator id="rev1" runat="server" ControlToValidate="tbnumvis" ValidationExpression="\d+">*</asp:RegularExpressionValidator></TD>
			    </TR>
		    </TABLE>
	    </asp:panel></p>
    <p><asp:panel id="pnlContactos" runat="server" Visible="False">
		    <TABLE>
			    <TR>
				    <TD>Información de los Contactos
				    </TD>
			    </TR>
			    <TR>
				    <TD></TD>
			    </TR>
			    <TR>
				    <TD>
					    <P>
						    <asp:DataGrid id="dgContactos" runat="server" CssClass="datagrid" OnCancelCommand="dgContactos_CancelCommand" OnEditCommand="dgContactos_EditCommand"
							    OnUpdateCommand="dgContactos_UpdateCommand" PageSize="15" GridLines="Vertical" AutoGenerateColumns="False"
							    ShowFooter="True" onItemCommand="dgContactos_ItemCommand">
							    <FooterStyle CssClass="footer"></FooterStyle>
						        <HeaderStyle CssClass="header"></HeaderStyle>
						        <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						        <ItemStyle CssClass="item"></ItemStyle>
							    <Columns>
								    <asp:TemplateColumn HeaderText="Nit del Contacto">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "NIT") %>
									    </ItemTemplate>
									    <FooterTemplate>
										    <asp:textbox id="tbnitcon" runat="server" onDblClick="ModalDialog(this,'SELECT CASE WHEN TNIT_TIPONIT = \'N\' THEN MNIT.mnit_nit concat \' - \' concat MNIT_APELLIDOS ELSE MNIT.mnit_nit concat \' - \' concat MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT_APELLIDO2,\'\') CONCAT \' \' CONCAT MNIT_NOMBRES CONCAT \' \' CONCAT coalesce(MNIT_NOMBRE2,\'\') END AS Nombre FROM mnit WHERE tVIG_VIGENCIA=\'V\' order by 1',new Array())"
											    ToolTip="Haga doble click para iniciar la busqueda, o si prefiere digite el nit del contacto" />
									    </FooterTemplate>
									    <EditItemTemplate>
										    <asp:Label id="lbednitcon" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "NIT") %>' />
									    </EditItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Sede donde labora el Contacto">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "SEDE") %>
									    </ItemTemplate>
									    <FooterTemplate>
										    <asp:textbox id="tbsedecon" runat="server" />
									    </FooterTemplate>
									    <EditItemTemplate>
										    <asp:textbox id="tbedsedcon" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SEDE") %>' />
									    </EditItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Departamento donde labora el Contacto">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "DEPTO") %>
									    </ItemTemplate>
									    <FooterTemplate>
										    <asp:textbox id="tbdeptocon" runat="server" />
									    </FooterTemplate>
									    <EditItemTemplate>
										    <asp:textbox id="tbeddepcon" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DEPTO") %>' />
									    </EditItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Cargo del Contacto">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "CARGO") %>
									    </ItemTemplate>
									    <FooterTemplate>
										    <asp:textbox id="tbcargcon" runat="server" />
									    </FooterTemplate>
									    <EditItemTemplate>
										    <asp:textbox id="tbedcarcon" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CARGO") %>' />
									    </EditItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Editar/Actualizar">
									    <ItemTemplate>
										    <asp:Button ID="Button1" runat="server" Text="Editar" CommandName="Edit" CausesValidation="false"></asp:Button>
									    </ItemTemplate>
									    <EditItemTemplate>
										    <asp:Button ID="Button2" runat="server" Text="Actualizar" CommandName="Update"></asp:Button>&nbsp;
										    <asp:Button ID="Button3" runat="server" Text="Cancelar" CommandName="Cancel" CausesValidation="false"></asp:Button>
									    </EditItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Insertar/Borrar">
									    <ItemTemplate>
										    <asp:Button id="btnbor" runat="server" Text="Eliminar" commandname="eliminar" />
									    </ItemTemplate>
									    <FooterTemplate>
										    <asp:Button id="btnagr" runat="server" Text="Agregar" commandname="agregar" />
									    </FooterTemplate>
								    </asp:TemplateColumn>
							    </Columns>
						    </asp:DataGrid>&nbsp;&nbsp;
					    </P>
				    </TD>
			    </TR>
		    </TABLE>
	    </asp:panel></p>
    <p><asp:panel id="pnlProductos" runat="server" Width="256px" Visible="False">
		    <TABLE>
			    <TR>
				    <TD>Productos que posee el Cliente
				    </TD>
			    </TR>
			    <TR>
				    <TD></TD>
			    </TR>
			    <TR>
				    <TD>
					    <asp:DataGrid id="dgProductos" runat="server" PageSize="15" cssclass="datagrid"
						    GridLines="Vertical" AutoGenerateColumns="False">
						    <FooterStyle CssClass="footer"></FooterStyle>
						    <HeaderStyle CssClass="header"></HeaderStyle>
						    <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						    <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						    <ItemStyle CssClass="item"></ItemStyle>
						    <Columns>
							    <asp:BoundColumn DataField="ID" ReadOnly="True" HeaderText="Id del Producto"></asp:BoundColumn>
							    <asp:BoundColumn DataField="NOMBRE" ReadOnly="True" HeaderText="Nombre del Producto"></asp:BoundColumn>
							    <asp:TemplateColumn HeaderText="Lo posee S/N">
								    <ItemTemplate>
									    <center>
										    <asp:CheckBox id="chbpos" runat="server" />
									    </center>
								    </ItemTemplate>
							    </asp:TemplateColumn>
						    </Columns>
					    </asp:DataGrid></TD>
			    </TR>
		    </TABLE>
	    </asp:panel></p>
    <p><asp:linkbutton id="lnbAnt" runat="server" CausesValidation="False" Enabled="False" onCommand="lnbAnt_Command">Anterior</asp:linkbutton>&nbsp;&nbsp;
	    <asp:linkbutton id="lnbSig" runat="server" onCommand="lnbSig_Command">Siguiente</asp:linkbutton></p>
    <p><asp:button id="btnGuardar" onclick="btnGuardar_Click" runat="server" Visible="False" CausesValidation="False"
		    Text="Guardar"></asp:button><asp:button id="btnCancelar" onclick="btnCancelar_Click" runat="server" CausesValidation="False"
		    Text="Cancelar"></asp:button></p>
    <p><asp:label id="lb" runat="server"></asp:label></p>
</fieldset>
