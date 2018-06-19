<%@ Control Language="c#" codebehind="AMS.Vehiculos.FacturaProveedor.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.FacturaProveedorControl" %>

<script type ="text/javascript">
    function mostrarDialogo(obj) {
        var catalogo = document.getElementById('_ctl1_' + obj);
        ModalDialog(catalogo, 'SELECT PC.pcat_codigo CONCAT SUBSTR(MC.MCAT_VIN,(LENGTH(MC.MCAT_VIN)-2),3)  as pcat_codigo, MC.MCAT_VIN concat \' - \' concat  pMAR_NOMBRE concat \' - \'  concat pcat_descripcion  concat \' - \'  concat MCAT_PLACA as descripcion  ' +
          'FROM  dbxschema.pcatalogovehiculo PC, PMARCA PM, dbxschema.mvehiculo MV, McatalogoVEHICULO MC ' +
          'WHERE PC.PMAR_CODIGO = PM.PMAR_CODIGO AND PC.pcat_codigo = MC.PCAT_CODIGO  AND (pdoc_codiordepago IS NULL or pdoc_codiordepago = \'\') AND MC.MCAT_VIN = MV.MCat_VIN ' + 
          'order by 2, 1', new Array());
    }
    function CambioRcp(obChk,obCmb1,obCmb2)
    {
        if(!obChk.checked)
        {
            obCmb1.disabled = true;
            obCmb2.disabled = true;
        }
        else
        {
            obCmb1.disabled = false;
            obCmb2.disabled = false;
        }
    }
</script>

<table class="filters" >
	<tbody>
		<tr>
			<th class="filterHead">
            <img height="80" src="../img/AMS.Flyers.Recepcion.png" border="0">
            </th>
			<td>
            <fieldset>
				<table class="filters">
					<tbody>
						<tr>
							<td colSpan="2">
                                    <asp:checkbox id="chkRecepDir" Checked="True" TextAlign="Left" Text="Existe un pedido a proveedor ? " runat="server"></asp:checkbox>
                            </td>
                        </tr>
						<tr>
							<td width="70">Prefijo del Pedido:<br>                            
                                <asp:dropdownlist id="prefijoPedido" runat="server" OnSelectedIndexChanged="Cambio_Prefijo" AutoPostBack="true"></asp:dropdownlist>
                            </td>
                        </tr>
						<tr>
							<td width="70">Número del Pedido: <br>
                                <asp:dropdownlist id="numeroPedido" runat="server"></asp:dropdownlist>
                            </td>
                        </tr>
						<tr>
							<td colSpan="2"><asp:checkbox id="factura" TextAlign="Left" Text="Realizar Facturación ? " runat="server"></asp:checkbox></td>
                        </tr>
						<tr>
						    <td colSpan="2">
                                <asp:button id="btnCfn" onclick="Confirmar_Recepcion" Text="Confirmar" runat="server"></asp:button> 
                            </td>
                       </tr>
                     </tbody>
                  </table>
              </fieldset>
           </td>
        </tr>
    </tbody>
</table>

                        
                      
 <asp:placeholder id="plFacturarN" runat="server">

   <table class="filters" >
	  <tbody>
	   <tr>
	    <th class="filterHead">
            <IMG height="80" src="../img/AMS.Flyers.Facturar.png" border="0">
            </th>
           <td>                   
                <fieldset>
                <legend class="Legends">Facturación Por Unidad</legend>
					<table class="filters">
						<tbody>
							<tr>
								<td colSpan="2">
									<p>A Continuación se presentan los vehículos
										que se recibieron y no se facturaron. </p>
                                </td>
                            </tr>
							<tr>
								<td width="70">Catálogo de Vehiculo :<br>
                                <asp:dropdownlist id="catalogo2" runat="server" OnSelectedIndexChanged="Cambio_Catalogo" AutoPostBack="true"></asp:dropdownlist>
                                <asp:Image id="imgLupaCatalogo" runat="server" ImageUrl="../img/AMS.Search.png" onClick="mostrarDialogo('catalogo2');" Visible="true"></asp:Image>
                                </td>
                            </tr>
							<tr>
								<td width="70">Vin Vehículo :<br>
								<asp:dropdownlist id="vinVehiculo" runat="server"></asp:dropdownlist>
                                </td>
                            </tr>
							<tr>
								<td colSpan="2">
									<asp:checkbox id="Checkbox2" TextAlign="Left" Text="Realizar Facturación ? " runat="server"></asp:checkbox>
                                    </td>
							</tr>
							<tr>
								<td colSpan="2">
									<asp:button id="btnCfn2" onclick="Confirmar_Factura" Text="Confirmar" runat="server"></asp:button>
                                    </td>
							</tr>
						</tbody>
					</table>
				</fieldset>


				<fieldset>
					<p></p><legend class="Legends">Facturación Por Pedido</legend>
					<table class="filters">
						<tbody>
							<tr>
								<td colSpan="2">
									<p>A continuación se muestran los pedidos que aun tienen vehículos sin facturar : </p>
                                    </td>
                            </tr>
							<tr>
								<td width="70">Prefijo del Pedido:<br>
								<asp:dropdownlist id="prefijoPedido2" runat="server" OnSelectedIndexChanged="Cambio_Prefijo2" AutoPostBack="true"></asp:dropdownlist>
                                </td>
                            </tr>
							<tr>
								<td width="70">Número del Pedido:<BR> 
								<asp:dropdownlist id="numeroPedido2" runat="server"></asp:dropdownlist>
                                </td>
                            </tr>
							<tr>
								<td colSpan="2">
									<asp:button id="btnCfn3" onclick="Confirmar_Factura2" Text="Confirmar" runat="server"></asp:button></td>
							</tr>
						</tbody>
					</table>
				</fieldset>
                </tr>
                </tbody>
                </table>
              </fieldset>  
			</td>
          </tr>
          </tbody>
          </table>
          </asp:PlaceHolder>         
 <asp:label id="lb" runat="server"></asp:label>
