<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.InventarioFisicoTodos.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_InventarioFisicoTodos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table>
	<tbody>
		<TR>
			<td><asp:label id="Label1" runat="server">Generacion de Tarjetas por :</asp:label></td>
			<td><asp:dropdownlist id="ddltarjeta" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltarjeta_SelectedIndexChanged"
				class="dpequeno">
					<asp:ListItem Value="1">Referencia</asp:ListItem>
					<asp:ListItem Value="2">Ubicacion</asp:ListItem>
				</asp:dropdownlist></td>
		</TR>
		<TR>
			<TD><asp:label id="ran" runat="server" Visible="False">Seleccione el Rango Inicial : </asp:label></TD>
			<TD><asp:listbox id="lbrango" runat="server" Width="320px" Visible="False" Height="104px"></asp:listbox></TD>
		</TR>
		<TR>
			<TD><asp:label id="ranf" runat="server" Visible="False">Seleccione el Rango Final :</asp:label></TD>
			<TD><asp:listbox id="lbrangof" runat="server" Width="320px" Visible="False" Height="104px"></asp:listbox></TD>
		</TR>
		<TR>
			<TD>
				<P>&nbsp;</P>
				<P>Numero&nbsp;De Tarjetas de Alta :</P>
				<P>&nbsp;</P>
			</TD>
			<TD>
				<asp:TextBox id="altas" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Seleccione el Numero de Tarjetas"
					ControlToValidate="altas">*</asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="altas" ErrorMessage="Solo Numeros"
					ValidationExpression="\d+">*</asp:RegularExpressionValidator></TD>
		</TR>
		<TR>
			<TD><asp:checkbox id="cbalmacen" runat="server" AutoPostBack="true" Text="Todo Los Almacenes" oncheckedchanged="cbalmacen_CheckedChanged"></asp:checkbox></TD>
		</TR>
	</tbody>
</table>

&nbsp;
<P>&nbsp;</P>
<P><asp:button id="btnRealizar" onclick="generar_Inventario" runat="server" Text="Generar"></asp:button></P>
<asp:datagrid id="dgitems" runat="server" AutoGenerateColumns="False">
	<Columns>
		<asp:BoundColumn DataField="PUBI_CODIGO" HeaderText="COD UBICACION"></asp:BoundColumn>
		<asp:BoundColumn DataField="MITE_CODIGO" HeaderText="CODIGO ITEM"></asp:BoundColumn>
		<asp:BoundColumn DataField="MITE_NOMBRE" HeaderText="DESCRIPCION"></asp:BoundColumn>
		<asp:BoundColumn DataField="PALM_ALMACEN" HeaderText="ALMACEN"></asp:BoundColumn>
		<asp:BoundColumn DataField="MSAL_CANTACTUAL" HeaderText="CANTIDAD"></asp:BoundColumn>
		<asp:BoundColumn DataField="MSAL_COSTPROM" HeaderText="COSTO PROMEDIO"></asp:BoundColumn>
	</Columns>
</asp:datagrid><asp:button id="btnSave" onclick="guardar_tabla" runat="server" Visible="False" Text="Guardar Inventario Fisico"></asp:button>
