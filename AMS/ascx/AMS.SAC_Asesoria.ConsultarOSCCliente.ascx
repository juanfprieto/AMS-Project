<%@ Control Language="C#" AutoEventWireup="false" Inherits="AMS.SAC_Asesoria.ConsultarOSCCliente" %>
<script language="JavaScript">
    function Lista() {
        w=window.open('SAC.DBManager.Reporte.aspx');
    }
</script>
<fieldset>
<p>
	<table id="Table" class="filtersIn">
		<tbody>
			<tr>
				<td class="filtersIntd">
					Escoja el número de la&nbsp;Orden de Servicio&nbsp;
				</td>
				<td>
					<asp:DropDownList id="ddlnumero" runat="server"></asp:DropDownList>
				</td>
				<td>
					<asp:Button id="btnconsultar" onclick="btnconsultar_Click" runat="server" CausesValidation="False"
						Text="Consultar"></asp:Button>
				</td>
			</tr>
		</tbody>
	</table>
</p>
<p>
	<asp:PlaceHolder id="toolsHolder" runat="server" visible="true">
		<TABLE class="tools">
			<TR>
				<TD><IMG height="30" src="../img/SAC.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/SAC.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator id="rfv1" runat="server" ControlToValidate="tbEmail">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="FromValidator2" runat="server" ControlToValidate="tbEmail" ErrorMessage="" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator></TD>
				<TD>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" ImageUrl="../img/SAC.Icon.Mail.jpg"
						alt="Enviar por email"></asp:ImageButton></TD>
				<TD></TD>
			</TR>
		</TABLE>
	</asp:PlaceHolder>
</p>
<p>
	<asp:PlaceHolder id="phOSC" runat="server">
		<TABLE class="main">
			<TR>
				<TD align="center" bgColor="white" colSpan="6">
					<P>
						<asp:Image id="Image2" runat="server" ImageUrl="../img/SAC.logo.ecas.jpg" ImageAlign="Left"></asp:Image></P>
					<P align="right"><FONT face="Arial" color="red" size="5"><STRONG>ORDEN DE SERVICIO N º</STRONG></FONT>
						<asp:Label id="lbnum" runat="server" forecolor="#004080" font-size="Large" font-names="Arial Black"></asp:Label></P>
				</TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="gray">
				<TD colSpan="6"><FONT face="Arial" color="white" size="4"><STRONG>Información de la 
							Solicitud del Cliente</STRONG></FONT></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD><FONT color="#004080">Realizada bajo Solicitud Nº :</FONT>
				</TD>
				<TD>
					<asp:Label id="lbnumsol" runat="server"></asp:Label></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD><FONT color="#004080">Nit del Cliente :</FONT>
				</TD>
				<TD>
					<asp:Label id="lbnitcli" runat="server"></asp:Label></TD>
				<TD></TD>
				<TD></TD>
				<TD><FONT color="#004080">Razón Social del Cliente : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbnomcli" runat="server"></asp:Label></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD><FONT color="#004080">Cedula del Contacto : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbcedcon" runat="server"></asp:Label></TD>
				<TD></TD>
				<TD></TD>
				<TD><FONT color="#004080">Nombre del Contacto : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbnomcon" runat="server"></asp:Label></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD><FONT color="#004080">Fecha y Hora de la Solicitud : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbfechorsol" runat="server"></asp:Label></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD><FONT color="#004080">Solicitud Realizada Via : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbsolvia" runat="server"></asp:Label></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="gray">
				<TD colSpan="6"><FONT face="Arial" color="white" size="4"><STRONG>Información de la Orden 
							de Servicio al Cliente</STRONG></FONT></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD><FONT color="#004080">Persona que abrio la Orden de Servicio : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbaseaper" runat="server"></asp:Label></TD>
				<TD></TD>
				<TD></TD>
				<TD><FONT color="#004080">Fecha y Hora de Apertura : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbfechoraper" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD><FONT color="#004080">Asesor Asignado : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbaseasig" runat="server"></asp:Label></TD>
				<TD></TD>
				<TD></TD>
				<TD><FONT color="#004080">Fecha&nbsp;y Hora de Asignación : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbfechorasig" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD><FONT color="#004080">Estado de la Orden de Servicio : </FONT>
				</TD>
				<TD>
					<asp:Label id="lbestosc" runat="server"></asp:Label></TD>
				<TD></TD>
				<TD></TD>
				<TD><FONT color="#004080">Fecha y Hora de Cierre :</FONT></TD>
				<TD>
					<asp:Label id="lbfechorcie" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD bgColor="gray" colSpan="6"><FONT face="Arial" color="white" size="4"><STRONG>Información 
							de Solicitudes y Respuestas</STRONG></FONT></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="6">
					<asp:DataGrid id="dgSolRes" runat="server" CssClass="datagrid" PageSize="15" GridLines="Vertical" AutoGenerateColumns="False">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="SOLICITUD" ReadOnly="True" HeaderText="Solicitud"></asp:BoundColumn>
							<asp:BoundColumn DataField="RESPUESTA" ReadOnly="True" HeaderText="Respuesta"></asp:BoundColumn>
						</Columns>
					</asp:DataGrid></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD bgColor="gray" colSpan="6"><FONT face="Arial" color="white" size="4"><STRONG>Archivos 
							Adjuntos</STRONG></FONT></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR bgColor="white">
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
				<TD></TD>
			</TR>
			<TR>
				<TD align="center" colSpan="6">
					<asp:DataGrid id="dgArc" runat="server" cssclass="datagrid" PageSize="15" AutoGenerateColumns="False">
						<HeaderStyle CssClass="header"></HeaderStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="ARCHIVO" ReadOnly="True" HeaderText="Nombre del Archivo"></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Descargar Archivo">
								<ItemTemplate>
									<center>
										<asp:HyperLink id="hpldes" runat="server">Descargar Archivo</asp:HyperLink>
									</center>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid></TD>
			</TR>
		</TABLE>
	</asp:PlaceHolder>
</p>
</fieldset>
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
