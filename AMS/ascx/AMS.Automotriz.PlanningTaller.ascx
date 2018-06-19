<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Automotriz.PlanningTaller.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_PlanningTaller" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/prototype.js"></script>
<script type ="text/javascript" src="../js/tooltip.js"></script>
<script type ="text/javascript">
var t1;
function init(){t1 = new ToolTip("divToolTip",true,40);}Event.observe(window, 'load', init, false);
function ConsOT(prefOT,numOT){var htmlConsulta = AMS_Automotriz_PlanningTaller.ConsultarInfoOT(prefOT,numOT).value;t1.Show(event,htmlConsulta);}
function ConsOT2(prefOT,numOT,durOper,tiemGas){var htmlConsulta = AMS_Automotriz_PlanningTaller.ConsultarInfoOT2(prefOT,numOT,durOper,tiemGas).value;t1.Show(event,htmlConsulta);}
function MostrarCumplidas(codMec){var divInfoCump = document.getElementById('div_cump_'+codMec);var spanInfoCump = document.getElementById('span_'+codMec);if(divInfoCump.style.display == 'none'){divInfoCump.style.display = '';spanInfoCump.innerText = 'Ocultar';}else{divInfoCump.style.display = 'none';spanInfoCump.innerText = 'Ver';}t1.Hide(event);}
</script>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>
	<tr>
		<td colSpan="2"><asp:label id="lbFechaProcesoConsulta" Font-Bold="True" ForeColor="Blue" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>Taller Relacionado :</td>
		<td><asp:dropdownlist id="ddlTaller" class="dpequeno" runat="server" AutoPostBack="True" onselectedindexchanged="ddlTaller_SelectedIndexChanged"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Fecha Consulta :</td>
		<td><asp:dropdownlist id="ddlFechaConsulta" class="dpequeno" runat="server" AutoPostBack="True" onselectedindexchanged="ddlFechaConsulta_SelectedIndexChanged"></asp:dropdownlist></td>
	</tr>
	<!-- <tr>
		<td width="30%"></td>
		<td align="right"></td>
	</tr>-->
    </td>
</tr>
    </table>
    </fieldset>
    <fieldset>
<asp:datagrid id="dgPlanning" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana"
	CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical"
	BorderWidth="1px" Font-Names="Verdana" Width="2000px">
	<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
	<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
	<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
	<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
	<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
</asp:datagrid>
</fieldset>
<fieldset>
<table id="Table" class="filtersIn">
	<tr>
		<td>
				<legend>
					Operaciones Sin Asignar</legend>
				
					<tr>
						<td>Total Operaciones Sin Asignar :
						</td>
						<td>
							<asp:Label id="lbTotalSinAsignar" Runat="server"></asp:Label>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<asp:datagrid id="dgSinAsignar" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt"
								Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None"
								GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" Width="200px" AutoGenerateColumns="False">
								<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
								<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
								<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="Operaciones Sin Asignar">
										<ItemTemplate>
											<span style="cursor:pointer;color:#555577" onclick=ConsOT('<%# DataBinder.Eval(Container.DataItem, "prefijo_orden") %>',<%# DataBinder.Eval(Container.DataItem, "numero_orden") %>)><%# DataBinder.Eval(Container.DataItem, "codigo_operacion") %> - <%# DataBinder.Eval(Container.DataItem, "descripcion_operacion") %></span>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:datagrid>
						</td>
					</tr>
				</table>
			</fieldset>

			<fieldset>
            <table id="Table" class="filtersIn">
            <tr>
            <td>
        <legend>Operaciones No Autorizadas</legend>
					<tr>
						<td width="85%">Total Operaciones No Autorizada :
						</td>
						<td align="right">
							<asp:Label id="lbTotalNoAutorizada" Runat="server"></asp:Label>
						</td>
					</tr>
					<tr>
						<td>
							<asp:datagrid id="dgNoAutorizada" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt"
								Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None"
								GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" Width="200px" AutoGenerateColumns="False">
								<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
								<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
								<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="Operaciones No Autorizada">
										<ItemTemplate>
											<span style="cursor:pointer;color:#555577" onclick=ConsOT('<%# DataBinder.Eval(Container.DataItem, "prefijo_orden") %>',<%# DataBinder.Eval(Container.DataItem, "numero_orden") %>)><%# DataBinder.Eval(Container.DataItem, "codigo_operacion") %> - <%# DataBinder.Eval(Container.DataItem, "descripcion_operacion") %></span>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:datagrid>
						</td>
					</tr>
                    </td>
            </tr>

				</table>
			</fieldset>
<div id="divToolTip" style="BORDER-RIGHT:gray 1px solid; BORDER-TOP:gray 1px solid; FILTER:alpha(Opacity=90); BORDER-LEFT:gray 1px solid; WIDTH:384px; BORDER-BOTTOM:gray 1px solid; HEIGHT:220px; BACKGROUND-COLOR:#ffff66; opacity:0.9"></div>
