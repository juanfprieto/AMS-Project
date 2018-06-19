<%@ Control Language="c#" codebehind="AMS.Automotriz.ControlKit.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.ControlKit" %>
<link rel="stylesheet" href="../css/tabber.css" TYPE="text/css" MEDIA="screen">
<!doctype html />
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<fieldset>
	<legend>Información General Sobre el Kit</legend>
	<table class="filtersIn" cellSpacing="5" cellPadding="2" border="0">
		<tbody>
			<tr> 
				<td>Código del Kit :<br>
					<asp:textbox id="tbCodigoKit" runat="server" MaxLength="10" Enabled="False"></asp:textbox>
                    <asp:requiredfieldvalidator id="validatorTbCodigoKit" runat="server" ControlToValidate="tbCodigoKit" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator>&nbsp;
                </td>
				<td>Descripción del Kit :<br>
                    <asp:textbox id="tbNombreKit" runat="server" MaxLength="60"></asp:textbox>
					<asp:requiredfieldvalidator id="validatorTbNombreKit" runat="server" ControlToValidate="tbNombreKit" Display="Dynamic"
						Font-Size="11" Font-Name="Arial">*</asp:requiredfieldvalidator>&nbsp;
                </td>
			</tr>
			<tr>
				<td>Grupo de Catálogo :<br>
					<asp:dropdownlist id="ddlGrupos" runat="server" Enabled="true"></asp:dropdownlist>
                </td>
				<td>Lista de Precios :<br>
					<asp:dropdownlist id="ddlListasPrecios" runat="server"></asp:dropdownlist>
                </td>
                </tr>
                <tr>
                    <td>Kilometraje Específico de Aplicación (si Aplica) :&nbsp;&nbsp;
                                <asp:TextBox id="TextBoxKms" class="tpequeno" MaxLength="10" runat="server" ></asp:TextBox>
           
				    </td>
                    <td>Meses de aplicación repetitiva del Kit (si Aplica) :
                                <asp:TextBox id="TextBoxMeses" class="tpequeno" MaxLength="4" runat="server" type="number" ></asp:TextBox>		
		  		</td>
			    </tr>
                <tr>
                 <td><i>Especifíque un kilometraje si el kit se aplica <u>exclusivamente</u> en ese Kilometraje ó defina cada cuantos meses se aplica este Kit <u>si es repetitivo</u> por tiempo (NO por recorrido), 
                        de lo contrario deje en blanco y parametrice las aplicaciones del KIT en <b>Plan de Mantenimiento Programado</b></i> 
                 </td>
                </tr>
                <tr>
                 <td>Kit Vigente:
                    <asp:CheckBox  id ="chkVigencia" runat ="server" Enabled ="true"></asp:CheckBox>
                    </td>
                    </tr>
		</tbody>
    </table>
</fieldset>
<p></p>

<fieldset>
	<legend>Items Relacionados con el Kit</legend>
	<asp:datagrid id="dgItems" OnItemCommand="DgItems_Event" cssclass="datagrid" runat="server"
		OnDeleteCommand="DgItems_Delete" OnItemDataBound="DgItems_DataBound" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="False">
		<FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Item">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ITEM") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbItem" runat="server" ReadOnly="true" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Descripci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbItema" runat="server" ReadOnly="true" class="tgrande"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad Usada">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CANTIDAD", "{0:n}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Item Genérico">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ITEM GENERICO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones">
				<ItemTemplate>
					<asp:Button CommandName="Delete" CausesValidation="False" Text="Borrar" ID="btnDel" Runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
		
	</asp:datagrid>
</fieldset>
		
<fieldset>
	<legend>Operaciones Relacionadas con el Kit</legend>
	<asp:datagrid id="dgOperaciones" runat="server" OnItemCommand="DgOperaciones_Event"  cssclass="datagrid" 
		OnDeleteCommand="DgOperaciones_Delete" OnItemDataBound="DgOperaciones_DataBound" ShowFooter="True" GridLines="Vertical" AutoGenerateColumns="false">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Codigo Operación">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodiOperacion" runat="server" ReadOnly="true" class="tmediano"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Descripción">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodiOperaciona" runat="server" ReadOnly="true" class="tgrande"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Tiempo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TIEMPO", "{0:n}") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodiOperacionb" runat="server" ReadOnly="true" class="tpequeno"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Operaciones">
				<ItemTemplate>
					<asp:Button CommandName="Delete" CausesValidation="False" Text="Borrar" ID="btnDel" Runat="server" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" />
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid>
</fieldset>	

<fieldset>
    <asp:button id="btnAceptar"  onclick="Aceptar"  runat="server" Text="Aceptar" onClientClick="espera();"></asp:button>&nbsp;&nbsp;&nbsp;
	<asp:button id="btnCancelar" onclick="Cancelar" runat="server" Text="Cancelar"></asp:button>
</fieldset>    
<p><asp:label id="lb" runat="server"></asp:label></p>
