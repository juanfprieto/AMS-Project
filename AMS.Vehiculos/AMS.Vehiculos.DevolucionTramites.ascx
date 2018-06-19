<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Vehiculos.DevolucionTramites.ascx.cs" Inherits="AMS.Vehiculos.DevolucionTramites" %>
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

        function abrirEmergente()
    {
        var nit = document.getElementById('<%=txtNit.ClientID%>');
            ModalDialog(nit, 'SELECT DISTINCT M.MNIT_NIT AS NIT, NOMBRE AS NOMBRES FROM MORDENTRAMITE M, VMNIT VM WHERE M.MNIT_NIT = VM.MNIT_NIT AND M.TEST_ESTADO = \'T\';', new Array(), 1);
                         
    }
   </script>
<fieldset>
<TABLE id="Table" class="filtersIn">
	<tr>
        <td>Almacén :
            <br /><asp:dropdownlist id="almacen" AutoPostBack="true" OnSelectedIndexChanged="cargarDocumentos_SelectedIndexChanged" class="dmediano" runat="server"></asp:dropdownlist></td>
	    </td>
        <td>Prefijo del Documento :
            <br /><asp:dropdownlist id="prefijoDocumento" AutoPostBack="True" class="dmediano" runat="server"></asp:dropdownlist>
		</td>
        <td>Número :
            <br /><asp:TextBox id="numero" ReadOnly="true" class="tpequeno"  runat="server"></asp:TextBox>
        </td>
	</tr>
    <tr>     
       <td> Nit:
       <br /> <asp:TextBox id="txtNit" ondblclick="abrirEmergente();" class="tpequeno" runat="server" ></asp:TextBox>
           <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente();"></asp:Image>
       </td>
        <td>Nombre:
       <br /><asp:TextBox id="txtNita" ReadOnly="true"  class="tmediano" runat="server" ></asp:TextBox></td>	
        <td>Fecha :
       <br /> <asp:TextBox id="txtFecha" class="tpequeno" runat="server" ></asp:TextBox></td>	
	</tr>
    <tr>
        <td><asp:button id="aceptar" runat="server" Enabled="False" Text="Aceptar" CausesValidation="False" OnClick="aceptarTramite_Click" ></asp:button></td>
    </tr>
</TABLE >
<P><asp:panel id="panelCruces" Visible="false"  runat="server">
    		<Table id="Table3">
			<tr>
				<td>
					<asp:Label id="Label1" runat="server"></asp:Label>Prefijo:
					</br><asp:dropdownlist id="ddlPrefijo" class="dmediano" AutoPostBack="True"  runat="server"></asp:dropdownlist></td>
				<TD>
					<asp:Label id="Label2" runat="server"></asp:Label>Numero:
					</br><asp:dropdownlist id="ddlNumero" class="dmediano" runat="server"></asp:dropdownlist></asp:TextBox></TD>
			</tr>
			<TR>	
                <td>
                <asp:button id="btnAceptarTramite" runat="server" Enabled="True" Text="Aceptar"   CausesValidation="False" ></asp:button>		
                </td>
			</TR>
		</Table>
   </asp:panel></P>
    <p><asp:DataGrid id="gridDatos" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False" CellPadding="3" onItemCommand="Agregar_tramite" >
	<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Prefijo del Tramite">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Número del Tramite">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Fecha">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "FECHA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Estado">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Observacion">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "OBSERVACION") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Agregar">
			<ItemTemplate>
				<center>
					<asp:Button id="btnAgregar" runat="server" Text="Agregar" CommandName="Agregar_Documento" />
				</center>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid></p>
    <table>         
      <tr>
		  <td>
            <asp:Label id="lbInfo" Visible="false" runat="server">A continuación se muestran los detalles de la orden de tramite seleccionada</asp:Label>
		  </td>
       </tr>
        </table> 
    <asp:datagrid id="gridDatosTramite" runat="server" cssclass="datagrid" Visible="True" AutoGenerateColumns="False" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
        <Columns> 
		<asp:TemplateColumn HeaderText="Codigo Tramite">
            <ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CODIGOTRA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Numero Tramite">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "NUMEROTRA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Detalle">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "DETALLETRA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Cargo">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "CARGOTRA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Estado Tramite">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "ESTADOTRA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Iva Tramite">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "IVATRA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
                   <asp:TemplateColumn HeaderText="Valor Tramite">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "VALORTRA") %>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	</asp:datagrid>
    <P><asp:panel id="valoresCruce" Visible="false" runat="server">
		<table id="Table2"  class="filtersIn">			
            <tr>
                <td>
                    <asp:Label id="lbValorServ">Valor Servicio</asp:Label></br>
                    <asp:TextBox id="txtvalorServ" class="tpequeno" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label id="lbIvaServ">Iva Servicio</asp:Label></br>
                    <asp:TextBox id="txtivaServ" class="tpequeno" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label id="lbTotalServ">Total Servicio</asp:Label></br>
                    <asp:TextBox id="txtTotal" class="tpequeno" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                     <asp:Label id="lbValorDoc">Valor Documento</asp:Label></br>
                    <asp:TextBox id="txtValorDoc" class="tpequeno" runat="server"></asp:TextBox>
                </td>
                <td>
                     <asp:Label id="lbIvaServ">Iva Documento</asp:Label></br>
                    <asp:TextBox id="txtIvaDoc" class="tpequeno" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label id="lbTotalServ">Total Documento</asp:Label></br>
                    <asp:TextBox id="txtTotalDoc" class="tpequeno" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td>
                    <asp:Label id="lbTotalTram">Total Tramite</asp:Label></br>
                    <asp:TextBox id="txtTotalTramite" class="tpequeno" runat="server" ></asp:TextBox>
                </td>
            </tr>
                        <tr>
				<td>
					<asp:Label id="lbDetalle" runat="server">Observacion</asp:Label></td>
				<td>
					<asp:TextBox id="detalleTransaccion" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                <td></td>
			</tr>
		</table>
	</asp:panel></P>
    <table>
     <tr>
		<td><asp:Label id="lbVendedor" Visible="false" runat="server">Vendedor</asp:Label>
			<asp:dropdownlist id="ddlVendedor" Visible="false" class="dmediano" runat="server"></asp:dropdownlist></td>
                <asp:PlaceHolder ID="plcVendedor" runat="server">
				<td><asp:Label id="lbClave" Visible="false" runat="server">Clave Vendedor</asp:Label>&nbsp;
				<asp:textbox id="tbClaveVend"  Visible="false" runat="server" class="tpequeno" TextMode="Password"></asp:textbox></td>
                </asp:PlaceHolder>
		</tr>
    </table>
<P><asp:button id="guardar" runat="server" Enabled="False" Text="Anular" onclick="guardar_Click"  onClientClick="espera();"></asp:button>&nbsp;
	<asp:button id="cancelar" runat="server" Text="Cancelar"  onclick="cancelar_Click" CausesValidation="False" ></asp:button></P>
<P><asp:label id="lb" runat="server"></asp:label></P>
</fieldset>

