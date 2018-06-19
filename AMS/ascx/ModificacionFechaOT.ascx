<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.ModificacionFechaOT.ascx.cs" Inherits="AMS.Automotriz.ModificacionFechaOT" %>

<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type="text/javascript">
    function confirm()
    {
        var acepta = confirm('Esta seguro de guardar los cambios?');
        if(acepta == false)
        {
            return;
        }
    }

</script>

<fieldset>
    <asp:Label>OT:</asp:Label>&nbsp;&nbsp;
    <asp:DropDownList id="ddlTipoOT" runat="server" OnSelectedIndexChanged="cambioOT" AutoPostBack="true" CssClass="dmediano"></asp:DropDownList><br /><br />
    <asp:Label>Número:</asp:Label>&nbsp;&nbsp;
    <asp:DropDownList id="ddlNumeroOT" runat="server" CssClass="dmediano"></asp:DropDownList><br /><br /><br />
    <asp:Button ID="btnCargar" runat="server" OnClick="cargaFecha" Text="Cargar"/>
</fieldset>
<br />
<br />

<fieldset id="fldFecha" runat="server" visible="false">
    <table>
        <tr>
            <td>
                <asp:Label>Fecha Entrega Actual</asp:Label>
            </td>
            <td>
                <asp:Label>Hora Entrega Actual</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtFechaActualOT"  runat="server" onKeyUp="DateMask(this)" Enabled="false"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtHoraActuaLOT" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Button id="btnEditar" runat="server" Text="Editar Fecha y Hora" OnClick="habilitaTxt"/><br /><br />
    <table id="tablaNueva" runat="server" visible="false">
        <tr>
            <td>
                <asp:Label>Nueva Fecha Entrega</asp:Label>
            </td>
            <td>
                <asp:Label>Nueva Hora Entrega</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtFechaNuevaOT"  runat="server" onKeyUp="DateMask(this)"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtHoraNuevaOT" runat="server" ></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Button id="btnGuardar" runat="server" Text="Guardar" OnClick="guardarDatos" Visible="false"/>
</fieldset>