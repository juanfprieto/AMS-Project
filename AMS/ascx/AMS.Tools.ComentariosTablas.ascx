<%@ Control Language="c#" codebehind="AMS.Tools.ComentariosTablas.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.ComentariosTablas" %>
<fieldset>
	<legend>Escoja una tabla y/o campo para realizar la busqueda 
    </legend>
	<table>
		<tbody>
			<tr>
				<td>
					Tabla
				</td>
				<td>
					<asp:DropDownList id="ddltabla" AutoPostBack="True" onSelectedIndexChanged="ddltabla_SelectedIndexChanged"
						runat="server"></asp:DropDownList>
				</td>
				<td>
					<asp:CheckBox id="chbtabla" runat="server" Text="Incluir en la busqueda"></asp:CheckBox>
				</td>
			</tr>
			<tr>
				<td>
					Campo
				</td>
				<td>
					<asp:DropDownList id="ddlcampo" runat="server"></asp:DropDownList>
				</td>
				<td>
					<asp:CheckBox id="chbcampo" runat="server" Text="Incluir en la busqueda"></asp:CheckBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button id="btnBuscar" onclick="btnBuscar_Click" runat="server" Text="Buscar"></asp:Button>
				</td>
				<td>
				</td>
				<td>
				</td>
			</tr>
		</tbody>
	</table>
</fieldset>
<P>
	<asp:LinkButton id="lnbimp1" runat="server" Visible="False" onclick="lnbimp1_Click">Imprimir</asp:LinkButton>
<P>
<P>
	<asp:PlaceHolder id="ph" runat="server">
		<P>
			<asp:DataGrid id="dgTools" runat="server" OnItemCommand="dgTools_ItemCommand" onEditCommand="dgTools_EditCommand"
				onUpdateCommand="dgTools_UpdateCommand" onCancelCommand="dgTools_CancelCommand" PageSize="25"
				AutoGenerateColumns="False" CssClass="datagrid">
				<FooterStyle CssClass="footer"></FooterStyle>
				<HeaderStyle CssClass="header"></HeaderStyle>
				<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
				<ItemStyle CssClass="item"></ItemStyle>
				<Columns>
					<asp:BoundColumn DataField="TABLA" ReadOnly="True" HeaderText="Tabla"></asp:BoundColumn>
					<asp:BoundColumn DataField="CAMPO" ReadOnly="True" HeaderText="Campo"></asp:BoundColumn>
					<asp:BoundColumn DataField="COMENTARIO" ReadOnly="True" HeaderText="Comentario"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Ayuda Contextual">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem,"AYUDA") %>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="tbcom" runat="server" TextMode="MultiLine" Width="400" Text='<%# DataBinder.Eval(Container.DataItem,"AYUDA")%>' />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" HeaderText="Editar/Actualizar" CancelText="Cancelar"
						EditText="Editar"></asp:EditCommandColumn>
					<asp:TemplateColumn HeaderText="Imprimir">
						<ItemTemplate>
							<center>
								<asp:ImageButton ID="imgbtnimp" Runat="server" ImageUrl="../img/AMS.Icon.Printer.png" CommandName="imprimir" AlternateText="Imprimir Información de esta fila"></asp:ImageButton>
							</center>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid></P>
	</asp:PlaceHolder>
<P>
	<asp:LinkButton id="lnbimp2" runat="server" Visible="False" onclick="lnbimp1_Click">Imprimir</asp:LinkButton>
<P></P>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
