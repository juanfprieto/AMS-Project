<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.DespachoPapeleriaPrepago.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_DespachoPapeleriaPrepago" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información del despacho:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Número de Recepción :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:label id="lblNumero" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:label id="lblFecha" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></td>
		</tr>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<TD style="WIDTH: 545px" colSpan="3"><B>Documentos:</B></TD>
		</TR>
		<TR>
			<TD align="center"><asp:datagrid id="dgrDocumentos" runat="server" AutoGenerateColumns="False" ShowFooter="True">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn DataField="NUMERO" HeaderText="Linea"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Ruta">
							<ItemTemplate>
								<asp:textbox id="txtRuta" ondblclick="ModalDialog(this,'SELECT MR.MRUT_CODIGO AS CODIGO, MR.MRUT_DESCRIPCION AS DESCRIPCION FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PCO, DBXSCHEMA.PCIUDAD PCD WHERE MR.PCIU_COD=PCO.PCIU_CODIGO AND MR.PCIU_CODDES=PCD.PCIU_CODIGO ORDER BY MR.MRUT_CODIGO', new Array(),1)"
									Font-Size="XX-Small" runat="server" Width="80px"></asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Talonarios">
							<ItemTemplate>
								<asp:textbox id="txtTalonarios" Font-Size="XX-Small" Runat="server" Width="50px" MaxLength="4"></asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Inicio">
							<ItemTemplate>
								<asp:textbox id="txtInicioDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'>
								</asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Fin">
							<ItemTemplate>
								<asp:textbox id="txtFinDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'>
								</asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="%Dcto.">
							<ItemTemplate>
								<asp:textbox id="txtDescuento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength="6" Text='<%#ViewState["PorcentajePrepago"]%>'>
								</asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
		<TR>
			<td>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center"><asp:button id="btnRecibir" Font-Size="XX-Small" Font-Bold="True" Text="Despachar" Runat="server"></asp:button></TD>
		</TR>
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
	</TABLE>
</DIV>
<script language="javascript">
<%=strActScript%>
	var cantTalonarios=<%=ViewState["CANTIDAD_TALONARIOS"]%>;
	function Total(tTalonarios,tInicio,tFin){
		var txtTalonarios=document.getElementById(tTalonarios);
		var txtInicio=document.getElementById(tInicio);
		var txtFin=document.getElementById(tFin);
		if(txtTalonarios.value.length>0 && txtInicio.value.length>0){
			try{
				txtFin.value=((parseInt(txtTalonarios.value)*cantTalonarios)+parseInt(txtInicio.value)-1);
			}
			catch(err){
				txtFin.value="";
			}
		}
	}
</script>
