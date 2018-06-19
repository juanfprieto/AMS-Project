<%@ Control Language="c#" codebehind="AMS.Inventarios.AdminSugerido.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.AdminSugerido" %>
<style type="text/css">
    table.paginador
    {
        border-width: 0px 0px 0px 0px;
        background-color: white;
    }
</style>


<table class="filters">
	<tbody>
		<tr>
       
			<th class="filterHead">
                <IMG height="60" src="../img/AMS.Flyers.News.png" border="0">
            </th>
           
			<td>
            <fieldset>            
               
				    <p>  
					    Se realizará el pedido sugerido para
					    <asp:Label id="lbFecha" runat="server"></asp:Label>&nbsp;.&nbsp;&nbsp;&nbsp;Por favor verifique 
					    que los parámetros de inventarios se encuentre correctamente definidos.<br />
				    </p>
                 
                <P>
                Desea generar sugerido para todas las líneas del Inventario  ? &nbsp;
                    <asp:CheckBox id="CheckLinea" runat="server" Checked="False" TextAlign="Left"></asp:CheckBox>
                </P
                <p>
                Línea de Inventario a sugerir : 
					<asp:dropdownlist id="ddlLinea" Class="dmediano" AutoPostBack="True" runat="server" onselectedindexchanged="ddlLinea_SelectedIndexChanged"></asp:dropdownlist>            
					<asp:Button id="btnCrear" onclick="CrearSugerido" runat="server" Text="Crear Sugerido" UseSubmitBehavior="false" 
                    OnClientClick="clickOnce(this, 'Cargando...')"></asp:Button>

				</p>
           </fieldset>
          </td>
			
		</tr>
        
        		<tr>
			<th class="filterHead">                 
            </th>
			<td>
				<table class="filters">
					<tbody>
						<tr>
							<td>
								Tipo de Sugerido :<br />
							
								<asp:DropDownList id="ddlSugerido" class="dpequeno" runat="server"></asp:DropDownList>
							</td>
						
							<td>
								Filtrar Por :</td>
							<td>
								<asp:RadioButtonList id="rblGrupoItems" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CambioFiltro"
									CssClass="main" RepeatDirection="Horizontal">
									<asp:ListItem Value="T" Selected="True">Todos</asp:ListItem>
									<asp:ListItem Value="P">Filtrar Por Proveedor</asp:ListItem>
								</asp:RadioButtonList>
							</td>
						</tr>
						<asp:PlaceHolder id="plProveedor" runat="server" Visible="False">
							<TR>
								<TD>Proveedor (Registrado en la maestra de items):
								</TD>
								<TD align="right">
									<asp:DropDownList id="ddlProveedor" runat="server"></asp:DropDownList></TD>
							</TR>
						</asp:PlaceHolder>
						<tr>
							<td align="right" colspan="2">
								<asp:Button id="btnConsultar" onclick="ConsultarSugerido" runat="server" Text="Consultar" UseSubmitBehavior="false" 
 OnClientClick="clickOnce(this, 'Cargando...')"></asp:Button>
							</td>
						</tr>
					</tbody>
				</table>
			</td>
		</tr>

	</tbody>
</table>
<p>
</p>
<p>
	<table class="filters">
		<tbody>
			<tr>
				<th width="16">
					<img height="30" src="../img/AMS.Flyers.Tools.png" border="0">
                    </th>
				<td>
					<%--Imprimir <a href="javascript: Lista()"><img height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0" >
					</a>--%>

                    <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel" OnClick="ImprimirExcelGrid" runat="server"
                                    alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.Printer.png" BorderWidth="0px" Width="60px">
                                </asp:ImageButton>
                   
				</td>
				<td>
					&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox>
				</td>
				<td>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail1.png"
						BorderWidth="0px"></asp:ImageButton>
				</td>
				<td width="380"></td>
			</tr>
		</tbody>
	</table>
</p>

<asp:PlaceHolder id="plReport" runat="server">
	<P>
		<asp:Label id="lbInfo" runat="server"></asp:Label></P>
	<P>
		<ASP:DataGrid id="dgSugerido" runat="server" cssclass="datagrid" CellPadding="3" 
			BorderStyle="None" GridLines="Vertical" AutoGenerateColumns="False">
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="ite,"></ItemStyle>
			<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="CODIGO" HeaderText="Codigo Item"></asp:BoundColumn>
				<asp:BoundColumn DataField="NOMBRE" HeaderText="Descripci&#243;n Item"></asp:BoundColumn>
                <asp:BoundColumn DataField="ABC"    HeaderText="Abc"></asp:BoundColumn>
				<asp:BoundColumn DataField="QACTUAL" HeaderText="Cantidad Actual" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="QASIGNADA" HeaderText="Cantidad Asignada" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="QSUGERIDO" HeaderText="Cantidad Sugerida" DataFormatString="{0:N}"></asp:BoundColumn>
			    <asp:BoundColumn DataField="COSTOPROMEDIO" HeaderText="Costo Promedio" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:BoundColumn DataField="VALORSUGERIDO" HeaderText="Valor del Sugerido" DataFormatString="{0:C}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAPROMEDIO" HeaderText="Demanda Promedio" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="QTRANSITO" HeaderText="Cantidad en Transito" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="QBACKORDER" HeaderText="Cantidad BackOrder" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES12" HeaderText="Demanda mes 12" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES11" HeaderText="Demanda mes 11" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES10" HeaderText="Demanda mes 10" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES9" HeaderText="Demanda mes 9" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES8" HeaderText="Demanda mes 8" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES7" HeaderText="Demanda mes 7" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES6" HeaderText="Demanda mes 6" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES5" HeaderText="Demanda mes 5" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES4" HeaderText="Demanda mes 4" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES3" HeaderText="Demanda mes 3" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES2" HeaderText="Demanda mes 2" DataFormatString="{0:N}"></asp:BoundColumn>
				<asp:BoundColumn DataField="DEMANDAMES1" HeaderText="Demanda mes 1" DataFormatString="{0:N}"></asp:BoundColumn>
			</Columns>
		</ASP:DataGrid></P>
	<P>
		<asp:Label id="lbInfo2" runat="server"></asp:Label></P>
</asp:PlaceHolder>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>


<script type ="text/javascript">
    function clickOnce(btn, msg) {
        // Comprobamos si se está haciendo una validación
        if (typeof (Page_ClientValidate) == 'function') {
            // Si se está haciendo una validación, volver si ésta da resultado false
            if (Page_ClientValidate() == false) { return false; }
        }

        // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
        if (btn.getAttribute('type') == 'button') {
            // El atributo msg es totalmente opcional. 
            // Será el texto que muestre el botón mientras esté deshabilitado
            if (!msg || (msg = 'undefined')) { msg = 'Procesando..'; }

            btn.value = msg;

            // La magia verdadera :D
            btn.disabled = true;
        }

        return true;
    }
        </script>
