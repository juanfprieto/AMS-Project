<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenesTaller.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.OrdenesTaller" %>
<link rel="stylesheet" href="../css/tabber.css" type="text/css" media="screen"/>
<script type ="text/javascript" src="../js/tabberOT.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>

<script type="text/javascript" language="javascript">
      // prueba.className = 
</script>

<asp:ValidationSummary ID="ValidationSummary2" runat="server" />
 <fieldset>
    <asp:Label ID="lbBienvenida" runat="server" Font-Size="13px" > <strong>*Ingresar información  pestaña por pestaña y haciendo click en botón Validar de la pestaña respectiva. Finalmente click en Guardar Orden de Trabajo.<br /></strong>
    </asp:Label> <br />
    <div class="contenedor" runat ="server">
        <div class="tabber" id="cabezera" runat ="server">
            <div id="prueba" class="tabbertab" title="Datos de la orden" runat="server" >
                <asp:PlaceHolder id="datosOrigen" runat="server" Visible="true"></asp:PlaceHolder>     
            </div>
            <div id="prueba1" class="tabbertab" title="Datos propietario" runat="server" >
                <asp:PlaceHolder id="datosPropietario" runat="server" Visible="true" ></asp:PlaceHolder>
            </div>
            <div id="prueba2" class="tabbertab" title="Datos Vehiculo" runat="server">
                <asp:PlaceHolder id="datosVehiculo" runat="server" Visible="true"></asp:PlaceHolder>
            </div>
            <div id="prueba3" class="tabbertab" title="Otros Datos" runat="server" >
                <asp:PlaceHolder id="otrosDatos" runat="server" Visible="true"></asp:PlaceHolder>
            </div>
            <div id="prueba4" class="tabbertab" title="Kits o Combos" runat="server" >
                <asp:PlaceHolder id="kitsCombos" runat="server" Visible="true"></asp:PlaceHolder>
            </div>
            <div id="prueba5" class="tabbertab" title="Peritaje" runat="server" >
                <asp:PlaceHolder id="operacionesPeritaje" runat="server" Visible="false"></asp:PlaceHolder>
            </div>
            <div id="prueba6" class="tabbertab" title="Modificar Ordenes" runat="server">
                <asp:PlaceHolder id="estadoOrdenes" runat="server" Visible="true"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    
</fieldset>



<%--<fieldset style="visibility:hidden">
    <asp:ImageButton id="origen" onclick="Cargar_DatosOrden" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False" ></asp:ImageButton>
    <asp:ImageButton id="propietario" onclick="Cargar_DatosPropietario" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
    <asp:ImageButton id="vehiculo" onclick="Cargar_DatosVehiculo" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
    <asp:ImageButton id="otros" onclick="Cargar_OtrosDatos" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
    <asp:ImageButton id="botonKits" onclick="Cargar_KitsCombos" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
    <asp:ImageButton id="opPeritaje" onclick="Cargar_Operaciones_Peritaje" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
    <asp:ImageButton id="estOrdenes" onclick="Cargar_EstadoOrdenes" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
</fieldset>--%>


<%--<fieldset style="visibility:hidden">
    <p>
        <table id="Table1" class="filtersIn">
            <tbody>
                <tr>
                    <td>
                        <asp:Label id="Label1" runat="server"><b>Datos de la Orden</b></asp:Label>
                    </td>
                    <td align="center">
                        <asp:ImageButton id="origen" onclick="Cargar_DatosOrden" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False" ></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:PlaceHolder id="datosOrigen" runat="server" Visible="False"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr> 
                    <td>
                        <asp:Label id="Label2" runat="server"><b>Datos Propietario</b></asp:Label>
                    </td>
                    <td align="center">
                        <asp:ImageButton id="propietario" onclick="Cargar_DatosPropietario" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:PlaceHolder id="datosPropietario" runat="server" Visible="False"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label id="Label3" runat="server"><b>Datos Vehiculo</b></asp:Label>
                    </td>
                    <td align="center">
                        <asp:ImageButton id="vehiculo" onclick="Cargar_DatosVehiculo" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:PlaceHolder id="datosVehiculo" runat="server" Visible="False"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label id="Label4" runat="server"><b>Otros Datos</b></asp:Label>
                    </td>
                    <td align="center">
                        <asp:ImageButton id="otros" onclick="Cargar_OtrosDatos" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:PlaceHolder id="otrosDatos" runat="server" Visible="False"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label id="Label5" runat="server"><b>Kits o Combos</b></asp:Label>
                    </td>
                    <td align="center">
                        <asp:ImageButton id="botonKits" onclick="Cargar_KitsCombos" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:PlaceHolder id="kitsCombos" runat="server" Visible="False"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label id="Label7" runat="server"><b>Peritaje</b></asp:Label>
                    </td>
                    <td align="center">
                        <asp:ImageButton id="opPeritaje" onclick="Cargar_Operaciones_Peritaje" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:PlaceHolder id="operacionesPeritaje" runat="server" Visible="False"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label id="Label6" runat="server"><b>Estado de las Ordenes</b></asp:Label>
                    </td>
                    <td align="center">
                        <asp:ImageButton id="estOrdenes" onclick="Cargar_EstadoOrdenes" runat="server" ImageUrl="../img/AMS.BotonExpandir.png"
                        CausesValidation="False"></asp:ImageButton>
                    </td>
                </tr>
                    <td colspan="2">
                       <asp:PlaceHolder id="estadoOrdenes" runat="server" Visible="False"></asp:PlaceHolder>
                    </td>
                </tr>
            </tbody>
        </table>
    </p>
</fieldset>--%>
    <p></p>

    <p style='text-align: center;'>
        <asp:Button id="grabar" onclick="Grabar_Orden" runat="server" Text="Grabar Orden de Trabajo"
        Enabled="false" style="margin-left:auto; margin-right:auto"></asp:Button>&nbsp;
        <%--<asp:Button id="grabar2" runat="server" OnClick="grabar_Orden2" Enabled="true" Text="Grabar Orden de Trabajo 2" ></asp:Button>&nbsp;--%>
        <asp:Button id="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="False"></asp:Button>
    </p>

    <p>
        <asp:Label id="lb" runat="server"></asp:Label>
    </p>

    <asp:HiddenField ID="hdTabIndex" runat="server" />
<div id="autorizar" runat="server"  visible="false" class="divHabeas">
    <asp:PlaceHolder id="plcAutorizar" runat="server" Visible="true"></asp:PlaceHolder>
</div>

<script language = "javascript" type="text/javascript">
    $(function () {
        var divAutorizar = "<%=autorizar.ClientID%>";
        $("#" + divAutorizar).draggable();
    });
    
    
</script>
