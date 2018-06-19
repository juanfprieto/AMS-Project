<%@ Control Language="c#" codebehind="AMS.Contabilidad.DefPresupuesto.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.DefPresupuesto" %>
<p>
    Año:&nbsp;&nbsp;
    <asp:DropDownList id="DropDownList1" runat="server"></asp:DropDownList>
    &nbsp;&nbsp; Cuenta:
    <asp:DropDownList id="DropDownList2" runat="server"></asp:DropDownList>
    &nbsp;&nbsp; Centro de Costo:
    <asp:DropDownList id="DropDownList3" runat="server"></asp:DropDownList>
</p>
<p>
    Enero:&nbsp;&nbsp;&nbsp;
    <asp:TextBox id="TextBox1" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Julio:
    <asp:TextBox id="TextBox8" runat="server"></asp:TextBox>
</p>
<p>
    Febrero:
    <asp:TextBox id="TextBox2" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Agosto:
    <asp:TextBox id="TextBox3" runat="server"></asp:TextBox>
</p>
<p>
    Marzo:&nbsp;&nbsp;
    <asp:TextBox id="TextBox4" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Septiembre:
    <asp:TextBox id="TextBox5" runat="server"></asp:TextBox>
</p>
<p>
    Abril:&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox id="TextBox6" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Octubre:
    <asp:TextBox id="TextBox7" runat="server"></asp:TextBox>
</p>
<p>
    Mayo:&nbsp;&nbsp;&nbsp;
    <asp:TextBox id="TextBox9" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Noviembre:
    <asp:TextBox id="TextBox10" runat="server"></asp:TextBox>
</p>
<p>
    Junio:&nbsp;&nbsp;&nbsp; &nbsp;<asp:TextBox id="TextBox11" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Diciembre:
    <asp:TextBox id="TextBox12" runat="server"></asp:TextBox>
</p>
<p>
    <asp:Button id="Button1" runat="server" Text="Definir presupuesto"></asp:Button>
</p>
<asp:Image runat="server" src="../img/AMS.Icon.Gears.gif"></asp:Image>
<asp:Label id="lbInfo" runat="server">Actualmente en desarrollo</asp:Label>