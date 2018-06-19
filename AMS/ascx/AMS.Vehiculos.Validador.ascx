<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.Validador.ascx.cs" Inherits="AMS.Vehiculos.Validador" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>

<fieldset>
  <table id="Table2" class="filtersIn">
	<tbody>
		<tr>
			<td>
				MODELO</td>
			<td>
				<asp:dropdownlist id="ddlcatalogo" AutoPostBack="False" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td>
				VALOR VENTA:
			</td>
			<td>
				<asp:TextBox id="valVenta" runat="server" onKeyUp="NumericMaskE(this,event)"></asp:TextBox>
			</td>
		</tr>
	</tbody>
  </table>
</fieldset>
<br>

<fieldset>
  <table id="Table3" class="filtersIn">
	<tbody>
    <tr>
    <td>
    <br>
    <br>
<asp:DataGrid id="dgGastos" runat="server" AutoGenerateColumns="False">
	<Columns>
		<asp:BoundColumn DataField="CODIGO" HeaderText="CODIGO"></asp:BoundColumn>
		<asp:BoundColumn DataField="NOMBRE" HeaderText="NOMBRE"></asp:BoundColumn>
		<asp:BoundColumn DataField="COSTO" HeaderText="COSTO" DataFormatString="{0:C}"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="SELECCIONAR">
			<ItemTemplate>
				<center>
					<asp:CheckBox id="chkBox" runat="server"></asp:CheckBox></center>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
<P>
	<div align="center"><asp:Button id="btnValidar" Onclick="validar" runat="server" Text="Validar"></asp:Button></div></P>
</td>
</tr>
</tbody>
  </table>
</fieldset>
<P></P>

<P>
	<br>
	<asp:Label id="lb" runat="server" Font-Italic="True" Font-Bold="True" Font-Size="Medium"></asp:Label></P>
<P></P>
<P>
	<asp:Panel id="pan" runat="server" Visible="False">
		<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="1">
			<TR>
				<TD>COSTO:</TD>
				<TD><asp:Label id="lbCosto" runat="server"></asp:Label></TD></TR>
			<TR>
				<TD>UTILIDAD:</TD>
				<TD><asp:Label id="lbUtilidad" runat="server"></asp:Label></TD></TR>
			<TR>
				<TD>PORCENTAJE UTILIDAD:</TD>
				<TD><asp:Label id="lbPorcentaje" runat="server"></asp:Label></TD></TR>
			<TR>
				<TD>UTILIDAD SUGERIDA:</TD>
				<TD><asp:Label id="lbUtilSugerida" runat="server"></asp:Label></TD></TR>
                </TABLE>
	</asp:Panel></P>
