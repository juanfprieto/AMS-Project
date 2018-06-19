<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Carga.DemandarCarga.ascx.cs" Inherits="AMS.Carga.AMS_Carga_DemandarCarga1" %>
<script type ="text/javascript" src="../js/AMS.Web.masks.js"></script>
<script type ="text/javascript" src="../js/generales.js"></script>
<script type="text/javascript" src="../js/jquery-ui.js"></script>
<script>
    $(function () {
        //mirar documentacion JQueryUI sobre datepicker
                var fechaVal = $("#<%=TxtFecha.ClientID%>").val();
        $("#<%=TxtFecha.ClientID%>").datepicker();
        $("#<%=TxtFecha.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=TxtFecha.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=TxtFecha.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=TxtFecha.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=TxtFecha.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=TxtFecha.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=TxtFecha.ClientID%>").val(fechaVal);
    });

</script>
 

    <div>
    
   <table>
    <tr>
       <td>
            <asp:Label ID="Label1" runat="server" Text="Placa"></asp:Label>
            <asp:TextBox ID="TxtPlaca" runat="server" Width="100"></asp:TextBox>
       </td>
       <td>
            <asp:Label ID="Label2" runat="server" Text="Clave Secreta" ></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server" Width="100"></asp:TextBox>
       </td>
       <td>
            <asp:TextBox ID="TextBox2" runat="server" Width="200px"></asp:TextBox>
       </td>
       <td>
            
       </td>
    </tr>
   </table>
        <table>
        <tr>
        <td>
        <asp:Label ID="Label3" runat="server" Text="Ciudad Origen de carga"></asp:Label>
            <br /><asp:DropDownList ID="DropDownList1" runat="server" Height="16px" Width="166px">
            </asp:DropDownList>
          </td>  
          <td>
            <asp:Label ID="Label5" runat="server" Text="Fecha de cargue"></asp:Label>
            <br /><asp:TextBox ID="TxtFecha" runat="server" onkeyup="DateMask(this)" placeholder="yyyy-mm-dd" Width="166px"></asp:TextBox>
           </td>
            </tr>
            </table>
            <br /><asp:Label ID="Label4" runat="server" Text="Buscar OFERTAS de carga"></asp:Label>
            <table>
        <tr>
        <td>
            <asp:Label ID="Label12" runat="server" Text="Destino de la Carga"></asp:Label>
            <br />
             <asp:DropDownList ID="DropDownList2" runat="server" Height="18px" Width="262px">
            </asp:DropDownList>

        </td>
        <td>
            <asp:Label ID="Label6" runat="server" Text="Fecha"></asp:Label>
            <br />
             <asp:TextBox ID="TxtFechaDestino" runat="server"></asp:TextBox>
        </td>
        <td>
             <asp:Label ID="Label7" runat="server" Text="Valor"></asp:Label>
             <br />
             <asp:TextBox ID="TxtValorDestino" runat="server"></asp:TextBox>               
            <asp:Button ID="BtnAplicar" runat="server" Text="Aplicar" Width="123px" />
             </td> 
         </tr>
        </table>
            <br /><asp:Label ID="Label8" runat="server" Text="Usted ha APLICADO por las siguentes CARGAS"></asp:Label>
        <br /><asp:TextBox ID="TextBox5" runat="server" Width="614px"></asp:TextBox>
        <asp:Button ID="Button1" CssClass="btnmediano" runat="server" Text="Quitar" Width="123px" />
        <br />
        <br />
        <asp:Label ID="Label9" runat="server" Text="Usted ha sido SELECCIONADO para los siguientes VIAJES"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox6" runat="server" Width="614px"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="Aceptar" Width="123px" />
        <asp:Button ID="Button3" runat="server" Text="Rechazar" Width="123px" />
        <br />
        <br />
        <asp:Label ID="Label10" runat="server" Text="Estado de sus VIAJES APROBADOS en tránsito"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label11" runat="server" Text="Estado"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox7" runat="server" Width="614px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
&nbsp;
        <asp:Button ID="Button4" runat="server" style="margin-left: 0px" Text="Rechazar" Width="74px" />
     </div>
