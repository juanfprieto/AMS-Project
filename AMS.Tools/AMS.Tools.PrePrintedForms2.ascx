<%@ Control Language="c#" codebehind="AMS.Tools.PrePrintedForms2.ascx.cs" autoeventwireup="True" Inherits="AMS.Tools.BuildBody" %>
<p>
	<asp:PlaceHolder id="plcVlrArea" runat="server">
		<P>A continuación vamos a configurar el detalle de esta factura : </P>
		<P>1. Dimension del area de detalle : Por favor ingrese la posicion(en
			centimetros) de inicio y fin del area de detalle </P>
		<P></P>
		<FIELDSET><LEGEND>Manejo Area de 
Deatlle</LEGEND>
			<TABLE class="main" cellSpacing="5">
				<TR>
					<TD>Posición Inicial&nbsp;Horizontal : <asp:TextBox id="posIX" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPosIX" runat="server" Display="Dynamic" ControlToValidate="posIX" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPosIX2" runat="server" Display="Dynamic" ControlToValidate="posIX" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>cms
					</TD>
					<TD>Posición Incial Vertical : <asp:TextBox id="posIY" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPosIY" runat="server" Display="Dynamic" ControlToValidate="posIY" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPosIY2" runat="server" Display="Dynamic" ControlToValidate="posIY" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>&nbsp;cms
					</TD></TR>
				<TR>
					<TD>Posición Final Horizontal : <asp:TextBox id="posFX" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPosFX" runat="server" Display="Dynamic" ControlToValidate="posFX" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPosFX2" runat="server" Display="Dynamic" ControlToValidate="posFX" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>&nbsp;cms</TD>
					<TD>Posición Final Vertical : <asp:TextBox id="posFY" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPosFY" runat="server" Display="Dynamic" ControlToValidate="posFY" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPosFY2" runat="server" Display="Dynamic" ControlToValidate="posFY" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>&nbsp;cms</TD></TR></TABLE></FIELDSET>

		<P><asp:Button id="btnAcpt1" onclick="Aceptar_Area" runat="server" Text="Aceptar"></asp:Button></P>
	</asp:PlaceHolder>
<P></P>
<p>
</p>
<asp:PlaceHolder id="plcDetalle" runat="server">
	<P>2. Elementos del detalle del documento: A continuación se muestran los
		elementos que se muestran en el detalle del documento elegido. Si se ha elegido
		la opción de dividir el detalle automaticamente, se ajustara el tamaño de cada
		columna del detalle </P>
	<P>Tablas Asociadas : <asp:DropDownList id="ddlTablas" runat="server" OnSelectedIndexChanged="Cambio_ddlTablas" AutoPostBack="true"></asp:DropDownList></P>
	<P>Campos :
		<asp:DropDownList id="ddlCampos" runat="server"></asp:DropDownList></P>
	<P><asp:Button id="btnCnf" onclick="Configurar_Campo" runat="server" Text="Aceptar"></asp:Button></P>
	<P><asp:PlaceHolder id="plcConf" runat="server" Visible="False">
<TABLE class="main" width="700">
				<TR>
					<TD colSpan="3">Tipo de Dato : <asp:Label id="lbTpDat" runat="server"></asp:Label></TD></TR>
				<TR>
					<TD>Porcentaje de Ancho de Columna <asp:TextBox id="porcAnch" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoPorcAnch" runat="server" Display="Dynamic" ControlToValidate="porcAnch" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoPorcAnch2" runat="server" Display="Dynamic" ControlToValidate="porcAnch" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>&nbsp;%</TD>
					<TD>Mascara : <asp:DropDownList id="ddlMascara" runat="server"></asp:DropDownList></TD>
					<TD>Orden de Columna : <asp:TextBox id="ordCol" runat="server" class="tpequeno"></asp:TextBox><asp:RequiredFieldValidator id="validatoOrdCol" runat="server" Display="Dynamic" ControlToValidate="ordCol" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="validatoOrdCol2" runat="server" Display="Dynamic" ControlToValidate="ordCol" Font-Size="11" Font-Name="Arial" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:RegularExpressionValidator></TD></TR>
				<TR>
					<TD colSpan="2"><asp:CheckBox id="chkSum" runat="server" Text="Mostrar Sumatoria de este Campo" Enabled="false" TextAlign="Left"></asp:CheckBox></TD>
					<TD>Alineación del Valor : <asp:DropDownList id="ddlAlineacion" runat="server"></asp:DropDownList></TD></TR></TABLE><asp:Button id="btnAcpCnf" onclick="Aceptar_Configuracion" runat="server" Text="Aceptar"></asp:Button>&nbsp; 
<asp:Button id="btnCnl" onclick="Cancelar_Configuracion" runat="server" Text="Cancelar"></asp:Button></asp:PlaceHolder></P>
	<P><asp:DataGrid id="dgCampos" runat="server" cssclass="datagrid" OnDeleteCommand="EliminarCampo" GridLines="Vertical" AutoGenerateColumns="False">
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
		</asp:DataGrid></P>
	<P><asp:Button id="btnAcpt2" onclick="Continuar_Footer" runat="server" Text="Aceptar"></asp:Button></P>
</asp:PlaceHolder>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
