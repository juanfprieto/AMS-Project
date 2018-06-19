<%@ Control Language="c#" codebehind="AMS.Inventarios.ListasEmpaque.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ListasEmpaque" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>    
  <table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<table class="main">
					<tbody>
						<asp:PlaceHolder id="plRow1" runat="server" visible="false">
							<TR>
								<TD>
									<P>Nit :<br>
										<asp:TextBox id="tbNit" ondblclick="ModalDialog(this,'**NITS_CLIENTELISTAEMPAQUE', new Array());"
										   class="tpequeno" runat="server" ontextchanged="buscarCliente" autopostback="true"></asp:TextBox>
										<asp:RequiredFieldValidator id="ValidatorTbNit" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
											ControlToValidate="tbNit">*</asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
										<br>
                                        Nombre :<br>
										<asp:TextBox id="tbNita" class="tmediano" runat="server" ReadOnly="True"></asp:TextBox></P>
								</TD>
								<TD>
									<P align="right">&nbsp;
										<asp:Button id="btnCargarListas" onclick="CargarLista" runat="server" Text="Cargar Listas" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
										</asp:Button></P>
								</TD>
							</TR>
						</asp:PlaceHolder>
						<asp:PlaceHolder id="plRow3" runat="server" visible="false">
						    <TR>
			                    <TD colSpan="2">Cupo:<asp:textbox id="txtCupo" class="tpequeno" runat="server" ReadOnly="True"></asp:textbox>&nbsp;
				                    Saldo Cartera:<asp:textbox id="txtSaldoCartera" class="tpequeno" runat="server" ReadOnly="True"></asp:textbox>&nbsp;
				                    Saldo Cartera Mora:<asp:textbox id="txtSaldoMoraCartera" class="tpequeno" runat="server" ReadOnly="True"></asp:textbox></TD>
		                    </TR>
		                </asp:PlaceHolder>
		                <asp:PlaceHolder id="plRow2" runat="server" visible="false">
							<TR>
								<TD>
									<asp:Label id="lbInfo1" runat="server">Numero lista de empaque :</asp:Label>&nbsp;&nbsp;&nbsp;
									<asp:DropDownList id="ddlNLis" class="dmediano" runat="server"></asp:DropDownList>
                                </TD>
							</TR>
                                    <p><ASP:DATAGRID id="dgItems" runat="server" cssclass="datagrid" ShowFooter="True"
		                            AutoGenerateColumns="false" >
                                    <FooterStyle cssclass="footer"></FooterStyle>
		                            <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
                                    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		                            <ItemStyle cssclass="item"></ItemStyle>
		                            <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		                            <Columns>
                                        <asp:BoundColumn DataField="mlis_numero" ReadOnly="True" HeaderText="# lis empaque:"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="mite_codigo" ReadOnly="True" HeaderText="Pedido:"></asp:BoundColumn>                           
			                            <asp:TemplateColumn HeaderText="Operaciones :">
                                            <HeaderTemplate>
                                                <center>
                                                    <asp:Label ID="lblChk" runat="server" Text="Selección"></asp:Label><br />
                                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="True" checked/>
                                                </center>
                                            </HeaderTemplate>
			                                <ItemTemplate>
                                                <center>
                                                    <asp:CheckBox ID="cbRows" runat="server"/>
                                                </center>
                                            </ItemTemplate>
			                            </asp:TemplateColumn>
		                            </Columns>
		                            <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	                            </ASP:DATAGRID></p>
									<P align="left">
										<asp:Button id="btnFacturar" onclick="Facturar" runat="server" Text="btnFacturar" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')" >
										</asp:Button></P>
						</asp:PlaceHolder>
					</tbody>
				</table>
				<asp:Label id="lb" runat="server"></asp:Label></td>
		</tr>
	</tbody>
</table>
</fieldset> 

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