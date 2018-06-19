<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.GestionVendedor.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_GestionVendedor" %>

<script type="text/javascript">
    function habilitar(obj)
    {
        if (obj.value.length > 0)
        {
            $('#' + '<%= btnConfirmar.ClientID %>').show("fast");
            $('#' + '<%= lbMensaje.ClientID %>').text("");
        }
        else
            $('#' + '<%= btnConfirmar.ClientID %>').hide("fast");
    }
    function ajustarFoco()
    {
        $('#' + '<%= txtPassword.ClientID %>').focus();
    }
    $(document).ready(function(){ 
        $(window).keydown(function (e) {
            if (e.keyCode == 13 && $('#<%=txtPassword.ClientID%>').is(':focus'))
            {
                e.preventDefault();
                $('#<%=btnConfirmar.ClientID%>').click();
            }
       }); 
    });
</script>

<fieldset id="fldValidasuario" runat="server" style="text-align:center">
    <table style="position: relative; width:400px">
        <tr>
            <%--<td style="font-size:large;">
                Vendedor:
            </td>--%>
            <td>
                <b><asp:Label ID="lbUsuario" runat="server" Font-Size="Large"></asp:Label></b>
            </td>
        </tr>
        <tr>
            <td style="font-size:large;">
                <b style="text-decoration: overline; font-size: smaller;">Vendedor: </b><br /> <asp:DropDownList ID="ddlVendedor" runat="server" cssClass="dmediano" AutoPostBack="false" onChange="ajustarFoco();"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <%--<td style="font-size:large;">
                Constraseña:
            </td>--%>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="tmediano" TextMode="Password" style="border: 5px; border-radius: 5px; height: 25px; margin: 5px; padding: 5px;" onkeyup="habilitar(this);" placeholder="Contraseña"></asp:TextBox>
            </td>
        </tr>
    </table><br />
    <asp:Label ID="lbMensaje" runat="server" style="color:red;display:block"></asp:Label>
    <asp:Button id="btnConfirmar" runat="server" Text="Confirmar" OnClick="confirmar" style="display:none;"/>
    <asp:PlaceHolder id="plhMain" runat="server" visible="false" >
        <table class="table-hover">
        <tr>
            <td>
                <asp:Button id="btnCotizacion" runat="server" Text="Cotizaciones" OnClick="cargarMenu"/>
            </td>
            <tr>
                <td>
                    <asp:Button id="btnSeguimiento" runat="server" Text="Seguimiento diario" OnClick="cargarMenu" />
                </td>
            </tr>
            
            <tr>
                <td>
                    <asp:Button id="btnCreaPedido" runat="server" Text="Crear pedidos" OnClick="cargarMenu"/>
                </td>
            </tr>
            
            <tr>
                <td>
                    <asp:Button id="btnModificaPedido" runat="server" Text="Modificar pedidos" OnClick="cargarMenu"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button id="btnDisponibilidad" runat="server" Text="Disponibilidad" OnClick="cargarMenu"/>
                </td>
            </tr>
            
            <tr>
                <td>
                    <asp:Button id="btnAsignaciones" runat="server" Text="Mis asignaciones" OnClick="cargarMenu"/>
                </td>
            </tr>
            
            <tr>
                
            </tr>
            <td>
                <asp:Button id="btnEntregaVehiculos" runat="server" Text="Entrega vehículos" OnClick="cargarMenu"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="brnFacturaProForma" runat="server" Text="Factura pro_forma" OnClick="cargarMenu"/>
            </td>
        </tr>
        <tr>
            <td style="padding:3px;">
                <br /><asp:Button id="btnTerminar" runat="server" Text="TERMINAR" style="text-decoration-color:white" OnClick="cargarMenu"/>
            </td>
        </tr>
    </table>
    </asp:PlaceHolder>
</fieldset>
