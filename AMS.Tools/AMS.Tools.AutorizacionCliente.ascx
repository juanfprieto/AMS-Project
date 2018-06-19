<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.AutorizacionCliente.ascx.cs" Inherits="AMS.Tools.AutorizacionCliente" %>

<script type="text/javascript">

    function cambiarEstado()
    {
        var nit = document.getElementById('<%= txtNit.ClientID%>');
        var obj = document.getElementById('<%= chkCambioNit.ClientID%>');
        if(obj.checked)
        {
            nit.disabled = false;
            nit.readOnly = true;
        }
        else {
            nit.disabled = true;
        }
    }
    function cargarNIT()
    {
        var nit = document.getElementById('<%= txtNit.ClientID%>');
        ModalDialog(nit, 'SELECT mnit_nit as NIT, mnit_apellidos concat \' \' concat coalesce(mnit_apellido2, \'\') concat \' \' concat mnit_nombres concat \' \' concat coalesce(mnit_nombre2,\'\') as Nombre FROM mnit where tvig_VIGENCIA = \'V\' order by mnit_nit', new Array(), 1);      
    }
    function cambiarNombre()
    {
        
        var nit = document.getElementById('<%= txtNit.ClientID%>');
        AutorizacionCliente.obtener_Nombre(nit.value, cambiarNombre_Response);
    }

    function cambiarNombre_Response(response)
    {
        var nombre = document.getElementById('<%= txtNombre.ClientID%>');
        var rta = response.value;
        nombre.value = rta;

    }
</script>
<div id="divMensaje"  runat="server" style="text-align : justify; cursor:move; padding: 2em;">
    <asp:DataGrid id="grillaElementos" runat="server" AutoGenerateColumns="false" ShowFooter="false"  Enabled="true" style=" box-shadow: 1px 5px 20px 3px; font-size: small; border-radius:25px; text-align-last: center; border-style: hidden; background-color: floralwhite;">
        <FooterStyle  ></FooterStyle>
		<HeaderStyle Font-Bold="true" Height="30px"></HeaderStyle>
		<SelectedItemStyle ></SelectedItemStyle>
		<AlternatingItemStyle ></AlternatingItemStyle>
		<ItemStyle ></ItemStyle>
		<Columns>
            <asp:TemplateColumn HeaderText="Código Encuesta" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PBAS_CODIGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Tipo Base De Datos" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "TBAS_CODIGO") %>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descripción" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "PBAS_DESCRIPCION") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Respuesta" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
				<ItemTemplate>
					<asp:radiobutton id="rdSi" runat="server" enabled="true" GroupName="grupo" Text="Si"></asp:radiobutton> <br />
                    <asp:radiobutton id="rdNo" runat="server" enabled="true" GroupName="grupo" Text="No"></asp:radiobutton> <br />
                    <asp:radiobutton id="rdNR" runat="server" enabled="true" GroupName="grupo" Text="No responde"></asp:radiobutton>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
    <asp:Label ID="lbRazon" runat="server" ></asp:Label>
</div>
<table id="Table" class="filtersIn">
    <tr>
        <td>Nit: <br /><asp:textbox id="txtNit" runat="server" enabled="false" onClick="cargarNIT();" onBlur="cambiarNombre();"></asp:textbox></td>
            
        <td><asp:CheckBox id="chkCambioNit" runat="server" AutoPostBack="false" onchange="cambiarEstado();" Text="CAMBIAR NIT" Visible="true" /></td>
    </tr>
    <tr>
        <td width="60%">Nombre:<asp:textbox id="txtNombre" runat="server" enabled="false"></asp:textbox></td>
        <td width="40%">Tipo Base De Datos: <br /><asp:textbox id="txtTipoBase" runat="server" enabled="false"></asp:textbox></td>
    </tr>
    <%--<tr>
        <br />
        <td>
            <asp:radiobutton id="rdSi" runat="server" enabled="true" GroupName="grupo" Checked="true" Text="Si"></asp:radiobutton> &nbsp;&nbsp;&nbsp;
            <asp:radiobutton id="rdNo" runat="server" enabled="true" GroupName="grupo" Text="No"></asp:radiobutton>
        </td>
        <td></td>
    </tr>--%>
</table>
<center>
    <p>
	    <asp:button id="btnGuardar" Runat="server" UseSubmitBehavior="false" onClientClick="espera();" onclick="GuardarDatos" Text="Guardar" class="bpequeno"></asp:button>
        <asp:button id="btnIgnorar" Runat="server" UseSubmitBehavior="false" onClientClick="espera();" onclick="IgnorarDatos" Text="Ignorar" class="bpequeno"></asp:button>
    </p>
</center>

