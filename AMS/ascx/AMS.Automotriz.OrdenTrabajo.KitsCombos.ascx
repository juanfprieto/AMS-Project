<%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenesTaller.KitsCombos.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.KitsCombos" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript">
function MostrarItems(/*textbox*/ob,/*string*/obGR,/*dropdownlist*/obCmbLin, /*string*/ listaPrecios)
{
    //ModalDialog(ob,'SELECT DISTINCT DBXSCHEMA.EDITARREFERENCIAS(MITE.mite_codigo,PLIN.plin_tipo) AS CODIGO, MITE.mite_nombre AS DESCRIPCION, TOR.tori_nombre AS ORIGEN, CASE WHEN MITE.mite_codigo NOT IN (SELECT mite_codigo FROM dbxschema.mitemsgrupo WHERE pgru_grupo = \''+obGR.value+'\' AND mig_cantidaduso IS NOT NULL) THEN MITE.mite_usoxvehi WHEN MITE.mite_codigo IN (SELECT mite_codigo FROM dbxschema.mitemsgrupo WHERE pgru_grupo = \''+obGR.value+'\' AND mig_cantidaduso IS NOT NULL) THEN MIG.mig_cantidaduso END AS CANTIDAD, MITE.pdes_codigo AS "CODIGO DE DESCUENTO", MITE.piva_porciva AS "PORCENTAJE DE IVA" FROM dbxschema.mitems MITE, dbxschema.mitemsgrupo MIG, dbxschema.torigenitem TOR, dbxschema.plineaitem PLIN WHERE MITE.tori_codigo = TOR.tori_codigo AND MITE.plin_codigo = PLIN.plin_codigo AND PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND (((MITE.mite_codigo=MIG.mite_codigo AND MIG.pgru_grupo= \''+obGR.value+'\') OR (MITE.mite_indigeneric = \'S\' AND MITE.mite_codigo NOT IN (SELECT mite_codigo FROM dbxschema.mitemsgrupo WHERE pgru_grupo = \''+obGR.value+'\' AND mig_cantidaduso IS NOT NULL))))', new Array());
    //dalDialog(ob,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MITE.mite_codigo,PLIN.plin_tipo) AS CODIGO, MITE.mite_nombre AS DESCRIPCION, TOR.tori_nombre AS ORIGEN, DBXSCHEMA.USOXVEH(MITE.mite_codigo,\''+obGR+'\',\'\') AS USO,MITE.pdes_codigo AS DESCUENTO, MITE.piva_porciva AS IVA FROM dbxschema.mitems MITE, dbxschema.torigenitem TOR, dbxschema.plineaitem PLIN WHERE MITE.tori_codigo = TOR.tori_codigo AND MITE.plin_codigo = PLIN.plin_codigo AND PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\'',new Array(),null,1);
    ModalDialog(ob, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MITE.mite_codigo,PLIN.plin_tipo) AS CODIGO, MITE.mite_nombre AS DESCRIPCION, TOR.tori_nombre AS ORIGEN, ' +
                    'DBXSCHEMA.USOXVEH(MITE.mite_codigo,\''+obGR+'\',\'\') AS USO,MITE.pdes_codigo AS DESCUENTO, MITE.piva_porciva AS IVA ' +
                    'FROM dbxschema.mitems MITE, dbxschema.torigenitem TOR, dbxschema.plineaitem PLIN, mprecioitem mp, pprecioitem pp ' +
                    'WHERE MITE.tori_codigo = TOR.tori_codigo AND MITE.plin_codigo = PLIN.plin_codigo AND PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' and  ' +
                    'mite.mite_codigo=mp.mite_codigo and pp.ppre_codigo = mp.ppre_codigo and pp.ppre_codigo=\''+listaPrecios+'\' ;',new Array(),null,1);
}
</script>
<script type ="text/javascript">
function LimpiarValores(ob1,ob2,ob3,ob4,ob5,ob6)
{
    ob1.value = "";
    ob2.value = "";
    ob3.value = "";
    ob4.value = "";
    ob5.value = "";
    ob6.value = "";
}
</script>
<p>
	Grupo Catálogo :
	<asp:TextBox id="valToInsertar1EX" Enabled="False" ReadOnly="True" class="tpequeno" runat="server"></asp:TextBox>
	<input id="hdcat" runat="server" type="hidden" />
    <input id="hdIndicativoAgregado" runat="server" type="hidden" />
</p>
<fieldset>
	<legend class="Legends">Kits o Combos Disponibles</legend>
		<asp:DataGrid id="kitsCompletos"  runat="server" cssclass="datagrid" OnItemCommand="dgSeleccion_Previo"
			AutoGenerateColumns="false" GridLines="Vertical">
			<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
			<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
			<FooterStyle cssclass="footer"></FooterStyle>
			<Columns>
				<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO"></asp:BoundColumn>
				<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCI&#211;N"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="SELECCIONAR">
					<ItemTemplate>
						<asp:Button CommandName="Seleccionar" Text="Seleccionar" ID="btnSlc" Runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="VER COMBO">
					<ItemTemplate>
						<asp:Literal ID="ltrLink" Runat="server"></asp:Literal>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
			<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
		</asp:DataGrid>
    </fieldset>
	<p>
	</p>    
   <table id="Table1">
    <tr>
     <td class="scrollable">
        <fieldset style="overflow-x:auto;">
	<legend class="Legends">Operaciones</legend>
	<asp:DataGrid id="kitsOperaciones" runat="server" cssclass="datagrid" OnItemCommand="dgSeleccion_Operaciones"
		AutoGenerateColumns="false" GridLines="Vertical" OnDeleteCommand="DgOperaciones_Delete" OnItemDataBound="dgSeleccion_Operaciones_Bound"
		ShowFooter="True">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="CODIGOKIT" HeaderText="Código del Kit"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Código de la Operación">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGOOPERACION") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsertar1" ReadOnLy="true" runat="server" class="tpequeno" onkeyup="aMayusculas(this)"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre de la Operación">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsertar1a" ReadOnLy="true" runat="server" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Observ de la Operac">
				<ItemTemplate>
					<asp:TextBox id="valDescripcion" runat="server" placeholder="Observaciones..."/>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Tiempo Operac">
				<ItemTemplate>
                    <asp:TextBox id="valTiempoEst" runat="server"/>
					<%# DataBinder.Eval(Container.DataItem, "TIEMPOEST", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Operac">
				<ItemTemplate>
					<asp:TextBox id="valOperacion" CssClass="AlineacionDerecha" onkeyup="NumericMaskE(this,event)" runat="server"
						Width="100px" Visible="true" placeholder="Precio SIN IVA"></asp:TextBox>
					<%# DataBinder.Eval(Container.DataItem, "VALOROPERACION", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Exento Iva">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "EXCENTOIVA") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsertar1b" ReadOnLy="true" runat="server" width="37px"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Técnico Asignado">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TECNICO") %>
                    <asp:DropDownList id="mecanicoE" class="dmediano" runat="server" Enabled="true" ></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cargo" >
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CARGO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="cargo" class="dmediano" runat="server" style="font-size:12px;"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Estado">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Código Incidente">
				<ItemTemplate>
					<asp:DropDownList id="valIncidente" class="dpequeno" runat="server" Visible="false"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Causal Garantía">
				<ItemTemplate>
					<asp:DropDownList id="valGarantia" class="dmediano" runat="server" Visible="false"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Código Naturalz">
				<ItemTemplate>
					<asp:DropDownList id="valRemedio" class="dpequeno" runat="server" Visible="false"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Código Defecto">
				<ItemTemplate>
					<asp:DropDownList id="valDefecto" class="dmediano" runat="server" Visible="false"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Opción">
				<ItemTemplate>
					<asp:Button CommandName="Delete" Text="Remover" ID="btnRmv2" runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow2" Text="Agregar" ID="btnAdd2" runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
         </fieldset>
        </td>
    </tr>
    <tr>
        <td class="scrollable">
            <fieldset>   
	<legend class="Legends">Items</legend>
	<asp:DataGrid id="kitsItems" runat="server" cssclass="datagrid" OnItemCommand="dgSeleccion_Items" AutoGenerateColumns="false"
		GridLines="Vertical" OnDeleteCommand="DgItems_Delete" OnItemDataBound="dgSeleccion_Bound" ShowFooter="True">
		<FooterStyle cssclass="footer" ></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle  cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="CODIGOKIT" HeaderText="Código del Kit"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Código del Repuesto">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGOREPUESTO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1" ReadOnLy="true" runat="server" class="tpequeno" onkeyup="aMayusculas(this)"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre del Repuesto">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "REFERENCIA") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1a" ReadOnLy="true" runat="server" class="tpequeno"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Línea Bodega">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "LINEABODEGA") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="ddlLineaBodega" width="150px" runat="server" style="font-size:12px;"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Origen del Repto">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ORIGEN") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1b" ReadOnLy="true" runat="server" class="tpequeno"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Precio Unitario Repto">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PRECIO", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantid del Repto">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CANTIDAD", "{0:N}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1c" ReadOnLy="true" runat="server" class="tpequeno"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Codig de Desct">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1d" ReadOnly="true" runat="server" class="tpequeno"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="% de Iva">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "IVA", "{0:N}%") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valToInsert1e" ReadOnLy="true" runat="server" class="tpequeno"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Total">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALORFACTURADO", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cargo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CARGO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="cargo" class="dmediano" runat="server" style="font-size:12px;"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Opción">
				<ItemTemplate>
					<asp:Button CommandName="Delete" Text="Remover" ID="btnRmv1" runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow1" Text="Agregar" ID="btnAdd1" Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
          </fieldset>
         
        </td>
    </tr>
    </table>
    
<br>
<fieldset>
	<legend class="Legends">Totales</legend>
	<table class="filtersIn">
		<tbody>
			<tr>
				<td>
					Totales :</td>
				<td>
					<asp:TextBox id="total" ReadOnly="True" runat="server" CssClass="AlineacionDerecha">$0</asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Fecha Estimada de Entrega :</td>
				<td>
					<asp:TextBox id="fechaEstimada" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					Hora Estimada de Entrega :</td>
				<td>
					<asp:TextBox id="horaEstimada" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>

<p>
</p>
<p>
	<asp:Button id="confirmar" onclick="Confirmar_Kits" Enabled="true" runat="server" Text="Validar"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
<INPUT id="hdnCita" type="hidden" runat="server">
