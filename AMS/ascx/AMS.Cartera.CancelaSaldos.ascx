<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Cartera.CancelaSaldos.ascx.cs" Inherits="AMS.Finanzas.AMS_Cartera_CancelaSaldos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ='text/javascript'>
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
 function CheckAll(Checkbox, idGrid) {
        var gridItems;
        if (idGrid == 1)
            gridItems = document.getElementById("<%=DATAGRIDFACTURAS.ClientID %>");
        else if (idGrid == 2)
            gridItems = document.getElementById("<%=DATAGRIDFACTURAS.ClientID %>");

        for (i = 1; i < gridItems.rows.length; i++) {
            if(gridItems.rows[i].cells[6].getElementsByTagName("INPUT")[0].disabled == false)
                gridItems.rows[i].cells[6].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
        }
    }
</script>
<fieldset>
    <table id="Table1" class="filtersIn">
        <tr>
            <td>
            <fieldset>    0
                    PROCESO DE CANCELACION DE FACTURAS DE MENOR CUANTIA:<br>
                    <P>En este proceso se cancelan los saldos de cartera de aquellas facturas cuyo 
                    saldo esten por debajo del siguiente parametro:</P>
                    </fieldset>    
            </td>
        </tr>   
    </table>
<p>
    
</p>
 <table id="Table2" class="filtersIn">
 <tr>
 <td>
	<asp:PlaceHolder id="psaldos" runat="server"><div>
<P><asp:Label ID="Label1" runat="server" Text="Valor frontera de saldos a cancelar"></asp:Label>
&nbsp;&nbsp;<br />
	<asp:TextBox id="tbTope" runat="server" class="tpequeno"></asp:TextBox>
	</P>
   </div></asp:PlaceHolder> 
   </td>
   </tr>
   <tr><td>
   <asp:PlaceHolder   id="pdocumento" runat="server"><div>
	<P><asp:Label     ID="Label3" runat="server" Text="Documento a cancelar:"></asp:Label>&nbsp;&nbsp;  <br /> 
	<asp:DropDownList id="DDLDocumento" runat="server" class="dmediano"></asp:DropDownList>
	</P>
	</div></asp:PlaceHolder> 
    </td>
    </tr>
<P></P>
<tr>
<td>
 <asp:PlaceHolder id="pcancela" runat="server"><div>
<P><asp:Label ID="Label2" runat="server" Text="Documento soporte de la cancelación"></asp:Label>&nbsp;&nbsp; <br />
	<asp:DropDownList id="DDLPrefijo" runat="server" class="dmediano"></asp:DropDownList></P>
<P>
</div></asp:PlaceHolder> 
</td>
</tr>            

</table>
	<asp:Button id="Efectuar" runat="server" Text="Buscar Facturas" onclick="Efectuar_Click" UseSubmitBehavior="false" 
 OnClientClick="clickOnce(this, 'Cargando...')">
	</asp:Button>
    </td>
<P>&nbsp;</P>
</tr>
</tbody>
</table>
</FIELDSET>
<P>
</P>
</fieldset>


	<asp:placeholder id="toolsHolder" runat="server">
		<table>
        <tbody>
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>                
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail.jpg"
						alt="Enviar por email" BorderWidth="0px"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
            </tbody>
		</TABLE>
	</asp:placeholder><asp:placeholder id="phGrilla" runat="server">


<fieldset>
        <P>Facturas a cancelar 
        <P></P>
        <P>
			        <asp:DataGrid id="DATAGRIDFACTURAS" runat="server" cssclass="datagrid" AutoGenerateColumns="False">
				        <FooterStyle cssclass="footer"></FooterStyle>
				        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
				        <PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
				        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
				        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
				        <Columns>
					        <asp:BoundColumn DataField="PREFIJO" HeaderText="PREFIJO"></asp:BoundColumn>
					        <asp:BoundColumn DataField="NUMERO"  HeaderText="NUMERO"></asp:BoundColumn>
					        <asp:BoundColumn DataField="NIT"     HeaderText="NIT"></asp:BoundColumn>
					        <asp:BoundColumn DataField="NOMBRE"  HeaderText="NOMBRE"></asp:BoundColumn>
					        <asp:BoundColumn DataField="FECHA"   HeaderText="FECHA"></asp:BoundColumn>
					        <asp:BoundColumn DataField="SALDO"   HeaderText="SALDO" DataFormatString="{0:C}"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Selección" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <center>
                                        <asp:Label ID="lblChk" runat="server" Text="Selección"></asp:Label><br />
                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAll(this,1);" />
                                    </center>
                                </HeaderTemplate>
			                    <ItemTemplate>
                                    <asp:CheckBox ID="cbRows" runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateColumn>   
				        </Columns>
			        </asp:DataGrid></P>
                    </fieldset>

        <P></P>
        <P></P></asp:placeholder>
        <P></P>
        <P>
	        <asp:Button id="Procesar" runat="server" Text="Efectuar Proceso" Enabled="False" onclick="Procesar_Click" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')">
	        </asp:Button></P>
    
<asp:Label ID="lbInfo" runat="server" Text=""></asp:Label>

<script type ="text/javascript">
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
                if (!msg || (msg='undefined')) { msg = 'Procesando..'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
        </script>
