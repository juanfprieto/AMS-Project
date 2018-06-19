<%@ Control Language="C#" AutoEventWireup="false" Inherits="AMS.SAC_Asesoria.CrearOSC" %>
<link href="../css/SAC.css" rel="stylesheet" type="text/css" /> 
	<asp:Panel id="pnlDatos" runat="server">
		<fieldset>
            <table id="Table" class="filtersIn">
			    <tr>
				    <td colspan="2">
                        <h4>Escoja el número de la solicitud a la que le desea abrir Orden de Servicio:            		   
					    <asp:DropDownList id="ddlnumsol" runat="server" AutoPostBack="true" class="dpequeno" OnSelectedIndexChanged="cargarSolicitud"></asp:DropDownList>				  
                        </h4>
				    </td>
			    </tr>
			    <tr>
				    <td>				    
						<h4>
                            <asp:CheckBox id="chbase" runat="server" Text="Desea asignar un asesor?" checked="false" onCheckedChanged="chbase_CheckedChanged"
							AutoPostBack="True"></asp:CheckBox>
                        </h4>             
						<asp:Label id="lbEsc" runat="server" visible="False"><h4>Escoja un asesor:</h4></asp:Label>
						<asp:DropDownList id="ddlasesor" runat="server"  class="dmediano" Visible="False"></asp:DropDownList>  					                                            
                    </td> 
                    <td>
                         <asp:Label id="lbproyecto" runat="server" ><h4>Escoja el Proyecto:</h4></asp:Label>
						<asp:DropDownList id="ddlproyecto" runat="server"  class="dmediano" ></asp:DropDownList> 
                        <asp:Button id="btnAceptar" onclick="btnAceptar_Click" runat="server" Text="Aceptar" Visible="False"></asp:Button>                    
                        <asp:Button id="btnCancelar" onclick="btnCancelar_Click" runat="server" Text="Cancelar"></asp:Button>        
                    </td>
                    <td>
                        <div id="Div1" class="tarjetaInfo" runat="server">
                            <h4><asp:Label id="lbAsignado" runat="server" visible="False"></asp:Label></h4>
                        </div>
                    </td>
			    </tr>
		    </table>
        </fieldset>
	</asp:Panel>   
    <br />
    <asp:Panel id="pnlDetalleSolicitud" runat="server" Visible="False">
    <fieldset>
		<table id="Table1" class="" style="border-radius: 1em; background-color: white" >
			<tr>
				<td align="center" colSpan="1">
					<asp:Image id="Image1" runat="server" ImageUrl="../img/ecasIcono.png" visible="false"></asp:Image>
                </td>	
                <td align="left" colSpan="3>		
						<FONT face="Arial" color="#000000"><STRONG><h3>SOLICITUD DE ASESORIA</h3></STRONG></FONT>
				</td>
			</tr>
			<tr>
				<td colSpan="4">
                    <div id="tarjetaInfo" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de Solicitud</div>
                </td>
			</tr>			
			<tr>
				<td>
					<font color="#004080"><strong><h4>Número de la Solicitud:</h4><strong></font>
				</td>
				<td>
					<asp:Label id="lbnum" runat="server" font-names="Arial" font-size="Medium" forecolor="Black"></asp:Label>
                </td>		
				<td>
					<font color="#004080"><strong><h4>Fecha y Hora de la Solicitud:</h4></strong></font>
				</td>
				<td>
					<asp:Label id="lbfechor" runat="server"></asp:Label>
                </td>
			</tr>
			<tr>
				<td>
					<font color="#004080"><strong><h4>Nit del Cliente:</h4> </strong></font>
				</td>
				<td>
					<asp:Label id="lbnitcli" runat="server"></asp:Label>
                </td>				
				<td>
					<font color="#004080"><strong><h4>Razón Social del Cliente:</h4></strong></font>
				</td>
				<td>
					<asp:Label id="lbnomcli" runat="server"></asp:Label></td>
			</tr>			
			<tr>
				<td>
					<font color="#004080"><strong><h4>Cedula del contacto:</h4></strong></font>
				</td>
				<td>
					<asp:Label id="lbnitcon" runat="server"></asp:Label>
                </td>				
				<td>
					<font color="#004080"><strong><h4>Nombre del contacto:</h4></strong></font>
				</td>
				<td>
					<asp:Label id="lbnomcon" runat="server"></asp:Label></td>
			</tr>		
			<tr>
				<td colSpan="4">
                    <div id="divInfoSol" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Detalles de la Solicitud</div>
                </td>
			</tr>
			<tr>
				<td align="center" colspan="4">
                    <asp:DataGrid id="dgSolicitud" runat="server" cssclass="datagrid" PageSize="15" GridLines="Vertical" ShowFooter="false" AutoGenerateColumns="false"
                        OnEditCommand="DgInserts_Edit" OnCancelCommand="DgInserts_Cancel" OnUpdateCommand="DgInserts_Update" enableViewState="true">
                        <FooterStyle CssClass="footer"></FooterStyle>
                        <HeaderStyle CssClass="header"></HeaderStyle>
                        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                        <ItemStyle CssClass="item"></ItemStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="DETALLE">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DETALLE") %>
								</ItemTemplate>
							</asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="PROGRAMA" ItemStyle-Width="250px" >
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PROGRAMA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ESTIMADO (HORAS)" ItemStyle-Width="80px">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "HORAS") %>
								</ItemTemplate>
                                <EditItemTemplate>
									<asp:TextBox id="txtHoras" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "HORAS") %>' ></asp:TextBox>
								</EditItemTemplate>
							</asp:TemplateColumn>
                            <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar Hora"></asp:EditCommandColumn>
                        </Columns>
                    </asp:DataGrid>
				</td>
			</tr>
			<tr>
				<td colSpan="4">
                    <div id="divInfoAdjuntos" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Archivos Adjuntos de la Solicitud</div>
                </td>
			</tr>
			<tr>
				<td align="center" colspan="4">
					<asp:DataGrid id="dgArchivos" runat="server" cssclass="datagrid" PageSize="15" GridLines="Vertical" AutoGenerateColumns="false" ShowFooter="false">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="ARCHIVO" ReadOnly="True" HeaderText="Nombre del Archivo"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Descargar Archivo">
								<ItemTemplate>
									<center>
										<asp:HyperLink id="hpldes" runat="server">Descargar Archivo</asp:HyperLink>
									</center>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
				</td>
			</tr>            		
		</table>
    </fieldset>
   	</asp:Panel>
    <br />
<asp:panel id="pnlTareas" runat="server" visible="False">
<fieldset>
    <table class="filtersIn">
	    <TR>
	        <TD colspan="2" style="text-align: center;">
				<strong>
                Tareas que se crearan a la Orden de Servicio:
                </strong>					
			</TD>
		</TR>
        <tr>
            <td style="vertical-align : top;">
               Informacion de la tarea :
               <asp:textbox id="txtDetalle" runat="server"  TextMode="MultiLine" style="overflow:scroll;height: 100px;"></asp:textbox>	
            </td>
            <td style="vertical-align : top;">
               <label>Asesor a Asignar</label>
               <asp:DropDownList id="ddlAsesores" class="dgrande" runat="server" AutoPostback="true"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button id="btnLLenarTarea" runat="server" Text="Agregar Tarea "  onClick="Agregar_Tarea" ></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:DataGrid id="dgTareas" runat="server" PageSize="15" cssclass="datagrid"
		        GridLines="Vertical" AutoGenerateColumns="False" ShowFooter="True"
		        onItemCommand="dgTareas_ItemCommand">
		        <FooterStyle CssClass="footer"></FooterStyle>
		        <HeaderStyle CssClass="header"></HeaderStyle>
		        <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		        <ItemStyle CssClass="item"></ItemStyle>
		        <Columns>
			    <asp:TemplateColumn HeaderText="Descripcion Tarea">
				    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
				    </ItemTemplate>
				    <FooterTemplate>
					    <asp:TextBox id="tbTarea" runat="server" TextMode="MultiLine" width="400" Height="100" Enabled="false" Text="..." Visible="false"/>
				    </FooterTemplate>
			    </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Asesor">
				    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "ASESOR") %>
				    </ItemTemplate>
				    <FooterTemplate>
					    <asp:DropDownList ID="ddlAsesor" Runat="server" Visible="false"></asp:DropDownList>
				    </FooterTemplate>
			    </asp:TemplateColumn>           			    
			    <asp:TemplateColumn HeaderText="Agregar/Eliminar">
				    <ItemTemplate>
					    <center>
						    <asp:Button id="btnEli" runat="server" Text="Eliminar" CommandName="eliminar" CausesValidation="false" />
					    </center>
				    </ItemTemplate>
				    <FooterTemplate>
					    <center>
						    <asp:Button id="btnAdd" runat="server" text="Agregar" CommandName="agregar" Visible="false"/>
					    </center>
				    </FooterTemplate>
			    </asp:TemplateColumn>
		        </Columns>
	            </asp:DataGrid>
            </td>
        </tr>     
        </table>
    </fieldset>
</asp:panel>
<asp:Label id="lb" runat="server" font-names="Arial" font-size="Medium" forecolor="Black"></asp:Label>
	<%--<asp:Panel id="pnlDetalles" runat="server" Visible="False">
            <fieldset>
		        <TABLE class="filtersIn">
			        <TR>
				        <TD>
					        <h4>Detalles de la Solicitud que pasaran a la Orden de Servicio:</h4>					
				        </TD>
			        </TR>
			        <TR>
				        <TD>
                            <asp:DataGrid id="dgDetalle" runat="server" Visible="False" cssclass="datagrid" PageSize="15" GridLines="Vertical" AutoGenerateColumns="False">
					        <FooterStyle CssClass="footer"></FooterStyle>
					        <HeaderStyle CssClass="header"></HeaderStyle>
					        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
					        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					        <ItemStyle CssClass="item"></ItemStyle>
						        <Columns>
							        <asp:BoundColumn DataField="DETALLE" ReadOnly="True" HeaderText="Detalle"></asp:BoundColumn>
							        <asp:TemplateColumn HeaderText="Tipo de Asesoria">
								        <ItemTemplate>
									        <asp:DropDownList id="ddltipase" runat="server" />
								        </ItemTemplate>
							        </asp:TemplateColumn>
						        </Columns>
					        </asp:DataGrid>
				        </TD>
			        </TR>
                    <TR>
                        <td>
                            
                        </td>
                    </TR>
		        </TABLE>
            </fieldset>
	</asp:Panel>--%>
