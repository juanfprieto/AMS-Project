<%@ Control Language="C#" AutoEventWireup="false" Inherits="AMS.SAC_Asesoria.ResponderOSC" %>
<link href="../css/SAC.css" rel="stylesheet" type="text/css" /> 
<!doctype html>  
<fieldset>
    <TABLE id="Table" class="filtersIn">
    		<TR bgColor="white">
                <td align="right">
                </td>
				<TD align="center">				
					<FONT face="Arial" color="red" size="5">
                        <STRONG>ORDEN DE SERVICIO N º</STRONG>
                    </FONT>
						<asp:Label id="lbnum" runat="server" font-names="Arial Black" font-size="x-Large" forecolor="#004080"></asp:Label>               
				</TD>
			</TR>
            <TR bgColor="white">				
                <td colSpan="2"> 
                    <div id="tarjetaInfo" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de la Solicitud del Cliente</div>
                </td>
			</TR>
            <TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Realizada bajo Solicitud Nº:</strong></FONT>
					<asp:Label id="lbnumsol" runat="server"></asp:Label></h4>
                </TD>		
                <TD>
                    <h4><FONT color="#004080"><strong>Fecha y Hora de la Solicitud:</strong></FONT>
					<asp:Label id="lbfechorsol" runat="server"></asp:Label></h4>
                </TD>		
			</TR>
            <TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Nit del Cliente:</strong></FONT>
					<asp:Label id="lbnitcli" runat="server"></asp:Label></h4>
                </TD>
				<TD>
                    <h4><FONT color="#004080"><strong>Razón Social del Cliente:</strong></FONT>
					<asp:Label id="lbnomcli" runat="server"></asp:Label></h4>
                </TD>
			</TR>			
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Cedula del Contacto:</strong></FONT>
					<asp:Label id="lbcedcon" runat="server"></asp:Label></h4>
                </TD>				
				<TD>
                    <h4><FONT color="#004080"><strong>Nombre del Contacto:</strong></FONT>
					<asp:Label id="lbnomcon" runat="server"></asp:Label></h4>
                </TD>
			</TR>	
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Solicitud Realizada Via:</strong></FONT>
					<asp:Label id="lbsolvia" runat="server"></asp:Label></h4>
                </TD>
                <td></td>				
			</TR>
			<TR bgColor="white">
                <td colSpan="2">
                    <div id="Div1" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de la Orden de Servicio al Cliente</div>
                </td>
			</TR>
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Persona que abrio la Orden de Servicio:</strong></FONT>
					<asp:Label id="lbaseaper" runat="server"></asp:Label></h4>
                </TD>				
				<TD>
                    <h4><FONT color="#004080"><strong>Fecha y Hora de Apertura:</strong></FONT>
					<asp:Label id="lbfechoraper" runat="server"></asp:Label></h4>
                </TD>
			</TR>
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Asesor Asignado:</strong></FONT>
					<asp:Label id="lbaseasig" runat="server"></asp:Label></h4>
                </TD>
				<TD>
                    <h4><FONT color="#004080"><strong>Fecha y Hora de Asignación:</strong></FONT>
					<asp:Label id="lbfechorasig" runat="server"></asp:Label></h4>
                </TD>
			</TR>
			<TR bgColor="white">
				<TD colspan="2">
                    <h4><FONT color="#004080"><strong>Estado de la Orden de Servicio:</strong></FONT>
					<asp:Label id="lbestosc" runat="server" style="color: red;"></asp:Label></h4>
                </TD>				
			</TR>
            <TR bgColor="white">
				<td colSpan="2">
                    <div id="Div2" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de Solicitudes y Respuestas</div>
                </td>
			</TR>
			    <asp:DataGrid id="dgRespuesta" CssClass="datagrid" AutoGenerateColumns="False" GridLines="Vertical" PageSize="15" runat="server" 
                    onUpdateCommand="dgRespuesta_UpdateCommand" onCancelCommand="dgRespuesta_CancelCommand" onEditCommand="dgRespuesta_EditCommand">
                        <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
                        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
                        <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
                        <ItemStyle forecolor="Black" backcolor="#FFFFFF"></ItemStyle>
                        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="NUMERO" ReadOnly="True" HeaderText="ID. Orden"></asp:BoundColumn>
                                <asp:BoundColumn DataField="SOLICITUD" ReadOnly="True" ItemStyle-Width="295px" HeaderText="Detalle de la Solicitud"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Respuesta Tecnica" ItemStyle-Width="250px">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "RESPUESTA") %> 
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RESPUESTA") %>' TextMode="MultiLine" id="tbRes" style="height: 100%; width: 100%;"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Respuesta Cliente" ItemStyle-Width="250px">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "RESPUESTACLIENTE") %> 
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RESPUESTACLIENTE") %>' TextMode="MultiLine" id="tbResCli" style="height: 100%; width: 100%;"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Orígen del Error" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                       <%# DataBinder.Eval(Container.DataItem, "ORIGENERROR") %> 
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList id="ddlOrigenError"  runat="server" style="width: 144px;font-size: 11px;"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Tipo de respuesta" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "TIPORESPUESTA") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList id="ddlTipoRespuesta"  runat="server" style="width: 99px;font-size: 11px;"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" HeaderText="Acción" CancelText="Cancelar" EditText="Editar" ItemStyle-Width="110px"></asp:EditCommandColumn>
                            </Columns>
                    </asp:DataGrid>
			<TR bgColor="white">
		        <td colSpan="2">
                    <div id="Div3" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Archivos Adjuntos</div>
                </td>
			</TR>
            <TR bgColor="white">
				<TD colSpan="2">
                    <fieldset>
                        <table>
                            <tr>
                                <td>
                                    Adjunte los archivos de soporte de cierre de Solicitud
                                    <br />
                                    <input id="uplFile" type="file" runat="server" /> 
                                </td>
                                <td>
                                    <asp:Button id="btnAgregar" onclick="btnAgregar_Click" runat="server" Height="30px" Text="Agregar"></asp:Button>
                                    <br />
                                    Usted ha agregado lo siguientes archivos: <br />
                                    <asp:Label id="lbArchivos" runat="server" forecolor="MidnightBlue"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </TD>
            </TR>
    </TABLE>
</fieldset>
<br />
<fieldset>
    <TABLE id="Table1" class="filtersIn" style="width: 336px; text-align: center;">
        <tr>
            <td align="center">
                <asp:CheckBox id="chbCierre" runat="server" Text=" Desea cerrar la orden de servicio ?" Width="10%"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button id="btnGuardar" onclick="btnGuardar_Click" runat="server" Text="Guardar Solución"></asp:Button>
                <asp:Button id="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                <asp:Button id="btnCancelar" onclick="btnCancelar_Click" runat="server" Text="Cancelar" Visible="false"></asp:Button>
            </td>
        </tr>
    </TABLE>
    
</fieldset><br />

    <p>
        <asp:Label id="lb" runat="server"></asp:Label>
    </p>    