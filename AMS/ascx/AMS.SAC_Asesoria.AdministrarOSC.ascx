<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.SAC_Asesoria.AdministrarOSC.ascx.cs" Inherits="AMS.SAC_Asesoria.AdministrarOSC" %>
<fieldset>
<table class="filters">
  <tbody>
    <tr>
        <th class="filterHead">
			<img height="70" src="../img/SAC.Avisos.CrearOSC.png" border="0">
		</th>
        <td>
            <h3>Cree una Orden de Servicio al Cliente</h3>
            <asp:button id=btnCrear onclick=btnCrear_Click runat="server" Text="Crear Orden"></asp:Button>          
        </td>
    </TR> 
    <tr>  
        <th class="filterHead">
	        <img id="imgAsignarOSC" src="../img/AsignarOSC.png" border="0">            
        </th>
        <td>
            <h3>Asignación y Reasignación de Ordenes de Servicios:</h3>
            <asp:button id=btnAsignar onclick=btnAsignar_Click runat="server" Text="Asignar"></asp:Button>
        </TD>
    </TR>
<%--    <tr>
        <th class="filterhead">
			<img height="70" src="../img/sac.avisos.responderosc.png" border="0">
		</th>
        <td>
            <h3>de respuesta a las solicitudes planteadas en una orden de servicio</h3>
            <h4>escoja la orden de servicio:</h4>
                <asp:dropdownlist id=ddloscresp runat="server"></asp:dropdownlist>
                <asp:button id=btnresponder onclick=btnresponder_click runat="server" text="aceptar"></asp:button> 
        </td>
    </tr>--%>
    <tr>
        <th class="filterHead">
	        <img height="70" src="../img/SAC.Avisos.ReAbrirOSC.png" border="0">
        </th>
        <td>
            <h3>Seleccione el número de la OSC para re-abrir</h3>
            <P><asp:dropdownlist id=ddlreabosc CssClass="dpequeno" runat="server"></asp:DropDownList>
			<asp:Button id="btnReAbrir" runat="server" Text="Aceptar" OnClick="btnReAbrir_Click" OnClientClick="return CallConfirmBox(this, '¿Esta seguro que desea re-abrir esta solicitud?');"></asp:Button></P>
        </td>
    </TR>
  </TBODY>
</TABLE>
<asp:Label id="lb" runat="server"></asp:Label>
</fieldset>






















