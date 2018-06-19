<%@ Control Language="c#" %>
<p>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label1" width="333px" runat="server">Actualiza
    Precios a Items de Ordenes de Trabajo</asp:Label>
</p>
<p>
</p>
<p>
    <asp:Label id="Label2" runat="server">En este proceso, se actualizan los precios a
    los items de una Orden de Trabajo, actualizandolos a los PRECIOS ACTUALES DE LOS MAESTROS.
    Este procedimiento es aplicable a ordenes muy retrazadas en el taller (más de 1 mes)
    y los precios se hayan modificado y que NO correspondan a PRESUPUESTOS como las compañias
    de seguros.</asp:Label>
</p>
<p>
</p>
<p>
    <asp:Label id="Label6" runat="server">Tipo de Orden</asp:Label>
    <asp:DropDownList id="DropDownList1" runat="server" Width="110px"></asp:DropDownList>
    <asp:Label id="Label9" runat="server">Número de Orden</asp:Label>
    <asp:DropDownList id="DropDownList2" runat="server" Width="107px"></asp:DropDownList>
</p>
<p>
    &nbsp;<asp:Label id="Label4" width="142px" runat="server">Catálogo del Vehículo</asp:Label>&nbsp;&nbsp;<asp:TextBox id="TextBox2" runat="server" Width="69px"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="Label7" runat="server">Número
    Identificación (VIN)</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="TextBox1" runat="server" Width="69px"></asp:TextBox>
    &nbsp;<asp:Label id="Label8" runat="server">PLACA del Vehículo</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="TextBox3" runat="server" Width="69px"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="Label10" runat="server">Propietario</asp:Label>&nbsp; <asp:Label id="Label11" runat="server">Nit</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label12" runat="server">Nombre</asp:Label>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label3" runat="server">Fecha
    de la Actualización</asp:Label>
    <asp:calendar BackColor=Beige id="Calendar1" runat="server"></asp:Calendar>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="Label5" runat="server">Actulizar</asp:Label>
    <asp:RadioButtonList id="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
        <asp:ListItem Value="No Actualiza">No Actualiza</asp:ListItem>
        <asp:ListItem Value="Semi_Autom&#225;tico">Semi_Autom&#225;tico</asp:ListItem>
        <asp:ListItem Value="Autom&#225;tico">Autom&#225;tico</asp:ListItem>
    </asp:RadioButtonList>
</p>
<p>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Image id="Image2" runat="server" src="../img/AMS.Icon.Gears.gif"></asp:Image>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button id="Button1" runat="server" Text="Actualizar Precios"></asp:Button>
</p>
<p>
    <asp:Label id="lbInfo" runat="server">Actualmente en desarrollo</asp:Label>
</p>
<p>
</p>
