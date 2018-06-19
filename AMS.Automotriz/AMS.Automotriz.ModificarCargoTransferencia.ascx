<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.ModificarCargoTransferencia.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_ModificarCargoTransferencia" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>

<asp:PlaceHolder id="plcorden" runat="server">
    <fieldset>
        <legend>Modificar el Cargo de la Transferencia</legend>
        <asp:Label>Prefijo de la Orden</asp:Label><br />
        <asp:DropDownList id="ddlprefijoorden" class="dmediano" runat="server" OnSelectedIndexChanged="CargarNumeroOrden" AutoPostBack="true"></asp:DropDownList><br />
        <asp:Label>Numero de la Orden</asp:Label><br />
        <asp:DropDownList id="ddlnumeroorden" class="dmediano" runat="server"></asp:DropDownList><br />
        <asp:Button id="btncargartransferencia" runat="server" onclick="RevisarTransferencia" Text="Cargar >>"/>
    </fieldset>
</asp:PlaceHolder>
<asp:PlaceHolder id="plctransferencia" runat="server">
    <fieldset>
        <legend>Modificar el Cargo de la Transferencia</legend>
        <asp:Label>PREFIJO ORDEN</asp:Label>
        <asp:Label id="lborden"></asp:Label>
        <asp:Label>NUMERO ORDEN</asp:Label>
        <asp:Label id="lbnumorden"></asp:Label>
        <asp:DataGrid id="dgtransferencia" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False" CellPadding="3" 
        OnEditCommand="DgInserts_Edit" OnItemDataBound="DgInserts_Databound" OnItemCommand="DgInserts_Item">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
        <Columns>
        <asp:TemplateColumn HeaderText="Codigo Orden">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CODIGO ORDEN")%>
                </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Numero Orden">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "NUMERO ORDEN")%>
                </ItemTemplate>
        </asp:TemplateColumn>
         <asp:TemplateColumn HeaderText="Cargo Transferencia">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CARGO_TRANSFERENCIA")%>
                </ItemTemplate>
                <FooterTemplate>
                 <asp:DropDownList ID="ddlCargoTran" runat="server" AutoPostBack="false"></asp:DropDownList>
                </FooterTemplate>
                <EditItemTemplate>
                <asp:DropDownList ID="ddlCargoTran" runat="server"></asp:DropDownList>
                </EditItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Codigo Transferencia">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CODIGO TRANSFERENCIA")%>
                </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Numero Transferencia">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "NUMERO TRANSFERENCIA")%>
                </ItemTemplate>
        </asp:TemplateColumn>
        <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
        </Columns>
        
	</asp:DataGrid>
    <asp:Button id="btngravar" runat="server" onclick="GuardarNuevoCargo" Text="Grabar"/>
    <asp:Label id="lberror" runat="server"></asp:Label>
    </fieldset>
</asp:PlaceHolder>