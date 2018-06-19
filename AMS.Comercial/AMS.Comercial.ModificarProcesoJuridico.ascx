<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarProcesoJuridico.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarProcesoJuridico" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<DIV align="center"><asp:panel id="pnlSeleccion" Runat="server" Visible="True">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Selección del proceso jurídico:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label4" runat="server" Font-Size="XX-Small" Font-Bold="True">Número :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:textbox id="txtNumero" ondblclick="ModalDialog(this,'SELECT RTRIM(CHAR(NUM_PROCESO)) NUMERO, COD_RADICACION CODIGO FROM DBXSCHEMA.MPROCESOS_JURIDICOS', new Array(),1)"
						Runat="server" Font-Size="XX-Small" Width="60px"></asp:textbox></TD>
			</TR>
			<TR>
				<TD align="right" colSpan="2">
					<asp:button id="btnSeleccionar" Runat="server" Font-Size="XX-Small" Font-Bold="True" Text="Seleccionar"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
			</TR>
		</TABLE>
	</asp:panel><asp:panel id="pnlInformacion" Runat="server" Visible="False">
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Información del proceso jurídico:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label13" runat="server" Font-Size="XX-Small" Font-Bold="True">Número :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:label id="lblNumero" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Bold="True">Juzgado :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlJuzgado" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<td>
					<asp:Label id="Label12" runat="server" Font-Size="XX-Small" Font-Bold="True">Código Radicación:</asp:Label></TD>
				<td>
					<asp:TextBox id="txtRadicacion" runat="server" Font-Size="XX-Small" MaxLength="10"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label2" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo Proceso Jurídico :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlProceso" runat="server" Font-Size="XX-Small" AutoPostBack="True"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label3" runat="server" Font-Size="XX-Small" Font-Bold="True">Tipo Subproceso Jurídico :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlSubproceso" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Bold="True">Clase :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlClase" runat="server" Font-Size="XX-Small">
						<asp:ListItem Value="O">Ordinario</asp:ListItem>
						<asp:ListItem Value="E">Ejecutivo</asp:ListItem>
					</asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">Descripción :</asp:label></TD>
				<td>
					<asp:TextBox id="txtDescripcion" runat="server" Font-Size="XX-Small" Width="570px" MaxLength="4000"
						TextMode="MultiLine" Height="200"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label7" runat="server" Font-Size="XX-Small" Font-Bold="True">Origen :</asp:label></TD>
				<td>
					<asp:TextBox id="txtOrigen" runat="server" Font-Size="XX-Small" Width="570px" MaxLength="100"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 107px">
					<asp:Label id="Label8" runat="server" Font-Size="XX-Small" Font-Bold="True">Demandante :</asp:Label></TD>
				<td>
					<asp:TextBox id="txtDemandante" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT', new Array(),1)"
						runat="server" Font-Size="XX-Small" Width="80px" ReadOnly="True"></asp:TextBox>&nbsp;
					<asp:textbox id="txtDemandantea" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 107px">
					<asp:Label id="Label9" runat="server" Font-Size="XX-Small" Font-Bold="True">Demandado :</asp:Label></TD>
				<td>
					<asp:TextBox id="txtDemandado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT', new Array(),1)"
						runat="server" Font-Size="XX-Small" Width="80px" ReadOnly="True"></asp:TextBox>&nbsp;
					<asp:textbox id="txtDemandadoa" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 107px">
					<asp:Label id="Label10" runat="server" Font-Size="XX-Small" Font-Bold="True">Abogado Demandante :</asp:Label></TD>
				<td>
					<asp:TextBox id="txtAboDemandante" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT IN (SELECT MNIT_NIT FROM DBXSCHEMA.MABOGADOS)', new Array(),1)"
						runat="server" Font-Size="XX-Small" Width="80px" ReadOnly="True"></asp:TextBox>&nbsp;
					<asp:textbox id="txtAboDemandantea" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 107px">
					<asp:Label id="Label11" runat="server" Font-Size="XX-Small" Font-Bold="True">Abogado Demandado :</asp:Label></TD>
				<td>
					<asp:TextBox id="txtAboDemandado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT IN (SELECT MNIT_NIT FROM DBXSCHEMA.MABOGADOS)', new Array(),1)"
						runat="server" Font-Size="XX-Small" Width="80px" ReadOnly="True"></asp:TextBox>&nbsp;
					<asp:textbox id="txtAboDemandadoa" runat="server" Font-Size="XX-Small" Width="300px" ReadOnly="True"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label14" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha Prox. Actuación :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtFechaProxAct" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small"
						Width="60px"></asp:textbox></TD>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label15" runat="server" Font-Size="XX-Small" Font-Bold="True">Normatividad :</asp:label></TD>
				<td>
					<asp:TextBox id="txtNormatividad" runat="server" Font-Size="XX-Small" Width="570px" MaxLength="400"
						TextMode="MultiLine" Height="100"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label16" runat="server" Font-Size="XX-Small" Font-Bold="True">Fecha ProcesoJurídico :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top">
					<asp:textbox id="txtFechaProceso" onkeyup="DateMask(this)" Runat="server" Font-Size="XX-Small"
						Width="60px"></asp:textbox></TD>
				<td>&nbsp;</TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label17" runat="server" Font-Size="XX-Small" Font-Bold="True">Estado :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlEstado" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label18" runat="server" Font-Size="XX-Small" Font-Bold="True">Archivo :</asp:label></TD>
				<TD style="WIDTH: 386px"><INPUT id="txtArchivo" type="file" name="txtArchivo" runat="server" Font-Size="XX-Small"
						width="40px"><BR>
					<asp:HyperLink id="lnkArchivo" Runat="server" Visible="False"></asp:HyperLink></TD>
			</TR>
			<TR>
				<TD align="right" colSpan="2">
					<asp:button id="btnGuardar" Runat="server" Font-Size="XX-Small" Font-Bold="True" Text="Guardar"></asp:button>&nbsp;&nbsp;&nbsp;
					<asp:button id="btnCancelar" Runat="server" Font-Size="XX-Small" Font-Bold="True" Text="Cancelar"></asp:button>&nbsp;&nbsp;</TD>
			</TR>
			<TR>
				<TD colSpan="2">&nbsp;</TD>
			</TR>
		</TABLE>
		<BR>
		<TABLE style="WIDTH: 773px" align="center">
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><B>Actuaciones:</B></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label19" runat="server" Font-Size="XX-Small" Font-Bold="True">Asistentes :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top" colSpan="2">
					<asp:textbox id="txtAsistentesA" Runat="server" Font-Size="XX-Small" Width="570px" TextMode="MultiLine"
						Height="50px"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px" vAlign="top">
					<asp:label id="Label20" runat="server" Font-Size="XX-Small" Font-Bold="True">Descripción :</asp:label></TD>
				<TD style="WIDTH: 386px; HEIGHT: 18px" vAlign="top" colSpan="2">
					<asp:textbox id="txtDescripcionA" Runat="server" Font-Size="XX-Small" Width="570px" TextMode="MultiLine"
						Height="50px"></asp:textbox></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px">
					<asp:label id="Label21" runat="server" Font-Size="XX-Small" Font-Bold="True">Estado :</asp:label></TD>
				<TD style="WIDTH: 386px">
					<asp:dropdownlist id="ddlEstadoA" runat="server" Font-Size="XX-Small"></asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 154px"></TD>
				<TD style="WIDTH: 386px" align="left">
					<asp:button id="btnAgregar" Runat="server" Font-Size="XX-Small" Font-Bold="True" Text="Agregar"></asp:button></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 545px" colSpan="3"><BR>
					<asp:datagrid id="dgrActuaciones" runat="server" Width="100%" AutoGenerateColumns="False">
						<FooterStyle BackColor="#CCCCCC"></FooterStyle>
						<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
						<ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
						<HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="NUM_ACTUACION" HeaderText="NUMERO" ItemStyle-Width="10"></asp:BoundColumn>
							<asp:BoundColumn DataField="ASISTENTES" HeaderText="ASISTENTES" DataFormatString="&lt;pre&gt;&lt;font size='1' style='FONT-FAMILY:Tahoma'&gt;{0}&lt;/font&gt;&lt;/pre&gt;">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCION" DataFormatString="&lt;pre&gt;&lt;font size='1' style='FONT-FAMILY:Tahoma'&gt;{0}&lt;/font&gt;&lt;/pre&gt;">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="NOMBRE" HeaderText="ESTADO"></asp:BoundColumn>
							<asp:BoundColumn DataField="FECHA_PROCESO" HeaderText="FECHA" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
						</Columns>
					</asp:datagrid></TD>
			</TR>
		</TABLE>
	</asp:panel>
	<TABLE style="WIDTH: 773px" align="center">
		<TR>
			<td>&nbsp;
				<asp:label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:label></TD>
		</TR>
	</TABLE>
</DIV>
