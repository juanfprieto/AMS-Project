<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.EmisionRecibos.Documentos.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.Documentos" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<p>Lista de Documentos Con Saldo
</p>
<table id="Table" class="filtersIn">
	<tr>
		<td>Si conoce el prefijo y número del documento seleccionelos de las 
			listas para facilitar la busqueda
		<br />
        prefijo :
        <asp:dropdownlist id="ddlPrefBus" AutoPostBack="True" class="dmediano" Runat="server" onselectedindexchanged="ddlPrefBus_SelectedIndexChanged"></asp:dropdownlist>
      
        Número :
		<asp:dropdownlist id="ddlNumBus" class="dpequeno" Runat="server"></asp:dropdownlist>
        
        
        <asp:button id="btnBus" Runat="server" Text="Buscar" onclick="btnBus_Click"></asp:button>
        </td>
	</tr>
</table>
<P></P>
<asp:datagrid id="gridDocumentos" OnItemDataBound="gridDocumentos_ItemDataBound" runat="server"
	onPageIndexChanged="gridDocumentos_PageIndexChange" AllowPaging="True" ShowFooter="True" AutoGenerateColumns="False"
	onItemCommand="gridDocumentos_Item" cssclass="datagrid">
	<FooterStyle CssClass="footer"></FooterStyle>
	<HeaderStyle CssClass="header"></HeaderStyle>
	<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
	<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
	<ItemStyle CssClass="item"></ItemStyle>
	<Columns>
		<asp:BoundColumn DataField="TIPO" HeaderText="Tipo"></asp:BoundColumn>
		<asp:BoundColumn DataField="NIT" HeaderText="Nit"></asp:BoundColumn>
		<asp:BoundColumn DataField="PREFIJO" HeaderText="Prefijo Factura"></asp:BoundColumn>
		<asp:BoundColumn DataField="NUMERODOCUMENTO" HeaderText="N&#250;mero Factura" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
		<asp:BoundColumn DataField="VALORDOCUMENTO" HeaderText="Valor Total Factura" DataFormatString="{0:C}"
			ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
		<asp:BoundColumn DataField="VALORABONADO" HeaderText="Saldo a Pagar" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Valor a Abonar">
			<ItemTemplate>
				<asp:TextBox id="valpagtxt" runat="server" style="width: 105px;" ReadOnly="false" onkeyup="NumericMaskE(this,event)" Text='<%#DataBinder.Eval(Container.DataItem,"VALORABONADO","{0:N}")%>' CssClass="AlineacionDerecha">
				</asp:TextBox>
			</ItemTemplate>
		</asp:TemplateColumn>
        <asp:BoundColumn DataField="FACTURA" HeaderText="Factura"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="Adicionar Documento" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<asp:Button CommandName="adicionarFilas" Text=">>" ID="btnAdd" runat="server" class="bpequeno"/>
			</ItemTemplate>
			<FooterTemplate>
				<asp:Button id="addAll" runat="server" Text="Agregar Todos" CommandName="addall" class="bpequeno"/>
			</FooterTemplate>
		</asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Selección Multiple" Visible="false">
			<ItemTemplate>
                <asp:CheckBox ID="cbRows" runat="server"/>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Button ID="btnAgregarChk" runat="server" Text="Agregar Selección" CommandName="addChecked" class="bpequeno"/>
            </FooterTemplate>
        </asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<br/>
<asp:button id="bntMostrarLista" Runat="server" Text="Mostrar Lista Proveedor" onclick="click_MostrarLista"></asp:button>
<asp:button id="bntMostrarListaAll" Runat="server" Text="Mostrar Lista General" onclick="click_MostrarListaAll"></asp:button>
<p>Documentos a Abonar o Pagar
</p>
<asp:datagrid id="gridPagar" OnItemDataBound="gridPagar_ItemDataBound" runat="server" ShowFooter="true"
	AutoGenerateColumns="False" onItemCommand="gridPagar_Item" class="datagrid">
	<FooterStyle CssClass="footer"></FooterStyle>
	<HeaderStyle CssClass="header"></HeaderStyle>
	<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
	<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
	<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
	<ItemStyle CssClass="item"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="Remover Documento" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<asp:Button CommandName="removerFilas" Text="<<" ID="btnRem" runat="server" Width="55" />
			</ItemTemplate>
			<FooterTemplate>
				<asp:Button id="remAll" Text="Remover Todos" CommandName="remAll" Width="110" runat="server" />
			</FooterTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn HeaderText="Tipo" DataField="TIPO" />
		<asp:BoundColumn HeaderText="Nit" DataField="NIT" />
		<asp:BoundColumn HeaderText="Prefijo Factura" DataField="PREFIJO" />
		<asp:BoundColumn Headertext="Número Factura" DataField="NUMERODOCUMENTO" ItemStyle-HorizontalAlign="Right" />
		<asp:BoundColumn HeaderText="Saldo a Pagar" DataField="VALORABONADO" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
		<asp:BoundColumn HeaderText="Valor Abonado" DataField="VALORABONAR" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
	</Columns>
</asp:datagrid>
<p></p>
<p><asp:panel id="pnlFacDis" runat="server" Visible="False">Si desea cruzar facturas o notas de un nit o 
persona distinta. Escoja el prefijo y el número de la factura a cruzar
<P>
			<asp:RadioButton id="rb1" onclick="Cambio_Indice(this);" Text="Facturas y Notas de Cliente" runat="server"
				Checked="True" GroupName="grupoFacturas"></asp:RadioButton>&nbsp;&nbsp;&nbsp;
			<asp:RadioButton id="rb2" onclick="Cambio_Indice(this);" Text="Facturas y Notas de Proveedor" runat="server"
				GroupName="grupoFacturas"></asp:RadioButton></P>
<P>
			<TABLE style="BACKGROUND-COLOR: transparent">
				<TR>
					<TD>Escoja el prefijo de la factura :
						<asp:DropDownList id="ddlPrefFac" runat="server" onChange="Cambio_Prefijo(this);"></asp:DropDownList></TD>
				
					<TD>Escoja el número de la factura :
						<asp:DropDownList id="ddlNumFac" runat="server"></asp:DropDownList></TD>
                        
				</TR>
				<TR>
					<TD align="center">
						<asp:Button id="aceptarFactura" onclick="aceptarFactura_Click" Text="Cargar" runat="server"></asp:Button></TD>
				</TR>
			</TABLE>
		</P>
<P>
			<asp:DataGrid id="gridDocDis" onItemCommand="gridDocDis_Item" cssclass="datagrid" AutoGenerateColumns="False"
				runat="server">
				<FooterStyle CssClass="footer"></FooterStyle>
				<HeaderStyle CssClass="header"></HeaderStyle>
				<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
				<ItemStyle CssClass="item"></ItemStyle>
				<Columns>
					<asp:BoundColumn DataField="TIPO" HeaderText="Tipo"></asp:BoundColumn>
					<asp:BoundColumn DataField="NIT" HeaderText="Nit"></asp:BoundColumn>
					<asp:BoundColumn DataField="PREFIJO" HeaderText="Prefijo Factura"></asp:BoundColumn>
					<asp:BoundColumn DataField="NUMERODOCUMENTO" HeaderText="N&#250;mero Factura" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
					<asp:BoundColumn DataField="VALORDOCUMENTO" HeaderText="Valor Total Factura" DataFormatString="{0:C}"
						ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
					<asp:BoundColumn DataField="VALORABONADO" HeaderText="Saldo a Pagar" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Valor a Abonar">
						<ItemTemplate>
							<asp:TextBox id="Textbox1" runat="server" class="tmediano" ReadOnly="false" onkeyup="NumericMaskE(this,event)" Text='<%#DataBinder.Eval(Container.DataItem,"VALORABONADO","{0:N}")%>' CssClass="AlineacionDerecha">
							</asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Agregar Documento" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<asp:Button CommandName="agregar" Text=">>" ID="Button1" runat="server" class="bpequeno"/>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid></P></asp:panel>
<P></P>
<table>
	<tbody>
		<tr>
			<td>Total Clientes :
			</td>
			<td><asp:textbox id="totalCli" runat="server" ReadOnly="true"></asp:textbox></td>
		</tr>
		<tr>
			<td>Total Proveedores :
			</td>
			<td><asp:textbox id="totalPro" runat="server" ReadOnly="true"></asp:textbox></td>
		</tr>
		<tr>
			<td>Total Faltante Cruce :
			</td>
			<td><asp:textbox id="totalCruce" runat="server" ReadOnly="True"></asp:textbox></td>
		</tr>
		<tr>
			<td align="center"><asp:button id="aceptar" onclick="aceptar_Click" Text="Aceptar" runat="server" ToolTip="Aceptar y Continuar"></asp:button></td>
		</tr>
	</tbody>
</table>
<INPUT id="hdnCli" type="hidden" name="hdnCli" runat="server"><INPUT id="hdnBen" type="hidden" name="hdnBen" runat="server">
<script type ='text/javascript'>
	function Cambio_Indice(obj)
	{
		var ddlpref=document.getElementById("<%=ddlPrefFac.ClientID%>");
		var cli=document.getElementById("<%=hdnCli.ClientID%>");
		var ben=document.getElementById("<%=hdnBen.ClientID%>");
		Documentos.Cargar_Facturas_Externas(obj.value,ddlpref.value,cli.value,ben.value,Cargar_Facturas_Externas_RollBack);
	}
	
	function Cargar_Facturas_Externas_RollBack(response)
	{
		if (response.error != null)
		{
			alert(response.error);
			return;
		}
		var ddlpref=document.getElementById("<%=ddlPrefFac.ClientID%>");
		var ddlnum=document.getElementById("<%=ddlNumFac.ClientID%>");
		var respuesta=response.value;
		ddlpref.options.length=0;
		ddlnum.options.length=0;
		if(respuesta.Tables[0].Rows.length>0)
		{
			for(var i=0;i<respuesta.Tables[0].Rows.length;i++)
			{
				ddlpref.options[ddlpref.options.length] = new Option(respuesta.Tables[0].Rows[i].DESCRIPCION,respuesta.Tables[0].Rows[i].PREFIJO);
			}
		}
		else
		{
			alert('No hay prefijos disponibles');
			return;
		}
		if(respuesta.Tables[1].Rows.length>0)
		{
			for(var i=0;i<respuesta.Tables[1].Rows.length;i++)
			{
				ddlnum.options[ddlnum.options.length] = new Option(respuesta.Tables[1].Rows[i].NUMERO,respuesta.Tables[1].Rows[i].NUMERO);
			}
		}
		else
		{
			alert('No hay facturas disponibles');
			return;
		}
	}
		
	function Cambio_Prefijo(obj)
	{
		var radio1=document.getElementById("<%=rb1.ClientID%>");
		var radio2=document.getElementById("<%=rb2.ClientID%>");
		var cli=document.getElementById("<%=hdnCli.ClientID%>");
		var ben=document.getElementById("<%=hdnBen.ClientID%>");
		var radio;
		if(radio1.checked==true)
			radio=radio1;
		else if(radio2.checked==true)
			radio=radio2;
		Documentos.Cargar_Numeros(radio.value,obj.value,cli.value,ben.value,Cargar_Numeros_RollBack)
	}
	
	function Cargar_Numeros_RollBack(response)
	{
		if (response.error != null)
		{
			alert(response.error);
			return;
		}
		var ddlnum=document.getElementById("<%=ddlNumFac.ClientID%>");
		var respuesta=response.value;
		if(respuesta.Tables[0].Rows.length>0)
		{
			ddlnum.options.length=0;
			for(var i=0;i<respuesta.Tables[0].Rows.length;i++)
			{
				ddlnum.options[ddlnum.options.length] = new Option(respuesta.Tables[0].Rows[i].NUMERO,respuesta.Tables[0].Rows[i].NUMERO);
			}
		}
		else
		{
			ddlnum.options.length=0;
			alert('No hay facturas disponibles');
			return;
		}
	}
	
	function Mostrar_Div()
	{
		var objDiv=document.getElementById("divFacturas");
		if(objDiv.style.display=='none')
			objDiv.style.display='';
		else
			objDiv.style.display='none';
	}
	
</script>
