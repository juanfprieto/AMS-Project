<%@ Control Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.DBManager.MasterTable.ascx.cs" Inherits="AMS.DBManager.MasterTable" %>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
        	<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
			<td>
				<asp:Table id="tbFilters" runat="server" CssClass="main" CellSpacing="10" Width="257px"></asp:Table>
			</td>
			<td>
			    <asp:Button id="btnVerificar" runat="server" Text="Verificar Información" 
                onclick="btnVerificar_Click" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
                </asp:Button>
			</td> 
		</tr>
	</tbody>
</table>
</fieldset>
<p>
    <fieldset>
	    <asp:Table id="tbOpciones" runat="server" CellSpacing="10" class="filters">
        </asp:Table>
    </fieldset>
</p>
<fieldset>
<p>
	<asp:PlaceHolder id="toolsHolder" runat="server">
		<table class="filters">
        <tbody>
			<TR>
				<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
           
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" ImageUrl="../img/AMS.Icon.Mail.jpg"
						alt="Enviar por email"></asp:ImageButton></TD>
				<TD width="380"></TD>
                </TR> 
			
            </tbody>
		</TABLE>
	</asp:PlaceHolder>
</p>
</fieldset>
<asp:PlaceHolder id="plhReport" runat="server">
	<TABLE class="main">
		<TR>
			<TD align="center">
				<asp:Table id="tbHeader" runat="server"></asp:Table></TD>
		</TR>
		<TR>
			<TD>
				<ASP:DataGrid id="dgReport" CellSpacing="1" runat="server" BorderWidth="2px" BorderStyle="Ridge"
					EnableViewState="False" width="690px" BorderColor="White" GridLines="None" BackColor="White"
					CellPadding="3">
					<FooterStyle forecolor="Black" backcolor="#C6C3C6"></FooterStyle>
					<HeaderStyle font-bold="True" forecolor="#E7E7FF" backcolor="#4A3C8C"></HeaderStyle>
					<PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#C6C3C6"></PagerStyle>
					<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#9471DE"></SelectedItemStyle>
					<AlternatingItemStyle backcolor="#F0F0F0"></AlternatingItemStyle>
					<ItemStyle forecolor="Black" backcolor="#DEDFDE"></ItemStyle>
				</ASP:DataGrid></TD>
		</TR>
	</TABLE>
</asp:PlaceHolder>
<p>
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p>


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