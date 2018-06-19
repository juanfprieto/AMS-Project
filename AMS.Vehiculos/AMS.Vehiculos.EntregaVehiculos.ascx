<%@ Control Language="c#" codebehind="AMS.Vehiculos.EntregaVehiculos.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.EntregaVehiculos" %>
<script type="text/javascript">
    function cargaVin()
    {
        var vehiculo = document.getElementById('<%=vehiculo.ClientID%>');
        var vendedor = document.getElementById('<%=vendedor.ClientID%>').value;
        ModalDialog(vehiculo, 'SELECT MVEHICULO.mveh_inventario,  \'\' CONCAT MVEHICULO.mcat_vin AS VIN, MC.pcat_codigo AS CODIGO, ' +
        'VMNIT.NOMBRE concat \' [ \' concat rtrim(char(MFACTURACLIENTE.MFAC_FACTURA)) concat \' ] \' concat PVENDEDOR.PVEN_NOMBRE ' +
        'FROM dbxschema.mvehiculo,dbxschema.VMNIT, dbxschema.MFACTURACLIENTE, dbxschema.MFACTURAPEDIDOVEHICULO, ' +
        'dbxschema.MASIGNACIONVEHICULO, dbxschema.PVENDEDOR, DBXSCHEMA.MCATALOGOVEHICULO MC ' +
        'WHERE test_tipoesta=40 ' +
        'AND MVEHICULO.MNIT_NIT = VMNIT.MNIT_NIT ' +
        'AND MFACTURACLIENTE.PVEN_CODIGO = PVENDEDOR.PVEN_CODIGO ' +
        'AND mvehiculo.MVEH_INVENTARIO = mASIGNACIONvehiculo.MVEH_INVENTARIO ' +
        'AND mASIGNACIONvehiculo.PDOC_CODIGO = MFACTURAPEDIDOVEHICULO.MPED_CODIGO ' +
        'AND mASIGNACIONvehiculo.MPED_NUMEPEDI = MFACTURAPEDIDOVEHICULO.MPED_NUMEPEDI ' +
        'AND MFACTURAPEDIDOVEHICULO.PDOC_CODIGO = MFACTURACLIENTE.PDOC_CODIGO ' +
        'AND MFACTURAPEDIDOVEHICULO.MFAC_NUMEDOCU = MFACTURACLIENTE.MFAC_NUMEDOCU ' +
        'AND MC.MCAT_VIN = MVEHICULO.MCat_VIN ' +
        'AND PVENDEDOR.PVEN_CODIGO = \'' + vendedor + '\' ' +
        'ORDER BY PVENDEDOR.PVEN_NOMBRE, MFACTURACLIENTE.MFAC_FACTURA', new Array());
    }
</script>

<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
				<img height="80" src="../img/AMS.Flyers.Formato.png" border="0" /> 
			</th>
			<td>
				    <p>
					    Vendedor&nbsp;:
                        <asp:DropDownList id="vendedor" class="dmediano" runat="server"  OnSelectedIndexChanged= "CargarVehiculos" AutoPostBack= "true"></asp:DropDownList>
                        <br /><br />Vehiculo&nbsp;: &nbsp;&nbsp;<asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png" onclick="cargaVin()" Visible="false"/>
					    <asp:DropDownList id="vehiculo" runat="server" ></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				    </p>
				    <p>
					    <asp:Button id="botonFormato" runat="server"  Text="Generar Formato" onclick="Generar_Formato_Salida" ></asp:Button>
					    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button id="Button1" onclick="programar_entrega" runat="server" class="bpequeno" Text="Programar Entrega" ></asp:Button>
					    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					    <asp:Button id="botonEntrega" onclick="Entregar_Vehiculo" runat="server" class="bpequeno" Text="Entregar Vehiculo" ></asp:Button>
				    </p>
			</td>
		</tr>
	</tbody>
</table>
<asp:Label id="lb" runat="server"></asp:Label>
</fieldset>




