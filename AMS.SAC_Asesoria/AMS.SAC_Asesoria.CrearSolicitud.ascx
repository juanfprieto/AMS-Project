<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.SAC_Asesoria.CrearSolicitud.ascx.cs" Inherits="AMS.SAC_Asesoria.CrearSolicitud" %>
<fieldset>
	<asp:Panel id="pnlCliente" runat="server" Visible="False" Width="830px">
		<TABLE id="Table" class="filtersIn">
			<TR>
				<TD>Escoja el nit del cliente:</td>
                <td>
					<asp:DropDownList id="ddlnitcli" class="dmediano" runat="server" onSelectedIndexChanged="ddlnitcli_IndexChanged" AutoPostback="true"></asp:DropDownList>
                    </TD>
                     <td rowspan = "3">
					 <center><img height="100" src="../img/SAC.logo.sac.jpg" border="0"></center>                     
                </TD>
			</TR>
			<tr>
				<td>
                    Escoja el nit del contacto:
				</td>
                <td>
					<asp:DropDownList id="ddlnitcon" class="dmediano" runat="server"></asp:DropDownList>
                </td>
			</tr>
			<tr>
				<td>Tipo de Solicitud:
				   </td>
                <td>
                    <font size="3"> 
                        <ASP:RADIOBUTTON TEXT="Asesoría"   ID="ER" GROUPNAME="GRUPO1" RUNAT="SERVER" autopostback="true"  OnCheckedChanged="Aparecer_click"/> &nbsp &nbsp &nbsp
                        <ASP:RADIOBUTTON TEXT="Desarrollo" ID="DE" GROUPNAME="GRUPO1" RUNAT="SERVER" autopostback="true" style="display:none;" OnCheckedChanged="Desaparecer_click" /> 
                    </font>
                </td>               
			</tr>
		</TABLE>
	</asp:Panel>
    <p>
    <asp:Panel id="pnlDescripcion" runat="server" Visible="true" Width="830px">
	    <table id="Table1" class="filtersIn" >
		    <tbody><tr>
            <th class="filterHead">
			       <%-- <center><img height="100" src="../img/SAC.logo.sac.jpg" border="0"></center>--%>
		            </th></tr>
                    <tr>
                    <td>
                    <p>
						    <font face="Arial" color="black">En este menú, por favor ingrese toda la 
							    información referente a sus necesidades de servicio ya sea corrección de error 
							    o solicitud de desarrollo nuevo;&nbsp;NO olvide ser lo màs específico posible 
							    en su solicitud. </font>
					    </p>
                        <p>
						    <font face="Arial"><font color="black">La información suministrada por usted será vital 
								    al&nbsp;momento del análisis y solución de su problema,&nbsp;ésta&nbsp;nos 
								    ayudará a&nbsp;prestarle un servicio más&nbsp;eficiente. Nuestro éxito es su progreso y el de su empresa.</font> </font>
					    </p>
                    </td>
                    </tr>
		    </tbody>
	    </table>
    </asp:Panel>
    </p>
    <p>
	<asp:Panel id="pnlDescripcionSol" runat="server" Visible="true" Width="830px">
		<TABLE id="Table" class="filtersIn">
            <tr>    
                <td colspan="5"><center><font size="4"><b>Descripción de detalles de Solicitud</b></font></center><br></td>
            </tr>
			<TR id="TbSolicitud" runat="server">
				<Td>Ruta del módulo :
				</Td>
				<TD colspan = "4">
                    <asp:DropDownList id="ddlMenuPrincipal" class="dpequeno" runat="server" autopostback="true" onselectedindexchanged="ddlMenuPrin_OnSelectedIndexChanged"></asp:DropDownList>                   
                    <asp:DropDownList id="ddlmenuCarpeta" class="dpequeno" runat="server"   autopostback="true" onselectedindexchanged="ddlmenuCarpeta_OnSelectedIndexChanged"></asp:DropDownList>        
                    <asp:DropDownList id="ddlmenuOpcion" class="dmediano" runat="server"></asp:DropDownList>
			    </TD>
              
			</TR>
			<TR>
				<td>Datos específicos :
				</td>
				<TD colspan="2">
				    <asp:textbox id="txtEspecificos" runat="server"  TextMode="MultiLine" width="307px" Height="60" style="overflow:scroll; overflow-x: hidden;"
                    placeholder="Diligencie aqui los datos de entrada de proceso. Ej: Formato RC-555, asociado a NIT: 11999123, Fecha: 2006-11-27... todos"></asp:textbox>	
                </TD>
                <td colspan="2" rowspan="2" style="vertical-align: top;">
                    <fieldset style="padding: 5px;height: 166px;">
                        Adjunte los archivos de soporte de su solicitud
                        <br />
                        <INPUT id="uplFile" type="file" runat="server">
                        <asp:Button id="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" CausesValidation="False"></asp:Button>
                        <br />
                        Usted ha agregado lo siguientes archivos:
	                    <asp:Label id="lbArchivos" runat="server" forecolor="MidnightBlue"></asp:Label>
                    </fieldset>
                </td>
			</TR>
			<TR>
				<Td>
                    Descripción completamente detallada de la solicitud:
				</Td>
				<TD colspan="2">
				    <asp:TextBox id="txtDescripcion" runat="server" TextMode="MultiLine" width="307px" Height="100" style="overflow:scroll; overflow-x: hidden;"
                    placeholder="Ej: Revisar el Formato y NIT asociado a la factura de caja, se encuentra un error referente a la fecha asociada, ya que sale año 2016 y debería ser año 2017.">
                    </asp:textbox>
                </TD>
			</TR>
            <TR>
                <td colspan ="4">
                    <br>
                    <center><asp:Button id="btnLLenarSol" runat="server" Text="Agregar Solicitud "  onClick="Agregar_Descripcion" ></asp:Button></center>
                </td>

			</TR>
		</TABLE>
	</asp:Panel>
    </p>
    <p>
    <asp:Panel id="pnlSolicitudes" runat="server" Visible="true" Width="830px">
	    <asp:DataGrid id="dgSolicitud" runat="server" PageSize="15" cssclass="datagrid"
		    GridLines="Vertical" AutoGenerateColumns="False" ShowFooter="True"
		    onItemCommand="dgSolicitud_ItemCommand" OnItemDataBound="dgSolicitud_ItemDataBound">
		    <FooterStyle CssClass="footer"></FooterStyle>
		    <HeaderStyle CssClass="header"></HeaderStyle>
		    <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		    <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		    <ItemStyle CssClass="item"></ItemStyle>
		    <Columns>
			    <asp:TemplateColumn HeaderText="Detalle de la Solicitud">
				    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "DETALLE") %>
				    </ItemTemplate>
				    <FooterTemplate>
					    <asp:TextBox id="tbdet" runat="server" TextMode="MultiLine" width="400" Height="100" Enabled="false" Text="..." Visible="false"/>
					    <asp:RequiredFieldValidator id="rfv1" runat="server" ControlToValidate="tbdet">Campo Requerido</asp:RequiredFieldValidator>
				    </FooterTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="M&#243;dulo">
				    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "PROGRAMA") %>
				    </ItemTemplate>
				    <FooterTemplate>
					    <asp:DropDownList ID="ddlprog" Runat="server" Visible="false"></asp:DropDownList>
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
    </asp:Panel>
    </p>
    <fieldset>
	   <center>
       <asp:Button id="btnEnviar" onclick="btnEnviar_Click" runat="server" Text="Guardar" CausesValidation="False" Enabled="False"></asp:Button>
	    <asp:Button id="btnCancelar" onclick="btnCancelar_Click" runat="server" Text="Cancelar" CausesValidation="False"></asp:Button>&nbsp;
        </center>        
        <asp:Button id="bntGuardarSOl" onclick="btnGuardar_Click" runat="server" Text="Guardado Rapido" CausesValidation="False" Visible="false" Enabled="true"></asp:Button>
    </fieldset>
    <asp:Label id="lb" runat="server"></asp:Label>
</fieldset> 
