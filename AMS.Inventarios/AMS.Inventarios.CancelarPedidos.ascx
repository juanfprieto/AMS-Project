<%@ Control Language="c#" codebehind="AMS.Inventarios.CancelarPedidos.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.CancelarPedidos" %>
<fieldset> 
    <TABLE id="Table1" class="filtersIn">
        <tbody>
          
            <TR>
                <td>
                         Cod. Pedido:<br />
                         <asp:DropDownList id="ddlCodPedido" OnSelectedIndexChanged="CambioPrefijo" AutoPostBack="True" class="dmediano" runat="server"></asp:DropDownList>
                  <br />
                        Num. Pedido:<br />
                        <asp:DropDownList id="ddlNumPedido" OnSelectedIndexChanged="CambioNumero" AutoPostBack="True" class="dpequeno" runat="server"></asp:DropDownList>
                 <br />
                        Tipo Pedido:<br />
                        <asp:Label id="lbTipPedido" runat="server">
                        </asp:Label>&nbsp;
                </td>
                             
                
                </tr>
                <asp:PlaceHolder id="plListasEmpaque" runat="server">
                <tr>
             
                <TD>
                <P>
                <asp:DataGrid id="dgListas" runat="server" cssclass="tagrid" CellPadding="3" 
                GridLines="Vertical"  AutoGenerateColumns="False">
                <FooterStyle cssclass="footer"></FooterStyle>
                <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
                <ItemStyle cssclass="item"></ItemStyle>
                <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
                <Columns>
                <asp:BoundColumn DataField="LISTAPRECIO" HeaderText="Listas De Empaque Asociadas"></asp:BoundColumn>
                </Columns>
                <PagerStyle forecolor="Black" cssclass="pager" mode="NumericPages"></PagerStyle>
                </asp:DataGrid></P>
                </TD>
                </tr>
                </asp:PlaceHolder>
               <tr>
                <td>
                <asp:Button id="btnCancelar" onclick="CancelarPedido" runat="server" Text="Cancelar Pedido" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
                </asp:Button>
                </td>
                 </TR> 
  
       
   </tbody>
  </table>
 </fieldset> 
	
<asp:Label id="lb" runat="server"></asp:Label>


<script language:javascript>
      function clickOnce(btn, msg)
        {
            // Comprobamos si se está haciendo una validación
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Procesando...'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
       </script>