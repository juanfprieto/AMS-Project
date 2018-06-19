<%@ Control Language="c#" Debug="true" codebehind="AMS.Marketing.SeguimientoOrdenes.ascx.cs" autoeventwireup="True" Inherits="AMS.Marketing.SeguimientoOrdenes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>


<%--<script type="text/javascript" >
    function habilitaCorreo(obj)
    {
        if(iobj.val() == true)
        {
            var checkeado = document.getElementById('<%=txtCorreo.ClientID%>');
            checkeado.disabled = true;
        }
        else 
        {
            checkeado.disabled = false;
        }
    }
</script>--%>
<fieldset>
<table id="Table" class="filtersIn">
<tbody>
<tr>
<td>
<p>
	En este proceso defina todas las actividades y sus resultados ejecutadas sobre 
	las Ordenes de Trabajo del Taller de Servicio, el ejecutante y fecha de la 
	acción.
</p>
<p>
	Tipo de Orden de Trabajo&nbsp;
	<asp:DropDownList id="tipoDocumento" runat="server" class="dmediano" AutoPostBack="True" onselectedindexchanged="tipoDocumento_SelectedIndexChanged"></asp:DropDownList>
</p>
<p>
	Número de la Orden&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:DropDownList id="numeroOrden" class="dpequeno" runat="server"></asp:DropDownList>
	&nbsp;&nbsp;
	<asp:Label id="labelPropietario" runat="server" class="lmediano"></asp:Label>
</p>
<p>
	<asp:Button id="confirmarOrden" onclick="Confirmar_Orden" runat="server" Text="Confirmar Orden"></asp:Button>
</p> 
</td>
</tr>
</tbody>
</table>
</fieldset>

<p>
	<asp:DataGrid id="acciones" runat="server" cssclass="datagrid"
		GridLines="Vertical" AutoGenerateColumns="false">
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
        <HeaderStyle CssClass="header"></HeaderStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="CODIGO DE LA ACCION">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGOACCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DESCRIPCION DE LA ACCION">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DESCRIPCIONACCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="RESULTADO DE LA ACCION">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "RESULTADOACCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="DETALLE DEL RESULTADO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DETALLERESULTADO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="EJECUTOR DE LA ACCION">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "EJECUTORACCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="FECHA DE LA ACCION">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "FECHAACCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Label id="labelTipoActividad" runat="server" visible="False">Tipo de Actividad
    :</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:DropDownList id="tipoActividad" class="dmediano" runat="server" visible="False"></asp:DropDownList>
	&nbsp;
</p>
<p>
	<asp:Label id="labelDetalleAccion" runat="server" visible="False">Detalle de la Acción</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:TextBox id="detalleAccion" class="tgrande" textmode="multiline" runat="server" visible="False" Width="90%"></asp:TextBox>
	&nbsp;
</p>
<fieldset id="fldContent" runat="server" visible ="false" style="margin:initial;">
    <p>
        <asp:Table ID="tablita" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                        &nbsp;<asp:Label id="labelResultadoAccion" runat="server" visible="False">Resultado
                        Acción</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
	                    <asp:DropDownList id="resultadoActividad" class="dmediano" runat="server" visible="False"></asp:DropDownList>
                    
                </asp:TableCell>
                <asp:TableCell >
                    <asp:FileUpload ID="fileUpload1" runat="server" visible="False" AllowMultiple="true" style="display: -webkit-inline-box;" /> &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbCarga" runat ="server"></asp:Label>
                    <br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label id="labelEjecutor" runat="server" visible="False">Ejecutor</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
                    <asp:TextBox id="ejecutor" class="tmediano" runat="server" visible="False"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell >
                    <asp:Label ID="lbCorreo" runat="server" Visible="false" Text="Enviar imágenes a esta Dirección correo electrónico: "></asp:Label> <br />
                    <asp:TextBox ID="txtCorreo" runat="server" placeholder="ejemplo@correo.com" Width="280px" Visible="false"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell><br />
                    <asp:CheckBox ID="chkCorreo" runat="server" Text="habilitar envio de Correo" OnCheckedChanged="habilitaCorreo" AutoPostBack="true" />
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell> <br />
                    <asp:Label id="labelFechaAccion" runat="server" visible="False">Fecha de la Acción</asp:Label>
                    <asp:calendar BackColor="Beige" id="calendario" runat="server" visible="False" ></asp:calendar>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </p>	
    <p>
	    <asp:Button id="grabarAccion" onclick="Grabar_Accion" runat="server" text="Grabar Accíon" visible="False"></asp:Button>
    </p>
    <asp:Label id="lb" runat="server"></asp:Label>
    </fieldset>
    <%--<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="subir_Imagen" visible="False" />--%>
    

	

