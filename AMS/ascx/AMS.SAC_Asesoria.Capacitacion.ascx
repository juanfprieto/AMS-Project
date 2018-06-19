<%@ Control Language="C#" AutoEventWireup="false" Inherits="AMS.SAC_Asesoria.Capacitacion" %>

<script runat="server">

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {       
        
        
        
        
    }
</script>

<script language="JavaScript">
    function Lista() {
        w=window.open('SAC.DBManager.Reporte.aspx');
    }
</script>
<p>
	Codigo:
    <asp:TextBox ID="TBCodigo" runat="server" class="tpequeno"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Software:
    <asp:TextBox ID="TBSoftware" runat="server" class="tmediano"></asp:TextBox>
</p>
<p style="height: 2px">
	Empresa:
    &nbsp;<asp:DropDownList ID="ddlEmpresa" runat="server" Height="16px" 
        class="dmediano">
    </asp:DropDownList>
&nbsp;Modulo:
    <asp:DropDownList ID="ddlModulo" runat="server" Height="16px" class="dmediano">
    </asp:DropDownList>
</p>
<p style="height: 2px">
	Capacitador:&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox1" runat="server" Width="343px"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </p>
<p>
	<asp:Button ID="btnAgregar" runat="server" Text="Agregar Participante" 
        Width="127px" />
</p>
<p>
	Temas Tratados&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox2" runat="server" Height="68px" class="tgrande"></asp:TextBox>
&nbsp;
</p>
<p>
	Tareas Asignadas&nbsp;&nbsp;
    <asp:TextBox ID="TextBox3" runat="server" Height="91px" class="tgrande"></asp:TextBox>
</p>
<p>
	Observaciones&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox4" runat="server" Height="91px" class="tgrande"></asp:TextBox>
</p>
<p>
	<asp:PlaceHolder id="toolsHolder" runat="server" visible="true">
		<TABLE class="tools">
			<TR>
				<TD><IMG height="30" src="../img/SAC.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/SAC.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>  &#160;Enviar por correo
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
					<P align="right"><FONT face="Arial" color="red" size="5"><STRONG>CAPACITACION N º</STRONG></FONT>
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
							Capacitacion al Cliente</STRONG></FONT></TD>
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
				<TD><FONT color="#004080"></FONT>
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
				<TD colSpan="6"><FONT face="Arial" color="white" size="4"><STRONG>Información de la Capacitacion Dada al Cliente</STRONG></FONT></TD>
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
				<TD><asp:Label id="lbfechorasig" runat="server"></asp:Label></TD>
				
			</TR>
			<TD><FONT color=blue>Participantes:&nbsp;</FONT>			
			</TD>
			<TD>			
			</TD>
			<asp:Label ID='lblParticipantes' runat="server"></asp:Label>
			
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
					<asp:DataGrid id="dgSolRes" runat="server" cssclass="datagrid" PageSize="15"
					    GridLines="Vertical" AutoGenerateColumns="False">
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
					<asp:DataGrid id="dgArc" runat="server" cssclass="datagrid" PageSize="15"
						BorderStyle="None" Font-Names="Verdana" AutoGenerateColumns="False">
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
<p>
	<asp:Label id="lb" runat="server"></asp:Label>
</p>
