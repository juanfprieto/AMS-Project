<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.GestorDocVehiculos.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_GestorDocVehiculos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="GirosLabel" runat="server" Font-Bold="True" Font-Size="Medium">Gestor Documentos Vehiculos</asp:label>

<fieldset>
<table id="Table1" class="filtersIn">
	<TR>
		<TD>
			<asp:Label id="Label1" runat="server">Vehiculos:</asp:Label></TD>
            </tr>
            <tr>
		<TD>
			<asp:DropDownList id="VehiDrop" class="dmediano" runat="server"></asp:DropDownList>
			<asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image></TD>
	</TR>
	<TR>
		<TD>
			<asp:Button id="Generar" onclick="Generar_Click" runat="server" Text="Generar"></asp:Button></TD>
		<TD></TD>
	</TR>
</TABLE>
<asp:Panel id="Panel1" runat="server" Height="336px" Width="284px" Visible="False">
<TABLE id=Table2 cellSpacing=0 cellPadding=0 width=300 border=0>
  <TR>
    <TD>
<asp:Panel id=Panel2 runat="server" Width="202px" Height="368px">
      <HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color=#000099 
      SIZE=8>

      <TABLE id=Table4 cellSpacing=0 cellPadding=0 width=300 border=0>
        <TR>
          <TD>
<asp:Label id=Label2 Font-Bold="True" runat="server" ForeColor="Black">Vehículo:</asp:Label></TD>
          <TD>
<asp:Label id=VehiLabel Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD></TR>
        <TR>
          <TD>
<asp:Label id=Label3 Font-Bold="True" runat="server" ForeColor="Black">Propietario:</asp:Label></TD>
          <TD>
<asp:Label id=PropLabel Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD></TR></TABLE>
      <TABLE id=Table3 cellSpacing=0 cellPadding=0 width=300 border=0>
        <TR>
          <TD>
            <HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" 
            color=#000099 SIZE=8>
            &nbsp;</TD>
          <TD></TD></TR>
        <TR>
          <TD>
<asp:Label id=SOATLabel Font-Bold="True" runat="server" ForeColor="Black">Documentos</asp:Label></TD>
          <TD>
<asp:Button id=Cargar1 onclick=Cargar1_Click runat="server" Text="Cargar"></asp:Button></TD></TR>
</TABLE></asp:Panel></TD>
</TR></TABLE>
</asp:Panel>
</fieldset>
