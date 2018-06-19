<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.CrearAlquiler.ascx.cs" Inherits="AMS.Vehiculos.CrearAlquiler" %>
<script type="text/javascript">
    $(function () {
        //mirar documentacion JQueryUI sobre datepicker
                var fechaVal = $("#<%=txtFecha.ClientID%>").val();
        $("#<%=txtFecha.ClientID%>").datepicker();
        $("#<%=txtFecha.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFecha.ClientID%>").val(fechaVal);
    });
     $(function () {
        //mirar documentacion JQueryUI sobre datepicker
                var fechaVal = $("#<%=txtFEchini.ClientID%>").val();
        $("#<%=txtFEchini.ClientID%>").datepicker();
        $("#<%=txtFEchini.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=txtFEchini.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFEchini.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFEchini.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=txtFEchini.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFEchini.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFEchini.ClientID%>").val(fechaVal);
     });
     $(function () {
        //mirar documentacion JQueryUI sobre datepicker
                var fechaVal = $("#<%=txtFechentrg.ClientID%>").val();
        $("#<%=txtFechentrg.ClientID%>").datepicker();
        $("#<%=txtFechentrg.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=txtFechentrg.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFechentrg.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFechentrg.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=txtFechentrg.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFechentrg.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFechentrg.ClientID%>").val(fechaVal);
    });
    function abrirEmergente(obj) {
        var nit = document.getElementById('_ctl1_' + obj);
        //ModalDialog(nit, 'SELECT NIT.mnit_nit AS NIT, Nombre AS NOMBRE FROM Vmnit NIT WHERE NIT.mnit_nit NOT IN (SELECT PNI.pnital_nittaller FROM pnittaller PNI) AND NIT.mnit_nit NOT IN (SELECT CEM.mnit_nit FROM cempresa CEM) order by NOMBRE', new Array(), 1);
        ModalDialog(nit, '**NITS_CLIENTE', new Array(), 1);
    }
    </script>

<placeholder id="Pltramite" runat="Server">
    <fieldset>
    <table class="filtersIn">
        <tr>
            <td>Almacen:</td>
            <td>
                <asp:DropDownList ID="ddlAlmacen" class="dmediano" runat="server" OnSelectedIndexChanged="Documentos_Alquiler" AutoPostBack="true" ></asp:DropDownList>
            </td>
            <td>Prefijo:</td>
            <td>
                <asp:DropDownList ID="ddlPrefijo" class="dmediano" runat="server" OnSelectedIndexChanged="Consecutivo_Alquiler" AutoPostBack="true"></asp:DropDownList>
            </td>        
            <td>Número:</td>
            <td>
                <asp:TextBox ID="txtNumero" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>                
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbNit" runat="server">Nit:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNit" onClick="abrirEmergente('txtNit');" class="tmediano" runat="server"></asp:TextBox>
                <asp:Image id="imglupa1" runat="server" onClick="abrirEmergente('txtNit');" ImageUrl="../img/AMS.Search.png"></asp:Image>
            </td>       
            <td>
                <asp:Label ID="lbNumero" runat="server">Nombre:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNita" class="tmediano" runat="server" ReadOnly="true"></asp:TextBox>                
            </td>
            <td>
                <asp:Label ID="lbFecha" runat="server">Fecha del Negocio:</asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtFecha" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>   
            </td>  
        </tr>
         <tr>
            <td>
                <asp:Label ID="lbObserv" runat="server">Observacion:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtObserv" class="tmediano" runat="server"></asp:TextBox>
                 <asp:Image id="imgMicrofono" runat="server" onClick="Escuchar_Click" ImageUrl="../img/Microphone.png"></asp:Image>
            </td>       
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        </table>
        <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server">Fecha Inicio:</asp:Label>
                <asp:TextBox ID="txtFEchini" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>   
                <asp:Label ID="Label3" runat="server">Hora Inicio:</asp:Label>  
                 <asp:TextBox ID="txtHHInicio" runat="server" placeholder="HH" Width="50px" style="display: inline-block;" MaxLength="2" onkeypress="return soloNumero(event, this)"></asp:TextBox> : 
                <asp:TextBox ID="txtMMInicio" runat="server" placeholder="MM" Width="50px" style="display:inline-block;" MaxLength="2" onkeypress="return soloNumero(event, this)"></asp:TextBox> <b>formato 24 Horas</b>                         
            </td>
             <td>
                <asp:Label ID="Label2" runat="server">Fecha Entrega:</asp:Label>
                <asp:TextBox ID="txtFechentrg" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>   
                <asp:Label ID="Label4" runat="server">Hora Entrega:</asp:Label>  
                <asp:TextBox ID="txtHHEntrega" runat="server" placeholder="HH" Width="50px" style="display: inline-block;" MaxLength="2" onkeypress="return soloNumero(event, this)"></asp:TextBox> : 
                <asp:TextBox ID="txtMMEntrega" runat="server" placeholder="MM" Width="50px" style="display:inline-block;" MaxLength="2" onkeypress="return soloNumero(event, this)"></asp:TextBox> <b>formato 24 Horas</b>                         
            </td>
           <td></td>
           <td></td>
        </tr>
    </table>
    <p style="font-weight: bold; font-style: italic" align="center">
        Elementos 
    </p>
    <p>
        <asp:DataGrid ID="grillaElementos" runat="server" CssClass="datagrid" OnItemCommand="dgEvento_Grilla" AutoGenerateColumns="false" GridLines="Vertical" OnItemDataBound="dgAccesorioBound" ShowFooter="True">
            <FooterStyle CssClass="footer"></FooterStyle>
            <HeaderStyle CssClass="header"></HeaderStyle>
            <PagerStyle CssClass="pager" Mode="NumericPages"></PagerStyle>
            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
            <ItemStyle CssClass="item"></ItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="CODIGO">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="obesequioTextBox" onclick="ModalDialog(this,'SELECT MA.MAFJ_CODIACTI, M.MAFJ_DESCRIPCION AS DESCRIPCION,M.MAFJ_PLACA AS PLACA, COALESCE (PP.PPRE_PRECIO,0) AS PRECIO, PP.TUNI_CODIGO AS TIEMPO FROM MACTIVOFIJO M, MALQUILERACTIVOS MA LEFT JOIN PPRECIOALQUILER PP ON PP.PGRU_CODIGO = MA.PGRU_CODIGO  WHERE MA.MAFJ_CODIACTI = M.MAFJ_CODIACTI AND MA.TEST_ESTADO = 1',new Array())" runat="server" ReadOnly="true"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCRIPCION">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="obesequioTextBoxa" runat="server" ReadOnly="true"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="PLACA">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "PLACA", "{0:C}") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="obesequioTextBoxb" runat="server" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)" ReadOnly="true"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="TIEMPO">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "TIEMPO", "{0:C}") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtOdom" runat="server" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="PERIODO">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "PERIODO", "{0:N}%") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlTiempo" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="VALOR">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="obesequioTextBoxc" runat="server" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="PORCENTAJE IVA">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "IVA", "{0:N}%") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlIVA" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="TOTAL">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "TOTAL", "{0:C}") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="AGREGAR">
                    <ItemTemplate>
                        <asp:Button CommandName="QuitarObsequios" Text="Borrar" ID="btnDel" runat="server" CssClass="bpequeno"/>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button CommandName="AgregarObsequios" Text="Agregar" ID="btnAdd" runat="server" CssClass="bpequeno" />
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </p>
    <table class="filtersIn">
            <tr>
                <td>Costo Elementos: </td>
                <td align="right">
                    <asp:TextBox id="costoOtrosElementos" ReadOnly="True" Text="$0" CssClass="tpqueno" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Iva Elementos: </td>
                <td align="right">
                    <asp:TextBox id="txtIva" ReadOnly="True" Text="$0" CssClass="tpqueno" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Valor Total de Venta : </td>
                <td align="right">
                    <asp:TextBox id="totalVenta" ReadOnly="True" Text="$0" CssClass="tpqueno"  runat="server"></asp:TextBox>
                </td>              
            </tr>
        </table>
         <table>
     <tr>
		<td><asp:Label id="lbVendedor" runat="server">Vendedor</asp:Label>
			<asp:dropdownlist id="ddlVendedor" class="dmediano" runat="server"></asp:dropdownlist></td>
                <asp:PlaceHolder ID="plcVendedor" runat="server">
				<td><asp:Label id="lbClave" runat="server">Clave Vendedor</asp:Label>&nbsp;
				<asp:textbox id="tbClaveVend" runat="server" class="tpequeno" TextMode="Password"></asp:textbox></td>
                </asp:PlaceHolder>
		</tr>
    </table>
        <asp:Button id="btnAccion" runat="server" Text="Crear Alquiler" UseSubmitBehavior="false" onclick="Crear_alquiler" onClientClick="espera();"> </asp:Button>
        <asp:Label id="lberror" runat="server"></asp:Label>
</fieldset>
</placeholder>


