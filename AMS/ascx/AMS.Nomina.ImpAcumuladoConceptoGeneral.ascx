<%@ Control Language="c#" codebehind="AMS.Nomina.ImpAcumuladoConceptoGeneral.cs" autoeventwireup="false" Inherits="AMS.Nomina.ImpAcumuladoConceptoGeneral" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<script runat="server">

    // Insert user control code here
    //

</script>
<p>
    Porfavor Seleccione&nbsp;las opciones&nbsp;para generar el acumulado correspondiente. 
</p>
<p>
    <table style="BACKGROUND-COLOR: white">
        <tbody>
            <tr>
                <td>
                    Año:</td>
                <td>
                    <asp:DropDownList id="DDLANO" class="dmediano" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>
    <!-- Insert content here -->
    <asp:Button id="BTNMOSTRAR" onclick="btnmostrar" runat="server" Text="MOSTRAR REPORTE"></asp:Button>
</p>
<p>
    <CR:CrystalReportViewer id="visor" runat="server" visible="false"></CR:CrystalReportViewer>
</p>