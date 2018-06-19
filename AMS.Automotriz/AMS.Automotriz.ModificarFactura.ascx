<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.ModificarFactura.ascx.cs" Inherits="AMS.Automotriz.ModificarFactura" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>

<style>
table
{
    table-layout:fixed;
    width: 94%;
}
</style>
<center><asp:Label ID="lblFactura" runat="server" Font-Size=Large></asp:Label></center>
<asp:placeholder id="plcBuscarFactura" runat="server">
    <FIELDSET>
        <LEGEND>Ajustes de Factura</LEGEND>
        <br />
        Revisar factura:  <asp:DropDownList id=ddlPrefijoFactura class="dmediano" runat="server" OnSelectedIndexChanged="CargarNumeroFacturas" AutoPostBack="true"></asp:DropDownList>
                          <asp:DropDownList id=ddlNumeroFactura class="dpequeno" runat="server"></asp:DropDownList>
                          <asp:button id="btnRevisarFac" onclick="RevisarFactura" runat="server" Text="Cargar >>" Width="108px"></asp:button>
    </FIELDSET>
</asp:placeholder>

<asp:placeholder id="plcTablasDatosFactura" runat="server" Visible="false">
    <FIELDSET>
        <LEGEND>Operaciones</LEGEND>
        <ASP:DATAGRID id="dgObservaciones" runat="server" enableViewState="true" 
            OnItemDataBound="DgInsertsDataBound" OnEditCommand="DgInserts_Edit" OnUpdateCommand="DgInserts_Update" OnCancelCommand="DgInserts_Cancel"
	        HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3" ShowFooter="false" BorderColor="#999999"
	        BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana"
	        AutoGenerateColumns="false"  style="table-layout=auto;">
	        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
	        <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
	        <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
	        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
	        <Columns>
		        <asp:TemplateColumn HeaderText="CODIGO">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="DESCRIPCION" >
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CANTIDAD" >
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="VALOR APROXIMADO">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "VAL_APROX", "{0:N2}") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="VALOR REAL">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "VAL_REAL", "{0:N3}") %>
			        </ItemTemplate>
			        <EditItemTemplate>
				        <asp:TextBox runat="server" id="edit_1"  CssClass="AlineacionDerecha" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "VAL_REAL","{0:N3}") %>' />
			        </EditItemTemplate>
		        </asp:TemplateColumn>
		        <asp:EditCommandColumn ButtonType="PushButton" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar"></asp:EditCommandColumn>
	        </Columns>
        </ASP:DATAGRID>

        <table>
            <tr>
                <td></td>
                <td></td>
                <td align="right"><b>Total Aproximado(Esperado):</b></td>
                <td><asp:TextBox id=txtTotalRound class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><asp:TextBox id=txtTotalReal class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><b>:Total Real</b></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td><asp:TextBox id=txtTotalRealRound class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><b>:Total Real Aproximado(Reporte)</b></td>
            </tr>
        </table>
    </FIELDSET>
    <br />
    <FIELDSET>
        <LEGEND>Repuestos y Repuestos de Bodega</LEGEND>
        <ASP:DATAGRID id="dgRepuestos" runat="server" enableViewState="true" 
            OnItemDataBound="DgInsertsDataBoundRep" OnEditCommand="DgInserts_EditRep" OnUpdateCommand="DgInserts_UpdateRep" OnCancelCommand="DgInserts_CancelRep"
	        HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3" ShowFooter="false" BorderColor="#999999"
	        BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana"
	        AutoGenerateColumns="false"  style="table-layout=auto;">
	        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
	        <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
	        <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
	        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
	        <Columns>
		        <asp:TemplateColumn HeaderText="CODIGO">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="DESCRIPCION" >
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CANTIDAD" >
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="VALOR APROXIMADO">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "VAL_APROX", "{0:N2}") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="VALOR REAL">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "VAL_REAL", "{0:N4}") %>
			        </ItemTemplate>
			        <EditItemTemplate>
				        <asp:TextBox runat="server" id="edit_1"  CssClass="AlineacionDerecha" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "VAL_REAL","{0:N4}") %>' />
			        </EditItemTemplate>
		        </asp:TemplateColumn>
		        <asp:EditCommandColumn ButtonType="PushButton" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar"></asp:EditCommandColumn>
	        </Columns>
        </ASP:DATAGRID>
        <ASP:DATAGRID id="dgBodegaRep" runat="server" enableViewState="true" 
            OnItemDataBound="DgInsertsDataBoundBod" OnEditCommand="DgInserts_EditBod" OnUpdateCommand="DgInserts_UpdateBod" OnCancelCommand="DgInserts_CancelBod"
	        HeaderStyle-BackColor="#ccccdd" Font-Size="8pt" Font-Name="Verdana" CellPadding="3" ShowFooter="false" BorderColor="#999999"
	        BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana"
	        AutoGenerateColumns="false"  style="table-layout=auto;">
	        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
	        <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
	        <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
	        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
	        <Columns>
		        <asp:TemplateColumn HeaderText="CODIGO">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="DESCRIPCION" >
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CANTIDAD" >
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "CANTIDAD") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="VALOR APROXIMADO">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "VAL_APROX", "{0:N2}") %>
			        </ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="VALOR REAL">
			        <ItemTemplate>
				        <%# DataBinder.Eval(Container.DataItem, "VAL_REAL", "{0:N3}") %>
			        </ItemTemplate>
			        <EditItemTemplate>
				        <asp:TextBox runat="server" id="edit_1"  CssClass="AlineacionDerecha" class="tpequeno" onkeyup="NumericMaskE(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "VAL_REAL","{0:N3}") %>' />
			        </EditItemTemplate>
		        </asp:TemplateColumn>
		        <asp:EditCommandColumn ButtonType="PushButton" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar"></asp:EditCommandColumn>
	        </Columns>
        </ASP:DATAGRID>
        <table>
            <tr>
                <td></td>
                <td></td>
                <td align="right"><b>Total Aproximado(Esperado):</b></td>
                <td><asp:TextBox id=txtTotalRoundRepBodRe class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><asp:TextBox id=txtTotalRealRepBodRe class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><b>:Total Real</b></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td><asp:TextBox id=txtTotalRealRoundRepBodRe class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><b>:Total Real Aproximado(Reporte)</b></td>
            </tr>
        </table>
    </FIELDSET>
    <br />
    <FIELDSET>
        <LEGEND>Totales</LEGEND>
        <table>
            <tr>
                <td></td>
                <td></td>
                <td align="right"><b>SubTotal DB*:</b></td>
                <td><asp:Label ID="lblsubTotalDB" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblIVADB" runat="server"></asp:Label></td>
                <td><b>:IVA DB*</b></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td align="right"><b>SubTotal:</b></td>
                <td><asp:TextBox id=txtSubTotal class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><asp:TextBox id=txtIVA class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><b>:IVA</b></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td><asp:button id="btnAjustarFactura" onclick="AjustarFactura" runat="server" Text="AJUSTAR" Width="140px" Height="42px"></asp:button></td>
                <td><asp:TextBox id=txtTotal class="tpequeno" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td><b>:TOTAL</b></td>
            </tr>
        </table>
    </FIELDSET>
</asp:placeholder>
