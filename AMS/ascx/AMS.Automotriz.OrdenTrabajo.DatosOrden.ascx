<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenesTaller.DatosOrden.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.DatosOrden" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type="text/javascript">
    function abrirEmergente1() {
        var placaOT = document.getElementById("<%=placa.ClientID%>");
        ModalDialog(placaOT, 'SELECT MCAT_PLACA as PLACA, PCAT.PCAT_CODIGO AS CATALOGO, PCAT_DESCRIPCION AS DESCRIPCION, MCAT_VIN AS VIN, MCAT_MOTOR AS MOTOR, MNIT_NIT AS NIT, MCAT_CONCVEND AS CONCECIONARIO ' +
                             'FROM MCATALOGOVEHICULO MCAT, PCATALOGOVEHICULO PCAT ' +
                             'WHERE PCAT.PCAT_CODIGO=MCAT.PCAT_CODIGO;', new Array(), 1);
    }
</script>
<style type="text/css">
    .style1
    {
        width: 223px;
    }
</style>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type="text/javascript" >
    function cambioPrefijo() {
        var tipoDoc = $('#<%=tipoDocumento.ClientID %>').val();
        DatosOrden.cambio_TipoOT(tipoDoc, cambiar_Prefijo_Callback);
    }
    function cambiar_Prefijo_Callback(response) {
        var prefijo = response.value;
        $('#<%=numOrden.ClientID%>').val(prefijo);
    }
</script>




<table id="datosOrden" class="filtersInAuto">
	<tbody>
		<tr>
			<td>
				<fieldset class="infield" >
                    <legend class="Legends">Almacén - Prefijo</legend>
				    <p>
                        Taller:&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="almacen" width="180" runat="server" autopostback="true" onselectedindexchanged="Cambio_Taller"></asp:dropdownlist>
				    </p>
                    <p>
                        Orden :&nbsp;<asp:dropdownlist id="tipoDocumento" width="180" runat="server" onChange="cambioPrefijo()" AutoPostBack="false" ></asp:dropdownlist>
                       <%-- Orden :&nbsp;<asp:dropdownlist id="tipoDocumento" width="180" runat="server" AutoPostBack="False" OnSelectedIndexChanged="cambioPrefijo"></asp:dropdownlist>--%>
				    </p>
				</fieldset>
			</td>
			<td>
				<fieldset class="infield">
					<legend>Datos Orden</legend>
                        <table>
                            <tr>
                                <td>
                                    Número Orden:&nbsp;
                                    <asp:textbox id="numOrden" runat="server" ReadOnly="True" 
                                        ToolTip="Este Campo no se deja Editar" width="70"></asp:textbox><asp:requiredfieldvalidator id="validatorNumOrden" runat="server" ControlToValidate="numOrden" Display="Dynamic"
						                Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorNumOrden2" runat="server" ControlToValidate="numOrden" Display="Dynamic"
						                Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:regularexpressionvalidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                     Placa:&nbsp;
                                    <asp:TextBox id="placa" width="80px" runat="server"  MaxLength="8" onkeyup="aMayusculas(this)" ondblclick="ModalDialog(this,'SELECT MCAT_PLACA, PCAT.PCAT_CODIGO, PCAT_DESCRIPCION, MCAT_VIN, MCAT_MOTOR, MNIT_NIT FROM MCATALOGOVEHICULO MCAT, PCATALOGOVEHICULO PCAT WHERE PCAT.PCAT_CODIGO=MCAT.PCAT_CODIGO;',new Array(),1)"></asp:TextBox>
                                    <asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" ControlToValidate="placa" Display="Dynamic" Font-Size="11"
						                Font-Name="Arial" ErrorMessage="Por favor ingresar Placa">*</asp:requiredfieldvalidator>
                                    <asp:Image id="imglupa3" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente1();"></asp:Image>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Rombo Entrada:&nbsp;
                                    <asp:textbox id="entrada" width="45" runat="server" MaxLength="3"></asp:textbox><asp:requiredfieldvalidator id="validatorEntrada" runat="server" ErrorMessage="Rombo Entrada es un campo necesario" ControlToValidate="entrada"  Display="Dynamic"
							            Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Locker:&nbsp;
                                    <asp:textbox id="locker" width="95" runat="server"  MaxLength="3"></asp:textbox>
                                </td>
                            </tr>
                        </table>

                    <%--<p></p>
                    <p></p>--%>
				</fieldset>
			</td>
            <td>
				<fieldset class="infield">
                    <legend>Cargo</legend>
                    <p>
                    Cargo :
					<asp:dropdownlist id="cargo" runat="server" Width="199px"></asp:dropdownlist></p>
					<p>Servicio:
						<asp:dropdownlist id="servicio" runat="server" Width="191px"></asp:dropdownlist></p>
					<p>Lista de Precios:
						<asp:dropdownlist id="listaPrecios" runat="server" class="dmediano"></asp:dropdownlist></p>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td>
				<fieldset class="infield">
					<p></p>
					<legend>
						Entrada</legend>Fecha :
					<asp:textbox id="fecha" runat="server" Width="130px" onKeyUp="DateMask(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorFecha" runat="server" ControlToValidate="fecha" Display="Dynamic" Font-Size="11"
						Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorFecha2" runat="server" ControlToValidate="fecha" Display="Dynamic"
						Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:regularexpressionvalidator>
					<p>Hora:
						<asp:textbox id="hora" runat="server" Width="44px" maxvalue="23" maxtext="2"></asp:textbox><asp:requiredfieldvalidator id="validatorHora" runat="server" ControlToValidate="hora" Display="Dynamic" Font-Size="11"
							Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorHora2" runat="server" ControlToValidate="hora" Display="Dynamic" Font-Size="11"
							Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:regularexpressionvalidator>&nbsp;&nbsp;:&nbsp;&nbsp;
						<asp:textbox id="minutos" runat="server" Width="44px" maxvalue="60" maxtext="2"></asp:textbox><asp:requiredfieldvalidator id="validatorMinutos" runat="server" ControlToValidate="minutos" Display="Dynamic"
							Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="validatorMinutos2" runat="server" ControlToValidate="minutos" Display="Dynamic"
							Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{2}">*</asp:regularexpressionvalidator></p>
				</fieldset>
			</td>
            <td>
				<fieldset class="infield">
					<p></p>
					<legend class="Legends">
						Observaciones</legend>El Cliente Manifiesta:
					<p><asp:textbox id="obsrCliente" runat="server" Width="90%" TextMode="MultiLine" onblur="aMayusculas(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorObsrCliente" runat="server" ControlToValidate="obsrCliente" Display="Dynamic"
							Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator></p>
					<p>Observaciones Recepcionista:
					</p>
					<p><asp:textbox id="obsrRecep" runat="server" Width="90%" TextMode="MultiLine" onblur="aMayusculas(this)"></asp:textbox><asp:requiredfieldvalidator id="validatorObsrRecep" runat="server" ControlToValidate="obsrRecep" Display="Dynamic"
							Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator></p>
				</fieldset>
			</td>
			<td>
				<fieldset class="infield">
					<p></p>
					<legend class="Legends">
						Recepcionista</legend>
					<!--	<p>Vendedor :
					<asp:dropdownlist id="codigoVende" runat="server" Width="190px"></asp:dropdownlist></p>
					-->	Código :
					<asp:dropdownlist id="codigoRecep" runat="server" Width="190px"></asp:dropdownlist>
					<p>Clave:&nbsp;<asp:textbox id="clave" runat="server" Width="57px" TextMode="Password" ></asp:textbox>
						&nbsp;
					</p>
				</fieldset>
			</td>
		</tr>
        <tr >
            <td colspan="3">
                <div id ="datosTransferencia" runat="server" visible = "true">
	                <fieldset class="infield" style="width:850px">
                        <legend class="Legends">Datos Transferencia</legend>
		                <table>
                            <tr>
                                <td>
                                    Lista de Precios (Items Fuera de Kits) :<asp:dropdownlist id="listaPreciosItems" runat="server" Width="150px" ></asp:dropdownlist>
                                </td>
                                <td>
                                    Nit Transferencias :<asp:dropdownlist id="nitTransferencias" runat="server" Width="150px"></asp:dropdownlist>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo&nbsp;Pedido:<asp:dropdownlist id="tipoPedido" Width="150px" runat="server"></asp:dropdownlist>
                                </td>
                                <td>
                                    Prefijo Transferencia:<asp:dropdownlist id="prefijoTransferencia" runat="server" Width="180px"></asp:dropdownlist>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Almacén&nbsp;Transferencia:<asp:dropdownlist id="almacenTransferencia" runat="server" Width="160px"></asp:dropdownlist>
                                </td>
                            </tr>
		                </table>
                    </fieldset>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3"><br />
                <p><asp:button id="confirmar" onclick="Confirmar"  runat="server" Width="207px" Visible="true" Text="Validar" style="position:relative; left:45%;"></asp:button>&nbsp;<asp:label id="lbEstCita" runat="server" visible="true"></asp:label>
					<asp:ValidationSummary id="ValidationSummary1"  runat="server" ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
				</p>
<asp:label id="lb" runat="server"></asp:label>
            </td>
        </tr>
	</tbody>
</table><br />


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
	var ddlNitsTaller = document.getElementById("<%=nitTransferencias.ClientID%>");
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
