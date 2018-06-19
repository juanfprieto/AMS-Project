<%@ Control Language="c#" CodeBehind="AMS.Nomina.CesantiasParciales.cs" AutoEventWireup="false"
    Inherits="AMS.Nomina.CesantiasParciales" %>
<script language="JavaScript">
    function Lista() {
        w = window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<fieldset>
    <h4>
        PRELIQUIDACION POR EMPLEADO&nbsp;
    </h4>
    <p>
        Ingrese los datos Solicitados
    </p>
    <table id="Table" class="filtersIn">
        <tbody>
            <tr>
                <td>
                    AÑO
                </td>
                <td>
                    <asp:DropDownList ID="DDLANO" class="dpequeno" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Mes Inicial
                </td>
                <td>
                    <asp:Label ID="LBMESINICIAL" runat="server" class="lpequeno">ENERO</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Mes Final
                </td>
                <td>
                    <asp:DropDownList ID="DDLMESFINAL" class="dpequeno" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Dias Adicionales a Ajustar
                </td>
                <td>
                    <asp:TextBox ID="DIASADICIONALES" class="tpequeno" runat="server">0</asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <p>
        Datos del Empleado
    </p>
    <p>
        <asp:DropDownList ID="DDLEMPLEADO" class="dmediano" runat="server">
        </asp:DropDownList>
    </p>
    <asp:placeholder id="phGrilla" runat="server">
    <p>
        <table id="Table" class="filtersIn">
            <tr>
                <h4>
                    PRELIQUIDACION DE CESANTIAS&nbsp;</h4>
                <p>
                </p>
            </tr>
            <tr>
                <td>
                    Año de Corte
                </td>
                <td>
                    <asp:Label ID="LBAÑOCORTE" runat="server" class="lpequeno"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Mes de Corte
                </td>
                <td>
                    <asp:Label ID="LBMESCORTE" runat="server" class="lpequeno"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Identificacion
                </td>
                <td>
                    <asp:Label ID="LBIDENTIFICACION" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Nombre del Empleado
                </td>
                <td>
                    <asp:Label ID="LBNOMBREEMPLEADO" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Cargo
                </td>
                <td>
                    <asp:Label ID="LBCARGO" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Dependencia
                </td>
                <td>
                    <asp:Label ID="LBDEPENDENCIA" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Sueldo Actual
                </td>
                <td>
                    <asp:Label ID="LBSUELDOCARGO" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </p>
    <table id="Table" class="filtersIn">
        <tr>
            <td>
                <p>
                    Cesantias Pagadas Anteriormente al Empleado
                </p>
                <p>
                    <asp:DataGrid ID="DATAGRIDCESAANTERIORES" runat="server" AutoGenerateColumns="False">
                        <FooterStyle CssClass="footer"></FooterStyle>
                        <HeaderStyle CssClass="header"></HeaderStyle>
                        <PagerStyle CssClass="pager" Mode="NumericPages"></PagerStyle>
                        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FECHA INICIO" HeaderText="FECHA INICIO"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FECHA FINAL" HeaderText="FECHA FINAL"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CESANTIAS" HeaderText="CESANTIAS" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="INTERESES DE CESANTIA" HeaderText="INTERESES DE CESANTIA"
                                DataFormatString="{0:C}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="DIAS TRABAJADOS" HeaderText="DIAS TRABAJADOS"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </p>
            </td>
        </tr>
    </table>
    <p></p>
    <table id="Table" class="filtersIn">
        <tr>
            <td>
                <p>
                    Resumen de&nbsp;Pagos&nbsp;que Afectan Cesantias&nbsp;por Mes:&nbsp;
                </p>
                <p>
                    <asp:DataGrid ID="DATAGRIDCESANTIAS" runat="server" AutoGenerateColumns="False">
                        <FooterStyle CssClass="footer"></FooterStyle>
                        <HeaderStyle CssClass="header"></HeaderStyle>
                        <PagerStyle CssClass="pager" Mode="NumericPages"></PagerStyle>
                        <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="MES" ReadOnly="True" HeaderText="MES"></asp:BoundColumn>
                            <asp:BoundColumn DataField="VALOR PAGADO" ReadOnly="True" HeaderText="VALOR PAGADO"
                                DataFormatString="{0:C}"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </p>
                <p>
                    Datos para la Liquidacion
                </p>
            </td>
        </tr>
    </table>
    <p></p>
    <table id="Table" class="filtersIn">
        <tbody>
            <tr>
                <td>
                    Valor Base Liquidacion:
                </td>
                <td>
                    <asp:Label ID="LBBASELIQUIDACION" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                </td>
                <td>
                    Vlr. Cesantías Causadas :
                </td>
                <td>
                    <asp:Label ID="LBCESAAPAGAR" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Valor Sueldo Promedio:
                </td>
                <td>
                    <asp:Label ID="LBSUELDOPROMEDIO" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    Vlr. Intereses causados :
                </td>
                <td>
                    <asp:Label ID="LBINTAPAGAR" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Número dias causados
                </td>
                <td>
                    <asp:Label ID="LBDIASTRABAJADOS" runat="server"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    <p></p>
    <table id="Table1" class="filtersIn">
        <p>
            Valores Preliquidacion
        </p>
        <tbody>
            <tr>
                <td>
                    Vlr. Cesantías a Pagar:
                </td>
                <td>
                    <asp:Label ID="LBCESANTIAFINAL" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Vlr. Intereses a Pagar :
                </td>
                <td>
                    <asp:Label ID="LBINTERESFINAL" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Número de días
                </td>
                <td>
                    <asp:Label ID="LBDIASFINAL" runat="server"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    </asp:placeholder>
    <p>
        <asp:Button ID="BTNLIQUIDAR" OnClick="LiquidacionCesantiasParciales" runat="server"
            Text="Pre Liquidar" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')">
        </asp:Button>
        <asp:Button ID="BTNLIQUIDARDEFINITIVAMENTE" OnClick="grabarcesantiasparciales" runat="server"
            Text="LIQUIDAR DEFINITIVAMENTE" Visible="False"></asp:Button></p>
    <p>
        <asp:PlaceHolder ID="toolsHolder" runat="server">
            <table class="tools">
                <tr>
                    <td width="16">
                        <img height="30" src="../img/AMS.Flyers.Tools.png" border="0">
                    </td>
                    <td>
                        Imprimir <a href="javascript: Lista()">
                            <img height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
                        </a>
                    </td>
                    <td>
                        &nbsp; &nbsp;Enviar por correo
                        <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RegularExpressionValidator ID="FromValidator2" Style="left: 100px; position: absolute;
                            top: 400px" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
                        <asp:ImageButton ID="ibMail" OnClick="SendMail" runat="server" BorderWidth="0px"
                            alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"></asp:ImageButton>
                    </td>
                    <td width="380">
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
    </p>
    <p>
        <asp:Label ID="LBPRUEBAS" runat="server" class="lmediano"></asp:Label>
    </p>
    <p>
    </p>
    <p>
    </p>
</fieldset>
<script language:javascript>
    function clickOnce(btn, msg) {
        // Comprobamos si se está haciendo una validación
        if (typeof (Page_ClientValidate) == 'function') {
            // Si se está haciendo una validación, volver si ésta da resultado false
            if (Page_ClientValidate() == false) { return false; }
        }

        // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
        if (btn.getAttribute('type') == 'button') {
            // El atributo msg es totalmente opcional. 
            // Será el texto que muestre el botón mientras esté deshabilitado
            if (!msg || (msg = 'undefined')) { msg = 'Procesando...'; }

            btn.value = msg;

            // La magia verdadera :D
            btn.disabled = true;
        }

        return true;
    }
</script>
