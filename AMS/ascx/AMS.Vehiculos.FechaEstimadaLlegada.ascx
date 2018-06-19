<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.FechaEstimadaLlegada.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_FechaEstimadaLlegada" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<FIELDSET>
<legend class=Legends>informacion</legend>
 <table id="Table1" class="filtersIn">
	<tbody>
		<tr>
			<td>Nit Proveedor
			</td>
			<td><asp:dropdownlist id="ddlNitProveedor" AutoPostBack="True" runat="server" OnSelectedIndexChanged="CambioNitProveedor"
					OnDataBinding="CambioNitProveedor"></asp:dropdownlist><asp:image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png" Visible="False"></asp:image></td>
		</tr>
		<TR>
			<td>Prefijo de Pedido
			</td>
			<td><asp:dropdownlist id="ddlPrefijo" AutoPostBack="True" OnSelectedIndexChanged="CambioNitProveedor"
					Runat="server"></asp:dropdownlist></td>
		</TR>
		<TR>
			<td>Número de Pedido
			</td>
			<td><asp:dropdownlist id="ddlNumPedido" AutoPostBack="True" OnSelectedIndexChanged="CargarPedido" Runat="server"
					Enabled="False"></asp:dropdownlist></td>
		</TR>
		<tr>
			<td align="center" colSpan="2"><asp:button id="btnCargarPedido" onclick="CargarPedido" Runat="server" Text="Cargar Pedido"
					Enabled="False"></asp:button></td>
		</tr>
	</tbody>
 </table>
</FIELDSET>

<P>&nbsp;</P>
<P><asp:datagrid id="dgPedido" runat="server" OnItemDataBound="DataBound_Colores" AutoGenerateColumns="False"
		onItemCommand="gridVehis">
		<Columns>
			<asp:BoundColumn DataField="pcat_codigo" HeaderText="Catalogo"></asp:BoundColumn>
			<asp:BoundColumn DataField="pcat_descripcion" HeaderText="Descripci&#243;n"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Color">
				<ItemTemplate>
					<asp:DropDownList id="ddlColor" runat="server"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Fecha Colorizaci&#243;n">
				<ItemTemplate>
					<asp:TextBox id="txtFechaColorizacion" onkeyup="DateMask(this)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"fechaColor","{0:N}")%>' Width="75px">
					</asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Fecha Fabricaci&#243;n">
				<ItemTemplate>
					<asp:TextBox id=txtFechaFabricacion onkeyup=DateMask(this) runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"fechaFabricacion","{0:N}")%>' Width="75px">
					</asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Fecha Llegada">
				<ItemTemplate>
					<asp:TextBox id=txtFechaLlegada onkeyup=DateMask(this) runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"fechaLlegada","{0:N}")%>' Width="75px">
					</asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Observaciones">
				<ItemTemplate>
					<asp:TextBox id=txtObs runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Observacion","{0:N}")%>' TextMode="MultiLine">
					</asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="estado" HeaderText="Estado"></asp:BoundColumn>
			<asp:ButtonColumn Text="Cambiar Estado" CommandName="Eliminar"></asp:ButtonColumn>
		</Columns>
	</asp:datagrid></P>
<P>Atencion : Esta información no afectara el pedido original (Estado 
	A:Activo,I:Inactivo).</P>
<asp:button id="btnIngresar" onclick="ingresar_Datos" Runat="server" Text="Ingresar Datos"></asp:button><asp:label id="error" Runat="server"></asp:label>
