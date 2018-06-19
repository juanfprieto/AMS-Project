<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ConsultarPapeleriaAgencia.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ConsultarPapeleriaAgencia" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script language="javascript" type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colSpan="3"><b>Información de la papelería:</b></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo:</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlTipoDocumento" runat="server" Font-Size="XX-Small"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Clase :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlClaseDocumento" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="M" Selected="True">Manual</asp:ListItem>
					<asp:ListItem Value="V">Virtual</asp:ListItem>
				</asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Agencia :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAgencia" runat="server" Font-Size="XX-Small" AutoPostBack="True" Width="150px"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Asignados :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlAsignados" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:dropdownlist>&nbsp;&nbsp;Nit
				<asp:textbox id="txtResponsable" onclick="MostrarPersonalAgencia(this);" runat="server" Font-Size="XX-Small"
					ReadOnly="False" Width="80px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Número :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">desde:<asp:textbox id=txtInicioDocumento Font-Size="XX-Small" Width="70px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>" Runat="server"></asp:textbox>&nbsp;&nbsp;hasta:<asp:textbox id=txtFinDocumento Font-Size="XX-Small" Width="70px" MaxLength="<%#AMS.Comercial.Tiquetes.lenTiquete%>" Runat="server"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Planillados :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:dropdownlist id="ddlPlanillados" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:dropdownlist>&nbsp;&nbsp;No.<asp:textbox id="txtPlanilla" runat="server" Font-Size="XX-Small" Width="80px"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Asignacion :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">Inicial :<asp:textbox id="TextFechaAsigI" onkeyup="DateMask(this)" runat="server" Width="84px"></asp:textbox>&nbsp;&nbsp;Final 
				:
				<asp:textbox id="TextFechaAsigF" onkeyup="DateMask(this)" runat="server" Width="79px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Uso :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">Inicial :<asp:textbox id="TextFechaUsoI" onkeyup="DateMask(this)" runat="server" Width="84px"></asp:textbox>&nbsp;&nbsp;Final 
				:
				<asp:textbox id="TextFechaUsoF" onkeyup="DateMask(this)" runat="server" Width="79px"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 130px; HEIGHT: 18px"><asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Anulacion :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px">Inicial :<asp:textbox id="TextFechaAnulI" onkeyup="DateMask(this)" runat="server" Width="84px"></asp:textbox>&nbsp;&nbsp;Final 
				:
				<asp:textbox id="TextFechaAnulF" onkeyup="DateMask(this)" runat="server" Width="79px"></asp:textbox></td>
		</tr>
		<TR>
			<TD align="right"><asp:button id="btnConsultar" Font-Size="XX-Small" Font-Bold="True" Runat="server" Text="Consultar"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
		</TR>
		<tr>
			<td colSpan="2">&nbsp;</td>
		</tr>
	</table>
	<br>
	<asp:panel id="pnlPapeleria" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Papelería:</B></TD>
			</TR>
			<TR>
				<TD align="center">
					<asp:datagrid id="dgrPapeleria" runat="server" AutoGenerateColumns="False" AllowPaging="True"
						PageSize="30">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="TDOC_CODIGO" HeaderText="Tipo"></asp:BoundColumn>
							<asp:BoundColumn DataField="NUM_DOCUMENTO" HeaderText="No."></asp:BoundColumn>
							<asp:BoundColumn DataField="TIPO_DOCUMENTO" HeaderText="Clase"></asp:BoundColumn>
							<asp:BoundColumn DataField="FECHA_RECEPCION" HeaderText="Recepci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							<asp:BoundColumn DataField="FECHA_DESPACHO" HeaderText="Despacho" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							<asp:BoundColumn DataField="AGENCIA" HeaderText="Agencia"></asp:BoundColumn>
							<asp:BoundColumn DataField="FECHA_ASIGNACION" HeaderText="Asignaci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							<asp:BoundColumn DataField="MNIT_RESPONSABLE" HeaderText="NitRspnsble"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_RESPONSABLE" HeaderText="Responsable"></asp:BoundColumn>
							<asp:BoundColumn DataField="FECHA_DEVOLUCION" HeaderText="Devoluci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							<asp:BoundColumn DataField="FECHA_ANULACION" HeaderText="Anulaci&#243;n" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
							<asp:BoundColumn DataField="MPLA_CODIGO" HeaderText="Planilla"></asp:BoundColumn>
							<asp:BoundColumn DataField="FECHA_USO" HeaderText="Uso" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"></asp:BoundColumn>
							<asp:BoundColumn DataField="MRUT_CODIGO" HeaderText="Ruta"></asp:BoundColumn>
							<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_DOCUMENTO"
								HeaderText="VALOR"></asp:BoundColumn>
						</Columns>
						<PagerStyle Mode="NumericPages"></PagerStyle>
					</asp:datagrid><BR>
					*Agencia preasignada</TD>
			</TR>
			<TR>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<td>&nbsp;
					<asp:label id="lblError" Font-Bold="True" Font-Size="XX-Small" runat="server"></asp:label></TD>
			</TR>
		</TABLE>
	</asp:panel>
</DIV>
<script language="javascript">
<%=strActScript%>
var ddlAgencia=document.getElementById("<%=ddlAgencia.ClientID%>");
function MostrarPersonalAgencia(obj){
		var sqlDsp='SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE,PCAR_DESCRIPCION AS CARGO from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO='+ddlAgencia.value.replace('|','')+' AND MP.MNIT_NIT=MNIT.MNIT_NIT AND PC.PCAR_CODIGO=MP.PCAR_CODIGO;';
		ModalDialog(obj,sqlDsp, new Array(),1)
	}
</script>
