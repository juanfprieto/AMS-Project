<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.ConsultaOT.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ConsultaOT" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="RemeasLabel" runat="server" Font-Bold="True" Font-Size="Medium">Consultar Ordenes de Trabajo</asp:label>
<P></P>
<script type ="text/javascript">
    //    function Lista() {
    //        w = window.open('AMS.DBManager.Reporte.aspx');
    //    }

    function printDiv(printableArea) {
        var printContents = document.getElementById(printableArea).innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    }
</script>

<fieldset>
    <TABLE id="Table" class="filtersIn">
        <tr>
            <td>
                <TR>
                    <TD><asp:label id="Label1" runat="server" Font-Bold="True" ForeColor="Black"> Prefijo OT:</asp:label></TD>
                    <TD><asp:dropdownlist id="OrdenOT" class="dmediano" runat="server"></asp:dropdownlist></TD>
                </TR>
                <tr>
                    <td><asp:label id="Label19" runat="server" Font-Bold="True" ForeColor="Black">Numero OT</asp:label></td>
                    <td><asp:textbox id="NumeOt" class="tmediano" runat="server"></asp:textbox></td>
                </tr>
                <TR>
                    <TD><asp:button id="Consultar" onclick="consultar" runat="server" Text="Consultar"></asp:button></TD>
                </TR>
            </td>
        </tr>
    </TABLE>
</fieldset>

<asp:panel id="Panel1" runat="server"  Visible="False">
    <TABLE>
        <tr>            
            <td>
                <b>Imprimir en alta calidad:</b>
                <a href="javascript: Lista()">                            
                    <img height="50" alt="Imprimir" src="../img/AMS.Icon.Printer.png" onclick="printDiv('printableArea')" border="0" />
                </a>
            </td>
        </tr>
    </TABLE>

    <div id="printableArea">
        <FIELDSET>
            <LEGEND>Datos Orden de Trabajo</LEGEND>
            <TABLE id="Table2" class="filtersIn">
                <TR>
                    <TD><asp:Label id="Label2" Font-Bold="True" runat="server" ForeColor="Black">Placa</asp:Label></TD>
                    <TD><asp:Label id="PlacaLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD colspan="2"><asp:Label id="Label5" Font-Bold="True" runat="server" ForeColor="Black">VIN&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>    
                        <asp:Label id="VinLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label12" Font-Bold="True" runat="server" ForeColor="Black">Vehiculo</asp:Label></TD>
                    <TD><asp:Label id="VehiLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label6" Font-Bold="True" runat="server" ForeColor="Black">Propietario</asp:Label></TD>
                    <TD><asp:Label id="PropLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label13" Font-Bold="True" runat="server" ForeColor="Black">Identificacion</asp:Label></TD>
                    <TD><asp:Label id="IDLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD></TD>
                    <TD></TD>
                </TR>
                <TR>
                    <TD></TD>
                    <TD></TD>
                    <TD></TD>
                    <TD></TD>
                    <TD></TD>
                    <TD></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label8" Font-Bold="True" runat="server" ForeColor="Black">Kilometraje</asp:Label></TD>
                    <TD><asp:Label id="KilLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label10" Font-Bold="True" runat="server" ForeColor="Black">Observaciones Recepcion</asp:Label></TD>
                    <TD><asp:Label id="ObseRLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label11" Font-Bold="True" runat="server" ForeColor="Black">Observaciones Cliente</asp:Label></TD>
                    <TD><asp:Label id="ObseCliLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label3" Font-Bold="True" runat="server" ForeColor="Black">Fecha de Entrada OT</asp:Label></TD>
                    <TD><asp:Label id="FechaELabel" Font-Bold="True" runat="server" ForeColor="Red">label</asp:Label></TD>
                    <TD><asp:Label id="Label4" Font-Bold="True" runat="server" ForeColor="Black">Hora Entrada OT</asp:Label></TD>
                    <TD><asp:Label id="HoraEnLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                     <TD><asp:Label id="Label24" Font-Bold="True" runat="server" ForeColor="Black">Prefijo de la  OT</asp:Label></TD>
                    <TD><asp:Label id="PrefOt" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label7" Font-Bold="True" runat="server" ForeColor="Black">Fecha de Entrega OT</asp:Label></TD>
                    <TD><asp:Label id="FechaEGLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label9" Font-Bold="True" runat="server" ForeColor="Black">Hora Entrega OT</asp:Label></TD>
                    <TD><asp:Label id="HoraEGLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>            
                    <TD><asp:Label id="Label25" Font-Bold="True" runat="server" ForeColor="Black">Numero de la  OT</asp:Label></TD>
                    <TD><asp:Label id="NumOt" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>                   
                    <TD></TD>
                </TR>
                <TR>
                    <TD></TD>
                    <TD></TD>
                    <TD></TD>
                    <TD></TD>
                    <TD></TD>
                    <TD></TD>
                </TR>
            </TABLE>
        </FIELDSET>
		
        <LEGEND>Movimiento Repuestos</LEGEND>
        
        <asp:datagrid id="Grid" runat="server" AutoGenerateColumns="False" HorizontalAlign="center" cssalass="datagrid">
            <FooterStyle CssClass="footer"></FooterStyle>
            <HeaderStyle CssClass="header"></HeaderStyle>
            <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
            <ItemStyle CssClass="item"></ItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="ORDEN TRANSFERENCIA">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "ORDENT") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="REPUESTO">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "REPUESTO") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CODIGO REPUESTO">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "CODREPUESTO") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CANTIDAD">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="VALOR UNITARIO">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "VALORU") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCUENTO">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "DESCUENTO") %>
                </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:datagrid>
		
        <LEGEND>Devoluciones</LEGEND>
        
        <asp:datagrid id="Grid1" runat="server" AutoGenerateColumns="False" HorizontalAlign="center">
            <FooterStyle CssClass="footer"></FooterStyle>
            <HeaderStyle CssClass="header"></HeaderStyle>
            <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
            <ItemStyle CssClass="item"></ItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="ORDEN DEVOLUCION">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "ORDENDEV") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DOC_REFERENCIA">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "TRANSFE") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="REPUESTO">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "REPUESTO1") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CODIGO REPUESTO">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "CODREPUESTO1") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CANTIDAD">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "CANTIDAD1") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="VALOR UNITARIO">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "VALORU1") %>
                </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:datagrid>
		
        <LEGEND>Operaciones Relacionadas</LEGEND>
        
        <asp:datagrid id="dgOperaciones" runat="server" AutoGenerateColumns="False" HorizontalAlign="center">
            <FooterStyle CssClass="footer"></FooterStyle>
            <HeaderStyle CssClass="header"></HeaderStyle>
            <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
            <ItemStyle CssClass="item"></ItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="CODIGO OPERACIÓN">
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "CODIGOOPERACION") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCRIPCION OPERACIÓN">
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="PRECIO">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "PRECIO", "{0:C}") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="ESTADO OPERACIÓN">
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "ESTADOPERACION") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="FECHA DE TERMINACIÓN">
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "FECHATERM") %>
                </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:datagrid>
		
        <LEGEND>Facturas Relacionadas</LEGEND>
        
        <asp:Datagrid id="dgFacturas" runat="server" AutoGenerateColumns="False" HorizontalAlign="center" cssclass="datagrid">
            <FooterStyle CssClass="footer"></FooterStyle>
            <HeaderStyle CssClass="header"></HeaderStyle>
            <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
            <ItemStyle CssClass="item"></ItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="PREFIJO-NÚMERO FACTURA">
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "PREFIJOFACTURA") %>
                -
                <%# DataBinder.Eval(Container.DataItem, "NUMEROFACTURA") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CARGO FACTURA">
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "CARGOFACTURA") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="VALOR FACTURA">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "VALORFACTURA", "{0:C}") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="VALOR IVA FACTURA">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "VALORIVAFACTURA", "{0:C}") %>
                </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="VALOR RETENCIONES">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "VALORRETENCIONES", "{0:C}") %>
                </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:Datagrid>

        <FIELDSET>
            <LEGEND align="Center">Detalles</LEGEND>
        
            <TABLE id="Table3" class="filtersIn">
                <TR>
                    <TD><asp:Label id="Label16" Font-Bold="True" runat="server" ForeColor="Black" Visible="False">Total Items</asp:Label></TD>
                    <TD><asp:Label id="TotReLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label20" Font-Bold="True" runat="server" ForeColor="Black">Total Repuestos</asp:Label></TD>
                    <TD><asp:Label id="totalRelabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label18" Font-Bold="True" runat="server" ForeColor="Black">Total Valor Repuestos</asp:Label></TD>
                    <TD><asp:Label id="TotReCoLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD></TD>
                    <TD></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label14" Font-Bold="True" runat="server" ForeColor="Black">Total Items Devueltos</asp:Label></TD>
                    <TD><asp:Label id="TotDevLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label22" Font-Bold="True" runat="server" ForeColor="Black">Total Repuestos Devueltos</asp:Label></TD>
                    <TD><asp:Label id="totalReDeLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label15" Font-Bold="True" runat="server" ForeColor="Black">Total Costo Devoluciones</asp:Label></TD>
                    <TD><asp:Label id="TotCosLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label21" Font-Bold="True" runat="server" ForeColor="Black">Cantidad de Operaciones</asp:Label></TD>
                    <TD><asp:Label id="lbCantOperaciones" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label17" Font-Bold="True" runat="server" ForeColor="Black">Total Neto(Transferencias-Devoluciones)</asp:Label></TD>
                    <TD><asp:Label id="TotNetoLabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label23" Font-Bold="True" runat="server" ForeColor="Black">Total Valor Operaciones</asp:Label></TD>
                    <TD><asp:Label id="lbValorOperaciones" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label26" Font-Bold="True" runat="server" ForeColor="Black">Total Cargo Interno:</asp:Label></TD>
                    <TD><asp:Label id="lbValorCargoInterno" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD><asp:Label id="Label28" Font-Bold="True" runat="server" ForeColor="Black">Total Cargo Seguro:</asp:Label></TD>
                    <TD><asp:Label id="lbValorCargoSeguro" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                </TR>
                <TR>
                    <TD><asp:Label id="Label30" Font-Bold="True" runat="server" ForeColor="Black">Total Cargo Cliente:</asp:Label></TD>
                    <TD><asp:Label id="lbValorCargoCliente" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
                    <TD></TD>
                    <TD></TD>
                </TR>
            </TABLE>
        </FIELDSET>
    </div>
</asp:panel>
