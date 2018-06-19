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
					A�O
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
	Si usted liquida DEFINITIVAMENTE las CESANT�AS, se grabar� el registro de esta liquidaci�n y grabar� el monto de los inter�ses como NOVEDAD para cada empleado en el PER�ODO de NOMINA VIGENTE !!!
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
            // Comprobamos si se est� haciendo una validaci�n
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se est� haciendo una validaci�n, volver si �sta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el bot�n sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Ser� el texto que muestre el bot�n mientras est� deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Procesando...'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
       </script>