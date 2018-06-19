<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Garantias.Mantenimientos.ascx.cs" Inherits="AMS.Garantias.AMS_Garantias_Mantenimientos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<P><asp:label id="lblUsuario" DESIGNTIMEDRAGDROP="2209" Width="8px" runat="server">Label</asp:label>&nbsp;Nit 
	:<asp:label id="lblNitDealer" Width="8px" runat="server" Height="8px">Label</asp:label></P>
<P><asp:label id="lblCiudad" runat="server">Label</asp:label></P>
<asp:panel id="Panel1" Width="459px" runat="server" Height="304px" BorderStyle="Solid" BorderColor="LightSteelBlue"
	BorderWidth="1px">
	<P>Datos de la Orden</P>
	<P>
		<TABLE id="Table2" class="filtersIn">
			<TR>
				<TD bgColor="whitesmoke" colSpan="1" rowSpan="1">Taller</TD>
				<TD bgColor="#f5f5f5" colSpan="6">
					<asp:Label id="lbTaller" runat="server" class="lmediano">Label</asp:Label></TD>
			</TR>
			<TR>
				<TD bgColor="#ffffff">Prefijo&nbsp;&nbsp;&nbsp;
				</TD>
				<TD bgColor="#ffffff">
					<asp:Label id="lbPrefOrden" runat="server" Width="80px">Label</asp:Label></TD>
				<TD bgColor="#ffffff">Nº Orden
				</TD>
				<TD bgColor="#ffffff">
					<asp:TextBox id="tbNumorden" runat="server" class="tpequeno"></asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbNumorden"
						ValidationExpression="\d+"></asp:RegularExpressionValidator></TD>
			</TR>
			<TR>
				<TD bgColor="#f5f5f5" colSpan="1" rowSpan="1">Placa</TD>
				<TD bgColor="#f5f5f5">
					<asp:TextBox id="tbPlaca" runat="server" class="tpequeno"></asp:TextBox>
					<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbPlaca"></asp:RequiredFieldValidator></TD>
				<TD bgColor="#f5f5f5">Kilometraje</TD>
				<TD bgColor="#f5f5f5">
					<asp:TextBox id="tbkilometraje" runat="server" Width="111px" CssClass="AlineacionDerecha"></asp:TextBox>
					<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbkilometraje"></asp:RequiredFieldValidator></TD>
			</TR>
			<TR>
				<TD bgColor="#ffffff">
					<P>Fecha&nbsp; Entrada
					</P>
				</TD>
				<TD bgColor="#ffffff">
					<asp:TextBox id="tbFechaEntrada" onkeyup="DateMask(this);" runat="server" class="tpequeno"></asp:TextBox>
					<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="tbFechaEntrada"></asp:RequiredFieldValidator></TD>
				<TD style="WIDTH: 62px; HEIGHT: 43px" bgColor="#ffffff">Hora:</TD>
				<TD style="WIDTH: 157px; HEIGHT: 43px" bgColor="#ffffff">
					<asp:TextBox id="tbHoraEntrada" runat="server" class="tpequeno"></asp:TextBox>
					<asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="tbHoraEntrada"></asp:RequiredFieldValidator>&nbsp;:
					<asp:TextBox id="tbMinutoEntrada" runat="server" Width="56px"></asp:TextBox>
					<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="tbMinutoEntrada"></asp:RequiredFieldValidator></TD>
			</TR>
			<TR>
				<TD bgColor="#f5f5f5">Fecha Entrega</TD>
				<TD bgColor="#f5f5f5">
					<asp:TextBox id="tbFechaEntrega" onkeyup="DateMask(this);" runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="tbFechaEntrega"></asp:RequiredFieldValidator></TD>
				<TD bgColor="#f5f5f5">Hora:</TD>
				<TD bgColor="#f5f5f5">
					<asp:TextBox id="tbHoraEntrega" runat="server" class="tpequeno"></asp:TextBox>&nbsp;
					<asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server" ErrorMessage="*" ControlToValidate="tbHoraEntrega"></asp:RequiredFieldValidator>:
					<asp:TextBox id="tbMinutoEntrega" runat="server" class="tpequeno"></asp:TextBox>
					<asp:RequiredFieldValidator id="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="tbMinutoEntrega"></asp:RequiredFieldValidator></TD>
			</TR>
			<TR>
				<TD bgColor="#ffffff">Observaciones Cliente</TD>
				<TD bgColor="#ffffff" colSpan="3">
					<asp:textbox id="obsrCliente" runat="server" class="tmediano" TextMode="MultiLine"></asp:textbox></TD>
			</TR>
			<TR>
				<TD bgColor="#f5f5f5">Observaciones 
					Recepcionista</TD>
				<TD bgColor="#f5f5f5" colSpan="3">
					<asp:textbox id="obsrRecep" runat="server" class="tmediano" TextMode="MultiLine"></asp:textbox></TD>
			</TR>
		</TABLE>
	</P>
	<asp:Button id="btConfirmar" runat="server" class="bpequeno" Text="Confirmar" onclick="btConfirmar_Click"></asp:Button>
</asp:panel>
<P><asp:panel id="Panel4" Width="536px" runat="server" Height="274px" HorizontalAlign="Left" Visible="False">&nbsp;</P>
<P><asp:label id="Label3"  runat="server" BackColor="WhiteSmoke" ForeColor="MidnightBlue">Datos del vehiculo</asp:label></P>
<P>Nombre del cliente:
	<asp:label id="tbCliente"  runat="server">NomCliente</asp:label>Nit:&nbsp;
	<asp:label id="tbDoc" class="lgrande" runat="server">DocCliente</asp:label></P>
<P>
	<TABLE id="Table1" class="filtersIn">
		<TR>
			<TD bgColor="whitesmoke" colSpan="1" rowSpan="1">Catálogo 
				:</TD>
			<TD bgColor="whitesmoke" colSpan="1" rowSpan="1"><asp:label id="lbPcatcodigo" class="lpequeno" runat="server">codigocat</asp:label></TD>
			<TD bgColor="whitesmoke" colSpan="4"><asp:label id="tbCatalogo" class="lmediano" runat="server">Catalogo</asp:label></TD>
		</TR>
		<TR>
			<TD bgColor="#ffffff">Año Modelo:</TD>
			<TD bgColor="#ffffff"><asp:label id="lbAModeloV" class="lpequeno" runat="server">AModeloV</asp:label></TD>
			<TD bgColor="#ffffff">Marca:</TD>
			<TD bgColor="#ffffff"><asp:label id="lbMarca" class="lmediano" runat="server">Marca</asp:label></TD>
		</TR>
		<TR>
			<TD bgColor="#f5f5f5">Color:</TD>
			<TD bgColor="#f5f5f5"><asp:label id="lbColorV" class="lpequeno" runat="server">ColorVeh</asp:label></TD>
			<TD bgColor="#f5f5f5">Motor:</TD>
			<TD bgColor="whitesmoke"><asp:label id="lbMotorV" class="lmediano" runat="server">MotorV</asp:label></TD>
		</TR>
		<TR>
			<TD bgColor="#ffffff">Concesionario</TD>
			<TD bgColor="#ffffff" colSpan="3"><asp:label id="lbConcesionario" Width="472px" runat="server">Concesionario</asp:label></TD>
		</TR>
		<TR>
			<TD bgColor="#f5f5f5"><asp:button id="btAceptar" class="bpeuqeno" runat="server" Text="Aceptar" onclick="btAceptar_Click"></asp:button></TD>
			<TD bgColor="#f5f5f5" colSpan="4"><asp:button id="btCancelar"  class="bpeuqeno"" runat="server" Text="Cancelar" onclick="btCancelar_Click"></asp:button></TD>
		</TR>
	</TABLE>
</P>
<P></asp:panel></P>
<P><asp:panel id="Panel3" Width="584px" runat="server" Height="260px">Kits o Combos</P>
<P></P>
<TABLE id="Table3" class="filtersIn">
	<TR>
		<TD bgColor="#f5f5f5">Codigo</TD>
		<TD bgColor="#f5f5f5">Nombre</TD>
		<TD bgColor="#f5f5f5">Seleccionar</TD>
		<TD align="center" bgColor="#f5f5f5">Ver Kit</TD>
	</TR>
</TABLE>
<div id="dv" style="OVERFLOW: auto; WIDTH: 536px; HEIGHT: 188px"><asp:datagrid id="dgKits" Width="512px" runat="server" AutoGenerateColumns="False">
		<Columns>
			<asp:BoundColumn DataField="pkit_codigo"></asp:BoundColumn>
			<asp:BoundColumn DataField="PKIT_NOMBRE"></asp:BoundColumn>
			<asp:TemplateColumn>
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
				<ItemTemplate>
					<asp:Button id="btSelectKit" runat="server" Width="82px" Text="Seleccionar" CommandName="SelectKit"></asp:Button>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
				<ItemTemplate>
					<asp:Label id="lbVerKit" runat="server" CssClass="PunteroMano">Ver Detalle</asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></div>
<P></P>
<P></asp:panel></P>
<P>Items</P>
<P><asp:datagrid id="dgItems" runat="server" AutoGenerateColumns="False" ShowFooter="True">
		<Columns>
			<asp:TemplateColumn HeaderText="C&#243;digo del Item">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "Codigo") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodItem" onclick="ModalDialog(this,'Select  DBXSCHEMA.EDITARREFERENCIAS(MI.MITE_CODIGO,PLI.PLIN_TIPO) as Codigo, mi.mite_nombre as Item from MITEMS mi inner join PLINEAITEM PLI on   MI.PLIN_CODIGO=PLI.PLIN_CODIGO', new Array(),1)"
						runat="server" class="tmediano" ReadOnly="True"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre del Item">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "Item") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodItema" runat="server" ReadOnly="True"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cant">
				<ItemTemplate>
					<asp:TextBox id="tbCantItem" runat="server" class="tpequeno" text='<%# DataBinder.Eval(Container.DataItem,"cantidad") %>' >0</asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Unitario">
				<ItemTemplate>
					<asp:TextBox id=tbValorItem runat="server" class="tpequeno" CssClass="AlineacionDerecha" text='<%# DataBinder.Eval(Container.DataItem,"ValorIt") %>'>0</asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" ErrorMessage="*" ControlToValidate="tbValorItem"
						ValidationExpression="\d+"></asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Seleccionado">
				<ItemTemplate>
					<asp:Button id="btRemoverItem" runat="server" class="bpequeno"" Text="Remover" CommandName="BorrarItem"></asp:Button>
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button id="btAgregarItem" runat="server" class="bpequeno" Text="Agregar" CommandName="AgregarItem"></asp:Button>
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></P>
<P>Operaciones</P>
<P><asp:datagrid id="dgOpers" runat="server" AutoGenerateColumns="False" ShowFooter="True">
		<Columns>
			<asp:TemplateColumn HeaderText="C&#243;digo de operaci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ptem_operacion") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodOper" onclick="ModalDialog(this,'Select  ptem_operacion AS Codigo,ptem_descripcion Operacion  from  ptempario', new Array(),1)"
						runat="server"class="tpequeno" ReadOnly="True"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nombre de Operaci&#243;n ">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ptem_descripcion") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodOpera" runat="server" class="tmediano" ReadOnly="True"></asp:TextBox>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Unitario">
				<ItemTemplate>
					<asp:TextBox id=tbValoper runat="server" class="tpequeno" CssClass="AlineacionDerecha" text='<%# DataBinder.Eval(Container.DataItem,"Valor") %>'>0</asp:TextBox>
					<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbValoper"
						ValidationExpression="\d+"></asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Seleccionado">
				<ItemTemplate>
					<asp:Button id="btRemoverOper" class="bpequeno" runat="server" Text="Remover" CommandName="BorrarOper"></asp:Button>
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button id="btAgregarOper" class="bpequeno" runat="server" Text="Agregar" CommandName="AgregarOper"></asp:Button>
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></P>
<P><asp:button id="btGuardar" runat="server" Text="Guardar" onclick="btGuardar_Click"></asp:button></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
<P>&nbsp;</P>
