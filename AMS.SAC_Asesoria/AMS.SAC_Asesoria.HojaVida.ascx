<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.SAC_Asesoria.HojaVida.ascx.cs" Inherits="AMS.SAC_Asesoria.HojaVida" %>
<link href="../css/SAC.css" rel="stylesheet" type="text/css" /> 

<fieldset>
    <table class="filtersIn">
        <tr>
            <td>
               <h3>Menu</h3>
                <asp:DropDownList id="ddlMenuPrincipal" class="dpequeno" runat="server" autopostback="true" onselectedindexchanged="ddlMenuPrin_OnSelectedIndexChanged"    ></asp:DropDownList>                   
            </td>
            <td>
                <h3>Carpeta</h3>
                <asp:DropDownList id="ddlmenuCarpeta"   class="dpequeno" runat="server" autopostback="true" onselectedindexchanged="ddlmenuCarpeta_OnSelectedIndexChanged" ></asp:DropDownList>        
            </td>
            <td colspan="2">
                <h3>Opcíón</h3>
                <asp:DropDownList id="ddlmenuOpcion"    class="dmediano" runat="server" autopostback="true" onselectedindexchanged="ddlmenuOpcion_OnSelectedIndexChanged" ></asp:DropDownList>
            </td>
            <td>
                <h3>Procedencia</h3>
                <asp:DropDownList id="tipoSolicitud"    class="dpequeno" runat="server" autopostback="true" onselectedindexchanged="tipoSolicitud_OnSelectedIndexChanged" ></asp:DropDownList>
            </td>
        </tr>        
    </table>
</fieldset>

<asp:Panel id="offerVehicle" runat="server" Visible="true" style="text-align:center; margin-top:20px;">
    <div id="DivHojaVida" runat= "server"></div>
</asp:Panel>
