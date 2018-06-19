<%@ Control Language="c#" codebehind="AMS.Inventarios.AjustesInv.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.AjustesInv" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script type="text/javascript">
    function MostrarRefs(obTex) {
        ModalDialog(obTex, 'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE MIT.plin_codigo=PLIN.plin_codigo ORDER By mite_codigo', new Array());
    }
    function Cambio_nombre_CallBack(ob) {
        var textNom = ob.value.split("-");
       $("#" + textNom[1]).val(textNom[0]);
    }

    function cargarNombre(objeto, textNom) {
        var id = $(textNom).attr("ID");
        return AjustesInv.Cambio_nombre(objeto.value, id, Cambio_nombre_CallBack);
    }
     
  </script>

<fieldset>
	<legend class="Legends">Inventario:</legend> 
	<table id="Table1" class="filtersIn">
		<tbody>
			<tr>
				<td>
					<p>
						Cod. Documento:<br>
						<asp:DropDownList id="ddlCodDoc" class="dmediano" runat="server" OnSelectedIndexChanged="ChangeDocument" AutoPostBack="True"></asp:DropDownList>
					</p>
				</td>
				<td>
					<p>
					  Num. Documento:
						<br>
						
						<asp:Label id="lbNumDoc" runat="server"></asp:Label>
					</p>
				</td>
                </tr>
                <tr>
                <td>
					Observaciones :<br>
					<asp:TextBox id="txObservaciones" runat="server" TextMode="MultiLine" class="amediano" ></asp:TextBox>
				</td>
			    <td>
					<p>
						Almacén:<br>
						<asp:DropDownList id="ddlAlmacen" class="dmediano" runat="server"></asp:DropDownList>
					</p>
				</td>
                </tr>

			    <tr>
				<td>
					Prefijo Documento Ref :<br />
					<asp:TextBox id="tbPrefDocRef" runat="server" class="tpequeno"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorTbPrefDocRef" runat="server" Font-Size="11" Font-Name="Arial" ControlToValidate="tbPrefDocRef" Display="Dynamic">*</asp:RequiredFieldValidator>
				</td>
           		<td>
					Número Documento Ref :<br />
					<asp:TextBox id="tbPrefNumRef" runat="server" class="tpequeno"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorTbPrefNumRef" runat="server" Font-Size="11" Font-Name="Arial" ControlToValidate="tbPrefNumRef" Display="Dynamic">*</asp:RequiredFieldValidator>
				</td>
			</tr>
				
			<tr>
				<td>
					<p>
					 Centro de Costo:<br>					
						<asp:DropDownList id="ddlCentro" class="dmediano" runat="server"></asp:DropDownList>
					</p>
				</td>
				<td>
					<p>
				 Responsable:<br>		
						<asp:DropDownList id="ddlVendedor" class="dmediano" runat="server"></asp:DropDownList>
					</p>
				</td>
			</tr>
			<tr>
				<td>
					<p>
						Fecha:&nbsp;&nbsp;<br /><asp:TextBox id="tbDate" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
						<img onmouseover="calendar.style.visibility='visibl" onmouseout="calendar.style.visibility='hidden'" src="../img/AMS.Icon.Calendar.gif" border="0">
						<table id="calendar" onmouseover="calendar.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute" onmouseout="calendar.style.visibility='hidden'">
							<tbody>
								<tr>
									<td>
										<asp:calendar BackColor=Beige id="calDate" runat="server" OnSelectionChanged="ChangeDate" enableViewState="true"></asp:Calendar>
									</td>
								</tr>
							</tbody>
						</table>
					</p>
				</td>
                <td>
                    <p>En la columna CANTIDAD digite signo - (menos = NEGATIVO) antes de la cantidad para registrar ajustes NEGATIVOS y SIN signo para registrar ajustes POSITIVOS.  La columna Valor Unidad NO LLEVA SIGNO. En Valor Unidad digite el costo, para Ajustes Negativos se toma el Costo Promedio del item</p>
                </td>
			</tr>
		</tbody>
	</table>
</fieldset>
			
<br>
<form name="dgFrm">
<fieldset>
	<p>
		<ASP:DataGrid id="dgItems" runat="server" OnItemDataBound="DgItemsDataBound" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="false" OnItemCommand="DgInserts_AddAndDel" OnCancelCommand="DgInserts_Cancel" OnEditCommand="DgInserts_Edit" OnUpdateCommand="DgInserts_Update" class="datagrid"  onblur="tap">
			<FooterStyle cssclass="footer"></FooterStyle>
			<HeaderStyle cssclass="header"></HeaderStyle>
		    <SelectedItemStyle cssclass="selected"></SelectedItemStyle>
			<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="items"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Código:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_codigo", "{0:N}") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="valToInsert1" runat="server" onclick="true" onBlur="tap" class="tmediano"></asp:TextBox>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Nombre:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_nombre") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="valToInsert1a" ReadOnLy="true" runat="server" onBlur="tap" class="tmediano"></asp:TextBox>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Línea:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "mite_linea") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:DropDownList id="ddlLineaBdg" class="dmediano" runat="server"></asp:DropDownList>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Cantidad:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "msal_cantasig", "{0:N}") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="valToInsert2" ReadOnLy="false" class="tpequeno" runat="server" Text="1"></asp:TextBox>
					</FooterTemplate>
					<EditItemTemplate>
						<asp:TextBox runat="server" id="edit_1" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "msal_cantasig") %>' />
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor Unidad:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "msal_costprom", "{0:C}") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="valToInsert3" class="tmediano" onkeyup="NumericMaskE(this,event)" runat="server"></asp:TextBox>
					</FooterTemplate>
					<EditItemTemplate>
						<asp:TextBox runat="server" id="edit_2" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "msal_costprom","{0:N}") %>' />
					</EditItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor Total:">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "msal_tot", "{0:C}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Operaciones:">
					<ItemTemplate>
						<p align="center">
							<asp:Button CommandName="DelDatasRow" Text="Quitar" ID="btnDel" Runat="server" class="bpequeno" />
						</p>
					</ItemTemplate>
					<FooterTemplate>
						<p align="center">
							<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" class="bpequeno" />
							<br />
							<asp:Button CommandName="ClearRows" Text="Reiniciar" ID="btnClear" Runat="server" class="bpequeno" />
						</p>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
			</Columns>
		</ASP:DataGrid>
        <br />
        &nbsp &nbsp &nbsp &nbsp
        <span style="CURSOR: pointer; COLOR: royalblue; TEXT-DECORATION: none" onclick="document.getElementById('divContainerExcelOption').style.display='block';">Mostrar Herramienta Carga Excel</span>
        <div id="divContainerExcelOption" style="DISPLAY: none">
	        <fieldset>
		        <legend>Opción de Carga desde Archivo Excel</legend>
		        <table class="tablewhite" cellSpacing="1" cellPadding="1" border="0">
			        <tr>
			        <tr>
				        <td align="right" colSpan="2"><span style="CURSOR: pointer; COLOR: royalblue; TEXT-DECORATION: none" onclick="document.getElementById('divContainerExcelOption').style.display='none';">Ocultar</span></td>
			        <tr>
				        <td colSpan="2">Por favor genere un archivo de excel con las siguientes columnas<br>
					        <li>
					        CODIGO : Código del Item.
					        <LI>
					        NOMBRES : Nombres del Item.
					        <LI>
					        LINEA : Linea del Item.
					        <LI>
					        CANTIDAD : Cantidad asignada
					        <LI>
					        VALOR UNIDAD : Valor Unitario
						        <br>
					        Notas :
					        <li>
					        El primer renglón del archivo debe llevar los titulos de las columnas como se 
					        encuentran listados anteriormente.
					        <LI>
					        Ningún campo puede ser vacio y los campos numéricos no deben llevar separadores de miles ni signo de peso.
					        <LI>
					        Debe seleccionar todo el espacio de la tabla  y asignarle&nbsp;el nombre 
					        TABLA.
					        <LI>
					        Solo utilizar Excel de formato xlsx (.xlsx)
					        <LI style="font-size:15px; color:firebrick; text-decoration-color:orangered">
					        El còdigo del item debe existir en la base de datos.
                            <LI>
                                Cambie el formato de la columna CODIGO a Texto (Esto evita que se desaparezcan 
                                los ceros(0) de un código numérico, ejemplo: '001')
					        </LI>
                            
                        </td>
			        </tr>
			        <tr>
				        <td width="697">
					        <input id="flArchivoExcel" runat="server" type="file"/></td>
				        <td align="right">
					        <asp:Button id="btnCargar" runat="server" Width="327px" Text="Cargar" onclick="btnCargar_Click"></asp:Button></td>
			        </tr>
		        </table>
	        </fieldset>
        </div>
      </fieldset>
	</p>
    

	<fieldset>
		Totales:
		<table id="Table1" class="filtersIn">
			<tbody>
				<tr>
					<td>
						Total Inventario:
					<br />
						<asp:TextBox id="txtTotal" runat="server" ReadOnly="True" class="tpequeno"></asp:TextBox>
					</td>
								
					<td>
						Numero Items:
					<br />
						<asp:TextBox id="txtNumItem" runat="server" ReadOnly="True" class="tpequeno"></asp:TextBox>
					</td>
								
					<td>
						Numero Unidades:
					<br />
						<asp:TextBox id="txtNumUnid" runat="server" ReadOnly="True" class="tpequeno"></asp:TextBox>
					</td>
								
                    <td>
	                    <p>
		                    <asp:Button id="btnAjus"  onclick="NewAjust" runat="server" Text="Ajustar Inventario"></asp:Button>
	                    </p>
                    </td>
                </tr>
			</tbody>
		</table>
	</fieldset>
	<p>

		<asp:Label id="lbInfo" runat="server"></asp:Label>
		<br>
	</p>
</form>
