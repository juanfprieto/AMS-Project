<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.importarCCA.ascx.cs" Inherits="AMS.Automotriz.importarCCA" %>
<p>
	Proceso de ACTUALIZACION de PRECIOS, CREACION de ITEMS y realización de 
    SUSTITUCIONES a partir del archivo CAREDISK2 suministrado por la C.C.A.</p>
<p>
	<asp:Label id="Label3" runat="server">Por favor indique la ruta del archivo para 
    importar la lista con nombre CAREDISK2 suministradas por la CCA:</asp:Label>
</p>
<p>
	Archivo a Utilizar :&nbsp;<input id="fDocument" type="file" runat="server">&nbsp;&nbsp;&nbsp;
	
	<asp:DropDownList id="tablaAct" runat="server" Visible="False"></asp:DropDownList>
</p>
<p>
	&nbsp;</p>
<p style="width: 266px">
	Línea de los Items a realizar la actualización: &nbsp;&nbsp;&nbsp;
<asp:DropDownList ID="DdlLineaItems" runat="server">
</asp:DropDownList></p>
<p style="width: 266px">
	Lista de Precios de los Items a Actualizar : &nbsp;&nbsp;&nbsp;
<asp:DropDownList ID="DdlListaPrecios" runat="server">
</asp:DropDownList></p>
<p>
<p style="width: 266px">
	Documento para realizar las sustituciones : &nbsp;&nbsp;&nbsp;
<asp:DropDownList ID="DdlAjusteSustitucion" runat="server">
</asp:DropDownList></p>
<p>
	&nbsp;</p>
<p>
	Por favor, asegurese de haber seleccionado correctamente las opciones y de tener 
    back-up de la Base de Datos</p>
<p>
	&nbsp;</p>
<p>
	<asp:Button id="actual3" onclick="actualizar" runat="server" Text="Actualizar"></asp:Button>
</p>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>

