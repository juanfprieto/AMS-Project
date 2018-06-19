<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.EditarCitaTaller.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_EditarCitaTaller" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<style type="text/css">
    input[type="text"]
        {
	        width: 80px;
        }
</style>
<script src="../js/lightbox.js" type="text/javascript" ></script>
<script type ="text/javascript">
    function probando(obj)
    {
        CambioRecepcionista(obj.firstElementChild.innerHTML);
    }
    function CargarFormEdicion(valFechAnt, valHorAnt, valRcpAnt, valPlaca, codVend)
    {
        document.getElementById('<%=hdFechAnt.ClientID%>').value = valFechAnt;
        var idCalEdi = document.getElementById('<%=cpFechaNuevaCita.ClientID%>');
        document.getElementById('<%=hdHorAnt.ClientID%>').value = valHorAnt;
        document.getElementById('<%=hdRecAnt.ClientID%>').value = valRcpAnt;
        var anio = String(valFechAnt).substring(6, 10); 
        var mes = String(valFechAnt).substring(0, 2);
        var dia = String(valFechAnt).substring(3, 5);
        var valFechCorregida = anio + "-" + mes + "-" + dia;
        idCalEdi.childNodes[0].value = dia + "/" + mes + "/" + anio;
		var placa = valPlaca;
		var ddlHorr = document.getElementById('<%=ddlHorario.ClientID%>');
		var contTotal = ddlHorr.options.length;
		for (var f = 0; f < contTotal; f++) {
		    if (ddlHorr.options[f].value == valHorAnt) {
		        ddlHorr.options[f].selected = true;
		        break;
		    }
		}
		VerificarAlmacen(valFechCorregida.toString(), valHorAnt.toString(), placa.toString());
		CambioRecepcionista(codVend);
        
		var arraySels = new Array('<%=ddlHorario.ClientID%>', '<%=ddlTaller.ClientID%>', '<%=ddlRecepcionista.ClientID%>');
		showLightbox(arraySels);
    }

    function VerificarAlmacen(fecha, hora, placa)
    {
        AMS_Automotriz_EditarCitaTaller.verificar_Almacen(fecha, hora, placa, Verificar_Almacen_CallBack);
    }

    function Verificar_Almacen_CallBack(response)
    {
        var almacen = response.value;
        var ddlAlmacen = document.getElementById('<%=ddlTaller.ClientID%>');
        var contAlmacen = ddlAlmacen.options.length;
        for (var i = 0; i < contAlmacen; i++)
        {
            if (ddlAlmacen.options[i].value == almacen)
            {
                ddlAlmacen.options[i].selected = true;
                break;
            }
        }
        var ddlRec = document.getElementById('<%=ddlRecepcionista.ClientID%>');
        var opciones = AMS_Automotriz_EditarCitaTaller.CambioTallerCarga(ddlAlmacen.value).value;
        ddlRec.options.length = 0;
        if (opciones.Tables[0].Rows.length > 0)
        {
            for (var i = 0; i < opciones.Tables[0].Rows.length; ++i)
                ddlRec.options[ddlRec.options.length] = new Option(opciones.Tables[0].Rows[i].PVEN_NOMBRE, opciones.Tables[0].Rows[i].PVEN_CODIGO);
        }

        var contTotalRec = ddlRec.options.length;
        for (var j = 0; j < contTotalRec; j++)
        {
            if (ddlRec.options[j].value == document.getElementById('<%=hdRecAnt.ClientID%>').value)
            {
                ddlRec.options[j].selected = true;
            }
        }
    
    }
	function CambioTaller()
	{
		var ddlRecepcionistas = document.getElementById("<%=ddlRecepcionista.ClientID%>");
		var opciones = AMS_Automotriz_EditarCitaTaller.CambioTallerCarga(document.getElementById("<%=ddlTaller.ClientID%>").value).value;
		ddlRecepcionistas.options.length = 0;
		if(opciones.Tables[0].Rows.length > 0)
		{
			for (var i = 0; i < opciones.Tables[0].Rows.length; ++i)
				ddlRecepcionistas.options[ddlRecepcionistas.options.length] = new Option(opciones.Tables[0].Rows[i].PVEN_NOMBRE,opciones.Tables[0].Rows[i].PVEN_CODIGO);
		}
		CambioRecepcionista();
	}
	
	function CambioRecepcionista()
	{
		var ddlHoras = document.getElementById("<%=ddlHorario.ClientID%>");
		var idCalEdi = '<%=cpFechaNuevaCita.ClientID%>';
	    var fechNue = document.getElementById(idCalEdi.substr(0, idCalEdi.indexOf('outer') - 1)).value;
	    fechNue = verificaFecha(fechNue);
		var opciones = AMS_Automotriz_EditarCitaTaller.ConstruirHorarioAtencionRecepcionista(fechNue,document.getElementById("<%=ddlRecepcionista.ClientID%>").value).value;
		ddlHoras.options.length = 0;
		if(opciones.Tables[0].Rows.length > 0)
		{
			for (var i = 0; i < opciones.Tables[0].Rows.length; ++i)
				ddlHoras.options[ddlHoras.options.length] = new Option(opciones.Tables[0].Rows[i].HORA,opciones.Tables[0].Rows[i].HORA);
		}
	}
    function CambioRecepcionista(recepcionista)
	{
		var ddlHoras = document.getElementById("<%=ddlHorario.ClientID%>");
		var idCalEdi = '<%=cpFechaNuevaCita.ClientID%>';
	    var fechNue = document.getElementById(idCalEdi.substr(0, idCalEdi.indexOf('outer') - 1)).value;
	    fechNue = verificaFecha(fechNue);
		var opciones = AMS_Automotriz_EditarCitaTaller.ConstruirHorarioAtencionRecepcionista(fechNue,recepcionista).value;
		ddlHoras.options.length = 0;
		if(opciones.Tables[0].Rows.length > 0)
		{
			for (var i = 0; i < opciones.Tables[0].Rows.length; ++i)
				ddlHoras.options[ddlHoras.options.length] = new Option(opciones.Tables[0].Rows[i].HORA,opciones.Tables[0].Rows[i].HORA);
		}
	}
    function verificaFecha(fecha)
    {
        if (fecha.includes("/")) {
            var dia, mes, ano, fechaFinal;
            var datos = fecha.split('/');
            dia = datos[0];//fecha.substring(0, 2);
            mes = datos[1];//fecha.substring(3, 5);
            ano = datos[2];//fecha.substring(6, fecha.length);
            //fechaFinal = dia + "-" + mes + "-" + ano;
            fechaFinal = ano + "-" + mes + "-" + dia;
            return fechaFinal;
        } else
            return fecha;
    }

	
	function ValidarProcesoEdicion()
	{
		var valorRetorno = true;
		var idCalEdi = '<%=cpFechaNuevaCita.ClientID%>';
		var fechNue = document.getElementById(idCalEdi.substr(0,idCalEdi.indexOf('outer')-1)).value;
		var horNue = document.getElementById('<%=ddlHorario.ClientID%>').value;
		var recNue = document.getElementById('<%=ddlRecepcionista.ClientID%>').value;
		if(!AMS_Automotriz_EditarCitaTaller.ConsultaFechaHoraVsSistema(fechNue,horNue).value)
		{
			alert('La fecha seleccionada es menor a la fecha registrada del sistema. Por favor seleccione una fecha superior.');
			valorRetorno = false;
		}
		else if(AMS_Automotriz_EditarCitaTaller.ConsultarExistenciaCita(fechNue,horNue,recNue).value)
		{
			alert('Este espacio ya se encuentra reservado, por favor seleccione otro');
			valorRetorno = false;
		}
		return valorRetorno;
	}
</script>

<fieldset>

    <P>
        <table id="Table" class="filtersIn">     
            <tr>
                <td>
                    Seleccione la fecha o placa que desea consultar en las citas de taller programadas. 
                    Tenga en cuenta que solo se podran modificar las citas que 
                    esten&nbsp;programadas para despues de la fecha del dia y de la hora actual del 
                    dia. 
                </td>
            </tr>
            <tr>
                <td>
                    <p><strong>Fecha: </strong> 
                        <ew:calendarpopup id="cpFechaCita" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage"
	                        runat="server" Culture="Spanish (Colombia)" >
	                        <WeekdayStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="White"></WeekdayStyle>
	                        <MonthHeaderStyle Font-Size="X-Small" Font-Names="Arial" ForeColor="Black" BackColor="Silver"></MonthHeaderStyle>
	                        <OffMonthStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="#FF8080"></OffMonthStyle>
	                        <GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
		                        BackColor="White"></GoToTodayStyle>
	                        <TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
		                        BackColor="LightGoldenrodYellow"></TodayDayStyle>
	                        <DayHeaderStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightBlue"></DayHeaderStyle>
	                        <WeekendStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightGray"></WeekendStyle>
	                        <SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
		                        BackColor="Khaki"></SelectedDateStyle>
	                        <ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
		                        BackColor="White"></ClearDateStyle>
	                        <HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
		                        BackColor="White"></HolidayStyle>
                        </ew:calendarpopup>
                        <asp:button id="btnConsultar" runat="server" Text="Consultar Fecha" onclick="btnConsultar_Click"></asp:button>
                    </p>
                </td>
            </tr>
            <tr>
                <td> 
                    <p> <strong>Placa: </strong> 
                        <asp:TextBox id="TextBoxPlaca" runat="server" Width="84px"></asp:TextBox>
                        <asp:button id="btnConsultarPlaca" runat="server" Text="Consultar Placa" onclick="btnConsultarPlaca_Click"></asp:button>
                        <INPUT id="hdFechProc" type="hidden" runat="server" name="hdFechProc">
                    </p>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>            
        </table>
    </p>
    

<asp:datagrid id="dgCitasDia" runat="server" AutoGenerateColumns="False"
	GridLines="Vertical" cssclass="datagrid">
			<FooterStyle CssClass="footer"></FooterStyle>
			<HeaderStyle CssClass="header"></HeaderStyle>
			<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Citas">
			<ItemTemplate>
				<table border="0" cellpadding="0" cellspacing="1" width="100%" style="FONT-SIZE: 8pt; COLOR: black; FONT-FAMILY: Verdana; BACKGROUND-COLOR: transparent">
					<tr>
						<td>Fecha de la Cita :</td>
						<td align="right">
                        <asp:label id="fechaCita" runat="server"> <%# DataBinder.Eval(Container.DataItem, "mcit_fecha") %></asp:label>
                        <input id="hdFecCita" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "mcit_fecha") %>'>
                        </td>
					</tr>
                    <tr>
						<td>Hora de la Cita :</td>
						<td align="right"><%# DataBinder.Eval(Container.DataItem, "mcit_hora") %><input id="hdHorCit" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "mcit_hora") %>'></td>
					</tr>
					<tr>
						<td>Recepcionista asignado :</td>
						<td align="right"><input id="inptCodVend" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "mcit_codven") %>'> <%# DataBinder.Eval(Container.DataItem, "pven_nombre") %><input id="hdCodRcp" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "mcit_codven") %>'><input id="hdNombRec" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "pven_nombre") %>'></td>
					</tr>
					<tr>
						<td>Nombre del Cliente :</td>
						<td align="right"><%# DataBinder.Eval(Container.DataItem, "mcit_nombre") %></td>
					</tr>
					<tr>
						<td>Placa del Vehículo :</td>
						<td align="right"><%# DataBinder.Eval(Container.DataItem, "mcit_placa") %><input id="hdPlaca" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "mcit_placa") %>'></td>
					</tr>
					<tr>
						<td>Catalogo del Vehículo :</td>
						<td align="right"><%# DataBinder.Eval(Container.DataItem, "pcat_descripcion") %><input id="hdCodCat" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "pcat_codigo") %>'></td>
					</tr>
					<tr>
						<td>Servicio Registrado :</td>
						<td align="right"><%# DataBinder.Eval(Container.DataItem, "pkit_nombre") %><input id="hdPkCod" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "pkit_codigo") %>'></td>
					</tr>
                    <tr> 
						<td>Observaciones :</td>
						<td align="right"><%# DataBinder.Eval(Container.DataItem, "mcit_observacion") %></td>
					</tr>
					<tr>
						<td colspan="2" align="center">
							<asp:Button id="btnEliminar" runat="server" Text="Eliminar" CommandName="delete" class="noEspera"></asp:Button>
                                <INPUT id="btnEditar" type="button" value="Editar" runat="server" class="noEspera">
						</td>
					</tr>
				</table>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<div id="overlay" style="DISPLAY: none; Z-INDEX: 90; LEFT: 0px; WIDTH: 100%; POSITION: absolute; TOP: 0px"></div>
<div id="lightbox" class="dialogoFlotante">
	<table id="Table1" class="filtersInAuto">
		<tr>
			<td width="35%">Fecha de Cita :</td>
			<td align="right"><input id="hdFechAnt" runat="server" type="hidden">
                <ew:calendarpopup id="cpFechaNuevaCita" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage"
					runat="server" Culture="Spanish (Colombia)" JavascriptOnChangeFunction="CambioRecepcionista">
					<WeekdayStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="White"></WeekdayStyle>
					<MonthHeaderStyle Font-Size="X-Small" Font-Names="Arial" ForeColor="Black" BackColor="Silver"></MonthHeaderStyle>
					<OffMonthStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="#FF8080"></OffMonthStyle>
					<GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="White"></GoToTodayStyle>
					<TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="LightGoldenrodYellow"></TodayDayStyle>
					<DayHeaderStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightBlue"></DayHeaderStyle>
					<WeekendStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightGray"></WeekendStyle>
					<SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="Khaki"></SelectedDateStyle>
					<ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="White"></ClearDateStyle>
					<HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="White"></HolidayStyle>
				</ew:calendarpopup></td>
		</tr>
		<tr>
			<td width="35%">Horario :</td>
			<td align="right"><input id="hdHorAnt" runat="server" type="hidden"><asp:dropdownlist id="ddlHorario" runat="server"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
		</tr>
		<tr>
			<td width="35%">Taller :</td>
			<td align="right"><asp:dropdownlist id="ddlTaller" runat="server" OnChange="CambioTaller();"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
		</tr>
		<tr>
			<td width="35%">Recepcionista :</td>
			<td align="right"><input id="hdRecAnt" runat="server" type="hidden"><asp:dropdownlist id="ddlRecepcionista" runat="server" OnChange="CambioRecepcionista();"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
		</tr>
		<tr>
			<td align="center" colSpan="2"><asp:button id="btnAceptarEdicion" runat="server" Text="Aceptar" onclick="btnAceptarEdicion_Click" class="noEspera"></asp:button>&nbsp;<INPUT id="btnCancelar" onclick="hideLightbox();" type="button" value="Cancelar" class="noEspera" runat="server"
					name="btnCancelar"></td>
		</tr>
	</table>
</div>
</fieldset>
