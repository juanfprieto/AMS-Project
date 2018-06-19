<%@ Control Language="C#" codebehind="AMS.Finanzas.Tesoreria.ActivosFijos.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ActivosFijos" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.News.png" border="0">
			</th> 
			<td>
            <p>
            <table id="Table" class="filtersIn">
            <tr>
            <td>
				&nbsp;Ingrese un nuevo activo fijo :
				<asp:Button id="btnIngresar" onclick="btnIngresar_Click" runat="server" Text="Ingresar"></asp:Button>
                </td>
                </tr>
                </table>
                </p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Edits.png" border="0">
			</th>
			<td>
                <p>
            <table id="Table1" class="filtersIn">
            <tr>
            <td>
					Edite la información de un activo fijo existente
				<br />
					Ingrese el código del activo fijo a editar :
					<asp:TextBox id="tbEditar" ondblclick="ModalDialog(this,'SELECT mafj_codiacti,mafj_descripcion FROM mactivofijo',new Array());"
						runat="server" class="tpequeno" ToolTip="Haga Doble Click para iniciar la busqueda"></asp:TextBox>
					&nbsp;<asp:Button id="btnEditar" onclick="btnEditar_Click" runat="server" Text="Editar"></asp:Button>
              </td>
                </tr>
                </table>
				</p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Eliminar.png" border="0">
			</th>
			<td>
				  <p>
            <table id="Table2" class="filtersIn">
            <tr>
            <td>
					Elimine un activo fijo existente
				<br />
					Ingrese el código del activo fijo a&nbsp;eliminar :
					<asp:TextBox id="tbEliminar" ondblclick="ModalDialog(this,'SELECT mafj_codiacti,mafj_descripcion FROM mactivofijo',new Array());"
						runat="server" class="tpequeno" ToolTip="Haga Docle Click para iniciar la busqueda"></asp:TextBox>
					&nbsp;<asp:Button id="btnEliminar" onclick="btnEliminar_Click" runat="server" Text="Eliminar"></asp:Button>
				</td>
                </tr>
                </table>
				</p>
			</td>
		</tr>
	</tbody>
</table>
</fieldset>