<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Asousados.BusquedaVehiculos.ascx.cs"
    Inherits="AMS.Asousados.BusquedaVehiculos" %>
<asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true" />
<script src="../js/jquery.js"></script>
<%--<script>

    $(document).on("ready", inicio);

    var Type;
    var Url;
    var Data;
    var ContentType;
    var DataType;
    var ProcessData;

    function inicio() {

       $("#<%=btnBusquedaInteligente.ClientID%>").on("click", busquedaInteligente);
    }

    function busquedaInteligente() {
        Type = "POST";
        Url = "http://ams.ecas.co/ASOUSADOS/WCFservices/Asousados.svc/BusquedaInteligente",
        Data = "{}";
        ContentType = "application/json; charset=utf-8";
        DataType = "json"; 
        varProcessData = true;
        CallService();
    }

    function CallService() {
        $.ajax({
            type: Type, //GET or POST or PUT or DELETE verb
            url: Url, // Location of the service
            data: Data, //Data sent to server
            contentType: ContentType, // content type sent to server
            dataType: DataType, //Expected data format from server
            processdata: ProcessData, //True or False
            success: function (msg) {//On Successfull service call
                $('#resutados').html(msg.d);
            },
            error: ServiceFailed// When Service call fails
        });
    }

    function ServiceFailed(result) {
        alert('Service call failed: ' + result.status + ' ' + result.statusText);
        Type = null;
        varUrl = null;
        Data = null;
        ContentType = null;
        DataType = null;
        ProcessData = null;
    }

    function busquedaInteligente2() {

        var txtBusquedaInteligente = document.getElementById("<%=txtBusquedaInteligente.ClientID%>");
        var strBusqueda = txtBusquedaInteligente.value;

        $.ajax({
            type: "POST",
            url: "http://localhost/WCFservices/Asousados.svc/BusquedaInteligente",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                // Insert the returned HTML into the <div>.
                $('#resutados').html(msg.d);
            
            }
        });
        
    }
</script>--%>


<table>
    <tr>
        <td>
            Búsqueda por Parámetros
        </td>
        <td>
            Búsqueda Inteligente
        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        Asociado
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAsociado" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Clase de Vehículo
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlClase" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddls_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Marca
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMarca" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddls_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Referencia Principal
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReferenciaPrincipal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddls_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Referencia Secundaria
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReferenciaSecundaria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddls_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Modelo
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlModelo" runat="server" />
                        <asp:TextBox ID="txtModelo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Carrocería
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCarroceria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddls_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Cilindraje
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCilindraje" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddls_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Tipo de Combustible
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoCombustible" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Color
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlColor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Caja
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCaja" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddls_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Placa
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPlaca" runat="server" AutoPostBack="true" OnSelectedIndexChanged="opcionPlaca" />
                        <asp:TextBox ID="txtPlaca" runat="server" maxlength = "1" min-value = "0" max-value = "9" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Tracción
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTraccion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddls_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Kilometraje
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlKilometraje" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Precio
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPrecio" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnBusquedaParametros" runat="server" Text="Buscar" OnClick="btnBusquedaParametros_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnLimpiarParametros" runat="server" Text="Limpiar Criterios de Busqueda" OnClick="btnLimpiarParametros_Click" />
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="txtBusquedaInteligente" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnBusquedaInteligente" runat="server" Text="Buscar" OnClick="btnBusquedaInteligente_Click" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
<br />
<div id="resutados"></div>
<asp:DataGrid ID="dgCarros" AutoGenerateColumns="true" runat="server"
	 ShowFooter="True" GridLines="Vertical" cssclass="datagrid">

	<FooterStyle CssClass="footer"></FooterStyle>
	<HeaderStyle CssClass="header"></HeaderStyle>
	<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
	<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
	<ItemStyle CssClass="item"></ItemStyle>
</asp:DataGrid>

<asp:Label ID="lblError" runat="server"></asp:Label>