<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.SAC_Asesoria.AgendaActualizaciones.ascx.cs" Inherits="AMS.SAC_Asesoria.AgendaActualizaciones" %>


<asp:PlaceHolder id="plcOpciones" runat="server" visible="true">
    <table>
        <tr>
            <td>
                <asp:ImageButton id="btnAgregar" OnClick="AgregarModificacion"  UseSubmitBehavior="false" runat="server" ImageUrl="../img/AgregarMod.png" BorderWidth="0px"></asp:ImageButton>
            </td>
            <td>
                AGREGAR: Adicione los objetos y descripciones necesarias para realizar su correcta actualización.
            </td>
            <td>
                <asp:ImageButton id="btnRevisar" OnClick="RevisarModificacion"  UseSubmitBehavior="false" runat="server" ImageUrl="../img/RevisarMod.png" BorderWidth="0px"></asp:ImageButton>    
            </td>
            <td>
                REVISAR: Indentifique que elementos estan pendientes por actualizar para un cliente especifico.
            </td>
            <td>
                <asp:ImageButton id="btnHistorial" OnClick="Cambio_Empresa"  UseSubmitBehavior="false" runat="server" ImageUrl="../img/HistorialMod.png" BorderWidth="0px"></asp:ImageButton>    
            </td>
            <td>
                HISTORIAL: Estudie las actualizaciones realizadas registradas en el sistema.
            </td>
        </tr>
    </table> 
</asp:PlaceHolder>
<br />
<asp:PlaceHolder id="plcMarcadorActualizaciones" runat="server" visible="true">
    <fieldset style="width:40%; left:0;position:absolute;left:8%;">
        <legend>Marcador de Actualizaciones Pendientes</legend>
        <br />
        <ASP:DATAGRID id="dgMarcadorAct" runat="server" enableViewState="true" OnItemDataBound="DgMarcadorActDataBound"
		Font-Size="8pt" Font-Name="Verdana" CellPadding="3" ShowFooter="False" BorderColor="#999999" HeaderStyle-BackColor="#ccccdd"
		BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana" 
		AutoGenerateColumns="false" style="table-layout=auto;">
	        <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
	        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
	        <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
	        <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
	        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
	        <Columns>
		        <asp:TemplateColumn HeaderText="Cliente">
                    <ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "Cliente") %>
					</ItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="Pendientes" >
                    <ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "Pendientes") %>
					</ItemTemplate>
		        </asp:TemplateColumn>
            </Columns>
        </ASP:DATAGRID>
        <br />
        <div style="font-size: 11px;">*La fila de borde azul indica el promedio de actualizaciones pendientes que las empresas deberian tener.</div>
    </fieldset>
</asp:PlaceHolder>

<asp:PlaceHolder id="plcAgregar" runat="server" visible="false">
    <fieldset>
        <legend>Agregar Actualización Pendiente</legend>
        <br />
        Motivo: <asp:TextBox id="txtMotivo" runat="server" class="tgrande" EnableViewState="false"></asp:TextBox>
        <br /><br />
        <ASP:DATAGRID id="dgAgregar" runat="server" enableViewState="true" 
        OnItemDataBound="DgInsertsDataBound" OnItemCommand="DgInserts_AddAndDel" OnUpdateCommand="DgInserts_Update"
		OnDeleteCommand="DgInserts_Delete" OnCancelCommand="DgInserts_Cancel"  OnEditCommand="DgInserts_Edit"
		Font-Size="8pt" Font-Name="Verdana" CellPadding="3" ShowFooter="True" BorderColor="#999999" HeaderStyle-BackColor="#ccccdd"
		BackColor="White" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" Font-Names="Verdana"
		AutoGenerateColumns="false" style="table-layout=auto;">
	        <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
	        <HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
	        <PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
	        <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
	        <AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
	        <ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
	        <Columns>
		        <asp:TemplateColumn HeaderText="Objeto">
                    <ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "Objeto") %>
					</ItemTemplate>
                    <EditItemTemplate>
					    <asp:DropDownList id="ddlEdit_Objetos" class="dpequeno" runat="server" ></asp:DropDownList>
				    </EditItemTemplate>
			        <FooterTemplate>
				         <asp:DropDownList id="ddlObjetos" class="dpequeno" runat="server"></asp:DropDownList>
			        </FooterTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="Especificación" >
                    <ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "Especificacion") %>
					</ItemTemplate>
                    <EditItemTemplate>
					    <asp:TextBox  id="txtEdit_Especificacion" TextMode="MultiLine" Wrap="true" runat="server" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem, "Especificacion") %>' />
				    </EditItemTemplate>
			        <FooterTemplate>
				        <asp:TextBox id="txtEspecificacion" TextMode="MultiLine" Wrap="true" runat="server" placeholder="Escriba su especificación..." EnableViewState="false"></asp:TextBox>
			        </FooterTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="Operación">
			        <ItemTemplate>
				        <asp:Button ID="btnDel"  CommandName="Delete" Text="Quitar" Runat="server"  />
			        </ItemTemplate>
			        <FooterTemplate>
				        <asp:Button ID="btnAdd" CommandName="AddDatasRow" Text="Agregar"  Runat="server" width="70px" />
				        <asp:Button ID="btnClear" CommandName="ClearRows" Text="Reiniciar"  Runat="server" width="70px" />
			        </FooterTemplate>
		        </asp:TemplateColumn>
		        <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar"></asp:EditCommandColumn>
	        </Columns>
        </ASP:DATAGRID>
        <br />
        <center><asp:Button id="btnEnviarMod" onclick="EnviarModificacion" runat="server" Text="ENVIAR"></asp:Button></center>
    </fieldset>
</asp:PlaceHolder>

<asp:PlaceHolder id="plcRevisar" runat="server" visible="false">
    <fieldset>
        <legend><div id="tituloAccion" runat="server">Revisión de Actualizaciones</div></legend>
        <br />
        <asp:DropDownList id="ddlEmpresas" class="dmediano" runat="server" OnSelectedIndexChanged="Cambio_Empresa" AutoPostBack="true"></asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList id="ddlPendientesCerrar" class="dmediano" runat="server" Visible="false"></asp:DropDownList>
        <asp:Button id="btnActualizado" onclick="ActualizacionHecha" runat="server" Text="ACTUALIZACION HECHA!" Visible="false"></asp:Button>
        <br /><br />
        <div id="divPreliminares" style="width:80%;">
            <fieldset style='background-color:#F5FFFF;'>
                PRELIMINARES:<br />
                P1. Siempre que se realice una modificación (agregar columna o modificar tipo de campo) a la estructura de una tabla en la DB, se debe realizar 
                    una operacion de REORG a la tabla modificada, ademas, en caso de existir Vistas inoperativas es necesario recrear todas las VISTAS en la base de datos
                    (Usar Quest DB para esto).<br />
            </fieldset>
        </div>
        <br />
        <div id="divPendientes" runat="server">
        </div>
    </fieldset>
</asp:PlaceHolder>
