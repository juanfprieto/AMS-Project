<%@ Control Language="c#" codebehind="AMS.Inventarios.BackOrder.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.AsignacionBackOrder" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript">
function MostrarRefs(obTex,obCmbLin)
{
    ModalDialog(obTex,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER By mite_codigo', new Array());
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
				<table class="filtersIn">
					<tbody>
						<tr>                          
							<td>
                                    <asp:radiobuttonlist id="rblTipoConsulta" AutoPostBack="True" OnSelectedIndexChanged="CambioTipoConsulta"
									BorderWidth="0px" RepeatDirection="vertical" runat="server">
									<asp:ListItem Value="T" Selected="True">Todos</asp:ListItem>
									<asp:ListItem Value="I">Item Específico</asp:ListItem>                                  
									<asp:ListItem Value="P">Tipo de Pedido</asp:ListItem>                              
                                    <asp:ListItem Value="OP">Orden de Producción</asp:ListItem>
									<asp:ListItem Value="OT">Orden de Trabajo Taller</asp:ListItem>
									<asp:ListItem Value="PE">Pedido Específico</asp:ListItem>
								</asp:radiobuttonlist>
							</td>
                            <td>desea asignar vendedores automaticamente
            <asp:CheckBox ID="CheckBox1" runat="server"></asp:CheckBox>

            </td>
						</tr>
						<tr>
							     <td>Prefijo a utilizar cuando sean items de taller :&nbsp;
								<asp:dropdownlist id="ddlPrefTrans" class="dmediano" runat="server"></asp:dropdownlist>
                                </td>
						</tr>
						<asp:placeholder id="plRow1" runat="server" Visible="false">
							<TR>
								<TD>
									<asp:Label id="lbInfo1" runat="server">Item Específico :</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:TextBox id="tbCodigoItem" ondblclick="ModalDialog(this, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(mite_codigo,plin_codigo), plin_codigo, mite_nombre FROM mitems ORDER BY plin_codigo,mite_codigo', new Array());"
										runat="server" class="tmediano"></asp:TextBox>
									<asp:RequiredFieldValidator id="validatorTbCodigoItem" runat="server" ControlToValidate="tbCodigoItem" Display="Dynamic"
										Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>&nbsp;&nbsp; 
									&nbsp;
									<asp:DropDownList id="ddlLinea" class="dmediano" runat="server"></asp:DropDownList></TD>
							</TR>
						</asp:placeholder>
						<asp:placeholder id="plRow2" runat="server" Visible="false">
							<TR>
								<TD>
									<asp:Label id="lbInfo2" runat="server">Tipo de Pedido : </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:DropDownList id="ddlTipoPedido" class="dpequeno" runat="server"></asp:DropDownList></TD>
							</TR>
						</asp:placeholder>
						<asp:placeholder id="PlaceHolderOP" runat="server" Visible="false">
							<TR>
								<TD>
									<asp:Label id="LabelOPP" runat="server">Prefijo Orden :</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:DropDownList id="DropDownListPO" class="dpequeno" runat="server" OnSelectedIndexChanged="SeleccionaPrefijoOrden"
										AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <br />
									<asp:Label id="LabelOPN" runat="server">Número Orden :</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:DropDownList id="DropDownListNO" class="dpequeno" runat="server"></asp:DropDownList>&nbsp;
								</TD>
							</TR>
						</asp:placeholder>
						<asp:placeholder id="plcPE" runat="server" Visible="false">
							<TR>
								<TD>
									<asp:Label id="Label1" runat="server">Prefijo de Pedido : </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:DropDownList id="ddlTPedido" class="dmediano" runat="server" AutoPostBack="True" onselectedindexchanged="ddlTPedido_SelectedIndexChanged"></asp:DropDownList></TD>
							</TR>
							<TR>
								<TD>
									<asp:Label id="Label2" runat="server">Número de Pedido : </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:DropDownList id="ddlNPedido" class="dpequeno" runat="server"></asp:DropDownList></TD>
							</TR>
						</asp:placeholder>
                        
					</tbody>
                 </table>
				<asp:button id="btnRealizar" onclick="RealizarAsignacion" runat="server" Text="Realizar" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
				</asp:button>
                </td>
		</tr>
	</tbody>
    </table>
    </fieldset>

<p><asp:datagrid id="dgBackOrder" runat="server" cssclass="datagrid" ShowFooter="False" ShowHeader="true" PageSize="30" AllowPaging="True" OnPageIndexChanged="DgBackOrderPage"
		HeaderStyle-BackColor="#ccccdd" CellPadding="3" AutoGenerateColumns="False">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle borderwidth="1px" borderstyle="Dotted" 
			position="TopAndBottom" cssclass="pager" mode="NumericPages"></PagerStyle>
		<Columns>
			<asp:BoundColumn DataField="CODIGO" HeaderText="Codigo Item"></asp:BoundColumn>
			<asp:BoundColumn DataField="LINEA" HeaderText="Linea"></asp:BoundColumn>
			<asp:BoundColumn DataField="PEDIDO" HeaderText="Pedido"></asp:BoundColumn>
			<asp:BoundColumn DataField="NIT" HeaderText="Nit de Cliente"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDADPENDIENTE" HeaderText="Cantidad Pendiente"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDADDISPONIBLE" HeaderText="Cantidad Disponible en Almacen"></asp:BoundColumn>
			<asp:BoundColumn DataField="CANTIDADASIGNADA" HeaderText="Cantidad Asignada"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALORTOTAL" HeaderText="Valor Asignación" DataFormatString="{0:C}"></asp:BoundColumn>
			<asp:BoundColumn DataField="LISTAEMPAQUE" HeaderText="ListaEmpaque"></asp:BoundColumn>
		</Columns>
	</asp:datagrid></p>
<p><asp:button id="btnReiniciar" onclick="ReiniciarControl" runat="server" Visible="False" Text="Reiniciar"
		CausesValidation="False"></asp:button></p>
<p><asp:label id="lb" runat="server"></asp:label></p>



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