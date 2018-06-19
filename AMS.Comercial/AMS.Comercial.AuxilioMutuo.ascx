<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.AuxilioMutuo.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_AuxilioMutuo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P><asp:label id="Label1" Font-Bold="True" runat="server" Font-Size="XX-Small">Calculo Fondo de Auxilio Mutuo</asp:label>
	<HR width="100%" color="#3300ff" SIZE="2">
	<FIELDSET><LEGEND>Filtros</LEGEND>
		<TABLE id="Table1" class="fieltersIn">
			<TR>
				<TD><asp:label id="Label6" Font-Bold="True" runat="server" Font-Size="XX-Small">#Acta</asp:label></TD>
				<TD><asp:textbox id="acta0" runat="server" class="tpequeno"></asp:textbox></TD>
				<TD><asp:label id="Label26" Font-Bold="True" runat="server" 
                        Font-Size="XX-Small">Fecha Acta</asp:label></TD>
				<td><asp:textbox id="fecha0" runat="server" class="tpequeno"></asp:textbox></TD>
			</TR>
			</TABLE>
	</FIELDSET>
	<HR width="100%" color="#3300ff" SIZE="2">
<P>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
	<FIELDSET title="Total"><LEGEND>Filtros</LEGEND>
		<TABLE id="Table2" class="fieltersIn">
			<TR>
				<TD><asp:label id="Label29" Font-Bold="True" runat="server" 
                        Font-Size="XX-Small" ForeColor="Black">Total Siniestro a Prorratear</asp:label></TD>
				<TD><asp:textbox id="TextBox2" runat="server" BackColor="Lime" 
                        BorderColor="Black" BorderStyle="Groove"
						BorderWidth="2px"></asp:textbox></TD>
				<TD><asp:button id="calcular0" onclick="Calcular_Click" 
                        runat="server" Text="Calcular"></asp:button></TD>
				<td></TD>
			</TR>
		</TABLE>
	</FIELDSET><asp:panel id="Panel1" runat="server" Width="1070px" Height="230px" Visible="False">
<TABLE id="Table3" class="fieltersIn">
  <TR>
    <TD align=center>
<asp:Label id=Label4 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="White">Categoria A</asp:Label></TD>
    <TD align=center>
<asp:Label id=Label7 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="White">Categoria B</asp:Label></TD>
    <TD align=center>
<asp:Label id=Label8 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="White">Categoria C</asp:Label></TD></TR>
  <TR>
    <TD vAlign=top>
<asp:datagrid id=Grid runat="server" cssclass="datagrid" HorizontalAlign="Center" AutoGenerateColumns="False" CellPadding="0">
						<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<FooterStyle csslcass="footer"></FooterStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="NUMERO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PLACA">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PLACA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FECHA">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PROPIETARIO">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PROPIETARIO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="AVALUO">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "AVALUO") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
					</asp:datagrid></TD>
    <TD vAlign=top>
<asp:datagrid id=Grid1 runat="server" cssclass="datagrid" HorizontalAlign="Center" AutoGenerateColumns="False" CellPadding="0">
						<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle csssclass="item"></ItemStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<FooterStyle cssclass="footer"></FooterStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="NUMERO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMEROB") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PLACA">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PLACAB") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FECHA">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHAB") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PROPIETARIO">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PROPIETARIOB") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="AVALUO">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "AVALUOB") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
					</asp:datagrid></TD>
    <TD vAlign=top>
<asp:datagrid id=Grid2 runat="server" cssclass="datagrid" HorizontalAlign="Center" AutoGenerateColumns="False" CellPadding="0">
						<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
						<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
						<ItemStyle cssclass="item"></ItemStyle>
						<HeaderStyle cssclass="header"></HeaderStyle>
						<FooterStyle cssclass="footer"></FooterStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="NUMERO">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "NUMEROC") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PLACA">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PLACAC") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="FECHA">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "FECHAC") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="PROPIETARIO">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PROPIETARIOC") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="AVALUO">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "AVALUOC") %>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle cssclass="pager" Mode="NumericPages"></PagerStyle>
					</asp:datagrid></TD></TR></TABLE>
<TABLE id="Table3" class="fieltersIn">
  <TR>
    <TD>
<asp:Label id=Label9 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Sumatoria:</asp:Label>
<asp:TextBox id=sumA Font-Size="XX-Small" runat="server" Width="120px" BorderStyle="Inset" BorderColor="Black" BackColor="#FF8000"></asp:TextBox>
<asp:Label id=Label12 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Porcentaje:</asp:Label>
<asp:TextBox id=PorA Font-Size="XX-Small" runat="server" Width="35px" BorderStyle="Dotted" BorderColor="Black" BackColor="Yellow"></asp:TextBox>%</TD>
    <TD>
<asp:Label id=Label10 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Sumatoria:</asp:Label>
<asp:TextBox id=sumB Font-Size="XX-Small" runat="server" Width="120px" BorderStyle="Inset" BorderColor="Black" BackColor="#FF8000"></asp:TextBox>
<asp:Label id=Label13 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Porcentaje:</asp:Label>
<asp:TextBox id=PorB Font-Size="XX-Small" runat="server" Width="35px" BorderStyle="Dotted" BorderColor="Black" BackColor="Yellow"></asp:TextBox>%</TD>
    <td>
<asp:Label id=Label11 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Sumatoria:</asp:Label>
<asp:TextBox id=sumC Font-Size="XX-Small" runat="server" Width="120px" BorderStyle="Inset" BorderColor="Black" BackColor="#FF8000"></asp:TextBox>
<asp:Label id=Label17 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Porcentaje:</asp:Label>
<asp:TextBox id=PorC Font-Size="XX-Small" runat="server" Width="35px" BorderStyle="Dotted" BorderColor="Black" BackColor="Yellow"></asp:TextBox>%</TD></TR>
  <TR>
    <td>
<asp:Label id=Label15 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Vr Prorrata:</asp:Label>
<asp:TextBox id=valoPA Font-Size="XX-Small" runat="server" Width="120px" BorderStyle="Inset" BorderColor="Black" BackColor="Lime"></asp:TextBox></TD>
    <TD style="WIDTH: 349px">
<asp:Label id=Label14 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Vr Prorrata:</asp:Label>
<asp:TextBox id=valoPB Font-Size="XX-Small" runat="server" Width="120px" BorderStyle="Inset" BorderColor="Black" BackColor="Lime"></asp:TextBox></TD>
    <td>
<asp:Label id=Label25 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Vr Prorrata:</asp:Label>
<asp:TextBox id=valoPC Font-Size="XX-Small" runat="server" Width="120px" BorderStyle="Inset" BorderColor="Black" BackColor="Lime"></asp:TextBox></TD></TR>
  <TR>
    <td>
<asp:Label id=Label16 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Total Prorrata Categoria A</asp:Label>
<asp:TextBox id=totalA Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="White" BorderStyle="Groove" BorderColor="Black" BackColor="Red"></asp:TextBox></TD>
    <td>
<asp:Label id=Label18 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Total Prorrata Categoria B</asp:Label>
<asp:TextBox id=totalB Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="White" BorderStyle="Groove" BorderColor="Black" BackColor="Red"></asp:TextBox></TD>
    <td>
<asp:Label id=Label19 Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="Black">Total Prorrata Categoria C</asp:Label>
<asp:TextBox id=totalC Font-Size="XX-Small" runat="server" Font-Bold="True" ForeColor="White" BorderStyle="Groove" BorderColor="Black" BackColor="Red"></asp:TextBox></TD></TR></TABLE>
<P>
<asp:Button id=Guardar onclick=Guardar_click runat="server" Width="112px" Text="Guardar"></asp:Button></P>
	</asp:panel>
<P></P>
