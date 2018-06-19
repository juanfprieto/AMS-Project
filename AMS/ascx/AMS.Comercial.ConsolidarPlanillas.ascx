<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsolidarPlanillas.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsolidarPlanillas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P>
	<HR width="100%" color="mediumblue" SIZE="2">
	<asp:label id="Label1" runat="server">Consolidar Planillas</asp:label>
	<HR width="100%" color="mediumblue" SIZE="2">
	<FIELDSET><LEGEND>Datos Planillas</LEGEND>
		<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
			<TR>
				<td><asp:label id="Label2" runat="server" Font-Bold="True" ForeColor="Black">Fecha</asp:label></TD>
				<td><asp:textbox id="fecha" runat="server" Width="102px"></asp:textbox><asp:button id="Buscar" onclick="buscar" runat="server" Text="Buscar"></asp:button></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 16px"><asp:label id="Label3" runat="server" Font-Bold="True" ForeColor="Black">Planilla #</asp:label></TD>
				<TD style="HEIGHT: 16px"><asp:dropdownlist id="planillas" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<td><asp:label id="label" runat="server" Font-Bold="True" ForeColor="Black">BUS:</asp:label></TD>
				<td><asp:label id="bus" runat="server">Label</asp:label></TD>
			</TR>
			<tr>
				<td><asp:label id="Label5" runat="server" Font-Bold="True" ForeColor="Black">Conductor:</asp:label></td>
				<td><asp:label id="conductor" runat="server">Label</asp:label></td>
			</tr>
			<tr>
				<td style="HEIGHT: 18px"><asp:label id="Label7" runat="server" Font-Bold="True" ForeColor="Black">Ruta:</asp:label></td>
				<td style="HEIGHT: 18px"><asp:label id="ruta" runat="server">Label</asp:label></td>
			</tr>
			<tr>
				<td><asp:button id="Generar" onclick="generar" runat="server" Text="Generar"></asp:button></td>
				<td></td>
			</tr>
		</TABLE>
	</FIELDSET>
<P></P>
<HR width="100%" color="mediumblue" SIZE="2">
<P><asp:panel id="Panel1" runat="server" Height="156px">
<TABLE id=Table2 style="WIDTH: 776px; HEIGHT: 19px" cellSpacing=0 cellPadding=0 
width=776 border=0>
  <TR>
    <TD style="HEIGHT: 116px">
      <FIELDSET style="WIDTH: 333px; HEIGHT: 48px"><LEGEND>Tiqutes 
      Vendidos</LEGEND>
      <TABLE id=Table3 style="WIDTH: 328px; HEIGHT: 13px" cellSpacing=0 
      cellPadding=0 width=328 border=0>
        <TR>
          <td>
<asp:Label id=Label6 runat="server" Font-Bold="True" ForeColor="Black">Numero de Tiquetes</asp:Label></TD>
          <td>
<asp:Label id=tiquetesV runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD></TR>
        <TR>
          <td>
<asp:Label id=Label8 runat="server" Font-Bold="True" ForeColor="Black">Valor Total Venta</asp:Label></TD>
          <td>
<asp:Label id=totTV runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
          <td></TD></TR></TABLE></FIELDSET> </TD>
    <TD style="HEIGHT: 116px">
      <FIELDSET style="WIDTH: 333px; HEIGHT: 56px"><LEGEND>Tiqutes Vendidos 
      Manualmente</LEGEND>
      <TABLE id=Table4 style="WIDTH: 328px; HEIGHT: 2px" cellSpacing=0 
      cellPadding=0 width=328 border=0>
        <TR>
          <td>
<asp:Label id=Label9 runat="server" Font-Bold="True" ForeColor="Black">Numero de Tiquetes</asp:Label></TD>
          <td>
<asp:Label id=tiquetesM runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
          <td></TD></TR>
        <TR>
          <td>
<asp:Label id=Label10 runat="server" Font-Bold="True" ForeColor="Black">Valor Total Venta</asp:Label></TD>
          <td>
<asp:Label id=totTM runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD></TR></TABLE></FIELDSET> 
    </TD></TR>
  <TR>
    <td>
      <FIELDSET style="WIDTH: 333px; HEIGHT: 45px"><LEGEND>Remesas o 
      Encomiendas</LEGEND>
      <TABLE id=Table5 cellSpacing=0 cellPadding=0 width=300 border=0>
        <TR>
          <td>
<asp:Label id=Label11 runat="server" Font-Bold="True" ForeColor="Black">Numero de Encomiendas</asp:Label></TD>
          <td>
<asp:Label id=NumEnco runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
          <td></TD></TR>
        <TR>
          <td>
<asp:Label id=Label13 runat="server" Font-Bold="True" ForeColor="Black">Valor Total Encomiendas</asp:Label></TD>
          <td>
<asp:Label id=totEn runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD></TR></TABLE></FIELDSET> 
      <asp:Panel id=TOTALPANEL runat="server" Width="304px" Visible="False">
      <TABLE id=Table7 cellSpacing=0 cellPadding=0 width=300 border=0>
        <TR>
          <td>
<asp:Label id=Label4 runat="server" Font-Bold="True" ForeColor="Black">Producido Planilla:</asp:Label></TD>
          <td>
<asp:TextBox id=prod runat="server" Font-Bold="True" ForeColor="Black" BorderStyle="Dashed" BackColor="Lime" ReadOnly="True"></asp:TextBox></TD>
          <td></TD></TR>
        <TR>
          <td>
<asp:Label id=Label18 runat="server" Font-Bold="True" ForeColor="Black">Costos Ruta:</asp:Label></TD>
          <td>
<asp:TextBox id=costos runat="server" Font-Bold="True" ForeColor="Black" BorderStyle="Dashed" BackColor="Red" ReadOnly="True"></asp:TextBox></TD>
          <td></TD></TR>
        <TR>
          <td>
<asp:Label id=Label15 runat="server" Font-Bold="True" ForeColor="Black">TOTAL</asp:Label>
<asp:Label id=Label16 runat="server" Font-Bold="True" Font-Size="XX-Small">(Producido-Costos)</asp:Label></TD>
          <td>
<asp:TextBox id=ultimototal runat="server" Font-Bold="True" ForeColor="Black" BorderStyle="Dashed" BackColor="Yellow" ReadOnly="True"></asp:TextBox></TD></TR></TABLE></asp:Panel></TD>
    <td>
<asp:Panel id=Panel3 runat="server" Height="156px" Visible="False">
      <FIELDSET style="WIDTH: 333px; HEIGHT: 160px"><LEGEND>Anticipos</LEGEND>
      <TABLE id=Table6 cellSpacing=0 cellPadding=0 width=300 border=0>
        <TR>
          <td>
<asp:Label id=Label12 runat="server" Font-Bold="True" ForeColor="Black">Numero de Anticipos</asp:Label></TD>
          <td>
<asp:Label id=NumAn runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
          <td></TD></TR>
        <TR>
          <td>
<asp:Label id=Label14 runat="server" Font-Bold="True" ForeColor="Black">Valor Total Anticipos</asp:Label></TD>
          <td>
<asp:Label id=totAN runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD>
          <td></TD></TR>
        <TR>
          <td>
<asp:Button id=Detalle onclick=Detalle_OnClick runat="server" Text="Detalle"></asp:Button></TD></TR></TABLE>
<asp:Panel id=Panel2 runat="server" Height="104px">
      <FIELDSET style="WIDTH: 417px; HEIGHT: 156px"><LEGEND>Detalles</LEGEND>
<asp:datagrid id=Grid runat="server" Width="411px" Height="114px" HorizontalAlign="Center" AutoGenerateColumns="False">
										<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
										<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
										<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
										<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
										<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="NUMERO ANTICIPO">
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="DESCRIPCION ANTICIPO">
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="VALOR ANTICIPO">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<%# DataBinder.Eval(Container.DataItem, "VALOR") %>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></asp:Panel></FIELDSET> </asp:Panel></FIELDSET> 
    </TD></TR></TABLE>
<asp:Button id=GenerarPlanilla onclick=Guardar_Click runat="server" Text="Generar Total Planilla"></asp:Button>
<asp:Label id=Label17 runat="server">Label</asp:Label>
	</asp:panel></P>
