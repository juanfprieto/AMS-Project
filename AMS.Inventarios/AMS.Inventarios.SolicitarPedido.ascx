<%@ Control Language="c#" codebehind="AMS.Inventarios.SolicitarPedido.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.SolicitarPedido" %>

<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
    <table class="filters">
        <tbody>
            <tr>
                <th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
            <td>
           
                <td>
                    NIT:<br />
                    <asp:DropDownList id="ddlNIT" AutoPostBack="True" class="dmediano" runat="server" OnSelectedIndexChanged="CambiaNIT"></asp:DropDownList>
                </td>
                <td>
                    Pedido:<br />
                    <asp:DropDownList id="ddlPeds" class="dpequeno" runat="server"></asp:DropDownList>
                </td>
                <td>
                    <asp:Button id="btnCarga" onclick="Cargar" runat="server" Text="Cargar"></asp:Button>
                </td>
          
                </td>
            </tr>
        </tbody>
    </table>
<p>
    <asp:PlaceHolder id="toolsHolder" runat="server" visible="false">
        <table class="tools" width="780">
            <tbody>
                <tr>
                    <td width="16">
                        <img height="30" src="../img/AMS.Flyers.Tools.png" border="0" /></td>
                    <td>
                        Imprimir <a href="javascript: Lista()"><img height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0" /> </a></td>
                    <td>
                        &nbsp; &nbsp;Enviar por correo 
                        <asp:TextBox id="tbEmail" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
                        <asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg" BorderWidth="0px"></asp:ImageButton>
                    </td>
                    <td></td>
                </tr>
            </tbody> 
        </table>
    </asp:PlaceHolder>
</p>
<p>
    <ASP:DataGrid id="dgItems" runat="server" cssclass="datagrid" AutoGenerateColumns="False" GridLines="Vertical"  CellPadding="3">
        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
        <ItemStyle cssclass="item"></ItemStyle>
        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
        <Columns>
            <asp:BoundColumn DataField="ped_tipo" ReadOnly="True" HeaderText="Numero:"></asp:BoundColumn>
            <asp:BoundColumn DataField="ped_ref" ReadOnly="True" HeaderText="Prefijo Pedido:"></asp:BoundColumn>
            <asp:BoundColumn DataField="ped_num" ReadOnly="True" HeaderText="N&#250;mero Pedido:"></asp:BoundColumn>
            <asp:BoundColumn DataField="mite_codigo" ReadOnly="True" HeaderText="Codigo Item:"></asp:BoundColumn>
            <asp:BoundColumn DataField="mite_nombre" ReadOnly="True" HeaderText="Nombre Item:"></asp:BoundColumn>
            <asp:BoundColumn DataField="mite_cant" ReadOnly="True" HeaderText="Cantidad Pedida:" DataFormatString="{0:N}"></asp:BoundColumn>
            <asp:BoundColumn DataField="mite_precio" ReadOnly="True" HeaderText="Precio Unitario:" DataFormatString="{0:C}"></asp:BoundColumn>
            <asp:BoundColumn DataField="mite_piva" ReadOnly="True" HeaderText="Iva:" DataFormatString="{0:N}%"></asp:BoundColumn>
            <asp:BoundColumn DataField="mite_total" ReadOnly="True" HeaderText="Total Item:" DataFormatString="{0:C}"></asp:BoundColumn>
        </Columns>
    </ASP:DataGrid>
</p>
<p>
    <asp:Button id="btnGuarda" onclick="Guardar" runat="server" Text="Crear Archivo" Visible="False"></asp:Button>
    &nbsp;&nbsp;&nbsp;&nbsp; 
    <asp:Button id="btnReiniciar" onclick="Reiniciar" runat="server" Text="Reiniciar" Visible="False"></asp:Button>
</p>
<p>
    <asp:PlaceHolder id="plDownload" runat="server"></asp:PlaceHolder>
</p>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>