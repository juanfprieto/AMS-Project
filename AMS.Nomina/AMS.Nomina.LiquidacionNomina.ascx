<%@ Control Language="c#" codebehind="AMS.Nomina.LiquidacionNomina.cs" autoeventwireup="false" Inherits="AMS.Nomina.LiquidacionNomina" %>
<fieldset>
<table id="Table" class="filtersIn">
<tbody>
<tr>
<td>

<h4>PRELIQUIDACION INFORMATIVA
</h4>
<p>
<table id="Table" class="filtersIn">
<tr>
<td>
<asp:label id="titulo" runat="server">Ingrese los datos</asp:label> &nbsp; <asp:label id="prueba" runat="server" class="lmediano" visible="False"></asp:label><asp:label id="lbdocref" runat="server"></asp:label><asp:label id="lbmas1" runat="server" visible="False"></asp:label><asp:label id="prueba2" runat="server"></asp:label>
			<p><asp:label id="Label5" runat="server">El proceso de 
                liquidación de nómina, realiza los pagos de los empleados correspondientes al 
                PERIODO VIGENTE, antes de realizar este procedimiento debe haber ingresado las 
                novedades que afecten al período de pago. Asegurese de tener calculado y 
                actualizado los porcentajes y tipos de UVTS para la Retención en la Fuente. EL 
                procedimiento 1 se toma de acuerdo a la tabla definida, 
                    y el procedimiento 2 se calcula con base en el % definido para cada empleado. 
                    <br />
                    <b>NOTA: Usted debe definir el porcentaje para el procedimiento 2, y actualizarlo en el sistema.</b>
                    </asp:label>
                    </td>
                    </tr>
                    </table>
                    </p>

			<table class="filtersIn">
				<tr>
					<td><asp:label id="Label1" runat="server" class="lmediano">Período a procesar </asp:label></td>
					<td><asp:dropdownlist id="DDLQUIN" runat="server" class="dmediano"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:label id="Label3" runat="server">Mes</asp:label></td>
					<td><asp:dropdownlist id="DDLMES" runat="server" class="dmediano"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td>&nbsp;<asp:label id="Label2" runat="server" class="lpequeno">Año</asp:label>
					</td>
					<td><asp:dropdownlist id="DDLANO" runat="server" class="dmediano"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:label id="Label4" runat="server">Tipo de pago</asp:label></td>
					<td><asp:label id="lbtipopago" runat="server" class="lmediano"></asp:label></td>
                    <td><asp:label id="lbempleado" runat="server" class="lgrande"></asp:label></td>
				</tr>
				<tr>
				</tr>
				<tr>
					<td><asp:button id="consulta" onclick="realizar_consulta" runat="server" Text="Pre Liquidar" onclientclick="espera();" ></asp:button></td>
				</tr>
				<tr>
				</tr>

<p></p>
<p></p>
<p>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					<p><asp:label id="Label8" runat="server" class="lmediano">Nombre del Liquidador:</asp:label></p>
				</td>
				<td><asp:label id="lbliquidador" runat="server" class="lmediano"></asp:label></td>
			</tr>
		</tbody>
        </table>
</p>
<p><asp:placeholder id="toolsHolder" runat="server">
		<TABLE class="tools">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
                <td> <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel" OnClick="ImprimirExcelGrid" runat="server"
                                    alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" cssClass="Excelimg" style="width: 25%; margin-bottom: -6px;">
                                </asp:ImageButton></td>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" alt="Enviar por email"
						ImageUrl="../img/AMS.Icon.Mail.jpg"></asp:ImageButton></TD>
				<td></td>
			</TR>
		</TABLE>
	</asp:placeholder></p>
<p>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td align="center">
                    <asp:button id="BTNCONFIRMACION" onclick="BTNCONFIRMACION_Click" runat="server" Text="LIQUIDAR DEFINITIVAMENTE"
						Visible="False" OnClientClick="espera();return confirm('Esta a punto de realizar la liquidacion definitiva. Tenga en cuenta que si lo hace, ya no habra marcha atras. Esta seguro de que desea realizar la liquidacion definitiva?');" >
                    </asp:button>
                    <asp:button id="btnGenInforme" onclick="generarInforme" runat="server" visible="False" Width="138px"
						Text="Generar Informe" UseSubmitBehavior="false" OnClientClick="espera();" class="noEspera">
                    </asp:button><asp:hyperlink id="hl" runat="server" Visible="False">Bajar Informe</asp:hyperlink>
               </td>
			</tr>
		</tbody></table>
</p>
<p><asp:placeholder id="phGrilla" runat="server">
		<p>
			<asp:DataGrid id="DataGrid2" runat="server" onEditCommand="editar" onDeleteCommand="eliminar"
				onCancelCommand="cancelar" onPageIndexChanged="Grid_Change" AllowPaging="true" AutoGenerateColumns="False" class="datagrid">
				<HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
            <FooterStyle cssclass="footer"></FooterStyle>
            <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
				<Columns>
					<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="CONCEPTO" HeaderText="CONCEPTO" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCION"></asp:BoundColumn>
					<asp:BoundColumn DataField="CANT EVENTOS" HeaderText="CANTIDAD"></asp:BoundColumn>
					<asp:BoundColumn DataField="A PAGAR" HeaderText="A PAGAR" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="A DESCONTAR" HeaderText="A DESCONTAR" DataFormatString="{0:C}"></asp:BoundColumn>
					<asp:BoundColumn DataField="SALDO" HeaderText="SALDO" DataFormatString="{0:C}"></asp:BoundColumn>
				</Columns>
			</asp:DataGrid></p>
	</asp:placeholder>
<P></P>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<p></p>
<br>
<br><asp:label id="lberroresdetectados" runat="server" visible="False" font-size="Larger"></asp:label><br/>
<br/>
<asp:label id="lbsaldorojo" runat="server" class="lmediano" visible="False" height="29px" font-size="Larger">Listado
Empleados con Saldo en Rojo</asp:label>
<br/>
<br/>
<br/>
<br/>
<asp:datagrid id="DataGrid3" runat="server" AutoGenerateColumns="False" 
    AllowPaging="True" PageSize="500" cssclass="datagrid">
	<HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
            <FooterStyle cssclass="footer"></FooterStyle>
            
	<Columns>
		<asp:BoundColumn DataField="EMPLEADO"   HeaderText="EMPLEADO">  </asp:BoundColumn>
		<asp:BoundColumn DataField="DEVENGADOS" HeaderText="DEVENGADOS" DataFormatString="{0:C}"></asp:BoundColumn>
		<asp:BoundColumn DataField="DESCUENTOS" HeaderText="DESCUENTOS" DataFormatString="{0:C}"></asp:BoundColumn>
		<asp:BoundColumn DataField="SALDO"      HeaderText="SALDO"      DataFormatString="{0:C}"></asp:BoundColumn>
	</Columns>
</asp:datagrid><br/>

<asp:label id="lbperipago" runat="server" visible="False" font-size="Larger">Listado
Empleados Diferente Período de Pago al Seleccionado</asp:label>
<p></p>
<p></p>
<p></p>
<p><asp:datagrid id="DataGrid1" runat="server" 
        onPageIndexChanged="Grid_Change2" AllowPaging="True"
		AutoGenerateColumns="False" ShowFooter="True" AllowCustomPaging="True" 
        PageSize="500">
		    <HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
            <FooterStyle cssclass="footer"></FooterStyle>
            <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<Columns>
			<asp:BoundColumn DataField="EMPLEADO" HeaderText="EMPLEADO"></asp:BoundColumn>
			<asp:BoundColumn DataField="PERIODO DE PAGO" HeaderText="PERIODO DE PAGO"></asp:BoundColumn>
		</Columns>
	</asp:datagrid></p>
<p></p>
<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<p>&nbsp;</p>
<p><asp:label id="lb" runat="server" visible="False"></asp:label><asp:label id="lb2" runat="server" visible="False"></asp:label><asp:label id="lbpag" runat="server" visible="False"></asp:label><asp:label id="lbpag2" runat="server"></asp:label><asp:label id="lb3" runat="server" width="59px" visible="False"></asp:label></p>
<p><asp:label id="lblError" runat="server" visible="True"></asp:label></p>
</td></tr></tbody></table>
</fieldset>
<script type="text/javascript">
    function Lista() {
        w=window.open('ImpresionGrillaLiquidacionNomina.aspx');
    }
</script>
