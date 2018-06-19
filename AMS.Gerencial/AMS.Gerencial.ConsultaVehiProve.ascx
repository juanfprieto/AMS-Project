<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Gerencial.ConsultaVehiProve.ascx.cs" Inherits="AMS.Gerencial.AMS_Gerencial_ConsultaVehiProve" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
	<fieldset>

<table id="Table" class="filtersIn">
	<TR>
		<TD >
			<asp:Label id="Label16" runat="server" Font-Bold="True" ForeColor="Red">Fecha Inicio:</asp:Label>
            <br />
		
			<asp:Label id="Label14" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black">Año:</asp:Label><br />
			<asp:DropDownList id="ano1" runat="server" class="dpequeno"></asp:DropDownList>
         </td>
		<td >
			<asp:Label id="Label18" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black">Mes:</asp:Label><br />
			<asp:DropDownList id="mes1" runat="server" class="dpequeno"></asp:DropDownList></td>
            
		<td>
			<asp:Label id="Label20" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="X-Small">Día</asp:Label><br />
			<asp:TextBox id="DiaInicio" runat="server" class="tpequeno">1</asp:TextBox></td>
	</TR>
	<tr>
		<td >
			<asp:Label id="Label15" runat="server" Font-Bold="True" ForeColor="Red">Fecha Finalización:</asp:Label>
            <br />
			<asp:Label id="Label17" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black">Año:</asp:Label><br />
			<asp:DropDownList id="ano2" runat="server" class="dpequeno"></asp:DropDownList></td>
		<td>
			<asp:Label id="Label19" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black">Mes:</asp:Label><br />
			<asp:DropDownList id="mes2" runat="server" class="dpequeno"></asp:DropDownList></td>
		<td>
			<asp:Label id="Label21" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="X-Small">Día</asp:Label><br />
			<asp:TextBox id="DiaFin" runat="server" class="tpequeno"></asp:TextBox></td>
	</tr>
	<tr>
		<td>
			<asp:Button id="Generar" runat="server" Text="Generar Informe" class="bpequeno" OnClick="Generar_Click"></asp:Button></td>
	</tr>
</TABLE>
</fieldset>

<asp:panel id="Panel2" runat="server" Height="280px" Width="834px" BackColor="#E0E0E0" Visible="False"
	BorderColor="Gray">
	<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
		<TR>
			<TD>
				<asp:Label id="Label1" runat="server" Font-Bold="True" ForeColor="Red">Pedidos de Vehículos a Proveedores</asp:Label></TD>
		</TR>
	</TABLE>
	<DIV style="OVERFLOW: auto; WIDTH: 800px; HEIGHT: 200px">
		<asp:datagrid id="Grid" runat="server" Width="780px" AutoGenerateColumns="False" HorizontalAlign="left"
			PageSize="5" AllowSorting="True">
			<FooterStyle CssClass="footer"></FooterStyle>
			<HeaderStyle CssClass="header"></HeaderStyle>
			<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Codigo Vehiculo">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Descripcion">
					<ItemStyle HorizontalAlign="left"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor Unitario">
					<ItemStyle HorizontalAlign="right"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Color">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "COLOR") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Cantidad">
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid></DIV>
	
	<TABLE id="Table4" class="filtersIn">
		<TR>
			<TD>
				<asp:Label id="Label2" runat="server" Font-Bold="True" ForeColor="Red">Cantidad Pedido:</asp:Label></TD>
			<TD>
				<asp:Label id="CantPedido" runat="server" Font-Bold="True"></asp:Label></TD>
			<TD>
				<asp:Label id="Label7" runat="server" Font-Bold="True" ForeColor="Red">Cantidad Vehículos:</asp:Label></TD>
			<TD>
				<asp:Label id="cantVehi" runat="server" Font-Bold="True"></asp:Label></TD>
			<TD>
				<asp:Label id="Label4" runat="server" Font-Bold="True" ForeColor="Red">Valor Total Pedido:</asp:Label></TD>
			<TD>
				<asp:Label id="ValoPedidos" runat="server" Font-Bold="True"></asp:Label></TD>
		</TR>
	</TABLE>
	
	<TABLE id="Table2" class="filtersIn">
		<TR>
			<TD>
				<asp:Label id="Label3" runat="server" Font-Bold="True" ForeColor="Red">Pedidos Vendedores:</asp:Label></TD>
		</TR>
	</TABLE>
	<DIV style="OVERFLOW: auto; WIDTH: 900px; HEIGHT: 156px">
		<asp:datagrid id="Grid2" runat="server" cssclass="datagrid" AutoGenerateColumns="False" HorizontalAlign="center"
			PageSize="5">
			<FooterStyle CssClass="footer"></FooterStyle>
			<HeaderStyle CssClass="header"></HeaderStyle>
			<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Codigo Vendedor">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CODIGO2") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Nombre Vendedor">
					<ItemStyle HorizontalAlign="left"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Pedidos">
					<ItemStyle HorizontalAlign="left"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "PEDIDOS") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Valor Pedido">
					<ItemStyle HorizontalAlign="right"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "TOTALPEDIDOS") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Asig">
					<ItemStyle HorizontalAlign="right"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "ASIGNADOS") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Fact">
					<ItemStyle HorizontalAlign="right"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "FACTURADOS") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Meta">
					<ItemStyle HorizontalAlign="right"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "META") %>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid></DIV>
	<TABLE id="Table3" class="filtersIn">
		<TR>
			<TD>
				<P>
					<asp:Label id="Label10" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small">Asig=Asignado</asp:Label>&nbsp;
					<asp:Label id="Label12" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small">Fact=Facturado</asp:Label>&nbsp; 
				    <asp:Label id="Label13" runat="server" Font-Bold="True" ForeColor="Black" Font-Size="XX-Small">Meta=Meta de Ventas</asp:Label>
               </P>
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label5" runat="server" Font-Bold="True" ForeColor="Red">Numero de Vendedores:</asp:Label></TD>
			<TD>
				<asp:Label id="numven" runat="server" Font-Bold="True"></asp:Label></TD>
			<TD>
				<asp:Label id="Label8" runat="server" Font-Bold="True" ForeColor="Red" Visible="False">Total Vehículos Asignados:</asp:Label></TD>
			<TD>
				<asp:Label id="totasig" runat="server" Font-Bold="True"></asp:Label></TD>
			<TD>
				<asp:Label id="Label11" runat="server" Font-Bold="True" ForeColor="Red">Total Vehículos Pedidos:</asp:Label></TD>
			<TD>
				<asp:Label id="totVehiP" runat="server" Font-Bold="True"></asp:Label></TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="Label6" runat="server" Font-Bold="True" ForeColor="Red">Total Pedidos:</asp:Label></TD>
			<TD>
				<asp:Label id="totalven" runat="server" Font-Bold="True"></asp:Label></TD>
			<TD>
				<asp:Label id="Label9" runat="server" Font-Bold="True" ForeColor="Red">Total Vehículos Facturados</asp:Label></TD>
			<TD>
				<asp:Label id="totfac" runat="server" Font-Bold="True"></asp:Label></TD>
		</TR>
	</TABLE>
</asp:panel>

