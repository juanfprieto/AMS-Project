<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.AnfitrionVentas.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_AnfitrionVentas" %>


<script type="text/javascript" language="javascript">
    function checkPostback(nit)
    {
        if (nit.value.length > 1)
        {
            __doPostBack('<%=txtNit.ClientID%>', nit.value);
        }
    }
    function abrirLupa()
    {
        var vin = document.getElementById('<%=txtNit.ClientID%>');
        ModalDialog(vin, 'SELECT MNIT_NIT, MNIT_NOMBRES || \' \' || MNIT_NOMBRE2 || \' \' || MNIT_APELLIDOS || \' \' || MNIT_APELLIDO2 FROM DBXSCHEMA.MNITCOTIZACION UNION SELECT MNIT_NIT, MNIT_NOMBRES || \' \' || MNIT_NOMBRE2 || \' \' || MNIT_APELLIDOS || \' \' || MNIT_APELLIDO2 FROM DBXSCHEMA.MNIT;', new Array(), 1);      
    }
    function cambiarVendedor()
    {
        var vendedores = document.getElementById('<%=chkVendedores.ClientID%>');
        var almacenes = document.getElementById('<%=rdbAlmacenes.ClientID%>');
        //alert(vendedores.firstElementChild.innerHTML);
        //alert(almacenes);
    }

</script>
<style type="text/css">
    .textos{
        width:150px;
    }
</style>
<fieldset>
    <table id="tabla" runat="server">
        <tr>
            <td colspan="2">
                <div>
                    <asp:Image id="imgEmpresa" runat="server" />
                </div>
            </td>
            <td>
                <b><asp:Label ID="lbEmpresa" runat="server"></asp:Label></b>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table id="Table1" runat="server">
        <tr>
            <td style="vertical-align: top;">
                <fieldset>
                    <legend>Almacenes</legend>
                    <asp:RadioButtonList ID="rdbAlmacenes" runat="server"  RepeatDirection="Vertical" TextAlign="Right"  AutoPostBack="true" OnSelectedIndexChanged="cambioVendedor"></asp:RadioButtonList> <%--onChange="cambiarVendedor()" --%>
                </fieldset>
                
            </td>
            <td style="vertical-align: top;">
                <fieldset>
                    <legend>Vendedores</legend>
                    <asp:RadioButtonList ID="chkVendedores" runat="server"  RepeatDirection="Vertical" ></asp:RadioButtonList>
                    <%--<asp:CheckBoxList ID="chkVendedores" runat="server"  RepeatDirection="Vertical" ></asp:CheckBoxList>--%>
                </fieldset>
            </td>
        </tr>
    </table><br /> <br />
    <table id="Table2" runat="server">
        <tr>
            <td>
                NIT: <asp:TextBox ID="txtNit" runat="server" CssClass="tmediano" AutoPostBack="true" OnTextChanged="cargarInfoNit" ondblclick="abrirLupa()"></asp:TextBox>
                <%--<asp:TextBox ID="txtNit" runat="server" CssClass="tmediano" onChange="checkPostback(this)"></asp:TextBox>--%><!-- falta la lupa-->
                <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirLupa();"></asp:Image>
            </td>
        </tr>
    </table><br /> <br />
    <div id="divInfo" runat="server" visible="false">
        <table>
            <tr>
                <td>
                    Nombre1: <asp:TextBox ID="txtNomb1" runat="server" class="textos"></asp:TextBox>
                </td>
                <td>
                    Nombre2: <asp:TextBox ID="txtNomb2" runat="server" class="textos"></asp:TextBox>
                </td>
                <td>
                    Apellido1: <asp:TextBox ID="txtApe1" runat="server" class="textos"></asp:TextBox>
                </td>
                <td>
                    Apellido2: <asp:TextBox ID="txtApe2" runat="server" class="textos"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Dirección: <asp:TextBox ID="txtDireccion" runat="server" ></asp:TextBox>
                </td>
                <td>
                    Telefono: <asp:TextBox ID="txtTelefono" runat="server" ></asp:TextBox>
                </td>
                <td>
                    Celular: <asp:TextBox ID="txtCelular" runat="server" ></asp:TextBox>
                </td>
                <td>
                    Correo: <asp:TextBox ID="txtCorreo" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button id="btnGuardar" Text="Guardar" runat="server" OnClick="guardarInfo"/>
                </td>
            </tr>
        </table>
    </div>
    <p>
        <asp:Label ID="lbError" runat="server" ></asp:Label>
    </p>
</fieldset>
