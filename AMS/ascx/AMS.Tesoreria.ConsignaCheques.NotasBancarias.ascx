<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConsignaCheques.NotasBancarias.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.NotasBancarias" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>

<table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>
				Código Cuenta Corriente :
			</td>

			<td>
				<asp:TextBox id="codigoCC" onclick="ModalDialog(this,'SELECT P.pcue_codigo AS Codigo,P.pcue_numero AS Numero,P.pban_banco AS Codigo_Banco,B.pban_nombre AS Banco FROM pcuentacorriente P,pbanco B WHERE P.pban_banco=B.pban_codigo',new Array())"
					runat="server" ToolTip="Haga Click" ReadOnly="True" Width="72px"></asp:TextBox>
				<asp:RequiredFieldValidator id="validatorCC" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="codigoCC">*</asp:RequiredFieldValidator>
			</td>
			<td>
				Fecha :
			</td>
			<td>
				<img onmouseover="calendario.style.visibility='visible'" onmouseout="calendario.style.visibility='hidden'"
					src="../img/AMS.Icon.Calendar.gif" border="0">
				<table id="calendario" onmouseover="calendario.style.visibility='visible'" style="VISIBILITY: hidden; WIDTH: 109px; POSITION: absolute"
					onmouseout="calendario.style.visibility='hidden'">
					<tbody>
						<tr>
							<td>
								<asp:calendar BackColor=Beige id="calendarioFecha" runat="server" OnSelectionChanged="Cambiar_Fecha"></asp:Calendar>
							</td>
						</tr>
					</tbody>
				</table>
				<asp:TextBox id="fecha" runat="server" Width="92px" onkeyup="DateMask(this)"></asp:TextBox>
				<asp:RegularExpressionValidator id="validatorFecha" runat="server" ErrorMessage="Formato de Fecha Invalido" ControlToValidate="fecha"
					ValidationExpression="\d{4}-\d{2}-\d{2}">*</asp:RegularExpressionValidator>
			</td>
			<td>
				<asp:Button id="aceptar" onclick="aceptar_Click" runat="server" Text="Aceptar"></asp:Button>
			</td>
		</tr>
	</tbody>
</table>

<p>
    <fieldset id ="fldTabla" runat="server" visible="false">
	<asp:DataGrid id="gridNotas" runat="server" ShowFooter="True" CellPadding="3" AutoGenerateColumns="False" onItemCommand="gridNotas_Item" Visible="True">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Cuenta P.U.C. que imputa la nota (inserte todas tantas lineas como requiera)">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="cuenta" runat="server" onclick="ModalDialog(this,'SELECT mcue_codipuc AS Cuenta,mcue_nombre AS Descripcion FROM mcuenta WHERE timp_codigo IN(\'A\',\'P\')',new Array())"
						ReadOnly="true" ToolTip="Haga Click" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor (valor POSITIVO DEBITA (suma) a mi saldo, valor NEGATIVO ACREDITA(resta) de mi saldo ">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VALOR","{0:C}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="valorTxt" onkeyup="NumericMaskE(this,event)" class="AlineacionDerecha" runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:templateColumn HeaderText="Agregar/Remover">
				<ItemTemplate>
					<center>
						<asp:Button id="remover" runat="server" Text="Remover" CommandName="remover" />
					</center>
				</ItemTemplate>
				<FooterTemplate>
					<center>
						<asp:Button id="agregar" runat="server" Text="Agregar" CommandName="agregar" />
					</center>
				</FooterTemplate>
			</asp:templateColumn>
		</Columns>
	</asp:DataGrid>

    
        <span style="CURSOR: pointer; COLOR: royalblue; TEXT-DECORATION: none" onclick="document.getElementById('divContainerExcelOption').style.display='block';">Mostrar Herramienta Carga Excel</span>
        <div id="divContainerExcelOption" style="DISPLAY: none">
		    <legend>Opción de Carga desde Archivo Excel</legend>
		    <table class="tablewhite" cellSpacing="1" cellPadding="1" border="0">
			    <tr>
			    <tr>
				    <td align="right" colSpan="2"><span style="CURSOR: pointer; COLOR: royalblue; TEXT-DECORATION: none" onclick="document.getElementById('divContainerExcelOption').style.display='none';">Ocultar</span></td>
			    <tr>
				    <td colSpan="2">Por favor genere un archivo de excel con las siguientes columnas<br>
					    <li>
					    CUENTA
					    <LI>
					    VALOR
						    <br>
					    Notas :
					    <li>
					    El primer renglón del archivo debe llevar los titulos de las columnas como se 
					    encuentran listados anteriormente.
					    <LI>
					    Ningún campo puede ser vacio y los campos numéricos no deben llevar separadores de miles ni signo de peso. (sólo número ejemplo: 300000)
					    <LI>
					    Debe seleccionar todo el espacio de la tabla  y asignarle&nbsp;el nombre 
					    CUENTA.
					    <LI>
					    Solo utilizar Excel formato xls (.xlsx)
					    <LI style="font-size:15px; color:firebrick; text-decoration-color:orangered">
					    LA CUENTA debe existir en la base de datos.
					    </LI>
                    </td>
			    </tr>
			    <tr>
				    <td width="697">
					    <input id="flArchivoExcel" runat="server" type="file"/></td>
				    <td align="right">
					    <asp:Button id="btnCargar" runat="server" Width="327px" Text="Cargar" onclick="btnCargar_Click1"></asp:Button></td>
			    </tr>
		    </table>
        </div>
    </fieldset>
    <br />
        <p>
            <asp:Label ID="lbError" runat="server" style="color:red"></asp:Label>
        </p>
    
</p>
