<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.PrincipalRelacionItemsGrupo.ascx.cs" Inherits="AMS.Inventarios.PrincipalRelacionItemsGrupo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table class="filters">
  <tbody>
	<tr>
		<th class="filterHead"> 
			<img height="50" src="../img/AMS.Flyers.Nueva.png" border="0">
        </th>
		<td>
        <fieldset>
			<P>
                Se permite la creación de Relaciones entre Items y Grupos, y 
				consecuentemente&nbsp;la subrelacion&nbsp;con&nbsp;catalogos especificos.
				<asp:Button id="ingresarTorre" runat="server" Text="Ingresar" Width="108px" onclick="ingresarTorre_Click"></asp:Button>
            </P>
		</fieldset>
        </td>
	</tr>
	<tr>
		<th class="filterHead">
			<img height="50" src="../img/AMS.Flyers.Edits.png" border="0">
        </th>
		<td>
        <fieldset>
			<P>
                Por favor selecciona un grupo para realizar la edición :<br />
				<asp:DropDownList id="ddlGrupo" class="dpequeno" runat="server" OnChange="CambioGrupo(this);"></asp:DropDownList>
            </P>
			<div id="Existentes" style="DISPLAY:none">
				<P>Por favor selecciona la referencia que desea editar :
					<asp:DropDownList id="ddlReferencia" runat="server"></asp:DropDownList>
					<asp:Button id="btnEditar" Text="Editar" runat="server" onclick="btnEditar_Click"></asp:Button></P>
			</div>
			<div id="NoExistentes" style="DISPLAY:none">
				<P>NO SE HAN ENCONTRADO REFERENCIAS RELACIONADAS</P>
			</div>
            </fieldset>
		</td>
	</tr>
</table>

<script language="javascript">
function CambioGrupo(obj)
{
	if(obj.options.length > 0)
	{
		PrincipalRelacionItemsGrupo.CambioGrupoCarga(obj.value,CambioGrupo_CallBack);
	}
	else
		obj.options.length = 0;
}
function CambioGrupo_CallBack(response)
{
	if (response.error != null)
	{
		alert(response.error);
		return;
	}
	var divEncontrados = document.getElementById("Existentes");	
	var divNoEncontrados = document.getElementById("NoExistentes");
	var ddlReferencias = document.getElementById("<%=ddlReferencia.ClientID%>");
	var opciones = response.value;
	if (opciones == null || typeof(opciones) != "object")
	{
		return;
	}
	ddlReferencias.options.length = 0;
	if(opciones.Tables[0].Rows.length > 0)
	{
		divEncontrados.style.display = '';
		divNoEncontrados.style.display = 'none';
		for (var i = 0; i < opciones.Tables[0].Rows.length; ++i)
		{
			ddlReferencias.options[ddlReferencias.options.length] = new Option(opciones.Tables[0].Rows[i].CodigoModificado,opciones.Tables[0].Rows[i].CodigoOriginal);
		}
	}
	else
	{
		divEncontrados.style.display = 'none';
		divNoEncontrados.style.display = '';
	}
}	
</script>
