<%@ outputcache duration="1" varybyparam="params" %>
<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.WizardDirection.aspx.cs" Inherits="AMS.Web.WizardDirection" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
<script language="javascript">
	function AgregarNom(obCmbNom, obOut) {
	    var seleccion = document.getElementById(obCmbNom);
	    var salida = document.getElementById(obOut);
	    salida.value = salida.value + ' ' + seleccion.value;
    }
	
    function AgregarLit(obLit,obOut)
    {
        var seleccion = document.getElementById(obLit);
        var salida = document.getElementById(obOut);
        salida.value = salida.value + ' ' + seleccion.value;
        seleccion.value = '';
    }
	
    function LimpiarControles(obLit,obOut) {
        var seleccion = document.getElementById(obLit);
        var salida = document.getElementById(obOut);
        salida.value = '';
        seleccion.value = '';
    }

    
	</script>
	</HEAD>
	<body class="mainApp">
		<form runat="server">
			<fieldset >
				<legend class="Legends">Información Nomenclatura</legend>
				<table class="filtersIn" >
					<tbody>
						<tr>
							<td width="33%">
								Opción Nomenclatura :
							</td>
							<td align="right" width="33%">
								<asp:DropDownList id="ddlNomDir" runat="server"></asp:DropDownList>
							</td>
							<td align="right" width="33%">
								<asp:Label id="lbAgregar1" runat="server" cssclass="PunteroMano">Agregar</asp:Label></td>
						</tr>
						<tr>
							<td>
								Opción Literatura :</td>
							<td align="right">
								<asp:TextBox id="tbLiteral" onkeyup="AlfaNumMask(this)" runat="server"></asp:TextBox>
							</td>
							<td align="right">
								<asp:Label id="lbAgregar2" runat="server" cssclass="PunteroMano">Agregar</asp:Label></td>
						</tr>
					</tbody>
				</table>
			</fieldset>
			<fieldset>
				<legend class="Legends">Nomenclatura </legend>
				<asp:TextBox id="tbOut" runat="server" ReadOnly="True"></asp:TextBox>
			</fieldset>
			<p>
				<asp:Label id="lbClear" runat="server" cssclass="PunteroMano">Limpiar</asp:Label>&nbsp;&nbsp;<asp:Label id="lbCerrar" runat="server" cssclass="PunteroMano" onClick="parent.CerrarVentana(document.getElementById('tbOut').value );">Terminar</asp:Label>
			</p>
		</form>
	</body>
</HTML>
