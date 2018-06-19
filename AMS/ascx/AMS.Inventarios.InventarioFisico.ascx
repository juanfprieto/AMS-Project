<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.InventarioFisico.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_InventarioFisico" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type="text/javascript">
	function ValidarInfoInicial()
	{
		var err = "";
		var salida = true;
		var generarAlmacenes = false;
		var estRevision = true;
		var regexInt = /[0-9]+/;
		
		if(document.getElementById('<%=ddlAlmacenes.ClientID%>').value == '0')
			generarAlmacenes = true;
		
		if(document.getElementById('<%=ddlInventario.ClientID%>').value == '0')
		{
			err += "- Seleccione un número de inventario fisico. \n";
			salida = false;
		}
		
		if(document.getElementById('<%=ddlCriterioSeleccionUbicaciones.ClientID%>').value == '-1')
		{
			err += "- Seleccione un criterio para la selección de ubicaciones de inventario físico. \n";
			salida = false;
		}
		
		if(document.getElementById('<%=ddlCriterioSeleccionUbicaciones.ClientID%>').value == '1')
		{
			if(document.getElementById('<%=lbrango.ClientID%>').value == '')
			{
				err += "- Seleccione una ubicación inicial. \n";
				salida = false;
			}
			
			if(document.getElementById('<%=lbrangof.ClientID%>').value == '')
			{
				err += "- Seleccione una ubicación final. \n";salida = false;
			}
		}
		
		if(!salida)
		{
			alert(err);
			return salida;
		}
		
		if(!generarAlmacenes)
			return salida;
		else 
			return confirm('Esta seguro de generar los conteos para todos los almacenes?');
	}
	
	function CambioCriterioSeleccionUbicaciones()
	{
		var valor = document.getElementById('<%=ddlCriterioSeleccionUbicaciones.ClientID%>').value;

		switch (valor)
		{
			case '-1':
				document.getElementById('<%=div_lbUbicaciones.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_cblUbicaciones.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_ran.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_lbrango.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_ranf.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_lbrangof.ClientID%>').style.display = 'none';
				break;
			case '1':
				document.getElementById('<%=div_lbUbicaciones.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_cblUbicaciones.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_ran.ClientID%>').style.display = '';
				document.getElementById('<%=div_lbrango.ClientID%>').style.display = '';
				document.getElementById('<%=div_ranf.ClientID%>').style.display = '';
				document.getElementById('<%=div_lbrangof.ClientID%>').style.display = '';
				break;
			case '2':
				document.getElementById('<%=div_lbUbicaciones.ClientID%>').style.display = '';
				document.getElementById('<%=div_cblUbicaciones.ClientID%>').style.display = '';
				document.getElementById('<%=div_ran.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_lbrango.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_ranf.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_lbrangof.ClientID%>').style.display = 'none';
				break;
			case '3':
				document.getElementById('<%=div_lbUbicaciones.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_cblUbicaciones.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_ran.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_lbrango.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_ranf.ClientID%>').style.display = 'none';
				document.getElementById('<%=div_lbrangof.ClientID%>').style.display = 'none';
				break;
		}
	}
	
	function CambioInventarioFisico()
	{
		var prefijoInv = (document.getElementById('<%=ddlInventario.ClientID%>').value.split('-'))[0];
		var numeroInv = (document.getElementById('<%=ddlInventario.ClientID%>').value.split('-'))[1];
		
		var tipoUbic = AMS_Inventarios_InventarioFisico.CargarTipoInventarioFisicoUbicacion(prefijoInv,numeroInv);
		
		if (tipoUbic.value == 'T')
		{
			document.getElementById('<%=div_lbAlmacenes.ClientID%>').style.display = 'none';
			document.getElementById('<%=div_ddlAlmacenes.ClientID%>').style.display = 'none';
		}
		else if (tipoUbic.value == 'A') 
		{
			document.getElementById('<%=div_lbAlmacenes.ClientID%>').style.display = '';
			document.getElementById('<%=div_ddlAlmacenes.ClientID%>').style.display = '';
		}
	}
</script>

<fieldset>
<legend>Información para la generación de Tarjetas de Conteo</legend>
	<table id="Table1" class="filtersIn">
		<tbody>
			<TR>
				<TD>Proceso:
				
				<br>
					<asp:dropdownlist id="ddlProceso" class="dmediano" runat="server">
						<asp:ListItem Value="1">Generación</asp:ListItem>
						<asp:ListItem Value="2">Actualización</asp:ListItem>
					</asp:dropdownlist>
				</TD>
			</TR>
			<TR>
				<TD>Número del Inventario:
				<br />
				<asp:dropdownlist id="ddlInventario" OnChange="CambioInventarioFisico();" class="dmediano" runat="server"></asp:dropdownlist>
                </TD>
			</TR>
			<TR>
				<TD valign="top">
					<div id="div_lbAlmacenes" style="DISPLAY: none" runat="server">Escoja el almacén :</div>
				</TD>
				<TD align="right">
					<div id="div_ddlAlmacenes" style="DISPLAY: none" runat="server"><asp:dropdownlist id="ddlAlmacenes" Width="320px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAlmacenes_SelectedIndexChanged"></asp:dropdownlist></div>
				</TD>
			</TR>
			<TR>
				<TD>Incluir Ítems Sin Ubicación :
				<br>
				<asp:checkbox id="chbItemsSinUbicacion" runat="server"></asp:checkbox>
                </TD>
			</TR>
			<TR>
				<td>
					<div id="div_lbCriterioSeleccionUbicaciones">Escoja el criterio para seleccionar las ubicaciones:
					</div>
				
				
					<div id="div_ddlCriterioSeleccionUbicaciones"><asp:dropdownlist id="ddlCriterioSeleccionUbicaciones" OnChange="CambioCriterioSeleccionUbicaciones();"
							class="dmediano" runat="server">
							<asp:ListItem Value="-1">Seleccione...</asp:ListItem>
							<asp:ListItem Value="1">Intervalo de Estantes</asp:ListItem>
							<asp:ListItem Value="2">Selección de Ubicaciones</asp:ListItem>
							<asp:ListItem Value="3">Todas las Ubicaciones</asp:ListItem>
						</asp:dropdownlist></div>

                        <p>
                        <asp:button id="btnRealizar" onclick="generar_Inventario" onClientClick = "espera();" runat="server" Text="Generar" class = "noEspera"> 
                        </asp:button>
                        </p>
				</td>
			</TR>
			<TR>
				<TD valign="top">
					<div id="div_lbUbicaciones" style="DISPLAY: none" runat="server"><asp:label id="lbUbicaciones" runat="server">Seleccione las ubicaciones sobre las que desea generar el conteo: </asp:label></div>
				</TD>
				<TD align="right">
					<div id="div_cblUbicaciones" style="DISPLAY: none; OVERFLOW: auto; WIDTH: 320px; HEIGHT: 300px"
						runat="server"><asp:checkboxlist id="cblUbicaciones" Width="320" runat="server"></asp:checkboxlist></div>
				</TD>
			</TR>
			<TR>
				<TD valign="top">
					<div id="div_ran" style="DISPLAY: none" runat="server"><asp:label id="ran" runat="server">Seleccione el estante inicial : </asp:label></div>
				</TD>
				<TD align="right">
					<div id="div_lbrango" style="DISPLAY: none" runat="server"><asp:listbox id="lbrango" Width="320px" runat="server" Height="104px"></asp:listbox></div>
				</TD>
			</TR>
			<TR>
				<TD valign="top">
					<div id="div_ranf" style="DISPLAY: none" runat="server"><asp:label id="ranf" runat="server">Seleccione el estante final :</asp:label></div>
				</TD>
				<TD align="right">
					<div id="div_lbrangof" style="DISPLAY: none" runat="server"><asp:listbox id="lbrangof" Width="320px" runat="server" Height="104px"></asp:listbox></div>
				</TD>
			</TR>
		</tbody>
	</table>
</fieldset>


<asp:datagrid id="dgitems" runat="server" cssclass="datagrid" AutoGenerateColumns="False">
	<Columns>
		<asp:BoundColumn DataField="NUMERO" HeaderText="N&#250;mero">
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="PUBI_NOMBRE" HeaderText="Ubicaci&#243;n"></asp:BoundColumn>
		<asp:BoundColumn DataField="MITE_CODIGOEDITADO" HeaderText="C&#243;digo &#205;tem"></asp:BoundColumn>
		<asp:BoundColumn DataField="MITE_NOMBRE" HeaderText="Descripci&#243;n"></asp:BoundColumn>
		<asp:BoundColumn DataField="PALM_ALMACEN" HeaderText="Almacen"></asp:BoundColumn>
		<asp:BoundColumn DataField="MSAL_CANTACTUAL" HeaderText="Cantidad">
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="MSAL_COSTPROM" HeaderText="Costo Promedio">
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="MITE_CODIGO" HeaderText="CodigoItem"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="PUBI_CODIGO" HeaderText="CodigoUbicacion"></asp:BoundColumn>
	</Columns>
</asp:datagrid>

<p><asp:button id="btnSave" onclick="guardar_tabla" runat="server" Text="Guardar Inventario Físico"
		Visible="False"></asp:button> &nbsp;&nbsp;&nbsp;
   <asp:button id="btnCancelar" runat="server" Text="Cancelar" Visible="false" onclick="btnCancelar_Click"></asp:button>
</p>
<asp:label id="lblInfo" runat="server"></asp:label>

