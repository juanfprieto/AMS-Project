<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.OrdenTrabajo.Cancelacion.ascx.cs" Inherits="AMS.Automotriz.OrdenTrabajo.Cancelacion" %>
<script type ="text/javascript">
function CambioFact(obFact,obSpan)
{
    var splitFact = obFact.value.split('-');
    obSpan.innerText = splitFact[1];
}
</script>
<fieldset>
    <table class="filtersIn"> 
        <tr>
		    <td>
                Orden de Trabajo:
                <asp:dropdownlist id="tipoDocumento1" class="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Documento1"></asp:dropdownlist>
            </td>
		    <td>
                Ordenes de Trabajo:
		        <asp:dropdownlist id="ordenesPreliquidar" class="dpequeno"  runat="server"></asp:dropdownlist>
            </td>
	    </tr>
        <tr>
            <td colspan="2" style="vertical-align: top;">
                Observación:                
                <asp:TextBox id="textObser" runat="server" TextMode="MultiLine" style="Width:40%; Height:100px; margin-left:35px" ></asp:TextBox>
            </td>
        </tr>
	    <tr>
		    <td>
			    <p>
                    <asp:button id="CancelarOperacion"  OnClick="cancelarOperacion" runat="server" Text="Cancelar Orden de Trabajo" Width="185px"></asp:button>
                </p>
		    </td>
	    </tr>
    </table>
</fieldset>
<p><asp:label id="lb" runat="server"></asp:label></p>