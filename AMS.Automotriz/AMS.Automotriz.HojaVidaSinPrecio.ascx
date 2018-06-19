<%@ Control Language="c#" codebehind="AMS.Automotriz.HojaVidaSinPrecio.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.HojaVidaSinPrecio" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<table class="filters">
	<tr>
		<th class="filterHead">
				<img height="100" src="../img/AMS.Flyers.ConsultarHojas.png" border="0">
			</th>
		<td>
			<table class="filters">
				<tr>
					<td colspan="4">
						Datos del vehículo a consultar
					</td>
				</tr>
				<tr>
					<td>VIN :</td>
					<td><asp:TextBox id="txtVin" class="tmediano" runat="server"></asp:TextBox></td>
					<td>Número de Placa :</td>
					<td><asp:TextBox id="txtPlaca" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Motor :</td>
					<td><asp:TextBox id="txtMotor" class="tmediano" runat="server"></asp:TextBox></td>
					<td>NIT Propietario :</td>
					<td><asp:TextBox id="txtNit" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Serie :</td>
					<td><asp:TextBox id="txtSerie" class="tmediano" runat="server"></asp:TextBox></td>
					<td>Chasis :</td>
					<td><asp:TextBox id="txtChasis" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Color :</td>
					<td><asp:dropdownlist id="ddlColor" Runat="server" AutoPostBack="false"></asp:dropdownlist></td>
					<td>Año Modelo :</td>
					<td><asp:TextBox id="txtAno" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Tipo de Servicio :</td>
					<td><asp:dropdownlist id="ddlServicio" Runat="server" AutoPostBack="false"></asp:dropdownlist></td>
					<td>Vencimiento Seguro :</td>
					<td><asp:TextBox id="txtSeguro" onkeyup="DateMask(this)" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Concesionario Vendedor :</td>
					<td><asp:TextBox id="txtConcesionario" class="tmediano" runat="server"></asp:TextBox></td>
					<td>Fecha Venta :</td>
					<td><asp:TextBox id="txtVenta" onkeyup="DateMask(this)" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Kilometraje Venta :</td>
					<td><asp:TextBox id="txtKilometrajeV" class="tmediano" runat="server"></asp:TextBox></td>
					<td>Número Radio :</td>
					<td><asp:TextBox id="txtRadio" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Último Kilometraje :</td>
					<td><asp:TextBox id="txtKilometrajeU" class="tmediano" runat="server"></asp:TextBox></td>
					<td>Kilometraje Promedio :</td>
					<td><asp:TextBox id="txtKilometrajeP" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Categoria :</td>
					<td><asp:TextBox id="txtCategoria" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>
						<asp:Button id="consultarHoja" onclick="Consultar_Hoja" Text="Consultar Hoja" Width="215px"
							runat="server"></asp:Button>
					</td>
				</tr>
			</table>
		</td>
	</tr>

		<td colspan="2">
			<asp:DataGrid id="dgrCatalogos" runat="server" AutoGenerateColumns="false" OnItemCommand="dgTable_Procesos" GridLines="Vertical"
				DataKeyField="MCAT_PLACA" cssclass="datagrid">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
				<Columns>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:ImageButton id="btEdit" ImageUrl="../img/Edit.jpg" AlternateText="Editar Registro" Runat="server"
								CommandName="Edit" Height="18px"></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="MCAT_VIN" HeaderText="VIN"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_PLACA" HeaderText="PLACA"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_MOTOR" HeaderText="MOTOR"></asp:BoundColumn>
					<asp:BoundColumn DataField="MNIT_NIT" HeaderText="PROPIETARIO"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_SERIE" HeaderText="SERIE"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_CHASIS" HeaderText="CHASIS"></asp:BoundColumn>
					<asp:BoundColumn DataField="PCOL_CODIGO" HeaderText="COLOR"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_ANOMODE" HeaderText="AÑO"></asp:BoundColumn>
					<asp:BoundColumn DataField="TSER_TIPOSERV" HeaderText="TIPO"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_VENCSEGUOBLI" HeaderText="VENCE<BR>SEGURO" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_CONCVEND" HeaderText="CONCESIONARIO"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_VENTA" HeaderText="VENTA" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_NUMEKILOVENT" HeaderText="KMS<BR>VENTA"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_NUMERADIO" HeaderText="RADIO"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_NUMEULTIKILO" HeaderText="KMS<BR>ULT."></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_NUMEKILOPROM" HeaderText="KMS<BR>PROM."></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_CATEGORIA" HeaderText="CATEG."></asp:BoundColumn>
				</Columns>
			</asp:DataGrid>
		</td>

<asp:Label id="lb" runat="server"></asp:Label>
<P></P>

