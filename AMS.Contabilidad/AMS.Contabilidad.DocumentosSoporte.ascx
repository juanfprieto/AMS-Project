<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Contabilidad.DocumentosSoporte.ascx.cs" Inherits="AMS.Contabilidad.AMS_Contabilidad_DocumentosSoporte" %>

<script type="text/javascript">
    
    function emergenteNumeros(numero)
    {
        var num = document.getElementById('<%= txtNumero.ClientID%>')
        ModalDialog(num, "SELECT MCOM_NUMEDOCU, PDOC_CODIGO || \' - \' || MCOM_NUMEDOCU AS COMPROBANTE, PANO_ANO, PMES_MES, MCOM_FECHA FROM MCOMPROBANTE  WHERE PDOC_CODIGO = '" + document.getElementById("<%= ddlPrefijo.ClientID %>").value + "'");
    }
    function abrirEmergente()
    {
        var ddl = document.getElementById('<%=ddlPrefijo.ClientID%>');
        ModalDialog(ddl, 'SELECT PDOC_CODIGO, PDOC_CODIGO || \' - \' || TDOC_TIPODOCU AS TIPO_DOCUMENTO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TVIG_VIGENCIA = \'V\';', new Array());
    }
</script>

<fieldset>
    <asp:DropDownList ID="ddlPrefijo" runat="server" Width="450px" ></asp:DropDownList><asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente();"></asp:Image> <br />
    <asp:TextBox ID="txtNumero" runat="server" ondblclick="emergenteNumeros();" CssClass="tpequeno" placeholder="doble click"></asp:TextBox><br />
    <asp:Button id="btnBuscar" runat="server" Text="Buscar Documentos" OnClick="cambioPrefijo"/> <br />
    <asp:DataGrid ID="dgDocumentos" runat="server" cssclass="datagrid" OnItemCommand="descargarDocumento"
		CellPadding="3" ShowFooter="True" AutoGenerateColumns="false">
		<SelectedItemStyle cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
		<FooterStyle cssclass="footer"></FooterStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Prefijo Documento">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PREFIJO", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Numero Documento">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "NUMERO", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Nombre Documento Soporte">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "MCOD_NOMBDOCUMENTO", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Nombre Archivo en Servidor">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "MCOD_NOMBARCHIVO", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descargar">
				<ItemTemplate>
					<asp:ImageButton id="imgDescargar" runat="server" ImageUrl="../img/img_desc.png" />
				</ItemTemplate>
			</asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</fieldset>