<%@ Control Language="c#" %>
<p>
</p>
<p>
    <asp:Label id="Label1" runat="server">Tipo de Pedido</asp:Label>
    <asp:DropDownList id="ddlTipoPedido" runat="server" Width="102px"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp; <asp:Label id="Label2" runat="server">Número</asp:Label>
    <asp:TextBox id="TextBox1" runat="server" Width="84px"></asp:TextBox>
    &nbsp;&nbsp; <asp:Label id="Label3" runat="server">Catálogo Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList2" runat="server" Width="67px"></asp:DropDownList>
    &nbsp; 
</p>
<p>
    <asp:Label id="Label6" runat="server">Tipo de Automovil</asp:Label>
    <asp:ListBox id="ListBox1" runat="server" Width="94px" Height="37px"></asp:ListBox>
    <asp:Label id="Label4" runat="server" width="76px">Vendedor</asp:Label>
    <asp:DropDownList id="DropDownList3" runat="server" Width="115px"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="Label10" runat="server">Año Modelo</asp:Label> 
    <asp:DropDownList id="DropDownList4" runat="server" Width="70px"></asp:DropDownList>
</p>
<p>
    <asp:Label id="Label5" runat="server">Fecha del Pedido</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; <img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0" /> 
    <table id="calendar1" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendar.style.visibility='hidden'">
        <tbody>
            <tr>
                <td>
                    <asp:calendar BackColor=Beige id="fecha" runat="server"></asp:Calendar>
                </td>
            </tr>
        </tbody>
    </table>
    &nbsp;&nbsp;&nbsp;&nbsp; Fecha Prometida de Entrega&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</p>
<p>
</p>
<img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0" /> 
<table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendar.style.visibility='hidden'">
    <tbody>
        <tr>
            <td>
                <asp:calendar BackColor=Beige id="Calendar1" runat="server"></asp:Calendar>
            </td>
        </tr>
    </tbody>
</table>
<asp:Label id="Label9" runat="server">Identificación del Cliente</asp:Label> 
<p>
    <asp:Label id="Label8" runat="server">Identificación del Cliente </asp:Label>
    <asp:DropDownList id="DropDownList5" runat="server"></asp:DropDownList>
</p>
<p>
    <asp:Label id="Label12" runat="server">Identificación del Cliente Alterno </asp:Label>
    <asp:DropDownList id="DropDownList6" runat="server"></asp:DropDownList>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
    <asp:Label id="Label7" runat="server" width="156px" height="43px">Valor del Vehículo(con
    impuestos incluidos)</asp:Label>
    <asp:TextBox id="TextBox2" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label11" runat="server" width="118px" height="35px">Valor
    Descuento sobre el vehículo</asp:Label>
    <asp:TextBox id="TextBox3" runat="server"></asp:TextBox>
    <asp:Label id="Label14" runat="server">Relación de otros elementos y servicios vendidos</asp:Label>
</p>
<p>
    <table>
        <tbody>
            <tr>
                <td>
                </td>
                <td>
                    Table</td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </tbody>
    </table>
</p>
<p>
    &nbsp;<asp:Label id="Label15" runat="server" width="72px" height="36px">Valor Total
    de la Venta</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label16" runat="server" width="88px">Suma
    de lo vendido</asp:Label>&nbsp;&nbsp; <asp:Label id="Label17" runat="server" width="82px" height="36px">Elementos
    Obsequiados</asp:Label>
    <asp:TextBox id="TextBox4" runat="server" Width="115px" Height="48px"></asp:TextBox>
    &nbsp; <asp:Label id="Label18" runat="server" width="67px" height="30px">Costo Obsequios</asp:Label>
    <asp:TextBox id="TextBox5" runat="server" Width="78px"></asp:TextBox>
</p>
<p>
</p>
<p>
    <asp:Label id="Label19" runat="server">Efectivo</asp:Label>
    <asp:TextBox id="TextBox6" runat="server"></asp:TextBox>
    <asp:Label id="Label20" runat="server">Cheques Post_Fechados</asp:Label>
    <asp:TextBox id="TextBox7" runat="server"></asp:TextBox>
    <asp:Label id="Label21" runat="server">Financiera</asp:Label>
    <asp:TextBox id="TextBox8" runat="server"></asp:TextBox>
</p>
<p>
    <asp:Label id="Label22" runat="server">Nombre Financiera</asp:Label>
    <asp:TextBox id="TextBox9" runat="server" Width="117px"></asp:TextBox>
    <asp:Label id="Label23" runat="server" width="141px" height="38px">Retoma VEHICULO:
    catálogo</asp:Label>
    <asp:DropDownList id="DropDownList7" runat="server"></asp:DropDownList>
    <asp:Label id="Label24" runat="server" width="89px" height="40px">Nro Contrato Consignación</asp:Label>
    <asp:TextBox id="TextBox10" runat="server"></asp:TextBox>
</p>
<p>
    <asp:Label id="Label25" runat="server">Año Modelo</asp:Label>
    <asp:DropDownList id="DropDownList8" runat="server"></asp:DropDownList>
    <asp:Label id="Label26" runat="server">Placa</asp:Label>
    <asp:TextBox id="TextBox11" runat="server" Width="74px"></asp:TextBox>
    <asp:Label id="Label27" runat="server" width="90px" height="37px">Cuenta Impuestos
    en</asp:Label>
    <asp:TextBox id="TextBox12" runat="server"></asp:TextBox>
    <asp:Label id="Label28" runat="server">Valor Recibido</asp:Label>
    <asp:TextBox id="TextBox13" runat="server" Width="106px"></asp:TextBox>
</p>
<p>
    <asp:Label id="Label29" runat="server">Otra Forma de pago</asp:Label>
    <asp:TextBox id="TextBox14" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="Label30" runat="server">Por</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    <asp:TextBox id="TextBox15" runat="server"></asp:TextBox>
    <asp:Label id="Label31" runat="server" width="77px" height="35px">Total Forma de Pago</asp:Label>
    <asp:TextBox id="TextBox16" runat="server" Width="127px"></asp:TextBox>
</p>
<p>
</p>
<p>
    <asp:Image id="Image2" runat="server" src="../img/AMS.Icon.Gears.gif"></asp:Image>
</p>
<p>
    <asp:Label id="lbInfo" runat="server">Actualmente en desarrollo</asp:Label>
</p>
<p>
</p>