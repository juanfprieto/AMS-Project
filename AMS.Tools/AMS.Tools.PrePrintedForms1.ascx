<%@ Control Language="c#" codebehind="AMS.Tools.PrePrintedForms1.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.BuildHeader" %>
<p>
	Por Favor Seleccione los datos que necesita que aparezcan en el cabezote de su 
	documento,&nbsp;para
	configurar las posiciones se&nbsp;colocar la medida en centimetros con respecto 
	al
	borde superior izquierdo de la hoja&nbsp;pre-impresa:
</p>
<p>
	Tablas Asociadas :
	<asp:DropDownList id="ddlTablas" OnSelectedIndexChanged="Cambio_ddlTablas" AutoPostBack="true" runat="server"></asp:DropDownList>
</p>
<p>
	Campos :
	<asp:DropDownList id="ddlCampos" runat="server"></asp:DropDownList>
</p>
<p>
	<asp:Button id="btnCnf" onclick="Configurar_Campo" runat="server" Text="Configurar"></asp:Button>
</p>
<p>
	<asp:PlaceHolder id="plcConf" runat="server" Visible="False">
		<P>
			<TABLE id="Table" class="filtersIn">
				<TR>
					<TD colSpan="3">Tipo de Dato : <asp:Label id="lbTpDat" runat="server"></asp:Label></TD></TR>
				<TR>
					<TD>Posicion Horizontal : <asp:TextBox id="posX" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPosX" runat="server" Font-Name="Arial" Font-Size="11" ControlToValidate="posX" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPosX2" runat="server" Font-Name="Arial" Font-Size="11" ControlToValidate="posX" Display="Dynamic" ValidationExpression="[0-9\,\.]+" ASPClass="RegularExpressionValidator">*</asp:RegularExpressionValidator>&nbsp;cms</TD>
					<TD>Posicion Vertical : <asp:TextBox id="posY" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPosY" runat="server" Font-Name="Arial" Font-Size="11" ControlToValidate="posY" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPosY2" runat="server" Font-Name="Arial" Font-Size="11" ControlToValidate="posY" Display="Dynamic" ValidationExpression="[0-9\,\.]+" ASPClass="RegularExpressionValidator">*</asp:RegularExpressionValidator>&nbsp;cms</TD>
					<TD>Mascara : <asp:DropDownList id="ddlMascara" runat="server"></asp:DropDownList></TD></TR></TABLE></P>
		<P><asp:Button id="btnAcp1" onclick="Aceptar_Configuracion" runat="server" Text="Aceptar"></asp:Button>&nbsp;
			<asp:Button id="btnCnl" onclick="Cancelar_Configuracion" runat="server" Text="Cancelar" CausesValidation="False"></asp:Button></P>
	</asp:PlaceHolder>
<P></P>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
</p>
<p>
	<asp:DataGrid id="dgCampos" runat="server" cssclass="datagrid" GridLines="Vertical" AutoGenerateColumns="False" OnDeleteCommand="EliminarCampo">
		<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="TABLA" HeaderText="TABLA ASOCIADA"></asp:BoundColumn>
			<asp:BoundColumn DataField="DOCUMENTO" HeaderText="DOCUMENTO"></asp:BoundColumn>
			<asp:BoundColumn DataField="CAMPO" HeaderText="CAMPO"></asp:BoundColumn>
			<asp:BoundColumn DataField="POSX" HeaderText="POSICI&#211;N HORIZONTAL"></asp:BoundColumn>
			<asp:BoundColumn DataField="POSY" HeaderText="POSICI&#211;N VERTICAL"></asp:BoundColumn>
			<asp:BoundColumn DataField="MASCARA" HeaderText="MASCARA"></asp:BoundColumn>
			<asp:BoundColumn DataField="ORDENCOLUMNA" HeaderText="ORDEN"></asp:BoundColumn>
			<asp:BoundColumn DataField="PORCANCHOCOLUMNA" HeaderText="PORCENTAJE ANCHO DE COLUMNA"></asp:BoundColumn>
			<asp:BoundColumn DataField="SUMATORIA" HeaderText="SUMATORIA"></asp:BoundColumn>
			<asp:BoundColumn DataField="PARTEIMPRESION" HeaderText="PARTE DE IMPRESION"></asp:BoundColumn>
			<asp:BoundColumn DataField="ETIQUETA" HeaderText="ETIQUETA"></asp:BoundColumn>
			<asp:BoundColumn DataField="JUSTIFY" HeaderText="JUSTIFICACIÓN"></asp:BoundColumn>
			<asp:ButtonColumn Text="Eliminar" ButtonType="PushButton" HeaderText="ELIMINAR CAMPO" CommandName="Delete"></asp:ButtonColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
	<asp:Button id="btnCnt" onclick="Continuar" runat="server" Text="Continuar" Visible="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
