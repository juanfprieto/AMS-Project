<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Carga.AsignarCarga.ascx.cs" Inherits="AMS.Carga.AMS_Carga_AsignarCarga" %>

<asp:Label ID="Label1" runat="server" Text="Oferente de CARGA:  Nit"></asp:Label>
<asp:TextBox ID="TxtNit" runat="server" Width="150px"></asp:TextBox>
<asp:TextBox ID="TxtNombre" runat="server" Width="217px"></asp:TextBox>
<asp:Label ID="Label2" runat="server" Text="Clave"></asp:Label>
<asp:TextBox ID="TxtClave" runat="server" Width="150px">></asp:TextBox>
<p>
</p>
<p style="width: 572px">
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Asignación de CARGA a postulantes</p>
 <asp:Label ID="Label3" runat="server" Text="Viaje "></asp:Label>
<asp:TextBox ID="TxtViaje" runat="server" Height="22px" Width="397px"></asp:TextBox>
<asp:Label ID="Label4" runat="server" Text="Estado del Viaje" ></asp:Label>
<asp:TextBox ID="TxtEstado" runat="server"  Width="150px"></asp:TextBox>
<br />
<asp:Table ID="TbAsignar" runat="server" Height="34px" Width="802px">
</asp:Table>
<p>
</p>
<p style="width: 566px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Confirmación de CARGA a postulantes</p>
<asp:Table ID="TbConfirmar" runat="server" Height="34px" Width="802px">
</asp:Table>



