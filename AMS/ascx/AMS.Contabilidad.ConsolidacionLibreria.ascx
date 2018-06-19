<%@ Control Language="C#" CodeBehind="AMS.Contabilidad.ConsolidacionLibreria.ascx.cs" AutoEventWireup="true" Inherits="AMS.Contabilidad.ConsolidacionLibreria" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">

<p>
    <table class="filtersIn" id="table1">
        <tbody>
            <tr>
                <td valign="center" bordercolor="gray" width="16" colspan="0">
                    <img height="60" src="../img/AMS.Flyers.Filters.png" border="0" /> 
                </td>
                <td valign="center">
                    <p>
                        Año: 
                        <asp:DropDownList id="year" runat="server"></asp:DropDownList>
                        &nbsp;&nbsp;&nbsp; Mes: 
                        <asp:DropDownList id="month" runat="server"></asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    </p>
                    <p>
                        <asp:Button id="Consulta" onclick="Consulta_Click" runat="server" Text="Generar"></asp:Button>
                    </p>
                </td>
            </tr>
        </tbody>
    </table>
</p>

<asp:Label id="lblAux" runat="server"></asp:Label>