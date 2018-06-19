<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="AMS.Produccion.OrdenProduccion.ascx.cs"
    Inherits="AMS.Produccion.AMS_Produccion_OrdenProduccion" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script src="../js/AMS.Web.Masks.js" type="text/javascript" language:javascript></script>
<fieldset>
<table>
    <tr>
        <td>
            Prefijo de la Orden :
        </td>
        <td>
            <asp:dropdownlist id="ddlPrefijo" runat="server" autopostback="True" onselectedindexchanged="ddlPrefijo_SelectedIndexChanged"></asp:dropdownlist>
        </td>
        <td>
            Número de la Orden :
        </td>
        <td>
            <asp:textbox id="tbNumero" runat="server" class="tpequeno"></asp:textbox>
        </td>
    </tr>
    <tr>
        <td>
            Fecha :
        </td>
        <td>
            <asp:textbox id="tbFecha" onkeyup="DateMask(this)" runat="server" class="tpequeno"></asp:textbox>
        </td>
        <td>
            Tipo de Orden:
        </td>
        <td>
            <asp:RadioButton id="rbConPrograma" Text="Orden con Programa de Producción" Checked="True" GroupName="tipoOrden" runat="server" OnCheckedChanged="tipoOrden_OnCheckedChanged" AutoPostBack="True" />
            <br />
            <asp:RadioButton id="rbSinPrograma" Text="Orden sin Programa de Producción" GroupName="tipoOrden" runat="server" OnCheckedChanged="tipoOrden_OnCheckedChanged" AutoPostBack="True" />
        </td>
    </tr>
    <asp:PlaceHolder ID="phConPrograma" runat="server">
    <tr>
        <td>
            Programa de Producción:
        </td>
        <td>
            <asp:dropdownlist id="ddlLote" runat="server"></asp:dropdownlist>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td>
            Nit de Transferencias : &nbsp;
        </td>
        <td>
            <asp:textbox id="tbnit" runat="server" onclick="ModalDialog(this,'SELECT MNIT.mnit_nit,MNIT.mnit_apellidos CONCAT \' \' CONCAT MNIT.mnit_nombres FROM DBXSCHEMA.mnit MNIT,dbxschema.pnitproduccion PNIT WHERE MNIT.mnit_nit=PNIT.pnit_nitprod ORDER BY MNIT.mnit_nit')"
                readonly="True" class="tpequeno"></asp:textbox>
        </td>
        <td>
            Planta :
        </td>
        <td>
            <asp:dropdownlist id="ddlAlmacen" runat="server"></asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td>
            Persona Responsable :
        </td>
        <td>
            <asp:dropdownlist id="ddlVendedor" runat="server"></asp:dropdownlist>
        </td>
        <td>
            Almacén Materia Prima :
        </td>
        <td>
            <asp:dropdownlist id="ddlAlmMat" runat="server"></asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
        <td>
            Prefijo del Pedido :
        </td>
        <td>
            <asp:dropdownlist id="ddlPedido" runat="server"></asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td>
            Observación :
        </td>
        <td>
            <asp:textbox id="tbObservacion" runat="server" textmode="MultiLine" class="tgrande"></asp:textbox>
        </td>
        <td>
            Prefijo de la Transferencia :
        </td>
        <td>
            <asp:dropdownlist id="ddlTransferencia" runat="server"></asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="4">
            <asp:button id="btnConfirmar" runat="server" text="Aceptar" onclick="btnConfirmar_Click"></asp:button>
        </td>
    </tr>
</table>
<p>
</p>
<asp:panel id="pnlObjetos" runat="server" visible="False">
	<TABLE>
		<TR>
			<TD>
				<asp:Label id="lblElemento" runat="server"></asp:Label></TD>
			<TD>
				<asp:DropDownList id="ddlCatalogo" Runat="server" AutoPostBack="True" onselectedindexchanged="ddlCatalogo_SelectedIndexChanged"></asp:DropDownList></TD>
			<TD>Ensamble :
			</TD>
			<TD>
				<asp:DropDownList id="ddlKit" Runat="server"></asp:DropDownList></TD>
		</TR>
		<TR>
			<TD>Cantidad a Ensamblar :
			</TD>
			<TD align="left">
				<asp:TextBox id="tbcant" onkeyup="NumericMaskE(this,event)" Runat="server" Width="50"></asp:TextBox></TD>
			<TD align="right" colSpan="2">
				<asp:Button id="btnAgregar" Runat="server" Text="Agregar" onclick="btnAgregar_Click"></asp:Button></TD>
		</TR>
	</TABLE>
	<P></P>
	<asp:datagrid id="dgProduccion" runat="server" Visible="False" FooterStyle-HorizontalAlign="Center"
		ItemStyle-HorizontalAlign="Center" CssClass="datagrid" AlternatingItemStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
		<FooterStyle CssClass="footer"></FooterStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Cat&#225;logo">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CATALOGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad en Producci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem,"CANTPROD", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad en Stock">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CANTSTOCK", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Pedidos de Clientes">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PEDCLI", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Disponibilidad">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "DISP", "{0:N}") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad a Producir">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "CANTPROC", "{0:N}")%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Ensamble">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ENSAMBLE")%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Agregar/Eliminar">
				<ItemTemplate>
					<asp:Button ID="btnDel" CommandName="del" Text="Remover" Runat="server"></asp:Button>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid>
</asp:panel>
<p>
    <asp:button id="btnGrabar" text="Guardar Orden" runat="server" enabled="False" onclick="btnGrabar_Click"></asp:button>
    &nbsp;&nbsp;<asp:button id="btnCancelar" text="Cancelar" runat="server" onclick="btnCancelar_Click"></asp:button></p>
<p>
    <asp:label id="lb" runat="server"></asp:label>
</p>
</fieldset>
