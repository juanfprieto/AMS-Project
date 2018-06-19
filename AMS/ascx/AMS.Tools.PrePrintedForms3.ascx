<%@ Control Language="c#" codebehind="AMS.Tools.PrePrintedForms3.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.BuildFooter" %>
<p align="justify">
	3. Aqui seleccionaremos los datos que conforman al footer, como por ejemplo los 
	totales
	de las facturas. Tendra la opción de colocar las sumatorias que indico en el 
	paso
	2 o los totales almacenados en las entidades que almacenan las facturas. Estos 
	datos
	seran alineados a la derecha, por favor cuando se le&nbsp;soliciten las 
	posiciones,
	digite la posición&nbsp;de la margen&nbsp;horizontal:
</p>
<p align="justify">
	Tabla u otra opcion :
	<asp:DropDownList id="ddlTablas" AutoPostBack="true" OnSelectedIndexChanged="Cambio_ddlTablas" runat="server"></asp:DropDownList>
</p>
<p align="justify">
	Campo o sumatoria :
	<asp:DropDownList id="ddlCampos" runat="server"></asp:DropDownList>
</p>
<p align="justify">
	<asp:Button id="btnCnf" onclick="Configurar_Campo" runat="server" Text="Configurar"></asp:Button>
</p>
<p align="justify">
	<asp:PlaceHolder id="plcConf" runat="server" Visible="False">
		<P>
			<TABLE class="main" width="700">
				<TR>
					<TD colSpan="3">Tipo de Dato : <asp:Label id="lbTpDat" runat="server"></asp:Label></TD></TR>
				<TR>
					<TD>Posicion Margen Horizontal : <asp:TextBox id="posX" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPosX" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic" ControlToValidate="posX">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPosX2" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic" ControlToValidate="posX" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>&nbsp;cms</TD>
					<TD>Posicion Vertical : <asp:TextBox id="posY" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPosY" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic" ControlToValidate="posY">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPosY2" runat="server" Font-Size="11" Font-Name="Arial" Display="Dynamic" ControlToValidate="posY" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>&nbsp;cms</TD>
					<TD>Mascara : <asp:DropDownList id="ddlMascara" runat="server"></asp:DropDownList></TD></TR>
				<TR>
					<TD colSpan="2">Etiqueta del Campo : <asp:TextBox id="etiqCamp" runat="server" Wclass="tpequeno"></asp:TextBox></TD>
					<TD>Alineación del Valor : <asp:DropDownList id="ddlAlineacion" runat="server"></asp:DropDownList></TD></TR></TABLE></P>
		<P><asp:Button id="btnAcp1" onclick="Aceptar_Configuracion" runat="server" Text="Aceptar"></asp:Button>&nbsp;
			<asp:Button id="btnCnl" onclick="Cancelar_Configuracion" runat="server" Text="Cancelar"></asp:Button></P>
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
<p align="justify">
</p>
<p align="justify">
	<asp:DataGrid id="dgCampos" runat="server" cssclass="datagrid" AutoGenerateColumns="False" GridLines="Vertical" OnDeleteCommand="EliminarCampo">
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
<p align="justify">
	Guardar Formato Como :&nbsp;
	<asp:TextBox id="nomFormato" runat="server" class="tpequeno" MaxLength="50"></asp:TextBox>
	&nbsp;&nbsp;&nbsp;
	<asp:Button id="btnSave" onclick="Guardar_Formato" runat="server" Text="Guardar Forma"></asp:Button>
</p>
<p align="justify">
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
