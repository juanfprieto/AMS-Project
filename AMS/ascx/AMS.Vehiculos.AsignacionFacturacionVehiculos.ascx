<%@ Control Language="c#" CodeBehind="AMS.Vehiculos.AsignacionFacturacionVehiculos.ascx.cs"
    AutoEventWireup="True" Inherits="AMS.Vehiculos.AsignacionFacturacionVehiculos" %>

<fieldset id="fldAsignacion" runat="server">
    <table class="filters">
        <tbody>
            <tr>
                <th class="filterHead">
                    <img height="75" src="../img/AMS.Flyers.Asignar.png" border="0">
                </th>
                <td>
                    <legend class="Legends">Escoja el Tipo de Proceso que Desea Ejecutar</legend>
                    <asp:RadioButtonList ID="tipoProcesoAsignar" CellSpacing="6" RepeatDirection="Horizontal"
                        BorderStyle="None" runat="server">
                        <asp:ListItem Value="A" Selected="True">Asignaci&#243;n</asp:ListItem>
                        <asp:ListItem Value="D">Desasignaci&#243;n</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:Button ID="ingresarAsignacionDesasignacion" OnClick="Ingresar_Proceso" runat="server"
                        Text="Ingresar" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')">
                    </asp:Button>
                </td>
            </tr>
        </tbody>
    </table>
</fieldset>
<fieldset id="fldFacturacion" runat="server">
    <table>
    <tbody>
        <tr>
                <th class="filterHead">
                    <img height="70" src="../img/AMS.Flyers.Liquidar.png" border="0">
                </th>
                <td>
                    <legend class="Legends">Proceso Liquidación</legend>
                    <table>
                        <tbody>
                            <tr>
                                <td width="50%">
                                    Prefijo del Pedido :
                                    <asp:DropDownList ID="prefijoPedido" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Tipo_Documento">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Número del Pedido :<br>
                                    <asp:DropDownList ID="numeroPedido" class="dpequeno" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="botonFacturar" OnClick="Facturar_Pedido" runat="server" Text="Facturar"
                                        UseSubmitBehavior="false"></asp:Button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
    </tbody>
    </table>
</fieldset>
<p>
    <asp:Label ID="lb" runat="server"></asp:Label>
</p>

<script language:javascript>
    function clickOnce(btn, msg) {
        // Comprobamos si se está haciendo una validación
        if (typeof (Page_ClientValidate) == 'function') {
            // Si se está haciendo una validación, volver si ésta da resultado false
            if (Page_ClientValidate() == false) { return false; }
        }

        // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
        if (btn.getAttribute('type') == 'button') {
            // El atributo msg es totalmente opcional. 
            // Será el texto que muestre el botón mientras esté deshabilitado
            if (!msg || (msg = 'undefined')) { msg = 'Procesando..'; }

            btn.value = msg;

            // La magia verdadera :D
            btn.disabled = true;
        }

        return true;
    }
</script>
