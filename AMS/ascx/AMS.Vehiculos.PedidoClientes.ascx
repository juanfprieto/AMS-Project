<%@ Control Language="c#" codebehind="AMS.Vehiculos.PedidoClientes.ascx.cs" autoeventwireup="True" Inherits="AMS.Vehiculos.PedidoClientes" %>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
<script type="text/javascript">

var url = document.location.href;
var getString = url.split('?')[1];
var GET = getString.split('&');
var get = {};//this object will be filled with the key-value pairs and returned.

for (var i = 0, l = GET.length; i < l; i++) {
    var tmp = GET[i].split('=');
    get[tmp[0]] = unescape(decodeURI(tmp[1]));
}
if (get.nitCli != '' && get.nitCli != undefined)
{
    Consultar_Cliente(get.nitCli)
}
function Consultar_Cliente(obj) {
    PedidoClientes.Consultar_Cliente(obj, Consultar_Cliente_CallBack);
}
function Consultar_Cliente_CallBack(response)
{
    var Nit = response.value;
    if (Nit == "")
    {
        ModalDialog(this, '*MCLIENTE' + '*' + get.nitCli, new Array());
    }
}
   
</script>
<fieldset>
        <legend> 
            <legend>Se realizará la modificación de un pedido:</legend>       
       </legend>

       <asp:placeholder id="plcVendedor" runat="server" Visible="true">
           <table class="filtersIn">
                <tr>
                    <td>
                        <asp:label id="lVendedorAutenticacion" runat="server">Vendedor:</asp:label>			
                    </td>
                    <td>
                        <asp:dropdownlist id="ddlVendedorAutenticacion" runat="server" class="dmediano"></asp:dropdownlist>
                    </td>
                </tr>
                <tr> 
                    <td>
                        <asp:label id="lContrasena" runat="server">Contraseña: </asp:label>			
                    </td>
                    <td>
                        <asp:textbox id="txtContrasena" runat="server" TextMode="Password" class="dmediano"></></asp:textbox>
                    </td> 
                </tr>
                <tr>   
                    <td>
                        <asp:button id="Button1" onclick="validaDatos" runat="server" Text="Cargar Pedidos"></asp:button>
                    </td>            
                </tr>
            </table>
        </asp:placeholder>
        <asp:PlaceHolder ID="plhDatosVeh" runat="server" Visible="false">
            <table class="filtersIn">
            <tr>
                <td>
                    Prefijo del Pedido:
                </td>
                <td>
                    <asp:dropdownlist id="prefijoDocumento" class="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Prefijo"></asp:dropdownlist>
                </td>        
            </tr>
            <tr>
                <td>
                    Número de Pedido:
                </td>
                <td>
                    <asp:dropdownlist id="numeroPedido" runat="server" class="dpequeno"></asp:dropdownlist> <asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image>        
                </td>
            </tr>
            <tr>         
                <td>
                    <asp:button id="modificarPedido" onclick="Ingresar_Modificar" runat="server" Text="Modificar" Width="110px"></asp:button>               
                    <asp:button id="btnAbonos" onclick="ConsultarAbonos" runat="server" Text="Consultar Abonos"></asp:button>
                <!--	<asp:button id="btnCancelar" onclick="CancelarPedido" Text="Cancelar Pedido" Runat="server"></asp:button></p> -->
                </td>       
            </tr>  
        </table>
        </asp:PlaceHolder>
          
</fieldset>			


<div id="autorizar" runat="server"  visible="false" class="divHabeas">
    <asp:PlaceHolder id="plcAutorizar" runat="server" Visible="true"></asp:PlaceHolder>
</div>
<p>
	<asp:label id="lb" runat="server"></asp:label>
</p>

<Script language = "javascript">
    $(function () {
        var divAutorizar = "<%=autorizar.ClientID%>";
        $("#" + divAutorizar).draggable();
    });
</Script>


