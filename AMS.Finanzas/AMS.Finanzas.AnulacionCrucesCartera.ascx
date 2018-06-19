<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Finanzas.AnulacionCrucesCartera.ascx.cs" Inherits="AMS.Finanzas.AnulacionCrucesCartera"%>
	<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        //mirar documentacion JQueryUI sobre datepicker
                var fechaVal = $("#<%=txtFecha.ClientID%>").val();
        $("#<%=txtFecha.ClientID%>").datepicker();
        $("#<%=txtFecha.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtFecha.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtFecha.ClientID%>").val(fechaVal);
    });
   </script>
<fieldset>
<TABLE id="Table" class="filtersIn">
	<TR>
		<td>Almacén :
		<br /><asp:dropdownlist id="almacen" AutoPostBack="True" class="dmediano" runat="server"></asp:dropdownlist></td>        
    </tr>
      <tr>
        <TD>Número :<br /><asp:label id="numeroTesoreria" class="lpequeno" runat="server"></asp:label></TD>
	  </TR>    
	<TR>
		<td>Prefijo del Documento :<br /><asp:dropdownlist id="prefijoDocumento" AutoPostBack="True" onselectedindexchanged="prefijoCruce_SelectedIndexChanged" class="dmediano" runat="server" ></asp:dropdownlist></td>
	</TR>
    <td>Fecha :
    <asp:TextBox id="txtFecha" onkeyup="DateMask(this)" runat="server" Width="92px"></asp:TextBox></td>
	<TR>

		<TD><asp:button id="aceptar" runat="server" Enabled="False" Text="Aceptar" CausesValidation="False" onclick="aceptar_Click"></asp:button></TD>
	</TR>
</TABLE  class="filtersIn">
<P><asp:panel id="panelCruces" Visible="false"  runat="server">
    		<TABLE id="Table3">
			<TR>
				<TD>
					<asp:Label id="Label1" runat="server"></asp:Label>Prefijo:
					</br><asp:dropdownlist id="ddlPrefijo" class="dmediano" AutoPostBack="True" onselectedindexchanged="prefijoCruce_SelectedIndexChanged" runat="server"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="Label2" runat="server"></asp:Label>Numero:
					</br><asp:dropdownlist id="ddlNumero" class="dmediano" runat="server"></asp:dropdownlist></asp:TextBox></TD>
			</TR>
			<TR>	
                <td>
                <asp:button id="btnAceptarCruce" runat="server" Enabled="True" Text="Aceptar" CausesValidation="False" onclick="aceptarTramite_Click"></asp:button>		
                </td>
			</TR>
		</TABLE>
   </asp:panel></P></P>
    <p><asp:datagrid id="gridDatos" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
	</asp:datagrid></p>
    <P><asp:panel id="valoresCruce" Visible="false" runat="server">
		<TABLE id="Table2"  class="filtersIn">
            <TR>
				<TD>
					<asp:Label id="txtTotal" runat="server">Total Cruce</asp:Label></TD>
				<TD>
                    <asp:Label id="lbValorCruce" runat="server"></asp:Label></TD>
            </TR>
			<TR>
				<TD>
					<asp:Label id="lbDetalle" runat="server">Detalle Anulación</asp:Label></TD>
				<TD>
					<asp:TextBox id="detalleTransaccion" runat="server" TextMode="MultiLine"></asp:TextBox></TD>
			</TR>
		</TABLE>
	</asp:panel></P>
<P><asp:button id="guardar" runat="server" Enabled="False" Text="Guardar" onclick="guardar_Click" onClientClick="espera();"></asp:button>&nbsp;
	<asp:button id="cancelar" runat="server" Text="Cancelar" CausesValidation="False" onclick="cancelar_Click"></asp:button></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
</fieldset>
