<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModComaparendo.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModComaparendo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="RemeasLabel" Font-Bold="True" Font-Size="Medium" runat="server">Modificar Comparendos</asp:label>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
&nbsp;
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
	<TR>
		<TD style="HEIGHT: 8px"><asp:label id="Label1" Font-Bold="True" runat="server" ForeColor="Black">Conductor:</asp:label></TD>
		<TD style="HEIGHT: 8px"><asp:dropdownlist id="conductor" runat="server" AutoPostBack="true"></asp:dropdownlist></TD>
		<TD style="HEIGHT: 8px"></TD>
	</TR>
	<TR>
		<td><asp:label id="Label2" Font-Bold="True" runat="server" ForeColor="Black">Numero Comparendo:</asp:label></TD>
		<td><asp:dropdownlist id="comparendo" runat="server"></asp:dropdownlist></TD>
		<td></TD>
	</TR>
	<TR>
		<td><asp:button id="Buscar" onclick="Buscar_Click" runat="server" Text="Buscar"></asp:button></TD>
		<td></TD>
		<td></TD>
	</TR>
</TABLE>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
<asp:panel id="Panel1" runat="server" Width="770px" Visible="False">
	<TABLE id="Table2" style="WIDTH: 544px; HEIGHT: 258px" cellSpacing="0" cellPadding="0"
		width="544" border="0">
		<TR>
			<td>
				<asp:Label id="Label3" Font-Bold="True" runat="server" ForeColor="Black">Nombre:</asp:Label></TD>
			<td>
				<asp:Label id="nombrelabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 175px">
				<asp:Label id="Label4" Font-Bold="True" runat="server" ForeColor="Black">Fecha Comparendo:</asp:Label></TD>
			<TD style="WIDTH: 352px">
				<asp:Label id="fechalabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 175px">
				<asp:Label id="Label5" Font-Bold="True" runat="server" ForeColor="Black">Numero Comparendo:</asp:Label></TD>
			<TD style="WIDTH: 352px">
				<asp:Label id="comparendolabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 175px">
				<asp:Label id="Label7" Font-Bold="True" runat="server" ForeColor="Black">Infraccion:</asp:Label></TD>
			<TD style="WIDTH: 352px">
				<asp:Label id="infraccionlabel" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 175px; HEIGHT: 10px">
				<asp:Label id="Label9" Font-Bold="True" runat="server" ForeColor="Black">Vehiculo:</asp:Label></TD>
			<TD style="WIDTH: 352px; HEIGHT: 10px">
				<asp:Label id="bus" Font-Bold="True" runat="server" ForeColor="Red">Label</asp:Label></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 175px; HEIGHT: 18px">
				<asp:Label id="Label11" Font-Bold="True" runat="server" ForeColor="Black">Estado:</asp:Label></TD>
			<TD style="WIDTH: 352px; HEIGHT: 18px">
				<asp:DropDownList id="estado" runat="server"></asp:DropDownList></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 175px">
				<asp:Label id="Label12" Font-Bold="True" runat="server" ForeColor="Black">Observaciones:</asp:Label></TD>
			<TD style="WIDTH: 352px">
				<asp:TextBox id="observaciones" runat="server" Height="120px" TextMode="MultiLine" Width="200px"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 175px">
				<asp:Button id="Actualizar" onclick="Actualizar_Click" runat="server" Text="Actualizar"></asp:Button></TD>
			<TD style="WIDTH: 352px"></TD>
		</TR>
	</TABLE>
</asp:panel>
