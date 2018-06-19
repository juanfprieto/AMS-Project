<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Documentos.Retenciones.ascx.cs" Inherits="AMS.Documentos.Retenciones" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language='javascript' src='../js/AMS.Tools.js' type='text/javascript'></script>
 
<fieldset>
  <legend>Retenciones</legend>
	<table class="filstersIn">
		<tbody>
			<tr>
				<td>
					<asp:DataGrid id="gridRtns" runat="server" Width="384px"  AutoGenerateColumns="False" HeaderStyle-BackColor="#ccccdd"
						Font-Size="8pt" Font-Name="Verdana" CellPadding="3" Font-Names="Verdana" onItemCommand="gridRtns_Item"
						OnItemDataBound="gridRtns_ItemDataBound" showfooter="True">
						<FooterStyle cssclass="footer"></FooterStyle>
						<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
						<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="C&#243;digo de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "CODRET") %>
								</ItemTemplate>
								<FooterTemplate>
									<center>
										<asp:TextBox id="codret" runat="server" ReadOnly="true" Width="70" ToolTip="Haga Click"></asp:TextBox>
									</center>
								</FooterTemplate>
							</asp:TemplateColumn>

							<asp:TemplateColumn HeaderText="Porcentaje de Retenci&#243;n (%)" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PORCRET","{0:N}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="codretb" runat="server" ReadOnly="true" Width="60"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>

							<asp:TemplateColumn HeaderText="Base">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALORBASE", "{0:C}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="base" runat="server" Enabled="true" Text="0" CssClass="AlineacionDerecha" Width="100"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>

							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "VALOR", "{0:C}") %>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="valor" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" Text="0" CssClass="AlineacionDerecha"
										Width="100" ReadOnly="True"></asp:TextBox>
								</FooterTemplate>

							</asp:TemplateColumn>
                            	<asp:TemplateColumn HeaderText="Nombre de Retenci&#243;n" HeaderStyle-HorizontalAlign="Center"
								FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NOMBRERET") %>
								</ItemTemplate>
								<FooterTemplate>
									<center>
										<asp:TextBox id="nombreret" runat="server" ReadOnly="true" Width="300" CssClass="AlineacionIzquierda"></asp:TextBox>
									</center>
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
						</Columns>
					</asp:DataGrid>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>
 

<script type="text/javascript">
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
