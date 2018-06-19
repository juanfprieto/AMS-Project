<%@ Control Language="c#" codebehind="AMS.Reportes.FiltrosReporte.ascx.cs" autoeventwireup="True" Inherits="AMS.Reportes.Filtros" %>
<p>
	Condicionales : Estos condicionales son parametros estaticos de la consulta que 
	son
	estrictamente necesarios cada vez que se vaya ha realizar cualquier reporte, 
	estos
	condicionales pueden ser la tabla principal o de cualquier tabla relacionada :
</p>
<p>
	Tabla :
	<asp:DropDownList id="ddlTablas" runat="server" OnSelectedIndexChanged="Cambio_Tabla" AutoPostBack="True"></asp:DropDownList>
</p>
<p>
	Campos :
	<asp:DropDownList id="ddlCampos" runat="server"></asp:DropDownList>
</p>
<p>
	<asp:Button id="btCnfg1" onclick="Configurar_Filtros" runat="server" Text="Configurar Condicional"></asp:Button>
</p>
<p>
	<asp:PlaceHolder id="confCondi" runat="server">
		<TABLE>
			<TR>
				<TD colSpan="2">Tipo de Dato : <asp:Label id="tipoDatos" runat="server"></asp:Label></TD></TR>
			<TR>
				<TD>Valor a&nbsp;Comparar&nbsp;: <asp:TextBox id="vlCmp" runat="server" Width="102px"></asp:TextBox><asp:RequiredFieldValidator id="validatoVlCmp" runat="server" Display="Dynamic" ControlToValidate="vlCmp" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator></TD>
				<TD>Opciones de Comparación&nbsp;: <asp:DropDownList id="oprCmp" runat="server"></asp:DropDownList></TD></TR></TABLE>
		<P><asp:Button id="btnAcpt" onclick="Aceptar_Filtro" runat="server" Text="Aceptar"></asp:Button>&nbsp;
			<asp:Button id="btnCnl" onclick="Cancelar_Filtro" runat="server" Text="Cancelar" CausesValidation="False"></asp:Button></P>
	</asp:PlaceHolder>
<P></P>
<p>
</p>
<p>
	<asp:DataGrid id="dgFltrRpt" runat="server" Width="650px" Font-Name="Verdana" Font-Size="8pt" AutoGenerateColumns="false" Font-Names="Verdana" BorderWidth="1px" GridLines="Vertical" BorderStyle="None" BackColor="White" BorderColor="#999999" CellPadding="3" HeaderStyle-BackColor="#ccccdd" OnDeleteCommand="EliminarCondicional">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="TABLA" HeaderText="TABLA"></asp:BoundColumn>
			<asp:BoundColumn DataField="CAMPO" HeaderText="CAMPO"></asp:BoundColumn>
			<asp:BoundColumn DataField="TIPOCOMPARACION" HeaderText="TIPO DE COMPARACIÓN"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALOR" HeaderText="VALOR"></asp:BoundColumn>
			<asp:BoundColumn DataField="TIPODATO" HeaderText="TIPO DE DATO"></asp:BoundColumn>
			<asp:ButtonColumn Text="Eliminar" ButtonType="PushButton" HeaderText="ELIMINAR CAMPO" CommandName="Delete"></asp:ButtonColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
</p>
<p>
	Filtros Dinamicos : Estos son condicionales que varian para la construccion 
	dinamica
	del reporte o consulta, es decir, son condicionales variables, opciones de 
	consulta.&nbsp;Estas
	opciones son estrictamente de igualdad:
</p>
<p>
	Tabla :
	<asp:DropDownList id="ddlTablas2" runat="server" OnSelectedIndexChanged="Cambio_Tabla2" AutoPostBack="True"></asp:DropDownList>
</p>
<p>
	Campos :
	<asp:DropDownList id="ddlCampos2" runat="server"></asp:DropDownList>
</p>
<p>
	<asp:Button id="btCnfg2" onclick="Configurar_Filtros2" runat="server" Text="Configurar Filtro"></asp:Button>
</p>
<p>
</p>
<p>
	<asp:PlaceHolder id="confFiltro" runat="server">
		<TABLE>
			<TR>
				<TD colSpan="2">Tipo de Dato : <asp:Label id="tipoDatos2" runat="server"></asp:Label></TD></TR>
			<TR>
				<TD>Etiqueta : <asp:TextBox id="vlCmp2" runat="server" Width="102px"></asp:TextBox><asp:RequiredFieldValidator id="validatoVlCmp2" runat="server" Display="Dynamic" ControlToValidate="vlCmp2" Font-Size="11" Font-Name="Arial">*</asp:RequiredFieldValidator></TD>
				<TD>Opciones de Comparación&nbsp;: <asp:DropDownList id="oprCmp2" runat="server"></asp:DropDownList></TD></TR>
			<TR>
				<TD colSpan="2"><asp:CheckBox id="chkValInt" runat="server" Text="Mostrar Valor Interpretado?" Checked="True" TextAlign="Left" Visible="False"></asp:CheckBox></TD></TR></TABLE>
		<P><asp:Button id="btnAcpt2" onclick="Aceptar_Filtro2" runat="server" Text="Aceptar"></asp:Button>&nbsp;
			<asp:Button id="btnCnl2" onclick="Cancelar_Filtro2" runat="server" Text="Cancelar" CausesValidation="False"></asp:Button></P>
	</asp:PlaceHolder>
<P></P>
<p>
</p>
<p>
</p>
<p>
	<asp:DataGrid id="dgFltrRpt2" runat="server" Width="650px" Font-Name="Verdana" Font-Size="8pt" AutoGenerateColumns="false" Font-Names="Verdana" BorderWidth="1px" GridLines="Vertical" BorderStyle="None" BackColor="White" BorderColor="#999999" CellPadding="3" HeaderStyle-BackColor="#ccccdd" OnDeleteCommand="EliminarFiltro">
		<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
		<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
		<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
		<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
		<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="TABLA" HeaderText="TABLA"></asp:BoundColumn>
			<asp:BoundColumn DataField="CAMPO" HeaderText="CAMPO"></asp:BoundColumn>
			<asp:BoundColumn DataField="TIPOCOMPARACION" HeaderText="TIPO DE COMPARACIÓN"></asp:BoundColumn>
			<asp:BoundColumn DataField="TIPODATO" HeaderText="TIPO DE DATO"></asp:BoundColumn>
			<asp:BoundColumn DataField="CONTROLASOCIADO" HeaderText="CONTROL ASOCIADO"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALORINTERPRETADO" HeaderText="VALOR INTERPRETADO"></asp:BoundColumn>
			<asp:ButtonColumn Text="Eliminar" ButtonType="PushButton" HeaderText="ELIMINAR CAMPO" CommandName="Delete"></asp:ButtonColumn>
		</Columns>
	</asp:DataGrid>
</p>
<p>
</p>
<p>
	<asp:Button id="btnAptFn" onclick="Construir_Consulta" runat="server" Text="Aceptar Condicionales y Filtros" Visible="False"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
