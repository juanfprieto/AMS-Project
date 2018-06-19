
<%@ Control Language="c#" Debug="true" AutoEventWireup="True" CodeBehind="AMS.DBManager.Selects.ascx.cs"    Inherits="AMS.DBManager.Selects" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="Email" Src="AMS.Tools.Email.ascx" %>

<style type="text/css">
    table.paginador
    {
        border-width: 0px 0px 0px 0px;
        background-color: white;
    }
</style>
<script type="text/javascript">
    function Lista() {
        w = window.open('AMS.DBManager.Reporte.aspx');
        // <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0"></A>
    }

    function abrirDocumento(valor, primaria, campo, tabla)
    {
        Selects.Abrir_Documento(valor, primaria, campo, tabla, abrir_Documento_CallBack);
    }

    function abrir_Documento_CallBack(response) {
        var respuesta = response.value;

        if (respuesta != "" && respuesta != undefined) {
            $("#divDocument").html(respuesta);
            $("#divContDocument").css("visibility", "visible");
        }
        else {
            $("#divDocument").html("");
            $("#divContDocument").css("visibility", "hidden");
        }
    }

    function cerrarDocumento() {
      
        $("#divContDocument").css("visibility", "hidden");
    }
</script>

    <fieldset id="actionbar">
        <table class="filtersInAuto" align="center">
            <tbody>
                <tr>
                    <td>
                        <asp:Button ID="Button1" OnClick="DgTable_Insert" Text="Insertar" runat="server">
                        </asp:Button>
                    </td>
                    <td>
                        <asp:TextBox ID="tbWord" placeholder="% Buscar %" class="tpequeno form-control" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <b class="list-inline" style="display:inline-block">En:</b> 
                        <asp:DropDownList ID="ddlCols" class="dpequeno form-control" runat="server" style="display:inline-block">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btSearch" OnClick="Search" Text="Buscar" runat="server"></asp:Button>
                    </td>
                    <td>
                        <asp:Button ID="btClasifica" OnClick="Clasifica" Text="Clasificar" runat="server">
                        </asp:Button>
                    </td>
                </tr>
            </tbody>
        </table>
    </fieldset>
    <br />
    <table>
        <tr>
            <td>
                <asp:PlaceHolder ID="toolsHolder" runat="server" Visible="false">
                    <table style="margin: 0px; width: 529px;">
                        <tr>
                            <td><uc1:Email runat="server" id="opcEnviarMail"></uc1:Email></td>
                            <td style="padding-left: 19px;text-align: center;">
                                <asp:ImageButton ToolTip="Imprimir" ID="BtnImprimirExcel" OnClick="ImprimirExcelGrid" runat="server"
                                    alt="Imprimir Excel" ImageUrl="../img/AMS.Icon.xls_icon.png" BorderWidth="0px" cssClass="Excelimg" style="width: 50%; margin-bottom: -6px;">
                                </asp:ImageButton>
                                <br />
                                <font size="1">Descargar Excel</font>
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbInfo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbError" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <div id="divItems" overflow: auto; height: 500px; width: 90%; margin: auto;">
        <asp:DataGrid ID="dgTable" runat="server"  BorderWidth="1px" OnItemCreated="validar"
            HeaderStyle-BackColor="#000" Font-Size="8pt" Font-Name="Verdana" CellPadding="0"
            ShowFooter="false" BorderColor="#999999" BackColor="White" 
            GridLines="Vertical" Font-Names="Verdana" OnItemCommand="dgTable_Procesos" OnItemDataBound="DgDataBound">
            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
            <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
            <ItemStyle HorizontalAlign="Center"  BorderColor="lightgray" ForeColor="Black" BackColor="#B8F1FB"></ItemStyle>
            <HeaderStyle CssClass="gridHeader" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:ImageButton align="align" ID="btEdit" OnClientClick="espera();" ImageUrl="../img/Edit.png"
                            AlternateText="Editar Registro" runat="server" ToolTip="Editar" CommandName="Edit"
                            Height="18px"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:ImageButton ID="btDelete"  ImageUrl="../img/Delete.png"
                            AlternateText="Borrar Registro" runat="server" ToolTip="Borrar Registro" CommandName="Borrar"
                            Height="18px"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:ImageButton ID="btPrint" OnClientClick="espera();" ImageUrl="../img/Imprimir.png"
                            AlternateText="Imprimir Registro" runat="server" ToolTip="Imprimir Registro"
                            CommandName="Imprimir" Height="18px"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:ImageButton ID="btCopiar" OnClientClick="espera();" runat="server" CommandName="Copiar"
                            Height="18px" ImageUrl="../img/Copiar.png" ToolTip="Copiar Registro" AlternateText="Copiar Registro">
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
 <p style="margin-top:1em;">
        <asp:Label ID="lblResult" class="bg-success" runat="server"></asp:Label></p>
    <div align="center">
        <table border="0px" class="paginador" cellpadding="0" cellspacing="0">
            <tr>
                <td align="right" valign="middle">
                    <div>
                        <asp:PlaceHolder ID="plcPaginacionIS" runat="server" ></asp:PlaceHolder>
                
                        <asp:Label ID="lblPaginaActual" runat="server"></asp:Label>
              
                        <asp:PlaceHolder ID="plcPaginacionDS" runat="server"></asp:PlaceHolder>
               
                        <asp:PlaceHolder ID="plcPaginacionII" runat="server" Visible="false"></asp:PlaceHolder>
                    </div>
                </td>
                <td  align="left" valign="middle" >
                    <asp:PlaceHolder ID="plcPaginacionDI" runat="server" Visible="false"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </div>
    <br>
    <div id="scroll1" style="overflow: auto; width: 632px; line-height: 0px; height: 20px"
        onscroll="OnScroll(this);">
        <div id="spacing1" style="visibility: hidden">
            &nbsp;</div>
    </div>   
    <p>
        <asp:TextBox ID="txtSort" runat="server" Visible="False"></asp:TextBox><asp:TextBox
            ID="txtFilt" runat="server" Visible="False"></asp:TextBox><asp:TextBox ID="likestr"
                runat="server" Visible="False"></asp:TextBox><asp:Table ID="Table1" runat="server">
                </asp:Table>
    </p>
    <p>
        <asp:DataGrid ID="dgAux" runat="server" BorderWidth="1px" Font-Names="Verdana" GridLines="Vertical"
            BorderStyle="None" BackColor="White" BorderColor="#999999" ShowFooter="True"
            CellPadding="0" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-BackColor="#ccccdd">
            <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
            <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
            <AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
            <ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
        </asp:DataGrid></p>

  <div id="divContDocument" style="
        width: 698px;
    height: 680px;
    position: absolute;
    top: 90px;
    left:20%;
    overflow: hidden; visibility: hidden">

        <div id="divDocument" 
            style="position: absolute;
        background-color: white;
        width: 670px;
        height: 650px;
        left: 1px;
        top: 1px;
        padding: 70px;
        overflow-y: scroll;
        box-shadow: 4px 10px 22px #888888;
        border-radius: 4px;
        border-style: solid;"></div>
        
       <asp:Button id="btnCerrarDocument" runat="server" Text="Cerrar"  
            style="padding: 1px;
        position: absolute;
        top: 14px;
        right: 43px;" OnClientClick="cerrarDocumento(); return false;" CssClass="noEspera"></asp:Button>
     </div>