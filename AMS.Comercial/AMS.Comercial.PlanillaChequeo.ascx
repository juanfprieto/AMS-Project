<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.PlanillaChequeo.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_PlanillaChequeo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<style type="text/css">
    .style1
    {
        height: 69px;
    }
    .style2
    {
        width: 164px;
        height: 30px;
    }
    .style3
    {
        height: 30px;
    }
</style>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Tools.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<fieldset>
	<table class="filtersIn">
		<tr>
			<td><h3>Información de la planilla:</h3></td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label5" runat="server" Font-Bold="True">Número de Planilla Chequeo :</asp:label>
            </td>
			<td>
                <asp:textbox id="txtPlanilla" runat="server" CssClass="tpequeno" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>"></asp:textbox>
            </td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label4" runat="server" visible="true" Font-Bold="True">Agencia Chequeadora:</asp:label>
            </td>
			<td>
                <asp:dropdownlist id="ddlAgencia"  Visible="true" runat="server" CssClass="dmediano"></asp:dropdownlist>
            </td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label11" runat="server" Font-Bold="True">Inspector:</asp:label></td>
			<td colspan="2">
                <asp:textbox id="txtInspector" onclick="MostrarInspector(this);" runat="server" CssClass="tpequeno" ReadOnly="True"></asp:textbox>
				<asp:textbox id="txtInspectora" runat="server" Width="300px" ReadOnly="True"></asp:textbox>
            </td>
		</tr>
		<tr>
			<td vAlign="top">
                <asp:label id="Label1" runat="server" Font-Bold="True">Ruta Principal:</asp:label>
            </td>
			<td>
                <asp:dropdownlist id="ddlRuta" runat="server" CssClass="dmediano" AutoPostBack="True"></asp:dropdownlist>
            </td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label15" runat="server" visible="true" Font-Bold="True">Agencia Chequeada:</asp:label>
            </td>
			<td>
                <asp:dropdownlist id="ddlAgenciaChequeada"  Visible="true" runat="server" CssClass="dmediano"></asp:dropdownlist>
            </td>
		</tr>
	    <tr>
			<td>
                <asp:label id="Label2" runat="server" Font-Bold="True">Número del Bus:</asp:label>
            </td>
			<td>
                <asp:textbox id="txtNumeroBus" ondblclick="mostrarPlaca(this);"	runat="server" CssClass="tpequeno"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label7" runat="server" Font-Bold="True">Placa del Bus:</asp:label>
            </td>
			<td>
                <asp:textbox id="txtNumeroBusa" runat="server" CssClass="tpequeno" MaxLength="6" ReadOnly="True"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label6" runat="server" Font-Bold="True">Fecha Chequeo:</asp:label>
            </td>
			<td>
                <asp:textbox id="txtFecha" onkeyup="DateMask(this)" CssClass="tpequeno" Runat="server"></asp:textbox>
            </td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label3" runat="server" Font-Bold="True">Hora Chequeo:</asp:label>
            </td>
			<td>
                <asp:dropdownlist id="ddlHora" runat="server" Width="40px"></asp:dropdownlist>
			    <asp:dropdownlist id="ddlMinuto" runat="server" Width="48px"></asp:dropdownlist>
            </td>
		</tr>
		<tr>
			<td>
                <asp:label id="Label8" runat="server" Font-Bold="True">Conductor:</asp:label>
            </td>
			<td colspan="2">
                <asp:textbox id="txtConductor"  runat="server" ondblclick="MostrarConductor(this);" CssClass="tpequeno" ReadOnly="False"></asp:textbox>
				<asp:textbox id="txtConductora" runat="server" Width="300px" ReadOnly="True"></asp:textbox>
            </td>
		</tr>
        <tr>
			<asp:label id="Label19" runat="server"  Font-Bold="True">Pasajeros:</asp:label>      
		</tr>
		<tr>
			<td>   
                <asp:label id="Label10" runat="server" Font-Bold="True">Sin Tiquete: </asp:label>
            </td>
            <td>
			    <asp:textbox id="txtNumSinTiquete" runat="server" CssClass="tpequeno"></asp:textbox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:label id="Label9" runat="server" Font-Bold="True">Con Tiquete:</asp:label>
            </td>
            <td>
                <asp:textbox id="txtNumTiquete" runat="server" CssClass="tpequeno"></asp:textbox>
	        </td>
		</tr>
        <tr>
            <td>
                <asp:label id="Label12" runat="server" Font-Bold="True">Total Pasajeros:</asp:label>
            </td>
            <td>
                <asp:textbox id="txtNumTotal" runat="server" CssClass="tpequeno"></asp:textbox>
	        </td>
		</tr>
		<tr>
            <td>
                <asp:label id="Label13" runat="server" Font-Bold="True">Valor total pasajes sin tiquete:</asp:label> 
            </td>
            <td>
                <asp:textbox id="txtTotalPre" onkeyup="NumericMask(this);" runat="server" CssClass="tpequeno"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td>
                <asp:Label ID="labelpago" runat="server" Font-Bold="true">Pago:</asp:Label>
            </td>
			<td>
                <asp:RadioButtonList ID="chkpago" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"> 
			    <asp:ListItem Value="S">Si</asp:ListItem>	
			    <asp:listItem Value="N">No</asp:listItem>
                </asp:RadioButtonList>
            </td>
        </tr>
		<tr>
			<td>
            <asp:label id="Label14" runat="server" Font-Bold="True">Lugar del Chequeo:</asp:label>
            </td>
			<td colspan="2">
                <asp:textbox id="txtLugar" runat="server" onclick="ModalDialog(this,'SELECT PCHE_CODIGO AS CODIGO,  PCHE_NOMBRE as Nombre from DBXSCHEMA.PCHEQUEOSITIO ORDER BY PCHE_NOMBRE;', new Array(),1);" 
			    MaxLength="20" ReadOnly="true" Columns="20" style="width:24%;"></asp:textbox>
			    <asp:textbox id="txtLugara" runat="server" Width="300px" ReadOnly="True"></asp:textbox>
			</td>
		</tr>
		<tr>
		    <td>
                <asp:Label ID="Label16" runat="server" Text="Es Transbordo ?"></asp:Label>             
            </td>
            <td >
                <asp:CheckBox ID="chkTransbordo" runat="server" width="30px" AutoPostBack="True" />
                <asp:Label ID="lblTransbordo" runat="server" Text="Abono a favor Bus " Visible="False"></asp:Label>
                <asp:TextBox ID="txtbusRecibe" runat="server" width="50px" ondblclick="mostrarPlaca(this);" ></asp:TextBox>                   
                <asp:Label ID="lblvalorTransbordo" runat="server" Text="Valor Transbordo "  Visible="False"></asp:Label>
                <asp:TextBox ID="txtvalorTransbordo" runat="server" CssClass="tpequeno"  onkeyup="NumericMask(this);"></asp:TextBox>
            </td>
		</tr>
		<tr>
		   <td class="style2">
                <asp:Label ID="Label18" runat="server" Text="Auxiliar con Seguridad Social ?" width="200px"></asp:Label> 
            </td>
            <td class="style3" >
                <asp:CheckBox ID="chkseguridadSocial" runat="server" width="155px" />
            </td>
       </tr>
		<tr>
			<td align="center" class="style1" colspan="2">
			    Observaciones:  
                <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Height="43px" Width="510px"></asp:TextBox>			        
			</td>            
		</tr>
		<tr>
            <td align="center">
				<asp:button id="btnGuardar" Font-Bold="True" Runat="server" Text="Guardar" Enabled="false"></asp:button>
            </td>
			<td>
				<asp:label id="lblError" runat="server" Font-Bold="True"></asp:label>
            </td>
		</tr>
	</table>


	
	</br>
	<asp:panel id="pnlDestinos" Runat="server" Visible="False">
		<table class="filtersIn">
			<tr>
				<td><h3>Destinos:</h3></td>
			</tr>
			<tr>
				<td align="center">
					<asp:datagrid id="dgrDocumentos" runat="server" AutoGenerateColumns="False" ShowFooter="True">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="Linea"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Destino">
								<ItemTemplate>
									<asp:textbox ReadOnly="True" id="txtRuta" onclick="VerSubrutas(this);" runat="server"
										CssClass="tpequeno"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cantidad">
								<ItemTemplate>
									<asp:textbox id="txtCantidad" Runat="server" Width="50px" MaxLength="3"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<asp:TextBox id="txtValor" runat="server"></asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Total">
								<ItemTemplate>
									<asp:TextBox id="txtTotal" runat="server" ReadOnly="True"></asp:TextBox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="txtSumaTotal" runat="server" ReadOnly="True"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid>
                </td>
			</tr>
		</table>
	</asp:panel>
</fieldset>
	
<script language="javascript">
	var tDocumentos='<%=ViewState["strTotal"]%>';
	var txtNumeroBus=document.getElementById("<%=txtNumeroBus.ClientID%>");
    var txtNumeroBusa=document.getElementById("<%=txtNumeroBusa.ClientID%>");
	
	function Total(tCantidad,tValor,tTotal){
		var tCantidad=document.getElementById(tCantidad);
		var tValor=document.getElementById(tValor);
		var tTotal=document.getElementById(tTotal);
		if(tCantidad.value.length>0 && tValor.value.length>0){
			try{
				tTotal.value=(parseInt(tCantidad.value)*parseFloat(tValor.value.replace(/\,/g,'')));
				tTotal.value=formatoValor(tTotal.value);
			}
			catch(err){
				tTotal.value="";
			}
		}
		totalesDocs();
	}
	function totalesDocs(){
		arTiqs=tDocumentos.split(',');
		totalT=0;
		if(arTiqs.length<=1)return;
		nTq=0;
		tqA=0;
		for(nTq=0;nTq<arTiqs.length-1;nTq++){
			tqAs=document.getElementById(arTiqs[nTq]).value.replace(/\,/g,'');
			tqA=parseFloat(tqAs);
			if(tqA>0)totalT+=tqA;
		}
		if(totalT>0)totalT=(Math.round(totalT*100))/100;
		objTot=document.getElementById(arTiqs[nTq]);
		objTot.value=formatoValor(totalT);
	}
	function MostrarInspector(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODIGO=2 and mag_codigo=9303;';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	function MostrarConductor(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODICARGO=\'CO\' and test_estado=1 union SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MRELEVADORES_TRANSPORTES Mr  WHERE Mr.MNIT_NIT=MNIT.MNIT_NIT ;';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
	function VerSubrutas(obj){
		var ddlRutaP=document.getElementById('<%=ddlRuta.ClientID%>');
		if(ddlRutaP.value.length==0){
			alert('Seleccione la ruta principal.');
			return;
		}
		ModalDialog(obj,'SELECT MR.MRUT_CODIGO AS CODIGO, MR.MRUT_DESCRIPCION AS DESCRIPCION FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.MRUTA_INTERMEDIA mi where mi.mruta_secundaria=mr.mrut_codigo and mi.mruta_principal=\''+ddlRutaP.value+'\' ORDER BY MR.MRUT_CODIGO', new Array(),1)
	}
	
	
   
    function mostrarPlaca(obj){
    var sqlDsp='SELECT mbus_numero as numero,mcat_placa AS placa from DBXSCHEMA.mbusafiliado where testa_codigo>0;';
    ModalDialog(obj,sqlDsp, new Array(),1)
    }
    
    function TraerPlaca(){

	 AMS_Comercial_PlanillaChequeo.TraerPlaca(txtNumeroBus.value,TraerPlaca_Callback);
	return(false);
    }

    function TraerPlaca_Callback(response){
	    txtNumeroBusa.value=response.value;
	    
     }

    function KeyDownHandlerPlaca(){
	    if(event.keyCode==13 && txtNumeroBus.value.length>0)TraerPlaca();
    }
     function KeyDownHandlerNit(){
	    if(event.keyCode==13 && txtConductor.value.length>0)TraerNit();
    }
 
</script>
