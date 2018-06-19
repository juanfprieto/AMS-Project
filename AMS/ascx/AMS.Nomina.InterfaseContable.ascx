<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Nomina.InterfaseContable.ascx.cs" Inherits="FabianNomina.AMS_Nomina_InterfaseContable" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<meta name="vs_showGrid" content="False">
<P>INTERFASE CONTABLE
</P>
<P>
	<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="1">
		<TR>
			<TD style="HEIGHT: 21px"><asp:label id="Label1" width="165px" height="21px" runat="server">Periodo a procesar </asp:label></TD>
			<TD style="HEIGHT: 21px"><asp:dropdownlist id="DDLQUIN" runat="server" Width="138px"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label3" runat="server">Mes</asp:label></TD>
			<TD><asp:dropdownlist id="DDLMES" runat="server" Width="138px"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD><asp:label id="Label2" width="32px" runat="server">Año</asp:label></TD>
			<TD><asp:dropdownlist id="DDLANO" runat="server" Width="137px"></asp:dropdownlist></TD>
		</TR>
	</TABLE>
</P>
<P>Se generaran todas las interfases contables (ARPs,EPS..)Por Defecto esta 
	escogida la Ultima Quincena Liquidada por el Sistema.</P>
<P>Porfavor Seleccione de cada lista el codigo del comprobante indicado para 
	generar la interfase contable (Previamente el contador debio generar dichos 
	documentos del tipo Nomina.</P>
<P>
	<TABLE id="Table3" cellSpacing="1" cellPadding="1" width="300" border="1">
		<TR>
			<TD style="HEIGHT: 26px">Interfaz de Nómina</TD>
			<TD style="HEIGHT: 26px">
				<asp:DropDownList id="DDLNOMINA" runat="server"></asp:DropDownList></TD>
		</TR>
		<TR>
			<TD>Interfaz de ARP's</TD>
			<TD>
				<asp:DropDownList id="DDLARP" runat="server"></asp:DropDownList></TD>
		</TR>
		<TR>
			<TD>Interfaz de EPS</TD>
			<TD>
				<asp:DropDownList id="DDLEPS" runat="server"></asp:DropDownList></TD>
		</TR>
	</TABLE>
</P>
<P>&nbsp;</P>
<P><asp:button id="btn_interfase" runat="server" Text="Enviar"></asp:button></P>
<P>
	<TABLE class="reports" id="Table2" width="780" align="center" bgColor="gray">
		<TR>
			<TD><asp:table id="tabPreHeader" Width="100%" BorderWidth="0px" EnableViewState="False" CellSpacing="0"
					CellPadding="1" BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana"
					HorizontalAlign="Center"></asp:table></TD>
		</TR>
		<TR>
			<TD align="center">
				<P><asp:datagrid id="dg" runat="server" Width="100%" OnItemDataBound="Report_ItemDataBound" AutoGenerateColumns="False">
						<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
						<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
					</asp:datagrid></P>
			</TD>
		</TR>
		<TR>
			<TD><asp:table id="tabFirmas" Width="100%" BorderWidth="0px" EnableViewState="False" CellSpacing="0"
					CellPadding="1" BackColor="White" GridLines="Both" Runat="server" Font-Size="8pt" Font-Name="Verdana"
					HorizontalAlign="Center"></asp:table></TD>
		</TR>
	</TABLE>
</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
