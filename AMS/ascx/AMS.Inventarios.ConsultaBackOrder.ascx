<%@ Control Language="c#" codebehind="AMS.Inventarios.ConsultaBackOrder.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ConsultaBackOrder" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript">
function MostrarRefs(obTex,obCmbLin)
{
    ModalDialog(obTex,'SELECT DISTINCT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) AS CODIGO, MIT.mite_nombre AS NOMBRE FROM dpedidoitem DPI, mitems MIT, plineaitem PLIN WHERE DPI.mite_codigo = MIT.mite_codigo AND DPI.mped_clasregi=\'C\' AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo AND PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' ', new Array());
}
</script>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
				<img height="70" src="../img/AMS.Flyers.Filters.png" border="0">
			</th>
			<td>
				<table class="filtersIn">
					<tbody>
						<asp:PlaceHolder id="plFil1" runat="server">
							<TR>
								<TD colSpan="2">Consultar Por : &nbsp;
									<asp:DropDownList id="ddlFiltro" class="dmediano" runat="server">
										<asp:ListItem Value="I" Selected="True">Items</asp:ListItem>
										<asp:ListItem Value="P">Pedidos</asp:ListItem>
									</asp:DropDownList>
								</TD>
						    </TR>
							<TR>
								<TD colSpan="3">
									<asp:RadioButtonList id="rdlstFiltro" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="CambioGrupoConsulta"
										AutoPostBack="True">
										<asp:ListItem Value="T" Selected="True">Todos </asp:ListItem>
										<asp:ListItem Value="F">Filtrar Grupo</asp:ListItem>
									</asp:RadioButtonList></TD>
                                    
                                    <TD align="center">
									<asp:Button id="btnConsultar1" onclick="ConsultarFiltro1" runat="server" Text="Consultar" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
									</asp:Button></TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plFil2" runat="server">
							<TR>
								<TD>Codigo Item :&nbsp;
									<asp:TextBox id="tbFiltroCodI" runat="server" Width="180px"></asp:TextBox>
								</TD>
								<TD>Linea Bodega :
									<asp:DropDownList id="ddlLinea" runat="server"></asp:DropDownList></TD>
								<TD align="right">
									<asp:Button id="btnConsultar2" onclick="ConsultarFiltro2" runat="server" Text="Consultar" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
									</asp:Button></TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plFil3" runat="server">
							<TR>
								<TD>Prefijo Pedido :
									<asp:DropDownList id="ddlPrefPed" runat="server" OnSelectedIndexChanged="CambioPrefijoPedido" AutoPostBack="True"></asp:DropDownList></TD>
								<TD>Número Pedido :
									<asp:DropDownList id="ddlNumPed" runat="server"></asp:DropDownList></TD>
								<TD align="right">
									<asp:Button id="btnConsultar3" onclick="ConsultarFiltro3" runat="server" Text="Consultar" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
									</asp:Button></TD>
							</TR>
						</asp:PlaceHolder>
					</tbody>
				</table>
			</td>
		</tr>
	</tbody>
</table>
</fieldset>
<p>
	<asp:DataGrid id="dgConsulta" runat="server"></asp:DataGrid>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
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

