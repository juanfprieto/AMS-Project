<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenTrabajoCotizaciones.DatosOrden.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.DatosOrdenCotizaciones" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<style type="text/css">
    .style1
    {
        width: 223px;
    }
</style>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<table id="datosOrden" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<fieldset class="infield">
                    <legend class="Legends">Almacen - Prefijo</legend>
				    <p>
                        Taller:&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="almacen" width="180" runat="server" autopostback="true" onselectedindexchanged="Cambio_Taller"></asp:dropdownlist>
				    </p>
                    <p>
                        Orden :&nbsp;<asp:dropdownlist id="tipoDocumento" width="180" AutoPostBack="true"  runat="server" OnSelectedIndexChanged="CambioTipoOT"></asp:dropdownlist>
				    </p>
				</fieldset>
			</td>
			<td>
				<fieldset class="infield">
					<legend>Datos Orden</legend>
                    <p>
                        Nr Orden:&nbsp;
					    <asp:textbox id="numOrden" runat="server" ReadOnly="True" 
                            ToolTip="Este Campo no se deja Editar" width="45"></asp:textbox><asp:requiredfieldvalidator id="validatorNumOrden" runat="server" ControlToValidate="numOrden" Display="Dynamic"
						    Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorNumOrden2" runat="server" ControlToValidate="numOrden" Display="Dynamic"
						    Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:regularexpressionvalidator>
                        Placa:&nbsp;
					    <asp:textbox id="placa" width="55" runat="server"  MaxLength="8" onkeyup="aMayusculas(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorPlaca" runat="server" ControlToValidate="placa" Display="Dynamic" Font-Size="11"
						    Font-Name="Arial" ErrorMessage="Por favor ingresar Placa">*</asp:requiredfieldvalidator>
					</p>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td>
				<fieldset class="infield">
                    <legend>Cargo</legend>
                    <p>
                    Cargo :
					<asp:dropdownlist id="cargo" runat="server" Width="199px"></asp:dropdownlist></p>
					<p>Servicio:
						<asp:dropdownlist id="servicio" runat="server" Width="191px"></asp:dropdownlist></p>
					<p>Lista de Precios:
						<asp:dropdownlist id="listaPrecios" runat="server" Width="142px"></asp:dropdownlist></p>
				</fieldset>
			</td>
			<td>
				<fieldset class="infield">
					<p></p>
					<legend>
						Entrada</legend>Fecha :
					<asp:textbox id="fecha" runat="server" Width="130px" onKeyUp="DateMask(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorFecha" runat="server" ControlToValidate="fecha" Display="Dynamic" Font-Size="11"
						Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorFecha2" runat="server" ControlToValidate="fecha" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator>
					<p>Hora:
						<asp:textbox id="hora" runat="server" Width="38px"></asp:textbox><asp:requiredfieldvalidator id="validatorHora" runat="server" ControlToValidate="hora" Display="Dynamic" Font-Size="11"
							Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorHora2" runat="server" ControlToValidate="hora" Display="Dynamic" Font-Size="11"
							Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:regularexpressionvalidator>&nbsp;&nbsp;:&nbsp;&nbsp;
						<asp:textbox id="minutos" runat="server" Width="38px"></asp:textbox><asp:requiredfieldvalidator id="validatorMinutos" runat="server" ControlToValidate="minutos" Display="Dynamic"
							Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorMinutos2" runat="server" ControlToValidate="minutos" Display="Dynamic"
							Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{2}">*</asp:regularexpressionvalidator></p>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td>
				<fieldset class="infield">
					<p></p>
					<legend class="Legends">
						Recepcionista</legend>
					<!--	<p>Vendedor :
					<asp:dropdownlist id="codigoVende" runat="server" Width="190px"></asp:dropdownlist></p>
					-->	Codigo :
					<asp:dropdownlist id="codigoRecep" runat="server" Width="190px"></asp:dropdownlist>
					<p>Clave:&nbsp;<asp:textbox id="clave" runat="server" Width="57px" TextMode="Password"></asp:textbox>
						&nbsp;
					</p>
				</fieldset>
			</td>
			<td>
				<fieldset class="infield">
					<p></p>
					<legend class="Legends">
						Observaciones</legend>Observaciones Cliente:
					<p><asp:textbox id="obsrCliente" runat="server" Width="90%" TextMode="MultiLine" onblur="aMayusculas(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorObsrCliente" runat="server" ControlToValidate="obsrCliente" Display="Dynamic"
							Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator></p>
					<p>Observaciones Recepcionista:
					</p>
					<p><asp:textbox id="obsrRecep" runat="server" Width="90%" TextMode="MultiLine" onblur="aMayusculas(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorObsrRecep" runat="server" ControlToValidate="obsrRecep" Display="Dynamic"
							Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator></p>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td>
				<fieldset class="infield">
					<p></p>
					<legend class="Legends">
						Datos Transferencia</legend>Lista de Precios (Items Fuera de Kits) :
					<p><asp:dropdownlist id="listaPreciosItems" runat="server" Width="225px"></asp:dropdownlist></p>
					<p>Tipo&nbsp;Pedido:
						<asp:dropdownlist id="tipoPedido" Width="216px" runat="server"></asp:dropdownlist></p>
				</fieldset>
			</td>
			<td>
				<p><asp:button id="confirmar" onclick="Confirmar" runat="server" Width="207px" Text="Confirmar"></asp:button>&nbsp;<asp:label id="lbEstCita" runat="server" visible="False"></asp:label>
					<asp:ValidationSummary id="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
				</p>
			</td>
		</tr>
	</tbody>
</table>

<asp:label id="lb" runat="server"></asp:label>
<script type ="text/javascript">
function CambioTaller(obj)
{
	if(obj.options.length > 0)
	{
		DatosOrden.CambioTallerCarga(obj.value,CambioTaller_CallBack);
	}
	else
		obj.options.length = 0;
}

function CambioTaller_CallBack(response)
{
	if (response.error != null)
	{
		alert(response.error);
		return;
	}
	var ddlRecepcionistas = document.getElementById("<%=codigoRecep.ClientID%>");
	var opciones = response.value;
	ddlRecepcionistas.options.length = 0;
	ddlNitsTaller.options.length = 0;
	if (opciones == null || typeof(opciones) != "object")
	{
		return;
	}
	if(opciones.Tables[0].Rows.length > 0)
	{
		for (var i = 0; i < opciones.Tables[0].Rows.length; ++i)
			ddlRecepcionistas.options[ddlRecepcionistas.options.length] = new Option(opciones.Tables[0].Rows[i].PVEN_NOMBRE,opciones.Tables[0].Rows[i].PVEN_CODIGO);
		for (var j = 0; j < opciones.Tables[1].Rows.length; ++j)
			ddlNitsTaller.options[ddlNitsTaller.options.length] = new Option(opciones.Tables[1].Rows[j].PNITAL_NITTALLER,opciones.Tables[1].Rows[j].PNITAL_NITTALLER);
	}
}	
</script>
