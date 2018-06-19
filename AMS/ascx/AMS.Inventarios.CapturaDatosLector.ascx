<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Inventarios.CapturaDatosLector.ascx.cs" Inherits="AMS.Inventarios.CapturaDatosLector" %>
<script language="javascript">
    

    function obtenerItem(objCodigo) {
        CapturaDatosLector.Cargar_Item(objCodigo.value, obtenerItem_CallBack);
    }

    function obtenerItem_CallBack(response) {
        var respuesta = response.value;
        if (respuesta == '') {
            alert('El codigo ingresado no existe en la tabla maestra de items!');
            var lblNombreItem = document.getElementById("<%=txtNombreReferencia.ClientID%>");
            lblNombreItem.value = '';
        }
        else {
            var lblNombreItem = document.getElementById("<%=txtNombreReferencia.ClientID%>");
            lblNombreItem.value = respuesta;
            var btnGuardar = document.getElementById("<%=btnGuardarConteo.ClientID%>");
            var chkAutomatico = document.getElementById("<%=chkAutomatico.ClientID%>");
            if (chkAutomatico.checked) {
                $(btnGuardar).trigger('click');
            }
        }
    }
</script>

<fieldset><legend>Inventario Físico</legend>
	<TABLE id="Table1" class="filtersIn">
		<tr>
			<td>Seleccione el Inventario:<br />
			<asp:dropdownlist id="ddlInventarios" class="dmediano" runat="server">
            </asp:dropdownlist>
            </td>
		</tr>
		<tr>
			<td colSpan="2"><asp:button id="btnAceptar" runat="server" Text="Cargar Información" CausesValidation="False" onclick="btnAceptar_Click"></asp:button>&nbsp;
				<asp:button id="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False" onclick="btnCancelar_Click"></asp:button></td>
		</tr>
	</table>
</fieldset>

<asp:panel id="pnlInfoProceso" Runat="server" Visible="False">
<DIV class="tab-page" id="pnlTarjetaPendiente" align="center" runat="server">
	<FIELDSET><LEGEND>Información Tajeta de Conteo</LEGEND>
		<TABLE id="Table2" class="filtersIn">
            <TR>
				<TD>
                    <asp:CheckBox id="chkAutomatico" runat="server"
                    Text="Guardar Automaticamente"
                    TextAlign="left"/>
                </TD>
                <td></td>
            </TR>
			<TR>
				<TD>
					<asp:Label id="lbTxAlm" runat="server">Almacen de la Ubicación :</asp:Label></TD>
				<TD>
					<asp:DropDownList id="ddlAlmacen" runat="server" class="dpequeno" OnChange=""></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="lbTxtContAct" runat="server">Cantidad Leida con el Lector :</asp:Label></TD>
				<TD>
					<asp:TextBox id="txtCantidadLeida" runat="server" class="tpequeno"></asp:TextBox></TD>
			</TR>
			<TR>
			    <TD>
				    <asp:Label id="lbTxCodRef" runat="server">Código de Referencia :</asp:Label></TD>
			    <TD>
				   <asp:TextBox id="txtCodigoReferencia" runat="server" class="tpequeno" onblur="obtenerItem(this);" ></asp:TextBox></TD></TD>
		    </TR>
		    <TR>
			    <TD>
				    <asp:Label id="lbTxNomRef" runat="server">Nombre de Referencia :</asp:Label></TD>
			    <TD>
				    <asp:TextBox id="txtNombreReferencia" runat="server" class="tmediano" Enabled="false"></asp:TextBox></TD>
		    </TR>
            <TR>
                <TD>
                </TD>
				<TD>
					<P>
						<asp:Label id="lblInfo" runat="server"></asp:Label></P>
				</TD>
			</TR>
			<TR>
				<TD align="right" colSpan="2">
					<P>
						<asp:Button id="btnGuardarConteo" runat="server" Text="Guardar Conteo" onclick="btnGuardarConteo_Click"></asp:Button></P>
				</TD>
			</TR>
		</TABLE>
	</FIELDSET>
</DIV>

</asp:panel>