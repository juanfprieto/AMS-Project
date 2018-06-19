<%@ Control Language="c#" codebehind="AMS.Nomina.FormatoComprobantePagoVacaciones.cs" autoeventwireup="false" Inherits="AMS.Nomina.FormatoComprobantePagoVacaciones" %>
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<fieldset>
	<asp:PlaceHolder id="toolsHolder" runat="server">
		<TABLE id="Table1" class="filtersIn">
			<TR>
				<th class="filterHead">
				<img height="30" src="../img/AMS.Flyers.Tools.png" border="0">
			</th>
				<TD>Imprimir <A href="javascript: Lista()">
                <IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail.jpg"
						alt="Enviar por email" BorderWidth="0px"></asp:ImageButton></TD>
				<TD></TD>
			</TR>
		</TABLE>
	</asp:PlaceHolder>
</fieldset>
<p>
	<asp:PlaceHolder id="phFormatoPagoVacaciones" runat="server">
   <br>
   <br>
		<TABLE id="table2" class="filtersIn">			
            <TR>
				<TD>LIQ. VACACIONES
				</TD>
				<TD></TD>
			</TR>
			<TR>
				<TD>Número de días:
				</TD>
				<TD>
					<asp:Label id="LBDTVACACIONES" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Vlr. Vacaciones a Pagar:</TD>
				<TD>
					<asp:Label id="LBVACAAPAGAR" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Fechas:</TD>
				<TD>
					<asp:Label id="LBPERIODO" runat="server" visible="False" width="187px"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Dias Efectivos</TD>
				<TD>
					<asp:Label id="LBDIASEFECTIVOS" runat="server" class="lpequeno"></asp:Label></TD>
			</TR>          
		</TABLE>
       
          
		<TABLE id="Table3" class="filtersIn">
			<TR>
				<TD colspan="5">
                Comprobante de Pago 
				</TD>
			</TR>
			<TR>
				<TD>Código:
				</TD>
				<TD>Nombre del Empleado:
				</TD>
				<TD>Cédula:
				</TD>
				<TD>Fecha:
				</TD>
				<TD>Sueldo:
				</TD>
			</TR>
			<TR>
				<TD>
					<asp:Label id="LBCODIGO" runat="server" class="lpequeno"></asp:Label></TD>
				<TD>
					<asp:Label id="LBNOMBRE" runat="server" class="lpequeno"></asp:Label></TD>
				<TD>
					<asp:Label id="LBCEDULA" runat="server" class="lpequeno"></asp:Label></TD>
				<TD>
					<asp:Label id="LBFECHA" runat="server" class="lpequeno"></asp:Label></TD>
				<TD>
					<asp:Label id="LBSUELDO" runat="server" class="lpequeno"></asp:Label></TD>
			</TR>
            </table>
            <table>
  			<TR>
				<TD colSpan="5">
					<asp:DataGrid id="DATAGRIDPAGOVACACIONES" cssclass="datagrid" runat="server" autogenerateColumns="false">
						<Columns>
							<asp:BoundColumn DataField="CONCEPTO" HeaderText="CONCEPTO" DataFormatString="{0:C}"></asp:BoundColumn>
							<asp:BoundColumn DataField="DESCRIPCION" HeaderText="DESCRIPCION"></asp:BoundColumn>
							<asp:BoundColumn DataField="CANT EVENTOS" HeaderText="CANT EVENTOS"></asp:BoundColumn>
							<asp:BoundColumn DataField="VALOR EVENTO" HeaderText="VALOR EVENTO" DataFormatString="{0:C}"></asp:BoundColumn>
							<asp:BoundColumn DataField="A PAGAR" HeaderText="A PAGAR" DataFormatString="{0:C}"></asp:BoundColumn>
							<asp:BoundColumn DataField="A DESCONTAR" HeaderText="A DESCONTAR" DataFormatString="{0:C}"></asp:BoundColumn>
							<asp:BoundColumn DataField="TIPO EVENTO" HeaderText="TIPO EVENTO"></asp:BoundColumn>
							<asp:BoundColumn DataField="SALDO" HeaderText="SALDO" DataFormatString="{0:C}"></asp:BoundColumn>
							<asp:BoundColumn DataField="DOC REFERENCIA" HeaderText="DOC REFERENCIA"></asp:BoundColumn>
						</Columns>
					</asp:DataGrid>
                </TD>
			</TR>
            </table>
            <table id="Table5" class="filtersIn">
			<TR>
				<TD></TD>
				<TD>Subtotal
				</TD>
				<TD>&nbsp;
					<asp:Label id="LBSUBTP" runat="server"></asp:Label></TD>
				<TD>&nbsp;
					<asp:Label id="LBSUBTD" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD></TD>
				<TD>Neto:</TD>
				<TD>
					<asp:Label id="LBNETO" runat="server"></asp:Label></TD>
			</TR>
			<TR>
				<TD>Recibido</TD>
				<TD>Firma Autorizada</TD>
			</TR>
			<TR>
				<TD>--------------------------</TD>
				<TD>-----------------------</TD>
			</TR>
			<TR>
				<TD>c.c. No.</TD>
				<TD></TD>
			</TR>
		</TABLE>
        
	</asp:PlaceHolder>
</p>
<p>
</p>
<p>
</p>
