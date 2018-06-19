<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarBusAfiliado.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarBusAfiliado" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center"><asp:panel id="pnlBus" Runat="server" Visible="True">
		<TABLE style="WIDTH: 683px; HEIGHT: 419px" align="center">
			<TR>
				<TD style="HEIGHT: 20px" colSpan="2"><B>Por Favor Seleccione el Vehículo a Modificar:</B>
				</TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 16px">
					<asp:label id="Label9" Font-Size="XX-Small" Font-Bold="True" runat="server">Placa del Vehículo :</asp:label></TD>
				<TD style="HEIGHT: 16px">
					<asp:dropdownlist id="ddlplaca" Font-Size="XX-Small" runat="server" OnChange="cargarPlacaDB(this)"
						Width="150px"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 20px">
					<asp:label id="Label10" Font-Size="XX-Small" Font-Bold="True" runat="server">Vin Vehículo:</asp:label></TD>
				<TD style="HEIGHT: 20px">
					<asp:textbox id="txtVin" Font-Size="XX-Small" runat="server" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 24px">
					<asp:label id="Label11" Font-Size="XX-Small" Font-Bold="True" runat="server">Catálogo del Vehículo :</asp:label></TD>
				<TD style="HEIGHT: 24px">
					<asp:textbox id="txtCatalogo" Font-Size="XX-Small" runat="server" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 23px">
					<asp:label id="Label12" Font-Size="XX-Small" Font-Bold="True" runat="server">Propietario del Vehículo : </asp:label></TD>
				<TD style="HEIGHT: 23px">
					<asp:textbox id="ddlpropietario" Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:textbox id="ddlpropietarioa" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox>
					<asp:button id="btnPropietarios" Runat="server" Font-Size="XX-Small" Text="Modificar"></asp:button></TD>
			</TR>
			<TR>
				<td>
					<asp:label id="Label13" Font-Size="XX-Small" Font-Bold="True" runat="server">Asociado Cooperativa : </asp:label></TD>
				<td>
					<asp:textbox id="ddlasociado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
						Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:textbox id="ddlasociadoa" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="HEIGHT: 9px">
					<asp:label id="Label14" Font-Size="XX-Small" Font-Bold="True" runat="server">Conductor Principal : </asp:label></TD>
				<TD style="HEIGHT: 9px">
					<asp:textbox id="ddlconductor" onclick="MostrarConductor(this);" Font-Size="XX-Small" runat="server"
						Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:textbox id="ddlconductora" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			</TR>
			<TR>
				<td>
					<asp:label id="Label8" Font-Size="XX-Small" Font-Bold="True" runat="server">2ª Conductor Principal</asp:label></TD>
				<td>
					<asp:textbox id="conductor2" onclick="MostrarConductor(this);" Font-Size="XX-Small" runat="server"
						Width="80px" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:textbox id="conductor2a" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<td>
					<asp:label id="Label1" Font-Size="XX-Small" Font-Bold="True" runat="server">Valor Comercial:</asp:label></TD>
				<td>
					<asp:textbox id="valor" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<td>
					<asp:label id="Label2" Font-Size="XX-Small" Font-Bold="True" runat="server">Categoria</asp:label></TD>
				<td>
					<asp:dropdownlist id="categoria" Font-Size="XX-Small" runat="server" Width="71px"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<td>
					<asp:label id="Label20" Font-Size="XX-Small" Font-Bold="True" runat="server">Fecha de Ingreso : </asp:label></TD>
				<td>
					<asp:textbox id="FechaIngreso" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small" ReadOnly="True"></asp:textbox>&nbsp;
					<asp:label id="Label17" Font-Size="XX-Small" Font-Bold="True" runat="server">(Año-Mes-Dia)</asp:label></TD>
			</TR>
			<TR>
				<td>
					<asp:label id="Label15" Font-Size="XX-Small" Font-Bold="True" runat="server">Numero del Vehículo : </asp:label></TD>
				<td>
					<asp:label id="Label19" Font-Size="XX-Small" Font-Bold="True" runat="server">Numero Actual : </asp:label>
					<asp:textbox id="numeroviejo" Font-Size="XX-Small" Font-Bold="True" runat="server" Width="74px"
						ReadOnly="True" Wrap="False"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:label id="Label18" Font-Size="XX-Small" Font-Bold="True" runat="server">Nuevo Numero :</asp:label>&nbsp;
					<asp:textbox id="NumBus" Runat="server" Font-Size="XX-Small" Font-Bold="True" Width="65px" ForeColor="Brown"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 29px">
					<asp:Label id="Label6" Font-Size="XX-Small" Font-Bold="True" runat="server">Capacidad Pasajeros:</asp:Label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 29px">
					<asp:TextBox id="txtCapacidad" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"
						Width="50px" MaxLength="3"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 29px">
					<asp:label id="Label22" Font-Size="XX-Small" Font-Bold="True" runat="server">Potencia</asp:label></TD>
				<TD style="WIDTH: 374px; HEIGHT: 29px">
					<asp:textbox id="txtPotencia" Font-Size="XX-Small" runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 29px">
					<asp:label id="Label5" Font-Size="XX-Small" Font-Bold="True" runat="server">Capacidad Combustible</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 29px">
					<asp:textbox id="txtCapacidadC" onkeyup="NumericMask(this);" Font-Size="XX-Small" runat="server"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label4" Font-Size="XX-Small" Font-Bold="True" runat="server">Configuración asociada :</asp:label></TD>
				<TD style="WIDTH: 374px; HEIGHT: 18px">
					<asp:dropdownlist id="ddlConfiguracion" Runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label21" Font-Size="XX-Small" Font-Bold="True" runat="server">Reposición del vehículo: </asp:label></TD>
				<TD style="WIDTH: 374px; HEIGHT: 18px">
					<asp:textbox id="txtReposicion" onclick="ModalDialog(this,'SELECT MBUSAFILIADO.MCAT_PLACA AS PLACA,MBUSAFILIADO.MBUS_NUMERO AS NUMERO from DBXSCHEMA.MBUSAFILIADO MBUSAFILIADO', new Array(),1)"
						Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px; HEIGHT: 18px">
					<asp:label id="Label23" Font-Size="XX-Small" Font-Bold="True" runat="server">Observaciones </asp:label></TD>
				<TD style="WIDTH: 374px; HEIGHT: 18px">
					<asp:textbox id="txtObservaciones" Font-Size="XX-Small" runat="server" Width="250px" Height="60px"
						TextMode="MultiLine"></asp:textbox></TD>
			</TR>
			<TR>
				<td>
					<asp:label id="Label16" Font-Size="XX-Small" Font-Bold="True" runat="server">Estado del Vehículo :</asp:label></TD>
				<td>
					<asp:dropdownlist id="ddlestado" Font-Size="XX-Small" runat="server" Width="204px"></asp:dropdownlist>
					<asp:label id="Label3" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="2">
					<asp:button id="btnGuardar" Runat="server" Font-Size="XX-Small" Text="Guardar"></asp:button></TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE id="Table2" style="WIDTH: 683px" align="center">
			<TR>
				<TD style="HEIGHT: 20px" colSpan="2"><B>Componentes del vehiculo:</B>
				</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 340px; HEIGHT: 120px" vAlign="top" align="center"><BR>
					<asp:datagrid id="dgrComponentes" runat="server" AutoGenerateColumns="False" DataKeyField="TCOMP_TIPOCOMP">
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="TCOMP_NOMBCOMP" HeaderText="COMPONENTE"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="VALOR">
								<ItemTemplate>
									<asp:TextBox id="txtValorComponente" Font-Size="XX-Small" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "VALOR") %>' onkeyup="NumericMask(this);">
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></TD>
				<TD style="HEIGHT: 23px" vAlign="top" align="left" colSpan="2">
					<P><BR>
						<asp:button id="btnGuardarComponentes" Runat="server" Font-Size="XX-Small" Text="Guardar"></asp:button></P>
				</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE id="Table3" style="WIDTH: 683px" align="center">
			<TBODY>
				<TR>
					<TD style="HEIGHT: 20px" colSpan="2"><B>Documentos del vehiculo:</B>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 340px" vAlign="top" align="center" colSpan="2"><BR>
						<asp:datagrid id="dgrDocumentos" runat="server" AutoGenerateColumns="False" DataKeyField="TDOC_TIPODOC">
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<FooterStyle BackColor="#CCCCCC"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="TDOC_NOMBDOC" HeaderText="DOCUMENTO"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="NUMERO">
									<ItemTemplate>
										<asp:TextBox id="txtNumeroDocumento" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NUMERO") %>' MaxLength="50" Width="100px" Font-Size="XX-Small">
										</asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="ENTIDAD">
									<ItemTemplate>
										<asp:textbox id="txtEntidad" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"
											Font-Size="XX-Small" runat="server" Width="80px" ReadOnly="True"></asp:textbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="DESDE">
									<ItemTemplate>
										<asp:textbox id="txtFechaDesde" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server"
											Width="60px"></asp:textbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="HASTA">
									<ItemTemplate>
										<asp:textbox id="txtFechaHasta" onkeyup="DateMask(this)" Font-Size="XX-Small" Runat="server"
											Width="60px"></asp:textbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="IMAGEN">
									<ItemTemplate>
										<input type="file" id="txtImagen" runat="server" NAME="txtImagen" width="40px" Font-Size="XX-Small" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Ver">
									<ItemTemplate>
										<asp:HyperLink ID="lnkImagen" Runat="server"></asp:HyperLink>
	</asp:panel>
	</ItemTemplate> </asp:TemplateColumn> </Columns> </asp:datagrid></TD></TR>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td vAlign="middle" align="center" colSpan="2"><asp:button id="btnGuardarDocumentos" Runat="server" Font-Size="XX-Small" Text="Guardar"></asp:button></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	</TBODY></TABLE></DIV>
<script language:javascript>
function cargarPlacaDB(Obj)
{
	AMS_Comercial_ModificarBusAfiliado.CargarPlaca(Obj.value,CargarPlaca_Callback);
}
function CargarPlaca_Callback(response)
{
	var txtVin=document.getElementById("<%=txtVin.ClientID%>");
	var txtCatalogo=document.getElementById("<%=txtCatalogo.ClientID%>");
	var FechaIngreso=document.getElementById("<%=FechaIngreso.ClientID%>");
	var numeroviejo=document.getElementById("<%=numeroviejo.ClientID%>");
	var valor=document.getElementById("<%=valor.ClientID%>");
	var ddlpropietario=document.getElementById("<%=ddlpropietario.ClientID%>");
	var ddlconductor=document.getElementById("<%=ddlconductor.ClientID%>");
	var ddlconfig=document.getElementById("<%=ddlConfiguracion.ClientID%>");
	var ddlasociado=document.getElementById("<%=ddlasociado.ClientID%>");
	var FechaIngreso=document.getElementById("<%=FechaIngreso.ClientID%>");
	var conductor2=document.getElementById("<%=conductor2.ClientID%>");
	var potencia=document.getElementById("<%=txtPotencia.ClientID%>");
	var galonaje=document.getElementById("<%=txtCapacidadC.ClientID%>");
	var pasajeros=document.getElementById("<%=txtCapacidad.ClientID%>");
	var observaciones=document.getElementById("<%=txtObservaciones.ClientID%>");
	var reposicion=document.getElementById("<%=txtReposicion.ClientID%>");
	var categoria=document.getElementById("<%=categoria.ClientID%>");
	var estado=document.getElementById("<%=ddlestado.ClientID%>");
	var respuesta=response.value;
	var nomPropietario=document.getElementById("<%=ddlpropietarioa.ClientID%>");
	var nomAsociado=document.getElementById("<%=ddlasociadoa.ClientID%>");
	var nomConductor1=document.getElementById("<%=ddlconductora.ClientID%>");
	var nomConductor2=document.getElementById("<%=conductor2a.ClientID%>");

		if(respuesta.Tables[0].Rows.length>0)
		{
			txtVin.value=respuesta.Tables[0].Rows[0].VIN;
			valor.value=formatoValor(respuesta.Tables[0].Rows[0].VALOR);
			numeroviejo.value=respuesta.Tables[0].Rows[0].NUMERO;
			txtCatalogo.value=respuesta.Tables[0].Rows[0].CATALOGO;
			ddlpropietario.value=respuesta.Tables[0].Rows[0].PROPIETARIO;
			categoria.value=respuesta.Tables[0].Rows[0].CATEGORIA;
			estado.value=respuesta.Tables[0].Rows[0].ESTADO;
			galonaje.value=formatoValor(respuesta.Tables[0].Rows[0].GALONAJE);
			pasajeros.value=respuesta.Tables[0].Rows[0].PASAJEROS;
			if(respuesta.Tables[0].Rows[0].CONFIG!=null)ddlconfig.value= respuesta.Tables[0].Rows[0].CONFIG;
			if(respuesta.Tables[0].Rows[0].CHOFER!=null) ddlconductor.value=respuesta.Tables[0].Rows[0].CHOFER;
			else ddlconductor.value="";
			
			if(respuesta.Tables[0].Rows[0].ASOCIADO!=null) ddlasociado.value=respuesta.Tables[0].Rows[0].ASOCIADO;
			else ddlasociado.value="";

			if(respuesta.Tables[0].Rows[0].CHOFER2!=null) conductor2.value=respuesta.Tables[0].Rows[0].CHOFER2;
			else conductor2.value="";
			
			if(respuesta.Tables[0].Rows[0].REPOSICION!=null) reposicion.value=respuesta.Tables[0].Rows[0].REPOSICION;
			else reposicion.value="";

			if(respuesta.Tables[0].Rows[0].OBSERVACIONES!=null) observaciones.value=respuesta.Tables[0].Rows[0].OBSERVACIONES;
			else observaciones.value="";
			
			if(respuesta.Tables[0].Rows[0].POTENCIA!=null) potencia.value=respuesta.Tables[0].Rows[0].POTENCIA;
			else potencia.value="";
			
			FechaIngreso.value=convertirFecha(respuesta.Tables[0].Rows[0].FECHA);
			
			if(respuesta.Tables[0].Rows[0].NOMPROPIETARIO!=null)nomPropietario.value=respuesta.Tables[0].Rows[0].NOMPROPIETARIO;
			else nomPropietario.value="";
			if(respuesta.Tables[0].Rows[0].NOMASOCIADO!=null)nomAsociado.value=respuesta.Tables[0].Rows[0].NOMASOCIADO;
			else nomAsociado.value="";
			if(respuesta.Tables[0].Rows[0].NOMCONDUCTOR1!=null)nomConductor1.value=respuesta.Tables[0].Rows[0].NOMCONDUCTOR1;
			else nomConductor1.value="";
			if(respuesta.Tables[0].Rows[0].NOMCONDUCTOR2!=null)nomConductor2.value=respuesta.Tables[0].Rows[0].NOMCONDUCTOR2;
			else nomConductor2.value="";
		}
		else{
			txtVin.value="";
			valor.value="";
			numeroviejo.value="";
			txtCatalogo.value="";
			ddlpropietario.value="";
			ddlconductor.value="";
			ddlasociado.value="";
			FechaIngreso.value="";
			conductor2.value="";
			return;
		}
		strComponentes='<%=idComponentes%>';
		arrComponentes=strComponentes.split("?");
		for(nC=0;nC<arrComponentes.length;nC++)
			if(respuesta.Tables[1].Rows[nC].VALOR==null)
				document.getElementById(arrComponentes[nC]).value="";
			else
				document.getElementById(arrComponentes[nC]).value=formatoValor(respuesta.Tables[1].Rows[nC].VALOR);

		strDocP='<%=idNumerosDocs%>';
		arrDocNums=strDocP.split("?");
		strDocP='<%=idEntidades%>';
		arrDocIds=strDocP.split("?");
		strDocP='<%=idDesdeFchs%>';
		arrFchDesde=strDocP.split("?");
		strDocP='<%=idHastaFchs%>';
		arrFchHasta=strDocP.split("?");
		strDocP='<%=idArchivos%>';
		arrArch=strDocP.split("?");
		strDocP='<%=idLinksImg%>';
		arrLnksI=strDocP.split("?");
		for(nC=0;nC<arrDocNums.length;nC++){
			if(respuesta.Tables[2].Rows[nC].NUMERO==null)document.getElementById(arrDocNums[nC]).value="";
			else document.getElementById(arrDocNums[nC]).value=respuesta.Tables[2].Rows[nC].NUMERO;
			if(respuesta.Tables[2].Rows[nC].MNIT_ENTIDAD==null)document.getElementById(arrDocIds[nC]).value="";
			else document.getElementById(arrDocIds[nC]).value=respuesta.Tables[2].Rows[nC].MNIT_ENTIDAD;
			if(respuesta.Tables[2].Rows[nC].FECHA_DESDE==null)document.getElementById(arrFchDesde[nC]).value="";
			else document.getElementById(arrFchDesde[nC]).value=convertirFecha(respuesta.Tables[2].Rows[nC].FECHA_DESDE);
			if(respuesta.Tables[2].Rows[nC].FECHA_HASTA==null)document.getElementById(arrFchHasta[nC]).value="";
			else document.getElementById(arrFchHasta[nC]).value=convertirFecha(respuesta.Tables[2].Rows[nC].FECHA_HASTA);
			if(respuesta.Tables[2].Rows[nC].IMAGEN==null){
				document.getElementById(arrLnksI[nC]).href="";
				document.getElementById(arrLnksI[nC]).target="_self";
				document.getElementById(arrLnksI[nC]).innerHTML="";}
			else{
				document.getElementById(arrLnksI[nC]).href="../img/DOC_VEHICULOS/"+respuesta.Tables[2].Rows[nC].IMAGEN;
				document.getElementById(arrLnksI[nC]).target="_blank";
				document.getElementById(arrLnksI[nC]).innerHTML="ver";}
		}
}
function convertirFecha(fechaIni){
	var someD = new Date();
	someD = fechaIni;
	var un = someD.getMonth()+1;
	if (un<9){
		un= '0'+un;
	}
	un = un.toString();
	var dosF = someD.getDate();
	if (dosF<9){
		dosF= '0'+dosF;
	}
	dosF = dosF.toString();
	var tresF = someD.getFullYear();
	tresF=tresF.toString();
	return(tresF+'-'+un+'-'+dosF);
}
function MostrarPersonal(obj,flt){
	var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO=\''+flt+'\';';
	ModalDialog(obj,sqlDsp, new Array(),1)
}
function MostrarConductor(obj){
	var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME  WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND ME.PCAR_CODICARGO=\'CO\';';
	ModalDialog(obj,sqlDsp, new Array(),1)
}
</script>
<%=imgCambia%>
</asp:panel><asp:panel id="pnlPropietarios" Runat="server" Visible="False">
	<TABLE style="WIDTH: 683px" align="center">
		<TR>
			<TD style="HEIGHT: 20px" colSpan="3"><B>Información de los propietarios:</B>
			</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 100px; HEIGHT: 16px">
				<asp:label id="Label7" Font-Size="XX-Small" Font-Bold="True" runat="server">Vehículo :</asp:label></TD>
			<TD style="HEIGHT: 16px" colSpan="2">
				<asp:label id="lblPlaca" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 100px; HEIGHT: 16px">
				<asp:label id="Label25" Font-Size="XX-Small" Font-Bold="True" runat="server">Fecha :</asp:label></TD>
			<TD style="HEIGHT: 16px" colSpan="2">
				<asp:textbox id="txtFechaN" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small" Width="80px"
					ReadOnly="False"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 100px; HEIGHT: 16px" colSpan="3">
				<asp:label id="Label24" Font-Size="XX-Small" Font-Bold="True" runat="server">Propietarios :</asp:label></TD>
		</TR>
		<TR>
			<TD align="center" colSpan="3">
				<asp:datagrid id="dgrPropietarios" runat="server" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="NIT">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MNIT_NIT") %>
							</ItemTemplate>
							<FooterTemplate>
								<asp:textbox id="txtNITN" ReadOnly="False" Font-Size="XX-Small" Runat="server" Width="100px"
									onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)"></asp:textbox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Porcentaje<br>Propiedad">
							<ItemTemplate>
								<asp:TextBox id="txtPorcProp" Font-Size="XX-Small" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE") %>' onkeyup="NumericMask(this);" Width="50px">
								</asp:TextBox>
							</ItemTemplate>
							<FooterTemplate>
								<asp:textbox id="txtPorcPropN" ReadOnly="False" Font-Size="XX-Small" Runat="server" Width="50px"></asp:textbox>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Principal">
							<ItemTemplate>
								<asp:CheckBox id="chkPrincipal" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "PRINCIPAL")) %>'>
								</asp:CheckBox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Button id="btnEliminar" runat="server" CommandName="Eliminar" Text="Remover" CausesValidation="false" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:Button id="btnAgregar" runat="server" CommandName="Agregar" Text="Agregar" CausesValidation="false" />
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 100px; HEIGHT: 16px">&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 100px; HEIGHT: 16px" colSpan="3">
				<asp:label id="Label26" Font-Size="XX-Small" Font-Bold="True" runat="server">Historial Propietarios :</asp:label></TD>
		</TR>
		<TR>
			<TD align="center" colSpan="3">
				<asp:datagrid id="dgrPropietariosHst" runat="server" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Fecha">
							<ItemTemplate>
								<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FECHA_PROPIETARIO")).ToString("yyyy-MM-dd") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="NIT">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MNIT_NIT") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Porcentaje">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "PORCENTAJE") %>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 100px; HEIGHT: 16px">&nbsp;</TD>
		</TR>
		<TR>
			<TD vAlign="middle" align="center" colSpan="3">
				<asp:button id="btnGuardarP" Runat="server" Font-Size="XX-Small" Text="Actualizar"></asp:button>&nbsp;&nbsp;
				<asp:button id="btnCancelar" Runat="server" Font-Size="XX-Small" Text="Cancelar"></asp:button></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 100px; HEIGHT: 16px" colSpan="3">&nbsp;</TD>
		</TR>
	</TABLE>
</asp:panel><asp:label id="lblError" Font-Size="XX-Small" Font-Bold="True" runat="server"></asp:label>
