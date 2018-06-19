<%@ Control Language="c#" autoeventwireup="false" Inherits="AMS.Tools.AdministrarClientes" %>

<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<fieldset>
<table class="filters">
    <tbody>
        <tr>
        
            <th class="filterHead">
				<img height="70" src="../img/SAC.Flyers.Crear.png" border="0">
			</th>
            <td>
                Crear un nuevo&nbsp;cliente&nbsp;<asp:Button id="btnCrear" onclick="btnCrear_Click" runat="server" Text="Crear Cliente"></asp:Button>
            </td>
       </tr>
       <tr>
            <th class="filterHead">
				<img height="70" src="../img/SAC.Flyers.Modificar.png" border="0">
			</th>
            <td>
                <p>
                    Modificar la información de un usuario existente 
                </p>
                <p>
                    Ingrese el nit del cliente 
                    <asp:TextBox id="tbMod" ondblclick="ModalDialog(this,'SELECT MCLI.mnit_nit, MNIT.mnit_nombres CONCAT \' \' CONCAT MNIT.mnit_nombre2 CONCAT \' \' CONCAT MNIT.mnit_apellidos CONCAT \' \' CONCAT MNIT.mnit_apellido2 as Nombre FROM dbxschema.mnit MNIT,dbxschema.mclientesac MCLI WHERE MNIT.mnit_nit=MCLI.mnit_nit  ORDER BY MCLI.mnit_nit',new Array())" runat="server" ToolTip="Haga doble click para iniciar la busqueda o si prefiere digite el nit"></asp:TextBox>
                    <asp:Button id="btnModificar" onclick="btnModificar_Click" runat="server" Text="Modificar Cliente"></asp:Button>
                </p>
            </td>
            </tr>
            <tr>
      
            <th class="filterHead">
				<img height="70" src="../img/SAC.Flyers.EliminarCliente.png" border="0">
			</th>
            <td>
                <p>
                    Eliminar un cliente 
                </p>
                <p>
                    Ingrese el nit del cliente 
                    <asp:TextBox id="tbEli" ondblclick="ModalDialog(this,'SELECT MCLI.mnit_nit, MNIT.mnit_nombres CONCAT \' \' CONCAT MNIT.mnit_nombre2 CONCAT \' \' CONCAT MNIT.mnit_apellidos CONCAT \' \' CONCAT MNIT.mnit_apellido2 as Nombre FROM dbxschema.mnit MNIT,dbxschema.mclientesac MCLI WHERE MNIT.mnit_nit=MCLI.mnit_nit AND MNIT.tnit_tiponit=\'C\' AND MCLI.tvig_codigo <> \'C\' ORDER BY MCLI.mnit_nit',new Array())" runat="server" ToolTip="Haga doble click para iniciar la busqueda o si prefiere digite el nit"></asp:TextBox>
                    <asp:Button id="btnEliminar" onclick="btnEliminar_Click" runat="server" Text="Eliminar Cliente"></asp:Button>
                </p>
            </td>
        </tr>
    </tbody>
</table>
</fieldset>
