<%@ Control Language="c#" codebehind="AMS.Nomina.PrimaNormal.cs" autoeventwireup="false" Inherits="AMS.Nomina.PrimaNormal" %>
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
<p>
	Seleccione tipo de prima
</p>
<p>
	<asp:DropDownList id="DDLTIPOPRIMA" class="dmediano" runat="server">
		<asp:ListItem Value="1">Normal</asp:ListItem>
		<asp:ListItem Value="2">Ajustada</asp:ListItem>
	</asp:DropDownList>
</p>
<p>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					AÑO
				</td>
				<td>
					<asp:DropDownList id="DDLANO" class="dpequeno" runat="server"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					Mes Inicial
				</td>
				<td>
					<asp:DropDownList id="DDLMESINICIAL" class="dpequeno" runat="server" AutoPostBack="True" onSelectedIndexChanged="cambiomes">
						<asp:ListItem Value="Enero">Enero</asp:ListItem>
						<asp:ListItem Value="Julio">Julio</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					Mes Final
				</td>
				<td>
					<asp:Label id="LBMESFINAL" runat="server" height="16px" class="lpequeno"></asp:Label></td>
			</tr>
		</tbody>
	</table>
</p>
<p>
	<asp:Button id="BTNLIQUIDAR" onclick="LiquidacionPrimas" runat="server" Text="Pre_Liquidar" >
	</asp:Button>
</p>
<p>
	<asp:placeholder id="toolsHolder" runat="server">
<TABLE class="filtersIn">
  <TR>
    <th class="filterHead">
				<img height="30" src="../img/AMS.Flyers.Tools.png" border="0">
	</th>
    <TD>Imprimir <A href="javascript: Lista()"><img height="18" alt="Imprimir" 
      src="../img/AMS.Icon.Printer.png" width="20" border="0"> </A></TD>
    <TD>&nbsp; &nbsp;Enviar por correo 
        <asp:TextBox id="tbEmail" runat="server"></asp:TextBox>
    </TD>
    <TD>
        <asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server" ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        <asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail.jpg" alt="Enviar por email" BorderWidth="0px"></asp:ImageButton>
    </TD>
   
    <td>Generar Excel
        <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel"  OnClick="ImprimirExcelGrid" runat="server"
         alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" Width="40px">
          </asp:ImageButton>
    </td>

  </TR>
</TABLE>
	</asp:placeholder><asp:placeholder id="phGrilla" runat="server">
<P>Liquidación de Prima para el Período Seleccionado 
<asp:Label id="lbfechainicio" runat="server"></asp:Label>&nbsp; 
<asp:Label id="lbfechafinal" runat="server"></asp:Label></P>
<P>
<asp:DataGrid id="DATAGRIDPRIMA" runat="server" AutoGenerateColumns="False">
				<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
				<Columns>
					<asp:BoundColumn DataField="CODIGO EMPLEADO" HeaderText="CODIGO EMPLEADO"></asp:BoundColumn>
					<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE"></asp:BoundColumn>
					<asp:BoundColumn DataField="VALOR PRIMA" HeaderText="VALOR PRIMA" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="DIAS TRABAJADOS" HeaderText="DIAS TRABAJADOS"></asp:BoundColumn>
					<asp:BoundColumn DataField="SUELDO PROMEDIO" HeaderText="ACUMULADO BASE" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="SUELDO ACTUAL" HeaderText="SUELDO ACTUAL" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="DESCUENTOS" HeaderText="DESCUENTOS" DataFormatString="{0:C}"></asp:BoundColumn>
				</Columns>
			</asp:DataGrid></P>
<P>Valor Total Prima del Período </P>
<P>
<asp:Label id="LBTOTALPRIMA" runat="server" width="117px" height="16px"></asp:Label></P>
	</asp:placeholder>
	<asp:Button id="BTNLIQUIDARDEFINITIVAMENTE" onclick="grabarPrimas" runat="server" Text="LIQUIDAR DEFINITIVAMENTE"
		Visible="False" >
		</asp:Button>
<P></P>
<p>
	<asp:Label id="LBPRUEBAS" runat="server" class="lmediano" visible="False"></asp:Label>
</p>
</td></tr></table></fieldset>