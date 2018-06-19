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
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información de la planilla:</b></td>
		</tr>
		<TR>
			<TD style="WIDTH: 164px; HEIGHT: 18px"><asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Planilla Chequeo :</asp:label></TD>
			<td><asp:textbox id=txtPlanilla runat="server" Font-Size="XX-Small" Width="80px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>">
				</asp:textbox></TD>
		</TR>
		<tr>
			<td style="WIDTH: 164px; HEIGHT: 18px"><asp:label id="Label4" runat="server" visible="true" Font-Size="XX-Small" Font-Bold="True">Agencia Chequeadora:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia"  Visible="true" runat="server" Font-Size="XX-Small" Width="150px"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 164px; HEIGHT: 18px"><asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Inspector :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:textbox id="txtInspector" onclick="MostrarInspector(this);" runat="server" Font-Size="XX-Small"
					Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
				<asp:textbox id="txtInspectora" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></td>
		</tr>
		<TR>
			<TD style="WIDTH: 164px" vAlign="top"><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Ruta Principal :</asp:label></TD>
			<td><asp:dropdownlist id="ddlRuta" runat="server" Font-Size="XX-Small" AutoPostBack="True"></asp:dropdownlist></TD>
		</TR>
		<tr>
			<td style="WIDTH: 164px; HEIGHT: 18px"><asp:label id="Label15" runat="server" visible="true" Font-Size="XX-Small" Font-Bold="True">Agencia Chequeada:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgenciaChequeada"  Visible="true" runat="server" Font-Size="XX-Small" Width="150px"></asp:dropdownlist></td>
		</tr>
	    <TR>
			<TD style="WIDTH: 164px"><asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Número del Bus :</asp:label></TD>
			<td><asp:textbox id="txtNumeroBus" ondblclick="mostrarPlaca(this);"	runat="server" Font-Size="XX-Small" Width="80px"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 164px"><asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Placa del Bus :</asp:label></TD>
			<td><asp:textbox id="txtNumeroBusa" runat="server" Font-Size="XX-Small" Width="80px" MaxLength="6"
					ReadOnly="True"></asp:textbox>&nbsp;
			</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 164px; HEIGHT: 18px"><asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Chequeo :</asp:label></TD>
			<TD style="WIDTH: 374px; HEIGHT: 18px"><asp:textbox id="txtFecha" onkeyup="DateMask(this)" Font-Size="XX-Small" Width="60px" Runat="server"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 164px"><asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Hora Chequeo :</asp:label></TD>
			<TD style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlHora" runat="server" Width="40px" font-Size="XX-Small"></asp:dropdownlist>&nbsp;:&nbsp;
				<asp:dropdownlist id="ddlMinuto" runat="server" Width="48px" font-Size="XX-Small"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 164px"><asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Conductor :</asp:label></TD>
			<td><asp:textbox id="txtConductor"  runat="server" ondblclick="MostrarConductor(this);" Font-Size="XX-Small" Width="80px" ReadOnly="False"></asp:textbox>&nbsp;
				<asp:textbox id="txtConductora" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
		</TR>
		
		<tr>
			<td style="Width: 164px"><asp:label id="Label19" runat="server" 
                    Font-Size="XX-Small" Font-Bold="True">Pasajeros:</asp:label></TD>
			<td><asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Sin 
                Tiquete :</asp:label>&nbsp;
			<asp:textbox id="txtNumSinTiquete" runat="server" Font-Size="XX-Small" Width="80px"></asp:textbox>&nbsp; <asp:label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Con 
                Tiquete :</asp:label>&nbsp; <asp:textbox id="txtNumTiquete" runat="server" Font-Size="XX-Small" Width="80px"></asp:textbox>
			</td>
		</tr>
		<TR>
			<TD style="Width: 164px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD>
			<td>
                <asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Total 
                Pasajeros :</asp:label>&nbsp; <asp:textbox id="txtNumTotal" runat="server" Font-Size="XX-Small" Width="80px"></asp:textbox>&nbsp; <asp:label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Valor 
                total pasajes sin tiquete:</asp:label>
			&nbsp; <asp:textbox id="txtTotalPre" onkeyup="NumericMask(this);" runat="server" Font-Size="XX-Small"
					Width="80px"></asp:textbox>
			</TD>
			
		</TR>
				
			
		<tr>
			<td style="WIDTH: 164px"><asp:Label ID="labelpago" runat="server" Font-Size="XX-Small" Font-Bold="true">Pago :</asp:Label>
			<td><asp:RadioButtonList ID="chkpago" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"> 
			<asp:ListItem Value="S">Si</asp:ListItem>	
			<asp:listItem Value="N">No</asp:listItem>
            </asp:RadioButtonList>
        </tr>
		</TR>
		
		<TR>
			<TD style="WIDTH: 164px"><asp:label id="Label14" runat="server" Font-Size="XX-Small" Font-Bold="True">Lugar del Chequeo :</asp:label></TD>
			<td><asp:textbox id="txtLugar" runat="server" onclick="ModalDialog(this,'SELECT PCHE_CODIGO AS CODIGO,  PCHE_NOMBRE as Nombre from DBXSCHEMA.PCHEQUEOSITIO ORDER BY PCHE_NOMBRE;', new Array(),1);" 
			    Font-Size="XX-Small" MaxLength="20" ReadOnly="true" Columns="20"></asp:textbox>&nbsp;
			    <asp:textbox id="txtLugara" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TD>
		</TR>
		
		<tr>
		   <td style="WIDTH: 164px">
                <asp:Label ID="Label16" runat="server" Text="Es Transbordo ?"></asp:Label>             </td>
            <td >
                <asp:CheckBox ID="chkTransbordo" runat="server" width="30px" AutoPostBack="True" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="lblTransbordo" runat="server" Text="Abono a favor Bus " 
                    Visible="False"></asp:Label>
                <asp:TextBox ID="txtbusRecibe" runat="server" width="50px" 
                    ondblclick="mostrarPlaca(this);" ></asp:TextBox>
                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="lblvalorTransbordo" runat="server" Text="Valor Transbordo " 
                    Visible="False"></asp:Label>
                <asp:TextBox ID="txtvalorTransbordo" runat="server" width="80px" 
                      onkeyup="NumericMask(this);"></asp:TextBox>
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
       
		<TR>
				<TD align="center" class="style1">
					<asp:button id="btnGuardar" Font-Size="XX-Small" Font-Bold="True" 
                        Runat="server" Text="Guardar" Enabled="false"></asp:button></TD>
			    <TD align="center" class="style1">
			        Observaciones   <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" 
                        Font-Size="XX-Small"  Height="43px" Width="541px"></asp:TextBox>
			        
			    </TD>
			</TR>
			
			<TR>
				<td>&nbsp;
					<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
		
	</table>
	
	<br>
	<asp:panel id="pnlDestinos" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px"><B>Destinos:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrDocumentos" runat="server" AutoGenerateColumns="False" ShowFooter="True">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="NUMERO" HeaderText="Linea"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Destino">
								<ItemTemplate>
									<asp:textbox ReadOnly="True" id="txtRuta" onclick="VerSubrutas(this);" Font-Size="XX-Small" runat="server"
										Width="80px"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Cantidad">
								<ItemTemplate>
									<asp:textbox id="txtCantidad" Font-Size="XX-Small" Runat="server" Width="50px" MaxLength="3"></asp:textbox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Valor">
								<ItemTemplate>
									<asp:TextBox id="txtValor" runat="server" Font-Size="XX-Small"></asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Total">
								<ItemTemplate>
									<asp:TextBox id="txtTotal" runat="server" Font-Size="XX-Small" ReadOnly="True"></asp:TextBox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox id="txtSumaTotal" runat="server" Font-Size="XX-Small" ReadOnly="True"></asp:TextBox>
								</FooterTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
			
		</TABLE>
	</asp:panel></DIV>
	
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
