<%@ Control Language="c#" codebehind="AMS.Inventarios.AdminUbicaciones.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.AdminUbicaciones" %>

<script type="text/javascript">
    if(window.returnValue === 'help')
    {
        window.close();
    }
</script>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
                <IMG height="70" src="../img/AMS.Flyers.News.png" border="0">
            </th>
			<td>
				<table class="filtersIn">
					<tbody>
						<tr><p>
							<td>
								Ingrese el Nombre de la nueva ubicación de
									nivel 1 (Por lo general, son bodegas) : 
                                
                            
                                <asp:textbox id="tbNombUbicacion" class="tpequeno" runat="server"></asp:textbox>
                                
                            </td>
                            </p>
                            
                            </tr>
                            <tr>
                        <p>
							<td>Almacén Relacionado : 
                            <asp:dropdownlist id="ddlAlmacen" class="dpequeno" runat="server"></asp:dropdownlist>
                            </td>
                            </p>
                            
							<td align="right">
                                <asp:button id="btnCrear" onclick="CrearUbicacionNivelUno" runat="server" Text="Crear"></asp:button>
                            </td>
                        
                        </tr>
						
                    </tbody>
                </table>
            </td>
        </tr>
		<tr>
			<th>
                <IMG height="85" src="../img/AMS.Flyers.Configuracion.png" border="0">
            </th>
			<td>
				<table class="filtersIn">
					<tbody>
						<tr>
							<td>Almacén Relacionado : 
                            <br />
                                <asp:dropdownlist id="ddlAlmacenCfg" class="dmediano" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CambioAlmacenConfiguracion"></asp:dropdownlist>
                            </td>
                            </tr>
                        <tr>
							<td>Ubicación Nivel 1 :
                            <br />
                                <asp:dropdownlist id="ddlUbicacionCfg" class="dmediano" runat="server"></asp:dropdownlist>
                            </td>
                        </tr>
						<tr>
							<td align="right" colSpan="2">
                                <asp:button id="btnConfiguración" onclick="ConfigurarUbicacion" runat="server" Text="Configuración"></asp:button>&nbsp;
								<asp:button id="btnEliminar" runat="server" Text="Eliminar" onclick="EliminarUbicacion"></asp:button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>

<p><asp:label id="lb" runat="server"></asp:label></p>
