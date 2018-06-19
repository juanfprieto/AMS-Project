<%@ Control Language="C#" AutoEventWireup="false" Inherits="AMS.SAC_Asesoria.MostrarOSC" %>
<link href="../css/SAC.css" rel="stylesheet" type="text/css" /> 
<script language="JavaScript">
    function Lista() {
        w=window.open('SAC.DBManager.Reporte.aspx');
    }
</script>
	<asp:PlaceHolder id="phOSC" runat="server">
    <fieldset>
		<TABLE id="Table" class="filtersIn">
			<TR bgColor="white">
                <td align="right">
                    <asp:Image id="Image1" runat="server" ImageUrl="../img/ecasIcono.png" visible ="false"></asp:Image>
                </td>
				<TD align="center">				
					<FONT face="Arial" color="red" size="5">
                        <STRONG>ORDEN DE SERVICIO N º</STRONG>
                    </FONT>
						<asp:Label id="lbnum" runat="server" font-names="Arial Black" font-size="x-Large" forecolor="#004080"></asp:Label>               
				</TD>
			</TR>
			<TR bgColor="white">				
                <td colSpan="2">
                    <div id="tarjetaInfo" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de la Solicitud del Cliente</div>
                </td>
			</TR>	
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Realizada bajo Solicitud Nº:</strong></FONT>
					<asp:Label id="lbnumsol" runat="server"></asp:Label></h4>
                </TD>		
                <TD>
                    <h4><FONT color="#004080"><strong>Fecha y Hora de la Solicitud:</strong></FONT>
					<asp:Label id="lbfechorsol" runat="server"></asp:Label></h4>
                </TD>		
			</TR>			
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Nit del Cliente:</strong></FONT>
					<asp:Label id="lbnitcli" runat="server"></asp:Label></h4>
                </TD>
				<TD>
                    <h4><FONT color="#004080"><strong>Razón Social del Cliente:</strong></FONT>
					<asp:Label id="lbnomcli" runat="server"></asp:Label></h4>
                </TD>
			</TR>			
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Cedula del Contacto:</strong></FONT>
					<asp:Label id="lbcedcon" runat="server"></asp:Label></h4>
                </TD>				
				<TD>
                    <h4><FONT color="#004080"><strong>Nombre del Contacto:</strong></FONT>
					<asp:Label id="lbnomcon" runat="server"></asp:Label></h4>
                </TD>
			</TR>	
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Solicitud Realizada Via:</strong></FONT>
					<asp:Label id="lbsolvia" runat="server"></asp:Label></h4>
                </TD>
                <td></td>				
			</TR>
			<TR bgColor="white">
                <td colSpan="2">
                    <div id="Div1" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de la Orden de Servicio al Cliente</div>
                </td>
			</TR>
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Persona que abrio la Orden de Servicio:</strong></FONT>
					<asp:Label id="lbaseaper" runat="server"></asp:Label></h4>
                </TD>				
				<TD>
                    <h4><FONT color="#004080"><strong>Fecha y Hora de Apertura:</strong></FONT>
					<asp:Label id="lbfechoraper" runat="server"></asp:Label></h4>
                </TD>
			</TR>
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Asesor Asignado:</strong></FONT>
					<asp:Label id="lbaseasig" runat="server"></asp:Label></h4>
                </TD>
				<TD>
                    <h4><FONT color="#004080"><strong>Fecha y Hora de Asignación:</strong></FONT>
					<asp:Label id="lbfechorasig" runat="server"></asp:Label></h4>
                </TD>
			</TR>
			<TR bgColor="white">
				<TD>
                    <h4><FONT color="#004080"><strong>Estado de la Orden de Servicio:</strong></FONT>
					<asp:Label id="lbestosc" runat="server" style="color:red;"></asp:Label></h4>
                </TD>
				<TD>
                    <h4><FONT color="#004080"><strong>Fecha y Hora de Cierre:</strong></FONT>
					<asp:Label id="lbfechorcie" runat="server"></asp:Label></h4>
                </TD>
			</TR>			
			<TR bgColor="white">
				<td colSpan="2">
                    <div id="Div2" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Información de Solicitudes y Respuestas</div>
                </td>
			</TR>
			<TR bgColor="white">
				<TD align="center" colSpan="2">
					<asp:DataGrid id="dgSolRes" runat="server" cssclass="datagrid" PageSize="15" GridLines="Vertical" AutoGenerateColumns="False">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
						<Columns>
							<asp:BoundColumn DataField="SOLICITUD" ReadOnly="True" HeaderText="Solicitud" ItemStyle-Width="300px"></asp:BoundColumn>
							<asp:BoundColumn DataField="RESPUESTATEC" ReadOnly="True" HeaderText="Respuesta Técnica" ItemStyle-Width="300px"></asp:BoundColumn>
                            <asp:BoundColumn DataField="RESPUESTACLI" ReadOnly="True" HeaderText="Respuesta Cliente" ItemStyle-Width="300px"></asp:BoundColumn>
						</Columns>
					</asp:DataGrid>
                </TD>
			</TR>
			<TR bgColor="white">
		        <td colSpan="2">
                    <div id="Div3" class="tarjetaInfo" runat="server">&nbsp;-->&nbsp;&nbsp;Archivos Adjuntos</div>
                </td>
			</TR>
			<TR bgColor="white">
				<TD align="center" colSpan="2">
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
					</asp:DataGrid>
                </TD>
			</TR>
		</TABLE>
    </fieldset>
   	</asp:PlaceHolder>
    <asp:Button id="btnRegresar" runat="server" Text="Cargar Solicitudes" OnClick="btnRegresarClick" style="margin: 20px; margin-left: 80px;"/>
<asp:Label id="lb" runat="server"></asp:Label>