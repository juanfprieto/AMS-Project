<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.DescargaFTP.ascx.cs" Inherits="AMS.Tools.AMS_Tools_DescargaFTP" %>

 <fieldset>
    <legend>Servidor FTP: Archivos disponibles para Descarga</legend>
     <br>
    <asp:DataGrid id="dgFilesFTP" runat="server" HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" 
    Font-Name="Verdana" CellPadding="3" BorderColor="#999999" BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" 
    Font-Names="Verdana" AutoGenerateColumns="false" style="    width: 60%;margin: 1px;"
    OnItemCommand="DescargarArchivo">
        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
        <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
        <AlternatingItemStyle backcolor="#DCDCDC"></AlternatingItemStyle>
        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Fecha">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "FECHA") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Hora">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "HORA") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Nombre del Archivo">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "NOMBRE") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Tamaño">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "TAMANO") %> 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descargar" >
                <ItemStyle Width="22px" />
                <ItemTemplate>
                    <asp:ImageButton id="ibtnDescarga" CommandName="Descargar" runat="server" ImageUrl="../img/downloadIcon.png" style="width: 35%;" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Eliminar" >
                <ItemStyle Width="22px" />
                <ItemTemplate>
                    <asp:ImageButton id="ibtnEliminar" CommandName="Eliminar" runat="server" ImageUrl="../img/Delete.png" style="width: 35%;" OnClientClick="return confirm('Recuerde descargar y verificar este archivo antes de eliminarlo! \n¿Esta seguro que desea eliminar el archivo?');" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</fieldset>
