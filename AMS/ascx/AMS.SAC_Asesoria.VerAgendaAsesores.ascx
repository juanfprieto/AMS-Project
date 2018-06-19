<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.SAC_Asesoria.VerAgendaAsesores.ascx.cs" Inherits="AMS.SAC_Asesoria.VerAgendaAsesores" %>
<style type="text/css">
    
.contenedorAgendas
{
    border-radius: 6px;
    width: 90%;
}
.tablaAsesor
{
    border-radius: 20px;
    width: 98%;
    background: white;
}

.tablaAsesor td
{
	border-width: 1px;
	border-style: solid;
	color: Black;
	padding: 5px;
	
}
.ordenAsignada
{
	background:rgba(0, 163, 255, 0.5);
}
.ordenDesarrollo
{
	background:rgba(255, 216, 0, 0.5);
}
.ordenCerrada
{
	background:rgba(101, 255, 87, 0.5);
}
.cuerpoTablaAsesor
{
    display: none;
}
</style>
<script type="text/javascript">
    function desplegar(cuerpoDiv)
    {
        var objeto = "#" + cuerpoDiv;
        if($(objeto).is(":visible") )
            $(objeto).hide();
        else
            $(objeto).show();
    }
</script>
<fieldset>
    <div id="contenedorAgendas" runat="server">
    </div>
</fieldset>