<%@ Control Language="c#" codebehind="AMS.Nomina.LiquidacionDefinitiva.cs" autoeventwireup="false" Inherits="AMS.Nomina.LiquidacionDefinitiva" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<fieldset>
<p></p>

<table id="Table" class="filtersIn">
	<tbody>
		<tr>
        Empleado
                    <br />
                    <asp:dropdownlist id="DDLEMPLEADO" runat="server" class="dmediano"></asp:dropdownlist>
			<td>
					<tr>                    
						<td>Fecha de Retiro<br />AÑO</td>
						<td>MES</td>
						<td>DIA</td>
					</tr>
							<tr>
								<td>&nbsp;<asp:dropdownlist id="DDLANORETI" class="dpequeno" runat="server"></asp:dropdownlist></td>
								<td><asp:dropdownlist id="DDLMESRETI" class="dpequeno" runat="server"></asp:dropdownlist></td>
								<td><asp:dropdownlist id="DDLDIARETI" class="dpequeno" runat="server"></asp:dropdownlist></td>
                         <%--       <td>Días ajuste primer Semestre&nbsp; &nbsp;
					            <asp:TextBox id="DiasPrimer" class="tpequeno" runat="server">0</asp:TextBox></td>--%>
							</tr>
				
			</td>
			<td>
						<tr>
							<td> Fecha de Liquidación<br />AÑO</td>
							<td>MES</td>
							<td>DIA</td>
						</tr>
						<tr>
							<td>&nbsp;<asp:dropdownlist id="DDLANOLIQ" class="dpequeno" runat="server"></asp:dropdownlist></td>
							<td><asp:dropdownlist id="DDLMESLIQ" class="dpequeno" runat="server"></asp:dropdownlist></td>
							<td><asp:dropdownlist id="DDLDIALIQ" class="dpequeno" runat="server"></asp:dropdownlist></td>
                    <%--        <td>
					Días ajuste&nbsp;segundo Semestre&nbsp;&nbsp;
					<asp:TextBox id="DiasSegundo" class="tpequeno" runat="server">0</asp:TextBox></td>--%>
						</tr>
			</td>
		</tr>
		<tr>
			<td>&nbsp;
				<DIV style="DISPLAY:inline; WIDTH:248px; HEIGHT:24px" ms_positioning="FlowLayout">
					<asp:CheckBox id="CheckBox1" runat="server">
					</asp:CheckBox>Ya liquidó y Consignó Cesantías del Año Anterior?</DIV>
			</td>
			<td>
				<p>
				</p>
			</td>
		</tr>
	</tbody>
</table>
<table id="Table" class="filtersIn">
<tr>
<td>

<p>Motivo del Retiro
</p>
<p><asp:dropdownlist id="DDLMOTRETIRO" class="dmediano" runat="server"></asp:dropdownlist></p>
<p><asp:button id="Button2" onclick="LiquidacionParcial" runat="server" Text="Base Liquidación" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')">
</asp:button></p>
</td>
</tr>
</table>
<asp:placeholder id="toolsHolder" runat="server">
<TABLE class=tools>
  <TR>
    <th class="filterHead">
			   <IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0">
			</th>
    <TD>Imprimir <A href="javascript: Lista()"><IMG height=18 alt=Imprimir 
      src="../img/AMS.Icon.Printer.png" width=20 border=0> </A></TD>
    <TD>&nbsp; &nbsp;Enviar por correo 
<asp:TextBox id=tbEmail runat="server"></asp:TextBox></TD>
    <TD>
<asp:RegularExpressionValidator id=FromValidator2 style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
<asp:ImageButton id=ibMail onclick=SendMail runat="server" BorderWidth="0px" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"></asp:ImageButton></TD>
    <TD width=380></TD></TR></TABLE>
</asp:placeholder><asp:placeholder id="phGrilla" runat="server">
<P>
<asp:panel id=panelbaseliquidacion Width="370px" runat="server" BackColor="Transparent" Visible="false">
<TABLE style="BACKGROUND-COLOR: white">
 <TR>
     <td></td>
     <td>
     <Th> Empleador:</Th></td>
<TD><asp:Label id="LabelEmpresa" runat="server" class="lpequeno"></asp:Label></TD></TR>
    <tr>
        <td></td>
        <td><Th>Nit Empleador:</Th></td>
<TD><asp:Label id=LabelNitEmpresa runat="server" class="lpequeno"></asp:Label></TD>
<TD><asp:Label id=LabelDigitoEmpresa runat="server" class="lpequeno"></asp:Label></TD>
</TR>
<TR>
    <th colspan="5">Liquidaciones de Prestaciones Sociales a Empleado</th></TR></td> 
  <TR>
    <Th colspan="1">Fecha Ingreso del Empleado:</Th>
    <TD>
<asp:Label id=LBFECHAINGRESO runat="server" class="lpequeno"></asp:Label></TD>
    <Th colspan="1">Fecha Retiro del Empleado:</Th>
    <TD>
<asp:Label id=LBFECHARETIRO runat="server" class="lpequeno"></asp:Label></TD>
     <Th colspan="1">Nombre del Empleado: </Th>
    <TD>
<asp:Label id=LBNOMBREEMPLEADO runat="server" class="lpequeno"></asp:Label></TD></TR>
  <TR>
    <Th colspan="1">Identificación:</Th><br />
      <TD><asp:Label id=LBIDENTIFICACION runat="server" class="lpequeno"></asp:Label></TD><br />
      <Th colspan="1"> Cargo: </Th>
    <TD><asp:Label id=LBCARGO runat="server" class="lpequeno"></asp:Label></TD><br />
    <Th colspan="1">Dependencia:</Th>
    <TD><asp:Label id=LBDEPENDENCIA runat="server" class="lpequeno"></asp:Label></TD><br />
  </TR>
  <TR>
    <Th colspan="1">Período&nbsp;de Pago: </Th>
    <TD>
<asp:Label id=lbtipopago runat="server" class="lpequeno"></asp:Label></TD>
    <Th colspan="1">Sueldo Actual: </Th><br />
    <TD>
<asp:Label id=LBSUELDOCARGO runat="server" class="lpequeno"></asp:Label></TD>
    <Th colspan="1">Base Liquidación Cesantías: </Th>
    <TD>
<asp:Label id=LBBASELIQUIDACION runat="server" class="lpequeno"></asp:Label></TD></TR>
  <TR>
    <Th>Base Liquidación Vacaciones: </Th>
    <TD>
<asp:Label id=LBBASELIQVACA runat="server" class="lpequeno"></asp:Label></TD>
    <Th>Base Liquidación Primas: </Th>
    <TD>
<asp:Label id=LBBASELIQPRIMA runat="server" class="lpequeno"></asp:Label></TD>
  </TR></TABLE></asp:panel></P>Cesantias 
Pagadas Anteriormente al Empleado: 
<P></P>
<P>
<asp:DataGrid id=DATAGRIDCESAANTERIORES runat="server" AutoGenerateColumns="False">
			<HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
            <FooterStyle cssclass="footer"></FooterStyle>
			<Columns>
				<asp:BoundColumn DataField="FECHA INICIO" HeaderText="FECHA INICIO"></asp:BoundColumn>
				<asp:BoundColumn DataField="FECHA FINAL" HeaderText="FECHA FINAL"></asp:BoundColumn>
				<asp:BoundColumn DataField="CESANTIAS" HeaderText="CESANTIAS" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:BoundColumn DataField="INTERESES DE CESANTIA" HeaderText="INTERESES DE CESANTIA" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DIAS TRABAJADOS" HeaderText="DIAS TRABAJADOS"></asp:BoundColumn>
			</Columns>
		</asp:DataGrid></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P>Pagos del Mes 
<P></P>
<asp:datagrid id=DataGrid2 runat="server" AutoGenerateColumns="False" onPageIndexChanged="Grid_Change" AllowPaging="True">
		<HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
            <FooterStyle cssclass="footer"></FooterStyle>
            <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<Columns>
			<asp:BoundColumn DataField="CONCEPTO" HeaderText="CONCEPTO"></asp:BoundColumn>
			<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCION"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANT EVENTOS" HeaderText="CANT EVENTOS" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR EVENTO" HeaderText="VALOR EVENTO" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="A PAGAR" HeaderText="A PAGAR" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="A DESCONTAR" HeaderText="A DESCONTAR" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="TIPO EVENTO" HeaderText="TIPO EVENTO"></asp:BoundColumn>
			<asp:BoundColumn DataField="SALDO" HeaderText="SALDO" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="DOC REFERENCIA" HeaderText="DOC REFERENCIA"></asp:BoundColumn>
		</Columns>
	</asp:datagrid>
<P></P>
<P>
<asp:panel id=panelcesantias  runat="server" Visible="False" >
<TABLE class="filtersIn" >
  <TR>
    <TD>
      <P>LIQ. CESANTIAS </P></TD>
    <TD>LIQ. PRIMA </TD>
    <TD>LIQ. VACACIONES </TD></TR>
  <TR>
    <TD>
      <P>Número de días&nbsp; : 
<asp:Label id=LBDIASTRABAJADOS runat="server"></asp:Label></P></TD>
    <TD>Número de días: 
<asp:Label id=LBDTPRIMAS runat="server"></asp:Label></TD>
    <TD>Número de días: 
<asp:Label id=LBDTVACACIONES runat="server"></asp:Label></TD></TR>
  <TR>
    <TD>
      <P>Vlr. Cesantías a Pagar: 
<asp:Label id=LBCESAAPAGAR runat="server"></asp:Label></P></TD>
    <TD>Vlr.&nbsp;Prima a Pagar: 
<asp:Label id=LBPRIMAAPAGAR runat="server"></asp:Label></TD>
    <TD>Vlr.&nbsp;Vacaciones a Pagar: 
<asp:Label id=LBVACAAPAGAR runat="server"></asp:Label></TD></TR>
  <TR>
    <TD>
      <P>Vlr. Intereses a Pagar : 
<asp:Label id=LBINTAPAGAR runat="server"></asp:Label></P></TD>
    <TD>
      <P>Vlr. Novedades: 
<asp:Label id=LBDESCUENTONOVEDADES runat="server"></asp:Label></P></TD>
    <TD> </TD>
    <TD> </TD></TR></TABLE>
<P> </P>
<P> </P>
<TABLE>
  <TR>
  <TR>
    <TD>Indemnización: </TD>
    <TD>
<asp:Label id=LBINDEMNIZACION runat="server"></asp:Label></TD></TR>
  <TR>
    <TD>Descuentos: </TD>
    <TD>
<asp:Label id=LBDECUENTOSEMPLEA runat="server"></asp:Label></TD></TR>
  <TR>
    <TD>Valor Total:</TD>
    <TD><asp:Label id=LBVALORTOTAL runat="server"></asp:Label></TD>
  </TR>
       <tr><td>&nbsp;</td></tr>
       <tr><td>&nbsp;</td></tr>
       <tr><td>&nbsp;</td></tr>
       <tr><td>&nbsp;</td></tr>
      <TR><td>____________________________</td>
          <td></td>
          <td>____________________________</td>
      </TR>
      <tr><Td colspan="1">Nombre del Empleado </Td>
          <td></td>
          <Td colspan="1">Nombre del Pagador </Td>
      </tr>
      <tr><TD><asp:Label id=LBFIRMAEMPL runat="server" class="lpequeno"></asp:Label></TD>
           <td></td>
           <TD><asp:Label id=LBFIRMAPAGO runat="server" class="lpequeno"></asp:Label></TD>
      </tr>
      <tr><Td colspan="1">Identificación</Td><br />
          <TD></TD>
          <Td colspan="1">Identificación</Td>
      </tr>
     <tr><TD><asp:Label id=LBIDENTFIRMA runat="server" class="lpequeno"></asp:Label></TD>
          <td></td>
         <TD><asp:Label id=lbindentpaga runat="server" class="lpequeno"></asp:Label></TD>
     </TR> 
  </TR>
</TABLE></asp:panel>
</asp:placeholder>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P></P>
<P><asp:label id="LBPRUEBAS" runat="server" class="lmediano"></asp:label><asp:button id="BTNLIQUIDAR" onclick="realizar_liquidacion" runat="server" Text="LIQUIDAR" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')">
</asp:button></P>
<P><asp:label id="lbpag" runat="server" visible="False"></asp:label><asp:label id="prueba" runat="server" visible="False"></asp:label><asp:label id="lb3" runat="server" visible="False"></asp:label><asp:label id="lbmas1" runat="server" visible="False"></asp:label><asp:label id="lb2" runat="server" visible="False"></asp:label><asp:label id="lb" runat="server" visible="False"></asp:label><asp:label id="lbdocref" runat="server" visible="False"></asp:label></P>
<P>&nbsp;</P>
</fieldset>


<script language:javascript>
 function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
 }

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
                if (!msg || (msg='undefined')) { msg = 'Liquidando...'; } 
                btn.value = msg;
                // La magia verdadera :D
                btn.disabled = true;
            }
            return true;
        }
</script>