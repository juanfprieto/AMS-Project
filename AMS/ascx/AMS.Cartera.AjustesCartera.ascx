<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Cartera.AjustesCartera.ascx.cs" Inherits="AMS.Finanzas.AMS_Cartera_AjustesCartera"%>
<script language='javascript' src='../js/AMS.Tools.js' type='text/javascript'></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<table id="Table1" class="filtersIn">
	<tr>
		<td>Tipo:</td>
		<td><asp:RadioButtonList id="rblTipo" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" onselectedindexchanged="rblTipo_SelectedIndexChanged">
				<asp:ListItem Value="C">Cartera de Clientes</asp:ListItem>
				<asp:ListItem Value="P">Obligaciones con Proveedores</asp:ListItem>
			</asp:RadioButtonList></td>
	</tr>
</table>
<asp:Panel ID="pnlDocumento" Runat="server" Visible="False">
	<TABLE id="Table2" class="filtersIn">
		<TR>
			<TD>Prefijo:&nbsp;</TD>
			<TD>
				<asp:DropDownList id="ddlPrefijo" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefijo_SelectedIndexChanged"></asp:DropDownList></TD>
			<TD>Numero:&nbsp;</TD>
			<TD>
				<asp:DropDownList id="ddlNumero" runat="server"></asp:DropDownList></TD>
			<TD>&nbsp;&nbsp;
				<asp:Button id="btnSeleccionar" runat="server" Visible="True" Text="Seleccionar" onclick="btnSeleccionar_Click"></asp:Button></TD>
		</TR>
	</TABLE>

	<asp:Panel id="pnlRetenciones" Runat="server" Visible="False">
		<TABLE id="Table3" class="filtersIn">
			<TR>
				<TD>&nbsp;</TD>
			</TR>
			<TR>
				<TD>
					<asp:DataGrid id="gridRtns" runat="server" cssclass="datagrid" showfooter="True" OnItemDataBound="gridRtns_ItemDataBound"
						onItemCommand="gridRtns_Item" CellPadding="3"
						AutoGenerateColumns="False" OnCancelCommand="dgrItems_Cancel"
						OnUpdateCommand="dgrItems_Update" OnEditCommand="dgrItems_Edit">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="C&#243;digo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
								</EditItemTemplate>
								<FooterTemplate>
									<center>
										<asp:TextBox id="codret" runat="server" ReadOnly="true" Width="70" ToolTip="Haga Click"></asp:TextBox>
									</center>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Nombre Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
								</EditItemTemplate>
								<FooterTemplate>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Tipo Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TRET_NOMBRE") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TRET_NOMBRE") %>
								</EditItemTemplate>
								<FooterTemplate>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Tipo Proceso" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TTIP_PROCESO") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TTIP_PROCESO") %>
								</EditItemTemplate>
								<FooterTemplate>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Tipo Persona" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TTIP_NOMBRE") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "TTIP_NOMBRE") %>
								</EditItemTemplate>
								<FooterTemplate>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Porcentaje de Retenci&#243;n (%)" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
								</ItemTemplate>
								<EditItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="codretb" runat="server" ReadOnly="true" Width="60"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Base">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
								</ItemTemplate>
								<EditItemTemplate>
								<asp:TextBox id="txtEdV" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:#,###.##}") %>' CssClass="AlineacionDerecha"
										Width="100"></asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="base" runat="server" Enabled="true" Text="0" CssClass="AlineacionDerecha" Width="100"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
								</ItemTemplate>
								<EditItemTemplate>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="valor" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha"
										Width="100" ReadOnly="True"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Agregar" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<asp:Button id="remRet" runat="server" CommandName="RemoverRetencion" Text="Remover" CausesValidation="false" />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button id="agRet" runat="server" CommandName="AgregarRetencion" Text="Agregar" Enabled="true"
										CausesValidation="false" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
						</Columns>
					</asp:DataGrid></TD>
			</tr>
			<tr>
				<td>
					<asp:Button id="btnAceptar" runat="server" Visible="True" Text="Guardar" onclick="btnAceptar_Click"></asp:Button>&nbsp;&nbsp;&nbsp;
					<asp:Button id="btnVolver"  runat="server" Visible="True" Text="Volver"  onclick="btnVolver_Click"> </asp:Button>
				</td>
			</tr>
		</TABLE>
	</asp:Panel>
</asp:Panel>
<asp:Label id="lblError" runat="server" ></asp:Label>

<script type = "text/javascript">
	function PorcentajeVal(tPorcentaje,tBase,tTotal){
		var txtT=document.getElementById(tTotal);
		try{
			var prct=parseFloat(document.getElementById(tPorcentaje).value.replace(/\,/g,''));
			var bse=parseFloat(document.getElementById(tBase).value.replace(/\,/g,''));
			var pt=Math.round((prct*bse)/100);
			txtT.value=formatoValor(pt);
		}
		catch(err){
			txtT.value="";
		}
	}
</script>
