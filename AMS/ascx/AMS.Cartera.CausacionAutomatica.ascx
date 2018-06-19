<%@ Control Language="c#" codebehind="AMS.Finanzas.Cartera.CausacionAutomatica.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Cartera.CausacionAutomatica" %>
<p>
</p>
<p>
</p>
<p>
<FIELDSET>
	<asp:Panel id="pnlControles" runat="server" Visible="False">
		<table id="Table1" class="filtersIn">
        <tbody>
			<TR>
				<TD>Escoja el mes a facturar :
				</TD>
				<TD>
					<asp:DropDownList id="ddlMes" runat="server"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>Digite el detalle de las facturas :
				</TD>
				<TD>
					<asp:TextBox id="tbDetalle" runat="server" TextMode="MultiLine" MaxLength="120"></asp:TextBox>
					<asp:RequiredFieldValidator id="rfv1" Visible="True" runat="server" ErrorMessage="*" ControlToValidate="tbDetalle"></asp:RequiredFieldValidator></TD>
			</TR>
			<TR>
				<TD>Escoja el prefijo para la facturas :
				</TD>
				<TD>
					<asp:DropDownList id="ddlPrefijo" runat="server"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>Escoja el almacen :
				</TD>
				<TD>
					<asp:DropDownList id="ddlAlmacen" runat="server"></asp:DropDownList></TD>
			</TR>
		
		
 </tbody>
 </TABLE>
 <P>
			<asp:Button id="btnGenerar" onclick="btnGenerar_Click" runat="server" Text="Generar Factura" UseSubmitBehavior="false" 
 OnClientClick="clickOnce(this, 'Cargando...')"></asp:Button></P>
	</asp:Panel>
    </FIELDSET>
<P></P>
<p>
</p>
<p>
</p>
<p>
</p>
</p>
<fieldset>
<table>
<p>
	<asp:DataGrid id="dgConceptos" runat="server" cssclass="datagrid" PageSize="10" CellPadding="3"
		AutoGenerateColumns="False">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="NIT" HeaderText="Nit"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" HeaderText="Valor a Causar" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="PORCIVA" HeaderText="Porcentaje de IVA"></asp:BoundColumn>
			<asp:BoundColumn DataField="DETALLE" HeaderText="Detalle"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>
</p>
</table>
</fieldset>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>


<script type ="text/javascript">
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
                if (!msg || (msg='undefined')) { msg = 'Procesando..'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
        </script>