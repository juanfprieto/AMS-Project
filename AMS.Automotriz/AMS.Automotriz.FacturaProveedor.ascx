<%@ Control Language="c#" %>
<p>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label1" width="333px" runat="server">Recepción
    y Entrada de Vehiculos</asp:Label>
</p>
<p>
    &nbsp;&nbsp;&nbsp; <asp:Label id="Label4" class="lpequeno" runat="server">Proceso</asp:Label>
    <asp:DropDownList id="DropDownList3" runat="server" class="dmediano"></asp:DropDownList>
</p>
<p>
</p>
<p>
    &nbsp;<asp:Label id="Label3" runat="server">Catálogo Vehículo</asp:Label> 
    <asp:DropDownList id="DropDownList2" runat="server" class="dpequeno"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Label id="Label6" class="lpequeno" runat="server" height="35px">Nro
    Identificación (VIN) del Vehículo</asp:Label> 
    <asp:DropDownList id="DropDownList1" runat="server" class="dmediano"></asp:DropDownList>
</p>
<p>
    &nbsp;<asp:Label id="Label10" runat="server">PreFijo Factura Proveedor</asp:Label> 
    <asp:TextBox id="TextBox1" runat="server" class="tpequeno"></asp:TextBox>
    &nbsp;&nbsp;&nbsp; <asp:Label id="Label2" runat="server">Número de la Factura</asp:Label>
    <asp:TextBox id="TextBox2" runat="server" class="tpequeno"></asp:TextBox>
</p>
<p>
    <asp:Label id="Label7" runat="server">Documento de Entrada</asp:Label>
    <asp:DropDownList id="DropDownList4" runat="server"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label8" runat="server">Número
    de la Entrada</asp:Label>
    <asp:TextBox id="TextBox3" runat="server" class="tpequeno"></asp:TextBox>
</p>
<p>
    <asp:Label id="Label5" runat="server">Fecha de la Factura</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; <img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0" /> 
</p>
<p>
    <table id="calendar1" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendar.style.visibility='hidden'">
        <tbody>
            <tr>
                <td>
                    <asp:calendar BackColor=Beige id="fecha" runat="server"></asp:Calendar>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp; Fecha Prometida de Entrega&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</p>
<p>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</p>
<p>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    <asp:Image id="Image2" runat="server" src="../img/AMS.Icon.Gears.gif"></asp:Image>
</p>
<p>
    <asp:Label id="lbInfo" runat="server">Actualmente en desarrollo</asp:Label>
</p>
<p>
</p>