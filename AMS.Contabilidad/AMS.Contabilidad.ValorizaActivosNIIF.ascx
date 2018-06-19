<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Contabilidad.ValorizaActivosNIIF.ascx.cs" Inherits="AMS.Contabilidad.ValorizaActivosNIIF" %>
<fieldset>
    <table id="Table" class="filtersIn">
        <tr>
            <td>
                <asp:Label id="Label1" runat="server">Este proceso nos permite crear el comprobante de valorización y desvalorización de activos fijos.
                Este es un comprobante que afecta solamente NIIF.</asp:Label> 
                <p>
                </p>
                <p>
                    <asp:Label id="Label3" runat="server">Comprobante: </asp:Label>
                    <asp:DropDownList id="tComprobante" class="dgrande" runat="server"></asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label id="Label2" runat="server"><br />Año: </asp:Label> 
                    <asp:DropDownList id="ddlAno" class="dpequeno" runat="server"  ></asp:DropDownList>
                    &nbsp; &nbsp; <asp:Label id="Label4" runat="server">Mes: </asp:Label>
                    <asp:DropDownList id="ddlMes" class="dpequeno" runat="server" ></asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </p>
                <p>
                    <asp:Button id="btnEfectuarVal" onclick="Efectuar_Valorizacion" runat="server" Text="Generar Comprobante de Valorización"></asp:Button>
                </p>
            </td>
        </tr>
    </table>
</fieldset>

<asp:Label ID="lb" runat="server"></asp:Label>