<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RecepcionPlanillasChequeo.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RecepcionPlanillasChequeo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información de la recepción:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Número de Recepción :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:label id="lblNumero" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">Fecha :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:label id="lblFecha" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<TD style="WIDTH: 545px" colSpan="3"><B>Planillas de Chequeo:</B></TD>
		</TR>
		<TR>
			<TD align="center"><asp:datagrid id="dgrDocumentos" runat="server" AutoGenerateColumns="False">
					<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
					<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
					<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
					<FooterStyle BackColor="#CCCCCC"></FooterStyle>
					<Columns>
						<asp:BoundColumn DataField="NUMERO" HeaderText="Linea" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Talonarios" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
							<ItemTemplate>
								<asp:textbox id="txtTalonarios" Font-Size="XX-Small" Runat="server" Width="50px" MaxLength="4"></asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Inicio" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
							<ItemTemplate>
								<asp:textbox id="txtInicioDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'>
								</asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Fin" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
							<ItemTemplate>
								<asp:textbox id="txtFinDocumento" Font-Size="XX-Small" Runat="server" Width="70px" MaxLength='<%#AMS.Comercial.Tiquetes.lenTiquete%>'>
								</asp:textbox>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid></TD>
		</TR>
		<TR>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center"><asp:button id="btnRecibir" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Recibir"></asp:button></TD>
		</TR>
		<TR>
			<TD>&nbsp;
				<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
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
				txtFin.value=((parseInt(txtTalonarios.value)*cantTalonarios)+parseInt(txtInicio.value)-1)
			}
			catch(err){
				txtFin.value="";
			}
		}
	}
</script>
