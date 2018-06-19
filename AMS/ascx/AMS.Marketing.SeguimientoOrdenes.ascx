<%@ Control Language="c#" codebehind="AMS.Marketing.SeguimientoOrdenes.ascx.cs" autoeventwireup="True" Inherits="AMS.Marketing.SeguimientoOrdenes" %>
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
	<asp:TextBox id="detalleAccion" class="tgrande" textmode="multiline" runat="server" visible="False"></asp:TextBox>
	&nbsp;
</p>
<p>
	&nbsp;<asp:Label id="labelResultadoAccion" runat="server" visible="False">Resultado
    Acción</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:DropDownList id="resultadoActividad" class="dmediano" runat="server" visible="False"></asp:DropDownList>
</p>
<p>
	<asp:Label id="labelEjecutor" runat="server" visible="False">Ejecutor</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:TextBox id="ejecutor" class="tmediano" runat="server" visible="False"></asp:TextBox>
</p>
<p>
	<asp:Label id="labelFechaAccion" runat="server" visible="False">Fecha de la Acción</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:calendar BackColor=Beige id="calendario" runat="server" visible="False"></asp:Calendar>
	&nbsp;&nbsp;&nbsp;
</p>
<p>
	<asp:Button id="grabarAccion" onclick="Grabar_Accion" runat="server" text="Grabar Accíon" visible="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
