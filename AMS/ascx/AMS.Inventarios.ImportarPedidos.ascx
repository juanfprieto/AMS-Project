<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.ImportarPedidos.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_ImportarPedidos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>



<FIELDSET><LEGEND>Archivo Maestro</LEGEND>
	<P>Seleccione un archivo Excel: <INPUT id="inpArchivoMaestro" type="file" size="50" name="inpArchivoMaestro" runat="server"></P>
	<P>Nombre del Rango: <asp:textbox id="txtNombreRangoMaestro" runat="server"></asp:textbox></P>
</FIELDSET>
<FIELDSET ><LEGEND>Archivo Detalle</LEGEND>
	<P>Seleccione un archivo Excel: <INPUT id="inpArchivoDetalle" type="file" size="50" name="inpArchivoDetalle" runat="server"></P>
	<P>Nombre del Rango: <asp:textbox id="txtNombreRangoDetalle" runat="server"></asp:textbox></P>
</FIELDSET>
<P><asp:button id="btnSubir" runat="server" Text="Cargar Archivo" onclick="btnSubir_Click"></asp:button></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
