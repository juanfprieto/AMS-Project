<%@ Control Language="c#" codebehind="AMS.Reportes.CamposReporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Campos" %>
<p>
	A Continuación crearemos los campos del cuerpo del reporte, por favor 
	seleccione el campo de una tabla y a continuacion se le indicara como 
	configurar el campo:
</p>
<p>
	Tabla Seleccionadas :
	<asp:DropDownList id="ddlTablas" runat="server" OnSelectedIndexChanged="Cambio_Tabla" AutoPostBack="True"></asp:DropDownList>
</p>
<p>
	Campos de tabla seleccionada :
	<asp:DropDownList id="ddlCampos" runat="server"></asp:DropDownList>
</p>
<p>
	<asp:Button id="btCnfg" onclick="ConfigurarCampo" runat="server" Text="Configurar"></asp:Button>
</p>
<p>
	<asp:PlaceHolder id="configurar" runat="server">
		<TABLE>
			<TR>
				<TD colSpan="3">Tipo de Dato :
					<asp:Label id="tipoDatos" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Etiqueta del Campo :
					<asp:TextBox id="etCmp" runat="server" Width="102px"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatoRetCmp" runat="server" ControlToValidate="etCmp" Display="Dynamic" Font-Name="Arial"
						Font-Size="11">*</asp:RequiredFieldValidator></TD>
				<TD>Orden de Presentacion :
					<asp:TextBox id="ordPst" runat="server" Width="31px" ReadOnly="True"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatoOrdPst" runat="server" ControlToValidate="ordPst" Display="Dynamic"
						Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorOrdPst2" runat="server" ControlToValidate="ordPst" Display="Dynamic"
						Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:RegularExpressionValidator>&nbsp;</TD>
				<TD>Operaciones :
					<asp:DropDownList id="oprCmp" runat="server"></asp:DropDownList></TD>
				<TD>Mascaras Disponibles :
					<asp:DropDownList id="mscrVlr" runat="server"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD colSpan="3">
					<asp:CheckBox id="chkFk" runat="server" Text="Mostrar Valor Interpretado ? " TextAlign="Left"
						Visible="False"></asp:CheckBox></TD>
			</TR>
		</TABLE>
		<P>
			<asp:Button id="btnAcpt" onclick="Aceptar_Adicion" runat="server" Text="Aceptar"></asp:Button>&nbsp;
			<asp:Button id="btnCnl" onclick="Cancelar_Adicion" runat="server" Text="Cancelar" CausesValidation="False"></asp:Button></P>
	</asp:PlaceHolder>
<P></P>
<p>
</p>
<p>
</p>
<asp:DataGrid id="dgCmpsRpt" runat="server" Width="650px" Font-Size="8pt" Font-Name="Verdana"
	HeaderStyle-BackColor="#ccccdd" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None"
	GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" AutoGenerateColumns="False" OnDeleteCommand="EliminarCampo">
	<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
	<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
	<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
	<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
	<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
	<Columns>
		<asp:BoundColumn DataField="TABLA" HeaderText="TABLA"></asp:BoundColumn>
		<asp:BoundColumn DataField="NOMBRECAMPO" HeaderText="NOMBRE CAMPO"></asp:BoundColumn>
		<asp:BoundColumn DataField="ETIQUETA" HeaderText="ETIQUETA"></asp:BoundColumn>
		<asp:BoundColumn DataField="ORDEN" HeaderText="ORDEN PRESENTACION"></asp:BoundColumn>
		<asp:BoundColumn DataField="OPERACION" HeaderText="OPERACI&#211;N RELACIONADA"></asp:BoundColumn>
		<asp:BoundColumn DataField="MASCARA" HeaderText="MASCARA RELACIONADA"></asp:BoundColumn>
		<asp:BoundColumn DataField="VALORINTERPRETADO" HeaderText="VALOR INTERPRETADO"></asp:BoundColumn>
		<asp:ButtonColumn Text="Eliminar" ButtonType="PushButton" HeaderText="ELIMINAR CAMPO" CommandName="Delete"></asp:ButtonColumn>
	</Columns>
</asp:DataGrid>
<p>
	<asp:Button id="btnAcpt2" onclick="AceptarCampos" runat="server" Text="Aceptar Estos Campos"
		Visible="False" CausesValidation="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
