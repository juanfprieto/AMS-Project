<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.ClasificacionABC.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_ClasificacionABC" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>


<fieldset>
<legend class="Legends">Clasificación ABC</legend>
	<table id="Table1" class="filtersIn">
    <tbody>
    <tr>
    <td>
		<tr><p>

			<td>
                    Por favor indique el numero de meses que requiere para realizar el 
				    analisis de clasificación ABC:
            </td>
			<td><asp:requiredfieldvalidator id="rqTbCantidadMeses" Display="None" ErrorMessage="Por favor ingrese la cantidad de meses para realizar el analisis"
					runat="server" ControlToValidate="tbCantidadMeses"></asp:requiredfieldvalidator>
                    <asp:regularexpressionvalidator id="revTbCantidadMeses" Display="None" ErrorMessage="El valor ingresado para los meses debe ser numerico"
					runat="server" ControlToValidate="tbCantidadMeses" ValidationExpression="[0-9]+"></asp:regularexpressionvalidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:textbox id="tbCantidadMeses" class="tpequeno" runat="server"></asp:textbox>
                    </td></p>
		</tr>
		<tr>
			<td>Por favor indique que tipo de variable requiere para realizar este estudio:</td>
			<td align="right"><asp:dropdownlist id="ddlVariable" class="dmediano" runat="server">
					<asp:ListItem Value="A" Selected="True">Por Vlr y Unidds Vendidas </asp:ListItem>
                    <asp:ListItem Value="C">Por Unidades Vendidas </asp:ListItem>
					<asp:ListItem Value="D">Por Costo de Venta</asp:ListItem>
				</asp:dropdownlist></td>
		</tr>
		<tr>
			<td align="center" colSpan="2"><asp:button id="btnClasificar" runat="server" Text="Clasificar" onclick="btnClasificar_Click" onclientclick="espera();"></asp:button></td>
		</tr>
        </td>
        </tr>
        </tbody>
	</table>
</fieldset>
<P>&nbsp;
	<asp:DataGrid id="dgClasificacionABC" runat="server" cssclass="datagrid" AllowPaging="true" PageSize="3000" ShowHeader="true" CellPadding="2" OnPageIndexChanged="DgUpdate_Page">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
         <PagerStyle borderwidth="2px" borderstyle="Dotted" cssclass="pager"
		             position="TopAndBottom" mode="NumericPages"></PagerStyle>
	</asp:DataGrid></P>
<P>
	<asp:Label id="lbText" runat="server"></asp:Label></P>
<asp:ValidationSummary id="vsTotal" runat="server" ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
