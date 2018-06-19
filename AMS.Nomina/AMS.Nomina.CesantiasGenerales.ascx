<%@ Control Language="c#" codebehind="AMS.Nomina.CesantiasGenerales.cs" autoeventwireup="false" Inherits="AMS.Nomina.CesantiasGenerales" %>
<fieldset>
<h4>PRELIQUIDACION GENERAL
</h4>
<p>
	Ingrese los datos Solicitados
</p>
<p>

	<table id="Table" class="filtersIn">
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
					<asp:Label id="LBMESINICIAL" runat="server" class="lpequeno">ENERO</asp:Label></td>
			</tr>
			<tr>
				<td>
					Mes Final
				</td>
				<td>
					<asp:Label id="LBMESFINAL" runat="server" class="lpequeno">DICIEMBRE</asp:Label></td>
			</tr>
		</tbody>
	</table>
</p>
<p>
	Cesantias e Intereses de Cesantia&nbsp;Periodo Seleccionado
	<asp:Label id="lbfechainicio" runat="server"></asp:Label>&nbsp;<asp:Label id="lbfechafinal" runat="server"></asp:Label>&nbsp;
</p>
<p>
	&nbsp;<asp:DataGrid id="DATAGRIDCESANTIAS" CssClass="datagrid" runat="server" AutoGenerateColumns="False">
		<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<Columns>
			<asp:BoundColumn DataField="CODIGO EMPLEADO" HeaderText="CODIGO EMPLEADO"></asp:BoundColumn>
			<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE"></asp:BoundColumn>
			<asp:BoundColumn DataField="CESANTIAS" HeaderText="CESANTIAS" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="INTERESES DE CESANTIA" HeaderText="INTERESES DE CESANTIA" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="DIAS TRABAJADOS" HeaderText="DIAS TRABAJADOS"></asp:BoundColumn>
			<asp:BoundColumn DataField="SUELDO PROMEDIO" HeaderText="SUELDO PROMEDIO" DataFormatString="{0:C}"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	Si usted liquida DEFINITIVAMENTE las CESANTÍAS, se grabará el registro de esta liquidación y grabará el monto de los interéses como NOVEDAD para cada empleado en el PERÍODO de NOMINA VIGENTE !!!
</p>
<p>
	<asp:Button id="BTNLIQUIDAR" onclick="LiquidacionCesantiasGenerales" runat="server" Text="Liquidar" UseSubmitBehavior="false" 
	OnClientClick="clickOnce(this, 'Cargando...')" >
	</asp:Button>

    <table>
    <tr>
        <td>
            <td>Generar Excel
            <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel"  OnClick="ImprimirExcelGrid" Visible="False" runat="server"
             alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" Width="40px">
              </asp:ImageButton>
        </td>    
    
    
    </tr>
</table>
    
	<asp:Button id="BTNLIQUIDARDEFINITIVAMENTE" onclick="grabarcesantiasgenerales" runat="server"
		Text="LIQUIDAR DEFINITIVAMENTE" Visible="False" UseSubmitBehavior="false" 
	OnClientClick="clickOnce(this, 'Cargando...')">
		</asp:Button>
</p>

</fieldset>




<script language:javascript>
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
                if (!msg || (msg='undefined')) { msg = 'Procesando...'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
       </script>