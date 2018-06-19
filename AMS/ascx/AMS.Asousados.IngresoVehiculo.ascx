<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Asousados.IngresoVehiculo.ascx.cs" Inherits="AMS.Asousados.IngresoVehiculo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>

<script src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script type="text/javascript">
    function mayusculaEnter(e, obj) {
        if (e.keyCode == 13) {
            aMayusculas(obj);
        }   
    }
</script>
<asp:ScriptManager ID="scriptManager1" runat="server" ></asp:ScriptManager>

<div>
    <table>
        <tr>
            <td>
                Placa:
            </td>
            <td>
                <asp:TextBox ID="txtPlaca" runat="server" onblur="aMayusculas(this);" onkeydown="mayusculaEnter(event, this);" OnTextChanged="txtPlaca_OnTextChanged" AutoPostBack="true" />  
            </td>
        </tr>
    </table>
</div>
<div>
<table class="tablewhite3">
    <tr>
        <td>
            Clase de Vehículo:
        </td>
        <td>
            <asp:DropDownList ID="ddlClaseVeh" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClaseVeh_OnSelectedIndexChanged" class="dmediano"/>
        </td>
        <asp:PlaceHolder ID="phMarca" runat="server" Visible="false">
        <td>
            Marca:
        </td>
        <td>
            <asp:DropDownList ID="ddlMarca" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMarca_OnSelectedIndexChanged" class="dmediano"/>
        </td>
        </asp:PlaceHolder>
    </tr>
    <tr>
        <asp:PlaceHolder ID="phRefPrincipal" runat="server" Visible="false">
        <td>
            Referencia Principal:
        </td>
        <td>
            <asp:DropDownList ID="ddlRefPrincipal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRefPrincipal_OnSelectedIndexChanged" />
        </td>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phRefComplementaria" runat="server" Visible="false">
        <td>
            Referencia Complementaria:
        </td>
        <td>
            <asp:DropDownList ID="ddlRefComplementaria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRefComplementaria_OnSelectedIndexChanged" />
        </td>
        </asp:PlaceHolder>
    </tr>
    <tr>
        <asp:PlaceHolder ID="phCarroceria" runat="server" Visible="false">
        <td>
            Carrocería:
        </td>
        <td>
            <asp:DropDownList ID="ddlCarroceria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCarroceria_OnSelectedIndexChanged" />
        </td>
        </asp:PlaceHolder>
    </tr>
    <asp:PlaceHolder ID="phDatosCatalogo" runat="server" Visible="false">
    <tr>
        <td>
            Cilindraje:
        </td>
        <td>
            <asp:DropDownList ID="ddlCilindraje" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCilindraje_OnSelectedIndexChanged" />
        </td>
        <td>
            Tipo de Combustible:
        </td>
        <td>
            <asp:DropDownList ID="ddlTipoCombustible" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCombustible_OnSelectedIndexChanged" />
        </td>
    </tr>
    <tr>
        <td>
            Aspiración del Motor:
        </td>
        <td>
            <asp:DropDownList ID="ddlAspiracion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAspiracion_OnSelectedIndexChanged" />
        </td>
        <td>
            Tracción:
        </td>
        <td>
            <asp:DropDownList ID="ddlTraccion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTraccion_OnSelectedIndexChanged" />
        </td>
    </tr>
    <tr>
        <td>
            Caja de Cambios:
        </td>
        <td>
            <asp:DropDownList ID="ddlCaja" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DatosCatalogo_OnSelectedIndexChanged" />
        </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phCatalogo" runat="server" Visible="false">
    <tr>
        <td>
            Catálogo:
        </td>
        <td colspan="3">
            <asp:DropDownList ID="ddlCatalogo" runat="server" />
        </td>
    </tr>
    </asp:PlaceHolder>
</table>
</div>
<br /><br /><br />

    
    <asp:PlaceHolder ID="phOferente" runat="server" Visible="false">
    <div>
<table class="tablewhite3">
    <tr>
        <td>
            Nombre:
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtOferenteNombre" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Teléfono:
        </td>
        <td>
            <asp:TextBox ID="txtOferenteTelefono" runat="server" />
        </td>
        <td>
            Celular:
        </td>
        <td>
            <asp:TextBox ID="txtOferenteCelular" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            e-mail:
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtOferenteMail" runat="server" onblur="aMinusculas(this);" />
        </td>
    </tr>
    </table>
    </div>
    <br /><br /><br />
    </asp:PlaceHolder>

<asp:PlaceHolder ID="phDatosVehiculo" runat="server" Visible="false">
<table>
    <asp:PlaceHolder ID="phAsociado" runat="server">
    <tr>
        <td>
            Asociado:
        </td>
        <td>
            <asp:DropDownList ID="ddlAsociado" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAsociado_OnSelectedIndexChanged" />
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td>
            Ciudad de la Placa:
        </td>
        <td>
            <asp:DropDownList ID="ddlCiudad" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Color:
        </td>
        <td>
            <asp:DropDownList ID="ddlColor" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Año Modelo:
        </td>
        <td>
            <asp:DropDownList ID="ddlModelo" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Servicio:
        </td>
        <td>
            <asp:DropDownList ID="ddlServicio" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Kilometraje:
        </td>
        <td>
            <asp:TextBox ID="txtKilometraje" runat="server" onkeyup="NumericMaskE(this,event)" />
        </td>
    </tr>
    <tr>
        <td>
            Precio de Venta:
        </td>
        <td>
            <asp:TextBox ID="txtPrecio" runat="server" onkeyup="NumericMaskE(this,event)" />
        </td>
    </tr>
    <asp:PlaceHolder ID="phOpcionesNoOferente" runat="server">
    <tr>
        <td>
            Propiedad del Vehículo:
        </td>
        <td>
            <asp:DropDownList ID="ddlPropiedad" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Ubicación:
        </td>
        <td>
            <asp:DropDownList ID="ddlUbicacion" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Estado del Vehículo:
        </td>
        <td>
            <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_OnSelectedIndexChanged" />
        </td>
    </tr>
    <tr>
        <td>
            Fecha de Ingreso del Vehículo:
        </td>
        <td>
            <asp:TextBox ID="txtFechaIngreso" runat="server" />
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaIngreso" Format="yyyy-MM-dd"/>
        </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phVenta" runat="server" >
    <tr>
        <td>
            Fecha de Venta del Vehículo:
        </td>
        <td>
            <asp:TextBox ID="txtFechaVenta" runat="server" />
            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaVenta" Format="yyyy-MM-dd"/>
        </td>
    </tr>
    <tr>
        <td>
            Número de Factura:
        </td>
        <td>
            <asp:TextBox ID="txtNumFactura" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Valor:
        </td>
        <td>
            <asp:TextBox ID="txtValorVenta" runat="server"  onkeyup="NumericMaskE(this,event)"/>
        </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phRetirado" runat="server">
    <tr>
        <td>
            Fecha de Retiro:
        </td>
        <td>
            <asp:TextBox ID="txtFechaRetiro" runat="server" />
            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFechaRetiro" Format="yyyy-MM-dd"/>
        </td>
    </tr>
    <tr>
        <td>
            Motivo:
        </td>
        <td>
            <asp:TextBox ID="txtMotivoRetiro" runat="server" TextMode="MultiLine" />
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td>
            Información Adicional:
        </td>
        <td>
            <asp:TextBox ID="txtInfoAdicional" runat="server" TextMode="MultiLine" Width="600" Height="100" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:PlaceHolder ID="phFotos" runat="server"></asp:PlaceHolder>
        </td>
    </tr>
    <tr>
        <td>
          <div ID="LbAgregar" runat="server">  Agregar Fotos:</div>
        </td>
        <td>
            <asp:FileUpload ID="upFotos" runat="server" />
            <asp:Button ID="btnUpload" runat="server" Text="Cargar" OnClick="btnUpload_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnInsertar" runat="server" Text="Guardar Ingreso" OnClick="btnInsertar_Click" />
            <asp:Button ID="btnEditar" runat="server" Text="Guardar Edición" OnClick="btnEditar_Click" Visible="false" />
        </td>
    </tr>
</table>
            <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" Visible="false" />
</asp:PlaceHolder>
<asp:Label ID="lblError" runat="server" />