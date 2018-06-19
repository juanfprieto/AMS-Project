<%@ Control Language="c#" %>
<p>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label8" runat="server">Modificación
    de Datos del Vehículo</asp:Label>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label20" runat="server" width="209px">D
    A T O S _ A C T U A L E S</asp:Label>
</p>
<p>
    <asp:Label id="Label3" runat="server">Catálogo Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList2" runat="server" Width="67px"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label12" runat="server">Identificación
    del Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList6" runat="server" Width="290px"></asp:DropDownList>
</p>
<p>
    <asp:Label id="Label6" runat="server" height="28px">Color del Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList3" runat="server" Width="181px" Height="30px"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp; <asp:Label id="Label10" runat="server" height="24px">Año Modelo</asp:Label>
    <asp:DropDownList id="DropDownList4" runat="server" Width="70px"></asp:DropDownList>
    &nbsp;&nbsp; <asp:Label id="Label7" runat="server" width="119px" height="23px">Placa
    del Vehículo</asp:Label>
    <asp:TextBox id="TextBox2" runat="server" Width="102px"></asp:TextBox>
</p>
<p>
    <asp:Label id="Label1" runat="server" height="22px">Tipo de Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList1" runat="server" Width="102px"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp; <asp:Label id="Label2" runat="server">Número de Manifiesto</asp:Label>
    <asp:TextBox id="TextBox1" runat="server" Width="84px"></asp:TextBox>
    &nbsp;&nbsp;<asp:Label id="Label5" runat="server">Fecha del Manifiesto</asp:Label><img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0" /> <asp:Label id="Label4" runat="server">Levate
    Nro.</asp:Label>
    <asp:TextBox id="TextBox3" runat="server" Width="77px"></asp:TextBox>
    <asp:calendar BackColor=Beige id="fecha" runat="server"></asp:Calendar>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="Label21" runat="server" width="222px">D
    A T O S _ N U E V O S</asp:Label>
</p>
<p>
    <asp:Label id="Label9" runat="server">Catálogo Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList5" runat="server" Width="67px"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label11" runat="server">Identificación
    del Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList7" runat="server" Width="290px"></asp:DropDownList>
</p>
<p>
    <asp:Label id="Label13" runat="server" height="28px">Color del Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList8" runat="server" Width="181px" Height="30px"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp; <asp:Label id="Label14" runat="server" height="24px">Año Modelo</asp:Label>
    <asp:DropDownList id="DropDownList9" runat="server" Width="70px"></asp:DropDownList>
    &nbsp;&nbsp; <asp:Label id="Label15" runat="server" width="119px" height="23px">Placa
    del Vehículo</asp:Label>
    <asp:TextBox id="TextBox4" runat="server" Width="102px"></asp:TextBox>
</p>
<p>
    <asp:Label id="Label16" runat="server" height="22px">Tipo de Vehículo</asp:Label>
    <asp:DropDownList id="DropDownList10" runat="server" Width="102px"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp; <asp:Label id="Label17" runat="server">Número de Manifiesto</asp:Label>
    <asp:TextBox id="TextBox5" runat="server" Width="84px"></asp:TextBox>
    &nbsp;&nbsp;<asp:Label id="Label18" runat="server">Fecha del Manifiesto</asp:Label><img onmouseover="calendar.style.visibility='visible'" onmouseout="calendar.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0" /> <asp:Label id="Label19" runat="server">Levate
    Nro.</asp:Label>
    <asp:TextBox id="TextBox6" runat="server" Width="77px"></asp:TextBox>
    <asp:calendar BackColor=Beige id="Calendar1" runat="server"></asp:Calendar>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button id="Button1" runat="server" Text="Grabar"></asp:Button>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
