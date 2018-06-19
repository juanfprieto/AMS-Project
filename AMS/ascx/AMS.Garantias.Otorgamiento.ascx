<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Garantias.Otorgamiento.ascx.cs" Inherits="AMS.Garantias.AMS_Garantias_Otorgamiento" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript">
function MostrarRefs(obTex,obCmbLin)
{
    ModalDialog(obTex,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER By mite_codigo', new Array());
}
</script>
<P>
<P>OTORGAMIENTO DE GARANTIA</P>
<P>
<P>Grupo Catalogo :
	<asp:label id="lbGrupo" runat="server"></asp:label>&nbsp;
	<asp:label id="lbCodcat" runat="server"></asp:label>&nbsp;
	<asp:label id="lbNoncat" runat="server"></asp:label></P>
<P>
<P></P>
<P></P>
<P></P>
<P></P>
Items
<P></P>
<P><asp:datagrid id="dgItems" runat="server" ShowFooter="True" AutoGenerateColumns="False" onselectedindexchanged="dgItems_SelectedIndexChanged">
		<Columns>
			<asp:TemplateColumn HeaderText="C&#243;digo del Item">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "Codigo") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodItem" runat="server" Width="162px"></asp:TextBox>
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
					<asp:TextBox id="tbCantItem" runat="server" Text='	<%# DataBinder.Eval(Container.DataItem, "cantidad") %>' Width="42px">
					</asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Linea">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "linea") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:DropDownList id="ddlLineas" runat="server"></asp:DropDownList>
				</FooterTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Valor Unitario">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ValorIt") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cubre S/N">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "cubresino") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Seleccionado">
				<ItemTemplate>
					<asp:Button id="btRemoverItem" runat="server"  class="bpequeno" Text="Remover" CommandName="BorrarItem"></asp:Button>
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button id="btAgregarItem" runat="server" class="bpequeno" Text="Agregar" CommandName="AgregarItem"></asp:Button>
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid></P>
<P>Operaciones</P>
<P><asp:datagrid id="dgOpers" runat="server" ShowFooter="True" AutoGenerateColumns="False">
		<Columns>
			<asp:TemplateColumn HeaderText="C&#243;digo de operaci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ptem_operacion") %>
				</ItemTemplate>
				<FooterTemplate>
					<asp:TextBox id="tbCodOper" onclick="ModalDialog(this,'Select  ptem_operacion AS Codigo,ptem_descripcion Operacion  from  ptempario', new Array(),1)"
						runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
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
			<asp:TemplateColumn HeaderText="Valor Operaci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem,"Valor","{0:c}")%>
					<asp:TextBox id=tbValoper runat="server" class="tpequeno" CssClass="AlineacionDerecha" text='<%# DataBinder.Eval(Container.DataItem,"Valor","{0:n}")%>' Visible="False">
					</asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Exento IVA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "exeivaOper") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="%IVA">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ivaOper") %>
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
<P>
	<TABLE id="Table1" class="filtersIn">
		<TR>
			<TD>
				<P><asp:label id="e1" runat="server" ForeColor="DarkOrange">*</asp:label></P>
			</TD>
			<TD><asp:label id="lbParcial" runat="server" Font-Size="X-Small" Width="536px"></asp:label></TD>
		</TR>
		<TR>
			<TD>
				<P><asp:label id="e2" runat="server" ForeColor="DarkOrange">*</asp:label></P>
			</TD>
			<TD><asp:label id="lbParcial2" runat="server" Font-Size="X-Small" Width="536px"></asp:label></TD>
		</TR>
	</TABLE>
</P>
<P><asp:button id="btGuardar" runat="server" Text="Enviar solicitud" onclick="btGuardar_Click"></asp:button></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
<P>&nbsp;</P>
