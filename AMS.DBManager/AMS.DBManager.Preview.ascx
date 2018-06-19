<%@ Control Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.DBManager.Preview.ascx.cs" Inherits="AMS.DBManager.Preview" %>
<p>
	<script type ="text/javascript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
	</script>
	<asp:PlaceHolder id="toolsHolder" runat="server">
		<TABLE class="tools" width="780">
			<TR>
				<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
				<TD>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
					</A>
				</TD>
				<TD>&nbsp; &nbsp;Enviar por correo
					<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
				<TD>
					<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
						ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" ErrorMessage=""></asp:RegularExpressionValidator>
					<asp:ImageButton id="ibMail" onclick="SendMail" runat="server" BorderWidth="0px" alt="Enviar por email"
						ImageUrl="../img/AMS.Icon.Mail.jpg"></asp:ImageButton></TD>
				<TD width="380"></TD>
			</TR>
		</TABLE>
	</asp:PlaceHolder>
</p>
<p>
	<asp:HyperLink id="hlTableView" runat="server">Ver Tabla</asp:HyperLink>
</p>
<p>
	<asp:PlaceHolder id="phForm" runat="server"></asp:PlaceHolder>
</p>
<p>
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p>
