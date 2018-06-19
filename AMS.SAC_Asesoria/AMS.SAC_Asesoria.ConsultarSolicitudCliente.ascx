<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.SAC_Asesoria.ConsultarSolicitudCliente.ascx.cs" Inherits="AMS.SAC_Asesoria.ConsultarSolicitudCliente" %>
<style>

</style>
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }

    var dialogEnc;
    $(document).on("ready", inicio);

    function inicio() {
        dialogEnc = $("#divEncuesta").dialog({
            autoOpen: false,
            height: 455,
            width: 350,
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            }
        });
    }

    function enviarEncuesta(obj) {
        var id = $(obj).closest("div").prop("id").replace("divDetalle", "");
        $("#" + "<%=lblID.ClientID%>").text("Solicitud No. " + id);
        document.getElementById('<%=hdLabelVal.ClientID %>').value = id;

        $(".ui-widget-overlay").css({ background: "#000", opacity: 0.46 });
        dialogEnc.dialog("open");
        //dialogEnc.dialog("close");
    }

    function MostrarDiv(idDiv)
	{
		var divDetalle = document.getElementById(idDiv);
		if(divDetalle.style.display == 'none')
			divDetalle.style.display = '';
		else
			divDetalle.style.display = 'none';
	}

</script>
<link href="../css/SAC.css" rel="stylesheet" type="text/css" /> 
<table>
    <tr style="text-align: right;">
        <td align="left" style="width: 50%;">
            <div id="filtroEmpresa" runat="server" visible="false"><b>Filtro por Empresa:</b><asp:DropDownList id="ddlEmpresa" class="dmediano" runat="server" onSelectedIndexChanged="DdlChanged_Empresa" AutoPostback="true"></asp:DropDownList>
                <br /><asp:CheckBox id="chkCerradas" runat="server" AutoPostBack="True" Text="&nbsp;&nbsp;Mostrar Solicitudes Cerradas" TextAlign="Right"  OnCheckedChanged="Check_MostrarCerradas"/>
            </div>
        </td>
        <td>
            <ul style="margin: 1.2px;">
                <li style="background:rgba(201, 201, 164, 0.7);box-shadow: 5px 5px 10px;border-radius: 0.5em;display:inline-block; display:inline-block;">
                        <h5>En Revisión</h5>
                </li>
                <li style="background:rgba(0, 163, 255, 0.7); box-shadow: 5px 5px 10px; border-radius: 0.5em; display:inline-block; display:inline-block;">
  	                    <h5>Asignada</h5>
                </li>
  	            <li style="background:rgba(255, 216, 0, 0.7);box-shadow: 5px 5px 10px;border-radius: 0.5em; display:inline-block; display:inline-block;">
                        <h5>Proceso</h5>
                </li>
  	            <li style="background:rgba(245, 165, 54, 0.8);box-shadow: 5px 5px 10px;border-radius: 0.5em; display:inline-block; display:inline-block;">
                        <h5>Control Calidad</h5>
                </li>
  	            <li style="background:rgba(238, 130, 238, 0.7);box-shadow: 5px 5px 10px;border-radius: 0.5em; display:inline-block; display:inline-block;">
                        <h5>Implementada</h5>
                </li>
  	            <li style="background:rgba(0, 255, 0, 0.7);box-shadow: 5px 5px 10px;border-radius: 0.5em;display:inline-block; display:inline-block;">
                        <h5>Cerrada</h5>
                </li>
            </ul>
        </td>
    </tr>
</table>
<asp:DataGrid id="dgSols" AutoGenerateColumns="False" cssclass="datagrid" GridLines="Horizontal"
	PageSize="15" runat="server" Visible="true" OnItemDataBound="dgSols_ItemDataBound" OnItemCommand=dgSols_ItemCommand>
	<FooterStyle CssClass="footer"></FooterStyle>
	<HeaderStyle CssClass="header"></HeaderStyle>
	<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
	<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
	<ItemStyle CssClass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="&nbsp;&nbsp;SOLICITUDES Y ORDENES PENDIENTES...">
			<ItemTemplate>
				<table>
					<tr>
                        <td>
                            <asp:Panel id="divSolicitud" runat="server">
                                <a href="javascript:MostrarDiv('divDetalle<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>');">
                                    <div id="divNumero" class='tarjetaN' runat= "server">
                                        Nº.<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
                                    </div>
                                </a>
                                <div runat= "server" style="display:inline-block"> 
                                    <a href="javascript:MostrarDiv('divDetalle<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>');"> <%# DataBinder.Eval(Container.DataItem, "CONTACTO") %>. [<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FECHA")).ToString("yyyy-MM-dd") %>]</a>
                                    <br>
                                    <asp:LinkButton ID=lbnorden Runat=server Visible=False></asp:LinkButton>
                                </div>
                                <div  style="text-align:right;" class="tarjetaE">
                                    <%# DataBinder.Eval(Container.DataItem, "CLIENTE") %>
                                </div>
                            </asp:Panel>
                            <div id="dias" style="text-align:right;display:inline-block;position:relative;right:-10%;"><%# DataBinder.Eval(Container.DataItem, "diasespera") %></div>
                        </td>
                    </tr>
					<tr>
						<td>
							<div id='divDetalle<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>' style="display:none; text-align:left; width:650px; margin-left: 43px;">
								<TABLE id="Table1" style="BACKGROUND-COLOR: white; padding: 5px; width:650px; " bgColor="white">
						            <TR>
                                        <td align="center">
                                            <asp:Image id="Image1" runat="server" ImageUrl="../img/AMS.LogoEmpresa.png" style="width: 27px;"></asp:Image>
                                        </td>
								        <td  colSpan="2">    
								            <FONT face="Arial" color="#000000" size="3"><STRONG>SOLICITUD DE ASESORIA</STRONG></FONT>    
							            </TD>
                                        <TD style="text-align:right; width: 220px;">								           
                                            <asp:ImageButton id="imgAsignarSol"         ImageUrl="../img/OSC_Asignar.png"       oncommand="AsignarSol"          ToolTip="Asignar"           runat="server" class="imgSol" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>'    AutoPostBack="true"  Visible="false"></asp:ImageButton>
                                            <asp:ImageButton id="imgDesarrolloSol"      ImageUrl="../img/OSC_Desarrollo.png"    oncommand="DesarrollarSol"      ToolTip="Desarrollo"        runat="server" class="imgSol" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>'    AutoPostBack="true"  Visible="false"></asp:ImageButton>
                                            <asp:ImageButton id="imgControlCalidadSol"  ImageUrl="../img/OSC_Calidad.png"       oncommand="ControlCalidadSol"   ToolTip="Control Calidad"   runat="server" class="imgSol" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>'    AutoPostBack="true"  Visible="false"></asp:ImageButton>
                                            <asp:ImageButton id="ImgImplementadaSol"    ImageUrl="../img/OSC_Implementada.png"  oncommand="ImplementarSol"      ToolTip="Implementada"      runat="server" class="imgSol" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>'    AutoPostBack="true"  Visible="false"></asp:ImageButton>
                                            <asp:ImageButton id="imgCerrarSol"          ImageUrl="../img/OSC_Cerrar.png"        onClientClick='enviarEncuesta(this); return false;'           ToolTip="Cerrar"            runat="server" class="imgSol" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>'    AutoPostBack="false"  Visible="false"></asp:ImageButton>
							            </TD>
						            </TR>
						            <TR>
							            <TD colSpan="4">
                                            <div id="tarjetaInfo" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de Solicitud</div>
                                        </TD>
						            </TR>
						            <TR>
							            <TD>
                                            <FONT color="#004080"><h5><strong> Número de la Solicitud :</strong></h5></FONT>
							            </TD>
							            <TD>
								            <h5><%# DataBinder.Eval(Container.DataItem, "NUMERO") %></h5>
								        </TD>
                                        <TD>
                                            <FONT color="#004080"><STRONG><h5>Fecha y Hora de la Solicitud:</h5></STRONG></FONT>
							            </TD>
							            <TD>
								            <h5><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FECHA")).ToString("yyyy-MM-dd") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "HORA")%></h5>
                                        </TD>
						            </TR>
						            <TR>
							            <TD>
                                            <STRONG><FONT color="#004080"><h5>Nit del Cliente:</h5></FONT> </STRONG>
							            </TD>
							            <TD>
								            <h5><%# DataBinder.Eval(Container.DataItem, "NITCLI") %></h5>
                                        </TD>
							            <TD>
                                            <FONT color="#004080"><STRONG><h5>Razón Social del Cliente:</h5></STRONG></FONT>
							            </TD>
							            <TD>
								            <h5><%# DataBinder.Eval(Container.DataItem, "CLIENTE") %></h5>
                                        </TD>
						            </TR>
						            <TR>
							            <TD>
                                            <FONT color="#004080"><STRONG><h5>Cédula del contacto:</h5></STRONG></FONT>
							            </TD>
							            <TD>
								            <h5><%# DataBinder.Eval(Container.DataItem, "NITCON") %></h5>
                                        </TD>
							            <TD>
                                            <FONT color="#004080"><STRONG><h5>Nombre del contacto:</h5></STRONG></FONT>
							            </TD>
							            <TD>
								            <h5><%# DataBinder.Eval(Container.DataItem, "CONTACTO") %></h5>
                                        </TD>
						            </TR>
						            <TR>
							            <TD colSpan="4">
                                            <div id="divInfoSol" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Detalles de la Solicitud</div>
                                        </TD>
						            </TR>
						            <TR>
							            <TD align="center" colSpan="4">
								            <asp:DataGrid id="dgSolicitud" runat="server" CssClass="datagrid" PageSize="15" GridLines="Vertical" ShowFooter="false" AutoGenerateColumns="false">
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
                                                    <asp:TemplateColumn HeaderText="PROGRAMA">
								                        <ItemTemplate>
									                        <%# DataBinder.Eval(Container.DataItem, "PROGRAMA") %>
								                        </ItemTemplate>
							                        </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="HORAS">
								                        <ItemTemplate>
									                        <%# DataBinder.Eval(Container.DataItem, "HORAS") %>
								                        </ItemTemplate>
							                        </asp:TemplateColumn>
									            </Columns>
								            </asp:DataGrid>
                                        </TD>
						            </TR>
						            <TR>
							            <TD colSpan="4">
                                            <div id="divInfoAdjuntos" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Archivos Adjuntos de la Solicitud</div>
                                        </TD>
						            </TR>
						            <TR>
							            <TD align="center" colSpan="4">
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
                                        </TD>
						            </TR>
					            </TABLE>        
							</div>
						</td>
					</tr>
				</table>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
<div id="divEncuesta" title="Encuesta de Servicio" >
    <asp:label id="lblID" runat="server"></asp:label><br /><br />
    Con el propósito  de mejorar nuestro servicio, le agradecemos se sirva diligenciar la presente encuesta.
    <br /><br />
    La calidad del servicio prestado para resolver su Orden de Servicio fue: <br /><br />
    <asp:RadioButton id="rdExcelente" Text="Excelente" Checked="false" GroupName="rdGrupo" runat="server" /><br />
    <asp:RadioButton id="rdBuena" Text="Buena" Checked="false" GroupName="rdGrupo" runat="server" /><br />
    <asp:RadioButton id="rdRegular" Text="Regular" Checked="false" GroupName="rdGrupo" runat="server" /><br />
    <asp:textbox id="txtObservacion" runat="server"  TextMode="MultiLine" width="250px" Height="70" style="overflow:scroll; overflow-x: hidden;"
    placeholder="Observaciones..."></asp:textbox>	<br /><br />
    Gracias por su colaboración.<br /><br />
    <asp:Button id="btnCerrarOrden" onClick="CerrarSol" runat="server"  Text="Cerrar Solicitud"  ></asp:Button>
</div>  
<asp:HiddenField ID="hdLabelVal" runat="server" />
</p>
<p>
<asp:Label id="lb" runat="server"></asp:Label>
</p>
