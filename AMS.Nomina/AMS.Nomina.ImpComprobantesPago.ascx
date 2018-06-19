<%@ Control Language="c#" codebehind="AMS.Nomina.ImpComprobantesPago.cs" autoeventwireup="false" Inherits="AMS.Nomina.ImpComprobantesPago" %>
<fieldset>
<table id="Table" class="filtersIn">
<tr>
<td>

<p>
	Por favor escoja la Quincena(codigo,año,mes,periodo nomina)
</p>
<p>
	<asp:DropDownList id="DDLQUINCENA" class="dpequeno" runat="server"></asp:DropDownList>
</p>
<p>
	Empleado
</p>
<p>
	<asp:DropDownList id="DDLEMPLEADO" class="dmediano" runat="server"></asp:DropDownList>
</p>
<p>
	<asp:Button id="BTNGENERAR" onclick="generar" runat="server" Text="Generar"></asp:Button>
</p>
<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
</script>
<p>
	<asp:PlaceHolder id="toolsHolder" runat="server">
		<TABLE class="tools">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server" width="250px"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" ImageUrl="../img/AMS.Icon.Mail1.png"
						alt="Enviar por email" BorderWidth="0px"></asp:ImageButton></TD>
                <td colspan="1"> Enviar a Todos: 
                    <asp:ImageButton id="sendAll" onclick="enviarATodos" runat="server" ImageUrl="../img/send-all-mail-icon.png"
						alt="Enviar por email" BorderWidth="0px" onClientclick="espera();"></asp:ImageButton></td>
                </td>
				<TD width="380"></TD>
			</TR>
		</TABLE> <br />
	</asp:PlaceHolder>
</p>
    <asp:Label ID="lbError" runat="server"></asp:Label>
<p>
	<asp:PlaceHolder id="phcomprobantepago" runat="server" Visible="False"></asp:PlaceHolder>
</p>
</td>
</tr>
</table>
</fieldset>
