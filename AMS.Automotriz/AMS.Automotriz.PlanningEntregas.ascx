<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.PlanningEntregas.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_PlanningEntregas" %>

<fieldset><legend>Tablero Entregas Vehículos</legend>
    <asp:Label style="font-size: large; margin: 3px;">Bienvenido a la sección de planning</asp:Label><br /><br />
    <asp:Label>Elija cuál información desea mostrar en el tablero</asp:Label>
    <asp:DropDownList ID="ddlProcedimiento" runat="server">
        <asp:ListItem Value ="TABLERO_PLANNING" Text="Planing de Entregas de vehículos" Selected="true"></asp:ListItem>
        <asp:ListItem Value ="OTRO_PLANNING" Text="Planing de ..."></asp:ListItem>
    </asp:DropDownList>

<asp:Button runat="server" Text="Abrir Tablero" OnClick="abrirPlaning"/>
</fieldset>
