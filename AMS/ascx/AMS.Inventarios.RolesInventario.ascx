<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="AMS.Inventarios.RolesInventario.ascx.cs"
    Inherits="AMS.Inventarios.AMS_Inventarios_RolesInventario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link href="../css/tab.webfx.css" type="text/css" rel="StyleSheet">
<script language="javascript" src="../js/tabpane.js" type="text/javascript"></script>
<fieldset>
    <legend>Inventario Físico</legend>
    <table id="Table1" class="filtersIn">
        <tr>
            <td>
                Prefijo :
              <br />
                <asp:DropDownList ID="ddlprefijo" AutoPostBack="true" class="dmediano" runat="server" OnSelectedIndexChanged="ddlprefijo_OnSelectedIndexChanged">
                </asp:DropDownList>
          <br />
                Número de Inventario :
          <br />
                <asp:DropDownList ID="ddlNumero" class="dpequeno" runat="server">
                </asp:DropDownList>
          
                <asp:Button ID="aceptar" runat="server" Text="Agregar Roles" OnClick="aceptar_Click">
                </asp:Button>
                <input id="hdPrefSelec" type="hidden" name="hdPrefSelec" runat="server">
                <input id="hdNumSelec" type="hidden" name="hdNumSelec" runat="server">
            </td>
        
        </tr>
       
    </table>
</fieldset>

<asp:Panel ID="Panel2" runat="server" Visible="False">
    <fieldset>
        <legend>Roles de Inventario Físico</legend>
        <div class="tab-pane" id="tab-pane-1">
            <div class="tab-page" align="center">
                <h2 class="tab">
                    Gerente de Inventario</h2>
                <table class="tablewhite" cellspacing="3" cellpadding="3" width="100%" border="0">
                    <tr>
                        <td width="35%">
                            Nombre del Gerente de Inventario :
                        </td>
                        <td align="right">
                            <input id="TextBox1a" type="hidden" runat="server">
                            <asp:TextBox ID="TextBox1" onclick="ModalDialog(this,'select MNI.mnit_nombres concat \' \' concat COALESCE(MNI.mnit_nombre2,\' \') concat \' \' concat MNI.mnit_apellidos concat \' \' concat COALESCE(MNI.mnit_apellido2,\' \') as Nombre,MNI.mnit_nit as Nit from mnit MNI inner join mempleado MEM on MNI.mnit_nit = MEM.mnit_nit order by MNI.mnit_apellidos', new Array(),1)"
                                runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Codigo del Rol&nbsp;Gerente :
                        </td>
                        <td align="right">
                            <asp:DropDownList ID="dllRolGer" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tab-page" align="center">
                <h2 class="tab">
                    Auditor de Inventario</h2>
                <table class="tablewhite" cellspacing="3" cellpadding="3" width="100%" border="0">
                    <tr>
                        <td>
                            Nombre del Auditor de Inventario :
                        </td>
                        <td align="right">
                            <input id="TextBox2a" type="hidden" runat="server">
                            <asp:TextBox ID="TextBox2" onclick="ModalDialog(this,'select MNI.mnit_nombres concat \' \' concat COALESCE(MNI.mnit_nombre2,\' \') concat \' \' concat MNI.mnit_apellidos concat \' \' concat COALESCE(MNI.mnit_apellido2,\' \') as Nombre,MNI.mnit_nit as Nit from mnit MNI inner join mempleado MEM on MNI.mnit_nit = MEM.mnit_nit order by MNI.mnit_apellidos', new Array(),1)"
                                runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Codigo del&nbsp;Rol&nbsp;Auditor :
                        </td>
                        <td align="right">
                            <asp:DropDownList ID="dllRolAudi" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tab-page" align="center">
                <h2 class="tab">
                    Digitadores</h2>
                <table class="tablewhite" cellspacing="3" cellpadding="3" width="100%" border="0">
                    <tr>
                        <td>
                            Número de Digitadores :
                        </td>
                        <td align="right">
                            <asp:TextBox ID="numdig" class="tpequeno" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Button ID="cargard" OnClick="Cargar_Tabla_Rtns" runat="server" Text="Cargar Digitadores">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Número de digitadores Escogidos :
                        </td>
                        <td align="right" colspan="2">
                            <asp:Label ID="dig" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Codigo de Rol de Digitador :
                        </td>
                        <td align="right" colspan="2">
                            <asp:DropDownList ID="ddlDigRol" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:DataGrid ID="DataGrid1" runat="server" cssclass="datagrid"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Nombre Digitador">
                                        <ItemTemplate>
                                            <asp:TextBox ID="Nombre" onclick="ModalDialog(this,'select MNI.mnit_nombres concat \' \' concat COALESCE(MNI.mnit_nombre2,\' \') concat \' \' concat MNI.mnit_apellidos concat \' \' concat COALESCE(MNI.mnit_apellido2,\' \') as Nombre,MNI.mnit_nit as Nit from mnit MNI order by MNI.mnit_apellidos', new Array(),1)"
                                                runat="server" Width="360px" Text='<%#DataBinder.Eval(Container.DataItem,"Nombre")%>'
                                                ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Mnit">
                                        <ItemTemplate>
                                            <asp:TextBox ID="Nombrea" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Nit")%>'
                                                ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tab-page" align="center">
                <h2 class="tab">
                    Patinadores</h2>
                <table class="tablewhite" cellspacing="3" cellpadding="3" width="100%" border="0">
                    <tr>
                        <td width="30%">
                            Número Patinadores :
                        </td>
                        <td align="right">
                            <asp:TextBox ID="numpat" Width="56px" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
                        </td>
                        <td align="right">
                            <p>
                                <asp:Button ID="cargarp" OnClick="Cargar_Tabla_Rtns" runat="server" Text="Cargar Patinadores">
                                </asp:Button></p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Número de Patinadores Escogidos :
                        </td>
                        <td align="right" colspan="2">
                            <asp:Label ID="pat" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Codigo de Rol Patinador :
                        </td>
                        <td align="right" colspan="2">
                            <asp:DropDownList ID="dllRolPat" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:DataGrid ID="DataGrid2" Width="624px" runat="server" BackColor="DarkGray" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Nombre Patinador">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbNombre2" onclick="ModalDialog(this,'select MNI.mnit_nombres concat \' \' concat COALESCE(MNI.mnit_nombre2,\' \') concat \' \' concat MNI.mnit_apellidos concat \' \' concat COALESCE(MNI.mnit_apellido2,\' \') as Nombre,MNI.mnit_nit as Nit from mnit MNI order by MNI.mnit_apellidos', new Array(),1)"
                                                runat="server" Width="360px" Text='<%#DataBinder.Eval(Container.DataItem,"Nombre")%>'
                                                ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Mnit">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbNombre2a" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Nit")%>'
                                                ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Grupo">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="grupo" runat="server" Width="72px">
                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                <asp:ListItem Value="6">6</asp:ListItem>
                                                <asp:ListItem Value="7">7</asp:ListItem>
                                                <asp:ListItem Value="8">8</asp:ListItem>
                                                <asp:ListItem Value="9">9</asp:ListItem>
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="11">11</asp:ListItem>
                                                <asp:ListItem Value="12">12</asp:ListItem>
                                                <asp:ListItem Value="13">13</asp:ListItem>
                                                <asp:ListItem Value="14">14</asp:ListItem>
                                                <asp:ListItem Value="15">15</asp:ListItem>
                                                <asp:ListItem Value="16">16</asp:ListItem>
                                                <asp:ListItem Value="17">17</asp:ListItem>
                                                <asp:ListItem Value="18">18</asp:ListItem>
                                                <asp:ListItem Value="19">19</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="21">21</asp:ListItem>
                                                <asp:ListItem Value="22">22</asp:ListItem>
                                                <asp:ListItem Value="23">23</asp:ListItem>
                                                <asp:ListItem Value="24">24</asp:ListItem>
                                                <asp:ListItem Value="25">25</asp:ListItem>
                                                <asp:ListItem Value="26">26</asp:ListItem>
                                                <asp:ListItem Value="27">27</asp:ListItem>
                                                <asp:ListItem Value="28">28</asp:ListItem>
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                            </TD>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tab-page" align="center">
                <h2 class="tab">
                    Coordinadores de Zona</h2>
                <table class="tablewhite" cellspacing="3" cellpadding="3" width="100%" border="0">
                    <tr>
                        <td width="30%">
                            Número Coordinadores&nbsp;:
                        </td>
                        <td align="right">
                            <asp:TextBox ID="numcor" Width="56px" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
                        </td>
                        <td align="right">
                            <p>
                                <asp:Button ID="cargarcor" OnClick="Cargar_Tabla_Rtns" runat="server" Text="Cargar Coordinadores">
                                </asp:Button></p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Número de Coordinadores Escogidos :
                        </td>
                        <td align="right" colspan="2">
                            <asp:Label ID="cor" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Codigo de Rol Coordinador :
                        </td>
                        <td align="right" colspan="2">
                            <asp:DropDownList ID="dllRolCoor" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:DataGrid ID="DataGrid4" Width="624px" runat="server" BackColor="DarkGray" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Nombre Coordinador">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbNombre3" onclick="ModalDialog(this,'select MNI.mnit_nombres concat \' \' concat COALESCE(MNI.mnit_nombre2,\' \') concat \' \' concat MNI.mnit_apellidos concat \' \' concat COALESCE(MNI.mnit_apellido2,\' \') as Nombre,MNI.mnit_nit as Nit from mnit MNI inner join mempleado MEM on MNI.mnit_nit = MEM.mnit_nit order by MNI.mnit_apellidos', new Array(),1)"
                                                runat="server" Width="360px" Text='<%#DataBinder.Eval(Container.DataItem,"Nombre")%>'
                                                ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Mnit">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbNombre3a" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Nit")%>'
                                                ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Grupo">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="grupo2" runat="server" Width="72px">
                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                <asp:ListItem Value="6">6</asp:ListItem>
                                                <asp:ListItem Value="7">7</asp:ListItem>
                                                <asp:ListItem Value="8">8</asp:ListItem>
                                                <asp:ListItem Value="9">9</asp:ListItem>
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="11">11</asp:ListItem>
                                                <asp:ListItem Value="12">12</asp:ListItem>
                                                <asp:ListItem Value="13">13</asp:ListItem>
                                                <asp:ListItem Value="14">14</asp:ListItem>
                                                <asp:ListItem Value="15">15</asp:ListItem>
                                                <asp:ListItem Value="16">16</asp:ListItem>
                                                <asp:ListItem Value="17">17</asp:ListItem>
                                                <asp:ListItem Value="18">18</asp:ListItem>
                                                <asp:ListItem Value="19">19</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="21">21</asp:ListItem>
                                                <asp:ListItem Value="22">22</asp:ListItem>
                                                <asp:ListItem Value="23">23</asp:ListItem>
                                                <asp:ListItem Value="24">24</asp:ListItem>
                                                <asp:ListItem Value="25">25</asp:ListItem>
                                                <asp:ListItem Value="26">26</asp:ListItem>
                                                <asp:ListItem Value="27">27</asp:ListItem>
                                                <asp:ListItem Value="28">28</asp:ListItem>
                                            </asp:DropDownList>
                                            </TD>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tab-page" align="center">
                <h2 class="tab">
                    Contadores</h2>
                <table class="tablewhite" cellspacing="3" cellpadding="3" width="100%" border="0">
                    <tr>
                        <td width="30%">
                            Número Contadores&nbsp;:
                        </td>
                        <td align="right">
                            <asp:TextBox ID="numcont" class="tpequeno" runat="server" CssClass="AlineacionDerecha"></asp:TextBox>
                        </td>
                        <td align="right">
                            <p>
                                <asp:Button ID="cargarc" OnClick="Cargar_Tabla_Rtns" class="bmediano" runat="server"
                                    Text="Cargar Contadores"></asp:Button></p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Número de Contadores Escogidos :
                        </td>
                        <td align="right" colspan="2">
                            <asp:Label ID="cont" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Codigo de Rol Contador :
                        </td>
                        <td align="right" colspan="2">
                            <asp:DropDownList ID="dllRolCont" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:DataGrid ID="DataGrid3" Width="624px" runat="server" BackColor="DarkGray" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Nombre Contador">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbNombre4" onclick="ModalDialog(this,'select MNI.mnit_nombres concat \' \' concat COALESCE(MNI.mnit_nombre2,\' \') concat \' \' concat MNI.mnit_apellidos concat \' \' concat COALESCE(MNI.mnit_apellido2,\' \') as Nombre,MNI.mnit_nit as Nit from mnit MNI order by MNI.mnit_apellidos', new Array(),1)"
                                                runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Nombre")%>'
                                                ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Mnit">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbNombre4a" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Nit")%>'
                                                ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Grupo">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="grupo3" runat="server" Width="72px">
                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                <asp:ListItem Value="6">6</asp:ListItem>
                                                <asp:ListItem Value="7">7</asp:ListItem>
                                                <asp:ListItem Value="8">8</asp:ListItem>
                                                <asp:ListItem Value="9">9</asp:ListItem>
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="11">11</asp:ListItem>
                                                <asp:ListItem Value="12">12</asp:ListItem>
                                                <asp:ListItem Value="13">13</asp:ListItem>
                                                <asp:ListItem Value="14">14</asp:ListItem>
                                                <asp:ListItem Value="15">15</asp:ListItem>
                                                <asp:ListItem Value="16">16</asp:ListItem>
                                                <asp:ListItem Value="17">17</asp:ListItem>
                                                <asp:ListItem Value="18">18</asp:ListItem>
                                                <asp:ListItem Value="19">19</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="21">21</asp:ListItem>
                                                <asp:ListItem Value="22">22</asp:ListItem>
                                                <asp:ListItem Value="23">23</asp:ListItem>
                                                <asp:ListItem Value="24">24</asp:ListItem>
                                                <asp:ListItem Value="25">25</asp:ListItem>
                                                <asp:ListItem Value="26">26</asp:ListItem>
                                                <asp:ListItem Value="27">27</asp:ListItem>
                                                <asp:ListItem Value="28">28</asp:ListItem>
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                            </TD>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <p>
            <asp:Button ID="guardarR" OnClick="Guardar_Roles" runat="server" Text="Guardar Roles en Inventario">
            </asp:Button></p>
    </fieldset>
</asp:Panel>
