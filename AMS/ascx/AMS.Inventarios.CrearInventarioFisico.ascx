<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.CrearInventarioFisico.ascx.cs" Inherits="AMS.Inventarios.CrearInventarioFisico" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<fieldset>
	<legend>
		Información basica para la creación del Inventario Físico</legend>
	<table id="Table1" class="filtersIn">
		<TR>
			<TD><STRONG>Prefijo del Inventario:</STRONG><br />
               <asp:DropDownList id="ddlPrefijo" class="dmediano" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefijo_SelectedIndexChanged">
               </asp:DropDownList>
                </TD>
		
			<TD><STRONG>Número Inventario:</STRONG><br />
			
				<asp:TextBox id="NumeroInv" runat="server" class="tpequeno" Enabled="False"></asp:TextBox>
				<asp:RequiredFieldValidator id="rqNumInv" runat="server" ControlToValidate="NumeroInv" ErrorMessage="Ingrese un Número de Inventario Valido" Display="None"></asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator id="revNumInv" runat="server" ValidationExpression="\d+" ControlToValidate="NumeroInv" ErrorMessage="El valor de número de Inventario ingresado es Invalido" Display="None"></asp:RegularExpressionValidator>
                </TD>
		</TR>
		<TR>
			<TD><STRONG>Tipo de Inventario:</STRONG>
			<br/>
				<asp:DropDownList id="ddltipo" runat="server" class="dpequeno"></asp:DropDownList>
				<asp:RequiredFieldValidator id="rqTipInv" runat="server" ControlToValidate="ddltipo" ErrorMessage="Seleccione un Tipo de Inventario" Display="None"></asp:RequiredFieldValidator>
                </TD>
		
			<TD>
            <div id="div_lbTipoUbicacion"><STRONG>Tipo de Inventario Ubicación:</STRONG>
            </div>
            
			
				<div id="div_ddlTipoUbicacion" >
					<asp:DropDownList id="ddltipoubic" runat="server" class="dmediano"></asp:DropDownList>
					<asp:RequiredFieldValidator id="rqTipoUb" runat="server" ControlToValidate="ddltipoubic" ErrorMessage="Por favor seleccione un Tipo de Ubicación para el Inventario" Display="None"></asp:RequiredFieldValidator>
				</div>
                </TD>
		</TR>

		<TR>
			<TD ><STRONG>Costo de Contador por Día:</STRONG>
            <br />
			
				<asp:TextBox id="costo" runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
				<asp:RequiredFieldValidator id="rqCostCont" runat="server" ControlToValidate="costo" ErrorMessage="Por favor ingrese el valor de costo por dia de contador" Display="None"></asp:RequiredFieldValidator>
			</TD>
		
		
			<TD><STRONG>Fecha de Inicio:</STRONG>
            <br>
			<ew:CalendarPopup id="cldFechInventario" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage" runat="server" Culture="Spanish (Colombia)">
					<WeekdayStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="White"></WeekdayStyle>
					<MonthHeaderStyle Font-Size="X-Small" Font-Names="Arial" ForeColor="Black" BackColor="Silver"></MonthHeaderStyle>
					<OffMonthStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="#FF8080"></OffMonthStyle>
					<GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></GoToTodayStyle>
					<TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="LightGoldenrodYellow"></TodayDayStyle>
					<DayHeaderStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightBlue"></DayHeaderStyle>
					<WeekendStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightGray"></WeekendStyle>
					<SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="Khaki"></SelectedDateStyle>
					<ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></ClearDateStyle>
					<HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black" BackColor="White"></HolidayStyle>
				</ew:CalendarPopup>
				<asp:RequiredFieldValidator id="rqFechInvFsc" class="amediano" runat="server" ControlToValidate="cldFechInventario" ErrorMessage="Por favor ingrese el valor de fecha de inicio de inventario fisico" Display="None"></asp:RequiredFieldValidator>
                </TD>
		</TR>
		<tr>
			<td colspan="2" align="left"><asp:Button id="botoncrear" runat="server" Text="Crear Inventario" onclick="botoncrear_Click"></asp:Button></td>
		</tr>
	</TABLE>
</fieldset>
<asp:ValidationSummary id="vsTotal" runat="server" ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
