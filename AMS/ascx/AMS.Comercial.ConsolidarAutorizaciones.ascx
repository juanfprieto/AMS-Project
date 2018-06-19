<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsolidarAutorizaciones.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsolidarAutorizaciones" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P><asp:label id="Label1" Font-Bold="True" runat="server">Consolidar Planillas</asp:label>
	<HR width="100%" color="#3300ff" SIZE="2">
<P></P>
<FIELDSET><LEGEND>Filtros</LEGEND>
	<TABLE id="Table1" class="fieltersIn">
		<TR>
			<TD><asp:label id="Label2" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Fecha:</asp:label></TD>
			<td><asp:label id="Label3" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Año</asp:label></TD>
			<td><asp:dropdownlist id="añoI" Font-Bold="True" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			<td><asp:label id="Label6" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Año</asp:label></TD>
			<td><asp:dropdownlist id="añoF" Font-Bold="True" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
		</TR>
		<TR>
			<TD></TD>
			<td><asp:label id="Label4" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Mes</asp:label></TD>
			<td><asp:dropdownlist id="mesI" Font-Bold="True" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			<td><asp:label id="Label7" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Mes</asp:label></TD>
			<td><asp:dropdownlist id="mesF" Font-Bold="True" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
		</TR>
		<TR>
			<TD></TD>
			<td><asp:label id="Label5" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Dia</asp:label></TD>
			<td><asp:textbox id="diaI" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="30px"></asp:textbox></TD>
			<td><asp:label id="Label8" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Dia</asp:label></TD>
			<td><asp:textbox id="diaF" Font-Bold="True" runat="server" Font-Size="XX-Small" Width="30px"></asp:textbox></td>
		</TR>
	</TABLE>
	<TABLE id="Table2" class="fieltersIn">
		<TR>
			<TD><asp:label id="Label9" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Vehiculo:</asp:label></TD>
			<TD><asp:dropdownlist id="placa" Font-Bold="True" runat="server" Font-Size="XX-Small" AutoPostBack="true"></asp:dropdownlist></TD>
			<TD><asp:label id="Label10" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Bus #:</asp:label></TD>
			<td><asp:label id="busLabel" Font-Bold="True" runat="server" ForeColor="Red">#</asp:label></TD>
		</TR>
		<TR>
			<TD></TD>
			<TD></TD>
			<TD><asp:label id="Label11" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Propietario:</asp:label></TD>
			<TD><asp:label id="PropLabel" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">-------</asp:label></TD>
		</TR>
		<TR>
			<TD></TD>
			<TD></TD>
			<TD><asp:label id="Label12" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Conductor:</asp:label></TD>
			<td><asp:label id="conductor" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">-------</asp:label></TD>
		</TR>
		<tr>
			<td><asp:button id="Generar" runat="server" Width="64px" Text="Generar" OnClick="generar_OnClick"></asp:button></td>
			<td></td>
		</tr>
	</TABLE>
</FIELDSET>
<P>
	<HR width="100%" color="#3300ff" SIZE="2">
<P><asp:panel id="Panel1" runat="server" Height="248px">
		<TABLE id="Table3" class="fieltersIn">
			<TR>
				<TD>
					<asp:Label id="Label13" Font-Bold="True" runat="server" Font-Size="XX-Small"># Autorizaciones de Servicio:</asp:Label></TD>
				<TD>
					<asp:Label id="numauto" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">------</asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label17" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Total Autorizaciones:</asp:Label></TD>
				<TD>
					<asp:Label id="totauto" Font-Bold="True" runat="server" ForeColor="Red" Font-Size="XX-Small">------</asp:Label></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label15" Font-Bold="True" runat="server" ForeColor="Black" Font-Size="XX-Small">Total</asp:Label></TD>
				<TD>
					<asp:TextBox id="TextBox1" runat="server" BorderColor="Black" BorderStyle="Dotted" BackColor="Lime"></asp:TextBox></TD>
			</TR>
			<TR>
				<td>
					<asp:Button id="Detalles" onclick="Detalles_OnClick" runat="server" Text="Detalles"></asp:Button></TD>
				<TD></TD>
			</TR>
		</TABLE>
		<asp:Panel id="Panel2" runat="server" Height="85px">
			<asp:datagrid id="Grid" runat="server" cssclass="datagrid" AutoGenerateColumns="False" HorizontalAlign="Left">
				<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
				<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
				<ItemStyle cssclass="item"></ItemStyle>
				<HeaderStyle cssclass="heater"></HeaderStyle>
				<FooterStyle cssclass="footer"></FooterStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="NUMERO DOC">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="FECHA">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="CANTIDAD">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="DESCRIPCION">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="VALOR">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
		</asp:Panel>
		<asp:Button id="Guardar" onclick="Guardar_Onclick" runat="server" Text="Guardar"></asp:Button>
	</asp:panel></P>
