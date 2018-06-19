<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Inventarios.CapturaDatos.ascx.cs" Inherits="AMS.Inventarios.AMS_Inventarios_CapturaDatos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript">
	function ValSelInvFis()
	{
		if(document.getElementById('<%=ddlInventarios.ClientID%>').value == '')
		{
			alert('Seleccione un inventario físico para mirar la observación!');
			return false;

		}
	}
	
	function CambioReferencia()
	{
		var ddlInvFisi = document.getElementById('<%=ddlInventarios.ClientID%>');
		var ddlReferen = document.getElementById('<%=ddlReferencias.ClientID%>');
		var arrValores = AMS_Inventarios_CapturaDatos.ConsultarInfoReferenciaInventarioFisico((ddlReferen.value.split('$'))[0],parseInt((ddlReferen.value.split('$'))[1]),(ddlInvFisi.value.split('-'))[0],parseInt((ddlInvFisi.value.split('-'))[1])).value.split('&');
		
		document.getElementById('<%=lbUltimoConteo.ClientID%>').innerText = arrValores[0];
		document.getElementById('<%=lbConteo1.ClientID%>').innerText = arrValores[1];
		document.getElementById('<%=lbConteo2.ClientID%>').innerText = arrValores[2];
		document.getElementById('<%=lbConteo3.ClientID%>').innerText = arrValores[3];
	}
	
	function ValidacionGeneracion()
	{
		if(document.getElementById('<%=ddlConteosRelacionados.ClientID%>').value == '1' && document.getElementById('<%=rblTipoConteo.ClientID%>_1').checked)
		{
			alert('Para generar el primer conteo debe seleccionar tipo de conteo automatico!');
			return false;
		}
	
		if(document.getElementById('<%=rblReferencias.ClientID%>_1').checked && document.getElementById('<%=ddlReferencias.ClientID%>').value == '')
		{
			alert('Debe seleccionar una referencia.');
			return false;
		}
		
		return true;
	}
	
	function ContainerValidacionGeneracion()
	{
		if(ValidacionGeneracion())
			return confirm('Desea generar los registros de ingreso para el conteo '+document.getElementById('<%=ddlConteosRelacionados.ClientID%>').value+' ?');
		else 
			return false;
	}
	
	function CambioConteo()
	{
		var ddlConteos = document.getElementById('<%=ddlConteosRelacionados.ClientID%>');
		var ddlInvFisi = document.getElementById('<%=ddlInventarios.ClientID%>');
		var ddlReferen = document.getElementById('<%=ddlReferencias.ClientID%>');
		var ddlTarjetas = document.getElementById('<%=ddlTarjetas.ClientID%>');
		var dsConsulta = AMS_Inventarios_CapturaDatos.CargarItemsRelacionadosConteo(parseInt(ddlConteos.value),(ddlInvFisi.value.split('-'))[0],parseInt((ddlInvFisi.value.split('-'))[1])).value;
		
		ddlReferen.options.length = 0;
		ddlTarjetas.options.length = 0;
		
		if(dsConsulta.Tables[0].Rows.length > 0)
		{
			var strSearchValues = '';
			
			for (var i = 0; i < dsConsulta.Tables[0].Rows.length; ++i)
			{
				ddlReferen.options[ddlReferen.options.length] = new Option(dsConsulta.Tables[0].Rows[i].REFERENCIA_EDITADA+' - '+dsConsulta.Tables[0].Rows[i].DINV_MITE_NOMBRE,dsConsulta.Tables[0].Rows[i].DINV_MITE_CODIGO+'$'+dsConsulta.Tables[0].Rows[i].DINV_TARJETA);
				
				if(strSearchValues.indexOf(dsConsulta.Tables[0].Rows[i].DINV_TARJETA) == -1)
				{
					ddlTarjetas.options[ddlTarjetas.options.length] = new Option(dsConsulta.Tables[0].Rows[i].DINV_TARJETA,dsConsulta.Tables[0].Rows[i].DINV_TARJETA);
					strSearchValues = strSearchValues + String(dsConsulta.Tables[0].Rows[i].DINV_TARJETA);
				}
			}
			
			CambioReferencia();
		}
	}
	
	function CambioTipoGeneracion()
	{
		if(document.getElementById('<%=rblReferencias.ClientID%>_0').checked || document.getElementById('<%=rblReferencias.ClientID%>_1').checked)
			document.getElementById('<%=ddlTarjetas.ClientID%>').disabled = true;
		else if(document.getElementById('<%=rblReferencias.ClientID%>_2').checked)
			document.getElementById('<%=ddlTarjetas.ClientID%>').disabled = false;
	}
	
	function ValidarInfoGrilla()
	{
		var salida = '';
		
		if(ElementosValidar == null)
			return false;
		
		var regexDecimal = /^(?:\+|-)?\d+(?:\.\d+)?$/;
		
		for(var i=0;i<ElementosValidar.length;i++)
		{
			if(!regexDecimal.test(document.getElementById(ElementosValidar[i].split('$')[0]).value))
				salida += '- Por favor ingrese un valor númerico de cantidad para la tarjeta '+document.getElementById(ElementosValidar[i].split('$')[1]).value+'\n';
		}
		
		if(salida == '')
		{
			return true;
		}
		else
		{
			alert(salida)
			return false;
		}
	}
</script>
<fieldset><legend>Inventario Físico</legend>
	<table class="tablewhite" cellSpacing="1" cellPadding="1" >
		<tr>
			<td width="30%">Seleccione el Inventario :</td>
			<td align="right"><asp:dropdownlist id="ddlInventarios" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td colSpan="2"><asp:button id="btnAceptar" runat="server" CausesValidation="False" Text="Cargar Información" onclick="btnAceptar_Click"></asp:button>&nbsp;
				<asp:button id="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" onclick="btnCancelar_Click"></asp:button></td>
		</tr>
	</table>
</fieldset>
<asp:panel id="pnlInfoProceso" Visible="False" Runat="server">
	<FIELDSET style="WIDTH: 100%"><LEGEND>Información Proceso de Inventario</LEGEND>
		<TABLE class="tablewhite" cellSpacing="1" cellPadding="1" width="100%" border="0">
			<TR>
				<TD width="30%">Número de Items :</TD>
				<TD align="right">
					<asp:label id="lbNumItems" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>Conteo Actual :</TD>
				<TD style="HEIGHT: 23px" align="right">
					<asp:label id="lbNumConteo" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>Conteos Relacionados :</TD>
				<TD align="right">
					<asp:dropdownlist id="ddlConteosRelacionados" runat="server" onchange="CambioConteo();"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD>Referencias Relacionadas :</TD>
				<TD align="right">
					<asp:dropdownlist id="ddlReferencias" runat="server" onchange="CambioReferencia();"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD>Último conteo realizado a
					<asp:label id="lbRefUltCont" runat="server"></asp:label></TD>
				<TD style="HEIGHT: 19px" align="right">
					<asp:label id="lbUltimoConteo" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>Cantidad Conteo 1 de
					<asp:label id="lbRefCont1" runat="server"></asp:label></TD>
				<TD align="right">
					<asp:label id="lbConteo1" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD>Cantidad Conteo 2 de
					<asp:Label id="lbRefCont2" runat="server"></asp:Label></TD>
				<TD align="right">
					<asp:Label id="lbConteo2" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Cantidad Conteo 3 de
					<asp:Label id="lbRefCont3" runat="server"></asp:Label></TD>
				<TD align="right">
					<asp:Label id="lbConteo3" runat="server"></asp:Label></TD>
			</TR>
		</TABLE>
	</FIELDSET>
	<FIELDSET style="WIDTH: 100%"><LEGEND>Información Proceso de Inventario</LEGEND>
		<TABLE class="tablewhite" cellSpacing="1" cellPadding="1" width="100%" border="0">
			<TR>
				<TD vAlign="top" width="30%">Tipo de Conteo a Realizar</TD>
				<TD vAlign="top" align="right">
					<asp:RadioButtonList id="rblTipoConteo" runat="server" BorderStyle="None" BackColor="Transparent">
						<asp:ListItem Value="A" Selected="True">Automática</asp:ListItem>
						<asp:ListItem Value="SA">Semi-automática</asp:ListItem>
					</asp:RadioButtonList>
					<P>&nbsp;</P>
				</TD>
			</TR>
			<TR>
				<TD vAlign="top" width="30%">Referencias a Cargar :</TD>
				<TD vAlign="top" align="right">
					<asp:RadioButtonList id="rblReferencias" onclick="CambioTipoGeneracion();" runat="server" BorderStyle="None"
						BackColor="Transparent">
						<asp:ListItem Value="GT" Selected="True">Cargar Todos los Items</asp:ListItem>
						<asp:ListItem Value="GI">Cargar Ítem Actual</asp:ListItem>
						<asp:ListItem Value="GJ">Cargar por Tarjeta</asp:ListItem>
					</asp:RadioButtonList>
					<P>&nbsp;</P>
				</TD>
			</TR>
			<TR>
				<TD vAlign="top" width="30%">Si desea realizar el conteo especificamente de una 
					tarjeta por favor seleccionela</TD>
				<TD vAlign="top" align="right">
					<asp:DropDownList id="ddlTarjetas" runat="server" Enabled="False"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD colSpan="2">
					<P>&nbsp;</P>
					<P>
						<asp:Button id="btnGenerar" runat="server" Text="Cargar" onclick="btnGenerar_Click"></asp:Button></P>
				</TD>
			</TR>
		</TABLE>
	</FIELDSET>
	<asp:DataGrid id="dgItemsConteo" runat="server" BorderStyle="None" HeaderStyle-BackColor="#ccccdd"
		CellPadding="3"  GridLines="Vertical" BorderWidth="1px" AutoGenerateColumns="false">
		<SelectedItemStyle Font-Bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<HeaderStyle Font-Bold="True" cssclass="header"></HeaderStyle>
		<FooterStyle cssclass="footer"></FooterStyle>
		<Columns>
			<asp:BoundColumn DataField="dinv_tarjeta" HeaderText="Tarjeta N&#186;"></asp:BoundColumn>
			<asp:BoundColumn DataField="referencia_editada" HeaderText="C&#243;digo de Referencia"></asp:BoundColumn>
			<asp:BoundColumn DataField="dinv_mite_nombre" HeaderText="Nombre de Referencia"></asp:BoundColumn>
			<asp:BoundColumn DataField="dinv_pubi_codigo" HeaderText="C&#243;digo de Ubicaci&#243;n"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Ubicaci&#243;n">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "dinv_stand") %>
					-
					<%# DataBinder.Eval(Container.DataItem, "dinv_ubicacion") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Cantidad">
				<ItemTemplate>
					<asp:TextBox ID="tbCantidad" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" runat="server"></asp:TextBox>
					<input id="hdCodigoOriginal" runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "dinv_mite_codigo") %>' />
					<input id="hdNumeroTarjeta"  runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "dinv_tarjeta") %>'/>
					<input id="hdConteoRelacionado"  runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "dinv_conteoactual") %>'/>
					<input id="hdCodigoModificado"  runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "referencia_editada") %>'/>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
		<PagerStyle HorizontalAlign="Center" cssclass="pager" Mode="NumericPages"></PagerStyle>
	</asp:DataGrid>
	<P>
		<asp:Button id="btnGrabar" runat="server" Text="Grabar" Visible="False" onclick="btnGrabar_Click"></asp:Button></P>
</asp:panel>
