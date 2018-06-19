<%@ Control Language="c#" codebehind="AMS.Vehiculos.AsignacionVehiculos.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.AsignacionVehiculos" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<p>
    <fieldset>
        <legend>Datos sobre el Pedido</legend>
	    <TABLE id="Table1" class="filtersIn">
		    <TR>
			    <TD>Prefijo del Pedido :<br><asp:dropdownlist id="prefijoPedido" OnSelectedIndexChanged="Cambio_Tipo_Documento" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
			    <TD>Número del Pedido :<br><asp:DropDownList id="numeroPedido" runat="server" OnSelectedIndexChanged="Cambio_Numero_Documento" AutoPostBack="True"></asp:DropDownList></TD>
            </tr>
            <tr>
	            <td>Nit Cliente Principal :<br><asp:label id="nitPrincipal" runat="server"></asp:label></td>
		        <td>Nombre Cliente Principal :<br><asp:label id="nombrePrincipal" runat="server"></asp:label></td>
            </tr>
            <tr>
	            <td>Nit Cliente Alterno :<br><asp:label id="nitAlterno" runat="server"></asp:label></td>
                <td>Nombre Cliente Alterno :<br><asp:label id="nombreAlterno" runat="server"></asp:label></td>
	        </tr>
            <tr>
                <td>Tipo de Vehiculo :<br><asp:label id="tipoVehiculoPedido" runat="server"></asp:label></td>
                <td>Año de Modelo :<br><asp:label id="anoModeloPedido" runat="server"></asp:label></td>
	        </tr>    
            <tr>
                <td>Color Vehiculo :<br><asp:label id="colorPrincipal" runat="server"></asp:label></td>
		        <td>Color Opcional :<br><asp:label id="colorOpcional" runat="server"></asp:label></td>
            </TR>
	    </TABLE>
    </fieldset>
</p>
<p>

    <fieldset>
	    <legend>Datos Sobre el Automovil</legend>
		<table class=filtersIn>
			<tbody>
				<tr>
					<td>Catalogo Vehiculo :</td>
					<td><asp:dropdownlist id="catalogoVehiculo" OnSelectedIndexChanged="Cambio_Catalogo" runat="server" AutoPostBack="true"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:label id="lbVinVehiculo" Runat="server">VIN Vehiculo :</asp:label></td>
					<td><asp:dropdownlist id="vinVehiculo" OnSelectedIndexChanged="Cambio_Vin" runat="server" AutoPostBack="true"></asp:dropdownlist></td>
				</tr>
			</tbody>
		</table>
        <p>
		<asp:datagrid id="dgVehiculos" Width="256px" runat="server" OnItemCommand="validar" AutoGenerateColumns="False">
			<Columns>
				<asp:BoundColumn DataField="vinVehiculo" HeaderText="Vin Veh&#237;culo"></asp:BoundColumn>
				<asp:BoundColumn DataField="color" HeaderText="Color"></asp:BoundColumn>
				<asp:BoundColumn HeaderText="RGB"></asp:BoundColumn>
				<asp:BoundColumn DataField="diasInv" HeaderText="D&#237;as Inventario"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Selecci&#243;n">
					<ItemTemplate>
						<asp:Button id="btnVehic" runat="server" CommandName="Seleccionar" Text="Seleccionar"></asp:Button>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid>
        </p>
        <p>
	    <asp:panel id="panel" Runat="server" Visible="False">
            <fieldset>
	            <legend>Información Adicional</legend>
		        <TABLE class="filtersIn">
			        <TR>
				        <TD>Año de Modelo :
				        </TD>
				        <TD>
					        <asp:label id="anoModeloVehiculo" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>Color del Vehiculo :
				        </TD>
				        <TD>
					        <asp:label id="colorVehiculo" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>Clase de Vehiculo :
				        </TD>
				        <TD>
					        <asp:label id="claseVehiculo" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>Tipo de Servicio :
				        </TD>
				        <TD>
					        <asp:label id="tipoServicio" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>No. Inventario :</TD>
				        <TD>
					        <asp:label id="numeInventario" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>No. Recepción :
				        </TD>
				        <TD>
					        <asp:label id="numeRecepcion" runat="server"></asp:label></TD>
			        <TR>
				        <TD>Fecha de Recepción :</TD>
				        <TD>
					        <asp:label id="fechRecepcion" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>Fecha Disponibilidad:
				        </TD>
				        <TD>
					        <asp:label id="fechDisponible" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>No. Manifiesto :
				        </TD>
				        <TD>
					        <asp:label id="numeManifiesto" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>Motor :
				        </TD>
				        <TD>
					        <asp:label id="motor" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>Chasis :
				        </TD>
				        <TD>
					        <asp:label id="chasis" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>Número de Placa :
				        </TD>
				        <TD>
					        <asp:label id="placa" runat="server"></asp:label></TD>
			        </TR>
			        <TR>
				        <TD>Días Inventario :
				        </TD>
				        <TD>
					        <asp:label id="diasInventario" runat="server"></asp:label></TD>
			        </TR>
		        </TABLE>
            </fieldset>
	    </asp:panel>
        </p>
        <p>
            <asp:button id="botonAccion" onclick="Realizar_Accion" runat="server" Text="Asignar" UseSubmitBehavior="false" 
            OnClientClick="clickOnce(this, 'Cargando...')"></asp:button>
        </p>
        <p><asp:label id="lb" runat="server"></asp:label></p>
    </fieldset>
</p>	

<script language = "javascript">
 function clickOnce(btn, msg)
        {
            // Comprobamos si se está haciendo una validación
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Procesando..'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
</script>
