<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tesoreria.EmisionRecibos.Obligaciones.ascx.cs" Inherits="AMS.Finanzas.AMS_Tesoreria_EmisionRecibos_Obligaciones" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<asp:datagrid id="dgrObligaciones" AutoGenerateColumns="False" ShowFooter="True" OnCancelCommand="dgrObligaciones_Cancel"
	OnUpdateCommand="dgrObligaciones_Update" OnEditCommand="dgrObligaciones_Edit" runat="server" cssclass="datagrid"
	onItemCommand="dgrObligaciones_Item">
	<FooterStyle cssclass="footer"></FooterStyle>
	<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
	<PagerStyle horizontalalign="Center" cssclass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
	<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	<ItemStyle cssclass=""></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Número">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_NUMERO") %>
			</ItemTemplate>
			<EditItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_NUMERO") %>
			</EditItemTemplate>
			<FooterTemplate>
				<asp:TextBox id="txtAddNumOblig" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"
					Columns="10" onclick="ModalDialog(this,'SELECT m.MOBL_NUMERO NUMERO, m.PCUE_CODIGO concat \'  \' concat pcue_numero as CUENTA, m.MOBL_FECHA as FECHA, varchar_format(m.MOBL_MONTPESOS,\'999,999,999,999.99\') as monto, varchar_format(m.MOBL_MONTPESOS-MOBL_MONTPAGADO,\'999,999,999,999.99\') as SALDO, (select count (*) from DOBLIGACIONFINANCIERAPLANPAGO d where m.MOBL_NUMERO = d.MOBL_NUMERO and d.mobl_montpesos > d.dobl_montpago) as Cuotas_Pendientes FROM MOBLIGACIONFINANCIERA m, pcuentacorriente pc WHERE (MOBL_MONTPESOS-MOBL_MONTPAGADO)>0 and m.PCUE_CODIGO = pc.PCUE_CODIGO ORDER BY MOBL_FECHA, MOBL_NUMERO;',new Array())" />
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Pago">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "DOBL_NUMEPAGO") %>
			</ItemTemplate>
			<EditItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "DOBL_NUMEPAGO") %>
			</EditItemTemplate>
			<FooterTemplate>
				<asp:TextBox id="txtAddPagoOblig" runat="server" ReadOnly="True" CssClass="AlineacionDerecha"
					Columns="10" onclick="MostrarPagos(this);" />
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Banco">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "PBAN_NOMBRE") %>
			</ItemTemplate>
			<EditItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "PBAN_NOMBRE") %>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Saldo<BR>Capital">
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_SALDO", "{0:C}") %>
			</ItemTemplate>
			<EditItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_SALDO", "{0:C}") %>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Fecha<br>Vence">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_FECHA", "{0:yyyy-MM-dd}") %>
			</ItemTemplate>
			<EditItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_FECHA", "{0:yyyy-MM-dd}") %>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Cuota<BR>Capital">
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTPESOS", "{0:C}") %>
			</ItemTemplate>
			<EditItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTPESOS", "{0:C}") %>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Interés">
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTINTERESES", "{0:C}") %>
			</ItemTemplate>
			<EditItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTINTERESES", "{0:C}") %>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Abono<BR>Cuota Capital">
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTPESOSED", "{0:C}") %>
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox id="txtEdCuota" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTPESOS", "{0:#,##0.##}") %>' />
			</EditItemTemplate>
			<FooterTemplate>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Abono<BR>Interés">
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTINTERESESED", "{0:C}") %>
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox id="txtEdInteres" runat="server" Enabled="true" onkeyup="NumericMaskE(this,event)" CssClass="AlineacionDerecha" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "MOBL_MONTINTERESES", "{0:#,##0.##}") %>' />
			</EditItemTemplate>
			<FooterTemplate>
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Agregar" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<asp:Button CommandName="Remover" Text="Remover" ID="btnRem" runat="server" Width="65" />
			</ItemTemplate>
			<FooterTemplate>
				<asp:Button CommandName="Agregar" Text="Agregar" ID="btnAdd" runat="server" Width="60" />
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Actualizar" CancelText="Cancelar" EditText="Actualizar"></asp:EditCommandColumn>
	</Columns>
</asp:datagrid><BR>
Total:&nbsp;&nbsp;&nbsp;<asp:TextBox id="txtTotalO" runat="server" ReadOnly="True" CssClass="AlineacionDerecha" Width="100px" />
<script type ='text/javascript'>
	function MostrarPagos(objP){
		var objNum=document.getElementById('<%=ViewState["TXT_OBLIGACION"].ToString()%>');
		var num=objNum.value;
		if(num.length==0){
			alert('Debe seleccionar el número de obligación primero')
			return;
		}

ModalDialog(objP, "SELECT DOBL_NUMEPAGO AS NUMERO, MOBL_FECHA AS FECHA, MOBL_MONTPESOS AS MONTO, MOBL_MONTINTERES AS INTERESES, dobl_montpago as monto_pagado, dobl_intepago as interes_pagado FROM DOBLIGACIONFINANCIERAPLANPAGO WHERE MOBL_NUMERO='" + num + "' AND MOBL_MONTPESOS>DOBL_MONTPAGO ORDER BY MOBL_FECHA, MOBL_NUMERO;", new Array());
	}
</script>
<%=ViewState["TXT_PAGO"]%>