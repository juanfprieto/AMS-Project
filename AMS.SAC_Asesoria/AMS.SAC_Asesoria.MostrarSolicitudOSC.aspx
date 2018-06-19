<%@ Page Language="C#" AutoEventWireup="false" Inherits="AMS.SAC_Asesoria.MostrarSolicitudOSC" %>
<html>
<head>
</head>
<body>
    <link href="../css/SAC.css" type="text/css" rel="stylesheet" />
    <form id="Form1" runat="server">
        <asp:PlaceHolder id="phSolicitud" runat="server">
            <table>
                <tbody>
                    <tr>
                        <td align="middle" colspan="6">
                            SOLICITUD DE ASESORIA</td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            Número de la Solicitud : 
                        </td>
                        <td>
                            <asp:Label id="lbnum" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            Nit del Cliente : 
                        </td>
                        <td>
                            <asp:Label id="lbnitcli" runat="server"></asp:Label></td>
                        <td></td>
                        <td></td>
                        <td>
                            Razón Social del Cliente : 
                        </td>
                        <td>
                            <asp:Label id="lbnomcli" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            Cedula del contacto : 
                        </td>
                        <td>
                            <asp:Label id="lbnitcon" runat="server"></asp:Label></td>
                        <td></td>
                        <td></td>
                        <td>
                            Nombre del Contacto : 
                        </td>
                        <td>
                            <asp:Label id="lbnomcon" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            Fecha y Hora de la Solicitud : 
                        </td>
                        <td>
                            <asp:Label id="lbfechor" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            Detalle de la Solicitud 
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="middle" colspan="6">
                            <asp:DataGrid id="dgSolicitud" runat="server" BorderWidth="1px" PageSize="15" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical" Font-Names="Verdana" AutoGenerateColumns="true" ShowFooter="false">
                                <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
                                <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
                                <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
                                <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
                                <Columns></Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:PlaceHolder>
        <!-- Insert content here -->
    </form>
</body>
</html>
