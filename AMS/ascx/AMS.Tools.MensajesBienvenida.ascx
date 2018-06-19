<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.MensajesBienvenida.ascx.cs" Inherits="AMS.Tools.AMS_Tools_MensajesBienvenida" %>

<!-- Script y links -->
<link rel="stylesheet" href="//code.jquery.com/ui/1.12.0/themes/base/jquery-ui.css" />
<link rel="stylesheet" href="../css/AMS.nvoTabs.css" />
<link rel="stylesheet" href="../css/AMS.css" />
<link rel="stylesheet" href="../css/AMS.TabDemo.css" />
<link rel="stylesheet" href="../css/AMS.TabNormalize.css" />

<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
<script src="https://code.jquery.com/ui/1.12.0/jquery-ui.js"></script>

<script language="javascript">

    $(function() {
        $("#tabs" ).tabs().addClass( "ui-tabs-vertical ui-helper-clearfix" );
        $("#tabs li").removeClass("ui-corner-top").addClass("ui-corner-left");
    });
</script>
<script type="text/javascript">
    function probando(obj)
    {
        //alert(obj.outerText);
        if (confirm("Está seguro de borrar la fila?")) {
            AMS_Tools_MensajesBienvenida.elimina_Fila(obj.outerText, eliminar_Fila_Callback);
            $(event.target).closest("tr").toggle('slow');
        }
        else {
            return;
        }
        //var grid = document.getElementById("<%= miGrid.ClientID %>");
        //var theDropdown = grid.rows[0].cells[0];
        //var selIndex = theDropdown.selectedIndex;
        //cellPivot = theDropdown.options[selIndex].value;
        //alert();
        //alert(theDropdown.value);
        
    }
    function eliminar_Fila_Callback(response)
    {
        $('#' + '<%=lbResult.ClientID%>').empty();
        if (response.value === "ok") {
            $('#' + '<%=lbResult.ClientID%>').text("Se ha eliminado la fila y esta ya no será visible.");
        }
        else
        {
            alert('Se generó un problema al tratar de eliminar la fila, por favor recargue los datos e inténtelo de nuevo!.');
            $('#' + '<%=lbResult.ClientID%>').text("Recargue los datos por favor!");
        }
            
    }
</script>

<style type="text/css">
    .ui-tabs-vertical { width: 55em; }
    .ui-tabs-vertical .ui-tabs-nav { padding: .2em .1em .2em .2em; float: left; width: 12em; }
    .ui-tabs-vertical .ui-tabs-nav li { clear: left; width: 100%; border-bottom-width: 1px !important; border-right-width: 0 !important; margin: 0 -1px .2em 0; }
    .ui-tabs-vertical .ui-tabs-nav li a { display:block; }
    .ui-tabs-vertical .ui-tabs-nav li.ui-tabs-active { padding-bottom: 0; padding-right: .1em; border-right-width: 1px; }
    .ui-tabs-vertical .ui-tabs-panel { padding: 1em; float: right; width: 50em;}
</style>

<!-- body -->
<fieldset class="fldMensajes">
    <div id="tabs" style="background: rgba(198, 198, 228, 0.9);">
      <ul>
        <li><a href="#tabs-1">Escribir nuevo Mensaje</a></li>
        <li><a href="#tabs-2">Editar mis Mensajes</a></li>
      </ul>
      <div id="tabs-1" style="background: rgba(198, 198, 228, 0.6);">
        <h2>Nuevo Mensaje</h2>
        <p>
            <asp:Table ID="tblMsn" runat="server" >
                <asp:TableRow>
                    <asp:TableCell>
                            Autor: &nbsp;&nbsp; <asp:TextBox ID="txtAutor" runat="server" Width="450px" placeholder="Anonimo si deja en blanco este campo." style="font-family: fantasy, -webkit-body;"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <br />Mensaje<br />
                        <asp:TextBox ID="txtAreaMensaje" runat="server" TextMode="MultiLine" ToolTip="Cualquier mensaje, frase célebre o acontecimiento puede escribirlo." Rows="6" Columns="15" placeholder="Algún acontecimiento importante?" style="font-family: cursive;"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button id="btnGuardar" runat="server" OnClick="grabarDatos" Text="Grabar Mensaje"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </p>
      </div>
      <div id="tabs-2" style="background:rgba(198, 198, 228, 0.3);">
        <h2>Eliminar Mis Mensajes</h2>
        <p>
			<ASP:DataGrid id="miGrid" runat="server" cssclass="gridMsn" BorderWidth="2px" BorderStyle="Ridge" AutoGenerateColumns="false"
				CellPadding="3" CellSpacing="1">
				<FooterStyle cssclass="footer"></FooterStyle>
				<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
				<PagerStyle horizontalalign="Right" cssclass="pager"></PagerStyle>
				<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
				<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
				<ItemStyle cssclass="item"></ItemStyle>
                <Columns >
                    <asp:TemplateColumn HeaderText="ID">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "ID") %>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="AUTOR">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "AUTOR") %>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="MENSAJE">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "MENSAJE") %>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="MOSTRAR_MENSAJE">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "MOSTRAR_MENSAJE") %>
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
                    <asp:TemplateColumn HeaderText="Borrar">
						<ItemTemplate>
                            <div onclick="probando(this)">
                                <asp:Label runat="server" style="color:lightcyan"><%# DataBinder.Eval(Container.DataItem, "ID") %></asp:Label> 
                                <img src="../img/deleteGrid.png" alt="Borrar Fila" style="z-index:10;"/>
                            </div>
						</ItemTemplate>
					</asp:TemplateColumn>
                </Columns>
			</ASP:DataGrid><br /><br /> 
            <asp:Button id="btnRecarga" runat="server" OnClick="recargaDatos" Text="Recargar Datos"/> <br />
            <asp:Label ID="lbResult" runat="server"></asp:Label>
        </p>
      </div>
    </div>
</fieldset><br />
