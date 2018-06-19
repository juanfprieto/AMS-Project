<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.VIP.Prueba1.ascx.cs" Inherits="AMS.VIP.Prueba1" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>

<asp:PlaceHolder ID="contenedor1" runat="server">
	<fieldset>
		<table>
			<tr>
				<td>Seleccione un usuario: 
					<asp:DropDownList ID="checkUser" runat="server" Width="305px" AutoPostBack="true" style="border:2px; padding: 3px;" OnSelectedIndexChanged="ddldetails_SelectedIndexChanged">
						
					 </asp:DropDownList> 
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="txtNitUsuario" runat="server" style="width:5em; height:25px" text="Tipo de Perfil: "> </asp:Label>
					<asp:TextBox ID="nit_usuario" runat="server" style="width:215px; margin-left:50px; border:2px; padding: 3px" />    
				</td>
			</tr>
			<tr>
			<td>
				<asp:Label ID="txtClaveUsuario" runat="server" style="height:25px" text="Digite su clave: " />
				<asp:TextBox ID="password" runat="server" style=" padding: 3px; width:40px; margin-left:41px; border:2px" textMode="Password" /></td>
			</tr>
		</table>
			<br />
				<asp:Button ID="submit" runat="server" style="position:relative; left:200px; padding: 5px 35px; text-align:center; border:outset"  Text="Validar" OnClick="ValidarClave" />	
	</fieldset>
</asp:PlaceHolder>

<asp:PlaceHolder ID="contenedor2" runat="server" Visible="false">
	<fieldset >	 
		<fieldset runat="server" style="background-color:#60A1BF;  border:inherit; margin: inherit; padding: 15px; position:relative">
			<asp:Label ID="txtBienvenido" runat="server" style="font-size:44px; font-family:Arial Baltic; position:relative; border-radius:0.2em; text-align:center"/>
		</fieldset>
		<fieldset runat="server">	
			<asp:DataGrid id="dgItems" runat="server" Font-Name="Verdana"  EnableViewState="true" ShowFooter="True" ShowHeader="true" BorderColor="#999999"
						BackColor="White" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="false" style="table-layout:auto">
						<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084" Font-Size="20px"></HeaderStyle>
						<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Código">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "PDOC_CODIGO")%>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Visita" >
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DVIS_NUMEVISI", "{0:N}")%>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Nombre">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DVIS_NOMBRE")%>
								</ItemTemplate>							
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Observaciones">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "DVISC_OBSERCONTACTO")%>
								</ItemTemplate>							
							</asp:TemplateColumn>
						</Columns>
				</asp:DataGrid>
		</fieldset>
		<asp:Button ID="Button1" runat="server" style="position:relative; padding: 5px; text-align:center; border:outset; left:1200px; top:10px; font-size:22px; font-family:Sans-Serif" Text="Siguiente" OnClick="RecibirDatos"/>
	</fieldset>
</asp:PlaceHolder>

 <asp:PlaceHolder ID="contenedor3" runat="server" Visible="false">
		<fieldset runat="server">	
			<asp:DataGrid id="dgInfoUsuario" runat="server" Font-Name="Verdana"  EnableViewState="true" ShowFooter="True" ShowHeader="true" BorderColor="#999999"
						BackColor="White" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="false" style="table-layout:auto" OnItemCommand="AgregarInfoPersonal" OnCancelCommand="DGInfoUsuarioCancel" OnEditCommand="DGInfoUsuarioEdit" OnUpdateCommand="DGInfoUsuarioUpdate" OnItemDataBound="DgInfoDataBound">
						<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
						<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084" Font-Size="20px"></HeaderStyle>
						<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
						<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
						<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Cédula">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "cedula")%>
								</ItemTemplate>
                                <EditItemTemplate>
									<asp:TextBox runat="server" id="txbCedulaEdit"  width="70%" CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "cedula")%>' />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="txbCedula" runat="server" style="width:215px; margin-left:50px; border:2px; padding: 3px" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Nombre">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "nombre", "{0:N}")%>
								</ItemTemplate>
                                <EditItemTemplate>
									<asp:TextBox runat="server" id="txbNombreEdit"  width="70%" CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "nombre")%>' />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="txbNombre" runat="server" style="width:215px; margin-left:50px; border:2px; padding: 3px" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Ciudad">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "ciudad")%>
								</ItemTemplate>
                                <EditItemTemplate>
									<asp:DropDownList runat="server" id="ddlCiudadesEdit"  CssClass="AlineacionDerecha" AutoPostBack="false" class="tpequeno">
                                        <asp:ListItem Value="Bogotá" Text="Bogotá"></asp:ListItem>
										<asp:ListItem Value="Toronto" Text="Toronto"></asp:ListItem>
										<asp:ListItem Value="Ontario" Text="Ontario"></asp:ListItem>
                                    </asp:DropDownList> 
								</EditItemTemplate>
								<FooterTemplate>	
									<asp:DropDownList ID="ddlCiudades" runat="server" Width="305px" AutoPostBack="false" style="border:2px; padding: 3px;">
										<asp:ListItem Value="Bogotá" Text="Bogotá"></asp:ListItem>
										<asp:ListItem Value="Toronto" Text="Toronto"></asp:ListItem>
										<asp:ListItem Value="Ontario" Text="Ontario"></asp:ListItem>
									</asp:DropDownList> 					
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Ahorros Actuales">
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "ahorrosActuales")%>
								</ItemTemplate>
                                <EditItemTemplate>
									<asp:TextBox runat="server" id="txbAhorrosEdit"  width="70%" CssClass="AlineacionDerecha" class="tpequeno" Text='<%# DataBinder.Eval(Container.DataItem, "ahorrosActuales")%>' />
								</EditItemTemplate>
								<footertemplate>	
									<asp:TextBox ID="txbAhorros" runat="server" style="width:215px; margin-left:50px; border:0.1em; padding: 3px" onKeyUp="NumericMaskE(this,event)" />									
								</footertemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Acciones:">
								<ItemTemplate>
									<asp:Button CommandName="Delete" Text="Quitar" ID="btnDel" Runat="server"  />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" Runat="server" width="70px" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
						</Columns>
				</asp:DataGrid>

		</fieldset>
	</asp:PlaceHolder>