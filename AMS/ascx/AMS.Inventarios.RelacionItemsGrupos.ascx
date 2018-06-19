<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.RelacionItemsGrupos.ascx.cs" Inherits="AMS.Inventarios.RelacionItemsGrupos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript">
function MuestreoReferencias(ob,obCmbLin)
{
	ModalDialog(ob,'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE FROM mitems MIT, plineaitem PLIN  WHERE MIT.plin_codigo=\''+(obCmbLin.value.split('-'))[0]+'\' AND MIT.plin_codigo=PLIN.plin_codigo ORDER By MIT.mite_codigo', new Array());
}
function NumericInput()
{
	var charCode = event.keyCode;
	if(charCode < 48 || charCode > 57)
		return false;
}

function StateCantidadCatalogo(objChk, objTb)
{
	if(objChk.checked)
	{
		objTb.readOnly = false;
	}
	else
	{
		objTb.readOnly = true;
		objTb.value = '';
	}
}
</script>

<fieldset>
    <legend>Información basica</legend>
    	<table id="table1" class"filtersIn">      
	    	    <tr>
	    		    <td>
                        Grupos:<br>
                        <asp:dropdownlist id="ddlGrupos" runat="server"></asp:dropdownlist>
                </td>
		        </tr>
		        <tr>
			        <td>
                            Linea:<br>
                            <asp:dropdownlist id="ddlLinea" runat="server"></asp:dropdownlist>                    
                    <br>
                            Item: <br>            
                            <asp:requiredfieldvalidator id="rqItem" runat="server" ErrorMessage="Por favor ingrese el Item" Display="None"
			                 ControlToValidate="tbItems"></asp:requiredfieldvalidator><asp:textbox id="tbItems" runat="server" CssClass="AlineacionDerecha"></asp:textbox>
                    </td>
		        </tr>
		    <tr>
		    <td>
                Cantidad de uso por vehiculo de este grupo:<br>        
                <asp:textbox onkeypress="return NumericInput();" id="tbCantidad" runat="server" MaxLength="3"
					    class="tpequeno" CssClass="AlineacionDerecha"></asp:textbox><asp:regularexpressionvalidator id="revCantidad" runat="server" ErrorMessage="Por favor ingresa un valor valido para la cantidad de uso"
					    Display="None" ControlToValidate="tbCantidad"></asp:regularexpressionvalidator>           
            <BR>
                <asp:button id="btnMostrarCatalogos" runat="server" Text="Mostrar Catalogos" CausesValidation="False" onclick="btnMostrarCatalogos_Click"></asp:button><INPUT id="hdCodigoGrupo" type="hidden" runat="server"><INPUT id="hdModo" type="hidden" runat="server">
				    <INPUT id="hdCodigoItem" type="hidden" runat="server">
				    <asp:requiredfieldvalidator id="rqGrupo" runat="server" ErrorMessage="Por favor ingrese el valor del grupo"
					    Display="None" ControlToValidate="ddlGrupos"></asp:requiredfieldvalidator>
            </td>
		    </tr>
          
	</table>
</fieldset>

<asp:datagrid id="dgCatalogosRelacionados" runat="server" cssclass="datagrid" AutoGenerateColumns="False"
	GridLines="Vertical" CellPadding="3">
	<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass="item"></ItemStyle>
	<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
	<FooterStyle cssclass="footer"></FooterStyle>
	<Columns>
		<asp:BoundColumn DataField="pcat_codigo" HeaderText="Codigo de Catalogo"></asp:BoundColumn>
		<asp:BoundColumn DataField="pcat_descripcion" HeaderText="Descripci&#243;n del Catalogo"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Agregar como relacionado">
			<ItemTemplate>
				<asp:CheckBox id="chkAdd" runat="server" Text="Agregar" TextAlign="Left"></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Cantidad de Uso por Vehiculo">
			<ItemTemplate>
				<asp:TextBox id="tbCantidadCata" runat="server" ReadOnly="True" OnKeyPress="return NumericInput();"
					class="tpequeno" MaxLength="3" CssClass="AlineacionDerecha"></asp:TextBox>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle HorizontalAlign="Center" cssclass="pager" Mode="NumericPages"></PagerStyle>
</asp:datagrid><asp:button id="btnAceptar" runat="server" cssclass="datagrid" Text="Aceptar" onclick="btnAceptar_Click"></asp:button><asp:label id="lb" runat="server"></asp:label><asp:validationsummary id="vsTotal" runat="server" ShowMessageBox="True" ShowSummary="False"></asp:validationsummary>
