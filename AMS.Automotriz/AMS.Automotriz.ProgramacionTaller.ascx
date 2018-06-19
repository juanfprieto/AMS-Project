<%@ Control Language="c#" codebehind="AMS.Automotriz.ProgramacionTaller.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.ProgramacionTaller" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<script type ="text/javascript">
	function MostrarOcultarDiv(idDiv)
	{
		var objDiv = document.getElementById(idDiv);
		if(objDiv.style.display == '')
			objDiv.style.display = 'none';
		else if(objDiv.style.display == 'none')
			objDiv.style.display = '';
	}
</script>
<fieldset>

<table id="Table" class="filtersIn">
<legend>Datos de la Consulta</legend>
    <tr>
       <td>
            Taller:
            <asp:DropDownList id="taller" runat="server" class="dmediano">
            </asp:DropDownList>
        </td>
        <td>Fecha Inicial:
            <ew:calendarpopup id="FechaInicial" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage"
									runat="server" Culture="Spanish (Colombia)" width="80">
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
        </td>
        <td>Fecha Final:
        <ew:calendarpopup id="FechaFinal" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage"
	     runat="server" Culture="Spanish (Colombia)" width="80"> 
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
            </td>           
           <td><asp:button id="Consultar" onclick="Cambio_Taller" runat="server" Text="Consultar"></asp:button></td>
        </tr>
    </table>
</fieldset>
<fieldset>
<table id="Table1" class="filtersIn">
<tr>
<td>
    <legend>Ordenes En El Taller</legend>
	<P>
		En Proceso :
		<asp:Label id="enProceso" runat="server" font-bold="True"></asp:Label>&nbsp; 
		Preliquidadas :
		<asp:Label id="preliquidadas" runat="server" font-bold="True"></asp:Label>&nbsp;
	</P>
	<P>
		Facturadas SIN SALIDA :
		<asp:Label id="facturadasSinSalida" runat="server" font-bold="True"></asp:Label>
    </P>

<p>
	<asp:RadioButtonList id="tipGraficos" OnSelectedIndexChanged="Cambio_Grafico" AutoPostBack="true" runat="server"
		RepeatDirection="Horizontal">
		<asp:ListItem Value="M">Gr&#225;fico de Mec&#225;nicos</asp:ListItem>
		<asp:ListItem Value="O">Gr&#225;fico de Operaciones</asp:ListItem>
	</asp:RadioButtonList>
</p>
<p>
</p>
<table id="Table2" class="filtersIn">
<tr>
        <td class="scrollable">
    º 
	<legend>Actividad Recepcionistas</legend>
	<asp:DataGrid id="actividadesRecepcionistas" runat="server" AutoGenerateColumns="false"
		Font-Names="Verdana" BorderWidth="1px" GridLines="Vertical" BorderStyle="None" BackColor="White"
		BorderColor="#999999" CellPadding="3" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="CODIGO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NOMBRE">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ORDENES TRABAJO ABIERTAS (CLIENTES)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OTPC", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ORDENES TRABAJO ABIERTAS (INTERNO)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OTPI", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ORDENES TRABAJO PRELIQUIDADAS (CLIENTE)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OTPRC", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ORDENES TRABAJO PRELIQUIDADAS (INTERNO)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OTPRI", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ORDENES TRABAJO FACTURADAS SIN SALIDA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OTFSS", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="TOTAL ORDENES">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TOTALORDENES", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ULTIMAS ORDENES ABIERTAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ULTIMASORDENES") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
             
        </td>
    </tr>
    </table>
    <table id="Table3" class="filtersIn">
<tr>
        <td class="scrollable">
     <fieldset> 
	<legend>Actividad Mecanicos/Contratistas</legend>
	<asp:DataGrid id="actividadesMecanicos"  runat="server" AutoGenerateColumns="false"
		Font-Names="Verdana" BorderWidth="1px" GridLines="Vertical" BorderStyle="None" BackColor="White"
		BorderColor="#999999" CellPadding="3" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="CODIGO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NOMBRE">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="OPERACIONES ASIGNADAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OPRASIG", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS ASIGNADAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HORASIG", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="OPERACIONES CUMPLIDAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OPRCUMP", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS CUMPLIDAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HORCUMP", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="OPERACIONES PARALIZADAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "OPRPARA", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS PARALIZADAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HORPARA", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS TOTALES">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HORTOTAL", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS DISPONIBLES">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HORDISPO", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NUMERO DE ORDENES ASIGNADAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NUMORDEN", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ULTIMAS ORDENES ASIGNADAS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ULTIMASORDENES") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
             </fieldset>
        </td>
    </tr>
    </table>


	<p>
	</p>
     <table id="Table4" class="filtersIn">
<tr>
        <td class="scrollable">
     <fieldset> 
	<legend>Total Operaciones en Ordenes</legend>
	<asp:DataGrid id="totalOperacionesOrdenes"  runat="server" AutoGenerateColumns="false"
		Font-Names="Verdana" BorderWidth="1px" GridLines="Vertical" BorderStyle="None" BackColor="White"
		BorderColor="#999999" CellPadding="3" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="TIPO DE TRABAJO">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TIPOTRABAJO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS ASIGNADAS (CLIENTE)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HACL", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS ASIGNADAS (INTERNO)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HAIN", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS CUMPLIDAS (CLIENTE)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HCCL", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS CUMPLIDAS (INTERNO)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HCIN", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS PARALIZADAS REPUESTOS (CLIENTE)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HPRCL", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS PARALIZADAS REPUESTOS (INTERNO)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HPRIN", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS PARALIZADAS POR AUSENTISMO, HERRAMIENTA O SIN ASIGNACIÓN (CLIENTE)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HPTCL", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="HORAS PARALIZADAS POR AUSENTISMO, HERRAMIENTA O SIN ASIGNACIÓN (INTERNO)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "HPTIN", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
                 </fieldset>
        </td>
    </tr>
    </table>

<p>
</p>
<asp:Label id="lb" runat="server"></asp:Label>
</td>
</tr>
</table>
</fieldset>
