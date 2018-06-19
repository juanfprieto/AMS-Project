<%@ Control Language="c#" codebehind="AMS.Contabilidad.AjusInflacion.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.AjusInflacion" %>
<p>
	<asp:Label id="Label1" width="680px" runat="server"> Este proceso ejecuta la inflación
    a las cuentas definidas en el plan de cuentas que están definidas como 'no monetarias',
    'inflación y corrección'; para ello debe definir previamente el porcentaje del PAAG
    en la sección configuración/Parámetros Contabilidad/PAAG, válidos para este año y
    mes vigente. </asp:Label>
</p>
<p>
	Documento:&nbsp;
	<asp:DropDownList id="typeDoc" runat="server"></asp:DropDownList>
	&nbsp;&nbsp;&nbsp;&nbsp; Año:
	<asp:DropDownList id="year" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Fecha"></asp:DropDownList>
	&nbsp;&nbsp;&nbsp; Mes:
	<asp:DropDownList id="month" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Fecha"></asp:DropDownList>
</p>
<p>
	<asp:Label id="valorPaag" width="129px" runat="server" height="15px"></asp:Label>
</p>
<p>
	<asp:Button id="Efectuar" onclick="efectuar_ajuste" runat="server" Text="Efectuar"></asp:Button>
</p>
<asp:Label id="lb" width="680px" runat="server"></asp:Label>
<asp:Datagrid id="Grid" runat="server" cssclass="datagrid" AutoGenerateColumns="False" align="center">
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle cssclass="header"></HeaderStyle>
	<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="CODIGO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="NOMBRE DE LA CUENTA">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="INFLACION %">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "INFLACION","{0:N}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="SALDO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "SALDO","{0:N}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="VALOR CALCULADO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALORCALCULADO","{0:N}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="NATURALEZA CUENTA">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NATURALEZA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="CUENTA AJUSTE">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "AJUSTE") %>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:Datagrid>
<br>
<br>
<asp:Datagrid id="header" runat="server" cssclass="datagrid" AutoGenerateColumns="False" align="center">
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle cssclass="header"></HeaderStyle>
	<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alernate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="TIPO DOCUMENTO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "TIPO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="NUMERO DE DOCUMENTO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="AÑO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "ANO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="MES">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MES") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="FECHA COMPROBANTE">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="RAZON">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "RAZON") %>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:Datagrid>
<br>
<br>
<asp:Datagrid id="comprobanteG" runat="server" cssclass="datagrid" AutoGenerateColumns="False" align="center">
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle cssclass="header"></HeaderStyle>
	<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="CUENTA">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CUENTA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="NIT">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NIT") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="PREF">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "PREF") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="DOCREF">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "DOC_REF") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="DETALLE">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "DETALLE") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="SEDE">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "SEDE") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="C.COSTO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CENTRO_COSTO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="DEBITO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "DEBITO","{0:N}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="CREDITO">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CREDITO","{0:N}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="BASE">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "BASE","{0:N}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:Datagrid>
<br>
<div align="center">
	<asp:Button id="guardar" onclick="guardar_comprobante" runat="server" Text="Guardar Comprobante"
		Visible="False"></asp:Button>
</div>
<div align="center">
	<br>
	&nbsp;
</div>
