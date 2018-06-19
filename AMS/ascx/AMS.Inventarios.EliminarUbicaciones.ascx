<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.EliminarUbicaciones.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_EliminarUbicaciones" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript">
	function ConfirmarEliminarUbicacion()
	{
		if(confirm('¿Esta seguro de eliminar esta ubicación? \nLos datos se eliminaran permanentemente. \nNo podra deshacer los cambios.'))
		{
			var codigo = document.getElementById('<%=hdnCodigo.ClientID%>').value;
			var tipoUbicacion = document.getElementById('<%=hdnTipoUbicacion.ClientID%>').value;
			var ubicacionEspacial = document.getElementById('<%=hdnUbicacionEspacial.ClientID%>').value;
			
			var url = AMS_Inventarios_EliminarUbicaciones.EliminarUbicacion(codigo,tipoUbicacion,ubicacionEspacial).value;
			
			if (url != null)
				document.location.replace(url);
		}
	}
</script>
<INPUT type="hidden" id="hdnCodigo" runat="server"> <INPUT type="hidden" id="hdnTipoUbicacion" runat="server">
<INPUT type="hidden" id="hdnUbicacionEspacial" runat="server">
<p>
	<asp:Label id="lbPregunta" runat="server">Label</asp:Label></p>
<P>Eliminara las siguientes ubicaciones con sus respectivos ítems asociados.</P>
<asp:datagrid id="dgitems" runat="server" cssclass="datagrid" AutoGenerateColumns="False">
	<Columns>
		<asp:BoundColumn DataField="CODIGO" HeaderText="C&#243;digo">
			<HeaderStyle Width="50px"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="BODEGA" HeaderText="Bodega">
			<HeaderStyle Width="100px"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="ESTANTE" HeaderText="Estante">
			<HeaderStyle Width="100px"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="CAJON" HeaderText="Caj&#243;n">
			<HeaderStyle Width="100px"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="ITEM" HeaderText="&#205;tem">
			<HeaderStyle Width="200px"></HeaderStyle>
		</asp:BoundColumn>
	</Columns>
</asp:datagrid>

<P>&nbsp;</P>
<P><INPUT id="btnEliminar" type="button" value="Eliminar" onclick="ConfirmarEliminarUbicacion();">&nbsp;
	<asp:Button id="btnCancelar" runat="server" Text="Cancelar" onclick="btnCancelar_Click"></asp:Button></P>
<p><asp:label id="lb" runat="server"></asp:label></p>
