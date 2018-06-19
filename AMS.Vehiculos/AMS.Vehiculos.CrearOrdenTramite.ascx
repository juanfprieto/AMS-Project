<%@ control language="C#" autoeventwireup="true" codebehind="AMS.Vehiculos.CrearOrdenTramite.ascx.cs" inherits="AMS.Vehiculos.CrearOrdenTramite" %>
<script>
    function abrirEmergente(obj) {
        var nit = document.getElementById('_ctl1_' + obj);
        //ModalDialog(nit, 'SELECT NIT.mnit_nit AS NIT, Nombre AS NOMBRE FROM Vmnit NIT WHERE NIT.mnit_nit NOT IN (SELECT PNI.pnital_nittaller FROM pnittaller PNI) AND NIT.mnit_nit NOT IN (SELECT CEM.mnit_nit FROM cempresa CEM) order by NOMBRE', new Array(), 1);
        ModalDialog(nit, '**NITS_CLIENTE', new Array(), 1);
    }
</script>
<placeholder id="PlOpcion" runat="Server">
    <fieldset>
    <table>
    <tr>
    <td class="tittleBox"> Permite Crear Ordenes de tramite con o sin Pedido de vehículos.</td></tr>
    <tr>
      <td>Seleccione el tipo de tramite a crear. :           
       <asp:DropDownList id="tipoTramite" class="dmediano" runat="server">
            <asp:ListItem Value="S">Seleccione...</asp:ListItem>
            <asp:ListItem Value="SP">Sin Pedido</asp:ListItem>
            <asp:ListItem Value="CP">Con Pedido</asp:ListItem>
        </asp:DropDownList>
      </td>
    </tr>
    <tr>
    <td>
      <asp:Button id="btnRealizar" onclick="Realizar_Tramite" runat="server" Text="Realizar"></asp:Button>
    </td>
    </tr>
    <tr></tr>
    </table>
    </fieldset>
</placeholder>
<br/>
<placeholder id="PlEditar" runat="Server">
    <fieldset>
        <table>
            <tr>
                <td class="tittleBox"> Permite Editar Ordenes de Tramite</td>
            </tr>
            <tr>
                <td>Seleccione el prefijo del tramite a editar. :
                    <asp:DropDownList id="ddlPrefEdit" class="dmediano" AutoPostBack="true" OnSelectedIndexChanged="Cargar_Numeros" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Numero de tramite:
                    <asp:DropDownList ID="ddlNumEdit" Class="dmediano" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button id="btnEditar" onclick="Editar_Tramite" runat="server" Text="Editar"></asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
</placeholder>
<br/>
<placeholder id="PlEliminar" runat="Server">
    <fieldset>
        <table>
            <tr>
                <td class="tittleBox"> Permite eliminar Ordenes de tramite.</td></tr>
            <tr>
                <td>Seleccione el prefijo del tramite a eliminar. :           
                    <asp:DropDownList id="ddlPrefijo" class="dmediano" AutoPostBack="true" OnSelectedIndexChanged="Cargar_Numeros" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Numero de Tramite:           
                    <asp:DropDownList id="ddlNumInic" class="dmediano" runat="server"></asp:DropDownList><br/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button id="btnEliminar" onclick="Eliminar_Tramite" runat="server" Text="Eliminar"></asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
</placeholder>

<placeholder id="Pltramite" runat="Server" visible="false">
    <fieldset>
    <table class="filtersIn">
        <tr>
            <td>Prefijo del Tramite:</td>
            <td>
                <asp:DropDownList ID="ddlTramite" class="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_PrefijoTramite"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Número de Tramite:</td>
            <td>
                <asp:TextBox ID="txtNumero" class="tmediano" runat="server" ReadOnly="true"></asp:TextBox>                
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lbNit" runat="server">Nit:</asp:Label></td>
            <td>
                <asp:TextBox ID="txtNit" class="tmediano" runat="server" onClick="abrirEmergente('txtNit');"></asp:TextBox>
                <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('txtNit');"></asp:Image>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lbNumero" runat="server">Nombre:</asp:Label></td>
            <td>
                <asp:TextBox ID="txtNita" class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox>                
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lbPrefijoPed" runat="server">Prefijo del Pedido:</asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlPedido" class="dmediano" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_PrefijoPedido"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lbNumeroPed" runat="server">Número de Pedido:</asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlNumPedi" runat="server" class="dpequeno"></asp:DropDownList>
                <asp:Image ID="Image1" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image>
            </td>
        </tr>
    </table>
    <p style="font-weight: bold; font-style: italic" align="center">
        Elementos de Venta 
    </p>
    <p>
        <asp:DataGrid ID="grillaElementos" runat="server" CssClass="datagrid" OnItemCommand="dgEvento_Grilla" AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" 
        OnItemDataBound="dgAccesorioBound" OnEditCommand="dgServicios_Edicion">
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
                        <asp:TextBox ID="obesequioTextBox" onclick="ModalDialog(this,'SELECT pite_codigo,pite_nombre,pite_costo,PTRA_CODIGO FROM DBXSCHEMA.pitemventavehiculo',new Array())" runat="server" ReadOnly="true"></asp:TextBox>
                    </FooterTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="obesequioTextBox" onclick="ModalDialog(this,'SELECT pite_codigo,pite_nombre,pite_costo,PTRA_CODIGO FROM DBXSCHEMA.pitemventavehiculo',new Array())" runat="server" ReadOnly="true"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCRIPCION">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="obesequioTextBoxa"  runat="server" ReadOnly="true"></asp:TextBox>
                    </FooterTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="obesequioTextBoxa"  runat="server" ReadOnly="true"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="COSTO">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "COSTO", "{0:C}") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="obesequioTextBoxb" runat="server" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
                    </FooterTemplate>
                     <EditItemTemplate>
                        <asp:TextBox ID="obesequioTextBoxb" runat="server" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="PORCENTAJE IVA">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "IVA", "{0:N}%") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlIVA" runat="server" AutoPostBack="false"></asp:DropDownList>
                    </FooterTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIVA" runat="server" AutoPostBack="false"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="PLACA/DOC. REFE">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "OBSERVACION") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtObservacion" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
                    </FooterTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtObservacion" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="AGREGAR">
                    <ItemTemplate>
                        <asp:Button CommandName="QuitarObsequios" Text="Borrar" ID="btnDel" runat="server" CssClass="bpequeno"/>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button CommandName="AgregarObsequios" Text="Agregar" ID="btnAdd" runat="server" CssClass="bpequeno" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                 <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
    </p>
    <table class="filtersIn">
            <tr>
                <td>Costo Elementos Venta : </td>
                <td align="right">
                    <asp:TextBox id="costoOtrosElementos" runat="server" ReadOnly="True" Text="$0" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Observacion: </td>
                <td align="right">
                  <%--  FICAR EL CAMPO obsequios a varchar (300)--%>
                    <asp:TextBox id="descripcionObsequios" runat="server" CssClass="AlineacionDerecha" maxlength="299" TextMode="MultiLine" Height="100px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbOpcion" runat="server" Text="Detalle del vehículo" Visible="false"></asp:Label> <br />
                    <asp:DropDownList id="ddlOpciVehiDetalle" Visible="false" runat="server" class="dmediano" ></asp:DropDownList>
                </td>
                <td></td>
                <td align="right">
                    <asp:TextBox id="costoObsequios" Visible="false" onkeyup="NumericMaskE(this,event)" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Valor Total de Venta:</td>
                <td align="right">
                    <asp:TextBox id="totalVenta" runat="server" ReadOnly="True" Text="$0" CssClass="AlineacionDerecha"></asp:TextBox>
                </td>
                <td></td>
                <td></td>                
            </tr>
        </table>
        <asp:Button id="btnAccion" onclick="EjecutarAccion" runat="server" Text="Crear Tramite" UseSubmitBehavior="false" onClientClick="espera();"> </asp:Button>
        <asp:Button id="btnEdicion" onclick="Guardar_Edicion" runat="server" Text="Guardar Modificacion" Visible="false" UseSubmitBehavior="false" onClientClick="espera();"> </asp:Button>
        <asp:Label id="lberror" runat="server"></asp:Label>
</fieldset>
</placeholder>

