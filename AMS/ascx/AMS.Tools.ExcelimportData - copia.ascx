<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.ExcelimportData.ascx.cs" Inherits="AMS.Tools.ExcelimportData" %>
<script>
    function mostrarInfo() {
        var estado = $('#infoBody').css('display');
        
        $("#infoBody").show();
    }
</script>

<table style="width: 70%; margin: 0; display: inline-block;">
    <tr>
        <td>
            <asp:Label ID="label1" runat="server" Text="Archivo Excel:"></asp:Label>
            <asp:FileUpload ID="xlsUpload" runat="server" Font-Size="Small" />
            <asp:Button ID="btnUpload" runat="server" Text="Cargar"  OnClick="btnUpload_Click"/>
            <asp:Label ID="lblResultado" runat="server" Text="" style="color: Green;"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <div id="info" style="color: blue; text-decoration: underline;  cursor:pointer;" onClick="mostrarInfo()">Click Instrucciones!</div>
            <div id="infoBody" style="display:none;">blaaaaaaaaaaaaaaaaaaaaaa asdasdasdasdasfdsfdsaf dsfsdf sda fsda fsda fsd afasdf</div>
        </td>
    </tr>
</table>
    
