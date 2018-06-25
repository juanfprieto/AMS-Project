<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.SustentoTributario.ascx.cs" Inherits="AMS.Tools.SustentoTributario" %>
<div id="divMensaje"  runat="server" style="text-align : justify; cursor:move; padding: 2em;">
<asp:DropDownList id="ddlSustento" class="dmediano" runat="server"></asp:DropDownList>   
<asp:DropDownList id="ddlPago class="dmediano" runat="server"></asp:DropDownList> 
<asp:Button id="btnGuardar" Text="Guardar" runat="server"/>
<asp:Label ID="lbError" runat="server"></asp:Label>
</div>
