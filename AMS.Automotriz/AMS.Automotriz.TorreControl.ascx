<%@ Control Language="c#" codebehind="AMS.Automotriz.TorreControl.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.TorreControl" %>
 <script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
 <script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<fieldset>
	<legend class="Legends">Ordenes de Trabajo Abiertas</legend>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					Prefijo del Documento&nbsp;<asp:DropDownList id="tipoDocumento" runat="server" OnSelectedIndexChanged="Cambio_Documento" AutoPostBack="true"></asp:DropDownList>
				</td>
				<td>
					Número de Ordenes : &nbsp;<asp:DropDownList id="ordenes" runat="server"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<asp:PlaceHolder id="plhGrpCata" runat="server" visible="false">
					<td>Grupo Catálogo : &nbsp;
						<asp:Label id="grupoCatalogo" runat="server"></asp:Label></td>
				</asp:PlaceHolder>
				<td>
					<asp:Button id="cargar" onclick="Cargar_Orden" Width="79px" runat="server" Text="Cargar" CausesValidation="False"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>
<div id="divOperaciones" runat="server">
    <p>
	    <asp:DataGrid id="operaciones" runat="server" AutoGenerateColumns="false" Font-Names="Verdana"
		    BorderWidth="1px" GridLines="Vertical" BorderStyle="None" BackColor="White" BorderColor="#999999"
		    ShowFooter="True" CellPadding="3" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd"
		    OnItemCommand="dgOpcion_Operaciones" OnItemDataBound="dgOperacionesBound">
		    <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		    <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		    <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		    <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		    <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		    <Columns>
			    <asp:TemplateColumn HeaderText="CODIGO DE LA OPERACION">
				    <ItemTemplate>
					    <%# DataBinder.Eval(Container.DataItem, "CODIGOOPERACION") %>
				    </ItemTemplate>
				    <FooterTemplate>
			    <%--		<asp:TextBox id="valToInsertar1" onclick="ModalDialog(this, 'SELECT ptem_operacion AS CODIGO, ptem_descripcion AS NOMBRE, ptem_exceiva AS ExCENCION_Iva FROM ptempario where TVIG_VIGENCIA <> \'N\' and (PTEM_INDIGENERIC = 'S' OR ptem_operacion IN (SELECT PTEM_OPERACION FROM PTIEMPOTALLER PT WHERE PT.PTIE_TEMPARIO = T.PTEM_OPERACION AND PT.PTIE_GRUPCATA = \'"+grupoCatalogo.text+"\')) ORDER BY ptem_descripcion;', new Array())"
		       --%> 		<asp:TextBox id="valToInsertar1" onclick="ModalDialog(this, 'SELECT ptem_operacion AS CODIGO, ptem_descripcion AS NOMBRE, TTIP_DETALIQU AS LIQUIDA, ptem_exceiva AS ExCENCION_Iva, TOPE_NOMBRE AS ESPECIALIDAD  FROM ptempario PT, TTIPOLIQUIDACIONTALLER TL,  TTIPOOPERACIONTALLER OP where TVIG_VIGENCIA <> \'N\' AND PT.TTIP_CODILIQU = TL.TTIP_CODILIQU AND PT.TOPE_CODIGO = OP.TOPE_CODIGO ORDER BY ptem_descripcion;', new Array(),1)"
		   			        ReadOnLy="true" runat="server" Width="100px"></asp:TextBox>
					    <asp:RequiredFieldValidator id="validatorValToInsertar1" runat="server" ControlToValidate="valToInsertar1" Display="Dynamic"
						    Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator>
				    </FooterTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="DESCRIPCION DE LA OPERACION">
				    <ItemTemplate>
					    <asp:Label id="lbDesc" runat="server">
                        <%# DataBinder.Eval(Container.DataItem, "DESCRIPCIONOPERACION") %>
                        </asp:Label>
                        <br>
                        <asp:TextBox id="txtObservaciones" runat="server"  Width="100px"></asp:TextBox>
                        <%# DataBinder.Eval(Container.DataItem, "OBSERVACIONOP") %>
				    </ItemTemplate>
				    <FooterTemplate>
					    <asp:TextBox id="valToInsertar1a" ReadOnLy="true" runat="server" Width="100px"></asp:TextBox><br>
                        <asp:TextBox id="txtObserva"  runat="server" Width="100px" placeholder="Observaciones..."></asp:TextBox>
                        <%--<asp:TextBox id="txtObserva"  runat="server" Width="100px" Text="Observaciones" OnClick="limpiarTxt(this)" OnBlur="ponerTxt(this)"></asp:TextBox>--%>
				    </FooterTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="TÉCNICO">
				    <ItemTemplate>
					    <asp:DropDownList id="mecanicoE" class="dpequeno" runat="server"></asp:DropDownList>
                        <asp:Image id="imglupa1" visible="true" runat="server"  ImageUrl="../img/AMS.Search.png"></asp:Image>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="TIEMPO">
				    <ItemTemplate>
					    <asp:TextBox id="tiempoOperacion" CssClass="AlineacionDerecha" runat="server" Width="50px" OnTextChanged="calcularPrecio" AutoPostBack= "true"></asp:TextBox>
					    <%# DataBinder.Eval(Container.DataItem, "TIEMPO", "{0:N}") %>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="ESTADO">
				    <ItemTemplate>
					    <asp:DropDownList id="estadoE" runat="server"></asp:DropDownList>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="VALOR">
				    <ItemTemplate>
					    <asp:TextBox id="valorOperacion" runat="server" Visible="false" Width="90px" CssClass="AlineacionDerecha"
						    onkeyup="NumericMaskE(this,event);"></asp:TextBox>
					    <%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C2}") %>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="FECHA Y HORA DEL EVENTO">
				    <ItemTemplate>
					    <asp:Label id="fechaOpc" text="Fecha" runat="server"></asp:Label>
					    <asp:TextBox id="fechaOpcion" CssClass="AlineacionDerecha" onkeyup="DateMask(this);" runat="server"
						    Width="80px"></asp:TextBox>
					    <asp:Label id="horaOpc" text="Hora" runat="server"></asp:Label>
					    <asp:TextBox id="horaOpcion" runat="server" Width="80px"></asp:TextBox>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="CARGO DE LA OPERACIÓN">
				    <ItemTemplate>
					    <asp:DropDownList id="ddlCargoOp" runat="server"></asp:DropDownList>
				    </ItemTemplate>
				    <FooterTemplate>
					    <asp:DropDownList id="mecanicoE" runat="server"></asp:DropDownList>  
				    </FooterTemplate>
			    </asp:TemplateColumn>
			
            <%--	esta columna se debe poner visible o editable y no visible o no editable, dependiento que el cargo seleccionado sea G o diferente
		       --%>
                <asp:TemplateColumn HeaderText="INFORMACIÓN GARANTIAS">
				    <ItemTemplate>
					    <table cellpadding="1" border="0" cellspacing="1" class="tablewhite2">
						    <tr>
							    <td>Incidente :</td>
							    <td>
								    <asp:DropDownList id="ddlCodigoIncidente" runat="server"></asp:DropDownList></td>
						    </tr>
						    <tr>
							    <td>Causa de Garantía :</td>
							    <td>
								    <asp:DropDownList id="ddlCausalGarantia" runat="server"></asp:DropDownList></td>
						    </tr>
						    <tr>
							    <td>Remedio:</td>
							    <td>
								    <asp:DropDownList id="ddlCodigoRemedio" runat="server"></asp:DropDownList></td>
						    </tr>
						    <tr>
							    <td>Defecto o Naturaleza :</td>
							    <td>
								    <asp:DropDownList id="ddlCodigoDefecto" runat="server"></asp:DropDownList></td>
						    </tr>
					    </table>
				    </ItemTemplate>
			    </asp:TemplateColumn>
           
                <asp:TemplateColumn HeaderText="OPCI&#211;N">
				    <ItemTemplate>
					    <asp:Button CommandName="removerRestaurar" Text="Remover" ID="btnRmv" CausesValidation="False"
						    runat="server" />
				    </ItemTemplate>
				    <FooterTemplate>
					    <asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" runat="server" />
				    </FooterTemplate>
			    </asp:TemplateColumn>
		    </Columns>
		    <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	    </asp:DataGrid>
    </p>
    <p>
	    <asp:Button id="aceptar" onclick="Guardar_Cambios" Width="153px" runat="server" Text="Aceptar"
		    Enabled="False" CausesValidation="False"></asp:Button>
	    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	    <asp:Button id="btnCancelar" onclick="Cancelar_Torre" Width="153px" runat="server" Text="Cancelar"
		    CausesValidation="False"></asp:Button>
    </p>
    <p>
	    <asp:Label id="lb" runat="server"></asp:Label>
    </p>
</div>
<div id="divCosto" runat="server" visible="false" style="position:absolute; top:100px; left:35%; background: lightgray; border-style:solid; border-width:3px; width:740px; box-shadow: 4px 7px 13px #888888; padding-bottom: 10px; border-radius: 60px; padding-top: 20px; padding-left: 5px;">
    <fieldset id="fldCosto" runat="server" style="margin-top: 10px;">
        <asp:Label ID="lbinfoCambios" runat="server" Text="Se han guardado los cambios,es NECESARIO que llene la siguiente tabla." style="font-size: large;" ></asp:Label><br /><br />
        <asp:DataGrid id="gridTerceros" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False" CellPadding="3" ShowFooter="True">
			                <FooterStyle cssclass="footer"></FooterStyle>
			                <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			                <PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
			                <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			                <ItemStyle cssclass="item"></ItemStyle>
			                <Columns>
				                <asp:TemplateColumn HeaderText="Prefijo Doc">
					                <ItemTemplate>
						                <%# DataBinder.Eval(Container.DataItem, "PREFIJODOC") %>
					                </ItemTemplate>
				                </asp:TemplateColumn>
				                <asp:TemplateColumn HeaderText="Núm. Orden">
					                <ItemTemplate>
						                <%# DataBinder.Eval(Container.DataItem, "NUMORDEN") %>
					                </ItemTemplate>
				                </asp:TemplateColumn>
				                <asp:TemplateColumn HeaderText="Código Op">
					                <ItemTemplate>
						                <%# DataBinder.Eval(Container.DataItem, "CODIGOOP") %>
					                </ItemTemplate>
				                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Nombre Op">
					                <ItemTemplate>
						                <%# DataBinder.Eval(Container.DataItem, "NOMBREOP") %>
					                </ItemTemplate>
				                </asp:TemplateColumn>
				                <asp:TemplateColumn HeaderText="NIT Proveedor">
					                <ItemTemplate>
						                <center>
                                            <asp:TextBox id="txtProveedor" runat="server" placeholder="Nit del proveedor" ReadOnly="true" CssClass="AlineacionDerecha" />
						                </center>
					                </ItemTemplate>
				                </asp:TemplateColumn>
				                <asp:TemplateColumn HeaderText="Costo">
					                <ItemTemplate>
						                <center>
                                            <asp:TextBox id="txtCosto" runat="server" placeholder="sólo números" ReadOnly="false" CssClass="AlineacionDerecha" />
						                </center>
					                </ItemTemplate>
				                </asp:TemplateColumn>
			                </Columns>
		                </asp:DataGrid>
    </fieldset><br />
    <asp:Button id="btnAceptar" runat="server" Text="Aceptar" style="margin: auto; display: -webkit-box;" OnClick="guadarTerceros"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
<%--    <asp:Button id="btnCancela" runat="server" Text="Cancelar" style="margin: auto; display: -webkit-box;" OnClick="cancelarTerceros"></asp:Button><br />--%>
    <asp:Label ID="lbError" runat="server"></asp:Label><br /><br />
</div>


<script language = "javascript" type="text/javascript">
    $(function () {
        var divCst = "<%=divCosto.ClientID%>";
        $("#" + divCst).draggable();
    });
</script>

<script type ="text/javascript">

   
var grupoCatalogo=document.getElementById("<%=grupoCatalogo.ClientID%>");

function capturarGrupoCatalogo(obj){      
                                                                                                                                                                                                                                              
var sqlDsp='SELECT * FROM PTIEMPOTALLER PT WHERE  PT.PTIE_GRUPCATA = \''+grupoCatalogo+'\'';
	 ModalDialog(obj,sqlDsp, new Array(),1)
}



function limpiarTxt(txt)
{
    if(txt.value == 'Observaciones')
        {txt.value = '';}
    return true;
}

function ponerTxt(txt) {
    if (txt.value == '')
    { txt.value = 'Observaciones'; }
    return true;
}


    
</script> 