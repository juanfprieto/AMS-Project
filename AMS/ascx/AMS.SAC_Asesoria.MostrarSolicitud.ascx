<%@ Control Language="C#" AutoEventWireup="false" Inherits="AMS.SAC_Asesoria.MostrarSolicitud" %>
<script language="JavaScript">
    function Lista() {
        w = window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<link href="../css/SAC.css" rel="stylesheet" type="text/css" /> 

<p>
    <asp:PlaceHolder id="phSolicitud" runat="server">
        <fieldset>
            <table id="Table1" class="" style="border-radius: 1em; background-color: white;">
                <tr>
                    <td align="center" colSpan="1">
                        <asp:Image id="Image1" runat="server" ImageUrl="../img/ecasIcono.png" visible="false"></asp:Image>
                    </td>	
                    <td align="left" colSpan="3>		
                        <FONT face="Arial" color="#000000"><h3><STRONG>SOLICITUD DE ASESORIA</STRONG></h3></FONT>
                    </td>
                </tr>
                <tr>
                    <td colSpan="4">
                        <div id="tarjetaInfo" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de Solicitud</div>
                    </td>
                </tr>			
                <tr>
                    <td>
                        <font color="#004080"><h4><strong> Número de la Solicitud :</strong></h4></font>
                    </td>
                    <td>
                        <asp:Label id="lbnum" runat="server" font-names="Arial" font-size="Medium" forecolor="Black"></asp:Label>
                    </td>		
                    <td>
                        <font color="#004080"><strong><h4>Fecha y Hora de la Solicitud:</h4></strong></font>
                    </td>
                    <td>
                        <asp:Label id="lbfechor" runat="server" font-names="Arial" font-size="Medium" forecolor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="#004080"><strong><h4>Nit del Cliente:</h4></strong></font> 
                    </td>
                    <td>
                        <asp:Label id="lbnitcli" runat="server" font-names="Arial" font-size="Medium" forecolor="Black"></asp:Label>
                    </td>				
                    <td>
                        <font color="#004080"><strong><h4>Razón Social del Cliente:</h4></strong></font>
                    </td>
                    <td>
                        <asp:Label id="lbnomcli" runat="server" font-names="Arial" font-size="Medium" forecolor="Black"></asp:Label>
                    </td>
                </tr>			
                <tr>
                    <td>
                        <font color="#004080"><strong><h4>Cedula del contacto:</h4></strong></font>
                    </td>
                    <td>
                        <asp:Label id="lbnitcon" runat="server" font-names="Arial" font-size="Medium" forecolor="Black"></asp:Label>
                    </td>				
                    <td>
                        <font color="#004080"><strong><h4>Nombre del contacto:</h4></strong></font>
                    </td>
                    <td>
                        <asp:Label id="lbnomcon" runat="server" font-names="Arial" font-size="Medium" forecolor="Black"></asp:Label>
                    </td>
                </tr>		
                <tr>
                    <td colSpan="4">
                        <div id="divInfoSol" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Detalles de la Solicitud</div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:DataGrid id="dgSolicitud" runat="server" cssclass="datagrid" PageSize="15" GridLines="Vertical" AutoGenerateColumns="true" ShowFooter="false">
                            <FooterStyle CssClass="footer"></FooterStyle>
                            <HeaderStyle CssClass="header"></HeaderStyle>
                            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                            <ItemStyle CssClass="item"></ItemStyle>
                            <Columns></Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td colSpan="4">
                        <div id="divInfoAdjuntos" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Archivos Adjuntos de la Solicitud</div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:DataGrid id="dgArchivos" runat="server" cssclass="datagrid" PageSize="15" GridLines="Vertical" AutoGenerateColumns="false" ShowFooter="false">
                            <FooterStyle CssClass="footer"></FooterStyle>
                            <HeaderStyle CssClass="header"></HeaderStyle>
                            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
                            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                            <ItemStyle CssClass="item"></ItemStyle>
                            <Columns>
                                <asp:BoundColumn DataField="ARCHIVO" ReadOnly="True" HeaderText="Nombre del Archivo"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Descargar Archivo">
                                    <ItemTemplate>
                                        <center>
                                            <asp:HyperLink id="hpldes" runat="server">Descargar Archivo</asp:HyperLink>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>			
            </table>
        </fieldset>
    </asp:PlaceHolder>
</p>
<asp:Button id="btnRegresar" runat="server" Text="Cargar Solicitudes" OnClick="btnRegresarClick" style="margin: 20px; margin-left: 80px;"/>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>
