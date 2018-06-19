<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.RecepcionAlquiler.ascx.cs" Inherits="AMS.Vehiculos.RecepcionAlquiler" %>
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
    function abrirEmergente(obj) {
        var nit = document.getElementById('_ctl1_' + obj);
        ModalDialog(nit, "SELECT DISTINCT M.MNIT_NIT,NOMBRE FROM MORDENALQUILER M, VMNIT V WHERE M.MNIT_NIT = V.MNIT_NIT AND M.TEST_ESTADO = '2';", new Array(), 1);
        //ModalDialog(nit, '**NITS_TALLER', new Array(), 1);
    }
     function CheckAll(Checkbox, idGrid) {
        var gridItems;
        if (idGrid == 1)
            gridItems = document.getElementById("<%=grillaElementos.ClientID %>");
        else if (idGrid == 2)
            gridItems = document.getElementById("<%=grillaElementos.ClientID %>");

        for (i = 1; i < gridItems.rows.length; i++) {
            if(gridItems.rows[i].cells[8].getElementsByTagName("INPUT")[0].disabled == false)
                gridItems.rows[i].cells[8].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
        }
    }
    </script>

<placeholder id="Pltramite" runat="Server">
    <fieldset>
    <table class="filtersIn">
        <tr>
            <td>Almacen Ingreso:</td>
            <td>
                <asp:DropDownList ID="ddlAlmacen" class="dmediano" runat="server" OnSelectedIndexChanged="Documentos_Alquiler" AutoPostBack="true" ></asp:DropDownList>
            </td>
            <td>Prefijo de ingreso:</td>
            <td>
                <asp:DropDownList ID="ddlPrefijoI" class="dmediano" runat="server" OnSelectedIndexChanged="Consecutivo_Alquiler" AutoPostBack="true"></asp:DropDownList>
            </td>        
            <td>Número:</td>
            <td>
                <asp:TextBox ID="txtNumeroI" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>                
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbNit" runat="server">Nit:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNit" onClick="abrirEmergente('txtNit');" class="tmediano" OnTextChanged="Cargue_Documento"  runat="server" AutoPostBack="true"></asp:TextBox>
                <asp:Image id="imglupa1" runat="server" onClick="abrirEmergente('txtNit');" ImageUrl="../img/AMS.Search.png"></asp:Image>
            </td>       
            <td>
                <asp:Label ID="lbNumero" runat="server">Nombre:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNita" class="tmediano" runat="server" ReadOnly="true"></asp:TextBox>                
            </td>
            <td>
                <asp:Label ID="lbFecha" runat="server">Fecha de Recepcion:</asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtFecha" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>   
            </td>  
            <td></td>
        </tr>
         <tr>
            <td>
                <asp:Label ID="lbObserv" runat="server">Prefijo Factura:</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlFactura" class="dmediano" runat="server"></asp:DropDownList>
            </td>       
            <td>
            <asp:Label ID="Label2" runat="server">Numero Factura:</asp:Label>
            </td>
            <td>
            <asp:DropDownList ID="ddlNumFac" class="dmediano" runat="server"></asp:DropDownList>
            </td>
            <td>
            <asp:Label ID="Label1" runat="server">Observacion:</asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtObserv" class="tpequeno" runat="server"></asp:TextBox>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Button id="btnInfo" runat="server" Text="Seleccionar" onclick="Cargar_Datos" UseSubmitBehavior="false" onClientClick="espera();"> </asp:Button>
            </td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
         </tr>
        </table>
</placeholder>
<placeholder id="PlcElementos" runat="Server" visible="false">
    <p style="font-weight: bold; font-style: italic" align="center">
        Elementos 
    </p>
    <p>
        <asp:DataGrid ID="grillaElementos" runat="server" CssClass="datagrid"  AutoGenerateColumns="false" GridLines="Vertical" OnItemDataBound="dgAccesorioBound" ShowFooter="True">
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
                        <%# DataBinder.Eval(Container.DataItem, "PERIODO", "{0:N}") %>
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
               <asp:TemplateColumn HeaderText="Selección" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <center>
                                        <asp:Label ID="lblChk" runat="server" Text="Selección"></asp:Label><br />
                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAll(this,1);" />
                                    </center>
                                </HeaderTemplate>
			                    <ItemTemplate>
                                    <asp:CheckBox ID="cbRows" runat="server"/>
                                </ItemTemplate>
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
        <asp:Button id="btnAccion" runat="server" Text="Recepcionar Alquiler" onclick="Recepcionar_Alquiler" UseSubmitBehavior="false" onClientClick="espera();"> </asp:Button>
        <asp:Label id="lberror" runat="server"></asp:Label>
</fieldset>
</placeholder>



