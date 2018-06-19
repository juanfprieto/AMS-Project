<%@ Control Language="c#" autoeventwireup="false" codebehind="AMS.Nomina.PlanillaBanco.ascx.cs" Inherits="AMS.Nomina.AMS_Nomina_PlanillaBanco" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>Se generará el archivo para el Banco que seleccione. Por favor asegurese en Configuración de Nómina de verificar el número de la cuenta y código de la Sucursal de su Banco y 
	la&nbsp;Ultima&nbsp;Quincena&nbsp;Liquidada. Además revice los nombres que contienen Ñ o ñ que no tengan caracteres Adicionales.
</p>
</td></tr>
</table>
<p>
	<table id="Table1" class="filtersIn">
		<tbody>
			<tr>
				<td>AÑO</td>
				<td><asp:dropdownlist id="DDLANO" class="dpequeno" runat="server"></asp:dropdownlist></td>
			</tr>
			<tr>
				<td>MES</td>
				<td><asp:dropdownlist id="DDLMES" class="dpequeno" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
			</tr>
			<tr>
				<td>QUINCENA</td>
				<td><asp:dropdownlist id="DDLQUIN" class="dpequeno" runat="server"></asp:dropdownlist></td>
			</tr>
		</tbody>
	</table>
</p>
<table id="Table3" class="filtersIn">
<tr>
<td>
<p>Recuerde revisar: Numeros de cuenta&nbsp;Empleados(Empleados).
</p>
<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Numero 
	de cuenta&nbsp;Empresa(Configuración de Nómina)
</p>
<P><asp:radiobuttonlist id="rblBancos" runat="server" Width="128px">
		<asp:ListItem Value="CONAVI" Selected="True">BANCOLOMBIA</asp:ListItem>
		<asp:ListItem Value="BOGOTA">BOGOTA</asp:ListItem>
	</asp:radiobuttonlist>
</P>
</td>
</tr>
</table>
<P>
	<table id="Table2" class="filtersIn">
		<tr>
			<td>Fecha de Aplicación(aaMMdd)</td>
			<td><asp:textbox id="txtFechaAplicacion" minlength="6" maxlength="6" runat="server"></asp:textbox></td>
		</tr>
	</table>
</P>
<table>
    <tr>
        <td>
        
         <P><asp:hyperlink id="hl" runat="server" Visible="False">HyperLink</asp:hyperlink></P>
        </td>
        <td>Generar Excel
            <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel"  OnClick="ImprimirExcelGrid" Visible="False" runat="server"
             alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" Width="40px">
              </asp:ImageButton>
        </td>    
    
    
    </tr>
</table>

<asp:button id="btnGenerar" runat="server" Text="Generar"></asp:button>
</fieldset>
