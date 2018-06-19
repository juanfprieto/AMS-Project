<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="AMS.Tools.ReImpresionDocumentos.ascx.cs"
    Inherits="AMS.Tools.ReImpresionDocumentos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<script type ="text/javascript" src="../js/AMS.Vehiculos.Tools.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
<script type="text/javascript">
    
    function crgPrefijoDoc() {
        var prfDoc = document.getElementById("_ctl1_ddlPrefijo");
        ModalDialog(prfDoc, 'SELECT pdoc_codigo,pdoc_codigo CONCAT \' - \' CONCAT pdoc_nombre FROM  pdocumento WHERE sfor_codigo IS NOT NULL ORDER BY pdoc_codigo', new Array());
    }
    function clickOnce(btn, msg) {
        // Comprobamos si se est� haciendo una validaci�n
        if (typeof (Page_ClientValidate) == 'function') {
            // Si se est� haciendo una validaci�n, volver si �sta da resultado false
            if (Page_ClientValidate() == false) { return false; }
        }
        // Asegurarse de que el bot�n sea del tipo button, nunca del tipo submit
        if (btn.getAttribute('type') == 'button') {
            // El atributo msg es totalmente opcional. 
            // Ser� el texto que muestre el bot�n mientras est� deshabilitado
            if (!msg || (msg = 'undefined')) { msg = 'Procesando..'; }
            btn.value = msg;
            // La magia verdadera :D
            btn.disabled = true;
        }

        return true;
    }
</script>


    
<fieldset id="Fieldset1" runat="server" >
    <legend>Re-impresi�n de Documentos</legend>
    <table class="filtersIn">
        <tr>
            <td colspan="2">
                <p>
                    Aqu� usted podra reimprimir un documento generado con anterioridad, tenga en cuenta
                    que dicho documento debe tener un formato asociado por la opci�n&nbsp;de Configuraci�n - Par�metros generales
                    - Documentos, Actividades o Hechos Econ�micos. <i>No incluye pedidos a items de inventario.</i>
                </p>
            </td>
        </tr>
        <tr>
            <td>
                Prefijo del Documento :
            </td>
            <td>
                <asp:DropDownList ID="ddlPrefijo" Runat="server" Height="25px">
                </asp:DropDownList>
            </td>
            <td runat="server"><asp:Image id="imglupita" runat="server" ImageUrl="../img/AMS.Search.png" onClick ="crgPrefijoDoc();"></asp:Image></td>
        </tr>
        <tr>
            <td>
                N�mero del Documento :
            </td>
            <td>
                <asp:TextBox id="tbnumero" runat="server" class="tpequeno">
                </asp:TextBox>
                <asp:RequiredFieldValidator id="rfv1" runat="server" ErrorMessage="Campo Obligatorio."
                    ControlToValidate="tbnumero" Display="Dynamic">
                </asp:RequiredFieldValidator>&nbsp;
                <asp:RegularExpressionValidator id="rev1" runat="server" ErrorMessage="Valor Num�rico Requerido."
                    ControlToValidate="tbnumero" Display="Dynamic" ValidationExpression="\d+">
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAceptar" Runat="server" Text="ReImprimir" onclick="btnAceptar_Click"
                    UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')"></asp:Button>
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="Fieldset2" runat="server" >
    <legend>Re-impresi�n a pedidos de items Inventario</legend>
    <table class="filtersIn">
        <tr>
            <td colspan="2">
                <p>
                    Aqu� usted podra reimprimir un documento generado con anterioridad que corresponda exclusivamente
                    a pedidos de item de inventario, dicho documento debe tener un formato asociado por la opci�n de
                     Configuraci�n - par�metros de inventario - tipos de pedido.
                </p>
            </td>
        </tr>
        
        <tr>
            <td>
                ReImpresi�n de Pedidos de Inventario
            </td>
            <td>
                <asp:RadioButtonList id="tipoPedido" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="True" onselectedindexchanged="tipoPedido_SelectedIndexChanged">
                    <asp:ListItem Value="C" Selected="True">Cliente</asp:ListItem>
                    <asp:ListItem Value="P">Proveedor</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                Prefijo del Pedido :
            </td>
            <td>
                <asp:DropDownList ID="ddlPrefPed" Runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefPed_SelectedIndexChanged" Height="25px">
                </asp:DropDownList>
            </td>
            <td id="Td1" runat="server"></td>
        </tr>
        <tr>
            <td>
                N�mero del Pedido :
            </td>
            <td>
                <asp:DropDownList ID="ddlNumPed" class="dpequeno" Runat="server" Height="20px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button CausesValidation="false" ID="btnAceptar2" Runat="server" Text="ReImprimir Pedido"
                        onclick="btnAceptar2_Click" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')">
                </asp:Button>
            </td>
        </tr>
    </table>
</fieldset>
    <asp:Label id="lb" runat="server"></asp:Label>


