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
                    <legend class="Legends">Escoja el Tipo de Proceso que desea Ejecutar</legend>
                    <asp:RadioButtonList ID="tipoProcesoAsignar" CellSpacing="6" RepeatDirection="Horizontal"
                        BorderStyle="None" runat="server">
                        <asp:ListItem Value="A" Selected="True">Asignaci&#243;n</asp:ListItem>
                        <asp:ListItem Value="D">Desasignaci&#243;n</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:Button ID="ingresarAsignacionDesasignacion" OnClick="Ingresar_Proceso" runat="server"
                        Text="Ingresar" UseSubmitBehavior="false" >
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

