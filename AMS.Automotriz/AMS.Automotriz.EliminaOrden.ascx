<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.EliminaOrden.ascx.cs" Inherits="AMS.Automotriz.EliminaOrden" %>

<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>

<fieldset>
    <Table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server">
                    Prefijo de la orden:
                </asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoOrden" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cargarNumOrdenes"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server">
                    Número de la orden: 
                </asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlNumeroOrden" runat="server" AutoPostBack="true" OnSelectedIndexChanged="buscarFacturas" Enabled="false" ></asp:DropDownList>
            </td>
        </tr>
    </Table>
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:Button id="btnBorrar" runat="server" Text="Borrar orden" OnClick="borrarOrden"> </asp:Button>
            </td>
        </tr>
    </table>
</fieldset>

<fieldset runat="server" id="plhTabla" visible="false">

</fieldset>

<script type="text/javascript" >
    function llenarNumOrden(obj) {
        var tipoDato = $('#<%=ddlTipoOrden.ClientID %>').val();
        EliminaOrden.llenar_NumOrden(obj.value, cambiar_numOrden_Callback);
    }

    function cambiar_numOrden_Callback(response) 
    {
        var respuesta = response.value;
        var numOrde = document.getElementById("<%=ddlNumeroOrden.ClientID%>");

        if (respuesta.Tables[0].Rows.length == 0)
        {
            alert('No existen órdenes de trabajo abiertas para el prefijo: ' + tipoDato.value);
            return;
        }
        else
        {
            
            if (respuesta.Tables[0].Rows.length > 0)
            {
                numOrde.disabled = false;
                numOrde.options[0] = new Option("Seleccione..", "0");
                for(var i = 0; i < respuesta.Tables[0].Rows.length; i ++)
                {
                    numOrde.options[i+1] = new Option(respuesta.Tables[0].Rows[i].ORDEN, respuesta.Tables[0].Rows[i].NUMERO);
                }
            }
        }
    }
</script>
