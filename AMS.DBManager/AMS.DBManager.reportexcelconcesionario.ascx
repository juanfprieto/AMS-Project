<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.DBManager.reportexcelconcesionario.ascx.cs" Inherits="AMS.DBManager.AMS_DBManager_reportexcelconcesionario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.WizardDirection.js"></script>
<script type ="text/javascript" src="../js/AMS.Tools.js"></script>
//<asp:PlaceHolder id="plcSeleccion" runat="server" Visible="True">

<form id="form1" runat="server">
<div>
    &nbsp;<asp:DropDownList ID="DropDownList1" runat="server">
        <asp:ListItem>(todas)</asp:ListItem>
        <asp:ListItem>AU</asp:ListItem>
        <asp:ListItem>VC</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList2" runat="server" >
        <asp:ListItem Value="(todas)">(todas)</asp:ListItem>
        <asp:ListItem Value="2009">2009</asp:ListItem>
        <asp:ListItem Value="2010">2010</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList3" runat="server">
        <asp:ListItem Value="(todas)">(todas)</asp:ListItem>
        <asp:ListItem Value="AUTOALEMANIA S.A.">AUTOALEMANIA S.A.AUTOALEMANIA S.A.</asp:ListItem>
        <asp:ListItem Value="COLWAGEN S.A.">COLWAGEN S.A.COLWAGEN S.A.</asp:ListItem>
        <asp:ListItem>COLWAGEN PREMIUM S.A.COLWAGEN PREMIUM S.A.</asp:ListItem>
        <asp:ListItem>EUROCAR S.A.EUROCAR S.A.</asp:ListItem>
        <asp:ListItem>AUTOBLITZ S.A.AUTOBLITZ S.A.</asp:ListItem>
        <asp:ListItem>LAS MAQUINASLAS MAQUINAS</asp:ListItem>
    </asp:DropDownList>
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Generar" />
        
        
        
</div>
</form>
