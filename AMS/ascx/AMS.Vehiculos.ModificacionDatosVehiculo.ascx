<%@ Control Language="c#" codebehind="AMS.Vehiculos.ModificacionDatosVehiculo.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.ModificacionDatosVehiculo" %>
<script type="text/javascript">
    function abrirEmergente(obj) {
        var ddl =document.getElementById("<%=catalogoVehiculo.ClientID%>");
        ModalDialog(ddl,'SELECT pcat_codigo, \'[\' concat pcat_codigo concat \'] - [\' concat pcat_descripcion concat \']\' descripcion FROM dbxschema.pcatalogovehiculo WHERE pcat_codigo IN (SELECT DISTINCT MC.pcat_codigo FROM DBXSCHEMA.MVEHICULO MV, McatalogoVEHICULO MC WHERE MC.MCAT_VIN = MV.MCat_VIN) ORDER BY pcat_descripcion, pcat_codigo', new Array(), 1);
    }
</script>

<table class="filters" >
	<tbody>
		<tr>
			<th class="filters" >
            <IMG height="60" src="../img/AMS.Flyers.Edits.png" border="0">
			</th>
             <td>
            <fieldset> 
				<p>
                     Seleccione el Catálogo del Vehículo : 
                     <br>
                     <asp:dropdownlist id="catalogoVehiculo" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Catalogo" class="dmediano" runat="server"></asp:dropdownlist>
                        &nbsp;&nbsp;<asp:image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente();"></asp:image>
                 </p>
				<P>
                    Seleccione el VIN del Vehículo :
                    <br>
                    <asp:dropdownlist id="vinVehiculo" class="dmediano" runat="server"></asp:dropdownlist>
						&nbsp;&nbsp;<asp:image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png" ></asp:image>
		    </font>
            </P>
				<P><FONT style="BACKGROUND-COLOR: #f2f2f2"><asp:button id="btnEditar" onclick="Editar_Datos_Vehiculo" runat="server" Text="Editar"></asp:button></P></FONT>
			</fieldset>
            </td>             
            </tr>
    </tbody>
</table>
<P><asp:label id="lb" runat="server"></asp:label></P>
