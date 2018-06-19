<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Produccion.AdminArbolProducto.ascx.cs" Inherits="AMS.Produccion.AMS_Produccion_AdminArbolProducto" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript">
	function MostrarRefs(obTex,obCmbLin)
	{
		ModalDialog(obTex,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE PLIN.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER By mite_codigo', new Array());
	}
	
	function ValidarExistenciaValor()
	{
		var valReferencia = document.getElementById('<%=tbItemCrear.ClientID%>').value;
		var valLineaBodega = (document.getElementById('<%=ddlLineas.ClientID%>').value.split('-'))[1];
		var salida = AMS_Produccion_AdminArbolProducto.VerificarExistenciaReferencia(valReferencia,valLineaBodega).value;
		if(!salida)
			alert('La referencia '+valReferencia+' no se encuentra registrada dentro del sistema. Por favor revise!');
		return salida;
	}
	
	function ValidarExistenciaValor2()
	{
		var valReferencia = document.getElementById('<%=tbItemCrearConsulta.ClientID%>').value;
		var valLineaBodega = (document.getElementById('<%=ddlLineasConsult.ClientID%>').value.split('-'))[1];
		var intSalida = AMS_Produccion_AdminArbolProducto.VerificarArbolReferencia(valReferencia,valLineaBodega).value;
		if(intSalida == -1)
			return true;
		else if(intSalida == 1)
		{
			alert('La referencia ingresada no se encuentrada registrada en la base de datos. Por favor Revise.');
			return false;
		}
		else if(intSalida == 2)
		{
			alert('La referencia ingresada no se ha configurado el arbol de producción. Por favor Revise.');
			return false;
		}
		return intSalida;
	}
</script>
<table id="Table" class="filtersIn">
	<tr>
		<td style="WIDTH: 22px" align="center" bgColor="#667ab3"><IMG height="70" src="../img/AMS.Avisos.Nuevo.png" border="0"></td>
		<td bgColor="#f2f2f2">
			<P>Mediante esta opción se crean los arboles de producto que se desea producir. Por 
				favor seleccione el item a configurar&nbsp;:</P>
			<table class="filters" cellSpacing="1" cellPadding="0" width="100%" bgColor="#f2f2f2" border="0">
				<tr>
					<td>Linea de Bodega :
						<asp:dropdownlist id="ddlLineas" runat="server"></asp:dropdownlist></td>
					<td>Referencia :
						<asp:textbox id="tbItemCrear" runat="server"></asp:textbox></td>
					<td><asp:button id="btnIngresar" runat="server" Text="Ingresar" onclick="btnIngresar_Click"></asp:button></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td style="WIDTH: 22px" align="center" bgColor="#667ab3"><IMG height="70" src="../img/AMS.Flyers.Consultar.png" border="0"></td>
		<td bgColor="#f2f2f2">
			<P>Por favor seleccione el item a consultar :</P>
			<table class="filters" cellSpacing="1" cellPadding="0" width="100%" bgColor="#f2f2f2" border="0">
				<tr>
					<td>Linea de Bodega :
						<asp:dropdownlist id="ddlLineasConsult" runat="server"></asp:dropdownlist></td>
					<td>Referencia :
						<asp:textbox id="tbItemCrearConsulta" runat="server"></asp:textbox></td>
					<td><asp:button id="btnIngresarConsulta" runat="server" Text="Ingresar" onclick="btnIngresarConsulta_Click"></asp:button></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
