<%@ Control Language="c#" codebehind="AMS.Automotriz.LiquidacionOrden.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.LiquidacionOrden" %>
<script type ="text/javascript">
function CambioFact(obFact,obSpan)
{
    var splitFact = obFact.value.split('-');
    obSpan.innerText = splitFact[1];
}
</script>

<fieldset>
<fieldset>
<legend>Torre de control</legend>
		<asp:PlaceHolder id="TorreControl" runat="server" Visible="true">
		<tr>
			<td>
				<p>
                <table id="Table" class="filtersIn">
                <tr>
                <td>
                <asp:label id="texto" runat="server">En este proceso se controla todas las operaciones
                    que realizan en el taller </asp:label>&nbsp;&nbsp;&nbsp;
					<asp:button id="ingresarTorre" onclick="Ingresar_Torre" runat="server" Text="Ingresar" Width="108px"></asp:button>
                    </td>
                    </tr>
                    </table>
                    </p>
			</td>
		</tr>
		</asp:PlaceHolder>
   </fieldset>
   <p>

   </p>
   <fieldset>
   <legend>Proceso Pre_Liquidación</legend>
		<asp:PlaceHolder id="LiquidacionOT" runat="server" Visible="true">
		<tr>
			
			<td>
            <p>
			<tr>
							<td>Orden de Trabajo&nbsp;:
								<asp:dropdownlist id="tipoDocumento1" class="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Documento1"></asp:dropdownlist></td>
							<td>Ordenes de Trabajo :
								<asp:dropdownlist id="ordenesPreliquidar" class="dpequeno" runat="server"></asp:dropdownlist></td>
						</tr>
						<tr>
							<td>
								<p><asp:button id="preliquidar" onclick="Preliquidar_Orden" runat="server" Text="Preliquidar" Width="185px"></asp:button></p>
							</td>
						</tr>
            </p>
	        </td>
	    </tr>
  </fieldset>

	
    	<tr>
		
					<td>

                    <p>
                                       
					<fieldset id="Table">
					<legend>Proceso Facturación</legend> 
							<tr>
                           
                            </tr>
                            <tr>
                            <br />
								<td>Orden de Trabajo :
                                <asp:dropdownlist id="tipoDocumento2" class="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Documento2"></asp:dropdownlist></td>
							    <td>Ordenes de Trabajo :
								<asp:dropdownlist id="ordenesLiquidar" class="dpequeno" runat="server"></asp:dropdownlist></td>
							<br />
								<td><asp:button id="liquidar" onclick="Liquidar_Orden"  runat="server" Text="Liquidar" Width="185px" CausesValidation="False" ></asp:button></td>
							</tr>
                    </fieldset>
                    </p>
                    <tr>
                    </tr>

                    <p>
                    <fieldset id="Table" >
                    <legend>Re_Impresión Facturas (Ultimo Año)</legend>
						<tr>
                           <td> <b>Reimpresión Facturas (Ultimo Año) </b><br />
                           Orden de Trabajo :
									<asp:dropdownlist id="ddlOTS" class="dgrande" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CambioOTLiqu"></asp:dropdownlist></td>
                                    </tr>
                                    <tr>
								<td>
									<p>Factura Relacionada :
										<asp:dropdownlist id="ddlFactRel" class="dgrande" runat="server"></asp:dropdownlist></p>
                                  
									<p>Cargo Relacionado :
										<asp:label id="lbCargo" class="lpequeno" runat="server" forecolor="RoyalBlue"></asp:label></p>
								</td>
							
								<td colspan="2"><asp:button id="btnReimpresion" onclick="ReimpresionFactura" runat="server" Text="Reimprimir Factura"></asp:button></td>
							</tr>
                            </fieldset>
                            </p>
                        </td>
		
		</tr>
	</asp:PlaceHolder>

	 
   
<p><asp:label id="lb" runat="server"></asp:label></p>
<p></p>
</fieldset>


