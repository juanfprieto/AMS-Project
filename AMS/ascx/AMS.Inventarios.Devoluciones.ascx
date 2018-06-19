<%@ Control Language="c#" codebehind="AMS.Inventarios.Devoluciones.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.Devoluciones" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<fieldset>    
  <table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
					    <tr>
		    			    <td>Sede:
                            
        						<br /><asp:dropdownlist id="ddlAlmacen" class="dmediano" runat="server"  OnSelectedIndexChanged="CambioAlmacen" AutoPostBack=true></asp:dropdownlist>
                                </td>
                                <td>
                              Vendedor Responsable:
								<br /><asp:dropdownlist id="ddlVendedor" class="dmediano" runat="server"></asp:dropdownlist></td>
                                </td>
						</tr>
                        <tr>
							<td>Nit:
								<br /><asp:textbox id="txtNIT" ondblclick="ModalDialog(this, 'SELECT mnit_nit as NIT, mnit_nombres concat \' \' concat mnit_apellidos as Nombre FROM mnit ORDER BY mnit_nit', new Array());"
									style="name: txtNIT" class="tpequeno" runat="server" ReadOnLy="false"></asp:textbox>
                                    <td> Nombre:
                                    <br /> <asp:textbox id="txtNITa" style="name: txtNIT" class="tpequeno" runat="server" ReadOnLy="true"></asp:textbox></td>
                                    </td>
                                   </tr>
                                   <tr>
							&nbsp;<td><asp:button id="btnSelNIT" onclick="SelNIT" runat="server" Text="Seleccionar"></asp:button></td>	
                            </tr>
                            </tr>					
						<asp:placeholder id="plcPrefNum" runat="server" Visible="false">
						<tr>
							<td>Prefijo Devolución :
								<asp:dropdownlist id="ddlCodigo" AutoPostBack="True" runat="server" 
                                    onselectedindexchanged="ddlCodigo_SelectedIndexChanged"></asp:dropdownlist><asp:Image id="imgPrefijo" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image></td>
							<td>Número Devolución :
								<asp:label id="lblNumDev" runat="server"></asp:label></td>
						</tr>
						<tr><td><asp:button id="Button1" onclick="CambiaNIT" runat="server" Text="Confirmar"></asp:button></td></tr></asp:placeholder>
						<asp:placeholder id="plhFact" runat="server">
							<TR>
								<TD>Factura a Cliente / Entrada de Almacén :
									<asp:DropDownList id="ddlFact" runat="server"></asp:DropDownList></TD>
								<TD align="right">
									<asp:Button id="btnSelFact" onclick="CambiaFact" runat="server" Text="Cargar"></asp:Button></TD>
							</TR>
							<TR>
								<TD colSpan="2">Fecha de Devolución :
									<asp:TextBox id="tbDate" runat="server" Width="78px" ReadOnly="True"></asp:TextBox><IMG onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'"
										src="../img/AMS.Icon.Calendar.gif" border="0">
									<TABLE id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
										onmouseout="calendar.style.visibility='hidden'">
										<TR>
											<TD>
												<asp:calendar BackColor=Beige id="calDate" runat="server" OnSelectionChanged="ChangeDate" enableViewState="true"></asp:Calendar></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD colSpan="2">Observaciones :
									<asp:TextBox id="txtObs" style="name: txtNIT" runat="server" Width="511px" MaxLength="100" Rows="5"></asp:TextBox></TD>
							</TR>
						</asp:placeholder>
					
			</td>
		</tr>
	</tbody>
    </table>
    </fieldset> 

<p><ASP:DATAGRID id="dgItems" runat="server" cssclass="datagrid"  enableViewState="true" ShowFooter="True"
		AutoGenerateColumns="false" OnItemCommand="DgInserts_AddAndDel" OnCancelCommand="DgInserts_Cancel"
		OnEditCommand="DgInserts_Edit" OnUpdateCommand="DgInserts_Update">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="dite_prefdocurefe" ReadOnly="True" HeaderText="Pref. Pedido :"></asp:BoundColumn>
			<asp:BoundColumn DataField="dite_numedocurefe" ReadOnly="True" HeaderText="Num. Pedido :"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_codigo" ReadOnly="True" HeaderText="Item :"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_nombre" ReadOnly="True" HeaderText="Nombre :"></asp:BoundColumn>
			<asp:BoundColumn DataField="plin_codigo" ReadOnly="True" HeaderText="Linea :"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_cantped" ReadOnly="True" HeaderText="Cantidad Inicial :"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Cantidad Devolver :">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "cantidad") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" CssClass="AlineacionDerecha" id="edit_1" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem, "cantidad") %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="mite_precio" ReadOnly="True" HeaderText="Valor Unidad :" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_desc" ReadOnly="True" HeaderText="Porcentaje Descuento :" DataFormatString="{0:N}%"></asp:BoundColumn>
			<asp:BoundColumn DataField="mite_iva" ReadOnly="True" HeaderText="Porcentaje Iva :" DataFormatString="{0:N}%"></asp:BoundColumn>
			<asp:BoundColumn DataField="msal_tot" ReadOnly="True" HeaderText="Valor Total :" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Operaciones :">
				<ItemTemplate>
					<p align="center">
						<asp:Button CommandName="DelDatasRow" Text="Quitar" ID="btnDel" Runat="server" width="80px" />
					</p>
				</ItemTemplate>
                <HeaderTemplate>
                    <center>
                        <asp:Label ID="lblChk" runat="server" Text="Selección"></asp:Label><br />
                        <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="Check_Clicked"/>
                    </center>
                </HeaderTemplate>
			    <ItemTemplate>
                    <center>
                        <asp:CheckBox ID="cbRows" runat="server"/>
                    </center>
                </ItemTemplate>
				<FooterTemplate>
					<p align="center">
						<asp:Button CommandName="ClearRows" Text="Reiniciar" ID="btnClear" Runat="server" width="80px" />
					</p>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
		</Columns>
		<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	</ASP:DATAGRID></p>
<p><br>
<fieldset>    
  <table id="Table2" class="filtersIn">
		<tbody>
			<tr>
				<td>
					
						Totales:
						
							<tbody>
								<tr>
									<td>Valor Neto Devolución:
									</td>
									<td><asp:textbox id="tbValNeto" class="tpequeno" runat="server" ReadOnly="True"></asp:textbox></td>
								
									<td>Valor Iva Devolución:
									</td>
									<td><asp:textbox id="tbValIva" class="tpequeno" runat="server" ReadOnly="True"></asp:textbox></td>
								</tr>
                                <br />
								<tr>
									<td>Numero Items:
									</td>
									<td><asp:textbox id="txtNumItem" class="tpequeno" runat="server" ReadOnly="True"></asp:textbox></td>
								
									<td>Numero Unidades:
									</td>
									<td><asp:textbox id="txtNumUnid" class="tpequeno" runat="server" ReadOnly="True"></asp:textbox></td>
								</tr>
                                <br />
								<tr>
									<td>Valor Fletes:
									</td>
									<td><asp:textbox id="txtvaloflet" class="tpequeno" runat="server" ReadOnly="False"  onkeyup="NumericMaskE(this,event)"></asp:textbox></td>
								
									<td>Valor iva Fletes:
									</td>
									<td><asp:textbox id="txtvaloivaflet" class="tpequeno" runat="server" ReadOnly="False"  onkeyup="NumericMaskE(this,event)"></asp:textbox></td>
								</tr>
							</tbody>
					
				</td>
			</tr>
			<tr>
				<td align="right">
					<p><asp:button id="btnAjus" runat="server" Text="Ejecutar" Enabled="False" onclick="NewAjust"></asp:button></p>
				</td>
			</tr>
		</tbody>
        </table>
        </fieldset>  
</p>
<asp:label id="lbInfo" runat="server"></asp:label>
