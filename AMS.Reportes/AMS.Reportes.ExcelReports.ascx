<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Reportes.ExcelReports.ascx.cs" Inherits="AMS.Reportes.ExcelReports" %>

<fieldset>
    <legend>Generador Excel: <asp:Label ID="lblTitulo" runat="server"></asp:Label></legend>
    Al hacer click en el boton "generar" se crea un archivo Excel basado en una plantilla predefinida de Excel.
    <br />
    <asp:PlaceHolder ID="plcContenido" runat="server">
        Año: <asp:DropDownList id=ddlYear runat="server" class="dpequeno"></asp:DropDownList><br />
        Mes Máximo:<asp:DropDownList id=ddlMes runat="server" class="dpequeno"></asp:DropDownList>
    </asp:PlaceHolder>
    <br />
    <asp:Button ID="btnGenerar" OnClick="GenerarExcel" Text="Generar" runat="server"></asp:button>      
    <br />
    <asp:HyperLink id="lkDescarga" runat="server" Visible=false>Descargar Archivo</asp:HyperLink>
</fieldset>