<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Gerencial.ConsultaActualizacion.ascx.cs" Inherits="AMS.Gerencial.AMS_Gerencial_ConsultaActualizacion" %>

<script type="text/javascript">

    $(function () {
        var fechaVal = $("#<%=txtFechaDesde.ClientID%>").val();
        $("#<%=txtFechaDesde.ClientID%>").datepicker();
        $("#<%=txtFechaDesde.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=txtFechaDesde.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFechaDesde.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFechaDesde.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=txtFechaDesde.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFechaDesde.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFechaDesde.ClientID%>").val(fechaVal);
    });

    $(function () {
        var fechaVal = $("#<%=txtFechaHasta.ClientID%>").val();
        $("#<%=txtFechaHasta.ClientID%>").datepicker();
        $("#<%=txtFechaHasta.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=txtFechaHasta.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFechaHasta.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFechaHasta.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=txtFechaHasta.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFechaHasta.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFechaHasta.ClientID%>").val(fechaVal);
    });
</script>

<fieldset>
   <asp:Table runat="server">
       <asp:TableRow>
           <asp:TableCell>
               Fecha desde: &nbsp; <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="tpequeno"></asp:TextBox><br />
           </asp:TableCell>
       </asp:TableRow>
       <asp:TableRow>
           <asp:TableCell>
               Fecha Hasta: &nbsp; <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="tpequeno"></asp:TextBox><br />
           </asp:TableCell>
       </asp:TableRow>
       <asp:TableRow>
           <asp:TableCell>
               <asp:Button id="btnBuscar" runat="server" Text="Filtrar Búsqueda" OnClick="buscar"/><br /><br />
           </asp:TableCell>
       </asp:TableRow>
   </asp:Table>
    <ASP:DataGrid id="miGrid" runat="server" cssclass="gridMsn" BorderWidth="2px" AllowPaging="true" PageSize="50" OnPageIndexChanged="dataGrid_Paging" BorderStyle="Ridge" AutoGenerateColumns="false"
				CellPadding="3" CellSpacing="1">
				<FooterStyle cssclass="footer"></FooterStyle>
				<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
				<PagerStyle horizontalalign="Center"  NextPageText="Siguiente.." Mode="NumericPages" ></PagerStyle>
				<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
				<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
				<ItemStyle cssclass="item"></ItemStyle>
                <Columns >
                    <asp:TemplateColumn HeaderText="ID">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "ID") %>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="RUTA">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "RUTA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="RAZON">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "RAZON") %>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="FECHA">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
						</ItemTemplate>
					</asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="USUARIO">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "USUARIO") %>
						</ItemTemplate>
					</asp:TemplateColumn>
                </Columns>
			</ASP:DataGrid><br /><br />
</fieldset>
