<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.CruceDocs.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.CruceDocumentos" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<p>
	<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
	<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
	
	<script type ='text/javascript'>
	function CambioPrefijo(obj)
	{
		CruceDocumentos.CargarNumeroCruce(obj.value,CargarNumeroCruce_CallBack);
	}
	
	function CargarNumeroCruce_CallBack(response)
	{
		if(response.error!=null)
		{
			alert(response.error);
			return;
		}
		var lbnum=document.getElementById("<%=lbNumero.ClientID%>");
		var hdnNum=document.getElementById("<%=hdnnum.ClientID%>");
		var respuesta=response.value;
		lbnum.innerHTML=respuesta;
		hdnNum.value=respuesta;
	}
	
	function Cambio_Indice(obj)
	{
		var ddlpref=document.getElementById("<%=ddlPrefFac.ClientID%>");
		var cli=document.getElementById("<%=datCli.ClientID%>");
		CruceDocumentos.Cargar_Facturas_Externas(obj.value,ddlpref.value,cli.value,Cargar_Facturas_Externas_CallBack);
	}
	
	function Cargar_Facturas_Externas_CallBack(response)
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
		var cli=document.getElementById("<%=datCli.ClientID%>");
		var radio;
		if(radio1.checked==true)
			radio=radio1;
		else if(radio2.checked==true)
			radio=radio2;
		CruceDocumentos.Cargar_Numeros(radio.value,obj.value,cli.value,Cargar_Numeros_CallBack)
	}
	
	function Cargar_Numeros_CallBack(response)
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
	
	function Retornar_Nombre_CallBack(response)
	{
		if (response.error != null)
		{
			alert(response.error);
			return;
		}
		var nit=document.getElementById("<%=datCli.ClientID%>");
		var desc=document.getElementById("<%=datClia.ClientID%>");
		var hdnNom=document.getElementById("<%=hdnnom.ClientID%>");
		var resultado=response.value;
		if(resultado=="Error")
		{
			alert('Nit Inexistente');
			nit.value="";
			desc.value="";
		}
		else
		{
			desc.value=resultado;
			hdnNom.value=resultado;
		}
	}
	
	function Cargar_Datos(val)
	{
		if(val.value=="")
			return;
		else
			CruceDocumentos.Retornar_Nombre(val.value,Retornar_Nombre_CallBack);
	}
	
</script>
<fieldset>
</p>
<P>Información del Cruce
</P>
<P>Aquí usted podra realizar cualquier cruce de documentos Cliente - Proveedor, 
	escoja como desea realizarlo, recuerde que al final el total faltante del cruce 
	tiene que ser igual a cero (0):</P>
<table id="table" class="filtersIn"
	<tr>
		<td>Tipo de Cruce :
		</td>
		<td><asp:radiobutton id="rbCliente" Runat="server" GroupName="tipoCruce"
                Text="Cliente (Lógica de Recibo de Caja)" AutoPostBack="true"
                oncheckedchanged="rbCliente_CheckedChanged"></asp:radiobutton><br />
            <asp:radiobutton id="rbProveedor" Runat="server" GroupName="tipoCruce" 
                Text="Proveedor (Lógica de Comprobante de Egreso)" AutoPostBack="true"
                oncheckedchanged="rbProveedor_CheckedChanged" ></asp:radiobutton></td>
	</tr>
	<tr>
		<td>Prefijo del Documento de Cruce :
		</td>
		<td><asp:dropdownlist id="ddlPrefijo" runat="server" onChange="CambioPrefijo(this)"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td>Fecha :
		</td>
		<td><asp:TextBox id="txtFecha" onkeyup="DateMask(this)" runat="server" Width="92px"></asp:TextBox></td>
	</tr>
	<tr>
		<td>Número de Cruce :
		</td>
		<td><asp:label id="lbNumero" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td>Nit del Cliente o Proveedor :
		</td>
		<td><asp:textbox id="datCli" onblur="Cargar_Datos(this)" runat="server" Width="136px" ToolTip="Haga doble click para iniciar la busqueda"></asp:textbox>
		<asp:requiredfieldvalidator id="validatorNit" runat="server" ControlToValidate="datCli" ErrorMessage="RequiredFieldValidator">*</asp:requiredfieldvalidator>&nbsp;Nombre 
			:
			<asp:textbox id="datClia" Runat="server" Width="254px" ReadOnly="True"></asp:textbox></td>
	</tr>
	<tr>
		<td align="center" colspan="2"><asp:button id="btnCargar" Runat="server" Text="Cargar Documentos" onclick="btnCargar_Click"></asp:button></td>
	</tr>
</table>
<p></p>
<asp:panel id="pnlInfo" Runat="server" Width="650" Visible="False">
<P>Lista de Documentos Con Saldo
	</P>
<TABLE id="table3" class="filtersIn">
		<TR>
			<TD>Si conoce el prefijo y número del documento seleccionelos de las 
				listas para facilitar la busqueda
		<br />
        Prefijo :
			<br />
				<asp:dropdownlist id="ddlPrefBus" class="dmediano" Runat="server" AutoPostBack="True" onselectedindexchanged="ddlPrefBus_SelectedIndexChanged"></asp:dropdownlist>
            <br />
            Número :
			<br />
				<asp:dropdownlist id="ddlNumBus" class="dpequeno" Runat="server"></asp:dropdownlist>
                </TD>
                </tr>
                <tr>
			<TD>
				<asp:button id="btnBus" Text="Buscar" Runat="server" onclick="btnBus_Click"></asp:button></TD>
		</TR>
	</TABLE>
<asp:datagrid id="gridDocumentos" runat="server" cssclass="datagrid" AllowPaging="True" ShowFooter="True"
		AutoGenerateColumns="False" CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
		<Columns>
			<asp:BoundColumn DataField="TIPO" HeaderText="Tipo" ></asp:BoundColumn>
			<asp:BoundColumn DataField="NIT" HeaderText="Nit"></asp:BoundColumn>
			<asp:BoundColumn DataField="PREFIJO" HeaderText="Prefijo Factura"></asp:BoundColumn>
			<asp:BoundColumn DataField="NUMERODOCUMENTO" HeaderText="N&#250;mero Factura" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALORDOCUMENTO" HeaderText="Valor Total Factura" DataFormatString="{0:C}"
				ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
			<asp:BoundColumn DataField="VALORABONADO" HeaderText="Saldo a Pagar" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Valor a Abonar">
				<ItemTemplate>
					<asp:TextBox id="valpagtxt" runat="server" Width="100" ReadOnly="false" onkeyup="NumericMaskE(this,event)" Text='<%#DataBinder.Eval(Container.DataItem,"VALORABONADO","{0:N}")%>' CssClass="AlineacionDerecha">
					</asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
		    <asp:BoundColumn DataField="FACTURA" HeaderText="Factura"></asp:BoundColumn>
            <asp:BoundColumn DataField="DESCRIPCION" HeaderText="Descripción_Factura"></asp:BoundColumn>
		    <asp:TemplateColumn HeaderText="Adicionar Documento" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Button CommandName="adicionarFilas" Text=">>" ID="btnAdd" runat="server" Width="65" />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Button id="addAll" runat="server" Text="Agregar Todos" CommandName="addall" Width="100" />
				</FooterTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid>
    <br/>
<asp:button id="bntMostrarLista" Runat="server" Text="Mostrar Lista Completa" onclick="click_MostrarLista"></asp:button>
<P>Documentos a Abonar o Pagar
	</P>
<asp:datagrid id="gridPagar" runat="server" cssclass="datagrid" ShowFooter="true" AutoGenerateColumns="False"
		CellPadding="3">
		<FooterStyle cssclass="footer"></FooterStyle>
		<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		<ItemStyle cssclass="item"></ItemStyle>
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
<P></P>
<P>Si desea cruzar facturas o notas de un nit o persona distinta. Escoja el 
prefijo y el número de la factura a cruzar 
<P>
		<asp:RadioButton id="rb1" onclick="Cambio_Indice(this);" runat="server" Text="Facturas y Notas de Cliente"
			GroupName="grupoFacturas" Checked="True"></asp:RadioButton>&nbsp;&nbsp;&nbsp;
		<asp:RadioButton id="rb2" onclick="Cambio_Indice(this);" runat="server" Text="Facturas y Notas de Proveedor"
			GroupName="grupoFacturas"></asp:RadioButton></P>
<P>
		<TABLE id="table1" class="filtersIn">
			<TR>
				<TD>Escoja el prefijo de la factura :
				</TD>
				<TD>
					<asp:DropDownList id="ddlPrefFac" runat="server" onChange="Cambio_Prefijo(this);"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>Escoja el número de la factura :
				</TD>
				<TD>
					<asp:DropDownList id="ddlNumFac" runat="server"></asp:DropDownList></TD>
			</TR>
			<TR>
				<TD>
					<asp:Button id="aceptarFactura" onclick="aceptarFactura_Click" runat="server" Text="Cargar"></asp:Button></TD>
			</TR>
		</TABLE>
	</P>
<P>
		<asp:DataGrid id="gridDocDis" runat="server" cssclass="datagrid" AutoGenerateColumns="False" CellPadding="3">
			<FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
			<HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			<PagerStyle cssclass="pager" mode="NumericPages"></PagerStyle>
			<SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
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
						<asp:TextBox id="Textbox1" runat="server" Width="100" ReadOnly="false" onkeyup="NumericMaskE(this,event)" Text='<%#DataBinder.Eval(Container.DataItem,"VALORABONADO","{0:N}")%>' CssClass="AlineacionDerecha">
						</asp:TextBox>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Agregar Documento" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<asp:Button CommandName="agregar" Text=">>" ID="Button1" runat="server" Width="65" />
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid></P>
<TABLE>
		<TR>
			<TD>Total Clientes :
			</TD>
			<TD>
				<asp:textbox id="totalCli" runat="server" ReadOnly="true"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Total Proveedores :
			</TD>
			<TD>
				<asp:textbox id="totalPro" runat="server" ReadOnly="true"></asp:textbox></TD>
		</TR>
		<TR>
			<TD>Total Faltante Cruce :
			</TD>
			<TD>
				<asp:textbox id="totalCruce" runat="server" ReadOnly="True"></asp:textbox></TD>
		</TR>
		<TR>
			<TD align="center">
				<asp:Button id="btnGuardar" runat="server" Text="Ejecutar Cruce" onclick="btnGuardar_Click"></asp:Button></TD>
		</TR>
	</TABLE></asp:panel>
<P></P>
<asp:button id="btnCancelar" Runat="server" Text="Cancelar Proceso" CausesValidation="False" onclick="btnCancelar_Click"></asp:button>
<P><asp:label id="lb" runat="server"></asp:label></P>
<input type="hidden" runat="server" id="hdnnum"/><input type="hidden" runat="server" id="hdnnom"/>
</fieldset>

